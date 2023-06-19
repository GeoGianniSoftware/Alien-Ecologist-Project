using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    PlayerController PC;
    public int toolbeltSize;
    public int inventorySize;


    [ReadOnly]
    public int currentToolSlot;
    public Item itemBeingHeld;
    int itemBeingHeldSlot;
    public Weapon currentWeapon;
    public BaseAbility currentAbility;

    public List<Buff> currentBuffs = new List<Buff>();

    public Item[] inventory;
    public ToolItem[] toolbelt;


    // Start is called before the first frame update
    void Start()
    {
        PC = GetComponent<PlayerController>();
        inventory = new Item[inventorySize];
        toolbelt = new ToolItem[toolbeltSize];
        
    }

    // Update is called once per frame
    void Update()
    {
        SetCurrentWeapon();
    }

    public void HoldItem(int itemSlot) {
        if(inventory[itemSlot] != null) {

            itemBeingHeld = inventory[itemSlot];
            itemBeingHeldSlot = itemSlot;
        }
    }

    public void ClearHeldItem() {
        itemBeingHeld = null;
        itemBeingHeldSlot = -1;
    }

    int lastSlot;
    void SetCurrentWeapon() {
        if(currentToolSlot >= 0 && currentToolSlot < toolbeltSize && toolbelt[currentToolSlot] != null ) {
            if (currentWeapon != null && currentWeapon.identity != toolbelt[currentToolSlot].weaponRef.identity && lastSlot == currentToolSlot) {
                lastSlot = currentToolSlot;
                return;
            }
                

            EquipWeapon(toolbelt[currentToolSlot].weaponRef);
        }
        else {
            if (currentToolSlot < 0 && currentWeapon != null) {
                print("Dequip Weapon");
                DequipWeapon();
                return;
            }


            if (currentToolSlot >= 0 && toolbelt[currentToolSlot] != null) {
                EquipWeapon(toolbelt[currentToolSlot].weaponRef);
                return;
            }
            else if (currentToolSlot >= 0 && toolbelt[currentToolSlot] == null) {
                DequipWeapon();
                currentToolSlot = -1;
                return;
            }
            else if (currentToolSlot < 0)
                currentToolSlot = -1;
        }
            
        

    }

    bool InventoryContains(Item itemToCheckFor) {
        foreach(Item i in inventory) {
            if(i == itemToCheckFor) {
                return true;
            }
        }
        return false;
    }


    public void RemoveItem(int placeToRemove, int slotToRemove) {
        if(placeToRemove == 0) {
            if(inventory[slotToRemove] != null) {
                inventory[slotToRemove] = null;
            }
        }
        else if (placeToRemove == 1) {
            if (toolbelt[slotToRemove] != null) {
                toolbelt[slotToRemove] = null;
                
            }
        }
    }

    int getIndexOfItem(Item itemToCheckFor) {
        for (int i = 0; i < inventory.Length; i++) {
            if (inventory[i] == itemToCheckFor)
                return i;
        }
        return -1;
    }

    public int getNextEmpty() {
        for (int i = 0; i < inventory.Length; i++) {
            if (inventory[i] == null)
                return i;
        }
        return -1;
    }

    int getNextEmptyTool() {
        for (int i = 0; i < toolbelt.Length; i++) {
            if (toolbelt[i] == null)
                return i;
        }
        return -1;
    }

    public int getItemBeingHeldSlot() {
        return itemBeingHeldSlot;
    }

    public Item transferItem(Item itemToTransfer) {
        if (InventoryContains(itemToTransfer)) {
            Item temp = itemToTransfer;
            inventory[getIndexOfItem(itemToTransfer)] = null;
            if(itemBeingHeld == itemToTransfer && itemBeingHeld != null) {
                itemBeingHeld = null;
            }
            return temp;
        }
        return null;
    }

    public void addItem(Item itemToAdd) {
        if(itemToAdd != null) {
            if (itemToAdd.TYPE == ItemType.SAMPLE && ((SampleItem)itemToAdd).sampleOf == null)
                return;

            if(itemToAdd.TYPE == ItemType.TOOL) {
                addTool(((ToolItem)itemToAdd));
            }
            else if(getNextEmpty() != -1){

                inventory[getNextEmpty()] = (itemToAdd);
            }
        }
    }

    public void addItem(Item itemToAdd, int slot) {
        if (itemToAdd != null) {
            if (itemToAdd.TYPE == ItemType.SAMPLE && ((SampleItem)itemToAdd).sampleOf == null)
                return;

            if (inventory[slot] == null) {

                inventory[slot] = (itemToAdd);
            }
        }
    }

    public void addTool (ToolItem itemToAdd, int slot) {
        if (itemToAdd != null) {
            if (itemToAdd.TYPE == ItemType.TOOL && (itemToAdd.weaponRef == null))
                return;

            if (toolbelt[slot] == null) {

                toolbelt[slot] = (itemToAdd);
            }
        }
    }

    public void addTool(ToolItem itemToAdd) {
        if (itemToAdd != null) {
            if (itemToAdd.TYPE == ItemType.TOOL && (itemToAdd.weaponRef == null))
                return;

            if (getNextEmptyTool() != -1) {
                toolbelt[getNextEmptyTool()] = ((ToolItem)itemToAdd);
            }
            else {

                toolbelt[getNextEmpty()] = (itemToAdd);
            }
        }
    }

    int lastEquip = -1;
    public void EquipWeapon(Weapon weaponToEquip) {
        
        currentWeapon = Weapon.CreateInstance(weaponToEquip);
        currentWeapon.Initialize(this.gameObject);
        lastEquip = currentToolSlot;
        if (currentWeapon.weaponType == WeaponType.RANGED) {
            ((RangedWeapon)currentWeapon).setProjectileSpawn(PC.senses.projectileSpawn);
        }
    }

    public void DequipWeapon() {
        
        currentWeapon = null;
    }

    public void SetToolbeltSlot(int slot) {
        if (slot > 0 && slot < toolbeltSize-1 && toolbelt[slot] == null)
            return;

        currentToolSlot = slot;
    }

    public int getNextToolSlot(int dir) {
        

        for (int i = currentToolSlot+dir; i < toolbelt.Length; i+= dir) {
            if (i < 0)
                return toolbelt.Length-1;
            if (i > toolbelt.Length-1)
                return 0;
            if (toolbelt[i] != null)
                return i;
        }
        return -1;
    }

    #region Buffs
    public void AddBuff(Buff buffToAdd) {
        currentBuffs.Add(Instantiate(buffToAdd));
        if (buffToAdd.buffType == BuffType.JUMP) {
            PC.jumpBuff += buffToAdd.amt;
        }
        else if (buffToAdd.buffType == BuffType.SPEED) {
            PC.speedBuff += buffToAdd.amt;
        }
        else if (buffToAdd.buffType == BuffType.DAMAGE) {
            PC.damageBuff += (int)buffToAdd.amt;
        }
    }

    public void RemoveBuff(Buff buffToRemove) {
        if (currentBuffs.Contains(buffToRemove)) {
            if (buffToRemove.buffType == BuffType.JUMP) {
                PC.jumpBuff -= buffToRemove.amt;
            }
            else if (buffToRemove.buffType == BuffType.SPEED) {
                PC.speedBuff -= buffToRemove.amt;
            }
            else if (buffToRemove.buffType == BuffType.DAMAGE) {
                PC.damageBuff -= (int)buffToRemove.amt;
            }

            currentBuffs.Remove(buffToRemove);
        }

    }
}



#endregion

