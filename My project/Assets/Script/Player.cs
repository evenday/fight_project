using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class Player : MonoBehaviour, ITakeDamage
{
    Rigidbody rigid;
    Animator anim;

    //Use
    //CameraController->main_camera.Transform
    //CameraController->mouse_x (Check rotation while moving)
    [SerializeField] CameraController cam_data;    


    [Header("Move Option")]
    [SerializeField] float walk_speed = 10.0f;
    [SerializeField] float run_speed = 20.0f;
    Vector3 move_dir;
    float cur_speed = 0.0f;
    float input_v;
    float input_h;

    [Header("Lean Rot Option")]
    [SerializeField] Transform lean_pivot;
    [SerializeField] float max_lean = 15.0f;
    float cur_lean = 0.0f;
    float target_lean = 0.0f;
    float smooth_vel = 0.0f;
    float smooth_time = 0.1f;

    //Battle Setting Values
    private float hp = 10.0f;

    public float Hp
    {
        get { return hp; }
        set { hp = Mathf.Clamp(value, 0, 10.0f); }
    }


    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        hp = 10.0f;
    }

    void FixedUpdate()
    {
        //Move
        rigid.velocity = new Vector3
            (
                move_dir.x * cur_speed,
                rigid.velocity.y,
                move_dir.z * cur_speed
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

        //Walk animation, current speed setting
        if (Input.GetButton("Vertical") || Input.GetButton("Horizontal"))
        {
            cur_speed = walk_speed;

            //run
            if (Input.GetKey(KeyCode.LeftShift))
            {
                cur_speed = run_speed;

                //Lean rotation
                target_lean = GetMouseRotLeanValue(cam_data.mouse_x);


                if(Input.GetKeyDown(KeyCode.Mouse0))
                {
                    cur_speed = 0.0f;

                    anim.SetTrigger("t_hit");
                }



            }
            else
                target_lean = 0.0f;        //lean init
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0))                              //hit
        {
            cur_speed = 0.0f;

            anim.SetTrigger("t_hit");

        }
        else
        {
            cur_speed = 0.0f;   //speed init
            target_lean = 0.0f;        //lean init

        }





        //=======================================apply========================================================

        //Anim 
        anim.SetFloat("f_cur_speed", rigid.velocity.magnitude);

        cur_lean = Mathf.SmoothDamp(cur_lean, target_lean, ref smooth_vel, smooth_time);


        //Charactor move rotation 
        if (move_dir != Vector3.zero && (Input.GetButton("Vertical") || Input.GetButton("Horizontal")))
            transform.rotation = Quaternion.LookRotation(move_dir);

        
        //====================================================================================================
  

    }

    private void LateUpdate()
    {
        //Lean apply
        lean_pivot.localRotation = lean_pivot.localRotation * Quaternion.Euler(0, 0, cur_lean);


    }

    //By Camera forward
    Vector3 InputMoveDirect()
    {
        Vector3 forward_axis = cam_data.Camera_Trans.forward;
        Vector3 right_axis = cam_data.Camera_Trans.right;

        forward_axis.y = 0;
        right_axis.y = 0;

        forward_axis.Normalize();
        right_axis.Normalize();

        return (forward_axis * input_v + right_axis * input_h).normalized;
    }
 
    float GetMouseRotLeanValue(float Mouse_rot_value)
    {
        if (Mouse_rot_value > 0.01f)
        {
            return -max_lean;
        }
        else if (Mouse_rot_value < -0.01f)
        {
            return max_lean;
        }
        else
            return  0.0f;
    }

    public void TakeDamage(float damange)
    {
        hp -= damange;
        Debug.Log(hp);
    }
}
