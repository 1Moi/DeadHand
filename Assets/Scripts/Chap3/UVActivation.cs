using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UVActivation : MonoBehaviour, IPointerDownHandler
{
    public List<GameObject> ObjectToSetActive;


    public void OnPointerDown(PointerEventData eventData)
    {
        for (int i = 0; i < ObjectToSetActive.Count; i++)
        {
            GameObject obj = ObjectToSetActive[i];
            if (obj != null)
                obj.SetActive(true);
        }
    }
}
