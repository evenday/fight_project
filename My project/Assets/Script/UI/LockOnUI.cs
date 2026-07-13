using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;

public class LockOnUI : MonoBehaviour
{
    public Vector3 pos { get; set; }
    RectTransform image_trans;
    Image image;
    public float alpha { get; set; } = 1.0f;


    private void Awake()
    {
        image_trans = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    private void Start()
    {
        image_trans.sizeDelta = new Vector2(10.0f, 10.0f);
        
    }


    // Update is called once per frame
    void Update()
    {

        transform.position = pos;


        image.color = new Color(1.0f, 1.0f, 1.0f, alpha);

    }
}
