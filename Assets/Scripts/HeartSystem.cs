using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartSystem : MonoBehaviour
{
    public GameObject[] Hearts;

    private int Life;
    private bool Dead;

    private void Start()
    {
        Life = 3;
    }

    void Update()
    {
        if (Dead == true)
        {
            Debug.Log("Game Over");
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
        for (int i = 0; i < d; i++)
        {
            Life--;
            Hearts[Life].SetActive(false);

            if (Life < 1)
            {
                Dead = true;
            }
        }
    }
}
