using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Vector3 offset = new Vector3(0, 0, 0);   //CameraManager 시작 위치(target.position + offset)
    [Header("Target")]
    [SerializeField] Player follow_target;                 //target = Player
    [Header("Rotation Axis")]
    [SerializeField] Transform y_axis;                        //Y axis Object.transform -> yaw
    [SerializeField] Transform x_axis;                        //X axis Object.transform -> pitch
    [Header("Camera")]
    Camera main_cam;
    [SerializeField] float distance = 12;
    //public float camera_velocity = 0.0f;
    [SerializeField] float rot_sensitiv = 1.0f;



    //Zoom
    int layer_mask;
    float zoom_pos = 0.0f;                          //Use zoom in/out target position
    float col_zoom_pos = 0.0f;                      //Use collider check Floor or Wall (enter collision position)
    float mouse_wheel = 0.0f;                       //Mouse wheel value -> Input 
    float move_time = 0.3f;                         //Time taken to camera move(SmoothDamp factor)
    float current_velocity = 0.0f;                  //Current camera move velocity(out value)

    //rotation
    public float mouse_x { get; private set; }
    float mouse_y;
    float yaw = 0.0f;                               //right, left
    float pitch = 0.0f;                             //up, down


    void Awake()
    {
        main_cam = Camera.main;

        //format Setting
        zoom_pos = distance;
        main_cam.transform.localPosition = new Vector3(0, 0, -distance);   //Camera distance

        //X axis rotation
        pitch += 20.0f;
        x_axis.localRotation = Quaternion.Euler(pitch, 0, 0);

        //This object position setting
        transform.position = follow_target.transform.position + offset;

        //Collider LayerMask Setting
        layer_mask = LayerMask.GetMask("Floor", "Wall");



    }


    private void Update()
    {
        //rotation Input
        mouse_x = Input.GetAxis("Mouse X") * rot_sensitiv;
        mouse_y = Input.GetAxis("Mouse Y") * rot_sensitiv;

        //Mouse wheel Input
        mouse_wheel = Input.GetAxis("Mouse ScrollWheel");

    }

    void LateUpdate()
    {
        //Camera 
        transform.position = follow_target.transform.position + offset;     //Update this.object position 
        CameraLookTarget();                                                 //Camera Look player
        CameraRotate();

        CameraZoom();                                                       //Camera Zoom

    }


    void CameraLookTarget()
    {
        //Camera look target 
        Vector3 camera_dir; //dir camera -> camera_manager

        camera_dir = transform.position - main_cam.transform.position;

        main_cam.transform.rotation = Quaternion.LookRotation(camera_dir);

    }

    void MouseZoom()
    {
        zoom_pos -= mouse_wheel * 10;
        zoom_pos = Mathf.Clamp(zoom_pos, 2, 15);
    }

    bool CameraCheckColRay(ref float target_pos)
    {
        Vector3 ray_dir = main_cam.transform.position - transform.position;

        //Look Ray
        if (Input.GetKey(KeyCode.Tab))
            Debug.DrawRay(transform.position, ray_dir.normalized * distance, Color.green);

        if (Physics.SphereCast(transform.position, 1.0f, ray_dir.normalized, out RaycastHit hit, distance, layer_mask))
        {
            target_pos = hit.distance;
            return true;
        }
        else
            return false;
    }

    void CameraSoothMove(float target_dis)                              //Apply Camera Move
    {
        distance = Mathf.SmoothDamp(distance, target_dis, ref current_velocity, move_time);
        distance = Mathf.Clamp(distance, 2, 15);
        main_cam.transform.localPosition = new Vector3(0, 0, -distance);
    }

    void CameraZoom()
    {
        //check Camera field out
        if (Physics.CheckSphere(main_cam.transform.position, 0.2f, layer_mask))
            distance -= 1.0f;
        //Physics.CheckSphere(,)
        if (CameraCheckColRay(ref col_zoom_pos))
        {
            CameraSoothMove(col_zoom_pos);
        }
        else
        {
            MouseZoom();
            CameraSoothMove(zoom_pos);
        }
    }

    void RotateLockOn()
    {
        Vector3 dir = follow_target.Lock_On_Target.Transform().position - follow_target.transform.position;

        yaw = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        pitch = -Mathf.Atan2(dir.y, new Vector2(dir.x, dir.z).magnitude) * Mathf.Rad2Deg;
        pitch += 30.0f;
    }

    void CameraRotate()
    {
        if (!follow_target.B_Lock_On_Target_Setting && follow_target.Lock_On_Target != null)
        {
            RotateLockOn();
        }
        else
        {
            yaw += mouse_x;
            pitch += mouse_y;
        }


        //limit pitch angle 
        pitch = Mathf.Clamp(pitch, -40, 80);


        y_axis.localRotation = Quaternion.Euler(0, yaw, 0);         //right left 
        x_axis.localRotation = Quaternion.Euler(pitch, 0, 0);       //up down

    }
}
