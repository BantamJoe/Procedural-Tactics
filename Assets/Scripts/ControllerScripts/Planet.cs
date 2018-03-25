using UnityEngine;

public class Planet : MonoBehaviour {

    #region Variables
    public Transform orbitingTarget;

    public float rotationSpeed = 1f;

    [Range(0.1f,10f)]
    public float orbitSpeed = 1f;
    #endregion

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    void Start () 
	{
		
	}
	
	void Update () 
	{
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        
        if (orbitingTarget == null)
        {
            transform.RotateAround(Vector3.zero, Vector3.up, orbitSpeed * Time.deltaTime);
        }
        else
        {
            transform.RotateAround(orbitingTarget.position, orbitingTarget.up, orbitSpeed * Time.deltaTime);
        }
    }
}
