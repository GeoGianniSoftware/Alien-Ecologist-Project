using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarPingUI : MonoBehaviour
{
    public Interactable source;
    public float yOffset;
    Text distanceText;

    // Start is called before the first frame update
    void Start()
    {
        distanceText = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (source != null && source.transform.parent != null) {
            var rt = GetComponent<RectTransform>();
            var screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, source.transform.position + (Vector3.up * yOffset));
            var canvas = GetComponentInParent<Canvas>();
            var canvasRT = canvas.GetComponent<RectTransform>();
            rt.anchoredPosition = screenPoint - canvasRT.sizeDelta / 2f ;
            distanceText.text = "" + Mathf.RoundToInt(Vector3.Distance(source.transform.position, FindObjectOfType<PlayerController>().transform.position)) + "m";

            Vector3 TargetVector = source.transform.position - Camera.main.transform.position;
            Vector3 TargetHorizontalVector = new Vector3(TargetVector.x, 0, TargetVector.z);
            Vector3 CameraHorizontalVector = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
            float angle = Vector3.SignedAngle(CameraHorizontalVector, TargetHorizontalVector, Camera.main.transform.up);

            if (angle > 60) {
                rt.anchoredPosition = new Vector2(canvasRT.sizeDelta.x /2.05f, 0);
            }
            else if (angle < -60) {
                rt.anchoredPosition = new Vector2(-canvasRT.sizeDelta.x / 2.05f, 0);
            }


        }
        else
            Destroy(gameObject);
    }
}
