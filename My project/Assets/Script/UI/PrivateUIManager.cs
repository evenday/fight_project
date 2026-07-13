using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrivateUIManager : MonoBehaviour
{
    [SerializeField] HPBarUI hp_bar_ui;
    [SerializeField] Vector3 UI_Pos = Vector3.zero;
    Enemy target;

    private void Awake()
    {
        target = GetComponentInParent<Enemy>();
    }

    void Start()
    {
        hp_bar_ui.UpdateHPBar(target.status);
        target.TakeDamageEvent += hp_bar_ui.UpdateHPBar;
    }

    // Update is called once per frame
    void Update()
    {
        hp_bar_ui.pos = target.Character_Center_Point.position + UI_Pos;
    }
}
