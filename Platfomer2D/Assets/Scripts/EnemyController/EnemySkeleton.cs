using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : MonoBehaviour
{
    //Vari�vel de componentes 
    private Rigidbody2D rb;
    [SerializeField] private Transform rayPoint;

    //Vari�vel de movimento
    [SerializeField] private float maxVision;
    
    //Vari�vel de Ataque

    //Vari�vel de Vida
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DetectPlayer();
    }

    void DetectPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(rayPoint.position, Vector2.right, maxVision);

          if(hit.collider != null)
        {
            if (hit.transform.CompareTag("Player")){
                Debug.Log("Player!");
            }
        }
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(rayPoint.position, Vector2.right * maxVision);
    }
}
