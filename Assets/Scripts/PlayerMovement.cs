using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Vector2 speed = new Vector2(50, 50);
    [SerializeField] public float grappleSpeed;
    [SerializeField] private GameObject loseCanvas = null;

    private Vector3 grappleVelocity = new Vector3(0, 0, 0);
    public Vector3 grapplePoint;
    private LineRenderer line;

    private SpriteRenderer spriteRenderer = null;
    private Color hiddenColor;
    private bool isHiding = false;
    public bool isCaptured = false;

    public LayerMask whatIsGround;
    public LayerMask whatIsMycelium;
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.material.color = new Vector4(1, 0, 0, 1);
        line = transform.GetChild(0).GetComponent<LineRenderer>();
        line.SetPosition(1, Vector3.zero);
        loseCanvas.SetActive(false);
    }
    void Update()
    {
        if (!isCaptured)
        {
            // TODO: Add check for on mycellium block
            if (Input.GetKeyDown(KeyCode.E) && grappleVelocity == Vector3.zero)
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
                if (grapplePoint != Vector3.zero)
                {
                    GetComponent<Rigidbody2D>().isKinematic = true;
                    line.transform.position = transform.position + Mathf.Sign(grapplePoint.x - transform.position.x) * (Vector3.right / 2);
                    line.SetPosition(1, grapplePoint - line.transform.position);
                    grappleVelocity = new Vector3(grapplePoint.x - transform.position.x - Mathf.Sign(grapplePoint.x - transform.position.x), grapplePoint.y - transform.position.y, 0).normalized * grappleSpeed;
                    transform.Translate(grappleVelocity * Time.deltaTime);

                    if (line.GetPosition(1).magnitude < 2)
                    {
                        line.SetPosition(1, Vector3.zero);
                        GetComponent<Rigidbody2D>().velocity = grappleVelocity / 4.5f;
                        grappleVelocity = Vector3.zero;
                        grapplePoint = Vector3.zero;
                        GetComponent<Rigidbody2D>().isKinematic = false;
                        AkSoundEngine.PostEvent("GrappleUnlatch", gameObject);
                    }
                }
                else
                {
                    float inputX = Input.GetAxis("Horizontal");
                    float inputY = Input.GetAxis("Vertical");
                    Vector3 mvmt = new(speed.x * inputX, 0, 0);
                    mvmt *= Time.deltaTime;
                    transform.Translate(mvmt);
                }
            }

            if (grapplePoint == Vector3.zero)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                    line.transform.position = transform.position + Mathf.Sign(mousePos2D.x - transform.position.x) * (Vector3.right / 1.99f);
                    grappleVelocity = new Vector3(mousePos2D.x - transform.position.x - Mathf.Sign(mousePos2D.x - transform.position.x), mousePos2D.y - transform.position.y, 0).normalized * grappleSpeed;
                    AkSoundEngine.PostEvent("GrappleShoot", gameObject);
                }

                line.SetPosition(1, line.GetPosition(1) + (grappleVelocity * Time.deltaTime));

                RaycastHit2D ray = Physics2D.Raycast(line.transform.position, grappleVelocity, line.GetPosition(1).magnitude, whatIsGround);

                if (Physics2D.Raycast(line.transform.position, grappleVelocity, line.GetPosition(1).magnitude, whatIsMycelium))
                {
                    Debug.Log("WHEEEEEEE");
                    grapplePoint = ray.point;
                    AkSoundEngine.PostEvent("GrappleLatch", gameObject);
                } else if(line.GetPosition(1).magnitude > 50 || Physics2D.Raycast(line.transform.position, grappleVelocity, line.GetPosition(1).magnitude, whatIsGround))
                {
                    line.SetPosition(1, Vector3.zero);
                    grappleVelocity = Vector3.zero;
                    grapplePoint = Vector3.zero;
                    AkSoundEngine.PostEvent("GrappleUnlatch", gameObject);
                }
            }
        }
        else
        {
            GetComponent<Rigidbody2D>().isKinematic = true;
            line.SetPosition(1, Vector3.zero);
            transform.Translate(Vector3.zero);
            Time.timeScale = 0;
            loseCanvas.SetActive(true);
        }

    }

    public bool getHiding()
    {
        return isHiding;
    }
}