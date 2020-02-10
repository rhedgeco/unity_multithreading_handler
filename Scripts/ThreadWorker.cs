using System;
using UnityEngine.Events;

namespace UnityMultithreading
{
    /// <summary>
    /// Abstract class for an object to be handled by a ThreadDispatcher
    /// </summary>
    public abstract class ThreadWorker
    {
        /// <summary>
        /// Enum representing the state of a ThreadWorker
        /// </summary>
        public enum WorkerState
        {
            Running,
            Closed,
            Error
        }

        /// <value>The current state of the ThreadWorker</value>
        public WorkerState State { get; protected set; }
        
        /// <value>Callback event for when the ThreadWorker is started by ThreadDispatcher</value>
        /// <remarks>Will be run on the main thread</remarks>
        protected internal UnityEvent OnStartWorker { get; } = new UnityEvent();
        /// <value>Callback event for when the ThreadWorker is updated by ThreadDispatcher</value>
        /// <remarks>Will be run on the main thread</remarks>
        protected internal UnityEvent OnUpdateWorker { get; } = new UnityEvent();
        /// <value>Callback event for when the ThreadWorker found closed by ThreadDispatcher</value>
        /// <remarks>Will be run on the main thread</remarks>
        protected internal UnityEvent OnCloseWorker { get; } = new UnityEvent();
        /// <value>Callback event for when the ThreadWorker found in an error state by ThreadDispatcher</value>
        /// <remarks>Will be run on the main thread</remarks>
        protected internal UnityEvent OnErrorWorker { get; } = new UnityEvent();

        /// <summary>
        /// Wraps around the WorkBody method to provide State changing and error handling
        /// </summary>
        /// <remarks>Will be run on a separate thread and must not do main thread work.</remarks>
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

        /// <summary>
        /// The working body of the ThreadWorker
        /// </summary>
        /// <remarks>Will be run on a separate thread and must not do main thread work.</remarks>
        protected abstract void WorkBody();
    }
}