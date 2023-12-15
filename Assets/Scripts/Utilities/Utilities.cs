
using System;
using System.Collections;
using UnityEngine;

public static class Utilities
{
    public static void ActionDelay(Action action, float time = 0)
    {
        Coroutine.Run(action, time);
    }
}

internal class Coroutine : MonoBehaviour
{
    private static Coroutine _instance;

    public static Coroutine Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new("CoroutineManager");
                _instance = obj.AddComponent<Coroutine>();
            }
            return _instance;
        }
    }
    public static void Run(Action action, float time = 0)
    {
        Instance.StartCoroutine(Instance.Routine(action, time));
    }

    IEnumerator Routine(Action action, float time = 0)
    {
        yield return new WaitForSeconds(time);
        action?.Invoke();
    }
}