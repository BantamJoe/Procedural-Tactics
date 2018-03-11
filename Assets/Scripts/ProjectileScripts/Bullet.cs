using UnityEngine;

public class Bullet : Projectile {

    void Start()
    {
        Destroy(gameObject, destroyDelay);
        originalPosition = transform.position;
        timeTravel = 0.1f;
    }

    void Update()
    {

        if (target != null)
        {
            if (isLaunched == false)
            {
                isLaunched = true;
                Launch(target);
            }
        }
    }


}
