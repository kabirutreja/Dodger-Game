using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XYPlayer : MonoBehaviour
{
    public Animator animator;
    public KeyCode jumpkey = KeyCode.Space;
    [Header("XY")]
    public float speedxy;
    public float groundDrag;
    public Transform orientation;
    float xinput;
    float yinput;
    Vector3 xydirection;
    Rigidbody rb;
    [Header("Ground Check")]
    //player height is 2.0f, so ground distance is 0.3f
    public float PlayerHeight = 2.0f;
    public LayerMask groundLayer;
     bool isGrounded = true;
    public float jumpCooldown = 0.25f;
    public float jumpForce;
    public float airMultiplier;
    bool readytojump = true;
    
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey(jumpkey) && readytojump && isGrounded)
        {
            readytojump = false;
            Jump();
            Invoke(nameof(resetjump), jumpCooldown);
        }
        xinput = Input.GetAxisRaw("Horizontal");
        yinput = Input.GetAxisRaw("Vertical");
        isGrounded = Physics.Raycast(transform.position, Vector3.down, PlayerHeight * 0.5f + 0.3f, groundLayer);
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
        Speedcontrol();
    }
    private void Playermove()
    {
        xydirection = orientation.forward * yinput + orientation.right * xinput;
        if (xydirection.magnitude > 0)
        {
            //animator.SetBool("Walking", true);
        }
        else
        {
            //animator.SetBool("Walking", false);
        }
        if (isGrounded)
        {
            rb.AddForce(xydirection.normalized * speedxy * 10f, ForceMode.Force);
        }
        else if (!isGrounded)
        {
            rb.AddForce(xydirection.normalized * speedxy * 10f * airMultiplier, ForceMode.Force);
        }
        
    }
    private void FixedUpdate()
    {
        Playermove();
    }
    private void Speedcontrol()
    {
       Vector3 flatvel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
       if (flatvel.magnitude > speedxy)
        {
            Vector3 limitedvel = flatvel.normalized * speedxy;
            rb.velocity = new Vector3(limitedvel.x, rb.velocity.y, limitedvel.z);
        }
    }
    private void Jump()
    {
        //animator.SetTrigger("Jump");
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void resetjump()
    {
        readytojump = true;
    }
}
