using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Generator : MonoBehaviour {

    public GameObject UI;

    public Stat energy;
    public int usedEnergy;
    public Vector3 barOriginalScale;
    public Vector3 barOriginalPosition;
    public int level;

    public List<GameObject> roomList;
    public GameObject mech;
    public Room generatorRoom;


    void Start()
    {
        
        mech = transform.parent.parent.gameObject;
        UI = mech.GetComponent<Mech>().UI;
        energy.bar = UI.transform.Find("EnergyBar").gameObject.GetComponent<Bar>();
        energy.Initialize();
        energy.MaxVal = 4 + GameManager.sectorNumber * 6;
        energy.CurrentVal = energy.MaxVal;
        level = energy.MaxVal;

        barOriginalScale = energy.bar.transform.localScale;
        SetBarScale(level);
        barOriginalPosition = energy.bar.transform.localPosition;
        SetBarPosition(level);

        roomList = mech.GetComponent<Mech>().roomList;
        generatorRoom = GetComponent<Room>();
    }

    public void SetBarScale(int level)
    {
        energy.bar.transform.localScale = new Vector3(barOriginalScale.x * level, barOriginalScale.y, barOriginalScale.z);
    }

    public void SetBarPosition(int level)
    {
        energy.bar.transform.localPosition = barOriginalPosition + (level - 1) * new Vector3(energy.bar.GetComponent<RectTransform>().rect.width * barOriginalScale.x / 2, 0, 0);
    }

    void Update() 
    {
        int temp = 0;

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i] != null)
            {
                if (roomList[i].GetComponent<Room>() != null)
                {
                    temp = temp + roomList[i].GetComponent<Room>().ener.CurrentVal;
                    usedEnergy = temp;
                }
            }
        }

        if (generatorRoom.health.CurrentVal <= 0)
        {
            usedEnergy += 3;
        }

        energy.CurrentVal = energy.MaxVal - usedEnergy;

        while (energy.CurrentVal < 0)
        {
            roomList[Random.Range(0, roomList.Count)].GetComponent<Room>().DecreaseEnergy();
        }
    }

}
