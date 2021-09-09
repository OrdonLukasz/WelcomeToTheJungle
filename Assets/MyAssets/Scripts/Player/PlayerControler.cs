using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PlayerControler : MonoBehaviour
{
    #region Variables
    public ScriptManager scriptManager;
    public RestartMenu restartMenu;

    public GameObject[] hearts;

    public int amountOfJumps = 1;
    public int facingDirection;
    public float playerSpeed = 10.0f;
    public float playerJumpForce = 12.0f;
    public float pushForce;
    public float groundCheckRadius;
    public float dashTime;
    public float dashSpeed;
    public float distanceBetweenImages;
    public float dashCoolDown;
    public float hittime;
    public float movement = 2.0f;
    public float virableJumpHeightMultiplier = 0.05f;

    public bool canJump;
    public bool canFlip;
    public bool isGrounded;
    public bool isRunning;
    public bool isTurnedLeft;
    public bool canMove;

    private float movemnentInputDirection;
    private float dashTimeLeft;
    private float lastImageXpos;

    private int amountOfJumpsLeft;
    private int life;

    private bool isDashing;
    private Animator anim;
    Rigidbody2D playerRigidbody2D;

    public Transform groundCheck;
    public LayerMask whatIsGround;
    #endregion

    public void Update()
    {
        CheckInput();
        UpdateAnimations();
        CheckMovementDirection();
        CheckIfCanJump();
        CheckDash();
        hittime += Time.deltaTime;
    }
    private void UpdateAnimations()
    {
        anim.SetBool("IsRunning", isRunning);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", playerRigidbody2D.velocity.y);
    }
    public void Start()
    {
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;
        life = hearts.Length;
    }
    public void FixedUpdate()
    {
        ApplyMovement();
        CheckGrounding();
    }
    public void GetDamage(int damage)
    {
        if (canBeHit())
        {
            hittime = 0;
            life -= damage;
            Destroy(hearts[life].gameObject);
            if (life < 1)
            {
                restartMenu.ActivationRestartMenu();
            }

            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(transform.GetComponent<SpriteRenderer>().DOColor(Color.red, 0.5f));
            mySequence.Append(transform.GetComponent<SpriteRenderer>().DOColor(Color.white, 0.2f));
        }
    }
    public int GetFacingDirection()
    {
        return facingDirection;
    }
    public bool canBeHit()
    {
        if (hittime >= 0.5f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void CheckIfCanJump()
    {
        if (isGrounded && playerRigidbody2D.velocity.y <= 0)
        {
            amountOfJumpsLeft = amountOfJumps;
        }
        if (amountOfJumpsLeft <= 0)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }
    }
    private void CheckGrounding()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }
    private void CheckMovementDirection()
    {
        if (isTurnedLeft && movemnentInputDirection > 0)
        {
            Flip();
            facingDirection = -1;
        }
        else if (!isTurnedLeft && movemnentInputDirection < 0)
        {
            Flip();
            facingDirection = 1;
        }
        if (Mathf.Abs(playerRigidbody2D.velocity.x) >= 0.01f)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }
    private void CheckInput()
    {
        movemnentInputDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }
        if (Input.GetButtonUp("Jump") || Input.GetKeyUp(KeyCode.W))
        {
            playerRigidbody2D.velocity = new Vector2(playerRigidbody2D.velocity.x, playerRigidbody2D.velocity.y * virableJumpHeightMultiplier);
        }
        if (Input.GetButtonDown("Dash"))
        {
            AttemptToDash();
        }
    }
    private void AttemptToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        PlayerAfterImagePool.Instance.GetFromPool();
        lastImageXpos = transform.position.x;
    }
    private void CheckDash()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                playerRigidbody2D.AddForce(playerSpeed * transform.forward * dashSpeed);
                dashTimeLeft -= Time.deltaTime;

                if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
                {
                    PlayerAfterImagePool.Instance.GetFromPool();
                    lastImageXpos = transform.position.x;
                }
            }
            if (dashTimeLeft <= 0)
            {
                isDashing = false;
            }
        }
    }
    private void ApplyMovement()
    {
        playerRigidbody2D.velocity = new Vector2(playerSpeed * movemnentInputDirection, playerRigidbody2D.velocity.y);
    }
    private void Flip()
    {
        isTurnedLeft = !isTurnedLeft;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    private void DisableFlip()
    {
        canFlip = false;
    }
    private void EnableFlip()
    {
        canFlip = true;
    }
    public void Jump()
    {
        if (canJump)
        {
            playerRigidbody2D.velocity = new Vector2(playerRigidbody2D.velocity.x, playerJumpForce);
            amountOfJumpsLeft--;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coins"))
        {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("EnemyMushroom"))
        {
            GetDamage(1);
            transform.GetComponent<Knockback>().DoKnockBack();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            playerRigidbody2D.transform.parent = collision.gameObject.transform;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            playerRigidbody2D.transform.parent = null;
        }
    }
}
