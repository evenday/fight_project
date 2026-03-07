using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rigid;

    float v;
    float h;



    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float v = Input.GetAxis("Vertical"); 
        float h = Input.GetAxis("Horizontal");
        
       

    }

    private void FixedUpdate()
    {
        
        

    }
}
