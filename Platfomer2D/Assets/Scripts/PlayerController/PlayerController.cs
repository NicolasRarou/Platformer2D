using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    //Variável de componentes 
    private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask EnemyLayer;


    //Variável de movimento
    private bool isJumping;
    private bool doubleJump;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    //Variável de Ataque
    private bool isAttacking; 
    [SerializeField] private float radius;

    //Variável de Vida
    [SerializeField] private int playerHealth; 

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



    //Lógica e Input de pulo
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


    //Detecta colisão com o chão, impedindo que ocorreram multiplos pulos
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 6)
        {
            isJumping = false; 
        }
    }


    //Função que aplica lógica de ataque
    void Attack() 
    {
        const int PlayerDamage = 4; 

        if (Input.GetButtonDown("Fire1"))
        {
            isAttacking = true;
            anim.SetInteger("Transition", 3);
            Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, radius, EnemyLayer); //gera um circulo apartir de um transform attackpoint e usa o radius para criar uma esfera que vai identificar a colisão

            if (hit != null)
            {
                hit.GetComponent<Enemy>().ApplyDamage(PlayerDamage);
            }

            StartCoroutine(OnAttack());
        }

    }

    IEnumerator OnAttack()
    {
        yield return new WaitForSeconds(0.35f);
        isAttacking = false; 

    }

    //Cria um desenho do Gizmo que ajuda a ter uma visualização da área de ataque
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, radius);
    }

    void OnHit()
    {
        anim.SetTrigger("Hit");

        playerHealth--;

        if(playerHealth <= 0)
        {
            anim.SetTrigger("Dead");
            Debug.Log("GameOver");
        }
    }

     void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 7)
        {
            OnHit();
        }
    }

}
