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
    [SerializeField] protected float timeOfDeath;



    public virtual void ApplyDamage(int dmg)
    {
                
    }

}
