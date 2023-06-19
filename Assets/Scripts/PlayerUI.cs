using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Text interactText;
    PlayerController PC;

    public GameObject toolbeltUI;
    public ItemFrame[] toolSprites;


    public GameObject inventoryUI;
    public ItemFrame[] inventorySprites;

    public GameObject storageUI;
    public ItemFrame[] storageSprites;
    ItemContainer activeStorage;
    float storageUpdatedTime;


    void Start()
    {
        PC = GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        InteractText();
        PopulateToolbeltUI();
        PopulateInventoryUI();

        if (storageUI.activeSelf)
            PC.setPlayerCanMove(false);
        else
            PC.setPlayerCanMove(true);

        if(activeStorage != null && storageUpdatedTime != activeStorage.lastUpdated) {
            showStorageUI(activeStorage);
        }

        if (Input.GetKeyDown(KeyCode.Tab)) {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
            if (storageUI.activeSelf) {
                activeStorage = null;
                hideStorageUI();
            }
        }
    }

    void clearStorageUI() {
        if(storageUI.transform.childCount > 0) {
            for (int i = 0; i < storageUI.transform.childCount; i++) {
                Destroy(storageUI.transform.GetChild((storageUI.transform.childCount-1)-i).gameObject);
            }
        }
    }

    

    public void setInventoryVisable(bool visible) {
        inventoryUI.SetActive(visible);
    }

    public void hideStorageUI() {
        storageUI.SetActive(false);
    }

    public void showStorageUI(ItemContainer container) {
        clearStorageUI();
        
        populateStorageUI(container);
        storageUpdatedTime = container.lastUpdated;
        setInventoryVisable(true);
        storageUI.SetActive(true);
    }

    void PopulateToolbeltUI() {
        if(toolSprites.Length <= 0) {
            int count = PC.inventory.toolbeltSize;
            toolSprites = new ItemFrame[count];

            for (int i = 0; i < count; i++) {
                GameObject frame = Instantiate(Resources.Load("UI/ToolFrames") as GameObject, toolbeltUI.transform);
                frame.GetComponent<ItemFrame>().setSlotType(FrameType.Toolbelt);
                frame.GetComponent<ItemFrame>().slotIndex = i;
                toolSprites[i] = frame.GetComponent<ItemFrame>();

            }

        }
        else {
            for (int i = 0; i < PC.inventory.toolbelt.Length; i++) {
                ItemFrame frame = toolSprites[i];
                if(frame.connectedItem != PC.inventory.toolbelt[i]) {
                    frame.connectedItem = PC.inventory.toolbelt[i];
                }
                if (PC.inventory.currentToolSlot == i) {
                    frame.equipped = true;
                }
                else {
                    frame.equipped = false;
                }
                
                
            }
        }
    }
    void PopulateInventoryUI() {
        if (inventorySprites.Length <= 0) {
            int count = PC.inventory.inventorySize;
            inventorySprites = new ItemFrame[count];

            for (int i = 0; i < count; i++) {
                GameObject frame = Instantiate(Resources.Load("UI/ToolFrames") as GameObject, inventoryUI.transform);
                frame.GetComponent<ItemFrame>().setSlotType(FrameType.Inventory);
                frame.GetComponent<ItemFrame>().slotIndex = i;
                inventorySprites[i] = frame.GetComponent<ItemFrame>();

            }

        }
        else {
            for (int i = 0; i < PC.inventory.inventory.Length; i++) {
                ItemFrame frame = inventorySprites[i];
                if (frame.connectedItem != PC.inventory.inventory[i]) {
                    frame.connectedItem = PC.inventory.inventory[i];
                }


            }
        }
    }
    void populateStorageUI(ItemContainer container) {
        if (container.storedItems.Length > 0) {
            activeStorage = container;
            int count = container.storedItems.Length;

            storageSprites = new ItemFrame[container.virtualStorageSize];

            for (int i = 0; i < container.storedItems.Length; i++) {
                GameObject frame = Instantiate(Resources.Load("UI/ToolFrames") as GameObject, storageUI.transform);
                frame.GetComponent<ItemFrame>().setSlotType(FrameType.Container);
                frame.GetComponent<ItemFrame>().setContainer(container);
                frame.GetComponent<ItemFrame>().slotIndex = i;
                storageSprites[i] = frame.GetComponent<ItemFrame>();

                if (container.storedItems[i] != null) {

                    print("add");
                    storageSprites[i].connectedItem = container.storedItems[i];
                }
            }
        }
    }
    void InteractText() {
        //Interact Box
        if (PC.lookingAtInteractable != null) {
            interactText.enabled = true;

            //Looking At Item
            Item itemBeingLookedAt = null;
            ItemSlot slot = null;

            if (PC.lookingAtInteractable.GetComponent<ItemSlot>() || PC.lookingAtInteractable.GetComponent<ItemData>()) {

                if (PC.lookingAtInteractable.GetComponent<ItemSlot>()) {
                    slot = PC.lookingAtInteractable.GetComponent<ItemSlot>();
                    itemBeingLookedAt = slot.storedItem;
                }
                if (PC.lookingAtInteractable.GetComponent<ItemData>()) {
                    itemBeingLookedAt = PC.lookingAtInteractable.GetComponent<ItemData>().connectedItem;
                }
            }
            if (PC.lookingAtInteractable.showText) {
                if (PC.lookingAtInteractable.GetComponent<DNAPC>()) {
                    DNAPC temp = PC.lookingAtInteractable.GetComponent<DNAPC>();
                    if(temp.storedItems[0] == null)
                        interactText.text = "No Sample";
                    else {
                        interactText.text = temp.storedItems[0].NAME;
                    }
                    return;
                }

                if (slot != null && slot.storedItem == null) {
                    interactText.text = "EMPTY";
                    return;
                }

                if (itemBeingLookedAt != null) {

                    interactText.text = itemBeingLookedAt.NAME;
                    return;

                }
                else {
                    interactText.text = "Press 'E' To Interact.";
                    return;
                }
            }
            else {
                interactText.text = "";
            }
            
        }
        else {
            interactText.enabled = false;
        }
    }
}
