using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class enemyAI : MonoBehaviour
{
    [SerializeField] private float moveSpeed = .8f;
    [SerializeField] private float aggroSpeedMultiplier = 1f;
    [SerializeField] private float jumpForce = .8f;
    [SerializeField] private Transform homeWaypt = null;

    private RaycastHit2D hit;
    private Rigidbody2D rigidBody;
    private Transform target = null;
    bool isGrounded = false;

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
        RaycastHit2D downHit = Physics2D.Raycast(transform.position, Vector2.down);
        if (Vector2.Distance(downHit.point, transform.position) < 0.8f)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        if (transform.localScale.x < 0)
        {
            hit = Physics2D.Raycast(transform.position, Vector2.left);
        }
        else
        {
            hit = Physics2D.Raycast(transform.position, Vector2.right);
        }

        if (hit.collider != null && hit.transform.gameObject.tag == "Player" && currentState != ENEMY_STATE.KILLING)
        {
            if (!hit.transform.gameObject.GetComponent<PlayerMovement>().getHiding())
            {
                target = hit.transform;
                StopAllCoroutines();
                currentState = ENEMY_STATE.AGGRO;
                StartCoroutine(AttackTimeout());
            }
        }

        switch(currentState)
        {
            case ENEMY_STATE.IDLE:
                aggroSpeedMultiplier = 1f;
                if (isGrounded && Vector2.Distance(homeWaypt.position, transform.position) > 1.5f) {
                    turnToDirection(homeWaypt.position.x - transform.position.x);
                }
                break;
            case ENEMY_STATE.AGGRO:
                aggroSpeedMultiplier = 2.5f;
                if (isGrounded && Vector2.Distance(target.position, transform.position) > 0.9f)
                {
                    turnToDirection(target.position.x - transform.position.x);
                } else if(Vector2.Distance(target.position, transform.position) < 1.5f)
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

        if (currentState != ENEMY_STATE.KILLING) {
            if (Vector2.Distance(hit.point, transform.position) < .9f && isGrounded)
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
        if(direction < -0.2 && IsFacingRight())
        {
            FlipScale();
        } else if(direction > 0.2 && !IsFacingRight())
        {
            FlipScale();
        }
    }

    private void FlipScale()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(rigidBody.velocity.x)), transform.localScale.y);
    }
}
