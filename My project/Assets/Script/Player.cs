using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class Player : MonoBehaviour
{
    Rigidbody rigid;
    //Animator anim;

    public Transform main_camera;

    [Header("Move Option")]
    public float move_speed = 1.0f;
    Vector3 move_dir;
    float v;
    float h;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        //anim = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void FixedUpdate()
    {
        rigid.MoveRotation(Quaternion.LookRotation(move_dir));
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
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        move_dir = InputMoveDirect();
        //Debug.DrawRay(transform.position, move_dir, Color.red);
        //Debug.Log(move_dir);   
    }

    Vector3 InputMoveDirect()
    {
        Vector3 forward_axis = main_camera.forward;
        Vector3 right_axis = main_camera.right;

        forward_axis.y = 0;
        right_axis.y = 0;

        forward_axis.Normalize();
        right_axis.Normalize();

        return forward_axis * v + right_axis * h;
    }
 
}
