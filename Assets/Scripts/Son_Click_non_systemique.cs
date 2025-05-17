using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Son_Click_non_systemique : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip sonClickNonSystemique;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (sonClickNonSystemique != null)
            {
                GlobalSoundManager.PlayUI(sonClickNonSystemique);
            }
        }   
    }
}
