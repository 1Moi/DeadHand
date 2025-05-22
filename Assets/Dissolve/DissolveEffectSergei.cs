using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;


public class DissolveEffectSergei : MonoBehaviour
{
    private Material runtimeMaterial;
    private Renderer rend;

    private float DissolveAmount = 0f;
    public bool IsDissolvingSergei;
    private bool wasDissolving = false;
    private bool hasBeenHidden = false;

    public GameObject Sergei;

    [Header("Audio")]
    [SerializeField] private AudioClip AudioFeu;

    void Start()
    {
        rend = GetComponent<Renderer>();
        runtimeMaterial = rend.material;
    }

    private void Update()
    {
        if (IsDissolvingSergei)
        {
            if (!wasDissolving)
            {
                GlobalSoundManager.PlaySFX(AudioFeu);
                wasDissolving = true;
                hasBeenHidden = false;
                UnityEngine.Debug.Log("SON LANCÉ !");
            }

            DissolveAmount = Mathf.Clamp01(DissolveAmount + Time.deltaTime);
            runtimeMaterial.SetFloat("_DissolveAmount", DissolveAmount);

            // Désactive l'objet seulement quand l'effet est terminé
            if (DissolveAmount >= 1f && !hasBeenHidden)
            {
                Sergei.SetActive(false);
                hasBeenHidden = true;
            }
        }
        else
        {
            wasDissolving = false;
            hasBeenHidden = false;

            DissolveAmount = Mathf.Clamp01(DissolveAmount - Time.deltaTime);
            runtimeMaterial.SetFloat("_DissolveAmount", DissolveAmount);

            if (DissolveAmount <= 0f)
            {
                Sergei.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            IsDissolvingSergei = true;
            UnityEngine.Debug.Log("Dissolve COMMENCE");
        }
    }
}
