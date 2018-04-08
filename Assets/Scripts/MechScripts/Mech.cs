using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mech : MonoBehaviour {

    public int roomCount;

    [Header("Stats")]

    public Stat health;
    public Stat heat;
    public Stat oxygen;
    public Stat stamina;
    public Stat pilotHealth;

    [Header("Objects")]

    public GameObject selectedEnemy;
    public GameObject selectedWeapon;
    public GameObject UI;

    public GameObject torso;

    public List<GameObject> roomList;
    public GameObject weaponRoom;
    public GameObject shieldRoom;
    public GameObject engineRoom;
    public GameObject generatorRoom;

    public GameObject targetRoom;
    public List<GameObject> targetRoomList;

    public GameObject shield;

    public List<GameObject> weaponList;

    public GameObject weapon;

    public GameObject weapon1;
    public GameObject weapon2;
    public GameObject weapon3;
    public GameObject weapon4;
    public GameObject weapon5;
    public GameObject weapon6;

    public GameObject enemyMechUIPrefab;

    public GameObject destination;
    public GameObject rotationDestination;
    public GameObject destinationPrefab;

    public GameObject groundTarget;

    public GameObject impactEffect;


    [Header("Attributes")]

    public float coolRate = 2f;
    public float coolCountdown;
    public int heatDamage = 1;
    public float heatDamageRate = 3f;
    public float nextHeatDamageTime = 0f;

    public int oxygenDamage = 2;
    public float oxygenDamageRate = 1f;
    public float nextoxygenDamageTime = 0f;

    public float maxSpeed;
    public float currentSpeed;
    public float acceleration;
    public float fullAcceleration;

    public float maxRotationSpeed;
    public float currentRotationSpeed;
    public float rotationAcceleration;
    public float fullRotationAcceleration;
    public float halfRotationAngle;
    public float previousTorsoRotation;

    public bool destinationReached;
    public bool rotationCompleted;
    public bool targetUnreachable;

    public float rotationY;
    public float rotationUnclamped;

    float angleAlpha;
    float angleBeta;
    float angleGamma;
    float step;

    public bool rotatingTowardsFront;

    public int shieldLevel;

    [Header("Variables")]

    public int equippedWeaponCount;
    public int weaponSlotCount;
    public int totalWeaponEnergyReq;

    public bool isDead;
    public bool gameOver;

    public enum Mode
    {
        Neutral,
        Offense,
        Defense,
        Movement,
        Repair,
        ShutDown
    }

    public Mode mechMode;
    public Mode previousMechMode;

    void Awake()
    {

    }

    public void DoAction(Mode mechMode)
    {
        switch (mechMode)
        {
            case Mode.Neutral:
                
                break;
            case Mode.Offense:
                
                break;
            case Mode.Defense:
                
                break;
            case Mode.Movement:
                
                break;
            case Mode.Repair:

                break;
            case Mode.ShutDown:

                break;
        }
    }

    public void ReduceHeat()
    {
        if (coolCountdown <= 0f && heat.CurrentVal > 0)
        {
            heat.CurrentVal -= 1;
            coolCountdown = 1f / coolRate;
        }

        if (coolCountdown > 0)
        {
            coolCountdown -= Time.deltaTime;
        }
    }

    public void HeatDOT()
    {
        if (heat.CurrentVal >= 100 && Time.time > nextHeatDamageTime)
        {
            nextHeatDamageTime = Time.time + heatDamageRate;
            health.CurrentVal -= heatDamage;
        }
    }

    public void OxygenDOT()
    {
        if (oxygen.CurrentVal <= 0 && Time.time > nextoxygenDamageTime)
        {
            nextoxygenDamageTime = Time.time + oxygenDamageRate;
            pilotHealth.CurrentVal -= oxygenDamage;
        }
    }

    public List<GameObject> GetRooms()
    {
        return roomList;
    }

    public virtual void SetRoomHealths()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            Room r = roomList[i].GetComponent<Room>();

            r.FindBars();

            r.health.Initialize();
            r.ener.Initialize();

            if (r.name == "CockpitRoom" || r.name == "OxygenRoom" || r.name == "ShieldRoom")
            {
                r.health.MaxVal = GameManager.sectorNumber;
            }
            else
            {
                r.health.MaxVal = GameManager.sectorNumber + 1;
            }

            r.ener.MaxVal = r.health.MaxVal;
            r.health.CurrentVal = r.health.MaxVal;
            r.ener.CurrentVal = r.ener.MaxVal;
            r.level = r.health.MaxVal;
        }

        weaponRoom = transform.Find("Torso").Find("WeaponRoom").gameObject;
        Room wr = weaponRoom.GetComponent<Room>();
        wr.health.MaxVal = GameManager.sectorNumber + 2;
        wr.health.CurrentVal = wr.health.MaxVal;
        wr.ener.MaxVal = wr.health.MaxVal;
        wr.ener.CurrentVal = 0;
        wr.level = wr.health.MaxVal;
    }

    public virtual void MoveToDestination()
    {
        if (destination == null || engineRoom.GetComponent<Room>().ener.CurrentVal <= 0 || stamina.CurrentVal <= 0)
        {
            destinationReached = true;
            Destroy(destination);

            if (previousMechMode != Mode.Movement)
            {
                mechMode = previousMechMode;
                previousMechMode = mechMode;
            }
            else
            {
                previousMechMode = Mode.Neutral;
                mechMode = previousMechMode;
            }

            currentSpeed = 0;

            if (stamina.CurrentVal <= 0 && stamina.CurrentVal > -2)
            {
                stamina.CurrentVal -= 600;
            }

            return;
        }
        else
        {
            float distanceThisFrame = currentSpeed * Time.deltaTime;
            Vector3 dir = destination.transform.position - transform.position;

            if (Vector3.ProjectOnPlane(dir, Vector3.up).magnitude <= distanceThisFrame)
            {
                destinationReached = true;
                Destroy(destination);
                mechMode = previousMechMode;
                currentSpeed = 0;
                return;
            }

            //Rotate towards destination
            float step = maxSpeed / 30 * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(engineRoom.transform.forward, Vector3.ProjectOnPlane(dir, Vector3.up), step, 0.0F);

            transform.rotation = Quaternion.LookRotation(newDir);

            if (stamina.CurrentVal > 0)
            {
                stamina.CurrentVal -= 1;
            }

            if (tag == "Enemy")
            {
                stamina.CurrentVal -= 1;
            }

            if (Vector3.Angle(Vector3.ProjectOnPlane(dir, Vector3.up), Vector3.ProjectOnPlane(transform.forward, Vector3.up)) < 10f)
            {
                if (currentSpeed < maxSpeed)
                {
                    currentSpeed += acceleration;
                }

                transform.Translate(Vector3.ProjectOnPlane(dir.normalized * distanceThisFrame, Vector3.up), Space.World);

                if (stamina.CurrentVal > 0)
                {
                    stamina.CurrentVal -= 2;
                }
            }
        }
    }

    public virtual void RotateTowardsTarget(GameObject objectToRotate)
    {

        if (rotationDestination == null) //engineRoom.GetComponent<Room>().ener.CurrentVal <= 0
        {
            rotationCompleted = true;
            Destroy(rotationDestination);
            currentRotationSpeed = 0;
            return;
        }
        else if (rotatingTowardsFront == false)
        {
            Vector3 dir = rotationDestination.transform.position - transform.position;

            if (currentRotationSpeed < maxRotationSpeed)
            {
                currentRotationSpeed += rotationAcceleration;
            }

            //Rotate towards destination
            step = maxRotationSpeed / 20 * Time.deltaTime;
            angleAlpha = Vector3.Angle(Vector3.ProjectOnPlane(dir, Vector3.up), transform.forward);
            angleBeta = Vector3.Angle(Vector3.ProjectOnPlane(dir, Vector3.up), -transform.right);
            angleGamma = Vector3.Angle(Vector3.ProjectOnPlane(dir, Vector3.up), transform.right);

            rotationUnclamped = objectToRotate.transform.localEulerAngles.y;
            float rotationClamped = ClampAngle(rotationUnclamped, -180, 180);

            if (rotationClamped > 90)
            {
                if (angleAlpha > 90 && angleBeta < 90)
                {
                    StartCoroutine(RotateTowardsFront(torso));
                }
                else
                {
                    Vector3 newDir = Vector3.RotateTowards(objectToRotate.transform.forward, Vector3.ProjectOnPlane(dir, Vector3.up), step, 0.0F);
                    objectToRotate.transform.rotation = Quaternion.LookRotation(newDir);
                }
            }
            else if (rotationClamped < -90)
            {
                if (angleAlpha > 90 && angleGamma < 90)
                {
                    StartCoroutine(RotateTowardsFront(torso));
                }
                else
                {
                    Vector3 newDir = Vector3.RotateTowards(objectToRotate.transform.forward, Vector3.ProjectOnPlane(dir, Vector3.up), step, 0.0F);
                    objectToRotate.transform.rotation = Quaternion.LookRotation(newDir);
                }
            }
            else
            {
                Vector3 newDir = Vector3.RotateTowards(objectToRotate.transform.forward, Vector3.ProjectOnPlane(dir, Vector3.up), step, 0.0F);
                objectToRotate.transform.rotation = Quaternion.LookRotation(newDir);
            }


            rotationUnclamped = objectToRotate.transform.localEulerAngles.y;
            rotationY = ClampAngle(rotationUnclamped, -halfRotationAngle, halfRotationAngle);
            objectToRotate.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, rotationY, transform.localEulerAngles.z);

            
            if (previousTorsoRotation == objectToRotate.transform.localEulerAngles.y && rotationY != -halfRotationAngle && rotationY != halfRotationAngle)
            {
                rotationCompleted = true;
                currentRotationSpeed = 0;
                return;
            }

            if(rotationDestination != null && previousTorsoRotation == objectToRotate.transform.localEulerAngles.y)
            {
                targetUnreachable = true;
            }
            else
            {
                targetUnreachable = false;
            }

            previousTorsoRotation = objectToRotate.transform.localEulerAngles.y;
        }
    }

    public virtual IEnumerator RotateTowardsFront(GameObject objectToRotate)
    {
        rotatingTowardsFront = true;

        while (objectToRotate.transform.rotation != transform.rotation)
        {
            Vector3 newDir = Vector3.RotateTowards(objectToRotate.transform.forward, transform.forward, step, 0.0F);
            objectToRotate.transform.rotation = Quaternion.LookRotation(newDir);

            if (Vector3.Angle(Vector3.ProjectOnPlane(objectToRotate.transform.forward, Vector3.up), Vector3.ProjectOnPlane(transform.forward, Vector3.up)) < 1f)
            {
                break;
            }

            yield return new WaitForSeconds(0.005f);
        }

        rotatingTowardsFront = false;
    }

    public virtual void TargetGround()
    {
        if (selectedWeapon != null)
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);

            if (hit)
            {
                groundTarget = Instantiate(destinationPrefab, hitInfo.point, Quaternion.identity);
                groundTarget.layer = 2;

                if (selectedWeapon.GetComponent<Weapon>().enemyRoom != null)
                {
                    if (selectedWeapon.GetComponent<Weapon>().enemyRoom.tag == "GroundTarget")
                    {
                        Destroy(selectedWeapon.GetComponent<Weapon>().enemyRoom);
                    }
                }

                selectedWeapon.GetComponent<Weapon>().enemyRoom = groundTarget;
                selectedWeapon.GetComponent<Weapon>().targetSelected = true;

                if (rotationDestination != null)
                {
                    Destroy(rotationDestination);
                }

                rotationDestination = Instantiate(destinationPrefab, hitInfo.point, Quaternion.identity);
                rotationDestination.layer = 2;
                rotationCompleted = false;
            }
        }
    }

    public void DepowerWeapons()
    {
        //Weapons
        int len = weaponList.Count;
        int temp = 0;

        for (int i = 0; i < len; i++)
        {

            if (weaponList[i] != null)
            {

                if (weaponList[i].GetComponent<Weapon>().weaponPowered == true)
                {
                    if (temp + weaponList[i].GetComponent<Weapon>().energyReq <= weaponRoom.GetComponent<Room>().ener.CurrentVal)
                    {
                        temp += weaponList[i].GetComponent<Weapon>().energyReq;
                        totalWeaponEnergyReq = temp;
                    }
                    else
                    {
                        weaponList[i].GetComponent<Weapon>().weaponPowered = false;
                        weaponList[i].GetComponent<Weapon>().weaponSelected = true;
                        weaponList[i].GetComponent<Weapon>().targetSelected = false;
                        weaponList[i].GetComponent<Weapon>().enemyRoom = null;
                        weaponList[i].GetComponent<Weapon>().target = null;
                    }
                }

            }
        }
    }

    public void CreateBigExplosion()
    {
        GameObject effectIns = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 6f);
    }

    public void CreateDeathExplosion()
    {
        Invoke("CreateBigExplosion", 2f);

        foreach (GameObject room in roomList)
        {
            room.GetComponent<Room>().Invoke("CreateExplosion", Random.Range(0f, 2f));
        }
    }

    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject != null && gameObject != null)
        {
            if (col.gameObject.tag == "Breakable")
            {
                col.gameObject.GetComponent<Breakable>().health = 0;
            }
        }

    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (min < 0 && max > 0 && (angle > max || angle < min))
        {
            angle -= 360;
            if (angle > max || angle < min)
            {
                if (Mathf.Abs(Mathf.DeltaAngle(angle, min)) < Mathf.Abs(Mathf.DeltaAngle(angle, max))) return min;
                else return max;
            }
        }
        else if (min > 0 && (angle > max || angle < min))
        {
            angle += 360;
            if (angle > max || angle < min)
            {
                if (Mathf.Abs(Mathf.DeltaAngle(angle, min)) < Mathf.Abs(Mathf.DeltaAngle(angle, max))) return min;
                else return max;
            }
        }

        if (angle < min) return min;
        else if (angle > max) return max;
        else return angle;
    }

}
