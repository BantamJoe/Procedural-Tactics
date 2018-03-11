using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WeaponSelection : MonoBehaviour
{

    public GameObject player;
    public GameObject playerMech;
    public Generator generator;
    public GameObject selectedWeapon;
    public GameObject attachedWeapon;
    public Weapon weapon;

    public Color32 baseColor;
    public Color32 dimColor;
    public Color32 selectColor;

    void Start()
    {
        player = GameObject.Find("Player");
        baseColor = gameObject.GetComponent<Image>().color;
        dimColor = new Color32(255, 255, 225, 100);
        selectColor = new Color32(0, 255, 0, 255);
    }

    public void SelectWeapon()
    {
        playerMech = GameObject.Find("PlayerMech(Clone)");

        if (playerMech != null)
        {
            generator = playerMech.transform.Find("Torso").Find("GeneratorRoom").gameObject.GetComponent<Generator>();
            selectedWeapon = playerMech.GetComponent<Mech>().selectedWeapon;
        }
        
        if (attachedWeapon != null)
        {
            if (attachedWeapon.GetComponent<Weapon>() != null)
            {
                weapon = attachedWeapon.GetComponent<Weapon>();
            }
        }

        if (attachedWeapon != null && generator != null && weapon != null)
        {
            if (weapon.weaponPowered == false)
            {
                if (generator.energy.CurrentVal >= attachedWeapon.GetComponent<Weapon>().energyReq)
                {
                    weapon.weaponPowered = true;
                    generator.energy.CurrentVal -= attachedWeapon.GetComponent<Weapon>().energyReq;

                    playerMech.transform.Find("Torso").Find("WeaponRoom").GetComponent<Room>().ener.CurrentVal += attachedWeapon.GetComponent<Weapon>().energyReq;
                }
            }
            else
            {
                if (playerMech.GetComponent<Mech>().selectedWeapon != null)
                {
                    playerMech.GetComponent<Mech>().selectedWeapon.GetComponent<Weapon>().weaponSelected = false;
                }
                
                playerMech.GetComponent<Mech>().selectedWeapon = attachedWeapon;
                selectedWeapon = attachedWeapon;
                
                weapon.weaponSelected = true;
                weapon.targetSelected = false;

                if (weapon.enemyRoom != null)
                {
                    if (weapon.enemyRoom.tag == "GroundTarget")
                    {
                        Destroy(weapon.GetComponent<Weapon>().enemyRoom);
                    }

                    weapon.enemyRoom = null;
                }
                
                weapon.target = null;

                playerMech.GetComponent<Mech>().selectedWeapon = selectedWeapon;
            }
        }
    }

    public void DeselectWeapon()
    {
        if (weapon != null)
        {
            if (weapon.weaponPowered == true)
            {
                weapon.weaponSelected = false;
                weapon.weaponPowered = false;

                generator.energy.CurrentVal += attachedWeapon.GetComponent<Weapon>().energyReq;

                playerMech.transform.Find("Torso").Find("WeaponRoom").GetComponent<Room>().ener.CurrentVal -= attachedWeapon.GetComponent<Weapon>().energyReq;
            }
        }
    }

    void Update() 
    {

        if (attachedWeapon != null)
        {
            if (attachedWeapon.GetComponent<Weapon>().weaponPowered == true)
            {
                gameObject.GetComponent<Image>().color = baseColor;

                if (attachedWeapon.GetComponent<Weapon>().weaponSelected == true)
                {
                    gameObject.GetComponent<Image>().color = selectColor;
                }
            }
            else
            {
                gameObject.GetComponent<Image>().color = dimColor;
            }
        }
        
		
        if (transform.parent.gameObject.name != "WeaponsUI")
        {
            gameObject.GetComponent<Image>().color = baseColor;
        }
	}
}
