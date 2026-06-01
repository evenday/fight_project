using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReserchBox : MonoBehaviour
{
    Enemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            enemy.SetReserchTarget(gameObject, other);

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            enemy.RemoveReserchTarget();
    }

}
