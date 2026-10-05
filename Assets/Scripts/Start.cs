using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start : MonoBehaviour
{
    bool isstarted = false;
    // Start is called before the first frame update

    void Update()
    {
        if(Input.anyKeyDown && !isstarted)
        {
            isstarted = true;
            GameManager.Instance.platformSpawner.isworking = true;
            //GameManager.Instance.scoreDisplay.SetVisible(true);
            GameManager.Instance.scoreDisplay.SetScore(0);
            //GameManager.Instance.AscentSpeed = GameManager.Instance.startingSpeed;
            Time.timeScale = 1f;
        }
    }
}
