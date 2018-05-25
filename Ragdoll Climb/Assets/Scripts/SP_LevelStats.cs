using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class SP_LevelStats 
{
    public bool completed = false;
    public int starAmount = 0;
    public float bestTime_flt = Mathf.Infinity;
    public string bestTime_str = "--:--:--";
}
