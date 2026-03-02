using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 0, 0);   //CameraManager 시작 위치(target.position + offset)
    [Header("Target")]
    public Transform target;
    [Header("Rotation Axis")]
    public Transform y_axis;                        //Y axis Object.transform
    public Transform x_axis;                        //X axis Object.transform
    [Header("Camera")]
    public Transform main_camera;
    public float distance = 12;
    //public float camera_velocity = 0.0f;
    public float rot_sensitiv = 1.0f;

    //Zoom
    int layer_mask;
    float zoom_pos = 0.0f;                          //Use zoom in/out target position
    float col_zoom_pos = 0.0f;                      //Use collider check Floor or Wall (enter collision position)
    float mouse_wheel = 0.0f;                       //Mouse wheel value -> Input 
    float move_time = 0.3f;                         //Time taken to camera move(SmoothDamp factor)
    float current_velocity = 0.0f;                  //Current camera move velocity(out value)

    //rotation
    float mouse_x;
    float mouse_y;
    float yaw = 0.0f;                               //right, left
    float pitch = 0.0f;                             //up, down


    void Awake()
    {
        //format Setting
        zoom_pos = distance;                                                                        
        main_camera.localPosition = new Vector3(0, 0, -distance);   //Camera distance

        //X axis rotation
        pitch += 20.0f;                                             
        x_axis.localRotation = Quaternion.Euler(pitch, 0, 0);       

        //This object position setting
        transform.position = target.position + offset;

        //Collider LayerMask Setting
        layer_mask = LayerMask.GetMask("Floor", "Wall");

    }

    private void Update()
    {
        //rotation Input
        mouse_x = Input.GetAxisRaw("Mouse X") * rot_sensitiv;
        mouse_y = Input.GetAxisRaw("Mouse Y") * rot_sensitiv;

        //Mouse wheel Input
        mouse_wheel = Input.GetAxis("Mouse ScrollWheel");


    }
    
    void LateUpdate()
    {
        //Camera 
        transform.position = target.position + offset;          //Update this.object position 
        CameraLookTarget();                                     //Camera Look player
        CameraRotation();

        CameraZoom();                                           //Camera Zoom

    }

    
    void CameraLookTarget()
    {
        //Camera look target 
        Vector3 camera_dir = transform.position - main_camera.position; //dir camera -> camera_manager
     
        main_camera.rotation = Quaternion.LookRotation(camera_dir);
        
    }

    void MouseZoom()
    {
        zoom_pos -= mouse_wheel * 10;
        zoom_pos = Mathf.Clamp(zoom_pos, 2, 15);
    }

    bool ColZoom()
    {
        Vector3 ray_dir = main_camera.position - transform.position;
        Debug.DrawRay(transform.position, ray_dir.normalized * (distance + 3.0f), Color.green);

        if (Physics.SphereCast(transform.position, 1.0f, ray_dir.normalized, out RaycastHit hit, distance + 3.0f, layer_mask))
        {
            col_zoom_pos = hit.distance - 3.0f;
            return true;
        }
        else
            return false;
    }

    void CameraSoothMove(float target_dis)                              //Apply Camera Move
    {
        distance = Mathf.SmoothDamp(distance, target_dis, ref current_velocity, move_time);
        distance = Mathf.Clamp(distance, 2, 15);
        main_camera.localPosition = new Vector3(0, 0, -distance);
    }

    void CameraZoom() 
    {
        if (Physics.CheckSphere(main_camera.position, 0.5f, layer_mask))
            distance -= 1.0f;

        if (ColZoom())
        {
            CameraSoothMove(col_zoom_pos);
        }
        else
        {
            MouseZoom();
            CameraSoothMove(zoom_pos);
        }    
    }


    void CameraRotation()
    {
        //상하 카메라 회전 제한
        pitch = Mathf.Clamp(pitch, -40, 80);
        yaw += mouse_x;
        pitch += mouse_y;

        y_axis.localRotation = Quaternion.Euler(0, yaw, 0);
        x_axis.localRotation = Quaternion.Euler(pitch, 0, 0);

    }
}
