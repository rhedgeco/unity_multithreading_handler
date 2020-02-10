using UnityEngine;
using UnityEngine.UI;
using UnityMultithreading;

namespace UnityMultithreadingHandler.Example
{
    [RequireComponent(typeof(Button))]
    public class ExampleThreadActivator : MonoBehaviour
    {
        private void Awake()
        {
            // Attach the method to the button to activate it when clicked
            GetComponent<Button>().onClick.AddListener(ActivateExampleThread);
        }

        public void ActivateExampleThread()
        {
            // Create it.
            ExampleThreadWorker worker = new ExampleThreadWorker();
            // Start it.
            ThreadDispatcher.StartWorker(worker);
            // Profit.
        }
    }
}