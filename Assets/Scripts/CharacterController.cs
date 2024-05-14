using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust this to change movement speed

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    [SerializeField] private Camera cam;
    private Vector2 mousePos;
    
    
    public GameObject bow;
    public GameObject arrow;
    public Rigidbody2D arrowRb;
    public float arrowSpeed;
    public float timePassed;
    
    
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
        
        
        if (Input.GetKey(KeyCode.Mouse1))
        {
            
            isShooting = true;
            bow.GetComponent<SpriteRenderer>().enabled = true;
            bowAnimator.SetBool("isShooting", isShooting);

        }

        else
        {
            isShooting = false;
            bow.GetComponent<SpriteRenderer>().enabled = false;
            bowAnimator.SetBool("isShooting", isShooting);

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
        arrow.transform.rotation = Quaternion.Euler(0f,0f,180 + angle);
        arrow.transform.position = rb.position;
        
        arrowRb.velocity = lookDir * arrowSpeed;
        
    }
}