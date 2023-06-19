using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemData : Interactable, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Item connectedItem;
    public ItemContainer container;
    public ItemFrame frameData;
    int slotIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public int getSlotIndex() {
        return slotIndex;
    }

    public void setSlotIndex(int index) {
        slotIndex = index;
    }
    void Awake() {
     
        
    }

    // Update is called once per frame
    void Update()
    {
        if(connectedItem != null && connectedItem.itemHash == -1) {
            connectedItem.itemHash = (long)(Time.time + (transform.position.magnitude * Random.Range(1,250)));
            print(connectedItem.itemHash);
        }
    }


    Transform parentSave;
    public void OnBeginDrag(PointerEventData eventData) {
        if(connectedItem != null) {
            parentSave = this.transform.parent;
            this.transform.SetParent(FindObjectOfType<PlayerInterfaceRef>().transform);
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            this.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData) {
        if (connectedItem != null) {
            this.transform.position = eventData.position;
        }
    }


    public void OnEndDrag(PointerEventData eventData) {
        this.transform.SetParent(parentSave);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        this.transform.position = parentSave.transform.position;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if(eventData.button == PointerEventData.InputButton.Right) {
            if(frameData.frameType == FrameType.Inventory) {
                FindObjectOfType<PlayerInventory>().HoldItem(frameData.slotIndex);
            }
        }
    }


    public override void Interact() {
        base.Interact();
        print("Interacting!");
        if (connectedItem != null) {
            FindObjectOfType<PlayerInventory>().addItem(connectedItem);
            if (this.container != null && slotIndex != -1) {

                container.RemoveItem(slotIndex);
            }
            else {
                Destroy(gameObject);
            }
        }
    }

    
}
