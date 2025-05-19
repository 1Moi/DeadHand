using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Unity.VisualScripting;

public class ResetButton : MonoBehaviour, IPointerDownHandler
{

    [Header("Audio")]
    [SerializeField] private AudioClip[] AudioClick;

    public GridManager gridManager;

    public void OnPointerDown(PointerEventData eventData)
    {
        gridManager.ClearGrid();

        if (AudioClick != null && AudioClick.Length > 0)
        {
            int index = Random.Range(0, AudioClick.Length);
            GlobalSoundManager.PlaySFX(AudioClick[index]);
        }


    }
}
