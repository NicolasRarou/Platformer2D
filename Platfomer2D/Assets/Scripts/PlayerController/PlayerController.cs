using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    //Vari�vel de componentes 
    private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask EnemyLayer;


    //Vari�vel de movimento
    private bool isJumping;
    private bool doubleJump;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    //Vari�vel de Ataque
    private bool isAttacking; 
    [SerializeField] private float radius;
    [SerializeField] private int playerDamage;

    //Vari�vel de Vida 
    private bool recoveryTime;
    private float recoveryCount;
    [SerializeField] private int playerHealth;
    [SerializeField] private float DamageTime;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        
        Attack();
        Jump();
    }

    void FixedUpdate()
    {
        Move();
        
    }




    //Fisicas e Inputs de movimento 
    void Move() 
    {
        //Retorna 1 para direita, -1 para esquerda e 0 para caso n�o precione nada. 

        float movement = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(movement * speed, rb.velocity.y);

        if(movement > 0) // Faz o personagem virar a esquerda ao andar para a esquerda. 
        {
            if (!isJumping && !isAttacking)
            {
                anim.SetInteger("Transition", 1);
            }

            transform.eulerAngles = new Vector2(0, 0);

        }else if(movement < 0)
        {
            if (!isJumping && !isAttacking)
            {
                anim.SetInteger("Transition", 1);
            }
            transform.eulerAngles = new Vector2(0, 180);


        }

        if (movement == 0 && !isJumping && !isAttacking)
        {
            anim.SetInteger("Transition", 0);
        }
    }



    //L�gica e Input de pulo
    void Jump()
    {
        if (Input.GetButtonDown("Jump")){
            if (!isJumping)
            {
                anim.SetInteger("Transition", 2);
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isJumping = true;
                doubleJump = true;
            } 
            else if (doubleJump)
            {
               anim.SetInteger("Transition", 2);
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                doubleJump = false;
            }
           
        }
    }


    //Detecta colis�o com o ch�o, impedindo que ocorreram multiplos pulos
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 6)
        {
            isJumping = false; 
        }
    }


    //Fun��o que aplica l�gica de ataque
    void Attack() 
    {
        if (Input.GetButtonDown("Fire1"))
        {
            isAttacking = true;
            anim.SetInteger("Transition", 3);
            //gera um circulo apartir de um transform attackpoint e usa o radius para criar uma esfera que vai identificar a colis�o
            Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, radius, EnemyLayer); 

            if (hit != null) //Aplica o dano no inimigo caso o mesmo se depare com o circulo de ataque gerado
            {
                hit.GetComponent<Enemy>().ApplyDamage(playerDamage);
            }

            StartCoroutine(OnAttack());
        }

    }

    //Corotina respons�vel por parar o ataque ap�s executar o primeiro, assim dando um intervalo entre os ataques
    IEnumerator OnAttack()
    {
        yield return new WaitForSeconds(0.35f);
        isAttacking = false; 

    }

    //Cria um desenho do Gizmo que ajuda a ter uma visualiza��o da �rea de ataque
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, radius);
    }


    //Fun��o respons�vel por retirar a vida do jogador uma vez que ele receba dano
    protected internal void OnHit()
    {
        recoveryCount += Time.deltaTime; //Faz o valor inicial do contador receber Time.DeltaTime para que tenha uma contagem de segundos

        //usa o contador para impedir que o jogador tome dano constamente, dando uma pausa entre os danos
        if (recoveryCount >= DamageTime) 
        {
            anim.SetTrigger("Hit");

            playerHealth--;

            recoveryCount = 0f; 

        }

        //Caso a vida do player seja zero, usa a anima��o de morte 
        //Utiliza recoveryTime para impedir que a condi��o seja lida novamente e n�o executar a anima��o de morte v�rias vezes
        if (playerHealth <= 0 && !recoveryTime) 
        {
            recoveryTime = true; 
            anim.SetTrigger("Dead");
            Debug.Log("GameOver");
        }
    }

    //Ativa o trigger quando o jogador entra em contato com um objeto com a layer 7 (Layer Enemy)
     void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 7)
        {
            OnHit();
        }

        if (collision.CompareTag("Coin")) //Compara��o para saber se pegou as moedas
        {
            collision.GetComponent<Animator>().SetTrigger("Hit"); //Ativa a anima��o quando pega a moeda
            GameController.instance.GetCoin(); //Fun��o que permite a captura de moedas 
            Destroy(collision.gameObject, 0.35f); //Tempo para destruir a moeda ap�s ela ter sido pega
        }
    }

}
