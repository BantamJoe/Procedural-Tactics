using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour {

    public GameObject enemy1;
    public GameObject enemy2;

    public GameObject enemyMechPrefab1;
    public GameObject enemyMechPrefab2;

    public GameObject enemyMechUIPrefab1;
    public GameObject enemyMechUIPrefab2;

    public List<GameObject> enemyList;
    public List<GameObject> enemyPrefabList;

    public bool allEnemiesDead;
    public bool lootGiven;
    public int temp;
    public GameObject playerMech;
    public GameObject newIcon;
    public GameObject newWeapon;

    public GameObject playerUI;

    public Transform spawnPoint1;
    public Transform spawnPoint2;


    void Awake()
    {
        playerUI = GameObject.Find("PlayerCanvas");
    }


    void Start()
    {
        enemyPrefabList.Add(enemyMechPrefab1);
        enemyPrefabList.Add(enemyMechPrefab2);

        allEnemiesDead = false;
        lootGiven = false;

        temp = Random.Range(0, 10);

        if (temp >= 2)
        {
            enemy1 = Instantiate(enemyPrefabList[Random.Range(0, enemyPrefabList.Count)], spawnPoint1.position, spawnPoint1.rotation) as GameObject;
            enemyList.Add(enemy1);
        }

        if (GameManager.sectorNumber > 2)
        {
            temp = Random.Range(0, 2);

            if (temp == 1)
            {
                enemy2 = Instantiate(enemyPrefabList[Random.Range(0, enemyPrefabList.Count)], spawnPoint2.position, spawnPoint2.rotation) as GameObject;
                enemyList.Add(enemy2);
            }
        }
    }
	
	
	void Update() 
    {

        if (Input.GetKeyDown(KeyCode.O))
        {
            Instantiate(enemyMechPrefab1, new Vector3(Random.Range(-200, 200), 4.3f, Random.Range(-200, 200)), Quaternion.identity);
        }

        int len = enemyList.Count;
        bool temp2 = true;

        for (int i = 0; i < len; i++)
        {
            if (enemyList[i] != null)
            {
                temp2 = false;
            }
        }

        allEnemiesDead = temp2;

        if (allEnemiesDead == true)
        {
            
            if (lootGiven == false)
            {
                lootGiven = true;

                playerUI.transform.Find("EvacuateButton").gameObject.SetActive(true);
                playerUI.transform.Find("InventoryLoot").gameObject.SetActive(true);
                playerUI.transform.Find("TakeLootButton").gameObject.SetActive(true);

                //Weapon loot
                temp = Random.Range(1, GameManager.weaponList.Count + 1);

                switch (temp)
                {
                    case 1:
                        newWeapon = Instantiate(Resources.Load("AC1")) as GameObject;
                        newIcon = Instantiate(Resources.Load("AC1 Icon") as GameObject);
                        
                        break;
                    case 2:
                        newWeapon = Instantiate(Resources.Load("Burst Laser I")) as GameObject;
                        newIcon = Instantiate(Resources.Load("Burst Laser I Icon") as GameObject);
                        
                        break;
                    case 3:
                        newWeapon = Instantiate(Resources.Load("Large Laser I")) as GameObject;
                        newIcon = Instantiate(Resources.Load("Large Laser I Icon") as GameObject);
                        
                        break;
                    case 4:
                        newWeapon = Instantiate(Resources.Load("Medium Laser I")) as GameObject;
                        newIcon = Instantiate(Resources.Load("Medium Laser I Icon") as GameObject);
                        
                        break;
                    case 5:
                        newWeapon = Instantiate(Resources.Load("Small Laser I")) as GameObject;
                        newIcon = Instantiate(Resources.Load("Small Laser I Icon") as GameObject);
                        
                        break;
                    case 6:
                        newWeapon = Instantiate(Resources.Load("Missile Launcher")) as GameObject;
                        newIcon = Instantiate(Resources.Load("Missile Launcher Icon") as GameObject);
                        
                        break;
                    case 7:
                        newWeapon = Instantiate(Resources.Load("ICBM")) as GameObject;
                        newIcon = Instantiate(Resources.Load("ICBM Icon") as GameObject);

                        break;
                    case 8:
                        newWeapon = Instantiate(Resources.Load("Burst Laser II")) as GameObject;
                        newIcon = Instantiate(Resources.Load("Burst Laser II Icon") as GameObject);

                        break;

                    default:

                        break;

                }

                newIcon.transform.SetParent(playerUI.transform.Find("InventoryLoot"), false);
                newIcon.GetComponent<WeaponSelection>().attachedWeapon = newWeapon;
                newIcon.GetComponent<WeaponSelection>().weapon = newWeapon.GetComponent<Weapon>();
                newIcon.GetComponent<WeaponIcon>().weapon = newWeapon.GetComponent<Weapon>();
                newWeapon.SetActive(false);

            }
        }
    }
}
