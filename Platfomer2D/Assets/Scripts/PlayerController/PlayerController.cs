using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask EnemyLayer;

    [Header("Moviment Variables")]
    [SerializeField] private float speed;


    [Header("Jump Variables")]
    private bool isGrounded;
    private bool isJumping;
    private float jumpTimeCounter;
    private Vector2 vecGravity;
    [SerializeField] private float fallMultipler; 
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpMultiplayer;
    [SerializeField] private Transform feetPos;
    [SerializeField] private float checkRadius;
    [SerializeField] private float jumpTime;
    [SerializeField] LayerMask ground;



    [Header("Attack Variables")]
    private bool isAttacking; 
    [SerializeField] private float radius;
    [SerializeField] private int playerDamage;


    [Header("Life Variables")]
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

         JumpLogic();

    }

    void FixedUpdate()
    {
        Move();

    }


    #region move
    //Fisicas e Inputs de movimento 
    void Move() 
    {
        //Retorna 1 para direita, -1 para esquerda e 0 para caso não precione nada. 

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
    #endregion

    #region jumpLogic
    void JumpLogic()
    {
       isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, ground);

        vecGravity = new Vector2(0, -Physics2D.gravity.y);

        if(isGrounded == true && Input.GetButtonDown("Jump"))
        {
            anim.SetInteger("Transition", 2);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); 
            isJumping = true;
            jumpTimeCounter = 0;
        }

        if (rb.velocity.y < 0) 
        {
            
            rb.velocity -= fallMultipler * Time.deltaTime * vecGravity;
        }

        if (Input.GetButton("Jump") && isJumping)
        {
            rb.velocity += jumpMultiplayer * Time.deltaTime * vecGravity;
            jumpTimeCounter += Time.deltaTime;

            if (jumpTimeCounter > jumpTime)
            {

                isJumping = false;
            }

            float t = jumpTimeCounter / jumpTime;
            float currentJumpM = jumpMultiplayer;

            if(t > 0.5)
            {
                currentJumpM = jumpMultiplayer * (1 - t);
            }

            rb.velocity += vecGravity * currentJumpM * Time.deltaTime;
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
            jumpTimeCounter = 0; 

            if(rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.6f);
            }
        }
    }

    #endregion

    #region attack
    //Função que aplica lógica de ataque
    void Attack() 
    {
        if (Input.GetButtonDown("Fire1"))
        {
            isAttacking = true;
            anim.SetInteger("Transition", 3);
            //gera um circulo apartir de um transform attackpoint e usa o radius para criar uma esfera que vai identificar a colisão
            Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, radius, EnemyLayer); 

            if (hit != null) //Aplica o dano no inimigo caso o mesmo se depare com o circulo de ataque gerado
            {
                hit.GetComponent<Enemy>().ApplyDamage(playerDamage);
            }

            StartCoroutine(OnAttack());
        }

    }

    //Corotina responsável por parar o ataque após executar o primeiro, assim dando um intervalo entre os ataques
    IEnumerator OnAttack()
    {
        yield return new WaitForSeconds(0.35f);
        isAttacking = false; 

    }

    //Cria um desenho do Gizmo que ajuda a ter uma visualização da área de ataque
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, radius);
    }
    #endregion

    #region takeDamage
    //Função responsável por retirar a vida do jogador uma vez que ele receba dano
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

        //Caso a vida do player seja zero, usa a animação de morte 
        //Utiliza recoveryTime para impedir que a condição seja lida novamente e não executar a animação de morte várias vezes
        if (playerHealth <= 0 && !recoveryTime) 
        {
            recoveryTime = true; 
            anim.SetTrigger("Dead");
            Debug.Log("GameOver");
        }
    }
    #endregion

    #region coinCollector
    //Ativa o trigger quando o jogador entra em contato com um objeto com a layer 7 (Layer Enemy)
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 7)
        {
            OnHit();
        }

        if (collision.CompareTag("Coin")) //Comparação para saber se pegou as moedas
        {
            collision.GetComponent<Animator>().SetTrigger("Hit"); //Ativa a animação quando pega a moeda
            GameController.instance.GetCoin(); //Função que permite a captura de moedas 
            Destroy(collision.gameObject, 0.35f); //Tempo para destruir a moeda após ela ter sido pega
        }
    }
    #endregion
}
