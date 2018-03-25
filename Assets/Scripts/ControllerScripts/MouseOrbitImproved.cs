using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class MouseOrbitImproved : MonoBehaviour
{
    public GameObject gameMaster;
    public GameManager gameManager;

    public Transform target;
    public float distance = 5.0f;
    public Vector3 negDistance;

    public float xSpeed = 3f;
    public float ySpeed = 3f;
    public float defaultXSpeed;
    public float defaultYSpeed;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;

    private Rigidbody orbitRigidbody;

    float x = 0.0f;
    float y = 0.0f;

    public GameObject sunPrefab;
    public GameObject sun;
    public GameObject[] planetListTemp;
    private int len;
    public int currentPlanetIndex;

    public List<GameObject> planetList;

    void Start()
    {
        gameMaster = GameObject.Find("GameMaster");
        gameManager = gameMaster.GetComponent<GameManager>();

        sun = Instantiate(sunPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
        target = sun.transform;

        defaultXSpeed = xSpeed;
        defaultYSpeed = ySpeed;

        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        orbitRigidbody = GetComponent<Rigidbody>();

        // Make the rigid body not change rotation
        if (orbitRigidbody != null)
        {
            orbitRigidbody.freezeRotation = true;
        }
        
        if (string.IsNullOrEmpty(gameManager.targetPlanetName) == false)
        {
            gameManager.targetPlanet = GameObject.Find(gameManager.targetPlanetName).transform;
        }

        if (gameManager.targetPlanet != null)
        {
            target = gameManager.targetPlanet;
        }

        

        UpdateTargetRadius(target);

        distanceMin = 1000;
        distanceMax = 2000;
        distance = (distanceMin + distanceMax) / 2;
        xSpeed = defaultXSpeed / 15;
        ySpeed = defaultYSpeed / 15;

        planetListTemp = GameObject.FindGameObjectsWithTag("Planet");
        len = planetListTemp.Length;

        for (int i = 0; i < len; i++)
        {
            planetList.Add(planetListTemp[i]);
        }

        currentPlanetIndex = planetList.IndexOf(target.gameObject);
    }

    void LateUpdate()
    {
        if (target)
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButton(2))
            {
                x += Input.GetAxis("Mouse X") * xSpeed * (distance + 2000 / distance) * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * (distance + 2000 / distance) * 0.02f;
            }
            
            y = ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);

            if (target.name == "Sun")
            {
                distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 200, distanceMin, distanceMax);
            }
            else
            {
                distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 20, distanceMin, distanceMax);
            }

            Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;

            transform.rotation = rotation;
            transform.position = position;
        }
    }

    public float UpdateTargetRadius(Transform target)
    {
        float radius = target.GetComponent<SphereCollider>().radius * target.transform.localScale.x;
        distanceMin = radius + 3;
        distanceMax = distanceMin + 150;
        distance = (distanceMin + distanceMax) / 2 - (75 - radius);
        return radius;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
        {
            angle += 360F;
        }

        if (angle > 360F)
        {
            angle -= 360F;
        }

        return Mathf.Clamp(angle, min, max);
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F3))
        {
            Cursor.visible = false;
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            Cursor.visible = true;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentPlanetIndex < len-1)
            {
                currentPlanetIndex++;
                //gameMaster.GetComponent<GameManager>().currentPlanetIndex = currentPlanetIndex;
                target = planetList[currentPlanetIndex].transform;
                UpdateTargetRadius(target);
                xSpeed = defaultXSpeed;
                ySpeed = defaultYSpeed;

                gameMaster.GetComponent<GameManager>().targetPlanet = target;
                gameMaster.GetComponent<GameManager>().targetPlanetName = target.name;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentPlanetIndex > 0)
            {
                currentPlanetIndex--;
                target = planetList[currentPlanetIndex].transform;
                UpdateTargetRadius(target);
                xSpeed = defaultXSpeed;
                ySpeed = defaultYSpeed;

                gameMaster.GetComponent<GameManager>().targetPlanet = target;
                gameMaster.GetComponent<GameManager>().targetPlanetName = target.name;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            target = sun.transform;
            distanceMin = 1000;
            distanceMax = 2000;
            distance = (distanceMin + distanceMax) / 2;
            xSpeed = defaultXSpeed / 15;
            ySpeed = defaultYSpeed / 15;
        }

    }
    
}
