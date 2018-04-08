using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerUI : MonoBehaviour {

    public Text resourcesText;
    public Text lootResourcesText;
    public Text energyText;
    public Text sectorNumberText;

    public Text upgradeCostText;

    public GameObject player;
    public GameObject playerMech;
    public Mech pMech;
    public Room cockpitRoom;
    public Room engineRoom;
    public Room generatorRoom;
    public Room oxygenRoom;
    public Room shieldRoom;
    public Room weaponRoom;


    public GameObject weaponsUI;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
    
    void Start()
    {
        playerMech = GameObject.Find("PlayerMech(Clone)");
        pMech = playerMech.GetComponent<Mech>();

        engineRoom = pMech.transform.Find("EngineRoom").GetComponent<Room>();

        cockpitRoom = pMech.transform.Find("Torso").Find("CockpitRoom").GetComponent<Room>();
        generatorRoom = pMech.transform.Find("Torso").Find("GeneratorRoom").GetComponent<Room>();
        oxygenRoom = pMech.transform.Find("Torso").Find("OxygenRoom").GetComponent<Room>();
        shieldRoom = pMech.transform.Find("Torso").Find("ShieldRoom").GetComponent<Room>();
        weaponRoom = pMech.transform.Find("Torso").Find("WeaponRoom").GetComponent<Room>();

        GameObject weaponsUI = transform.Find("WeaponsUI").gameObject;
    }

    public void ClearLoot()
    {
        foreach (Transform child in transform.Find("InventoryLoot"))
        {
            Destroy(child.gameObject);
        }

        transform.Find("InventoryLoot").gameObject.SetActive(false);
        lootResourcesText.gameObject.SetActive(false);
        transform.Find("TakeLootButton").gameObject.SetActive(false);
    }

    public void TakeLoot()
    {
        List<Transform> children = new List<Transform>();

        foreach (Transform child in transform.Find("InventoryLoot"))
        {
            children.Add(child);
        }

        foreach (Transform child in children)
        {
            child.transform.SetParent(transform.Find("Inventory"), false);
            child.gameObject.GetComponent<WeaponSelection>().attachedWeapon.transform.SetParent(playerMech.transform);
        }

        Player.resources += CalculateResourceLoot();

        transform.Find("InventoryLoot").gameObject.SetActive(false);
        lootResourcesText.gameObject.SetActive(false);
        transform.Find("TakeLootButton").gameObject.SetActive(false);
    }

    public int CalculateResourceLoot()
    {
        int resourceAmount = 2 * GameManager.sectorDifficulty + 2 * GameManager.sectorNumber + 10;
        return resourceAmount;
    }

    
    void Update()
    {
        resourcesText.text = "Resources:" + Player.resources.ToString();
        lootResourcesText.text = "Resources:" + CalculateResourceLoot().ToString();
        upgradeCostText.text = 
        "Level: " + cockpitRoom.level.ToString() + "  Upgrade Cost: " + cockpitRoom.upgradeCost.ToString() + "\n" + "\n" + "Level: " + engineRoom.level.ToString() + "  Upgrade Cost: " + engineRoom.upgradeCost.ToString() + "\n" + "\n" +
        "Level: " + generatorRoom.level.ToString() + "  Upgrade Cost: " + generatorRoom.upgradeCost.ToString() + "\n" + "\n" + "Level: " + oxygenRoom.level.ToString() + "  Upgrade Cost: " + oxygenRoom.upgradeCost.ToString() + "\n" + "\n" +
        "Level: " + shieldRoom.level.ToString() + "  Upgrade Cost: " + shieldRoom.upgradeCost.ToString() + "\n" + "\n" + "Level: " + weaponRoom.level.ToString() + "  Upgrade Cost: " + weaponRoom.upgradeCost.ToString();

        if (playerMech != null)
        {
            energyText.text = "Energy:" + playerMech.transform.Find("Torso").Find("GeneratorRoom").GetComponent<Generator>().energy.CurrentVal.ToString() + "/" + transform.Find("EnergyBar").gameObject.GetComponent<Bar>().maxValue.ToString();
        }

        sectorNumberText.text = "Sector:" + GameManager.sectorNumber.ToString();

        if (Input.GetKeyDown(KeyCode.I))
        {
            transform.Find("Inventory").gameObject.SetActive(!gameObject.transform.Find("Inventory").gameObject.activeInHierarchy);
            transform.Find("UpgradeUI").gameObject.SetActive(!gameObject.transform.Find("UpgradeUI").gameObject.activeInHierarchy);

        }

        //Weapon hotkeys
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (transform.Find("WeaponsUI").childCount > 0)
            {
                transform.Find("WeaponsUI").GetChild(0).gameObject.GetComponent<WeaponSelection>().SelectWeapon();
            }

        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (transform.Find("WeaponsUI").childCount > 1)
            {
                transform.Find("WeaponsUI").GetChild(1).gameObject.GetComponent<WeaponSelection>().SelectWeapon();
            }

        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (transform.Find("WeaponsUI").childCount > 2)
            {
                transform.Find("WeaponsUI").GetChild(2).gameObject.GetComponent<WeaponSelection>().SelectWeapon();
            }

        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (transform.Find("WeaponsUI").childCount > 3)
            {
                transform.Find("WeaponsUI").GetChild(3).gameObject.GetComponent<WeaponSelection>().SelectWeapon();
            }

        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (transform.Find("WeaponsUI").childCount > 4)
            {
                transform.Find("WeaponsUI").GetChild(4).gameObject.GetComponent<WeaponSelection>().SelectWeapon();
            }

        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            if (transform.Find("WeaponsUI").childCount > 5)
            {
                transform.Find("WeaponsUI").GetChild(5).gameObject.GetComponent<WeaponSelection>().SelectWeapon();
            }

        }
    }
}