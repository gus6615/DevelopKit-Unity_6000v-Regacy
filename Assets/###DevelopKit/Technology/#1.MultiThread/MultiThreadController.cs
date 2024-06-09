using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Async & Await 관련
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

// Job System 관련
using Unity.Jobs;

public struct TestJob : IJobParallelFor
{

    public void Execute(int index)
    {
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 10000000; j++)
            {
                int temp = 1 + 2;
            }
        }
    }
}

public class MultiThreadController : MonoBehaviour
{
    [SerializeField] private Button singleThreadButton, asyncThreadButton, jobSystemThreadButton;
    [SerializeField] private TMP_Text singleThreadResult, asyncThreadResult, jobSystemThreadResult;

    // Start is called before the first frame update
    void Start()
    {
        singleThreadButton.onClick.AddListener(AsyncOnSingleThread);
        asyncThreadButton.onClick.AddListener(AsyncOnMultiThread);
        jobSystemThreadButton.onClick.AddListener(AsyncOnJobSystem);
    }

    private void AsyncOnSingleThread()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        singleThreadResult.text = "Processing...";

        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 10000000; j++)
            {
                int temp = 1 + 2;
            }
        }

        stopwatch.Stop();

        singleThreadResult.text = $"{stopwatch.ElapsedMilliseconds} ms";
    }

    private async void AsyncOnMultiThread()
    {
        Stopwatch stopwatch = new Stopwatch();
        List<Task> tasks = new List<Task>();

        asyncThreadResult.text = "Processing...";

        stopwatch.Start();

        for (int i = 0; i < 100; i++)
        {
            tasks.Add(Task.Run(() =>
            {
                for (int j = 0; j < 10000000; j++)
                {
                    int temp = 1 + 2;
                }
            }));
        }

        await Task.WhenAll(tasks);

        stopwatch.Stop();

        asyncThreadResult.text = $"{stopwatch.ElapsedMilliseconds} ms";
    }

    private void AsyncOnJobSystem()
    {
        Stopwatch stopwatch = new Stopwatch();
        //JobHandle jobHandle = new TestJob().Schedule();

        jobSystemThreadResult.text = "Processing...";

        stopwatch.Start();

        //jobHandle.Complete();

        stopwatch.Stop();

        jobSystemThreadResult.text = $"{stopwatch.ElapsedMilliseconds} ms";
    }
}
