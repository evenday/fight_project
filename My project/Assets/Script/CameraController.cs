using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 0, 0);
    [Header("Target")]
    public Transform target;
    [Header("Rotation Axis")]
    public Transform y_axis;        //Y axis Object.transform
    public Transform x_axis;        //X axis Object.transform
    [Header("Camera")]
    public Transform main_camera;
    public float distance = 12;
    public float camera_velocity = 0.0f;
    public float rot_sensitiv = 1.0f;

    //Zoom
    float zoom_size = 0.0f;     //줌 거리 target
    float smooth_time = 0.3f;
    float current_velocity = 0.0f;

    //rotation
    float mouse_wheel = 0.0f;
    float yaw = 0.0f;                   //right, left
    float pitch = 0.0f;                 //up, down


    void Awake()
    {
        //format Setting
        zoom_size = distance;                                       //set zoom_size(SmoothDamp()-> target)
        main_camera.localPosition = new Vector3(0, 0, -distance);   //Camera distance

        //X axis rotation
        pitch += 20.0f;                                             
        x_axis.localRotation = Quaternion.Euler(pitch, 0, 0);

        //This object position setting
        transform.position = target.position + offset;              

    }

    private void Update()
    {
        //Zoom in / out
        mouse_wheel = Input.GetAxis("Mouse ScrollWheel");

        

    }
    float col_zoom_size = 12;
    void LateUpdate()
    {
        //Camera 
        transform.position = target.position + offset;        //Update this.object position 
        CameraLookTarget();     //Camera Look player
        CameraRotation();
        int layer_mask = LayerMask.GetMask("Floor", "Wall");

        Vector3 ray_dir = main_camera.position - transform.position;
        if (Physics.SphereCast(transform.position, 0.5f, ray_dir, out RaycastHit hit, distance, layer_mask))
        {

            col_zoom_size = hit.distance;
            Debug.Log(col_zoom_size);
            CameraSoothMove(col_zoom_size);
        }
        else
            CameraZoom();           //Camera Zoom


        Debug.DrawRay(transform.position, ray_dir, Color.green);
        //ASFDASF
        
    }

    
    void CameraLookTarget()
    {
        //Camera look target 
        Vector3 camera_dir = transform.position - main_camera.position; //dir camera -> camera_manager
        main_camera.rotation = Quaternion.LookRotation(camera_dir);
        
    }


    void CameraSoothMove(float target_dis)
    {
        distance = Mathf.SmoothDamp(distance, target_dis, ref current_velocity, smooth_time);
        main_camera.localPosition = new Vector3(0, 0, -distance);
    }

    void CameraZoom() 
    {
        zoom_size  -=  mouse_wheel * 10;
        zoom_size = Mathf.Clamp(zoom_size, 1, 15);

        CameraSoothMove(zoom_size);
        

    }


    void CameraRotation()
    {
        float mouse_x = Input.GetAxisRaw("Mouse X") * rot_sensitiv;
        float mouse_y = Input.GetAxisRaw("Mouse Y") * rot_sensitiv;

        //상하 카메라 회전 제한
        pitch = Mathf.Clamp(pitch, -80, 80);
        yaw += mouse_x;
        pitch += mouse_y;

        y_axis.localRotation = Quaternion.Euler(0, yaw, 0);
        x_axis.localRotation = Quaternion.Euler(pitch, 0, 0);

    }
}
