using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

    public GameObject gameMaster;
    public GameObject playerUI;
    public GameObject playerMech;
    public GameObject player;
    public Player p;

    public GameObject[] enemyUIToDelete;
    public GameObject mainCameraObject;
    public Camera mainCamera;

    

    void Start()
    {
        gameMaster = GameObject.Find("GameMaster");
        playerUI = GameObject.Find("PlayerCanvas");
        playerMech = GameObject.Find("PlayerMech(Clone)");
        player = GameObject.Find("Player");
        p = player.GetComponent<Player>();

        mainCameraObject = GameObject.Find("Main Camera");
        mainCamera = mainCameraObject.GetComponent<Camera>();
    }


    void Update() 
    {
	
	}

    public void ChangeToSolarSystem()
    {
        enemyUIToDelete = GameObject.FindGameObjectsWithTag("Enemy");
        int len = enemyUIToDelete.Length;

        for (int i = 0; i < len; i++)
        {
            Destroy(enemyUIToDelete[i].GetComponent<EnemyMech>().UI);
        }

        if (playerMech)
        {
            playerMech.SetActive(false);
        }

        playerUI.SetActive(false);
        SceneManager.LoadScene(3);

        if (p.sun != null)
        {
            p.sun.SetActive(true);

            foreach (GameObject planet in p.sun.GetComponent<Sun>().planetList)
            {
                planet.SetActive(true);
            }
        }
        
        mainCamera.gameObject.SetActive(false);
        gameMaster.GetComponent<MechFall>().readyForMechFall = false;
    }

    public void restartGame()
    {
        DeleteAll();
        SceneManager.LoadScene(0);
    }

    public void DeleteAll()
    {
        foreach (GameObject i in FindObjectsOfType<GameObject>())
        {
              Destroy(i);
        }
    }
}
