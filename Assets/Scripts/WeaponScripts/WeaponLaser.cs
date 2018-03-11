using UnityEngine;
using System.Collections;

public class WeaponLaser : ProjectileWeapon {

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
                //fireCountdown = 1f / fireRate;
                fireCountdown = originalFireCountdown;
            }

            fireCountdown -= Time.deltaTime;
        }
    }


    }
