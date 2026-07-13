using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour, ITakeDamageAble, IAnimationEvent, ITargetAble
{
    Rigidbody rigid;
    Animator anim;
    public CharacterStatus status;
    //Control action values
    [SerializeField] State cur_state; 
    [SerializeField] float pattern_duration_time_max = 5.0f;    //Random time Max
    [SerializeField] float pattern_duration_time_min = 1.0f;    //Rnadom time min
    float accumulated_time = 0.0f;                              //Current time
    float pattern_duration_time = 0.0f;                         //pattern duration time (compare accumlated_time)
    float cur_speed = 0.0f;
    bool anim_running = false;

    //Move pattern
    Vector3 move_dir;                                   //if not chase state move direction
    [SerializeField] float walk_speed = 9.0f;
    [SerializeField] float run_speed = 13.0f;


    //Battle
    HitBox hit_box;
    [SerializeField] float attack_cooldown = 2.0f;
    [SerializeField] float attack_damage = 1.0f;

    float attack_wait_time = 0.0f;                      //wait after attack

    //Research
    DetectBox[] detect_boxs;
    Transform target = null;                                       //Collider->Trigger Set gameobject.transform(tag == Player)
    [SerializeField] float attack_range = 5.0f;
    Vector3 target_distance = Vector3.zero;                     //(target.position - transform.position).magnitude

    public Transform Character_Center_Point;

    public event System.Action<CharacterStatus> TakeDamageEvent;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        hit_box = GetComponentInChildren<HitBox>();
        detect_boxs = GetComponentsInChildren<DetectBox>();
    }

    void Start()
    {
        move_dir = RandomDirect();
        pattern_duration_time = Random.Range(pattern_duration_time_min, pattern_duration_time_max);
        attack_wait_time = attack_cooldown;
        hit_box.Damage = attack_damage;
        hit_box.gameObject.SetActive(false);
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
        if (attack_wait_time < attack_cooldown)
        {
            attack_wait_time += Time.deltaTime;
            Debug.Log(attack_wait_time);
        }

            
        StateManager();
        //StateAction();
        accumulated_time += Time.deltaTime;

       
    }



    Vector3 RandomDirect()
    {
        return new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f)).normalized;
    }




    void StateManager()
    {
        if (anim_running)
            return;

        target = SetResearchBoxTarget();

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

            Debug.Log(target);
        }
        else if (target != null)
        {
            target_distance = target.position - transform.position;

            if (target_distance.magnitude >= attack_range)
            {
                cur_state = State.Chase;
            }
            else if(target_distance.magnitude <= attack_range && attack_wait_time >= attack_cooldown)
            {
                anim.SetTrigger("t_hit");
                cur_state = State.Attack;

            }
            else
            {
                cur_state = State.Idle;
            }


            Debug.Log(target.gameObject.name);
        }
        else
        {
            //TODO: target = wall
        }


    }
    void StateAction()
    {
        switch (cur_state)
        {
            case State.Idle:
                cur_speed = 0.0f;

                break;
            case State.Move:

                if (accumulated_time <= 0.0f)
                {
                    move_dir = RandomDirect();
                    Debug.Log(move_dir);
                }
                cur_speed = walk_speed;

                transform.rotation = Quaternion.LookRotation(move_dir);
                break;

            case State.Chase:

                move_dir = target_distance.normalized;
                transform.rotation = Quaternion.LookRotation(move_dir);

                anim.speed = 2.0f;
                cur_speed = run_speed;


                break;
            case State.Attack:
                cur_speed = 0.0f;

                anim_running = true;
                attack_wait_time = 0.0f;


                break;
            default:
                break;
        }

        anim.SetFloat("f_cur_speed", cur_speed);

    }

    //==========================================Reserch Box Manger Function====================================================

    Transform SetResearchBoxTarget() 
    {
        foreach (DetectBox db in detect_boxs)
        {
            //todo 여기서 Target값이 안잡힘 ResearchBox쪽 확인
            if (db.Targets.Count == 0)
                continue;

            return db.Targets[0].Transform();

        }

        return null;
    }

    //================================================Damage Interface===========================================================
    public void TakeDamage(float damage)
    {
        status.cur_hp -= damage;
        TakeDamageEvent?.Invoke(status);
        //Debug.Log(hp);
    }

    //============================================Animation Event Funtion======================================================

    public void StartAnimation()
    {
        Debug.Log("Attack start");

    }

    public void StartEvent()
    {
        hit_box.gameObject.SetActive(true);

    }


    public void EndEvent()
    {
        hit_box.gameObject.SetActive(false);

    }

    public void EndAnimation()
    {
        anim_running = false;
    }

    //==============================================ITargetAble===================================================
    public Transform Transform()
    {
        return transform;
    }

    public Vector3 CharacterCenterPoint()
    {
        return Character_Center_Point.position; 
    }

    public string ObjectName()
    {
        
        return gameObject.name;
    }

}
