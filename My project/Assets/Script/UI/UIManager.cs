using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] LockOnUI lock_on_ui;
    [SerializeField] HPBarUI hp_bar_ui;

    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    
    }
    
    void Start()
    {
        hp_bar_ui.UpdateHPBar(player.status);
        player.TakeDamageEvent += hp_bar_ui.UpdateHPBar;
    }

    // Update is called once per frame
    void Update()
    {

        LockOnUI();


    }


    void LockOnUI()
    {
        //UI Draw
        if (player.Lock_On_Target != null)
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

            lock_on_ui.pos = cam.WorldToScreenPoint(player.Lock_On_Target.CharacterCenterPoint());
        }

        HPBarUI();
    }

    void HPBarUI()
    {
        hp_bar_ui.pos = new Vector3(0.0f, Screen.height, 0.0f);
    }



}

