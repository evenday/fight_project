using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class Player : MonoBehaviour
{
    Rigidbody rigid;
    Animator anim;

    public Transform main_camera;

    [Header("Move Option")]
    public float move_speed = 1.0f;
    Vector3 move_dir;
    float input_v;
    float input_h;


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
        //character rotation
        if (Input.GetButton("Vertical") || Input.GetButton("Horizontal"))
            rigid.MoveRotation(Quaternion.LookRotation(move_dir));
        
        //Move
        rigid.velocity = new Vector3
            (
                move_dir.x * move_speed,
                rigid.velocity.y,
                move_dir.z * move_speed
            );

    }

    // Update is called once per frame
    void Update()
    {
        //Input Key
        input_h = Input.GetAxis("Horizontal");
        input_v = Input.GetAxis("Vertical");

        //diraction    
        move_dir = InputMoveDirect();

        //animation
        if (Input.GetButton("Vertical") || Input.GetButton("Horizontal"))
        {
            Debug.Log("Not" );
            anim.SetBool("b_Move", true);
        }
        else
        {
            anim.SetBool("b_Move", false);
        }
        
    }

    //By Camera forward
    Vector3 InputMoveDirect()
    {
        Vector3 forward_axis = main_camera.forward;
        Vector3 right_axis = main_camera.right;

        forward_axis.y = 0;
        right_axis.y = 0;

        forward_axis.Normalize();
        right_axis.Normalize();

        return (forward_axis * input_v + right_axis * input_h).normalized;
    }
 
}
