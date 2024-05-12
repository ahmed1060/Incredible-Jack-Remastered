using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;

    public Transform attackPoint;
    public LayerMask enemyLayers;

    public float attackRange = 4f;
    public int attackDamage = 40;

    public float attackRate = 2f;
    float nextAttackTime = 0;

    void Update()
    {
        if (Time.time > nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }

        if (animator.GetBool("isDead"))
        {
            this.enabled = false;
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack");

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
