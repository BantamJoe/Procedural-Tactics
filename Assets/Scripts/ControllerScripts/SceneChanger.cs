using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

    public GameObject gameMaster;
    public GameObject playerUI;
    public GameObject playerMech;

    public GameObject[] enemyUIToDelete;
    public GameObject mainCameraObject;
    public Camera mainCamera;

    void Start()
    {
        gameMaster = GameObject.Find("GameMaster");
        playerUI = GameObject.Find("PlayerCanvas");
        playerMech = GameObject.Find("PlayerMech(Clone)");

        mainCameraObject = GameObject.Find("Main Camera");
        mainCamera = mainCameraObject.GetComponent<Camera>();
    }


    void Update() 
    {
	
	}

    public void ChangeToRandomScene()
    {
        enemyUIToDelete = GameObject.FindGameObjectsWithTag("Enemy");
        int len = enemyUIToDelete.Length;

        for (int i = 0; i < len; i++)
        {
            Destroy(enemyUIToDelete[i].GetComponent<EnemyMech>().UI);
        }

        SceneManager.LoadScene(Random.Range(1, 3));
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
