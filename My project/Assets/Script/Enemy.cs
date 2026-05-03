using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //[SerializeField] float move_speed = 5.0f;
    [SerializeField] float change_random_time_max = 5.0f;
    [SerializeField] float change_random_time_min = 1.0f;
    [SerializeField] float move_speed = 10.0f;

    Rigidbody rigid;
    Vector3 move_dir;
    float accumulated_time = 0.0f;                      //Current time
    float pattern_chanage_time = 0.0f;                   //Next pattern change time (compare accumlated_time)
    float cur_speed = 0.0f;
    bool action = true;

    HashSet<Collider> col_check = new HashSet<Collider>();  //search_objects layer check



    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        move_dir = RandomDirect();
        pattern_chanage_time = Random.Range(change_random_time_min, change_random_time_max);

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


        MovePattern( accumulated_time, ref pattern_chanage_time, ref action);



        Debug.Log("Change_time " + pattern_chanage_time);
        Debug.Log("accumulated_time " + accumulated_time);



    }

    private void OnTriggerEnter(Collider other)
    {
        if (!col_check.Add(other))
            return;

        Debug.Log("check");



    }



    Vector3 RandomDirect()
    {
        return new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f)).normalized;
    }

    //True action value
    bool ChangeMovePattern(float Cur_time, ref float Change_time)
    {
        if (Cur_time <= Change_time)
            return false;

        Change_time = Random.Range(change_random_time_min, change_random_time_max);
        move_dir = RandomDirect();
        accumulated_time = 0.0f;

        return true;
    }

    //set move_speed, rotation
    void MovePattern(float Cur_time, ref float Change_time, ref bool Action)
    {
        if (Action)
        {
            cur_speed = move_speed;
            transform.rotation = Quaternion.LookRotation(move_dir);
        }
        else
            cur_speed = 0.0f;

        if (ChangeMovePattern(Cur_time, ref Change_time))
            Action = !Action;
    }


}
