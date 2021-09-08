using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeEnemy : MonoBehaviour
{
    public Transform borderLeft;
    public Transform borderRight;

    public void Start()
    {
        if(borderRight != null || borderLeft != null)
        {
            transform.gameObject.AddComponent<EnemyMushroom>();
            Destroy(transform.gameObject.GetComponent<InitializeEnemy>());
        }
        else
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
    }
}
