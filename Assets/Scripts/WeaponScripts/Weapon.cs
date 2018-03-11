using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {

    public bool weaponSelected = false;
    public bool targetSelected = false;
    public bool weaponPowered = true;

    public GameObject icon;
    public GameObject enemyRoom = null;
    public GameObject weaponRoom;
    public GameObject mech;
    public GameObject shield;
    public GameObject rotationDestination;
    public GameObject objectToRotate;

    public Transform target = null;
    public Transform firePoint;
    public Transform partToRotate;

    [Header("Attributes")]
    
    public int energyReq = 1;
    public int heat = 10;
    public float fireRate;
    public float fireCountdown = 1f;
    public float originalFireCountdown;
    public int burstDamage = 1;

    public float halfFiringAngle;

    public float maxRotationSpeed;
    public float currentRotationSpeed;
    public float rotationAcceleration;

    public float rotationY;
    public float rotationUnclamped;
    public float previousRotation;

    float angleAlpha;
    float angleBeta;
    float angleGamma;
    float step;

    public bool rotatingTowardsFront;
    public bool rotationCompleted;
    public bool targetUnreachable;

    public void Initialize()
    {
        mech = transform.root.gameObject;
    }

    public virtual bool GetWeaponSelected()
    {
        return weaponSelected;
    }

    public virtual void SetWeaponRoom()
    {
        weaponRoom = transform.parent.parent.parent.Find("WeaponRoom").gameObject;
    }

    public virtual void RotateWeapon()
    {
        rotationDestination = enemyRoom;

        if (rotationDestination == null || weaponPowered == false)
        {
            rotationCompleted = true;
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
                    StartCoroutine(RotateTowardsFront(gameObject));
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
                    StartCoroutine(RotateTowardsFront(gameObject));
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
            rotationY = ClampAngle(rotationUnclamped, -halfFiringAngle, halfFiringAngle);
            objectToRotate.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, rotationY, transform.localEulerAngles.z);


            if (previousRotation == objectToRotate.transform.localEulerAngles.y && rotationY != -halfFiringAngle && rotationY != halfFiringAngle)
            {
                rotationCompleted = true;
                currentRotationSpeed = 0;
                return;
            }

            if (rotationDestination != null && previousRotation == partToRotate.transform.localEulerAngles.y && mech.GetComponent<Mech>().targetUnreachable == true)
            {
                targetUnreachable = true;
            }
            else
            {
                targetUnreachable = false;
            }

            previousRotation = objectToRotate.transform.localEulerAngles.y;
            rotationCompleted = false;
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
