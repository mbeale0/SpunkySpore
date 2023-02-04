using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCheck : MonoBehaviour
{
    [SerializeField] private GameObject winCanvas = null;

    private void Start()
    {
        winCanvas.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Time.timeScale = 0;
            winCanvas.SetActive(true);
        }
    }
}
