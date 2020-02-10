# Unity Multithreading Handler
A simple script setup to inherit and quickly start multithreading in Unity Engine.

## Installation
Either clone this repo into your unity project or use the provided [UnityMultithreadingHandler.unitypackage](https://github.com/rhedgeco/UnityMultithreadingHandler/releases)

## How to use
Create a class that inherits from ThreadWorker.
The work body is where all your off main thread processing will go.
Use the provided callbacks to make updates happen on the main thread.
```c#
using UnityMultithreading;

public class ExampleThreadWorker : ThreadWorker
{
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
        // work to be done
    }

    private void ExampleStartProcess()
    {
    }

    private void ExampleUpdateProcess()
    {
    }

    private void ExampleCloseProcess()
    {
    }

    private void ExampleErrorProcess()
    {
    }
}
```
A ThreadDispatcher must be active in your unity scene. It will force itself into a singleton.
You may then call the thread dispatcher statically to start a ThreadWorker like this
```c#
ExampleThreadWorker worker = new ExampleThreadWorker();
ThreadDispatcher.StartWorker(worker); // This will start the worker immediately
```