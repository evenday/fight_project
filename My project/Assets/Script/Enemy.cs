using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    enum State
    {
        Idle,
        Move,
        Chase,
        Attack
    };


    Rigidbody rigid;
    Animator anim;

    //Control action values
    [SerializeField] State cur_state;
    [SerializeField] float pattern_duration_time_max = 5.0f;    //Random time Max
    [SerializeField] float pattern_duration_time_min = 1.0f;    //Rnadom time min
    float accumulated_time = 0.0f;                              //Current time
    float pattern_duration_time = 0.0f;                         //pattern duration time (compare accumlated_time)
    float cur_speed = 0.0f;


    //Move pattern
    Vector3 move_dir;                                   //if not chase state move direction
    [SerializeField] float walk_speed = 9.0f;

    //Chase
    [SerializeField] float run_speed = 13.0f;

    //Battle
    [SerializeField] float hit_range = 5.0f;
    [SerializeField] float hit_delay_time = 2.0f;
    Player target = null;                                    //Collider->Trigger Set gameobject.transform(tag == Player)
    Vector3 target_distance = Vector3.zero;                               //(target.position - transform.position).magnitude
    float accumulated_hit_cooldown = 0.0f;                      //wait after attack
    public bool b_hit { get; private set; } = false;
    HitBox hit_box;
    

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        hit_box = GetComponentInChildren<HitBox>();

    }

    void Start()
    {
        move_dir = RandomDirect();
        pattern_duration_time = Random.Range(pattern_duration_time_min, pattern_duration_time_max);
        accumulated_hit_cooldown = hit_delay_time;

    }

    private void FixedUpdate()
    {
        rigid.velocity = new Vector3(
            move_dir.x * cur_speed,
            rigid.velocity.y,
            move_dir.z * cur_speed);
    }




    void Update()
    {
        State_Manager();

        t_PatternExe();



        //Debug.Log("chanage_pattern_time " + change_pattern_time);
        //Debug.Log("accumulated_time " + accumulated_time);
        //Debug.Log("State: " + cur_state);

        accumulated_time += Time.deltaTime;

        

    }



    Vector3 RandomDirect()
    {
        return new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f)).normalized;
    }




    void State_Manager()
    {

        if (target == null)
        {
            if (cur_state != State.Idle && cur_state != State.Move)
            {
                cur_state = State.Idle;

                accumulated_time = 0.0f;
                pattern_duration_time = Random.Range(pattern_duration_time_min, pattern_duration_time_max);
            }

            if (accumulated_time >= pattern_duration_time)
            {
                if (cur_state == State.Idle)
                    cur_state = State.Move;
                else if (cur_state == State.Move)
                    cur_state = State.Idle;
                
                accumulated_time = 0.0f;
                pattern_duration_time = Random.Range(pattern_duration_time_min, pattern_duration_time_max);
            }
        }
        else if (target.tag == "Player")
        {
            target_distance = target.transform.position - transform.position;

            if (target_distance.magnitude >= hit_range)
            {
                cur_state = State.Chase;
                accumulated_hit_cooldown = hit_delay_time;
            }
            else
            {
                cur_state = State.Attack;

            }



        }
        else
        {
            //TODO: target = wall
        }



    }
    void t_PatternExe()
    {
        switch (cur_state)
        {
            case State.Idle:
                cur_speed = 0.0f;
                //anim.SetFloat("f_cur_speed", cur_speed);

                break;
            case State.Move:

                if (accumulated_time <= 0.0f)
                {
                    move_dir = RandomDirect();
                    Debug.Log(move_dir);
                }
                cur_speed = walk_speed;

                //anim.SetFloat("f_cur_speed", cur_speed);
                transform.rotation = Quaternion.LookRotation(move_dir);
                break;

            case State.Chase:

                move_dir = target_distance.normalized;
                transform.rotation = Quaternion.LookRotation(move_dir);

                anim.speed = 2.0f;
                cur_speed = run_speed;
                //anim.SetFloat("f_cur_speed", cur_speed);

                break;
            case State.Attack:
                cur_speed = 0.0f;
                //anim.SetFloat("f_cur_speed", cur_speed);

                //attack after delay
                if (accumulated_hit_cooldown >= hit_delay_time)
                {
                    anim.SetTrigger("t_hit");
                    accumulated_hit_cooldown = 0.0f;
                }
                else
                    accumulated_hit_cooldown += Time.deltaTime;

                break;
            default:
                break;
        }

        anim.SetFloat("f_cur_speed", cur_speed);

    }

    //==========================================Reserch Box Manger Function====================================================

    public void SetReserchTarget(GameObject obj, Collider col_target)
    {
        if (target != null)
            return;

        target = col_target.GetComponent<Player>() ;
    }

    public void RemoveReserchTarget()
    {
        if (target == null)
            return;

        target = null;
    }


    //================================================Hit Box Function=========================================================


    public void HitDamage()
    {
        target.Hp -= 1;
    }




    //============================================Animation Event Funtion======================================================
    public void AnimAttackStart()
    {
        Debug.Log("Attack start");
        hit_box.gameObject.SetActive(true);

    }

    public void AnimAttackDropHitBox()
    {

        hit_box.gameObject.SetActive(false);
            
    }

    public void AnimAttackEnd()
    {
        Debug.Log("Attack end");

    }


}
