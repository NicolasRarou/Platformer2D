using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{

    private Animator anim;
    [SerializeField] private Animator barrierAnim;
    [SerializeField] private LayerMask layer; 

     void Start()
    {
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        OnCollision();
    }


    void OnPressed()
    {
        anim.SetBool("Press", true);
        barrierAnim.SetBool("Down", true);
    }

    void OnExit()
    {
        anim.SetBool("Press", false);
        barrierAnim.SetBool("Down", false);
    }

    //Retorna enquanto um objeto encosta em outro
    //void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Stone"))
    //    {
    //        OnPressed();
    //    }
    //}

    //Retorna quando um objeto deixa de encostar com o outro.
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Stone"))
    //    {
    //        OnExit();
    //    }
    //}


    void OnCollision()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, 1, layer);

        if(hit != null)
        {
            OnPressed();
            hit = null;
        }
        else
        {
            OnExit();
        } 
        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, 1);
    }
}
