using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemySkeleton : Enemy
{
    //Variável de componentes 
    private Rigidbody2D rb;
    private Animator anim; 
    [SerializeField] private Transform rayPoint;
    [SerializeField] private Transform behindRayPoint;


    //Variável de movimento
    private Vector2 direction;
    private bool playerDetected; 
    [SerializeField] private float maxVision;
    [SerializeField] private bool isRight;

    //Variável de Ataque


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        //Código abaixo faz com que o inimigo receba um valor de direção inicial logo quando o jogo começa
        //Sem a direção inicial, o vetor dele sempre seria zero em ambos os eixos, impedindo o movimento provido pelo Vector2
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

        EnemyMoveInDirection();

    }

    void DetectPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(rayPoint.position, direction, maxVision);

        //Detecta colisão do Raycast com o ambiente 
        if (hit.collider != null)
        {
            //filtra a colisão para atingir somente objetos com a tag player
            if (hit.transform.CompareTag("Player"))
            {
                playerDetected = true;
                float distance = Vector2.Distance(transform.position, hit.transform.position);// distancia do inimigo ate o objeto com tag "Player"

                if(distance < stopMove && health > 0) //Distancia para atacar
                {
                    playerDetected = false;
                    anim.SetInteger("transition", 2);
                    rb.velocity =  Vector2.zero;
                    hit.transform.GetComponent<PlayerController>().OnHit();
                }
            }
            else if (!hit.transform.CompareTag("Player")) //Faz com queo inimigo pare caso não esteja vendo o jogador
            {
                playerDetected = false;
                rb.velocity = Vector2.zero;
                anim.SetInteger("transition", 0);
            }

        }

       //Fisica que cria um segundo Raycast por trás do inimigo para detectar se o player estiver atrás
        RaycastHit2D behindHit = Physics2D.Raycast(behindRayPoint.position, -direction, maxVision);

        //Detecta colisão do Raycast com o ambiente 
        if(behindHit.collider != null)
        {
            //filtra a colisão para atingir somente objetos com a tag player
            if (behindHit.transform.CompareTag("Player"))
            {
                isRight = !isRight;
                playerDetected = true; 
            }
        }

    }

    //Função para mudar a direção que o inimigo está olhando 
    //Adiciona uma velocidade fazendo ele se mover em direção para qual está virado
    void EnemyMoveInDirection() 
    {
        if(playerDetected)
        {
            anim.SetInteger("transition", 1);
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

    }

    //Cria um desenho do Gizmo que ajuda a ter uma visualização da área de ataque
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(rayPoint.position, direction * maxVision);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(behindRayPoint.position, -direction * maxVision);
    }

    //Sobrescreve a função de Applydamage de Enemy adicionando um trigger para a animação de morte
    //e reduzido a speed a zero caso esteja morto
    public override void ApplyDamage(int dmg)
    {
        health -= dmg;

        if (health > 0)
        {
            anim.SetTrigger("hit");

        }
        else if(health <= 0)
        {
            anim.SetTrigger("dead");
            speed = 0;
            Destroy(gameObject, timeOfDeath);
        }
        

    }
}
