using UnityEngine;
using System.Collections.Generic;

public class DropPod : MonoBehaviour {

    #region Variables
    public GameObject playerMech;
    public GameObject playerUI;
    public GameObject gameMaster;
    public float effectDelay = 6f;
    public float speed = 0.5f;
	#endregion
	
	void Start() 
	{
        gameMaster = GameObject.Find("GameMaster");
        playerMech = gameMaster.GetComponent<GameManager>().playerMech;
        playerUI = gameMaster.GetComponent<GameManager>().playerUI;
    }

    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject != null && gameObject != null)
        {
            if (col.gameObject.tag == "Breakable")
            {
                col.gameObject.GetComponent<Breakable>().health = 0;
            }

            if (col.gameObject.tag == "Environment")
            {
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

                Destroy(GetComponent<Collider>());
                Destroy(GetComponent<Rigidbody>());
                transform.localScale = new Vector3(0, 0, 0);
                Destroy(gameObject, effectDelay);
                transform.Find("WhiteSmoke").gameObject.GetComponent<ParticleScript>().StopSmoke();

                playerMech.SetActive(true);
                playerMech.transform.position = col.contacts[0].point + new Vector3(0, 4.9f, 0);

                playerUI.transform.Find("WeaponsUI").gameObject.SetActive(true);
            }
        }

    }

    void Update() 
	{
        transform.Translate(0, -speed, 0, Space.World);
    }
}
