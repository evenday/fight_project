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

        //rigid.velocity = new Vector3(move_dir.x * move_speed, rigid.velocity.y, move_dir.z * move_speed) ;
        //Vector3 point = rigid.position  + move_dir * move_speed * look_camera_dir
        //rigid.MovePosition()
        rigid.velocity = move_dir.normalized * move_speed;

    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        move_dir = main_camera.forward * v +  main_camera.right * h;
        move_dir.y = 0;

        Debug.DrawRay(transform.position, new Vector3(main_camera.forward.x, 0, main_camera.forward.z), Color.red);
        Debug.Log(move_dir.normalized);

    }
    
 
}
