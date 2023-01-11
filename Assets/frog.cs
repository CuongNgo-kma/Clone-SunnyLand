using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frog : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;
    [SerializeField] private float jumpLenght = 10f;
    [SerializeField] private float jumpHeight = 15f;
    public bool isGround;
    public Transform groundCheck;
    public LayerMask groundLayer;
    private bool facingLeft = true;
    public Collider2D coll;
    public Rigidbody2D rb;

    private Animator animator;
    void Start()
    {
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (animator.GetBool("jumping"))
        {
            if (rb.velocity.x < .1f)
            {
                animator.SetBool("falling", true);
                animator.SetBool("jumping", false);
                
            }

        }
        if (coll.IsTouchingLayers(groundLayer) && animator.GetBool("falling"))
        {
            animator.SetBool("falling", false);
        }
    }

    private void Move()
    {
        isGround = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(1.8f, 0.3f), CapsuleDirection2D.Horizontal, 0, groundLayer);

        if (facingLeft)
        {
            if (transform.position.x > leftCap)
            {
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);

                }
                if (coll.IsTouchingLayers(groundLayer))
                {
                    rb.velocity = new Vector2(-jumpLenght, jumpHeight);
                    animator.SetBool("jumping", true);
                }
            }
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < rightCap)
            {
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }
                if (coll.IsTouchingLayers(groundLayer))
                {
                    rb.velocity = new Vector2(jumpLenght, jumpHeight);
                    animator.SetBool("jumping", true);
                }
            }
            else
            {
                facingLeft = true;
            }
        }
    }
    // private void stateSwitch()
    // {
    //     isGround = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(1.8f, 0.3f), CapsuleDirection2D.Horizontal, 0, groundLayer);

    //     if (rb.velocity.x <= .1f)
    //     {
    //         state = State.idle;
    //     }

    //     else if (isGround)
    //     {
    //         state = State.jumpUp;
    //     }
    //     else if (state == State.jumpUp)
    //     {
    //         state = State.jumpFall;
    //     }
    // }
}
