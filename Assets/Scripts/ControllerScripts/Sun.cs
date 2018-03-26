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
        GenerateSolarSystem(Random.Range(8, 11));
	}

    public void GenerateSolarSystem(int planetCount)
    {
        for (int i = 0; i < planetCount; i++)
        {
            planetList.Add(Instantiate(planetPrefabList[Random.Range(0, planetPrefabList.Count)], new Vector3(i * 145 + 240, 0, 0), Quaternion.identity));
        }
    }

    void Update() 
	{
		
	}
}
