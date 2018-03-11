using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public Transform target;

    public GameObject origin;
    public GameObject impactEffect;
    public GameObject playerMech;

    public int damage = 1;
    public bool ignoreShield;

    //public float targetSize = 1.3f;
    public float destroyDelay = 30f;
    public float effectDelay = 15f;
    public bool targetHit;

    public bool isGuided;

    public Vector3 originalPosition;
    public float timeTravel = 0.1f;

    public float angle;
    public bool isLaunched = false;
    public bool fixDone = false;

    public virtual Vector3 calculateBestThrowSpeed(Vector3 origin, Vector3 target, float timeToTarget)
    {
        // calculate vectors
        Vector3 toTarget = target - origin;
        Vector3 toTargetXZ = toTarget;
        toTargetXZ.y = 0;

        // calculate xz and y
        float y = toTarget.y;
        float xz = toTargetXZ.magnitude;

        // calculate starting speeds for xz and y. Physics forumulase deltaX = v0 * t + 1/2 * a * t * t
        // where a is "-gravity" but only on the y plane, and a is 0 in xz plane.
        // so xz = v0xz * t => v0xz = xz / t
        // and y = v0y * t - 1/2 * gravity * t * t => v0y * t = y + 1/2 * gravity * t * t => v0y = y / t + 1/2 * gravity * t
        float t = timeToTarget + xz / 100;
        float v0y = y / t + 0.5f * Physics.gravity.magnitude * t;
        float v0xz = xz / t;

        // create result vector for calculated starting speeds
        Vector3 result = toTargetXZ.normalized;        // get direction of xz but with magnitude 1
        result *= v0xz;                                // set magnitude of xz to v0xz (starting speed in xz plane)
        result.y = v0y;                                // set y to v0y (starting speed of y plane)

        return result;
    }

    public virtual void Launch(Transform newTarget)
    {
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().AddForce(calculateBestThrowSpeed(originalPosition, target.position, timeTravel), ForceMode.VelocityChange);
        }
    }

    public void Seek(Transform _target)
    {
        target = _target;
    }

    public void GetOrigin(GameObject _origin)
    {
        origin = _origin;
    }

    public void HitTarget(GameObject obj)
    {
        Room enemyRoom = obj.GetComponent<Room>();

        if (enemyRoom != null)
        {
            if (enemyRoom.transform.root.GetComponent<Mech>() != null)
            {
                enemyRoom.RoomHit(damage);

                if (enemyRoom.health.CurrentVal > 0)
                {
                    enemyRoom.health.CurrentVal -= damage;
                }

                if (enemyRoom.transform.root.GetComponent<Mech>().health.CurrentVal > 0)
                {
                    enemyRoom.transform.root.GetComponent<Mech>().health.CurrentVal -= damage;
                }
            }
        }

        DestroyOnHit();
    }

    public virtual void DestroyOnHit()
    {
        GameObject effectIns = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 8f);

        targetHit = true;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        Destroy(GetComponent<Collider>());
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<Light>());
        transform.localScale = new Vector3(0, 0, 0);
        Destroy(gameObject, effectDelay);

        if (gameObject.transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).GetComponent<MeshRenderer>());
        }
    }

    public void OnCollisionEnter(Collision col)
    {
        if (targetHit == false && col.gameObject != null && gameObject != null)
        {
            if (col.gameObject.GetComponent<Shield>() != null && col.gameObject.GetComponent<Shield>().shield.CurrentVal > 0)
            {
                if (origin == null)
                {
                    DestroyOnHit();
                    return;
                }
                else
                {
                    if (col.gameObject.GetComponent<Shield>().regenAmount == col.gameObject.GetComponent<Shield>().regenTime)
                    {
                        col.gameObject.GetComponent<Shield>().regenAmount = 0;
                    }

                    col.gameObject.GetComponent<Shield>().shield.CurrentVal--;
                }
            }

            if (col.gameObject.tag == "Room")
            {
                HitTarget(col.gameObject);
            }

            if (col.gameObject.tag == "Breakable")
            {
                col.gameObject.GetComponent<Breakable>().health -= damage;
            }
        }

        DestroyOnHit();
    }

}
