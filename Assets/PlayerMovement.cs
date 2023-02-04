using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Vector2 speed = new Vector2(50, 50);
    private SpriteRenderer spriteRenderer = null;
    private Color hiddenColor;
    private bool isHiding = false;
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.material.color = new Vector4(1, 0, 0, 1);

    }
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isHiding)
            {
                spriteRenderer.material.color = new Vector4(1, 0, 0, 1);
                isHiding = false;
            }
            else
            {
                spriteRenderer.material.color = new Vector4(.5f, 0, 0, .4f);
                isHiding = true;
            }
        }
        if (!isHiding)
        {
            float inputX = Input.GetAxis("Horizontal");
            float inputY = Input.GetAxis("Vertical");
            Vector3 mvmt = new(speed.x * inputX, 0, 0);
            mvmt *= Time.deltaTime;
            transform.Translate(mvmt);
        }
        
    }
}
