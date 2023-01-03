using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Destructible : MonoBehaviour
{
    [SerializeField] protected int Life = 3;
    
    public virtual void TakeDamage()
    {
        if (Life > 0)
        {
            Life--;
        }
        if (Life == 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        
    }
}
