using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UVActivation : MonoBehaviour, IPointerDownHandler
{
    public List<GameObject> ObjectToSetActive;
    public GameObject Lampe;

    [Header("Audio")]
    [SerializeField] private AudioClip AudioClick;
    [SerializeField] private AudioClip AudioLoop;


    public void OnPointerDown(PointerEventData eventData)
    {
        for (int i = 0; i < ObjectToSetActive.Count; i++)
        {
            Lampe.SetActive(false);
            GameObject obj = ObjectToSetActive[i];
            if (obj != null)
                obj.SetActive(true);

            GlobalSoundManager.PlaySFX(AudioClick);
            GlobalSoundManager.PlayAmbiance(AudioLoop);
        }
    }
}
