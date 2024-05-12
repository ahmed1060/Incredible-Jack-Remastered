using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class monster_walk : StateMachineBehaviour
{
    Transform player;
    Transform monster;
    Transform monsterWeapon;
    public LayerMask playerLayer;

    public float speed = 10;
    public float rotationSpeed = 5;
    public float monsterSightRange = 20f;
    public float monsterAttackRange = 10f;
    public float monsterWeaponAttackRange = 5f;
    public int attackDamage = 1;
    private float attackTimer = 0;
    public float attackCooldown = 2f;

    private bool playerDead = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        monster = animator.GetComponent<Transform>();
        monsterWeapon = FindDeepChild(animator.transform, "mixamorig:LeftHand");
        attackTimer = attackCooldown;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!playerDead)
        {
            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }

            Collider[] hits = Physics.OverlapSphere(monster.position, monsterSightRange, playerLayer);
            if (hits.Length > 0)
            {
                animator.SetBool("inSight", true);

                Vector3 target = new Vector3(player.position.x, monster.position.y, player.position.z);
                MoveTowards(target);
            }
            else
            {
                animator.SetBool("inSight", false);
            }

            if (Vector3.Distance(player.position, monster.position) <= monsterAttackRange && attackTimer <= 0)
            {
                attackTimer = attackCooldown;

                animator.SetTrigger("Attack");

                Collider[] hitPlayer = Physics.OverlapSphere(monsterWeapon.position, monsterWeaponAttackRange, playerLayer);
                foreach (Collider player_ in hitPlayer)
                {
                    player_.GetComponent<HeartSystem>().TakeDamage(attackDamage);

                    if (player_.GetComponent<HeartSystem>().Dead == true)
                    {
                        animator.SetBool("inSight", false);
                        
                        playerDead = true;

                        Debug.Log("Game Over");
                    }
                }
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }



    void MoveTowards(Vector3 Target)
    {
        FaceTarget(Target);

        monster.position = Vector3.MoveTowards(monster.position, Target, speed * Time.deltaTime);
    }

    void FaceTarget(Vector3 Target)
    {
        Vector3 direction = (Target - monster.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        monster.rotation = Quaternion.Slerp(monster.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    Transform FindDeepChild(Transform aParent, string aName)
    {
        foreach (Transform child in aParent)
        {
            if (child.name == aName)
                return child;
            Transform result = FindDeepChild(child, aName);
            if (result != null)
                return result;
        }
        return null;
    }
}
