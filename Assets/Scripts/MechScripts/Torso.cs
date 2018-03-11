using UnityEngine;

public class Torso : MonoBehaviour
{
    #region Variables
    #endregion

    public Transform engineRoom;

    void Start()
    {
        engineRoom = transform.root.Find("EngineRoom");
    }

    public void OnTriggerStay(Collider col)
    {
        if (col.gameObject != null && gameObject != null)
        {
            if (col.gameObject.tag == "Player" || col.gameObject.tag == "Enemy")
            {
                float step = 0.05f + transform.root.GetComponent<Mech>().currentSpeed / 100;
                transform.root.GetComponent<Mech>().transform.position = Vector3.MoveTowards(Vector3.ProjectOnPlane(transform.position, Vector3.up) + new Vector3(0, transform.position.y, 0), Vector3.ProjectOnPlane(col.gameObject.transform.position, Vector3.up) + new Vector3(0, transform.position.y, 0), -step);
            }
        }
    }

    void Update()
    {

    }
}
