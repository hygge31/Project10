using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util: MonoBehaviour
{


    public string GetChangeTimeFormat(float num)
    {
        int m = (int)num / 60;
        int s = (int)num % 60;

        string time = $"{m:00} : {s:00}";
        return time;
    }
}
