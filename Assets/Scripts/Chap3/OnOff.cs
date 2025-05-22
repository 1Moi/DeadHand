using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnOff : MonoBehaviour, IPointerDownHandler
{
    public GameObject Halo;
    public bool isOn = true;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isOn)
        {
            Halo.SetActive(false);
            isOn = false;
        }
        else
        {
            Halo.SetActive(true);
            isOn = true;
        }
    }
}
