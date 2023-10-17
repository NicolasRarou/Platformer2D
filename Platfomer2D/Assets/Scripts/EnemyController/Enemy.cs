using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Vari�vel de movimento
    [SerializeField] protected float speed;
    [SerializeField] protected float stopMove;

    //Vari�vel de Vida
    [SerializeField] protected int health;
    [SerializeField] protected float timeOfDeath;



    public virtual void ApplyDamage(int dmg)
    {
                
    }

}
