using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : MonoBehaviour
{
    //Variável de componentes 
    private Rigidbody2D rb;
    [SerializeField] private Transform rayPoint;

    //Variável de movimento
    [SerializeField] private float maxVision;
    
    //Variável de Ataque

    //Variável de Vida
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
