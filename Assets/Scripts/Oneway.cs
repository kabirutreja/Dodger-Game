using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oneway : MonoBehaviour
{
    private Collider playercollider;
    // Start is called before the first frame update
    void Start()
    {
      playercollider = GetComponent<Collider>();  
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Oneway"))
        {
            Collider othercollider = collision.collider;
            foreach (ContactPoint contact in collision.contacts)
            {
                if (contact.point.y < playercollider.bounds.min.y)
                {
                    StartCoroutine(DisableCollisionTemporarily(othercollider));
                    break;
                }
            }
        }
    }
    private IEnumerator DisableCollisionTemporarily(Collider othercollider)
    {
        Physics.IgnoreCollision(playercollider, othercollider, true);
        while(playercollider.bounds.min.y < othercollider.bounds.max.y)
        {
            yield return null;
        }
    
        Physics.IgnoreCollision(playercollider, othercollider, false);
    }
}
