using UnityEngine;

public class Laser : Projectile {

	public float speed;
 
    void Start()
    {
        Destroy(gameObject, destroyDelay);
        speed = 130f;
        transform.LookAt(target);
    }

    void Update()
    {
        float distanceThisFrame = speed * Time.deltaTime;

        if (target == null)
        {
            transform.Translate(transform.forward * distanceThisFrame, Space.World);
            Destroy(gameObject, destroyDelay);
            return;
        }
        else
        {
            //Guided projectile
            if (targetHit == false)
            {
                if (isGuided == true)
                {
                    Vector3 dir = target.position - transform.position;
                    transform.Translate(dir.normalized * distanceThisFrame, Space.World);
                }
                else
                {
                    transform.Translate(transform.forward * distanceThisFrame, Space.World);
                }
                
            }
        }
    }

}
