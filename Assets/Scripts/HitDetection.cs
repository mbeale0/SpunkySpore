using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
    [SerializeField] GameObject bossDoors = null;

    private GameObject parent = null;
    private void Start()
    {
        parent = gameObject.transform.parent.gameObject;
    }
    private int lives = 3;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
            lives--;
            StartCoroutine(MovePlayer(collision));
            
            StartCoroutine(Flash());
            if (lives == 0) {
                bossDoors.SetActive(false); 
                Destroy(parent); 
            }
        }
    }

    private IEnumerator MovePlayer(Collider2D collision)
    {
        collision.GetComponent<Rigidbody2D>().velocity = new Vector2(-18, 20);
        yield return new WaitForSeconds(1f);
        collision.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }

    private IEnumerator Flash()
    {
        parent.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.01f, .1f));
        parent.GetComponent<SpriteRenderer>().enabled = true;

        yield return new WaitForSeconds(UnityEngine.Random.Range(0.01f, .2f));
        parent.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, .3f));
        parent.GetComponent<SpriteRenderer>().enabled = true;

        yield return new WaitForSeconds(UnityEngine.Random.Range(0.03f, .1f));
        parent.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.01f, .2f));
        parent.GetComponent<SpriteRenderer>().enabled = true;

        yield return new WaitForSeconds(UnityEngine.Random.Range(0.01f, .2f));
        parent.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, .3f));
        parent.GetComponent<SpriteRenderer>().enabled = true;
    }
}
