using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum State
{
    Idle,
    Move,
    Run,
    Chase,
    Attack
};

[System.Serializable]
public struct CharacterStatus
{
    public float max_hp;
    public float cur_hp;

}


enum InputState
{
    Down,
    Hold,
    Up
}


public interface ITakeDamageAble
{
    void TakeDamage(float damage);

}

public interface IAnimationEvent
{
    void StartAnimation();
    void StartEvent();
    void EndEvent();
    void EndAnimation();

}

public interface ITargetAble
{
    Transform Transform();

    Vector3 CharacterCenterPoint();
    
    string ObjectName();            //Debug

}





