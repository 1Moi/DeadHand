using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private Material material;

    private float DissolveAmount;
    public bool IsDissolving;

    private void Update()
    {
        if (IsDissolving)
        {
            DissolveAmount = Mathf.Clamp01(DissolveAmount + Time.deltaTime);
            material.SetFloat("_DissolveAmount", DissolveAmount);
        }

        else
        {
            DissolveAmount = Mathf.Clamp01(DissolveAmount - Time.deltaTime);
            material.SetFloat("_DissolveAmount", DissolveAmount);
        }
        
        if (Input.GetKeyDown(KeyCode.T))
        {
                IsDissolving = true;
                UnityEngine.Debug.Log("DIssolve COMMENCE");
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            IsDissolving = false;
            UnityEngine.Debug.Log("DIssolve RETREAT");
        }

    }
}