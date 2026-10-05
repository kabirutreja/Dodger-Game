using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerarig : MonoBehaviour
{
    public Transform player;
    public Transform centerPosition;
    public Transform deathLookTarget;
    [Range(0f, 1f)] public float horizontalInfluence = 0.125f;
    [Range(0f, 1f)] public float verticalInfluence = 0.166f;
    public float aliveSmoothing = 6f;
    public float deathSmoothing = 3f;
    public Vector3 currentLookPoint;
   
    void Start()
    {
       currentLookPoint = centerPosition.position; 
       transform.LookAt(currentLookPoint, Vector3.up);
    }
    void LateUpdate()
    {
        Vector3 desired;
        float smoothing;
        bool dead = GameManager.Instance.IsPlayerDead;
       if (!dead && player != null && centerPosition != null)
        {
            
            Vector3 c = centerPosition.position;
         Vector3 p = player.position;
 
            desired = new Vector3(
                c.x + (p.x - c.x) * horizontalInfluence,
                c.y + (p.y - c.y) * verticalInfluence,
               c.z + (p.z - c.z) * horizontalInfluence
            );
           smoothing = aliveSmoothing;
        }
        else if (deathLookTarget != null)
        {
            desired = deathLookTarget.position;
            smoothing = deathSmoothing;
        }
        else
        {
            return;
        }
 
        currentLookPoint = Vector3.Lerp(
            currentLookPoint, desired, 1f - Mathf.Exp(-smoothing * Time.deltaTime));
 
        transform.LookAt(currentLookPoint, Vector3.up);
    }
}


