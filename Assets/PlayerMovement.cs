using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Vector2 speed = new Vector2(50, 50);
    [SerializeField] private TMP_Text movingTExt = null;
    private SpriteRenderer spriteRenderer = null;
    private Color hiddenColor;
    private bool isHiding = false;
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.material.color = new Vector4(1, 0, 0, 1);
        movingTExt.text = "Moving";

    }
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isHiding)
            {
                movingTExt.text = "Moving";
                spriteRenderer.material.color = new Vector4(1, 0, 0, 1);
                isHiding = false;
            }
            else
            {
                movingTExt.text = "Hiding";
                spriteRenderer.material.color = new Vector4(.5f, 0, 0, .4f);
                isHiding = true;
            }
        }
        if (!isHiding)
        {
            float inputX = Input.GetAxis("Horizontal");
            float inputY = Input.GetAxis("Vertical");
            //Debug.Log($"X: {Input.GetKeyDown(KeyCode.D)} - Y: {inputY}");
            Vector3 mvmt = new(speed.x * inputX, 0, 0);
            mvmt *= Time.deltaTime;
            transform.Translate(mvmt);
        }
        
    }
}
