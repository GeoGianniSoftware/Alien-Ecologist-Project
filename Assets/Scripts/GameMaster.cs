using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public Slider timeSlider;

    public bool preformanceMode;


    // Start is called before the first frame update
    void Start()
    {
        timeSlider.maxValue = 5;
        timeSlider.minValue = 1;
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = timeSlider.value;
    }
}
