using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    float damage = 1.0f;
    public float Damage { get { return damage; } set { damage = value; } }


    private void OnTriggerEnter(Collider other)
    {
        ITakeDamage target = other.GetComponent<ITakeDamage>();

        if(target != null)
        {
            Debug.Log(target.GetType());
            target.TakeDamage(damage);
        }

           
    }


}
