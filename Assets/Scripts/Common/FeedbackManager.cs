using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager instance;

   
    void Awake()
    {
        instance = this;
    }

  

}
