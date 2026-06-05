using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchBox : MonoBehaviour
{

    public string Target_tag = "";
    public Collider Target { get; private set; } = null;

    public bool Colliding { get; private set; } = false;

    private void OnTriggerStay(Collider other)
    {
        if (Target_tag == "")
        {
            Debug.Log("ResearchBox not setting \"target_tag\" : station" + gameObject.name);
            return;
        }

        if (!other.CompareTag(Target_tag))
            return;

        Target = other;
        Colliding = true;
        Debug.Log("in");


    }

    private void OnTriggerExit(Collider other)
    {
        if (Target_tag != "" && !other.CompareTag(Target_tag))
            return;

        Target = null;
        Colliding = false;
        Debug.Log("out");
        
    }

    public T GetTargetComponent<T>() where T : Component
    {

        return Target.GetComponent<T>();
    }

    public void GetObjectNameLog()
    {
        Debug.Log(gameObject.name);
    }

}
