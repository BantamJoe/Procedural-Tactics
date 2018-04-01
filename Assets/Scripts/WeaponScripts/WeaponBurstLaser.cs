using UnityEngine;
using System.Collections;

public class WeaponBurstLaser : ProjectileWeapon {

    [Header("Extra Attributes")]

    public int burstCount;
    public float timeBetween = 0.15f;
    public bool burstActive = false;

    private int temp;

    [Header("Unity Setup Fields")]

    public float turnSpeed = 10f;

    void Awake()
    {

    }

    void Start()
    {
        objectToRotate = transform.Find("PartToRotate").gameObject;
        Initialize();
        SetWeaponRoom();
        weaponSelected = false;
        targetSelected = false;
        weaponPowered = false;
        enemyRoom = null;

        burstDamage = burstCount * bulletPrefab.GetComponent<Projectile>().damage;

        originalFireCountdown = fireCountdown;

        halfFiringAngle = 17.5f;

        maxRotationSpeed = 40f;
        currentRotationSpeed = 0f;
        rotationAcceleration = 0.2f;
    }


    void Update()
    {
        if (weaponPowered)
        {
            fireCountdown -= Time.deltaTime;

            if (targetSelected)
            {
                if (enemyRoom != null)
                {
                    target = enemyRoom.transform;
                }
            }

            if (target == null)
            {
                return;
            }

            RotateWeapon();

            if (fireCountdown <= 0f && rotationCompleted == true)
            {
                Shoot();
                if (temp < burstCount-1)
                {
                    temp++;
                    fireCountdown = timeBetween;
                }
                else
                {
                    temp = 0;
                    //fireCountdown = 1f / fireRate;
                    fireCountdown = originalFireCountdown;
                }
            }
        }
    }
}
