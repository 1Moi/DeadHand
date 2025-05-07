using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScratchTexture : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform rectTransform;
    public int width;
    public int height;
    public Texture2D maskTexture;
    public int scratchRadius;
    public bool hovering;

    private void Start()
    {
        initializeValues();
    }

    void initializeValues()
    {
        rectTransform = GetComponent<RectTransform>();
        width = (int)rectTransform.sizeDelta.x;
        height = (int)rectTransform.sizeDelta.y;
        maskTexture = new Texture2D(width, height);
        GetComponent<Image>().material.mainTexture = maskTexture;
        maskTexture.Apply(false);
    }

    public void ScratchAccordingToMousePosition(int xVal, int yVal)
    {
        int xPos, yPos, yRangeforScratch;
        Color32[] tempColorArray = maskTexture.GetPixels32();
        bool ischangedpixel = false;

        for (int xOffsetPos = -scratchRadius; xOffsetPos <= scratchRadius; xOffsetPos++)
        {
            yRangeforScratch = (int)Mathf.Pow(scratchRadius * scratchRadius - xOffsetPos * xOffsetPos, 0.5f);
            for (int yOffsetPos = -yRangeforScratch; yOffsetPos <= yRangeforScratch; yOffsetPos++)
            {
                xPos = xVal + xOffsetPos;
                yPos = yVal + yOffsetPos;

                if (CheckForScratch(xPos, yPos, tempColorArray))
                    ischangedpixel = true;

            }
        }
        if (ischangedpixel)
        {
            maskTexture.SetPixels32(tempColorArray);
            maskTexture.Apply();
        }
    }

    public bool CheckForScratch(int xPos, int yPos, Color32[] pixels)
    {
        if (xPos >= 0 && xPos < width && yPos >= 0 && yPos < height)
        {
            int index = yPos * width + xPos;
            if (pixels[index].a != 0)
            {
                pixels[index].a = 0;
                return true;
            }
        }
        return false;
    }

    private void Update()
    {
        if (hovering)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, Camera.main, out Vector2 localPos);
            ScratchAccordingToMousePosition((int)localPos.x, (int)localPos.y);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;
        Debug.Log("Hoverin : " + hovering);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
    }
}