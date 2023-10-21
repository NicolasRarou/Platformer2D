using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGhoul : Enemy
{

    //Script feito para inimigos que correm ate o player dando dano por contato e não por ataque. 

    //Variável de componentes
    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField] private Transform point;

    //Variável de Ataque
    [SerializeField] private float radius;
    [SerializeField] private LayerMask layer;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Move();
        OnCollision();
    }

    void Move()
    {
        rb.velocity = new Vector2(speed, rb.velocity.y);

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

    //Sobrescreve a função de Applydamage de Enemy adicionando um trigger para a animação de morte
    //e reduzido a speed a zero caso esteja morto
    public override void ApplyDamage(int dmg)
    {
        health -= dmg;

        if (health <= 0)
        {
            anim.SetTrigger("death");
            speed = 0;
            Destroy(gameObject, timeOfDeath);
        }
    }
    //Cria um desenho do Gizmo que ajuda a ter uma visualização da área de ataque

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(point.position, radius);
    }
}
