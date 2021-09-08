using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    #region Variables
    [SerializeField] 
    float knockBackLength = 0.5f;
    [SerializeField]
    float knockBackForce = 15f;

    bool isHurt = false;
    PlayerControler playerController;
    Rigidbody2D rb;
    #endregion

    void Start()
    {
        playerController = GetComponent<PlayerControler>();
        rb = GetComponent<Rigidbody2D>();
    }
    public bool IsHurt
    {
        get { return isHurt; }
    }
    public void DoKnockBack()
    {
        StartCoroutine(DisablePlayerMovement(knockBackLength));
        rb.velocity = new Vector2(-playerController.facingDirection * knockBackForce, knockBackForce);
    }
    IEnumerator DisablePlayerMovement(float time)
    {
        playerController.canMove = false;
        isHurt = true;
        yield return new WaitForSeconds(time);
        playerController.canMove = true;
        isHurt = false;
    }
}


