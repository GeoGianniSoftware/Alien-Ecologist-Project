using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentLocker : ItemContainer
{
    public bool isOpen = true;
    public Vector3 closedSize;
    public Vector3 closedCenter;
    public Vector3 openSize;
    public Vector3 openCenter;
    BoxCollider boxCol;

    private void Awake() {
        if (boxCol == null) {
            boxCol = GetComponent<BoxCollider>();
            boxCol.center = openCenter;
            boxCol.size = openSize;
        }
    }

    public override void Interact() {
        
        isOpen = !isOpen;
        print("Cabinet!");
        GetComponent<Animator>().SetBool("isOpen", isOpen);
        switch (isOpen) {
            case false:
                boxCol.center = openCenter;
                boxCol.size = openSize;
                break;
            case true:
                boxCol.center = closedCenter;
                boxCol.size = closedSize;
                break;
        }
    }

}
