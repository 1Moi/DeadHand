using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{

    public AutoFlip autoFlip;
    public Camera pageCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(autoFlip.FlipToEnd());
            pageCamera.transform.position = new Vector3(600, 0, -18);
        }
    }
}
