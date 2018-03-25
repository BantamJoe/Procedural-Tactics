using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour {

    public static int resources ;
    public int startRes;

    public GameObject gameMaster;
    public GameObject selectedEnemy;
    public GameObject enemyUI;

    private GameObject temp;
    private Transform parentSlot;

    public GameObject playerMech;
    public GameObject randomWeapon;
    public GameObject newWeapon;
    public GameObject newIcon;
    public GameObject playerUI;
    public GameObject playerSpawn;
    private Vector3 spawnPosition;
    private Quaternion spawnRotation;

    public GameObject mainCameraObject;
    public Camera mainCamera;
    public string previousTag;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
	
    void Start()
    {
        gameMaster = GameObject.Find("GameMaster");
        playerUI = GameObject.Find("PlayerCanvas");
        playerMech = GameObject.Find("PlayerMech(Clone)");
        playerSpawn = GameObject.Find("PlayerSpawn");
        spawnPosition = playerSpawn.transform.position;
        spawnRotation = playerSpawn.transform.rotation;

        resources = startRes;
        mainCameraObject = GameObject.Find("Main Camera");
        mainCamera = mainCameraObject.GetComponent<Camera>();
    }

    public void Select()
    {
        bool hit;
        RaycastHit hitInfo = new RaycastHit();
        //TEMPORARY
        if (mainCameraObject.activeSelf == true)
        {
            hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
        }
        else
        {
            hit = Physics.Raycast(Camera.allCameras[0].ScreenPointToRay(Input.mousePosition), out hitInfo);
        }

        if (hit)
        {
            //print(hitInfo.transform.gameObject.name);
            if (hitInfo.transform.gameObject.tag == "Enemy" || hitInfo.transform.gameObject.tag == "Enemy Shield")
            {
                if (selectedEnemy != null)
                {
                    temp = selectedEnemy.GetComponent<EnemyMech>().UI;
                }
                if (hitInfo.transform.gameObject.tag == "Enemy")
                {
                    selectedEnemy = hitInfo.transform.gameObject;
                }
                else
                {
                    selectedEnemy = hitInfo.transform.parent.gameObject;
                }

                enemyUI = selectedEnemy.GetComponent<EnemyMech>().UI;


                if (temp != null)
                {
                    if (temp != enemyUI)
                    {
                        temp.SetActive(false);
                    }
                }

                enemyUI.SetActive(true);
            }
            else if (hitInfo.transform.gameObject.tag == "Zone")
            {
                if (hitInfo.transform.parent.name == "Dirt(Clone)")
                {
                    SceneManager.LoadScene("DirtZone");
                }

                if (hitInfo.transform.parent.name == "Moon(Clone)")
                {
                    SceneManager.LoadScene("MoonZone");
                }

                if (hitInfo.transform.parent.name == "Concrete(Clone)")
                {
                    SceneManager.LoadScene("ConcreteZone");
                }

                if (hitInfo.transform.parent.name == "Asphalt(Clone)")
                {
                    SceneManager.LoadScene("AsphaltZone");
                }

                if (hitInfo.transform.parent.name == "Cliff(Clone)")
                {
                    SceneManager.LoadScene("CliffZone");
                }

                if (hitInfo.transform.parent.name == "Gravel(Clone)")
                {
                    SceneManager.LoadScene("GravelZone");
                }

                Camera.allCameras[0].gameObject.SetActive(false);

                playerUI.SetActive(true);
                playerUI.transform.Find("WeaponsUI").gameObject.SetActive(false);

                mainCamera.gameObject.SetActive(true);
                gameMaster.GetComponent<MechFall>().readyForMechFall = true;

                playerMech.transform.position = spawnPosition;
                playerMech.transform.rotation = spawnRotation;

            }
            else if (hitInfo.transform.gameObject.tag == "Sun")
            {
                MouseOrbitImproved m = Camera.allCameras[0].gameObject.GetComponent<MouseOrbitImproved>();
                m.target = hitInfo.transform;
                m.distanceMin = 1000;
                m.distanceMax = 2000;
                m.distance = (m.distanceMin + m.distanceMax) / 2;
                m.xSpeed = m.defaultXSpeed / 15;
                m.ySpeed = m.defaultYSpeed / 15;
                previousTag = hitInfo.transform.gameObject.tag;
            }
            else if (hitInfo.transform.gameObject.tag == "Planet" || hitInfo.transform.gameObject.tag == "Moon")
            {
                MouseOrbitImproved m = Camera.allCameras[0].gameObject.GetComponent<MouseOrbitImproved>();
                m.target = hitInfo.transform;
                gameMaster.GetComponent<GameManager>().targetPlanet = hitInfo.transform;
                gameMaster.GetComponent<GameManager>().targetPlanetName = hitInfo.transform.name;
                m.currentPlanetIndex = m.planetList.IndexOf(m.target.gameObject);

                m.UpdateTargetRadius(hitInfo.transform);
                m.xSpeed = m.defaultXSpeed;
                m.ySpeed = m.defaultYSpeed;
                previousTag = hitInfo.transform.gameObject.tag;
            }
            else
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    if (enemyUI != null)
                    {
                        enemyUI.SetActive(false);
                    }
                }
            }
        }
    }

    void Update()
    {
        //INPUT

        //Mouse
        if (Input.GetKeyDown(KeyCode.F3))
        {
            Cursor.visible = false;
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            Cursor.visible = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Select();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            GameManager.sectorNumber++;
        }


        //Pausing
        if (Input.GetKeyDown("space"))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }


        if (Input.GetKey(KeyCode.M))
        {
            Player.resources += 1000;
        }

    }
}
