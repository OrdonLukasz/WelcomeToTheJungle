using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMushroom : MonoBehaviour
{
    [SerializeField]
    private float speed = 1.5f;
    [SerializeField]
    private float enemyJumpForce = 6f;

    public bool moveRight;
    public bool isTurnedLeft;
    public bool isGrounded;
    public bool isRunning;
    public bool canJump;

    public int livePoints = 3;
    public GameObject enemyMushroom;


    private void FixedUpdate()
    {
        Movement();
        CheckGround();
    }
    public void Movement()
    {
        if (moveRight)
        {
            transform.Translate(2 * Time.deltaTime * speed, 0, 0);
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            transform.Translate(-2 * Time.deltaTime * speed, 0, 0);
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }
    public void TakeDamage(int playerDamage)
    {
        livePoints -= playerDamage;
        if (livePoints < 1)
        {
            Destroy(enemyMushroom.gameObject);
        }
    }
    public void Jump()
    {
        transform.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x, enemyJumpForce);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("movementBorder"))
        {
            if (moveRight)
            {
                moveRight = false;
            }
            else
            {
                moveRight = true;
            }
        }
    }
    public void CheckGround()
    {
        Debug.DrawRay(transform.position - new Vector3(0.3f, 0.65f, 0), Vector2.right * 0.6f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position - new Vector3(0.3f, 0.65f, 0), Vector2.right, 1);

        if (hit.collider != null && hit.collider.name == "TilemapWallsAndFloors")
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }
        Debug.DrawRay(transform.position - new Vector3(1f, -1.5f, 0), Vector2.down * 1.4f, Color.blue);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position - new Vector3(1f, -1.5f, 0), Vector2.down, 1.4f);
        if (hitLeft.collider != null && hitLeft.collider.name == "TilemapWallsAndFloors")
        {
            Jump();
        }
        Debug.DrawRay(transform.position - new Vector3(-1f, -1.5f, 0), Vector2.down * 1.4f, Color.green);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position - new Vector3(-1f, -1.5f, 0), Vector2.down, 1.4f);
        if (hitRight.collider != null && hitRight.collider.name == "TilemapWallsAndFloors")
        {
            Jump();
        }
    }
}
