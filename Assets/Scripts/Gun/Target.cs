using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    public float health = 100f;

    //Enemy Stuff
    public void TakeDamage(float damage)
    {

        health -= damage;
        
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
