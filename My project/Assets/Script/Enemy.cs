using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //[SerializeField] float move_speed = 5.0f;
    [SerializeField] float change_random_time_max = 5.0f;
    [SerializeField] float change_random_time_min = 1.0f;
    [SerializeField] float walk_speed = 9.0f;
    [SerializeField] float run_speed = 13.0f;
    HashSet<Collider> col_check = new HashSet<Collider>();  //search_objects layer check

    Rigidbody rigid;
    Animator anim;
    Transform target;                                   //Trigger Collider gameobject.transform
    Vector3 move_dir;
    float accumulated_time = 0.0f;                      //Current time
    float chanage_pattern_time = 0.0f;                  //Next pattern change time (compare accumlated_time)
    float cur_speed = 0.0f;
    bool action = true;

    bool chase = false;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        move_dir = RandomDirect();
        chanage_pattern_time = Random.Range(change_random_time_min, change_random_time_max);

    }

    void Start()
    {
        
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
        accumulated_time += Time.deltaTime;

        //anim.enabled = false;
        PatternExe( accumulated_time, ref chanage_pattern_time, ref action);

        //walk animation
        anim.SetFloat("f_move_speed", rigid.velocity.magnitude);


        Debug.Log("chanage_pattern_time " + chanage_pattern_time);
        Debug.Log("accumulated_time " + accumulated_time);




    }

    private void OnTriggerStay(Collider other)
    {
        if (!col_check.Add(other))
            return;

        if (other.gameObject.tag == "Player")
        {
            action = true;
            chase = true;

            target = other.transform;
        }


        
    }

    private void OnTriggerExit(Collider other)
    {

        if (!col_check.Remove(other))
            return;

        if (other.gameObject.tag == "Player")
        {

            Debug.Log("out");
            chase = false;
            action = false;
        }
        

    }
    

    Vector3 RandomDirect()
    {
        return new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f)).normalized;
    }

    //Init change_pattern_time, move_dir(Random)
    void InitValue(float Cur_Time, ref float Change_time)
    {
        Change_time = Random.Range(change_random_time_min, change_random_time_max);

        if (!chase)
            move_dir = RandomDirect();

        accumulated_time = 0.0f;
    }

    //
    void ChangeActionPattern(float Cur_time, ref float Change_time, ref bool Action)
    {
        if (Cur_time <= Change_time || chase)
            return;

        InitValue(Cur_time, ref Change_time);

        Action = !Action;
    }

    //set move_speed, rotation
    void PatternExe(float Cur_time, ref float Change_time, ref bool Action)
    {
        //Pattern Change
        ChangeActionPattern(Cur_time, ref Change_time, ref Action);

        if (Action)
        {

            //Chase Move(target -> Player)
            if (chase)       
            {
                Vector3 target_dir = target.transform.position - transform.position;
                move_dir = new Vector3(target_dir.x, 0.0f, target_dir.z).normalized;

                anim.speed = 2.0f;

                cur_speed = run_speed;
            }
            else //Move
                cur_speed = walk_speed;


            //rotation
            transform.rotation = Quaternion.LookRotation(move_dir);
        }
        else
            cur_speed = 0.0f;

    }


}
