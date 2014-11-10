using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityParseHelpers
{
    public interface ILoom
    {
        Task Delay(TimeSpan time);
        void Clear();
        Thread RunAsync(Action a);
        void QueueOnMainThread(Action action);
        void QueueOnMainThread(Action action, float time);
    }

    public class Loom : MonoBehaviour, ILoom
    {
        public int maxThreads = 8;

        private int numThreads;
        private int _count;

        private bool m_HasLoaded = false;

        private List<Action> _actions = new List<Action>();
        private List<DelayedQueueItem> _delayed = new List<DelayedQueueItem>();

        private List<DelayedQueueItem> _currentDelayed = new List<DelayedQueueItem>();
        private List<Action> _currentActions = new List<Action>();

        private static Loom _instance;
        public static Loom Instance
        {
            get 
            {
                if(_instance==null)
                    _instance = new GameObject("Loom").AddComponent<Loom>();
                return _instance;
            }
        }

        private int loomId = 0;

        public static void Init()
        {
            if (_instance != null) return;
            _instance = new GameObject("Loom").AddComponent<Loom>();
        }

        public struct DelayedQueueItem
        {
            public float time;
            public Action action;
        }

        protected virtual void Start()
        {
            m_HasLoaded = true;
            DontDestroyOnLoad(gameObject);
        }

        public Task Delay(TimeSpan time)
        {
            var lid = loomId;
            var tcs = new TaskCompletionSource<bool>();
            Task.Delay(time).Then(t =>
            {
                // If the id of the loom has changed since the delay then dont go any further
                if (loomId != lid)
                {
                    Debug.Log("Loom ID not equal, probably a scene change so going not executing delayed task");
                    return;
                }
                QueueOnMainThread(() => tcs.SetResult(true));
            });
            return tcs.Task;
        }

        public void Clear()
        {
            // Increment the loom ID so that delayed tasks dont get run
            loomId++;

            lock (_delayed)
            {
                loomId++;
                Debug.Log(_delayed.Count + " delayed actions cleared from loomer.");
                _delayed.Clear();
            }
            lock (_actions)
            {
                Debug.Log(_actions.Count + " actions cleared from loomer.");
                _actions.Clear();
            }
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
            while (numThreads >= maxThreads)
            {
                Thread.Sleep(1);
            }
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

        
    }
}
