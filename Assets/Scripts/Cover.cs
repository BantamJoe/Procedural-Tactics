using UnityEngine;

public class Cover : Breakable {

    #region Variables
	#endregion
	
	void Start() 
	{
        health = 5;
	}

    void Update() 
	{
		if (health <= 0)
        {
            Destroy(gameObject);
            GameObject effectIns = Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(effectIns, 6f);
        }
	}
}
