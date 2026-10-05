using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playershadow : MonoBehaviour
{
    public Transform player;
    public LayerMask groundLayer;
    public float maxCastDistance = 100f;
    public float surfaceOffset = 0.05f;
    public bool scaleWithHeight = true;
    public float maxShadowDistance = 10f;
    public float minScale = 0.5f;
    private Vector3 baseScale;
    private Renderer rend;
    void Awake()
    {
        rend = GetComponent<Renderer>();
        baseScale = transform.localScale;
    }
    // Start is called before the first frame update
    public void Update()
    
        
    
    {
      Vector3 origin = player.position;
      float surfaceY;
      if (Physics.Raycast(origin + Vector3.up * surfaceOffset, Vector3.down, out RaycastHit hit, maxCastDistance, groundLayer,QueryTriggerInteraction.Ignore ))
      {
          surfaceY = hit.point.y;
      }
      else
      {
         surfaceY = GameManager.Instance.lavaHeight;
      }
     transform.position = new Vector3(origin.x, surfaceY + surfaceOffset, origin.z);
      if (scaleWithHeight)
      {
         float drop = Mathf.Clamp(player.position.y - surfaceY, 0f, maxShadowDistance);
         float k = 1f - drop / maxShadowDistance;
        float scale = Mathf.Lerp(minScale, 1f, k);
          transform.localScale = baseScale * scale;
          if (rend != null)
          {
              Color c = rend.material.color;
              c.a = Mathf.Lerp(0.15f, 0.6f, k);
              rend.material.color = c;
          }
      }
    

}}
