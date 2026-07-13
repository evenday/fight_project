using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarUI : MonoBehaviour
{
    Slider hp_bar;

    public Vector3 pos { get; set; } = Vector3.zero;

    private void Awake()
    {
        hp_bar = GetComponent<Slider>();

    }

    private void Start()
    {
        hp_bar.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = pos;

    }


    public void UpdateHPBar(CharacterStatus status)
    {
        hp_bar.maxValue = status.max_hp;
        hp_bar.value = status.cur_hp;
    }

}
