using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class enemyAI : MonoBehaviour
{
    [SerializeField] private float moveSpeed = .8f;
    [SerializeField] private Transform homeWaypt = null;

    private RaycastHit2D hit;
    private Rigidbody2D rigidBody;
    private bool isAttacking = false;
    private bool isReturningHome = false;
    private Transform target = null;
    bool isGrounded = false;
    
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
            isGrounded= true;
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
        if (hit.collider != null && hit.transform.gameObject.tag == "Player")
        {
            target = hit.transform;
            StopAllCoroutines();
            isAttacking = true;
            
            StartCoroutine(AttackTimeout());
        }
        if (isReturningHome && Vector2.Distance(transform.position, homeWaypt.position) < .2f)
        {
            isReturningHome = false;
        }
        if ((isReturningHome || isAttacking) &&  Vector2.Distance(hit.point, transform.position) < .9f && isGrounded)
        {
            rigidBody.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
        }
        Move();
        

    }

    private IEnumerator AttackTimeout()
    {
        yield return new WaitForSeconds(1);
        isAttacking = false;
        isReturningHome = true;
        FlipScale();
    }

    private void Move()
    {
        if (IsFacingRight())
        {
            rigidBody.AddForce(Vector2.right * .1f, ForceMode2D.Impulse);
            //rigidBody.velocity = new Vector2(moveSpeed, 0);
        }
        else
        {
            rigidBody.AddForce(Vector2.left * .1f, ForceMode2D.Impulse);
            //rigidBody.velocity = new Vector2(-moveSpeed, 0);
        }
    }

    private bool IsFacingRight()
    {
        return transform.localScale.x > Mathf.Epsilon;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!isAttacking && !isReturningHome)
        {
            FlipScale();
        }

    }

    private void FlipScale()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(rigidBody.velocity.x)), transform.localScale.y);
    }
}
