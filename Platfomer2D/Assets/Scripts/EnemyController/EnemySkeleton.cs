using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : Enemy
{
    //Variável de componentes 
    private Rigidbody2D rb;
    [SerializeField] private Transform rayPoint;


    //Variável de movimento
    private Vector2 direction;
    private bool playerDetected; 
    [SerializeField] private float maxVision;
    [SerializeField] private bool isRight;

    //Variável de Ataque


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (isRight) //Vira para a direita 
        {
            transform.eulerAngles = new Vector2(0, 0);
            direction = Vector2.right;

        }
        else //Vira para a esquerda 
        {
            transform.eulerAngles = new Vector2(0, 180);
            direction = Vector2.left;

        }
    }


    void FixedUpdate()
    {
        DetectPlayer();

        if (playerDetected)
        {
            EnemyMoveInDirection();

        }
    }

    void DetectPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(rayPoint.position, direction, maxVision);

          if(hit.collider != null)
        {
            if (hit.transform.CompareTag("Player")){
                playerDetected = true;
                float distance = Vector2.Distance(transform.position, hit.transform.position);// distancia do inimigo ate o objeto com tag "Player"

                if(distance < stopMove) //Distancia para atacar
                {
                    playerDetected = false;
                   rb.velocity =  Vector2.zero;
                    hit.transform.GetComponent<PlayerController>().OnHit();
                }
            }
        }

    }

    void EnemyMoveInDirection() //Função para mudar a direção que o inimigo está olhando
    {     
            if (isRight) //Vira para a direita 
            {
                transform.eulerAngles = new Vector2(0, 0);
                direction = Vector2.right;
                rb.velocity = new Vector2(speed, rb.velocity.y);

            }
            else //Vira para a esquerda 
            {
                transform.eulerAngles = new Vector2(0, 180);
                direction = Vector2.left;
                rb.velocity = new Vector2(-speed, rb.velocity.y);

            }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(rayPoint.position, direction * maxVision);
    }
}
