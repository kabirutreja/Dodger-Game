using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingTex : MonoBehaviour
{
    public float scrollMultiplier = 0.5f;
    public Vector2 scrollDirection = Vector2.up;
    public bool scrollWhenDead = false;
    public string texturePropertyName = "_MainTex";
    private Material mat;
    private Vector2 offset;
    void Awake()
    {
        mat = GetComponent<Renderer>().material;
    }
    
   

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance == null || GameManager.Instance.IsPlayerDead && !scrollWhenDead)
        {
            return;
        }
        float speed = GameManager.Instance.AscentSpeed * scrollMultiplier;
        offset += scrollDirection * speed * Time.deltaTime;
        offset.x %= 1f;
        offset.y %= 1f;
        mat.SetTextureOffset(texturePropertyName, offset);
    }
}
