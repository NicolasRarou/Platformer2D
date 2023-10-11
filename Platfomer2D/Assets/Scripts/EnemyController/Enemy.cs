using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Vari�vel de movimento
    [SerializeField] protected float speed;

    //Vari�vel de Vida
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
