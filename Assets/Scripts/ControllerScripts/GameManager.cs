using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public GameObject playerMechPrefab;
    public Transform playerSpawnPoint;

    public GameObject playerMech;
    public GameObject playerUI;

    public static int sectorNumber = 1;
    public static int sectorDifficulty = 1;
    public static int evacuationCount = 0;

    public Transform targetPlanet;
    public string targetPlanetName;
    public bool solarSystemCreated;

    public static List<GameObject> weaponList;
    public GameObject smallLaserI;
    public GameObject mediumLaserI;
    public GameObject burstLaserI;
    public GameObject burstLaserII;
    public GameObject AC1;
    public GameObject rocketLauncher;
    public GameObject missileLauncher;
    public GameObject ICBM;


    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        instance = this;

        sectorNumber = 1;
        sectorDifficulty = 1;
        evacuationCount = 0;

        smallLaserI = Resources.Load("Small Laser I") as GameObject;
        mediumLaserI = Resources.Load("Medium Laser I") as GameObject;
        burstLaserI = Resources.Load("Burst Laser I") as GameObject;
        burstLaserII = Resources.Load("Burst Laser II") as GameObject;
        AC1 = Resources.Load("AC1") as GameObject;
        rocketLauncher = Resources.Load("Rocket Launcher") as GameObject;
        missileLauncher = Resources.Load("Missile Launcher") as GameObject;
        ICBM = Resources.Load("ICBM") as GameObject;

        weaponList = new List<GameObject> {
            smallLaserI,
            mediumLaserI,
            burstLaserI,
            burstLaserII,
            AC1,
            rocketLauncher,
            missileLauncher,
            ICBM
        };
    }
    
    void Start()
    {
       playerMech = Instantiate(playerMechPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation);
       playerUI = GameObject.Find("PlayerCanvas");
    }

    public void IncreaseEvacuationCount()
    {
        evacuationCount++;

        if (evacuationCount >= 10)
        {
            sectorNumber++;
            evacuationCount = 0;
        }
    }
	
	void Update() 
    {

    }

    //public class SomeClass : MonoBehaviour
    //{

    //    private GameObject _obj = null;
    //    private string _objName;

    //    public GameObject Obj
    //    {
    //        get
    //        {
    //            if (_obj == null)
    //            {
    //                _obj = GameObject.Find(_objName);
    //            }
    //            return _obj;
    //        }
    //        set
    //        {
    //            _obj = value;
    //            _objName = value.name;
    //        }
    //    }
    //}
}
