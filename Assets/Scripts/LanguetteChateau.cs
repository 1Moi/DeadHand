using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguetteChateau : MonoBehaviour
{
    public Vector3 TargetPosition;
    public Vector3 MauvaisePosition;
    public LanguetteGoToCranCall lalou;
    public GameObject RewardRec;

    [Header("Audio")]
    [SerializeField] private AudioClip[] AudioWin;
    [SerializeField] private AudioClip[] AudioPage;

    private bool hasPlayedWinSound = false;

    void Update()
    {
        // Si on atteint la bonne position pour la première fois
        if (transform.localPosition == TargetPosition && !hasPlayedWinSound)
        {
            RewardRec.SetActive(true);
            hasPlayedWinSound = true;

            if (AudioWin != null && AudioWin.Length > 0)
            {
                int indexWin = Random.Range(0, AudioWin.Length);
                AudioClip winClip = AudioWin[indexWin];

                GlobalSoundManager.PlaySFX(winClip);
                StartCoroutine(PlayPageSoundAfterDelay(winClip.length));
            }
            else
            {
                // Si pas de AudioWin, on joue directement AudioPage
                StartCoroutine(PlayPageSoundAfterDelay(0f));
            }
        }

        // Si on atteint la mauvaise position
        if (transform.localPosition == MauvaisePosition)
        {
            lalou.CallLanguetteGotoCran(1);
        }

        // Reset si l’objet quitte la bonne position
        if (transform.localPosition != TargetPosition && hasPlayedWinSound)
        {
            hasPlayedWinSound = false;
        }
    }

    private IEnumerator PlayPageSoundAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay + 0.1f); // petit délai en plus pour éviter un chevauchement

        if (AudioPage != null && AudioPage.Length > 0)
        {
            int indexPage = Random.Range(0, AudioPage.Length);
            GlobalSoundManager.PlaySFX(AudioPage[indexPage]);
        }
    }
}
