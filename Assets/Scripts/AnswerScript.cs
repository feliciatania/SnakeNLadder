using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerScript : MonoBehaviour
{
    public bool isCorrect = false;
    public GameManager GM;

    public void Answer()
    {
        if(isCorrect)
        {
            Debug.Log("benar");
            GM.correctAnswer();
        }
        else
        {
            Debug.Log("salah");
            GM.wrongAnswer();
        }
      
    }
}
