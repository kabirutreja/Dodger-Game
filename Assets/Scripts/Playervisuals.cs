using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playervisuals : MonoBehaviour
{
    public PlayerController playerController;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public bool billboardtoCamera = true;
    public string groundedParam = "Grounded";
    public string speedParam = "Speed";
    private Camera cam;

    void Awake()
    {
        cam = Camera.main;
        if (playerController == null)
        {
            playerController = GetComponentInParent<PlayerController>();
        }
    }
    void LateUpdate()
    {
        if(playerController == null || animator == null || spriteRenderer == null)
        {
            return;
        }
       Vector3 vel = playerController.HorizontalVelocity;
       bool grounded = playerController.IsGrounded;
       animator.SetBool(groundedParam, grounded);
        animator.SetFloat(speedParam, vel.magnitude);
       float screenX = vel.x - vel.z;
        if(Mathf.Abs(screenX) > 0.05f)
        {
           spriteRenderer.flipX = screenX < 0f;
        }
        if(billboardtoCamera && cam != null)
       {
              transform.forward = cam.transform.forward;
        }
    }

    
}
