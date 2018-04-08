using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenRoom : Room {

	void Start()
    {
        regenTime = 2f;
        regenSpeed = 0.07f;
        regenAmount = regenTime;

        upgradeCost = 50;

        barOriginalScale = health.bar.transform.localScale;
        SetBarScale(level);
        barOriginalPosition = health.bar.transform.localPosition;
        SetBarPosition(level);

        mech = transform.root.gameObject;
        m = mech.GetComponent<Mech>();

        InvokeRepeating("AutoRepair", 0f, 1f);

        defaultMat = GetComponent<Renderer>().material;

        if (transform.Find("SmokeEffect") != null)
        {
            defaultSmokeMat = transform.Find("SmokeEffect").gameObject.GetComponent<Renderer>().material;
            defaultSmokeColor = transform.Find("SmokeEffect").gameObject.GetComponent<Renderer>().material.color;
        }

    }

    public override void FindBars()
    {
        ener.bar = UI.transform.Find("OxygenEnergyBar").gameObject.GetComponent<Bar>();
        health.bar = UI.transform.Find("OxygenHealthBar").gameObject.GetComponent<Bar>();
    }

    void Update() 
    {
        SetEnergyToCurrentHealth();
        ManageColorAndSmoke();

        if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.R))
        {
            IncreaseEnergy(1);
            
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.R))
        {
            DecreaseEnergy();
            
        }

        if (ener.CurrentVal <= 0 && mech.GetComponent<Mech>().oxygen.CurrentVal > 0)
        {
            mech.GetComponent<Mech>().oxygen.CurrentVal--;
        }
        else if (mech.GetComponent<Mech>().oxygen.CurrentVal < 1000)
        {
            mech.GetComponent<Mech>().oxygen.CurrentVal++;
        }
    }
}
