using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class InventoryDropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    public GameObject playerMech;
    public GameObject playerUI;

    void Start()
    {
        playerMech = GameObject.Find("PlayerMech(Clone)");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
		if(eventData.pointerDrag == null)
			return;

		Draggable d = eventData.pointerDrag.GetComponent<Draggable>();

		if(d != null) {
			d.placeholderParent = transform;
		}
	}
	
	public void OnPointerExit(PointerEventData eventData)
    {
		if(eventData.pointerDrag == null)
			return;

		Draggable d = eventData.pointerDrag.GetComponent<Draggable>();

		if(d != null && d.placeholderParent==this.transform)
        {
			d.placeholderParent = d.parentToReturnTo;
		}
	}
	
	public void OnDrop(PointerEventData eventData)
    {

		Draggable d = eventData.pointerDrag.GetComponent<Draggable>();

        if (playerMech == null)
        {
            playerMech = GameObject.Find("PlayerMech(Clone)");
        }

		if (d != null && playerMech != null)
        {
            //From equipped to inventory
            if (d.parentToReturnTo == playerUI.transform.Find("WeaponsUI").transform)
            { 
                playerMech.GetComponent<PlayerMech>().equippedWeaponCount--;

                d.gameObject.GetComponent<WeaponSelection>().attachedWeapon.transform.parent.DetachChildren();
                d.gameObject.GetComponent<WeaponSelection>().attachedWeapon.SetActive(false);
                d.gameObject.GetComponent<WeaponSelection>().attachedWeapon.transform.SetParent(playerMech.transform);
                d.gameObject.GetComponent<WeaponSelection>().attachedWeapon.GetComponent<Weapon>().fireCountdown = d.gameObject.GetComponent<WeaponSelection>().attachedWeapon.GetComponent<Weapon>().originalFireCountdown;
            }
            //From loot to inventory
            else if (d.parentToReturnTo == playerUI.transform.Find("InventoryLoot").transform)
            {
                d.gameObject.GetComponent<WeaponSelection>().attachedWeapon.transform.SetParent(playerMech.transform);
                
            }

            d.parentToReturnTo = this.transform;
        }
	}

    void Update()
    {
     
    }
}
