using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXChildController : MonoBehaviour
{
    GameObject parentReference;
    // Start is called before the first frame update
    void Start()
    {
        parentReference = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(parentReference == null) {
            Destroy(gameObject);
        }
    }
}
