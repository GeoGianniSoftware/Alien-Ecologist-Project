using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAtInterval : MonoBehaviour
{
    AudioSource AS;
    public AudioClip clip;
    public float interval;
    float lastTime;
    public float maxDistance;
    bool started = false;
    // Start is called before the first frame update
    void Start()
    {
        AS = GetComponent<AudioSource>();
        AS.maxDistance = maxDistance;

    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= lastTime + interval) {
            AS.PlayOneShot(clip);
            lastTime = Time.time;
        }
    }
}
