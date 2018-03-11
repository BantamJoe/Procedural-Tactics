using UnityEngine;

public abstract class Room : MonoBehaviour {
    
    public Stat ener;
    public Stat health;
    public Generator generator;

    public int previousHealth;
    public int level = 1;
    public int upgradeCost = 50;

    public string roomType;
    public float regenTime;
    public float regenSpeed;
    public float regenAmount;

    public Vector3 barOriginalScale;
    public Vector3 barOriginalPosition;

    public GameObject UI;
    public GameObject mech;
    public Mech m;

    void Awake()
    {
        if (transform.parent.GetComponent<Mech>() != null)
        {
            UI = transform.root.GetComponent<Mech>().UI;
            mech = transform.root.gameObject;
        }
        else
        {
            UI = transform.root.GetComponent<Mech>().UI;
            mech = transform.root.gameObject;
        }

        if (transform.parent.Find("GeneratorRoom") != null)
        {
            generator = transform.parent.Find("GeneratorRoom").gameObject.GetComponent<Generator>();
        }
        else
        {
            generator = transform.parent.Find("Torso").Find("GeneratorRoom").gameObject.GetComponent<Generator>();
        }

        mech.GetComponent<Mech>().roomList.Add(gameObject);
    }

    public void SetBarScale(int level)
    {
        health.bar.transform.localScale = new Vector3(barOriginalScale.x * level, barOriginalScale.y, barOriginalScale.z);
        ener.bar.transform.localScale = new Vector3(barOriginalScale.x * level, barOriginalScale.y, barOriginalScale.z);
    }

    public void SetBarPosition(int level)
    {
        health.bar.transform.localPosition = barOriginalPosition + (level - 1) * new Vector3(health.bar.GetComponent<RectTransform>().rect.width * barOriginalScale.x / 2, 0, 0);
        ener.bar.transform.localPosition = barOriginalPosition + (level - 1) * new Vector3(ener.bar.GetComponent<RectTransform>().rect.width * barOriginalScale.x / 2, 0, 0);
    }

    public virtual void FindBars()
    {

    }

    public void ManageColor()
    {
        if (previousHealth != health.CurrentVal)
        {
            if (previousHealth > 0 && health.CurrentVal <= 0)
            {
                GetComponent<Renderer>().material.color = Color.red;
            }
            else if (health.CurrentVal < health.MaxVal && health.CurrentVal > 0)
            {
                GetComponent<Renderer>().material.color = Color.yellow;
            }
            else
            {
                GetComponent<Renderer>().material.color = Color.black;
            }
        }

        previousHealth = health.CurrentVal;

        //Temporary
        if (transform.childCount != 0)
        {
            transform.GetChild(0).GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;
        }
    }

    public void IncreaseEnergy(int amount)
    {
        if (generator.energy.CurrentVal-amount >= 0 && ener.CurrentVal+amount <= ener.MaxVal)
        {
            generator.energy.CurrentVal -= amount;
            ener.CurrentVal+= amount;
            SetEnergyToCurrentHealth();
        }
    }

    public void DecreaseEnergy()
    {
        if (generator.energy.CurrentVal < generator.energy.MaxVal && ener.CurrentVal > 0)
        {
            generator.energy.CurrentVal++;
            ener.CurrentVal--;
        }
    }

    public void SetEnergyToCurrentHealth()
    {
        if (health.CurrentVal < 0)
        {
            health.CurrentVal = 0;
        };

        if (ener.CurrentVal > health.CurrentVal)
        {
            ener.CurrentVal = health.CurrentVal;
        }
    }

    public void AutoRepair()
    {
        if (regenAmount < regenTime)
        {
            regenAmount += regenSpeed;

            if (regenAmount > regenTime)
            {
                regenAmount = regenTime;
            }
        }
        else if (health.CurrentVal < health.MaxVal)
        {
            regenAmount = 0;
            health.CurrentVal++;
        }

        if (health.CurrentVal == health.MaxVal)
        {
            regenAmount = 0;
        }
    }

    public virtual void RoomHit(int damage)
    {
        //Implement
        //if (enemyRoom.health.CurrentVal > 0)
        //{
        //    enemyRoom.health.CurrentVal -= damage;
        //}

        //if (enemyRoom.transform.root.GetComponent<Mech>().health.CurrentVal > 0)
        //{
        //    enemyRoom.transform.root.GetComponent<Mech>().health.CurrentVal -= damage;
        //}
    }

    public virtual void LevelUp()
    {
        level++;
        health.MaxVal++;
        health.CurrentVal++;
        ener.MaxVal++;
        health.UpdateBar();
        ener.UpdateBar();

        SetBarScale(level);
        SetBarPosition(level);

        if (level <= 4)
        {
            upgradeCost = upgradeCost * 2;
        }
        else
        {
            upgradeCost = upgradeCost + 200;
        }
        
    }

}
