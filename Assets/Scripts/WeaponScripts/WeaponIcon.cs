using UnityEngine;
using System.Collections;

public class WeaponIcon : MonoBehaviour {

    public Stat loadState;

    public GameObject UI;
    public Weapon weapon;

    void Start()
    {
        UI = GameObject.Find("PlayerCanvas");

        loadState.bar = transform.Find("LoadBar").gameObject.GetComponent<Bar>();
        loadState.Initialize();

        loadState.MaxVal = 100;
        loadState.CurrentVal = 0;

        //weapon = GetComponent<WeaponSelection>().weapon;
    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    void Update() 
    {
        if (weapon)
        {
            loadState.CurrentVal = (int)Map(weapon.originalFireCountdown - weapon.fireCountdown, 0, weapon.originalFireCountdown, 0, 100);
        }
    }
}
