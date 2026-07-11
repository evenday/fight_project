using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] LockOnUI lock_on_ui;
    
    private void Awake()
    {

    
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //UI Draw
        if(player.Lock_On_Target != null)
            lock_on_ui.gameObject.SetActive(true);
        else
            lock_on_ui.gameObject.SetActive(false);

        //Set Ui Pos / image.alpha setting
        if (lock_on_ui.gameObject.activeSelf)
        {
            if (player.B_Lock_On_Target_Setting && player.Mouse_Wheel_Hold_Time >= 0.26f)
                lock_on_ui.alpha = 0.5f;
            else
                lock_on_ui.alpha = 1.0f;

            lock_on_ui.Target_Trans = player.Lock_On_Target.ModelCenterPoint();
        }
        else
            lock_on_ui.Target_Trans = null;



    }
}
