using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;


public class DissolveEffectSergei : MonoBehaviour
{
    private Material runtimeMaterial;
    private Renderer rend;


    private float DissolveAmount;
    public bool IsDissolvingSergei;

    [Header("Audio")]
    [SerializeField] private AudioClip AudioFeu;


    void Start()
    {
        rend = GetComponent<Renderer>();
        runtimeMaterial = rend.material; // instancie le matériau à runtime
    }


    private void Update()
    {
        if (IsDissolvingSergei)
        {
            DissolveAmount = Mathf.Clamp01(DissolveAmount + Time.deltaTime);
            runtimeMaterial.SetFloat("_DissolveAmount", DissolveAmount);
            GlobalSoundManager.PlaySFX(AudioFeu);
        }

        else
        {
            DissolveAmount = Mathf.Clamp01(DissolveAmount - Time.deltaTime);
            runtimeMaterial.SetFloat("_DissolveAmount", DissolveAmount);

        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            IsDissolvingSergei = true;
                UnityEngine.Debug.Log("DIssolve COMMENCE");
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            IsDissolvingSergei = false;
            UnityEngine.Debug.Log("DIssolve RETREAT");
        }

    }
}