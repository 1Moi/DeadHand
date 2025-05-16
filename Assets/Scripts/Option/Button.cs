using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Audio")]
    [SerializeField] private AudioClip[] AudioClick;
    [SerializeField] private int valeur;
    [SerializeField] private enum ButtonType {Musique, SFX, Ambiance, UI};
    [SerializeField] private ButtonType Name;
    [SerializeField] private OptionMenu OptionMenu;


    public void OnPointerEnter(PointerEventData eventData)
    {
        // Hover effect
        
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        // Optional: Add any exit logic here
        // remove hover effect
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        // Play click sound
        int index = Random.Range(0, AudioClick.Length);
        GlobalSoundManager.PlaySFX(AudioClick[index]);

        OptionMenu.SetSound(valeur, Name.ToString());
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
