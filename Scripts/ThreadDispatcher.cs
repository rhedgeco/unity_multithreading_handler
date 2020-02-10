using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace UnityMultithreading
{
    /// <summary>
    /// ThreadDispatcher is in charge of running all ThreadWorkers
    /// </summary>
    /// <remarks>
    /// It should be attached to an empty GameObject in the Unity scene for it to function.
    /// It will force itself into a singleton.
    /// </remarks>
    [DisallowMultipleComponent]
    public class ThreadDispatcher : MonoBehaviour
    {
        /// <value>Static instance of the singleton in the Unity Scene</value>
        private static ThreadDispatcher _dispatcher;

        /// <value>Dictionary containing all ThreadWorkers and associated Threads currently being handled</value>
        private static Dictionary<ThreadWorker, Thread> Workers { get; } = new Dictionary<ThreadWorker, Thread>();

        /// <value>Gets the number of ThreadWorkers being handled by ThreadDispatcher</value>
        public static int ActiveThreadWorkers => Workers.Count;

        /// <summary>
        /// Runs on creation of the attached GameObject
        /// </summary>
        private void Awake()
        {
            CreateSingleton();
        }

        /// <summary>
        /// Runs every frame of Unity Scene when attached to a GameObject
        /// </summary>
        private void Update()
        {
            UpdateWorkers();
        }

        /// <summary>
        /// Forces throwing a NullReferenceException if the ThreadDispatcher has not been attached to a GameObject
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        private static void EnsureInstantiated()
        {
            if (!_dispatcher)
                throw new NullReferenceException(
                    "You must have an existing ThreadDispatcher in the scene to use it"
                );
        }

        /// <summary>
        /// Forces ThreadDispatcher into a Singleton state when attached to a GameObject
        /// </summary>
        private void CreateSingleton()
        {
            if (!_dispatcher) _dispatcher = this;
            if (_dispatcher != this) Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Handles all ThreadWorkers based on their state
        /// </summary>
        private void UpdateWorkers()
        {
            ThreadWorker[] keys = Workers.Keys.ToArray();
            for (int i = keys.Length - 1; i >= 0; i--)
            {
                ThreadWorker worker = keys[i];
                switch (worker.State)
                {
                    case ThreadWorker.WorkerState.Running:
                        worker.OnUpdateWorker.Invoke();
                        break;
                    case ThreadWorker.WorkerState.Closed:
                        worker.OnCloseWorker.Invoke();
                        Workers.Remove(worker);
                        break;
                    case ThreadWorker.WorkerState.Error:
                        worker.OnErrorWorker.Invoke();
                        Workers.Remove(worker);
                        break;
                }
            }
        }

        /// <summary>
        /// Starts and adds a ThreadWorker into the dispatcher's system
        /// </summary>
        /// <param name="worker">ThreadWorker to be started</param>
        public static void StartWorker(ThreadWorker worker)
        {
            EnsureInstantiated();
            Thread t = new Thread(worker.WorkHandleStart);
            worker.OnStartWorker.Invoke();
            t.Start();
            Workers.Add(worker, t);
        }

        /// <summary>
        /// Checks if a ThreadWorker is currently being handled by the dispatcher
        /// </summary>
        /// <param name="worker">ThreadWorker to check</param>
        /// <returns>true or false if the thread is in the dispatcher</returns>
        public static bool ContainsThreadWorker(ThreadWorker worker)
        {
            EnsureInstantiated();
            return Workers.ContainsKey(worker);
        }

        /// <summary>
        /// Forcefully aborts a thread associated with a thread worker
        /// </summary>
        /// <param name="worker">ThreadWorker to force abort</param>
        public static void ForceAbortThreadWorker(ThreadWorker worker)
        {
            EnsureInstantiated();
            if (!ContainsThreadWorker(worker)) return;
            Thread t = Workers[worker];
            if (!t.IsAlive) return;
            t.Abort();
            Workers.Remove(worker);
        }

        /// <summary>
        /// Forcefully aborts all threads being managed by the dispatcher
        /// </summary>
        public static void ForceAbortAllThreadWorkers()
        {
            EnsureInstantiated();
            ThreadWorker[] keys = Workers.Keys.ToArray();
            for (int i = keys.Length - 1; i >= 0; i--)
            {
                Thread t = Workers[keys[i]];
                if(t.IsAlive) t.Abort();
                Workers.Remove(keys[i]);
            }
        }
    }
}