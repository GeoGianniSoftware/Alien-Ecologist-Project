using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : Interactable
{
    ItemContainer container;
    public Item storedItem;
    public Vector3 rotationOnSpawn;
    public int slotIndex;

    List<ItemType> allowedItemTypes = new List<ItemType>();

    public GameObject storedItemObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void setContainer(ItemContainer toSet) {
        container = toSet;
        allowedItemTypes = toSet.allowedItemTypes;
    }

    public bool isEmpty() {
        if (storedItem == null || storedItem.ID == -1) {
            return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update() {
        if (isEmpty() && storedItemObject != null) {
            if (transform.childCount > 0 && transform.GetChild(0) != null)
                Destroy(storedItemObject);
        }

        if(storedItemObject == null) {
            storedItem = null;
        }
    }

    


    public override void Interact() {
        base.Interact();
        print("Interacting!");
        if (storedItem == null && container != null && container.PI.itemBeingHeld != null && container.allowedItemTypes.Contains(container.PI.itemBeingHeld.TYPE)) {
            print("ding");
            container.StoreItem(container.PI.transferItem(container.PI.itemBeingHeld), this);
            container.Additem(container.PI.itemBeingHeld, this.slotIndex);
        }
        else if(storedItem != null && container.PI.getNextEmpty() != -1) {
            print("Adding ");
            if (storedItem.TYPE == ItemType.TOOL) {
                print("Adding Weapon");
                
                container.PI.addItem((ToolItem)storedItem);
            }
            else {

                container.PI.addItem(storedItem);
            }
            container.RemoveItem(slotIndex);
        }
    }
}
