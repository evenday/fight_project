using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rigid;
    Animator anim;
    [Header("Move Option")]
    public float move_speed = 1.0f;
    Vector3 move_point;
    Vector3 input_vec;
    float v;
    float h;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void FixedUpdate()
    {
        move_point = rigid.position + input_vec * move_speed * Time.fixedDeltaTime;
        rigid.MovePosition(move_point);
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        input_vec = new Vector3(h, 0, v);

        Debug.Log(input_vec.normalized.magnitude);

        //anim.SetFloat("f_Walk", rigid.velocity);


    }
    
 
}
