using UnityEngine;

public class Missile : Projectile {

    void Start()
    {
        ignoreShield = true;

        if (target.root.Find("Shield"))
        {
            if (target.root.Find("Shield").GetComponent<Collider>())
            {
                Physics.IgnoreCollision(GetComponent<Collider>(), target.root.Find("Shield").GetComponent<Collider>());
            }
        }

        destroyDelay = 30;
        effectDelay = 20;
        Destroy(gameObject, destroyDelay);
        originalPosition = transform.position;
        timeTravel = 5f;
    }

    public override Vector3 calculateBestThrowSpeed(Vector3 origin, Vector3 target, float timeToTarget)
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
        float t = timeToTarget;
        float v0y = y / t + 0.5f * Physics.gravity.magnitude * t;
        float v0xz = xz / t;

        // create result vector for calculated starting speeds
        Vector3 result = toTargetXZ.normalized;        // get direction of xz but with magnitude 1
        result *= v0xz;                                // set magnitude of xz to v0xz (starting speed in xz plane)
        result.y = v0y;                                // set y to v0y (starting speed of y plane)

        return result;
    }

    public override void DestroyOnHit()
    {
        GameObject effectIns = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 2f);

        targetHit = true;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        Destroy(GetComponent<Collider>());
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<Light>());
        transform.localScale = new Vector3(0, 0, 0);
        Destroy(gameObject, effectDelay);

        //Exra for missile
        transform.Find("WhiteSmoke").gameObject.GetComponent<ParticleScript>().StopSmoke();
        Destroy(transform.Find("LightParent").gameObject.GetComponent<Light>());

        if (gameObject.transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).GetComponent<MeshRenderer>());
        }
    }

    void Update()
    {
        if (targetHit == false)
        {
            if (target != null)
            {
                if (isLaunched == false)
                {
                    isLaunched = true;
                    Launch(target);
                }
            }

            //Rotate in moving direction
            Vector3 vel = GetComponent<Rigidbody>().velocity;

            if (vel != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(vel);
            }
        }
    }


}
