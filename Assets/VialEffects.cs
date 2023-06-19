using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class VialEffects : MonoBehaviour
{
    public GameObject liquid;
    public Color liquidColor;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        setLiquidColor(liquidColor);
    }

    public void setLiquidColor(Color c) {
        liquidColor = c;
        if(liquid != null) {
                
                liquid.GetComponent<MeshRenderer>().material.color = liquidColor;

            
        }
    }
}
