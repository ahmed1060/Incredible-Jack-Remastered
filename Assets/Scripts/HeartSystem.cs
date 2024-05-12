using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartSystem : MonoBehaviour
{
    public Animator animator;

    public GameObject[] Hearts;

    public int Life;
    public bool Dead;

    private void Start()
    {
        Life = 3;
    }

    void Update()
    {

        if (Dead == true)
        {
            animator.SetBool("isDead", true);
        }
    }

    public void Heal()
    {
        if (Life < 5)
        {
            Hearts[Life].SetActive(true);
            Life++;
        }
    }

    public void TakeDamage(int d)
    {
        if (Life >= 1)
        {
            animator.SetTrigger("gettingHit");

            for (int i = 0; i < d; i++)
            {
                Life--;
                Hearts[Life].SetActive(false);

                if (Life < 1)
                {
                    Dead = true;
                    break;
                }
            }
        }
        
    }
}
