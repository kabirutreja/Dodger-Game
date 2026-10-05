using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platformspawner : MonoBehaviour
{
    public GameObject platformPrefab;
    public float gridSpacing;
    public float spawnHeight;
    public Vector2 gridCenter;
    public float spawnIntervalBase;
    //Always the center slot for the first platform, to ensure the player starts in a safe position
    [Range(0, 8)] public int firstPlatformSlot = 4;
    public float FirstPlatformHeight = 0f;
    public Transform player;
    private int lastSlot = 4;
    private int secondLastSlot = -1;
    public bool isworking = true;
    
    void Start()
    {
        //CreateFirstPlatform();
        StartCoroutine(SpawnLoop());
    }
    void CreateFirstPlatform()
    {
        int n = firstPlatformSlot;
        secondLastSlot = lastSlot;
        lastSlot = n;
        Vector3 pos = SlotToPosition(n, FirstPlatformHeight);
        GameObject go = Instantiate(platformPrefab, pos, Quaternion.identity);
        if (player != null)
{
    float top = pos.y + (platformPrefab.transform.localScale.y / 2f);
    CharacterController cc = player.GetComponent<CharacterController>();

    // Distance from the pivot down to the capsule's bottom
    float pivotToBottom = (cc != null) ? (cc.height * 0.5f - cc.center.y) * player.lossyScale.y : 0.5f;

    if (cc != null) cc.enabled = false;
    player.position = new Vector3(pos.x, top + pivotToBottom + 0.02f, pos.z);
    if (cc != null) cc.enabled = true;

    Physics.SyncTransforms();
}
        go.GetComponent<Platform>().PlaySpawnAnimation();
        }
        IEnumerator SpawnLoop()
    {
        while (true)
        {
            float speed = GameManager.Instance.AscentSpeed;
            //Takes more than 5 seconds
            yield return new WaitForSeconds(spawnIntervalBase / speed);
            if (GameManager.Instance.IsPlayerDead) yield break;
            CreatePlatform();
        }
    }
    

  
    void CreatePlatform()
    {if (isworking)
    {
        AudioSource spawnSound = GetComponent<AudioSource>();
        spawnSound.Play();
        int n = PickSlot();
        
        Vector3 pos = SlotToPosition(n, spawnHeight);
       
        GameObject go = Instantiate(platformPrefab, pos, Quaternion.identity);
        if (n == 7)
        {
            MeshRenderer[] rend = go.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in rend)
            {
                foreach(Material mat in renderer.materials)
                {
                    SetMaterialTransparent(mat);
                    Color color = mat.color;
                    color.a = 0.5f;
                    mat.color = color;
                }
            }
        }
        lastSlot = n;
        go.GetComponent<Platform>().PlaySpawnAnimation();
        GameManager.Instance.OnPlatformSpawned();}
    }
    void SetMaterialTransparent(Material mat)
    {
        mat.SetFloat("_Mode",3);
        mat.SetInt("_SrcBlend",(int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend",(int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite",0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("ALPHAMULTIPLY_ON");
        mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    }
   int PickSlot()
    {
        int n = Random.Range(0, 9);
        for (int guard = 0; guard < 9; guard++)
        {
            if (n != lastSlot && n != secondLastSlot) return n;
            n = (n + 1) % 9;
        }
        return n;
    }
    Vector3 SlotToPosition(int slot, float height)
    {
       int col = slot % 3;
       int row = slot / 3;
       float x = gridCenter.x + (col - 1) * gridSpacing;
       float z = gridCenter.y + (1-row) * gridSpacing;
       return new Vector3(x, height, z);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        for (int i = 0; i < 9; i++)
        {
            Gizmos.DrawWireCube(SlotToPosition(i, spawnHeight), Vector3.one * 0.6f);
            Gizmos.DrawWireCube(SlotToPosition(i, FirstPlatformHeight), Vector3.one * 0.4f);
        }
    }
}

