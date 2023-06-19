using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool showText = true;
    public virtual void Interact() {

    }

    public void OnMouseOver() {
        FindObjectOfType<PlayerController>().setInteractable(this);
    }

    private void OnMouseExit() {
        FindObjectOfType<PlayerController>().clearInteractable();
    }
}
