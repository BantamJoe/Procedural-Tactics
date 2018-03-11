using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactEffect : MonoBehaviour {

	
	void Start()
    {
		
	}
	
	
	void Update() 
    {
        if (gameObject.GetComponent<Light>() != null)
        {
            gameObject.GetComponent<Light>().intensity -= 0.02f;
        }
    }
}
