using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Variável de movimento
    [SerializeField] protected float speed;

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
