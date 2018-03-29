using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineRoom : Room {

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

        defaultMat = GetComponent<Renderer>().material;

        if (transform.Find("SmokeEffect") != null)
        {
            defaultSmokeMat = transform.Find("SmokeEffect").gameObject.GetComponent<Renderer>().material;
            defaultSmokeColor = transform.Find("SmokeEffect").gameObject.GetComponent<Renderer>().material.color;
        }
    }

    public override void FindBars()
    {
        ener.bar = UI.transform.Find("EngineEnergyBar").gameObject.GetComponent<Bar>();
        health.bar = UI.transform.Find("EngineHealthBar").gameObject.GetComponent<Bar>();
    }

    void Update() 
    {
        SetEnergyToCurrentHealth();
        ManageColorAndSmoke();

        if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.W))
        {
            IncreaseEnergy(1);
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.W))
        {
            DecreaseEnergy();
        }

        if (ener.CurrentVal <= 0 && m.mechMode == Mech.Mode.Movement)
        {
            m.mechMode = m.previousMechMode;
        }
    }
}
