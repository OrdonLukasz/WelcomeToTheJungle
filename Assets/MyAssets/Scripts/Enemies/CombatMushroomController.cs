using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMushroomController : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 3, knockbackSpeedX, knockbackSpeedY, knockbackDuration;
    [SerializeField]
    private float knockbackDeathSpeedX, knockbackDeathSpeedY, deathTorque;
    private float currentHealth, knockbackStart;
    [SerializeField]
    private bool applyKnockback;

    private PlayerControler pc;
    [SerializeField]
    private GameObject aliveGO, brokenTopGo, brokenBotGo;
    private Rigidbody2D rbAlive, rbBrokenTop, rbBrokenBot;
    private Animator aliveAnim;
    private int playerFacingDirection;

    private bool playerOnLeft, knockback;


    private void Start()
    {
        currentHealth = maxHealth;

        pc = GameObject.Find("Player").GetComponent<PlayerControler>();

        aliveGO = transform.Find("Alive").gameObject;
        brokenTopGo = transform.Find("DeadTop").gameObject;
        brokenBotGo = transform.Find("DeadBottom").gameObject;

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
        playerFacingDirection = pc.GetFacingDirection();

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
