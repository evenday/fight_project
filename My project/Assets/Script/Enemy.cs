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

    //Test
    [SerializeField] GameObject test_obj;

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
    HashSet<Collider> col_check = new HashSet<Collider>();      //search_objects layer check
    [SerializeField] float run_speed = 13.0f;

    //Battle
    [SerializeField] float attack_range = 5.0f;
    [SerializeField] float attack_delay_time = 2.0f;
    Transform target;                                           //Collider->Trigger Set gameobject.transform(tag == Player)
    float accumulated_attack_cooldown = 0.0f;                   //wait after attack
    float target_distance = 0.0f;                               //(target.position - transform.position).magnitude



    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();


    }

    void Start()
    {
        move_dir = RandomDirect();
        pattern_duration_time = Random.Range(pattern_duration_time_min, pattern_duration_time_max);
        accumulated_attack_cooldown = attack_delay_time;
        
        
        test_obj.SetActive(false);

    }

    private void FixedUpdate()
    {
        rigid.velocity = new Vector3(
            move_dir.x * cur_speed,
            rigid.velocity.y,
            move_dir.z * cur_speed);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && !col_check.Add(other))
        {
            target = other.transform;
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (!col_check.Remove(other))
            return;

        if (other.gameObject.tag == "Player"  )
        {
            Debug.Log("exit");
            target = null;
            Debug.Log(target);
        }


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
            target_distance = (target.position - transform.position).magnitude;

            if (target_distance >= attack_range)
                cur_state = State.Chase;
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
                anim.SetFloat("f_cur_speed", cur_speed);

                break;
            case State.Move:

                if (accumulated_time <= 0.0f)
                {
                    move_dir = RandomDirect();
                    Debug.Log(move_dir);
                }
                cur_speed = walk_speed;

                anim.SetFloat("f_cur_speed", cur_speed);
                transform.rotation = Quaternion.LookRotation(move_dir);
                break;

            case State.Chase:

                move_dir = (target.position - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(move_dir);

                anim.speed = 2.0f;
                cur_speed = run_speed;
                anim.SetFloat("f_cur_speed", cur_speed);

                break;
            case State.Attack:
                cur_speed = 0.0f;
                anim.SetFloat("f_cur_speed", cur_speed);

                //attack after delay
                if (accumulated_attack_cooldown >= attack_delay_time)
                {
                    anim.SetTrigger("t_attack");
                    accumulated_attack_cooldown = 0.0f;
                }
                else
                    accumulated_attack_cooldown += Time.deltaTime;

                    break;
            default:
                break;
        }
    }


    //============================================Animation Event Funtion======================================================
    public void AnimAttackStart()
    {
        Debug.Log("Attack start");
        test_obj.SetActive(true);

    }

    public void AnimAttackDropHitBox()
    {

        test_obj.SetActive(false);

    }

    public void AnimAttackEnd()
    {
        Debug.Log("Attack end");

    }

}
