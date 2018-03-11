using UnityEngine;
using System.Collections;

public class EnemyMech : Mech {

    [Header("Objects")]

    public GameObject canvas;
    public GameObject playerMech;
    public GameObject enemySpawner;
    public GameObject gameMaster;
    public GameManager gameManager;

    public Generator generator;
    public int totalBurstDamage;

    private int temp;
    private int enemyCount;
    private int divisor = 1;


    private void Awake()
    {
        //UI
        UI = Instantiate(enemyMechUIPrefab) as GameObject;
    }
    
    void Start()
    {
        enemySpawner = GameObject.Find("EnemySpawner");
        generator = transform.Find("Torso").Find("GeneratorRoom").GetComponent<Generator>();

        //UI
        canvas = GameObject.Find("PlayerCanvas");
        
        playerMech = GameObject.Find("PlayerMech(Clone)");
        gameMaster = GameObject.Find("GameMaster");
        gameManager = gameMaster.GetComponent<GameManager>();

        UI.transform.SetParent(canvas.transform, false);
        UI.SetActive(false);

        health.bar = UI.transform.Find("HealthBar").gameObject.GetComponent<Bar>();
        heat.bar = UI.transform.Find("HeatBar").gameObject.GetComponent<Bar>();
        oxygen.bar = UI.transform.Find("OxygenBar").gameObject.GetComponent<Bar>();
        stamina.bar = UI.transform.Find("StaminaBar").gameObject.GetComponent<Bar>();
        pilotHealth.bar = UI.transform.Find("PilotHealthBar").gameObject.GetComponent<Bar>();

        health.Initialize();
        oxygen.Initialize();
        heat.Initialize();
        stamina.Initialize();
        pilotHealth.Initialize();

        mechMode = Mode.Neutral;

        health.MaxVal = GameManager.sectorNumber+4;
        health.CurrentVal = health.MaxVal;

        heat.MaxVal = 100;
        heat.CurrentVal = 0;

        oxygen.MaxVal = 1000;
        oxygen.CurrentVal = oxygen.MaxVal;

        stamina.MaxVal = 4000;
        stamina.CurrentVal = stamina.MaxVal;

        pilotHealth.MaxVal = 100;
        pilotHealth.CurrentVal = pilotHealth.MaxVal;

        shieldLevel = 1;

        maxSpeed = 15f;
        currentSpeed = 0f;
        acceleration = 0.01f;

        maxRotationSpeed = 20f;
        currentRotationSpeed = 0f;
        rotationAcceleration = 0.2f;

        destinationReached = true;
        rotationCompleted = true;

        halfRotationAngle = 90f;

        torso = transform.Find("Torso").gameObject;
        shield = transform.Find("Shield").gameObject;

        shieldRoom = transform.Find("Torso").Find("ShieldRoom").gameObject;
        weaponRoom = transform.Find("Torso").Find("WeaponRoom").gameObject;
        generatorRoom = transform.Find("Torso").Find("GeneratorRoom").gameObject;
        engineRoom = transform.Find("EngineRoom").gameObject;

        SetRoomHealths();

        //Weapons
        weaponRoom = gameObject.transform.Find("Torso").Find("WeaponRoom").gameObject;
        enemyCount = enemySpawner.GetComponent<EnemySpawner>().enemyList.Count;

        if (enemyCount > 1)
        {
            divisor = enemyCount;
        }

        while (equippedWeaponCount < 6 && totalBurstDamage < (GameManager.sectorNumber+1)/divisor && totalWeaponEnergyReq < weaponRoom.GetComponent<Room>().ener.MaxVal)
        {
            temp = Random.Range(0, GameManager.weaponList.Count);
            weapon = Instantiate(GameManager.weaponList[temp], transform.Find("Torso").Find("WeaponSlotParent").GetChild(equippedWeaponCount).position, transform.Find("Torso").Find("WeaponSlotParent").GetChild(equippedWeaponCount).rotation) as GameObject;
            weapon.transform.SetParent(transform.Find("Torso").Find("WeaponSlotParent").GetChild(equippedWeaponCount));

            weaponList.Add(weapon);

            equippedWeaponCount++;
            totalBurstDamage += weapon.GetComponent<Weapon>().burstDamage;
            totalWeaponEnergyReq += weapon.GetComponent<Weapon>().energyReq;
        }

        InvokeRepeating("SelectTarget", 0f, 8f);
        InvokeRepeating("SelectRandomDestination", 15f, 20f);

    }

    void PowerEnemyRooms()
    {
        foreach (GameObject room in roomList)
        {
            if (generator.energy.CurrentVal > 0 && room.GetComponent<Room>().ener.CurrentVal < room.GetComponent<Room>().ener.MaxVal)
            {
                room.GetComponent<Room>().IncreaseEnergy(1);
            }
        }
    }

    public void PowerEnemyWeapons()
    {
        //Weapons
        int len = weaponList.Count;
        int temp = 0;

        for (int i = 0; i < len; i++)
        {
            if (weaponList[i] != null)
            {
                if (weaponList[i].GetComponent<Weapon>().weaponPowered == true)
                {
                    if (temp + weaponList[i].GetComponent<Weapon>().energyReq <= weaponRoom.GetComponent<Room>().ener.CurrentVal)
                    {
                        temp += weaponList[i].GetComponent<Weapon>().energyReq;
                        totalWeaponEnergyReq = temp;
                    }
                    else
                    {
                        weaponList[i].GetComponent<Weapon>().weaponPowered = false;
                        weaponList[i].GetComponent<Weapon>().weaponSelected = true;
                        weaponList[i].GetComponent<Weapon>().targetSelected = false;
                        weaponList[i].GetComponent<Weapon>().enemyRoom = null;
                        weaponList[i].GetComponent<Weapon>().target = null;
                    }
                }
                else if (temp + weaponList[i].GetComponent<Weapon>().energyReq <= weaponRoom.GetComponent<Room>().ener.CurrentVal)
                {
                    temp += weaponList[i].GetComponent<Weapon>().energyReq;
                    totalWeaponEnergyReq = temp;
                    weaponList[i].GetComponent<Weapon>().weaponPowered = true;
                }
            }
        }
    }

    public void SelectDestination(float distance, Vector3 v)
    {
        if (engineRoom.GetComponent<Room>().ener.CurrentVal > 0)
        {
            if (destination != null)
            {
                Destroy(destination);
            }

            float spawnDistance = distance;
            Vector3 spawnPos = transform.position + v * spawnDistance;

            destination = Instantiate(destinationPrefab, spawnPos, Quaternion.identity);
            destinationReached = false;

            if (previousMechMode != Mode.Movement)
            {
                previousMechMode = mechMode;
            }
            else
            {
                previousMechMode = Mode.Neutral;
            }

            mechMode = Mode.Movement;
        }
    }

    public void SelectRandomDestination()
    {
        if (stamina.CurrentVal > stamina.MaxVal / 2 + stamina.MaxVal / 4)
        {
            float distance = Random.Range(5f, 25f);
            int temp = Random.Range(1, 10);
            Vector3 v = transform.forward;

            if (temp == 1)
            {
                v = -transform.forward;
            }
            if (temp == 2 || temp == 3)
            {
                v = transform.right;
            }
            if (temp == 4 || temp == 5)
            {
                v = -transform.right;
            }
            SelectDestination(distance, v);
        }
    }

    void SelectTarget()
    {
        if (playerMech == null)
        {
            playerMech = GameObject.Find("PlayerMech(Clone)");
        }

        if (playerMech != null)
        {
            targetRoomList = playerMech.GetComponent<PlayerMech>().roomList;
            int len = weaponList.Count;

            for (int i = 0; i < len; i++)
            {
                if (weaponList[i] != null)
                {
                    weaponList[i].GetComponent<Weapon>().targetSelected = true;
                    weaponList[i].GetComponent<Weapon>().enemyRoom = targetRoom;

                    targetRoom = targetRoomList[Random.Range(0, targetRoomList.Count - 1)];

                    if (targetRoom != null)
                    {
                        if (rotationDestination != null)
                        {
                            Destroy(rotationDestination);
                        }

                        rotationDestination = Instantiate(destinationPrefab, targetRoom.transform.position, Quaternion.identity);
                        rotationDestination.transform.SetParent(targetRoom.transform);
                        rotationDestination.layer = 2;
                        rotationCompleted = false;
                    }
                    
                }
            }
        }
    }

    void Update()
    {
        if (destinationReached == false)
        {
            MoveToDestination();
        }

        RotateTowardsTarget(torso);

        if (mechMode != Mode.Movement)
        {
            if (targetUnreachable == true && stamina.CurrentVal > stamina.MaxVal/2)
            {
                SelectDestination(0.1f, -transform.forward);
            }
        }

        if (mechMode != Mode.Movement && stamina.CurrentVal < 4000)
        {
            stamina.CurrentVal += 2;
        }

        if (health.CurrentVal <= 0 || pilotHealth.CurrentVal <= 0)
        {
            Destroy(UI);
            Destroy(gameObject);

            GameObject effectIns = Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(effectIns, 6f);
        }

        PowerEnemyRooms();
        PowerEnemyWeapons();

        //Heat
        ReduceHeat();
        HeatDOT();

        //Oxygen
        OxygenDOT();

    }
}
