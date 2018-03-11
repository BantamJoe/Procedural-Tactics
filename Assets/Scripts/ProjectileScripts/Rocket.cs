using UnityEngine;

public class Rocket : Projectile {

    void Start()
    {
        Destroy(gameObject, destroyDelay);
        originalPosition = transform.position;
        transform.RotateAround(transform.position, transform.forward, Random.Range(0f,1f) * 360f);
        timeTravel = 1f;
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

            Vector3 vel = GetComponent<Rigidbody>().velocity;

            transform.RotateAround(transform.position, transform.forward, Time.deltaTime * 360f);
        }
    }


}
