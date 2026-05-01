using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class Player : MonoBehaviour
{
    Rigidbody rigid;
    Animator anim;

    //Use
    //CameraController->main_camera.Transform
    //CameraController->mouse_x (Check rotation while moving)
    [SerializeField] CameraController cam_data;    


    [Header("Move Option")]
    public float walk_speed = 10.0f;
    public float run_speed = 15.0f;
    Vector3 move_dir;
    float cur_speed = 0.0f;
    float input_v;
    float input_h;

    [Header("rot Option")]
    [SerializeField] Transform lean_pivot;
    [SerializeField] float max_lean = 15.0f;
    float cur_rot_z = 0.0f;
    float target_z = 0.0f;
    float smooth_vel = 0.0f;
    float smooth_time = 0.1f;



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

            if (!anim.GetBool("b_Run"))
                cur_speed = walk_speed;

            anim.SetBool("b_Move", true);
        }
        else
        {
            if (anim.GetBool("b_Run"))
                anim.SetBool("b_Run", false);

            anim.SetBool("b_Move", false);

            //currnet speed init
            cur_speed = 0.0f;
        }

        //Run animation, current speed setting;
        if (Input.GetKeyDown(KeyCode.LeftShift) && anim.GetBool("b_Move"))
        {
            anim.SetBool("b_Run", true);
            
            cur_speed = run_speed;
        }


        //Lean Controller
        if (cur_speed >= run_speed)
        {
            target_z = GetMouseRotLeanValue(cam_data.mouse_x);

            cur_rot_z = Mathf.SmoothDamp(cur_rot_z, target_z, ref smooth_vel, smooth_time);
        }
        else
            target_z = 0.0f;

        lean_pivot.localRotation = Quaternion.Euler(0, 0, cur_rot_z);


        //rotation
        if (move_dir != Vector3.zero && (Input.GetButton("Vertical") || Input.GetButton("Horizontal")))
            transform.rotation = Quaternion.LookRotation(move_dir);

    }



    //By Camera forward
    Vector3 InputMoveDirect()
    {
        Vector3 forward_axis = cam_data.camera_trans.forward;
        Vector3 right_axis = cam_data.camera_trans.right;

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

}
