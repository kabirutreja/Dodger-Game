using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transparent : MonoBehaviour
{
    public bool isworking = false;
    public float transparency = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if (isworking)
       {
         MeshRenderer[] rend = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in rend)
            {
                foreach(Material mat in renderer.materials)
                {
                    SetMaterialTransparent(mat);
                    Color color = mat.color;
                    color.a = transparency;
                    mat.color = color;
                }
            }
       } 
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
}
