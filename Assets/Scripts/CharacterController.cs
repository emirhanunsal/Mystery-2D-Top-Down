using Unity.VisualScripting;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust this to change movement speed

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    [SerializeField] private SpriteRenderer arrowSR;
    [SerializeField] private Camera cam;
    private Vector2 mousePos;
    
    
    [SerializeField] private GameObject bow;
    [SerializeField] private GameObject arrow;
    [SerializeField] private Rigidbody2D arrowRb;
    public float arrowSpeed;
    private float timer;
    private float startTime;
    [SerializeField] private float maxArrowSpeed;
    private bool isTimerStarted;
    
    
    bool isShooting = false;
    [SerializeField] private Animator bowAnimator;
    


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        cam = Camera.main;
    }

    void Update()
    {
        
        Move();
        Bow();

        if (isShooting)
        {
            if (!isTimerStarted)
            {
                startTime = Time.time;
                isTimerStarted = true;
            }
            
            
            FireArrow();

        }
    }



    void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

       

        if (moveX == 1)
        {
            sr.flipX = true;
        }
        else if (moveX == -1)
        {
            sr.flipX = false;
        }
        
        
        Vector2 movement = new Vector2(moveX, moveY).normalized;
        rb.velocity = movement * moveSpeed;

        if (movement == Vector2.zero)
        {
            rb.velocity = Vector2.zero;
        }

        if (isShooting)
        {
            rb.velocity = Vector2.zero;
        }
    }


    void Bow()
    {
        bowAnimator.SetBool("isShooting", isShooting);
        
        arrowSR.enabled = false;

        if (Input.GetKey(KeyCode.Mouse1))
        {
            
            isShooting = true;
            bowAnimator.SetBool("isShooting", isShooting);
            bow.GetComponent<SpriteRenderer>().enabled = true;
            arrowSR.enabled = false;


        }

        else
        {
            isShooting = false;
            bow.GetComponent<SpriteRenderer>().enabled = false;
            bowAnimator.SetBool("isShooting", isShooting);
            isTimerStarted = false;
            bowAnimator.SetBool("isMax", false);
            arrowSR.enabled = true;
        }
        
        
        
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        bow.transform.rotation = Quaternion.Euler(0f,0f,180 + angle);
        bow.transform.position = rb.position;
        
    }

    void FireArrow()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        arrow.transform.rotation = Quaternion.Euler(0f, 0f, 180 + angle);
        arrow.transform.position = rb.position;

        timer = Time.time - startTime; // Calculate time since shooting started

        // Arrow speed calculation based on elapsed time
        if (timer > 1)
        {
            arrowSpeed = maxArrowSpeed;
            bowAnimator.SetBool("isMax", true);

        }
        else if (timer < 0.3)
        {
            arrowSpeed = maxArrowSpeed * 0.5f;
            bowAnimator.SetBool("isMax", false);

        }
        else
        {
            arrowSpeed = maxArrowSpeed * timer;
            bowAnimator.SetBool("isMax", false);

        }
        
        Debug.Log(timer);
        arrowRb.velocity = lookDir.normalized * arrowSpeed; // Set arrow velocity
        arrowSR.enabled = false; // Ok görünmez olacak
        // Reset the start time for the next shot

    }


}
