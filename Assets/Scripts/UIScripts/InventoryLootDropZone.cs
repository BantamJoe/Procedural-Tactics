using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class InventoryLootDropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    public GameObject playerMech;
    public GameObject playerUI;

    public void OnPointerEnter(PointerEventData eventData)
    {
		//Debug.Log("OnPointerEnter");
		if(eventData.pointerDrag == null)
			return;

		Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
		if(d != null)
        {
			d.placeholderParent = transform;
		}
	}
	
	public void OnPointerExit(PointerEventData eventData)
    {
		//Debug.Log("OnPointerExit");
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
		//Debug.Log (eventData.pointerDrag.name + " was dropped on " + gameObject.name);

		//Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
	}

    void Update()
    {
     
    }
}
