using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class enemyAI : MonoBehaviour
{
    [SerializeField] private float moveSpeed = .8f;
    [SerializeField] private float aggroSpeedMultiplier = 1f;
    [SerializeField] private float jumpForce = .8f;
    [SerializeField] private float homeRange = 5f;
    [SerializeField] private Transform homeWaypt = null;
    [SerializeField] private float groundedRange = 1.2f;
    [SerializeField] private float killingRange = 1.6f;

    public float jumpRange = -1f;

    private RaycastHit2D edgeHit;
    private RaycastHit2D playerHit;
    private Rigidbody2D rigidBody;
    private Transform target = null;
    public bool isGrounded = false;
    public LayerMask whatIsGround;

    public enum ENEMY_STATE
    {
        IDLE,
        AGGRO,
        KILLING
    }

    public ENEMY_STATE currentState;

    void Start()
    {
        Physics2D.queriesStartInColliders = false;
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        RaycastHit2D downHit = Physics2D.Raycast(transform.position, Vector2.down, 5f, whatIsGround);
        if (Vector2.Distance(downHit.point, transform.position) < groundedRange)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        if (transform.localScale.x < 0)
        {
            edgeHit = Physics2D.Raycast(transform.position - (Vector3.down * jumpRange), Vector2.left);
            playerHit = Physics2D.Raycast(transform.position + (Vector3.down / 2f), Vector2.left);
            Debug.DrawRay(transform.position - (Vector3.down * jumpRange), Vector2.left);
        }
        else
        {
            edgeHit = Physics2D.Raycast(transform.position - (Vector3.down * jumpRange), Vector2.right);
            playerHit = Physics2D.Raycast(transform.position + (Vector3.down / 2f), Vector2.right);
            Debug.DrawRay(transform.position - (Vector3.down * jumpRange), Vector2.right);
        }

        if (playerHit.collider != null && playerHit.transform.gameObject.tag == "Player" && currentState != ENEMY_STATE.KILLING)
        {
            if (!playerHit.transform.gameObject.GetComponent<PlayerMovement>().getHiding())
            {
                target = playerHit.transform;
                StopAllCoroutines();
                currentState = ENEMY_STATE.AGGRO;
                StartCoroutine(AttackTimeout());
            }
        }

        switch (currentState)
        {
            case ENEMY_STATE.IDLE:
                aggroSpeedMultiplier = 1f;
                if(name == "Enemy (1)") Debug.Log(Vector2.Distance(homeWaypt.position, transform.position) > homeRange);
                if (isGrounded && Vector2.Distance(homeWaypt.position, transform.position) > homeRange)
                {
                    turnToDirection(homeWaypt.position.x - transform.position.x);
                }
                break;
            case ENEMY_STATE.AGGRO:
                aggroSpeedMultiplier = 2.5f;
                if (isGrounded && Vector2.Distance(target.position, transform.position) > 0.9f)
                {
                    turnToDirection(target.position.x - transform.position.x);
                }
                if (Vector2.Distance(target.position, transform.position) < killingRange)
                {
                    currentState = ENEMY_STATE.KILLING;
                }
                break;
            case ENEMY_STATE.KILLING:
                target.position = new Vector3(transform.position.x + transform.localScale.x, transform.position.y, transform.position.z);
                target.gameObject.GetComponent<PlayerMovement>().isCaptured = true;
                target.gameObject.GetComponent<SpriteRenderer>().material.color = new Vector4(1, 0, 0, 1);
                rigidBody.velocity = new Vector2(0f, rigidBody.velocity.y);
                break;
        }

        if (currentState != ENEMY_STATE.KILLING)
        {
            if (!edgeHit.transform.gameObject.CompareTag("Player") && Vector2.Distance(edgeHit.point, transform.position) < 2.7f && isGrounded)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
            }

            Move();
        }
    }

    private IEnumerator AttackTimeout()
    {
        yield return new WaitForSeconds(5);
        currentState = ENEMY_STATE.IDLE;
    }

    private void Move()
    {
        if (IsFacingRight())
        {
            rigidBody.velocity = new Vector2(moveSpeed * aggroSpeedMultiplier, rigidBody.velocity.y);
            //rigidBody.velocity = new Vector2(moveSpeed, 0);
        }
        else
        {
            rigidBody.velocity = new Vector2(-moveSpeed * aggroSpeedMultiplier, rigidBody.velocity.y);
            //rigidBody.velocity = new Vector2(-moveSpeed, 0);
        }
    }

    private bool IsFacingRight()
    {
        return transform.localScale.x > Mathf.Epsilon;
    }

    /*private void OnTriggerExit2D(Collider2D collision)
    {
        if(!isAttacking && !isReturningHome)
        {
            FlipScale();
        }
    }*/

    public void turnToDirection(float direction)
    {
        if (direction < -0.2 && IsFacingRight())
        {
            FlipScale();
        }
        else if (direction > 0.2 && !IsFacingRight())
        {
            FlipScale();
        }
    }

    private void FlipScale()
    {
        
        transform.localScale = new Vector2(-Mathf.Sign(rigidBody.velocity.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y);
    }
}