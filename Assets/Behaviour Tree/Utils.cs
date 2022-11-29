using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils 
{
    public static System.Random r = new();
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while(n>1)
        {
            n--;
            int k = r.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
