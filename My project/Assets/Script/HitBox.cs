using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public string Target_tag = "";
    public float Damage = 0.0f;


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(Target_tag))
            return;

        

           
    }


}
