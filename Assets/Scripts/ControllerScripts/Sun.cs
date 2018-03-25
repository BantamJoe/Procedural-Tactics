using UnityEngine;
using System.Collections.Generic;

public class Sun : MonoBehaviour {

    #region Variables
    public List<GameObject> planetList;
    public List<GameObject> planetPrefabList;
    #endregion

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    void Start() 
	{
        GenerateSolarSystem(Random.Range(5, 21));
	}

    public void GenerateSolarSystem(int planetCount)
    {
        for (int i = 0; i < planetCount; i++)
        {
            Instantiate(planetPrefabList[Random.Range(0, planetPrefabList.Count)], new Vector3(Random.Range(-1000, 1000), 0, Random.Range(-1000, 1000)), Quaternion.identity);
        }
    }

    void Update() 
	{
		
	}
}
