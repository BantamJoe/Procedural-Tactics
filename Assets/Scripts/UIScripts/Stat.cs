using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Stat {

    [SerializeField]
    public Bar bar;

    [SerializeField]
    private int maxVal;

    [SerializeField]
    private int currentVal;

    public int CurrentVal
    {
        get
        {
            return currentVal;
        }

        set
        {
            if (value <= maxVal)
            { 
                currentVal = value;
                bar.Value = currentVal;
            }
            else
            {
                currentVal = maxVal;
                bar.Value = currentVal;
            }
        }
    }

    public void UpdateBar()
    {
        bar.Value = currentVal;
    }

    public int MaxVal
    {
        get
        {
            return maxVal;
        }

        set
        {
            maxVal = value;
            if (bar != null)
            {
                bar.maxValue = maxVal;
            }
        }
    }

    public void Initialize()
    {
        MaxVal = maxVal;
        currentVal = maxVal;
        CurrentVal = currentVal;
    }
}
