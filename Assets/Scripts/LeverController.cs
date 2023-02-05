using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeverController : MonoBehaviour
{
    [SerializeField] private Texture flippedImage = null;
    [SerializeField] private GameObject doors = null;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameObject.GetComponent<RawImage>().texture = flippedImage;
            doors.SetActive(false);
        }
    }
}
