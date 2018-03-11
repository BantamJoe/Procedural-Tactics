using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenRoom : Room {

	void Start()
    {
        regenTime = 2f;
        regenSpeed = 0.05f;
        regenAmount = regenTime;

        upgradeCost = 50;

        barOriginalScale = health.bar.transform.localScale;
        SetBarScale(level);
        barOriginalPosition = health.bar.transform.localPosition;
        SetBarPosition(level);

        mech = transform.root.gameObject;
        m = mech.GetComponent<Mech>();

        InvokeRepeating("AutoRepair", 0f, 1f);
    }

    public override void FindBars()
    {
        ener.bar = UI.transform.Find("OxygenEnergyBar").gameObject.GetComponent<Bar>();
        health.bar = UI.transform.Find("OxygenHealthBar").gameObject.GetComponent<Bar>();
    }

    void Update() 
    {
        SetEnergyToCurrentHealth();
        ManageColor();

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
