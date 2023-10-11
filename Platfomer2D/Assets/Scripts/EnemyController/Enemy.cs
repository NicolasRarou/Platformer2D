using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Variável de movimento
    [SerializeField] protected float speed;
    [SerializeField] protected float stopMove;

    //Variável de Vida
    [SerializeField] protected int health;



    public virtual void ApplyDamage(int dmg)
    {
        health -= dmg; 

        if(health < 0)
        {
            Destroy(gameObject, 1f);
        }
        
    }

}
