using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //[SerializeField] float move_speed = 5.0f;
    [SerializeField] float next_action_max_time = 15.0f;
    [SerializeField] float next_action_min_time = 8.0f;
    [SerializeField] float move_speed = 10.0f;

    Rigidbody rigid;
    Vector3 move_dir;
    float accumulated_time = 0.0f;
    float next_action_time = 0.0f;
    float cur_speed = 0.0f;
    bool action = true;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        move_dir = RandomDirect();
        next_action_time = Random.Range(next_action_min_time, next_action_max_time);

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

        //True action value
        if (accumulated_time >= next_action_time)
        {  
            action = !action;            
            next_action_time = Random.Range(next_action_min_time, next_action_max_time);
            
            if(action)
                move_dir = RandomDirect();

            accumulated_time = 0.0f;   

        }

        //set move_speed 
        if (action)
        {
            cur_speed = move_speed;

            transform.rotation = Quaternion.LookRotation(move_dir);
        }
        else
            cur_speed = 0.0f;



    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collsion");
    }



    Vector3 RandomDirect()
    {
        return new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f)).normalized;
    }


}
