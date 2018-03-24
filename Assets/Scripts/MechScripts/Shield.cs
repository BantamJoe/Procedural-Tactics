using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

    public Stat shield;

    public float regenTime;
    public float regenSpeed;
    public float regenAmount;

    public int lastShieldValue;

    public GameObject shieldRoom;
    public GameObject playerMech;

    Color oldColor;


    void Start()
    {
        playerMech = transform.parent.gameObject;
        shieldRoom = transform.parent.Find("Torso").Find("ShieldRoom").gameObject;
        shield = shieldRoom.GetComponent<ShieldRoom>().shield;

        regenTime = 2f;
        regenSpeed = 1f;
        regenAmount = regenTime;

        oldColor = gameObject.GetComponent<Renderer>().material.color;

        InvokeRepeating("ManageShieldState", 0f, 1f);
    }

    public void ManageShieldState()
    {
        if (playerMech.GetComponent<Mech>().mechMode == Mech.Mode.Movement)
        {
            shield.CurrentVal = 0;
            regenAmount = 0;
        }
        else
        {
            if (regenAmount < regenTime)
            {
                regenAmount += regenSpeed;

                if (regenAmount > regenTime)
                {
                    regenAmount = regenTime;
                }
            }
            else if (shield.CurrentVal < shieldRoom.GetComponent<Room>().ener.CurrentVal)
            {
                regenAmount = 0;
                shield.CurrentVal++;
            }

            if (shield.CurrentVal > shieldRoom.GetComponent<Room>().ener.CurrentVal)
            {
                shield.CurrentVal = shieldRoom.GetComponent<Room>().ener.CurrentVal;
            }

            if (shield.CurrentVal == shieldRoom.GetComponent<Room>().ener.CurrentVal)
            {
                regenAmount = 0;
            }
        }
    }

    public void ManageTransparency()
    {
        //gameObject.GetComponent<Renderer>().material.color = new Color(oldColor.r, oldColor.g, oldColor.b, oldColor.a + shield.CurrentVal * 0.07f);
    }

    void Update()
    {

        //if (Input.GetKeyDown(KeyCode.F5))
        //{
        //    if (gameObject.GetComponent<BubbleShield>() != null)
        //    {
        //        Vector3 rowX = new Vector3(0.04094822f, 0.2440016f, 0.5316556f);
        //        gameObject.GetComponent<BubbleShield>().AddImpact(rowX);
        //    }
        //}

        //Manage shield status
        if (playerMech.GetComponent<Mech>().mechMode == Mech.Mode.Movement)
        {
            shield.CurrentVal = 0;
        }
        else
        {
            if (shieldRoom.GetComponent<Room>() != null)
            {
                if (shield.CurrentVal > shieldRoom.GetComponent<Room>().ener.CurrentVal)
                {
                    shield.CurrentVal = shieldRoom.GetComponent<Room>().ener.CurrentVal;
                }
            }
        }
        
        //Manage visuals
        if (shield.CurrentVal <= 0)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;

            if (gameObject.GetComponent<SphereCollider>() != null)
            {
                gameObject.GetComponent<SphereCollider>().enabled = false;
            }
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;

            if (gameObject.GetComponent<SphereCollider>() != null)
            {
                gameObject.GetComponent<SphereCollider>().enabled = true;
            }
        }

        if (lastShieldValue != shield.CurrentVal)
        {
            ManageTransparency();
        }

        lastShieldValue = shield.CurrentVal;
    }

}
