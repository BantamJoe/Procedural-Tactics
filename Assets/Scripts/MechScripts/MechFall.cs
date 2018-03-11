using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MechFall : MonoBehaviour {

    #region Variables
    public GameObject dropPod;
    public GameObject dropPodPrefab;
    public GameObject destination;
    public GameObject destinationPrefab;
    public GameObject playerMech;

    public float delay = 5f;
    public bool readyForMechFall;
    #endregion

    void Start() 
	{
        playerMech = GameObject.Find("PlayerMech(Clone)");
    }

    public void SelectFallLocation()
    {
        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);

        if (hit)
        {
            destination = Instantiate(destinationPrefab, hitInfo.point, Quaternion.identity);
            Destroy(destination, delay);
            dropPod = Instantiate(dropPodPrefab, hitInfo.point + new Vector3(0, 100f, 0), Quaternion.identity);
            //playerMech.transform.position = dropPod.transform.position;
        }
    }


    void Update() 
	{
        if (readyForMechFall)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    SelectFallLocation();
                    readyForMechFall = false;
                }
            }
        }
    }
}
