using System;
using System.Threading.Tasks;
using UnityEngine;

public static class Delaying
{   
    public async static void Delay(Action action, float time)
    {
        await Task.Delay((int)(time * 1000));
        action();
    }
}
