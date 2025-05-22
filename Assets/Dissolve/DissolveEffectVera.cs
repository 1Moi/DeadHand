using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;


public class DissolveEffectVera : MonoBehaviour
{
    private Material runtimeMaterial;
    private Renderer rend;


    private float DissolveAmount;
    public bool IsDissolvingVera;

    public GameObject Sergei;

    [Header("Audio")]
    [SerializeField] private AudioClip AudioFeu;


    void Start()
    {
        rend = GetComponent<Renderer>();
        runtimeMaterial = rend.material; // instancie le matériau à runtime
    }


    private void Update()
    {
        if (IsDissolvingVera)
        {
            DissolveAmount = Mathf.Clamp01(DissolveAmount + Time.deltaTime);
            runtimeMaterial.SetFloat("_DissolveAmount", DissolveAmount);
            GlobalSoundManager.PlaySFX(AudioFeu);
            Sergei.SetActive(false);
        }

        else
        {
            DissolveAmount = Mathf.Clamp01(DissolveAmount - Time.deltaTime);
            runtimeMaterial.SetFloat("_DissolveAmount", DissolveAmount);

        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            IsDissolvingVera = true;
            UnityEngine.Debug.Log("DIssolve COMMENCE");
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            IsDissolvingVera = false;
            UnityEngine.Debug.Log("DIssolve RETREAT");
        }

    }
}