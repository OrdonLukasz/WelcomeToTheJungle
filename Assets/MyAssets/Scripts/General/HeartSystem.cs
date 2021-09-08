using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject[] hearts;
    private int life;
    
    private void Start()
    {
        life = hearts.Length;
    }
    public void TakeDamage(int damage)
    {
        life -= damage;
        Destroy(hearts[life].gameObject);
    }
}

