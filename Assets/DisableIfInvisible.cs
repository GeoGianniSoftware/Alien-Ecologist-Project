using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableIfInvisible : MonoBehaviour
{
    bool init = false;
    private void Update() {
        if (GetComponent<CreatureController>() && !init) {

            GetComponent<CreatureController>().preformanceMode = true;
            init = true;
        }
    }

    void OnBecameVisible() {
        if (GetComponent<CreatureController>())
            GetComponent<CreatureController>().preformanceMode = false;
    }

    void OnBecameInvisible() {
        if (GetComponent<CreatureController>())
            GetComponent<CreatureController>().preformanceMode = true;
    }
    
}
