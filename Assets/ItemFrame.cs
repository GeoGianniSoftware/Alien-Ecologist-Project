using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum FrameType
{
    Inventory,
    Toolbelt,
    Container
}

public class ItemFrame : MonoBehaviour, IDropHandler
{
    public Item connectedItem;
    public bool equipped;
    Image spriteImage;
    GameObject spriteItem;
    public int slotIndex;
    public FrameType frameType;
    ItemContainer containterData;
    Color frameColor;


    public void setContainer(ItemContainer container) {
        containterData = container;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        frameColor = GetComponent<Image>().color;
    }

    public void setSlotType(FrameType Ftype) {
        frameType = Ftype;
    }

    public void Clear() {
        connectedItem = null;
        equipped = false;
        spriteItem = null;
        
    }

    public void OnDrop(PointerEventData eventData) {

        ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData>();
        if(droppedItem != null && connectedItem == null) {
            print("Ding");

            

            bool failed = false;

            if (frameType == FrameType.Inventory)
                FindObjectOfType<PlayerInventory>().addItem(droppedItem.connectedItem, slotIndex);
            else if (frameType == FrameType.Toolbelt) {
                if (droppedItem.connectedItem.TYPE != ItemType.TOOL)
                    failed = true;
                else
                FindObjectOfType<PlayerInventory>().addTool((ToolItem)droppedItem.connectedItem, slotIndex);
            }
                
            else if (frameType == FrameType.Container)
                if(containterData != null) {
                    containterData.Additem(droppedItem.connectedItem, slotIndex);
                }

            if (!failed) {
                if (droppedItem.frameData.frameType != FrameType.Container) {
                    FindObjectOfType<PlayerInventory>().RemoveItem((int)droppedItem.frameData.frameType, droppedItem.frameData.slotIndex);


                }
                else {
                    if(droppedItem.getSlotIndex() != -1)
                    droppedItem.container.RemoveItem(droppedItem.getSlotIndex());
                    else if(droppedItem.frameData != null) {
                        print(droppedItem.frameData.slotIndex);
                        droppedItem.container.RemoveItem(droppedItem.frameData.slotIndex);
                    }
                }


                droppedItem.frameData.Clear();
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        if(connectedItem != null) {
            if(transform.childCount == 0 && spriteItem == null) {
                spriteItem = Instantiate(Resources.Load("UI/ItemSprite") as GameObject, transform);
            }
            if (transform.childCount > 0 && GetComponentInChildren<ItemData>().connectedItem == null) {

                spriteImage = transform.GetChild(0).GetComponent<Image>();
                spriteImage.enabled = true;

                GetComponentInChildren<ItemData>().connectedItem = connectedItem;
                GetComponentInChildren<ItemData>().frameData = this;
                if(containterData != null)
                GetComponentInChildren<ItemData>().container = containterData;

            }

            if (spriteImage.sprite != connectedItem.SPRITE)
                spriteImage.sprite = connectedItem.SPRITE;


            
        }
        else {
            if(transform.childCount > 0) {
                for (int i = 0; i < transform.childCount; i++) {
                    Destroy(transform.GetChild(i).gameObject);
                    print("ADDDDD!");
                }
            }
        }

        if (equipped ) {
            GetComponent<Image>().color = Color.green;
        }else if(FindObjectOfType<PlayerInventory>().itemBeingHeld != null && FindObjectOfType<PlayerInventory>().itemBeingHeld == connectedItem) {

            GetComponent<Image>().color = Color.red + Color.blue;
        }
        else if(GetComponent<Image>().color != frameColor) {
            GetComponent<Image>().color = frameColor;
        }



    }

    GameObject holding = null;

  

    bool isMouseOverUI() {
        return EventSystem.current.IsPointerOverGameObject();
    }

    
}
