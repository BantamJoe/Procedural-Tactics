using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon {

    [Header("Projectile Fields")]
    public GameObject bulletPrefab;
    
    public List<GameObject> roomList;
    public List<GameObject> targetRoomList;


    public virtual void Shoot()
    {
        mech = transform.root.gameObject;

        if (transform.root.Find("Shield").gameObject != null)
        {
            shield = transform.root.Find("Shield").gameObject;

        }

        roomList = mech.GetComponent<Mech>().GetRooms();

        GameObject projectileGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Projectile projectile = projectileGO.GetComponent<Projectile>();

        if (mech.GetComponent<Collider>() != null)
        {
            Physics.IgnoreCollision(projectileGO.GetComponent<Collider>(), mech.GetComponent<Collider>());
        }

        if (target != null)
        {
            if (target.root.GetComponent<Collider>() != null)
            {
                Physics.IgnoreCollision(projectileGO.GetComponent<Collider>(), target.root.GetComponent<Collider>());
            }
        }

        if (shield.GetComponent<Collider>() != null)
        {
            Physics.IgnoreCollision(projectileGO.GetComponent<Collider>(), shield.GetComponent<Collider>());
        }

        foreach (GameObject room in roomList)
        {
            if (room.GetComponent<Collider>() != null)
            {
                Physics.IgnoreCollision(projectileGO.GetComponent<Collider>(), room.GetComponent<Collider>());
            }
        }

        GameObject[] temp = GameObject.FindGameObjectsWithTag("Projectile");
        int len = temp.Length;

        for (int i = 0; i < len; i++)
        {
            if (temp[i].GetComponent<Collider>() != null)
            {
                Physics.IgnoreCollision(projectileGO.GetComponent<Collider>(), temp[i].GetComponent<Collider>());
            }
        }

        if (projectile != null)
        {
            projectile.Seek(target);
            projectile.GetOrigin(mech);
        }

        mech.GetComponent<Mech>().heat.CurrentVal += heat;
    }

    public virtual IEnumerator ShootVolley(int count)
    {
        for (int f = 0; f < count; f++)
        {
            Shoot();
            yield return new WaitForSeconds(0.3f);
        }
    }

    public virtual void ShootAllRooms()
    {
        if (target != null)
        {
            mech = transform.root.gameObject;
            shield = transform.root.Find("Shield").gameObject;
            roomList = mech.GetComponent<Mech>().GetRooms();

            targetRoomList = target.transform.parent.GetComponent<Mech>().GetRooms();

            foreach (GameObject room in targetRoomList)
            {
                GameObject projectileGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                Projectile projectile = projectileGO.GetComponent<Projectile>();

                if (mech.GetComponent<Collider>() != null)
                {
                    Physics.IgnoreCollision(projectileGO.GetComponent<Collider>(), mech.GetComponent<Collider>());
                }

                if (target.root.GetComponent<Collider>() != null)
                {
                    Physics.IgnoreCollision(projectileGO.GetComponent<Collider>(), target.root.GetComponent<Collider>());
                }

                if (shield.GetComponent<Collider>() != null)
                {
                    Physics.IgnoreCollision(projectileGO.GetComponent<Collider>(), shield.GetComponent<Collider>());
                }

                foreach (GameObject ownRoom in roomList)
                {
                    if (room.GetComponent<Collider>() != null)
                    {
                        Physics.IgnoreCollision(projectileGO.GetComponent<Collider>(), ownRoom.GetComponent<Collider>());
                    }
                }

                if (projectileGO.GetComponent<Projectile>().ignoreShield == true)
                {
                    Physics.IgnoreCollision(projectileGO.GetComponent<Collider>(), target.root.Find("Shield").GetComponent<Collider>());
                }


                GameObject[] temp = GameObject.FindGameObjectsWithTag("Projectile");
                int len = temp.Length;

                for (int i = 0; i < len; i++)
                {
                    if (projectileGO.GetComponent<Collider>() != null)
                    {
                        Physics.IgnoreCollision(projectileGO.GetComponent<Collider>(), temp[i].GetComponent<Collider>());
                    }
                }

                if (projectile != null)
                {
                    projectile.Seek(room.transform);
                    projectile.GetOrigin(mech);
                }

                mech = transform.parent.parent.parent.gameObject;

                mech.GetComponent<Mech>().heat.CurrentVal += heat;
            }

        }

    }
}
