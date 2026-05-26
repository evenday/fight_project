using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Anim_Clip : MonoBehaviour
{
    Enemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AttackStart()
    {
        enemy.AnimAttackStart();
    }

    public void AttackDorpHitBox()
    {
        enemy.AnimAttackDropHitBox();
    }


    public void AttackEnd()
    {
        enemy.AnimAttackEnd();
    }
}
