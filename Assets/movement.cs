using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI; //use Button
using UnityEngine.EventSystems;


public class movement : MonoBehaviour
{

    public Rigidbody2D rb;
    [SerializeField] int jumpPower;
    [SerializeField] int speedPlayer;
    
    public FixedJoystick joystick;

    private enum State
    {
        idle,
        running,
        jumping,
        falling,
        hurt
    }
    private State state = State.idle;

    //check jumb on ground   
    public bool isGround;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Animator ani;
    [SerializeField] private AudioSource au;
    [SerializeField] private int cherries = 0;
    [SerializeField] private TextMeshProUGUI cherriesText;
    [SerializeField] private float hurtForce = 100f;
    private Collider2D coll;
    // Start is called before the first frame update

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }
    

    // Update is called once per frame
    void Update()
    {
        //movement
        
        if (state != State.hurt)
        {
            movementPlayer();
        }
        Debug.Log(isGround);
   
        switchStatePlayer();
        ani.SetInteger("state", (int)state);

    }

    //eat cherries
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Cherries")
        {
            Destroy(collision.gameObject);
            cherries += 1;
            cherriesText.text = cherries.ToString();
        }
        
    }
    public void movementPlayer()
    {
        
        isGround = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(1.8f, 0.3f), CapsuleDirection2D.Horizontal, 0, groundLayer);
        float horizontalPlayer = Input.GetAxisRaw("Horizontal");
        float verticalPlayer = Input.GetAxisRaw("Vertical");


        rb.velocity = new Vector2(joystick.Horizontal*speedPlayer, rb.velocity.y);
        if (joystick.Horizontal<0)
        {
            rb.transform.localScale = new Vector3(-1, 1, 1);
        }
        if (joystick.Horizontal>0)
        {  
            rb.transform.localScale = new Vector3(1, 1, 1);
        }
        // rb.velocity = new Vector2(horizontalPlayer*joystick.Horizontal, rb.velocity.y);

        //jumb
        if ((joystick.Vertical>0) && isGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            state = State.jumping;

        }

    }
    
    //enemy and player
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Frog")
        {
            if (state == State.falling)
            {
                Debug.Log("not bug");
                Destroy(other.gameObject);
                cherries =+3;
            }
            else
            {
                state = State.hurt;
                if (other.gameObject.transform.position.x > transform.position.x)
                {
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                }
            }
        }
        else if(other.gameObject.tag == "EndGame")
        {
            Debug.Log("Win");
        }
    }

    private void switchStatePlayer()
    {
        if (state == State.jumping)
        {
            if (rb.velocity.y < .1f)
            {
                state = State.falling;
            }
        }
        else if (state == State.falling)
        {
            if (coll.IsTouchingLayers(groundLayer))
            {
                state = State.idle;
            }
        }
        else if (state == State.hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }
        else if (Mathf.Abs(rb.velocity.x) > 2f)
        {
            state = State.running;
        }
        else if (Mathf.Abs(rb.velocity.x) <= .1f)
        {
            state = State.idle;
        }

    }


}
