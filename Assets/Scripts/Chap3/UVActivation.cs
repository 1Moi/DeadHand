using UnityEngine;
using UnityEngine.EventSystems;

public class UVActivation : MonoBehaviour, IPointerDownHandler
{
    public GameObject UVLight;
    public GameObject NightScreen;
    public GameObject ButtonPage1;
    public GameObject ButtonPage2;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (UVLight != null)
            UVLight.SetActive(true);

        if (NightScreen != null)
            NightScreen.SetActive(true);

        if (ButtonPage1 != null)
            ButtonPage1.SetActive(true);

        if (ButtonPage2 != null)
            ButtonPage2.SetActive(true);
    }
}
