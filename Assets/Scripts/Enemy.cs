using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;

    public int maxHealth = 100;
    int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        animator.SetTrigger("gettingHit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy Died");

        animator.SetBool("isDead", true);

        GetComponent<Collider>().enabled = false;

        //this.enabled = false;

        StartCoroutine(DisableAfterDelay(10f));
    }

    IEnumerator DisableAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified amount of time

        Destroy(gameObject);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(FindDeepChild(animator.transform, "mixamorig:LeftHand").position, 5);
    }
}
