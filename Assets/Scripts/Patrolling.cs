using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrolling : MonoBehaviour
{
    public Animator animator;

    public Transform[] points;
    int current;
    public float speed = 10;
    
    public float rotationSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        current = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(animator.GetBool("inSight") == false)
        {
            if (transform.position != points[current].position)
            {
                MoveTowards(points[current].position);
            }
            else
            {
                current = (current + 1) % points.Length;
            }
        }
        
        if(animator.GetBool("isDead") == true)
        {
            this.enabled = false;
        }
    }

    void MoveTowards(Vector3 target)
    {
        FaceTarget(target);

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }
}
