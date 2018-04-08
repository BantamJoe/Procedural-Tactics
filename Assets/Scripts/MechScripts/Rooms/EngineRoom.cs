using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineRoom : Room {

    public float originalMaxSpeed;
    public float originalMaxRotationSpeed;
    public float originalAcceleration;
    public float originalRotationAcceleration;

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

        originalMaxSpeed = m.maxSpeed;
        originalMaxRotationSpeed = m.maxRotationSpeed;
        originalAcceleration = m.acceleration;
        originalRotationAcceleration = m.rotationAcceleration;

        m.fullAcceleration = originalAcceleration;
        m.fullRotationAcceleration = originalRotationAcceleration;

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

    public override void LevelUp()
    {
        level++;
        health.MaxVal++;
        health.CurrentVal++;
        ener.MaxVal++;
        health.UpdateBar();
        ener.UpdateBar();

        SetBarScale(level);
        SetBarPosition(level);

        m.fullAcceleration = m.fullAcceleration + originalAcceleration * 0.2f;
        m.fullRotationAcceleration = m.fullRotationAcceleration + originalRotationAcceleration * 0.2f;

        if (level <= 4)
        {
            upgradeCost = upgradeCost * 2;
        }
        else
        {
            upgradeCost = upgradeCost + 200;
        }

    }

    void Update() 
    {
        SetEnergyToCurrentHealth();
        ManageColorAndSmoke();

        if (ener.CurrentVal > 0)
        {
            m.maxSpeed = originalMaxSpeed + ener.CurrentVal * 0.1f * originalMaxSpeed;
            m.maxRotationSpeed = originalMaxRotationSpeed + ener.CurrentVal * 0.1f * originalMaxRotationSpeed;
            m.acceleration = originalAcceleration + ener.CurrentVal * 0.2f * originalAcceleration;
            m.rotationAcceleration = originalRotationAcceleration + ener.CurrentVal * 0.2f * originalRotationAcceleration;
        }
        else
        {
            m.acceleration = 0f;
            m.rotationAcceleration = 0f;
        }
        

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
