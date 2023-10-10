using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonA : Enemy
{
    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField] private float speed;
    [SerializeField] private Transform point;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask layer;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        
    }


    void FixedUpdate()
    {
        rb.velocity = new Vector2(speed, rb.velocity.y);
        OnCollision();
    }
    
    void OnCollision() // Função responsável por identificar quando o inimigo bate em uma parede
    {
        Collider2D hit = Physics2D.OverlapCircle(point.position, radius, layer);
        if (hit != null)
        {
            //Chamado quando o inimigo bate no objeto e realiza uma mudança de direção
            //Mudando a velocidade para positiva no eixo x e o valor do eixo y para 180
            speed = -speed;

            if(transform.eulerAngles.y == 0)
            {

                transform.eulerAngles = new Vector2(0, 180);
            }
            else
            {
                transform.eulerAngles = new Vector2(0, 0);

            }

        }
    }

    public override void ApplyDamage(int dmg)
    {
        anim.SetTrigger("death");

        speed = 0; 
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(point.position, radius);
    }
}
