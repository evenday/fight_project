using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;

public class LockOnUI : MonoBehaviour
{
    public Transform Target_Trans { get; set; }
    RectTransform image_size;
    Image image;
    Camera cam;
    public float alpha { get; set; } = 1.0f;


    private void Awake()
    {
        cam = Camera.main;
        image_size = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    private void Start()
    {
        image_size.sizeDelta = new Vector2(10.0f, 10.0f);
        
    }


    // Update is called once per frame
    void Update()
    {
        if (Target_Trans != null)
        {
            transform.position = cam.WorldToScreenPoint(Target_Trans.position);
        }

        image.color = new Color(1.0f, 1.0f, 1.0f, alpha);

    }
}
