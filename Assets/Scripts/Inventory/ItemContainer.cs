using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer : Interactable
{
    [System.NonSerialized]
    public PlayerInventory PI;

    public bool isVirtual = false;
    public int virtualStorageSize;

    public Item[] storedItems;
    public List<ItemType> allowedItemTypes = new List<ItemType>();
    public ItemSlot[] slots;
    int slotCount;

    List<Item> prestored = new List<Item>();

    public float lastUpdated;

    // Start is called before the first frame update
    public void Start()
    {

        PI = FindObjectOfType<PlayerInventory>();
        prestored.AddRange(storedItems);

        storedItems = new Item[virtualStorageSize];
        

        for (int i = 0; i < prestored.Count; i++) {
            storedItems[i] = prestored[i];
        }

        if (!isVirtual) {
            slotCount = transform.GetChild(0).transform.childCount;
            slots = new ItemSlot[slotCount];
            storedItems = addEmptySlots(storedItems, slots.Length);

            for (int i = 0; i < slotCount; i++) {
                ItemSlot slot = transform.GetChild(0).transform.GetChild(i).GetComponent<ItemSlot>();

                slots[i] = (slot);
                slot.slotIndex = i;
                slot.setContainer(this);

            }

            for (int i = 0; i < storedItems.Length; i++) {
                if (storedItems[i] != null && storedItems[i].PREFAB != null && allowedItemTypes.Contains(storedItems[i].TYPE)) {
                    slots[i].storedItem = storedItems[i];
                    slots[i].storedItemObject = Instantiate(slots[i].storedItem.PREFAB, slots[i].transform.position, slots[i].transform.rotation);
                    slots[i].storedItemObject.GetComponent<ItemData>().container = this;
                    slots[i].storedItemObject.GetComponent<ItemData>().connectedItem = slots[i].storedItem;
                    slots[i].storedItemObject.transform.SetParent(slots[i].transform, true);
                    slots[i].storedItemObject.transform.rotation *= Quaternion.Euler(slots[i].rotationOnSpawn);

                    if (slots[i].storedItem.TYPE == ItemType.SAMPLE) {
                        SampleItem temp = (SampleItem)slots[i].storedItem;
                        temp.name = temp.NAME;
                        slots[i].storedItemObject.GetComponent<VialEffects>().liquidColor = temp.sampleColor;
                        slots[i].storedItemObject.AddComponent<ItemData>();
                        slots[i].storedItemObject.GetComponent<ItemData>().connectedItem = temp;
                    }
                }
            }
        }
        else {

        }


    }

    Item[] addEmptySlots(Item[] og, int numberOfSpots) {
        Item[] temp = new Item[og.Length + numberOfSpots];
        for (int i = 0; i < og.Length; i++) {
            temp[i] = og[i];
        }
        lastUpdated = Time.time;
        return temp;
    }

    int getNextEmpty() {
        for (int i = 0; i < storedItems.Length; i++) {
            if (storedItems[i] == null)
                return i;
        }
        return -1;
    }

    public virtual bool canStore() {

        if (!isVirtual) {
            foreach(ItemSlot s in slots) {
                if (s.storedItem == null)
                    return true;
            }
        }
        else {
            if (returnEmptySlots() > 0)
                return true;
        }

        return false;
    }

    int returnEmptySlots() {
        int empty = 0;
        for (int i = 0; i < storedItems.Length; i++) {
            if (storedItems[i] == null)
                empty++;
        }
        return empty;
    }

    public void Additem(Item i) {
        if (canStore()) {
            print(getNextEmpty());
            storedItems[getNextEmpty()] = (i);

            lastUpdated = Time.time;
        }
    }

    public void Additem(Item i, int slot) {
        print(canStore());

        if (canStore() && storedItems[slot] == null) {
            storedItems[slot] = (i);
            lastUpdated = Time.time;
        }
    }

    public void StoreItem(Item i, ItemSlot slotStored) {
        if (slotStored.isEmpty() && i.PREFAB != null && allowedItemTypes.Contains(i.TYPE)) {
            print("Ding");
            slotStored.storedItem = i;
            slotStored.storedItemObject = Instantiate(i.PREFAB, slotStored.transform.position, slotStored.transform.rotation);
            if(slotStored.storedItemObject.GetComponent<ItemData>() == null)
                slotStored.storedItemObject.AddComponent<ItemData>();

            slotStored.storedItemObject.GetComponent<ItemData>().container = this;
            slotStored.storedItemObject.GetComponent<ItemData>().setSlotIndex(slotStored.slotIndex);
            slotStored.storedItemObject.GetComponent<ItemData>().connectedItem = i;
            slotStored.storedItemObject.transform.SetParent(slotStored.transform, true);
            slotStored.storedItemObject.transform.rotation *= Quaternion.Euler(slotStored.rotationOnSpawn);

            if (i.TYPE == ItemType.SAMPLE) {
                SampleItem temp = (SampleItem)i;
                temp.name = temp.NAME;
                slotStored.storedItemObject.GetComponent<VialEffects>().liquidColor = temp.sampleColor;
                
                slotStored.storedItemObject.GetComponent<ItemData>().connectedItem = temp;
                
            }
            print("Adding " + i.NAME + " to " + gameObject.name);
            Additem(i, slotStored.slotIndex);

            lastUpdated = Time.time;
        }
        else {
            print("StoreFailed");
        }
    }

    public void RemoveItem(int slot) {
        print(slot);
        if(storedItems[slot] != null) {
            if(slots.Length > 0 && slots[slot].storedItemObject != null) {
                slots[slot].storedItem = null;
                Destroy(slots[slot].storedItemObject);
            }
            storedItems[slot] = null;

        }

        lastUpdated = Time.time;
    }

    int getIndexOfItem(Item itemToCheckFor) {
        for (int i = 0; i < storedItems.Length; i++) {
            if (storedItems[i] == itemToCheckFor)
                return i;
        }
        return -1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact() {
        if(isVirtual)
        PI.GetComponent<PlayerUI>().showStorageUI(this);
    }
}
