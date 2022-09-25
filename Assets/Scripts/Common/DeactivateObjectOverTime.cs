using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Deactivates an object when the set amount has passed
/// </summary>
public class DeactivateObjectOverTime : MonoBehaviour
{
    // Start is called before the first frame update

    public float time = 0.5f;
    float timer;

    void Start()
    {
        timer = time;    
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0) 
        {
            this.gameObject.SetActive(false);
            timer = time;
        }
        
    }
}
