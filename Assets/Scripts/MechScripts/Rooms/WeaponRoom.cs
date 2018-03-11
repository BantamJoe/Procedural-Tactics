using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRoom : Room {

    void Awake()
    {
        if (transform.parent.GetComponent<Mech>() != null)
        {
            UI = transform.parent.GetComponent<Mech>().UI;
        }
        else
        {
            UI = transform.parent.parent.GetComponent<Mech>().UI;
        }

        generator = transform.parent.Find("GeneratorRoom").gameObject.GetComponent<Generator>();

        mech = transform.parent.parent.gameObject;
        mech.GetComponent<Mech>().roomList.Add(gameObject);
    }

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
        ener.bar = UI.transform.Find("WeaponEnergyBar").gameObject.GetComponent<Bar>();
        health.bar = UI.transform.Find("WeaponHealthBar").gameObject.GetComponent<Bar>();
    }

    void Update() 
    {
        SetEnergyToCurrentHealth();
        ManageColor();

        if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Y))
        {
            IncreaseEnergy(1);
            
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Y))
        {
            DecreaseEnergy();
            
        }
    }
}
