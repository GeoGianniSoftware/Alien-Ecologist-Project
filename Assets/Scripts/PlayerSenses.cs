using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSenses : MonoBehaviour
{
    public Transform projectileSpawn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool canSee(GameObject objectToFind) {
        Vector3 pos = Camera.main.transform.position;
        Vector3 dir = (this.transform.position - Camera.main.transform.position).normalized;

        Debug.DrawLine(pos, pos+(dir * 5));
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, dir, out hit)){
            return true;
        }
        return false;
    }
}
