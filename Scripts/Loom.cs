using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace UnityParseHelpers
{
    public interface ILoom
    {
        Task Delay(float seconds);
        Task Delay(TimeSpan time);
        void Clear();
        Thread RunAsync(Action a);
        void QueueOnMainThread(Action action);
        void QueueOnMainThread(Action action, float time);
        int QueuedItemCount { get; }
    }

    public class Loom : MonoBehaviour, ILoom
    {
        public int maxThreads = 8;

        private int numThreads;
        private int _count;

        private bool m_HasLoaded = false;

        private List<Action> _actions = new List<Action>();
        private List<DelayedQueueItem> _delayed = new List<DelayedQueueItem>();
        private List<ConditionalQueueItem> _conditional = new List<ConditionalQueueItem>();

        private List<DelayedQueueItem> _currentDelayed = new List<DelayedQueueItem>();
        private List<Action> _currentActions = new List<Action>();
        private List<ConditionalQueueItem> _currentConditional = new List<ConditionalQueueItem>();

        private static Loom _instance;
        public static Loom Instance
        {
            get 
            {
                if (_instance == null) _instance = GameObject.FindObjectOfType<Loom>();
                if(_instance==null) _instance = new GameObject("Loom").AddComponent<Loom>();
                return _instance;
            }
        }

        public int QueuedItemCount
        {
            get
            {
                var total = 0;
                lock (_actions) { total += _actions.Count; }
                lock (_delayed) { total += _delayed.Count; }
                lock (_conditional) { total += _conditional.Count; }
                return total;
            }
        }

        public struct DelayedQueueItem
        {
            public float time;
            public Action action;
        }

        public struct ConditionalQueueItem
        {
            public TaskCompletionSource<bool> tcs;
            public Func<bool> condition;
        }

        protected virtual void Start()
        {
            m_HasLoaded = true;
            DontDestroyOnLoad(gameObject);
        }

        public Task Delay(float seconds)
        {
            return Delay(TimeSpan.FromSeconds(seconds));
        }

        public Task Delay(TimeSpan time)
        {
            var tcs = new TaskCompletionSource<bool>();
            QueueOnMainThread(() => tcs.SetResult(true), (float)time.TotalSeconds);            
            return tcs.Task;
        }

        public Task Until(Func<bool> condition)
        {
            var tcs = new TaskCompletionSource<bool>();
            lock (_conditional)
            {
                _conditional.Add(new ConditionalQueueItem {
                    condition = condition,
                    tcs = tcs
                });
            }
            return tcs.Task;
        }

        public void Clear()
        {
            var total = QueuedItemCount;
            lock (_delayed) { _delayed.Clear(); }
            lock (_actions) { _actions.Clear(); }
            lock (_conditional) { _conditional.Clear(); }
            Debug.Log(total + " actions cleared from loomer.");
        }

        public void QueueOnMainThread(Action action)
        {
            QueueOnMainThread(action, 0f);
        }

        public void QueueOnMainThread(Action action, float time)
        {
            if (time != 0)
            {
                lock (_delayed)
                {
                    _delayed.Add(new DelayedQueueItem { time = Time.time + time, action = action });
                }
            }
            else
            {
                lock (_actions)
                {
                    _actions.Add(action);
                }
            }
        }

        public Thread RunAsync(Action a)
        {
            while (numThreads >= maxThreads) Thread.Sleep(1);
            Interlocked.Increment(ref numThreads);
            ThreadPool.QueueUserWorkItem(RunAction, a);
            return null;
        }

        private void RunAction(object action)
        {
            try
            {
                ((Action)action)();
            }
            catch
            {
            }
            finally
            {
                Interlocked.Decrement(ref numThreads);
            }
        }

        protected virtual void Update()
        {
            if (m_HasLoaded == false)
                Start();

            HandleActions();
            HandleDelayed();
            HandleConditional();
        }

        private void HandleDelayed()
        {
            lock (_delayed)
            {
                _currentDelayed.Clear();
                _currentDelayed.AddRange(_delayed.Where(d => d.time <= Time.time));
                foreach (var item in _currentDelayed)
                    _delayed.Remove(item);
            }
            foreach (var delayed in _currentDelayed)
            {
                delayed.action();
            }
        }

        private void HandleActions()
        {
            lock (_actions)
            {
                _currentActions.Clear();
                _currentActions.AddRange(_actions);
                _actions.Clear();
            }
            foreach (var a in _currentActions)
            {
                a();
            }
        }

        private void HandleConditional()
        {
            // Lock the conditional and perform minimum amount of work
            // so that we dont deadlock anything
            lock (_conditional)
            {
                _currentConditional.Clear();
                _currentConditional.AddRange(_conditional);
            }

            // Work out which conditionals have met their condition
            var toRemove = new List<ConditionalQueueItem>();
            foreach (var conditional in _currentConditional)
            {
                // If the condition is met then return the task
                if (conditional.condition())
                {
                    conditional.tcs.SetResult(true);
                    toRemove.Add(conditional);
                }                    
            }                

            // Once the condtition has been met it can be removed so that next 
            // update it isnt checked again
            lock (_conditional)
            {
                foreach (var c in toRemove)
                    _conditional.Remove(c);
            }
        }

        
    }
}
