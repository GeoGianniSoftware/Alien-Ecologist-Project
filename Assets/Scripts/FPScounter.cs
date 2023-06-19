using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPScounter : MonoBehaviour
{
    public float interval;
    float lastTime;
    public float total;
    public int times;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= lastTime + interval) {
            float fps = Mathf.RoundToInt((1.0f / Time.deltaTime));
            times++;
            total += fps;

            GetComponent<Text>().text = "FPS: " + fps + "\n Avg: " + Mathf.RoundToInt(total / times);
            
            lastTime = Time.time;
        }
        
    }
}
