using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguetteChateau : MonoBehaviour
{
    public Vector3 TargetPosition;
    public GameObject RewardRec;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if (transform.localPosition == TargetPosition)
            RewardRec.SetActive(true);
    }
    
}
