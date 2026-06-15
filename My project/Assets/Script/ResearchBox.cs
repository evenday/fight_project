using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchBox : MonoBehaviour
{
    public Collider Target { get; private set; } = null;

    public bool Colliding { get; private set; } = false;

    private void OnTriggerStay(Collider other)
    {
        Target = other;
        Colliding = true;
        //Debug.Log("in");
        //Debug.Log(Target.GetType());

    }

    private void OnTriggerExit(Collider other)
    {
        Target = null;
        Colliding = false;
        //Debug.Log("out");
        
    }

    //public T GetTargetComponent<T>() where T : Component
    //{

    //    return Target.TryGetComponent<T>();
    //}

}
