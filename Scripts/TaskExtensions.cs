using UnityParseHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public static class TaskExtensions
{
    // Inspired by http://blogs.msdn.com/b/pfxteam/archive/2010/11/21/10094564.aspx

    public static Task Then(this Task task, Action<Task> next)
    {
        if (task == null) throw new ArgumentNullException("task");
        if (next == null) throw new ArgumentNullException("next");

        var tcs = new TaskCompletionSource<AsyncVoid>();

        task.ContinueWith(previousTask =>
        {
            if (previousTask.IsFaulted) tcs.TrySetException(previousTask.Exception);
            else if (previousTask.IsCanceled) tcs.TrySetCanceled();
            else
            {
                try
                {
                    next(previousTask);
                    tcs.TrySetResult(default(AsyncVoid));
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                    tcs.TrySetException(ex);
                }
            }
        });

        return tcs.Task;
    }

    public static Task Then(this Task task, Func<Task, Task> next)
    {
        if (task == null) throw new ArgumentNullException("task");
        if (next == null) throw new ArgumentNullException("next");

        var tcs = new TaskCompletionSource<AsyncVoid>();

        task.ContinueWith(previousTask =>
        {
            if (previousTask.IsFaulted) tcs.TrySetException(previousTask.Exception);
            else if (previousTask.IsCanceled) tcs.TrySetCanceled();
            else
            {
                try
                {
                    next(previousTask).ContinueWith(nextTask =>
                    {
                        if (nextTask.IsFaulted) tcs.TrySetException(nextTask.Exception);
                        else if (nextTask.IsCanceled) tcs.TrySetCanceled();
                        else tcs.TrySetResult(default(AsyncVoid));
                    });
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                    tcs.TrySetException(ex);
                }
            }
        });

        return tcs.Task;
    }

    public static Task<TNextResult> Then<TNextResult>(this Task task, Func<Task, TNextResult> next)
    {
        if (task == null) throw new ArgumentNullException("task");
        if (next == null) throw new ArgumentNullException("next");

        var tcs = new TaskCompletionSource<TNextResult>();

        task.ContinueWith(previousTask =>
        {
            if (previousTask.IsFaulted) tcs.TrySetException(previousTask.Exception);
            else if (previousTask.IsCanceled) tcs.TrySetCanceled();
            else
            {
                try
                {
                    tcs.TrySetResult(next(previousTask));
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                    tcs.TrySetException(ex);
                }
            }
        });

        return tcs.Task;
    }

    public static Task<TNextResult> Then<TNextResult>(this Task task, Func<Task, Task<TNextResult>> next)
    {
        if (task == null) throw new ArgumentNullException("task");
        if (next == null) throw new ArgumentNullException("next");

        var tcs = new TaskCompletionSource<TNextResult>();

        Action<Task> a = previousTask =>
        {
            if (previousTask.IsFaulted) tcs.TrySetException(previousTask.Exception);
            else if (previousTask.IsCanceled) tcs.TrySetCanceled();
            else
            {
                try
                {
                    Action<Task<TNextResult>> b = nextTask =>
                    {
                        if (nextTask.IsFaulted) tcs.TrySetException(nextTask.Exception);
                        else if (nextTask.IsCanceled) tcs.TrySetCanceled();
                        else
                        {
                            try
                            {
                                tcs.TrySetResult(nextTask.Result);
                            }
                            catch (Exception ex)
                            {
                                Debug.LogException(ex);
                                tcs.TrySetException(ex);
                            }
                        }
                    };
                    next(previousTask).ContinueWith(b);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                    tcs.TrySetException(ex);
                }
            }
        };

        task.ContinueWith(a);

        return tcs.Task;
    }

    public static Task Then<TResult>(this Task<TResult> task, Action<Task<TResult>> next)
    {
        if (task == null) throw new ArgumentNullException("task");
        if (next == null) throw new ArgumentNullException("next");

        var tcs = new TaskCompletionSource<AsyncVoid>();

        Action<Task<TResult>> a = previousTask =>
        {
            if (previousTask.IsFaulted) tcs.TrySetException(previousTask.Exception);
            else if (previousTask.IsCanceled) tcs.TrySetCanceled();
            else
            {
                try
                {
                    next(previousTask);
                    tcs.TrySetResult(default(AsyncVoid));
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                    tcs.TrySetException(ex);
                }
            }
        };

        task.ContinueWith(a);
        return tcs.Task;
    }

    public static Task Then<TResult>(this Task<TResult> task, Func<Task<TResult>, Task> next)
    {
        if (task == null) throw new ArgumentNullException("task");
        if (next == null) throw new ArgumentNullException("next");

        var tcs = new TaskCompletionSource<AsyncVoid>();

        Action<Task<TResult>> a = previousTask =>
        {
            if (previousTask.IsFaulted) tcs.TrySetException(previousTask.Exception);
            else if (previousTask.IsCanceled) tcs.TrySetCanceled();
            else
            {
                try
                {
                    next(previousTask).ContinueWith(nextTask =>
                    {
                        if (nextTask.IsFaulted) tcs.TrySetException(nextTask.Exception);
                        else if (nextTask.IsCanceled) tcs.TrySetCanceled();
                        else tcs.TrySetResult(default(AsyncVoid));
                    });
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                    tcs.TrySetException(ex);
                }
            }
        };

        task.ContinueWith(a);
        return tcs.Task;
    }

    public static Task<TNextResult> Then<TResult, TNextResult>(this Task<TResult> task, Func<Task<TResult>, TNextResult> next)
    {
        if (task == null) throw new ArgumentNullException("task");
        if (next == null) throw new ArgumentNullException("next");

        var tcs = new TaskCompletionSource<TNextResult>();

        Action<Task<TResult>> a = previousTask =>
        {
            if (previousTask.IsFaulted) tcs.TrySetException(previousTask.Exception);
            else if (previousTask.IsCanceled) tcs.TrySetCanceled();
            else
            {
                try
                {
                    tcs.TrySetResult(next(previousTask));
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                    tcs.TrySetException(ex);
                }
            }
        };

        task.ContinueWith(a);
        return tcs.Task;
    }

    public static Task<TNextResult> Then<TResult, TNextResult>(this Task<TResult> task, Func<Task<TResult>, Task<TNextResult>> next)
    {
        if (task == null) throw new ArgumentNullException("task");
        if (next == null) throw new ArgumentNullException("next");

        var tcs = new TaskCompletionSource<TNextResult>();

        Action<Task<TResult>> a = previousTask =>
        {
            if (previousTask.IsFaulted) tcs.TrySetException(previousTask.Exception);
            else if (previousTask.IsCanceled) tcs.TrySetCanceled();
            else
            {
                try
                {
                    Action<Task<TNextResult>> b = nextTask =>
                    {
                        if (nextTask.IsFaulted) tcs.TrySetException(nextTask.Exception);
                        else if (nextTask.IsCanceled) tcs.TrySetCanceled();
                        else
                        {
                            try
                            {
                                tcs.TrySetResult(nextTask.Result);
                            }
                            catch (Exception ex)
                            {
                                Debug.LogException(ex);
                                tcs.TrySetException(ex);
                            }
                        }
                    };
                    next(previousTask).ContinueWith(b);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                    tcs.TrySetException(ex);
                }
            }
        };

        task.ContinueWith(a);
        return tcs.Task;
    }

    /// <summary>
    /// Analogous to the finally block in a try/finally
    /// </summary>
    public static void Finally(this Task task, Action<Exception> exceptionHandler, Action finalAction = null)
    {
        task.ContinueWith(t =>
        {
            if (finalAction != null) finalAction();

            if (t.IsCanceled || !t.IsFaulted || exceptionHandler == null) return;
            var innerException = t.Exception.Flatten().InnerExceptions.FirstOrDefault();
            exceptionHandler(innerException ?? t.Exception);
        });
    }

    public static void Then<TResult>(this Task<TResult> task, Action<TResult> successHandler, Action<Exception> exceptionHandler)
    {
        Action<Task<TResult>> a = t =>
        {
            if (t.IsCanceled || !t.IsFaulted || exceptionHandler == null)
            {
                try { successHandler(t.Result); }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    exceptionHandler(e);
                }
            }
            else
            {
                var innerException = t.Exception.Flatten().InnerExceptions.FirstOrDefault();
                exceptionHandler(innerException ?? t.Exception);
            }
        };
        task.ContinueWith(a);
    }

    public static void Then(this Task task, Action successHandler, Action<Exception> exceptionHandler)
    {
        Action<Task> a = t =>
        {
            if (t.IsCanceled || !t.IsFaulted || exceptionHandler == null)
            {
                try { successHandler(); }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    exceptionHandler(e);
                }
            }
            else
            {
                var innerException = t.Exception.Flatten().InnerExceptions.FirstOrDefault();
                exceptionHandler(innerException ?? t.Exception);
            }
        };
        task.ContinueWith(a);
    }


    public static Task<T> OnMainThread<T>(this Task<T> task)
    { 
        var tcs = new TaskCompletionSource<T>();
        var loom = Loom.Instance;

        Action<Task<T>> a = t =>
        {
            loom.QueueOnMainThread(() =>
            {
                if (t.IsFaulted) tcs.SetException(t.Exception);
                else if (t.IsCanceled) tcs.SetCanceled();
                else { tcs.SetResult(t.Result); }
            });    
        };

        task.ContinueWith(a);
        return tcs.Task;
    }

    public static Task OnMainThread(this Task task)
    {
        var tcs = new TaskCompletionSource<AsyncVoid>();
        var loom = Loom.Instance;

        Action<Task> a = t =>
        {
            loom.QueueOnMainThread(() =>
            {
                if (t.IsFaulted) tcs.SetException(t.Exception);
                else if (t.IsCanceled) tcs.SetCanceled();
                else { tcs.SetResult(default(AsyncVoid)); }
            });
        };

        task.ContinueWith(a);

        return tcs.Task;
    }

    public static Task<T> DebugLog<T>(this Task<T> task)
    {
        var tcs = new TaskCompletionSource<T>();
        var loom = Loom.Instance;

        Action<Task<T>> a = t =>
        {
            loom.QueueOnMainThread(() =>
            {
                if (t.IsFaulted)  tcs.SetException(t.Exception);
                else if (t.IsCanceled) tcs.SetCanceled();
                else { tcs.SetResult(t.Result); }
            });
        };

        task.ContinueWith(a);
        return tcs.Task;
    }

    public static Task DebugLog(this Task task)
    {
        var tcs = new TaskCompletionSource<AsyncVoid>();
        var loom = Loom.Instance;

        Action<Task> a = t =>
        {
            if (t.IsFaulted) Debug.LogException(t.Exception);   
            loom.QueueOnMainThread(() =>
            {
                if (t.IsFaulted)  tcs.SetException(t.Exception);
                else if (t.IsCanceled) tcs.SetCanceled();
                else { tcs.SetResult(default(AsyncVoid)); }
            });
        };

        task.ContinueWith(a);
        return tcs.Task;
    }

    //public static Task ThrowErrors(this Task task)
    //{
    //    var tcs = new TaskCompletionSource<AsyncVoid>();
    //    var loom = Loom.Instance;

    //    Action<Task> a = t =>
    //    {
    //        if (t.IsFaulted) throw t.Exception;   
    //        loom.QueueOnMainThread(() =>
    //        {
    //            if (t.IsFaulted)  tcs.SetException(t.Exception);
    //            else if (t.IsCanceled) tcs.SetCanceled();
    //            else { tcs.SetResult(default(AsyncVoid)); }
    //        });
    //    };

    //    task.ContinueWith(a);
    //    return tcs.Task;
    //}

    
}


public struct AsyncVoid
{
    // Based on Brad Wilson's idea, to simulate a non-generic TaskCompletionSource
    // http://bradwilson.typepad.com/blog/2012/04/tpl-and-servers-pt4.html
}

