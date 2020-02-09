using System;
using UnityEngine.Events;

namespace UnityMultithreading
{
    public abstract class ThreadWorker
    {
        public enum WorkerState
        {
            Running,
            Closed,
            Error
        }

        public WorkerState State { get; protected set; }
        
        protected internal UnityEvent OnStartWorker { get; } = new UnityEvent();
        protected internal UnityEvent OnUpdateWorker { get; } = new UnityEvent();
        protected internal UnityEvent OnCloseWorker { get; } = new UnityEvent();
        protected internal UnityEvent OnErrorWorker { get; } = new UnityEvent();

        internal void WorkHandleStart()
        {
            try
            {
                State = WorkerState.Running;
                WorkBody();
                State = WorkerState.Closed;
            }
            catch (Exception)
            {
                State = WorkerState.Error;
                throw;
            }
        }

        protected abstract void WorkBody();
    }
}