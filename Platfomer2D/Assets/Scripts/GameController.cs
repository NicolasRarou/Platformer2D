using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance; //Singleton

    //Variável de pontuação 
    [SerializeField] private int score;
    [SerializeField] private Text scoreText; 
    
     void Awake()
    {
        instance = this;

        
    }
   




    public void GetCoin() //Função de captura de moedas
    {
        score++; //Aumenta o score sempre que uma moeda for pega
        scoreText.text = "x " + score.ToString(); //Modifica o texto refletido o score com o numero de moedas pegas
    }

}
