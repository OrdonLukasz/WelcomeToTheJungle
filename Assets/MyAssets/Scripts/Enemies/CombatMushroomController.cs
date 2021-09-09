using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMushroomController : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private float maxHealth = 3.0f;
    [SerializeField]
    private float knockbackSpeedX;
    [SerializeField]
    private float knockbackSpeedY;
    [SerializeField]
    private float knockbackDuration;
    [SerializeField]
    private float knockbackDeathSpeedX;
    [SerializeField]
    private float knockbackDeathSpeedY;
    [SerializeField]
    private float deathTorque;

    private float currentHealth;
    private float knockbackStart;

    private int playerFacingDirection;

    [SerializeField]
    private PlayerControler playerController;

    [SerializeField]
    private GameObject aliveGO;
    [SerializeField]
    private GameObject brokenTopGo;
    [SerializeField]
    private GameObject brokenBotGo;

    private Rigidbody2D rbAlive;
    private Rigidbody2D rbBrokenTop;
    private Rigidbody2D rbBrokenBot;
    private Animator aliveAnim;

    [SerializeField]
    private bool applyKnockback;

    private bool playerOnLeft;
    private bool knockback;
    #endregion

    private void Start()
    {
        currentHealth = maxHealth;

        aliveAnim = aliveGO.GetComponent<Animator>();
        rbAlive = aliveGO.GetComponent<Rigidbody2D>();
        rbBrokenTop = brokenTopGo.GetComponent<Rigidbody2D>();
        rbBrokenBot = brokenBotGo.GetComponent<Rigidbody2D>();

        aliveGO.SetActive(true);
        brokenTopGo.SetActive(false);
        brokenBotGo.SetActive(false);
    }
    private void Update()
    {
        CheckKnockback();
    }
    private void Damage(float amount)
    {
        currentHealth -= amount;
        playerFacingDirection = playerController.GetFacingDirection();

        if (playerFacingDirection == 1)
        {
            playerOnLeft = true;
        }
        else
        {
            playerOnLeft = false;
        }

        aliveAnim.SetBool("PlayerOnLeft", playerOnLeft);
        aliveAnim.SetTrigger("Damage");

        if (applyKnockback && currentHealth > 0f)
        {
            Knockback();
        }
        if (currentHealth <= 0f)
        {
            Die();
        }
    }
    private void Knockback()
    {
        knockback = true;
        knockbackStart = Time.time;
        rbAlive.velocity = new Vector2(knockbackSpeedX * -playerFacingDirection, knockbackSpeedY);
    }
    private void CheckKnockback()
    {
        if (Time.time >= knockbackStart + knockbackDuration && knockback)
        {
            knockback = false;
            rbAlive.velocity = new Vector2(0.0f, rbAlive.velocity.y);
        }
    }
    private void Die()
    {
        aliveGO.SetActive(false);
        brokenTopGo.SetActive(true);
        brokenBotGo.SetActive(true);

        brokenTopGo.transform.position = aliveGO.transform.position;
        brokenBotGo.transform.position = aliveGO.transform.position;

        rbBrokenBot.velocity = new Vector2(knockbackSpeedX * playerFacingDirection, knockbackSpeedY);
        rbBrokenTop.velocity = new Vector2(knockbackDeathSpeedX * playerFacingDirection, knockbackDeathSpeedY);
        rbBrokenTop.AddTorque(deathTorque * -playerFacingDirection, ForceMode2D.Impulse);

        Destroy(gameObject, 10);
    }
}
