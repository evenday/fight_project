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

public interface ITakeDamage
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
    string ObjectName();            //Debug
}



