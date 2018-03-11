using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class WeaponDropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    public GameObject playerMech;
    public List<GameObject> weaponList;

    void Start()
    {

    }

	public void OnPointerEnter(PointerEventData eventData) {
		//Debug.Log("OnPointerEnter");
		if(eventData.pointerDrag == null)
			return;

		Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
		if(d != null) {
			d.placeholderParent = transform;
		}
	}
	
	public void OnPointerExit(PointerEventData eventData) {
		//Debug.Log("OnPointerExit");
		if(eventData.pointerDrag == null)
			return;

		Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
		if(d != null && d.placeholderParent==this.transform) {
			d.placeholderParent = d.parentToReturnTo;
		}
	}
	
	public void OnDrop(PointerEventData eventData) {
        //Debug.Log (eventData.pointerDrag.name + " was dropped on " + gameObject.name);

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Draggable d = eventData.pointerDrag.GetComponent<Draggable>();

            if (d != null)
            {
                playerMech = GameObject.Find("PlayerMech(Clone)");

                if(playerMech != null)
                {
                    weaponList = playerMech.GetComponent<PlayerMech>().weaponList;

                    int weaponSlotCount = playerMech.GetComponent<PlayerMech>().weaponSlotCount;

                    for (int i = 0; i < weaponSlotCount; i++)
                    {
                        if (playerMech.transform.Find("Torso").Find("WeaponSlotParent") != null)
                        {
                            if (playerMech.transform.Find("Torso").Find("WeaponSlotParent").GetChild(i).childCount == 0 && d.parentToReturnTo != transform)
                            {
                                d.gameObject.GetComponent<WeaponSelection>().attachedWeapon.transform.SetParent(playerMech.transform.Find("Torso").Find("WeaponSlotParent").GetChild(i));
                                d.gameObject.GetComponent<WeaponSelection>().attachedWeapon.transform.rotation = d.gameObject.GetComponent<WeaponSelection>().attachedWeapon.transform.parent.rotation;
                                d.gameObject.GetComponent<WeaponSelection>().attachedWeapon.transform.localPosition = new Vector3(0, 0, 0);
                                d.gameObject.GetComponent<WeaponSelection>().attachedWeapon.SetActive(true);
                                d.parentToReturnTo = this.transform;
                                break;
                            }
                        }
                    }

                    playerMech.GetComponent<PlayerMech>().equippedWeaponCount++;
                }
                
            }
        }
	}
}
