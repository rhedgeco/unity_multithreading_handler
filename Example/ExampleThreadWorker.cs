using System.Threading;
using UnityEngine;
using UnityMultithreading;

namespace UnityMultithreadingHandler.Example
{
    public class ExampleThreadWorker : ThreadWorker
    {
        // you may use variables in this class to control your own state
        public float Progress { get; private set; }
    
        public ExampleThreadWorker()
        {
            // Add methods as listeners to have information update
            // These listeners will be called on the main thread
            OnStartWorker.AddListener(ExampleStartProcess);
            OnUpdateWorker.AddListener(ExampleUpdateProcess);
            OnCloseWorker.AddListener(ExampleCloseProcess);
            OnErrorWorker.AddListener(ExampleErrorProcess);
        }

        // Only work done in here will be run on seperate thread
        protected override void WorkBody()
        {
            // example of work to be done
            Thread.Sleep(2000);
            int iterations = 5000;
            for (int i = 0; i < iterations; i++)
            {
                Thread.Sleep(1);
                Progress = (float) i / iterations;
            }
            Thread.Sleep(1000);
        }

        private void ExampleStartProcess()
        {
            // Debug.Log can technically be used off the main thread,
            // but we will still use it here to display loading percentage
            Debug.Log("Starting example processing...");
        }

        private void ExampleUpdateProcess()
        {
            Debug.Log($"Progress : {(int)(Progress*100)}");
        }

        private void ExampleCloseProcess()
        {
            Debug.Log("Finished processing!");
        }

        private void ExampleErrorProcess()
        {
            Debug.Log("ERROR: This should no have happened :(");
        }
    }
}