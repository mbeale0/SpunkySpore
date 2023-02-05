using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
    [SerializeField] GameObject bossDoors = null;

    private int lives = 3;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            bossDoors.SetActive(false);
            lives--;
            if (lives == 0) { Destroy(gameObject.transform.parent.gameObject); }
        }
    }
}
