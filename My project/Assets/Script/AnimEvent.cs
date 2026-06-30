using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvent : MonoBehaviour
{
    // Start is called before the first frame update
    IAnimationEvent anim_event;
    private void Awake()
    {
        anim_event = GetComponentInParent<IAnimationEvent>();
    }


    public void StartAnimation()
    {
       

        anim_event.StartAnimation();
    }

    public void StartEvent()
    {
        anim_event.StartEvent();
    }
    
    public void EndEvent()
    {
        anim_event.EndEvent();
    }
    public void EndAnimation()
    {
        anim_event.EndAnimation();
    }

}
