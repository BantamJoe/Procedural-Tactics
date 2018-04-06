using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PlayerMech : Mech {

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        UI = GameObject.Find("PlayerCanvas");

        health.bar = UI.transform.Find("HealthBar").gameObject.GetComponent<Bar>();
        heat.bar = UI.transform.Find("HeatBar").gameObject.GetComponent<Bar>();
        oxygen.bar = UI.transform.Find("OxygenBar").gameObject.GetComponent<Bar>();
        stamina.bar = UI.transform.Find("StaminaBar").gameObject.GetComponent<Bar>();
        pilotHealth.bar = UI.transform.Find("PilotHealthBar").gameObject.GetComponent<Bar>();

        health.Initialize();
        heat.Initialize();
        oxygen.Initialize();
        stamina.Initialize();
        pilotHealth.Initialize();
    }
    
    void Start()
    {
        mechMode = Mode.Neutral;
        
        health.MaxVal = 20;
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

        maxRotationSpeed = 40f;
        currentRotationSpeed = 0f;
        rotationAcceleration = 0.2f;

        destinationReached = true;
        rotationCompleted = true;

        halfRotationAngle = 110f;

        torso = transform.Find("Torso").gameObject;
        shield = transform.Find("Shield").gameObject;

        shieldRoom = transform.Find("Torso").Find("ShieldRoom").gameObject;
        weaponRoom = transform.Find("Torso").Find("WeaponRoom").gameObject;
        engineRoom = transform.Find("EngineRoom").gameObject;

        SetRoomHealths();

        weaponSlotCount = 6;

        //weapon1 = Instantiate(Resources.Load("Burst Laser II") as GameObject, transform.Find("Torso").Find("WeaponSlotParent").Find("WeaponSlot 1").position, transform.Find("Torso").Find("WeaponSlotParent").Find("WeaponSlot 1").rotation) as GameObject;
        //UI.transform.Find("WeaponsUI/Burst Laser II Icon").gameObject.GetComponent<WeaponSelection>().attachedWeapon = weapon1;
        //UI.transform.Find("WeaponsUI/Burst Laser II Icon").gameObject.GetComponent<WeaponIcon>().weapon = weapon1.GetComponent<Weapon>();
        //weapon1.transform.SetParent(transform.Find("Torso").Find("WeaponSlotParent").Find("WeaponSlot 1"));
        //weapon1.GetComponent<Weapon>().icon = UI.transform.Find("WeaponsUI/Burst Laser II Icon").gameObject;

        //weapon2 = Instantiate(Resources.Load("Medium Laser I") as GameObject, transform.Find("Torso").Find("WeaponSlotParent").Find("WeaponSlot 2").position, transform.Find("Torso").Find("WeaponSlotParent").Find("WeaponSlot 2").rotation) as GameObject;
        //UI.transform.Find("WeaponsUI/Medium Laser I Icon").gameObject.GetComponent<WeaponSelection>().attachedWeapon = weapon2;
        //UI.transform.Find("WeaponsUI/Medium Laser I Icon").gameObject.GetComponent<WeaponIcon>().weapon = weapon2.GetComponent<Weapon>();
        //weapon2.transform.SetParent(transform.Find("Torso").Find("WeaponSlotParent").Find("WeaponSlot 2"));
        //weapon2.GetComponent<Weapon>().icon = UI.transform.Find("WeaponsUI/Medium Laser I Icon").gameObject;

        //weapon3 = Instantiate(Resources.Load("AC1") as GameObject, transform.Find("Torso").Find("WeaponSlotParent").Find("WeaponSlot 3").position, transform.Find("Torso").Find("WeaponSlotParent").Find("WeaponSlot 3").rotation) as GameObject;
        //UI.transform.Find("WeaponsUI/AC1 Icon").gameObject.GetComponent<WeaponSelection>().attachedWeapon = weapon3;
        //UI.transform.Find("WeaponsUI/AC1 Icon").gameObject.GetComponent<WeaponIcon>().weapon = weapon3.GetComponent<Weapon>();
        //weapon3.transform.SetParent(transform.Find("Torso").Find("WeaponSlotParent").Find("WeaponSlot 3"));
        //weapon3.GetComponent<Weapon>().icon = UI.transform.Find("WeaponsUI/AC1 Icon").gameObject;

        //weapon4 = Instantiate(Resources.Load("Burst Laser I") as GameObject, transform.Find("Torso").Find("WeaponSlotParent").Find("WeaponSlot 4").position, transform.Find("Torso").Find("WeaponSlotParent").Find("WeaponSlot 4").rotation) as GameObject;
        //UI.transform.Find("WeaponsUI/Burst Laser I Icon").gameObject.GetComponent<WeaponSelection>().attachedWeapon = weapon4;
        //UI.transform.Find("WeaponsUI/Burst Laser I Icon").gameObject.GetComponent<WeaponIcon>().weapon = weapon4.GetComponent<Weapon>();
        //weapon4.transform.SetParent(transform.Find("Torso").Find("WeaponSlotParent").Find("WeaponSlot 4"));
        //weapon4.GetComponent<Weapon>().icon = UI.transform.Find("WeaponsUI/Burst Laser I Icon").gameObject;

        //weapon5 = Instantiate(Resources.Load("Missile Launcher") as GameObject, transform.Find("Torso").Find("WeaponSlotParent").Find("WeaponSlot 5").position, transform.Find("Torso").Find("WeaponSlotParent").Find("WeaponSlot 5").rotation) as GameObject;
        //UI.transform.Find("WeaponsUI/Missile Launcher Icon").gameObject.GetComponent<WeaponSelection>().attachedWeapon = weapon5;
        //UI.transform.Find("WeaponsUI/Missile Launcher Icon").gameObject.GetComponent<WeaponIcon>().weapon = weapon5.GetComponent<Weapon>();
        //weapon5.transform.SetParent(transform.Find("Torso").Find("WeaponSlotParent").Find("WeaponSlot 5"));
        //weapon5.GetComponent<Weapon>().icon = UI.transform.Find("WeaponsUI/Missile Launcher Icon").gameObject;

        weapon6 = Instantiate(Resources.Load("Rocket Launcher") as GameObject, transform.Find("Torso").Find("WeaponSlotParent").Find("WeaponSlot 6").position, transform.Find("Torso").Find("WeaponSlotParent").Find("WeaponSlot 6").rotation) as GameObject;
        UI.transform.Find("WeaponsUI/Rocket Launcher Icon").gameObject.GetComponent<WeaponSelection>().attachedWeapon = weapon6;
        UI.transform.Find("WeaponsUI/Rocket Launcher Icon").gameObject.GetComponent<WeaponIcon>().weapon = weapon6.GetComponent<Weapon>();
        weapon6.transform.SetParent(transform.Find("Torso").Find("WeaponSlotParent").Find("WeaponSlot 6"));
        weapon6.GetComponent<Weapon>().icon = UI.transform.Find("WeaponsUI/Rocket Launcher Icon").gameObject;

        weapon3 = Instantiate(Resources.Load("Rocket Launcher") as GameObject, transform.Find("Torso").Find("WeaponSlotParent").Find("WeaponSlot 3").position, transform.Find("Torso").Find("WeaponSlotParent").Find("WeaponSlot 3").rotation) as GameObject;
        UI.transform.Find("WeaponsUI/Rocket Launcher Icon (1)").gameObject.GetComponent<WeaponSelection>().attachedWeapon = weapon3;
        UI.transform.Find("WeaponsUI/Rocket Launcher Icon (1)").gameObject.GetComponent<WeaponIcon>().weapon = weapon3.GetComponent<Weapon>();
        weapon3.transform.SetParent(transform.Find("Torso").Find("WeaponSlotParent").Find("WeaponSlot 3"));
        weapon3.GetComponent<Weapon>().icon = UI.transform.Find("WeaponsUI/Rocket Launcher Icon (1)").gameObject;

        //weaponList.Add(weapon1);
        //weaponList.Add(weapon2);
        weaponList.Add(weapon3);
        //weaponList.Add(weapon4);
        //weaponList.Add(weapon5);
        weaponList.Add(weapon6);
        equippedWeaponCount = weaponList.Count;
    }


    public void SelectDestination()
    {
        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);

        if (hit)
        {
            if (destination != null)
            {
                Destroy(destination);
            }

            destination = Instantiate(destinationPrefab, hitInfo.point, Quaternion.identity);
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

    public void SelectRotation()
    {
        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);

        if (hit)
        {
            if (rotationDestination != null)
            {
                Destroy(rotationDestination);
            }

            rotationDestination = Instantiate(destinationPrefab, hitInfo.point, Quaternion.identity);
            rotationDestination.layer = 2;
            rotationCompleted = false;
        }
    }

    void Update() 
    {
        if (destinationReached == false)
        {
            MoveToDestination();
        }

        RotateTowardsTarget(torso);

        DepowerWeapons();

        //Heat
        ReduceHeat();
        HeatDOT();


        //Oxygen
        OxygenDOT();

        if (mechMode != Mode.Movement && stamina.CurrentVal < 4000)
        {
            stamina.CurrentVal += 2;
        }

        if ((health.CurrentVal <= 0 || pilotHealth.CurrentVal <= 0) && gameOver == false)
        {
            gameOver = true;
            isDead = true;
            UI.transform.Find("RestartButton").gameObject.SetActive(true);
            Destroy(gameObject, 2.1f);

            CreateDeathExplosion();
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(0, 0, 1, Space.World);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(0, 0, -1, Space.World);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(-1, 0, 0, Space.World);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(1, 0, 0, Space.World);
        }

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (mechMode != Mode.Movement && engineRoom.GetComponent<Room>().ener.CurrentVal > 0)
                {
                    SelectDestination();
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (selectedWeapon != null)
                {
                    selectedWeapon.GetComponent<Weapon>().weaponSelected = false;
                    selectedWeapon = null;
                }
            }
        }

        if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.T))
        {
            shieldRoom.GetComponent<Room>().IncreaseEnergy(1);
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.T))
        {
            shieldRoom.GetComponent<Room>().DecreaseEnergy();
            shield.GetComponent<Shield>().regenAmount = 0;
        }

        if (Input.GetKey(KeyCode.H))
        {
            health.CurrentVal = health.MaxVal;
        }

        if (Input.GetKey(KeyCode.J))
        {
            foreach (GameObject room in roomList)
            {
                room.GetComponent<Room>().health.CurrentVal = room.GetComponent<Room>().health.MaxVal;
            }
        }

        if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Mouse2))
        {
            SelectRotation();
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Mouse2))
        {
            TargetGround();
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (destination != null)
            {
                destinationReached = true;
                Destroy(destination);
                mechMode = previousMechMode;
                currentSpeed = 0;
                stamina.CurrentVal = -600;
            }
        }
    }
}
