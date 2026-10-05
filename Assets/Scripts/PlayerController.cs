//using System.Threading.Tasks.Dataflow;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public ParticleSystem jumpeffect;
    public float duration = 1f;
    public GameObject cam;
    public AnimationCurve animationcur;
    public float moveSpeed = 5f;
 
    
    public float acceleration = 40f;
 
  
    public float groundCheckDistance = 0.15f;
     public LayerMask platformLayer;
 
       public float coyoteTime = 0.1f;
 
      public float jumpBuffer = 0.12f;
 
    
    private CharacterController cc;
    private Vector3 horizontalVelocity;
    private float verticalVelocity;    
    private bool isGrounded= false;           
    private float coyoteTimer;
    private float jumpBufferTimer;
    public Animator anim;
    public GameObject pauseMenu;
    bool isPaused = false;
 
   
    public bool IsGrounded => isGrounded;
    public AudioSource jump;
    public AudioSource landSound;
    public AudioSource hoverSound;
    public AudioSource buttonSound;
    public AudioSource pausemenuSound;
    

private bool wasGrounded = true;
    public Vector3 HorizontalVelocity => horizontalVelocity;
 
    void Awake()
    {
        cc = GetComponent<CharacterController>();
    }
 
    void Update()
    {
        
        if (GameManager.Instance.IsPlayerDead) return;

        if(Input.GetKeyDown(KeyCode.Escape)  || Input.GetKeyDown(KeyCode.P))
        {
            if(isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
            
        }
 
        ReadInput();
        HandleGroundCheck();
        HandleJump();
        ApplyGravity();
        MovePlayer();
        CheckDeath();
    }
    void PauseGame()
    {
        pausemenuSound.Play();
        Time.timeScale = 0f;
        GameManager.Instance.platformSpawner.isworking = false; 
        pauseMenu.SetActive(true);
        isPaused = true;
    }
    public void ResumeGame()
    {
        pausemenuSound.Play();
        Time.timeScale = 1f;
        GameManager.Instance.platformSpawner.isworking = true; 
        pauseMenu.SetActive(false);
        isPaused = false;
    }
    public void buttonSoundEffect()
    {
        buttonSound.Play();
    }
    public void hoverSoundEffect()
    {
        hoverSound.Play();
    }

    void ReadInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
 
        Vector3 input = new Vector3(h, 0f, v);
        

        if (input.sqrMagnitude > 1f) input.Normalize();
 
        Vector3 target = input * moveSpeed;
        horizontalVelocity = Vector3.MoveTowards(
            horizontalVelocity, target, acceleration * Time.deltaTime);
 
      
        if (Input.GetKeyDown(KeyCode.Space))
            jumpBufferTimer = jumpBuffer;
        else
            jumpBufferTimer -= Time.deltaTime;
    }
 
        void HandleGroundCheck()
{
    Vector3 pos = GetGroundCheckPos(out float radius);

    isGrounded = Physics.CheckSphere(pos, radius, platformLayer, QueryTriggerInteraction.Ignore);

    if (isGrounded && verticalVelocity > 0.1f)
        isGrounded = false;
         if (isGrounded && !wasGrounded)
    {
        if (landSound != null)
            landSound.Play();
    }
    wasGrounded = isGrounded;

    if (isGrounded) coyoteTimer = coyoteTime;
    else            coyoteTimer -= Time.deltaTime;
}

  Vector3 GetGroundCheckPos(out float radius)
{
    radius = cc.radius * 0.9f;
    Bounds b = cc.bounds;                       // world-space capsule bounds
    Vector3 bottom = new Vector3(b.center.x, b.min.y, b.center.z);
    return bottom + Vector3.up * (radius - groundCheckDistance);
}
void OnDrawGizmos()
{
    if (cc == null) cc = GetComponent<CharacterController>();
    if (cc == null) return;

    Vector3 pos = GetGroundCheckPos(out float radius);
    bool hit = Physics.CheckSphere(pos, radius, platformLayer, QueryTriggerInteraction.Ignore);
    Gizmos.color = hit ? Color.green : Color.red;
    Gizmos.DrawWireSphere(pos, radius);
}
 
   
    void HandleJump()
    {
        if (jumpBufferTimer > 0f && coyoteTimer > 0f)
        {
            jumpeffect.transform.position = transform.position;
            jumpeffect.Play();
            jump.Play();
            verticalVelocity = GameManager.Instance.jumpStrength;
            jumpBufferTimer = 0f;
            coyoteTimer = 0f;
            isGrounded = false;
        }
    }
 
    
    void ApplyGravity()
    {
        float descent = -GameManager.Instance.AscentSpeed;
 
        if (isGrounded)
        {
            verticalVelocity = descent;
        }
        else
        {
            verticalVelocity -= GameManager.Instance.gravity * Time.deltaTime;
        }
 
       
        if ((cc.collisionFlags & CollisionFlags.Above) != 0 && verticalVelocity > 0f)
            verticalVelocity = -2f;
    }
 
    
    void MovePlayer()
    {
        Vector3 motion = horizontalVelocity;
        motion.y = verticalVelocity;
        cc.Move(motion * Time.deltaTime);
    }
 
   
    void CheckDeath()
    {
        if (transform.position.y < GameManager.Instance.lavaHeight)
        {
            StartCoroutine(Shaking());
            anim.SetTrigger("Die");
            cc.enabled = false;
            transform.position = new Vector3(
                transform.position.x,
                GameManager.Instance.lavaHeight,
                transform.position.z);
 
            verticalVelocity = 0f;
            horizontalVelocity = Vector3.zero;
 
            GameManager.Instance.KillPlayer(transform.position);
        }
    }
    
    IEnumerator Shaking()
    {
        Vector3 startpos = cam.transform.position;
        float elapsedtime = 0f;
        while (elapsedtime < duration)
        {
            elapsedtime += Time.deltaTime;
            float strength = animationcur.Evaluate(elapsedtime/duration);
            cam.transform.position = startpos + Random.insideUnitSphere * strength;
            yield return null;
        }
        cam.transform.position = startpos;
    }
}