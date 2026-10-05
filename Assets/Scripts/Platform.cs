using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float destroyBelowY = -5f;
    public float spawnAnimDuration = 0.5f;
    public float overshoot = 0.25f;
    private Vector3 fullScale;
    private Renderer[] renderers;
    public ParticleSystem dieEffect;
    Animator anim;
    bool destroyed = false;
    void Awake()
    {
        anim = GetComponent<Animator>();
        fullScale = transform.localScale;
        renderers = GetComponentsInChildren<Renderer>();
    }
    void Start()
    {
        dieEffect = GameObject.FindGameObjectWithTag("Fire").GetComponent<ParticleSystem>();
    }
    void OnEnable()
    {
         GameManager.Instance.RegisterPlatform(this);
    }
    void OnDisable()
    {
         GameManager.Instance.UnregisterPlatform(this);
    }
    
    void Update()
    {
        float speed = GameManager.Instance.AscentSpeed;
       transform.position -= Vector3.up * speed * Time.deltaTime;
       if (transform.position.y < destroyBelowY)
       {
           Destroy(gameObject);
       }
       float more = transform.position.y - 0.1f;
        if (more <= GameManager.Instance.lavaHeight)
        {
            if (!destroyed)
            {
            
            destroyed = true;
            //Debug.Log("a");
            anim.SetTrigger("Die");
            dieEffect.transform.position = transform.position;
            dieEffect.Play();
            }
 
        }
     
    }
    public void PlaySpawnAnimation()
    {
        StartCoroutine(SpawnScaleRoutine());
    }
    private IEnumerator SpawnScaleRoutine()
    {
        float t = 0f;
        transform.localScale = Vector3.zero;
        while (t < spawnAnimDuration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / spawnAnimDuration);
            float c1 = 1.70158f * (overshoot /0.25f);
            float c3 = c1 + 1f;
            float eased = 1f + c3 * Mathf.Pow(k - 1f, 3f) + c1 * Mathf.Pow(k - 1f, 2f);
            transform.localScale = fullScale * eased;
            yield return null;
        }
        transform.localScale = fullScale;
    }
    public void FadeOut(float duration)
    {
        StartCoroutine(FadeOutRoutine(duration));
    }
    private IEnumerator FadeOutRoutine(float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float a = 1f - Mathf.Clamp01(t / duration);
            SetAlpha(a);
            yield return null;
        }
        SetAlpha(0f);
    }
    private void SetAlpha(float a)
    {
            foreach (Renderer r in renderers)
            {
                
                    Color c = r.material.color;
                    c.a = a;
                    r.material.color = c;
                
            }
            
        }
        
    }

