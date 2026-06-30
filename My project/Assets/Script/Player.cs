using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;

public class Player : MonoBehaviour, ITakeDamage, IAnimationEvent, ITargetAble
{
    Rigidbody rigid;
    Animator anim;

    //Use
    //CameraController->main_camera.Transform
    //CameraController->mouse_x (Check rotation while moving)
    [SerializeField] CameraController cam_rot;
    Camera main_cam;
    State cur_state = State.Idle;

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
    HitBox hit_box;
    DetectBox detect_box;
    private float hp = 10.0f;
    public Transform Attack_Target { get; private set; } = null;
    [SerializeField] float attack_damage = 1.0f;
    public bool B_Targeting { get; private set; } = false;                               //targeting check


    public float Hp
    {
        get { return hp; }
        set { hp = Mathf.Clamp(value, 0, 10.0f); }
    }


    //aniamtion Running ckeck
    bool b_anim_running = false;




    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        hit_box = GetComponentInChildren<HitBox>();
        detect_box = GetComponentInChildren<DetectBox>();

        main_cam = Camera.main;

    }

    // Start is called before the first frame update
    void Start()
    {
        hp = 10.0f;
        hit_box.Damage = attack_damage;
        hit_box.gameObject.SetActive(false);
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
        Debug.Log("Target Count: " + detect_box.Targets.Count);
        StateManager();



        //=======================================apply========================================================

        //Anim 
        anim.SetFloat("f_cur_speed", rigid.velocity.magnitude);

        cur_lean = Mathf.SmoothDamp(cur_lean, target_lean, ref smooth_vel, smooth_time);


        //Camera X, Y Axis * MoveDirect
        move_dir = GetCamRelativeMoveDirection();

        //Charactor move rotation 
        if (move_dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(move_dir);
        

        //====================================================================================================

        StateAction();

        //Debug.Log("Targeting: " + B_Targeting);

    }

    private void LateUpdate()
    {
        //Lean apply
        lean_pivot.localRotation = lean_pivot.localRotation * Quaternion.Euler(0, 0, cur_lean);


    }

    bool HasMovementInput()
    {
        input_h = Input.GetAxis("Horizontal");
        input_v = Input.GetAxis("Vertical");

        return input_h != 0 || input_v != 0;
    }


    //By Camera forward
    Vector3 GetCamRelativeMoveDirection()
    {
        Vector3 forward_axis = main_cam.transform.forward;
        Vector3 right_axis = main_cam.transform.right;

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

    Transform GetAttackTargetTrans()
    {
        if (detect_box.Targets.Count == 0)
            return null;

        ITargetAble attack_target = null;

        foreach (ITargetAble it in detect_box.Targets)
        {
            if (attack_target == null)
            {
                attack_target = it;
                continue;
            }

            float before_distance = (attack_target.Transform().position - transform.position).magnitude;
            float cur_distance = (it.Transform().position - transform.position).magnitude;

            if (before_distance > cur_distance)
            {
                attack_target = it;
            }

        }
        if (attack_target != null)
            Debug.Log(attack_target.ObjectName());


        return attack_target.Transform();

    }

    Transform GetLockOnTargetTrans()
    {
        if (detect_box.Targets.Count == 0)
        {
            B_Targeting = false;
            return null;
        }

        Vector2 screen_center = new Vector2(Screen.width / 2, Screen.height / 2);
        ITargetAble target = null;


        foreach(ITargetAble it in detect_box.Targets)
        {
            if (target == null)
            {
                target = it;
                continue;
            }

            Vector3 cur_screen_pos = main_cam.WorldToScreenPoint(it.Transform().position);
            Vector3 before_screen_pos = main_cam.WorldToScreenPoint(target.Transform().position);

            //pass behind the Cam
            if (cur_screen_pos.z <= 0) 
                continue;


            float before_target_distance = (new Vector2(before_screen_pos.x, before_screen_pos.y) - screen_center).magnitude;
            float cur_target_distance = (new Vector2(cur_screen_pos.x, cur_screen_pos.y) - screen_center).magnitude;

            if (before_target_distance >= cur_target_distance)
                target = it;
        }

        return target.Transform();
    }

    void StateManager()
    {
        if (b_anim_running)
            return;

        if (Input.GetKeyDown(KeyCode.Mouse2))
            B_Targeting = !B_Targeting;


        if (!B_Targeting)
            Attack_Target = GetAttackTargetTrans();
        else
            Attack_Target = GetLockOnTargetTrans();

        if (HasMovementInput())     //Move
        {
            cur_state = State.Move;

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                cur_state = State.Attack;
                b_anim_running = true;
                anim.SetTrigger("t_hit");
            }

            //run
            if (Input.GetKey(KeyCode.LeftShift))
            {
                cur_state = State.Run;

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    cur_state = State.Attack;
                    b_anim_running = true;
                    anim.SetTrigger("t_hit");
                }

            }

        }
        else if (Input.GetKeyDown(KeyCode.Mouse0))  //hit
        {
            cur_state = State.Attack;
            b_anim_running = true;
            anim.SetTrigger("t_hit");
        }
        else //Idle
        {
            cur_state = State.Idle;
        }



    }

    void StateAction()
    {
        switch (cur_state)
        {
            case State.Idle:
                cur_speed = 0.0f;
                cur_lean = 0.0f;


                break;
            case State.Move:
                cur_speed = walk_speed;
                target_lean = 0.0f;

                break;
            case State.Run:
                cur_speed = run_speed;

                //Lean rotation
                target_lean = GetMouseRotLeanValue(cam_rot.mouse_x);
                break;
            case State.Attack:
                cur_speed = 0.0f;


                if (Attack_Target != null)
                {
                    transform.rotation = Quaternion.LookRotation(Attack_Target.position - transform.position);
                }


                break;
            default:
                break;
        }

    }



    //================================================Damage Interface===========================================================
    public void TakeDamage(float damange)
    {
        hp -= damange;
        Debug.Log(hp);
    }

    //============================================Animation Event Funtion======================================================
    void IAnimationEvent.StartAnimation()
    {

        rigid.velocity = Vector3.zero;
        Debug.Log(b_anim_running);
    }

    public void StartEvent()
    {
        hit_box.gameObject.SetActive(true);
    }

    public void EndEvent()
    {
        hit_box.gameObject.SetActive(false);
    }

    void IAnimationEvent.EndAnimation()
    {
        b_anim_running = false;
        
    }
    //=============================================ITargetAble==============================================

    public Transform Transform()
    {
        return transform;
    }

    public string ObjectName()
    {
        return gameObject.name;
    }

}
