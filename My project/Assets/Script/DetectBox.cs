using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectBox : MonoBehaviour
{
    public List<ITargetAble> Targets { get; private set; } = new List<ITargetAble>();

    private void OnTriggerEnter(Collider other)
    {
        ITargetAble target = other.GetComponent<ITargetAble>();

        if(target != null)
            Targets.Add(target);
       

    }
    
    private void OnTriggerExit(Collider other)
    {
        ITargetAble target = other.GetComponent<ITargetAble>();

        if (target != null)
            Targets.Remove(target);


        
    }

    //public T GetTargetComponent<T>() where T : Component
    //{

    //    return Target.TryGetComponent<T>();
    //}

}
