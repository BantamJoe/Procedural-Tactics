using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldRoom : Room {

    public Stat shield;
    public GameObject shieldObj;
    public Vector3 shieldBarOriginalScale;
    public Vector3 shieldBarOriginalPosition;

    void Start()
    {
        shield.bar = UI.transform.Find("ShieldBar").gameObject.GetComponent<Bar>();

        shield.Initialize();

        shield.MaxVal = ener.MaxVal;
        shield.CurrentVal = shield.MaxVal;

        regenTime = 2f;
        regenSpeed = 0.05f;
        regenAmount = regenTime;

        upgradeCost = 50;

        barOriginalScale = health.bar.transform.localScale;
        SetBarScale(level);
        barOriginalPosition = health.bar.transform.localPosition;
        SetBarPosition(level);

        shieldBarOriginalScale = shield.bar.transform.localScale;
        SetShieldBarScale(level);
        shieldBarOriginalPosition = shield.bar.transform.localPosition;
        SetShieldBarPosition(level);

        mech = transform.root.gameObject;
        m = mech.GetComponent<Mech>();

        shieldObj = transform.parent.parent.Find("Shield").gameObject;

        InvokeRepeating("AutoRepair", 0f, 1f);

        defaultMat = GetComponent<Renderer>().material;

        if (transform.Find("SmokeEffect") != null)
        {
            defaultSmokeMat = transform.Find("SmokeEffect").gameObject.GetComponent<Renderer>().material;
            defaultSmokeColor = transform.Find("SmokeEffect").gameObject.GetComponent<Renderer>().material.color;
        }

    }

    public void SetShieldBarScale(int level)
    {
        shield.bar.transform.localScale = new Vector3(shieldBarOriginalScale.x * level, shieldBarOriginalScale.y, shieldBarOriginalScale.z);
    }

    public void SetShieldBarPosition(int level)
    {
        shield.bar.transform.localPosition = shieldBarOriginalPosition + (level - 1) * new Vector3(shield.bar.GetComponent<RectTransform>().rect.width * shieldBarOriginalScale.x / 2, 0, 0);
    }

    public override void FindBars()
    {
        ener.bar = UI.transform.Find("ShieldEnergyBar").gameObject.GetComponent<Bar>();
        health.bar = UI.transform.Find("ShieldHealthBar").gameObject.GetComponent<Bar>();
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

        shield.MaxVal = ener.MaxVal;
        shield.UpdateBar();

        SetShieldBarScale(level);
        SetShieldBarPosition(level);

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
    }
}
