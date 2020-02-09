using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace UnityMultithreading
{
    [DisallowMultipleComponent]
    public class ThreadDispatcher : MonoBehaviour
    {
        public static ThreadDispatcher Dispatcher { get; private set; }

        private List<ThreadWorker> Workers { get; } = new List<ThreadWorker>();

        private void Awake()
        {
            CreateSingleton();
        }

        private void Update()
        {
            UpdateWorkers();
        }

        private void CreateSingleton()
        {
            if (!Dispatcher) Dispatcher = this;
            if (Dispatcher != this) Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }

        private void UpdateWorkers()
        {
            for (int i = Workers.Count - 1; i >= 0; i--)
            {
                ThreadWorker worker = Workers[i];
                switch (worker.State)
                {
                    case ThreadWorker.WorkerState.Running:
                        worker.OnUpdateWorker.Invoke();
                        break;
                    case ThreadWorker.WorkerState.Closed:
                        worker.OnCloseWorker.Invoke();
                        Workers.RemoveAt(i);
                        break;
                    case ThreadWorker.WorkerState.Error:
                        worker.OnErrorWorker.Invoke();
                        Workers.RemoveAt(i);
                        break;
                }
            }
        }

        public void StartWorker(ThreadWorker worker)
        {
            Thread t = new Thread(worker.WorkHandleStart);
            worker.OnStartWorker.Invoke();
            t.Start();
            Workers.Add(worker);
        }
    }
}
