using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScratchTexture : MonoBehaviour
{
    public RectTransform rectTransform;
    public int width;
    public int height;
    public Texture2D maskTexture;
    public int scratchRadius;
    private int totalPixels;
    private int scratchedPixels;
    private bool autoScratchTriggered = false;
    private bool isScratchDisabled = false;
    private Coroutine autoScratchCoroutine;

    private void Start()
    {
        initializeValues();
    }

    void initializeValues()
    {
        rectTransform = GetComponent<RectTransform>();
        width = (int)rectTransform.sizeDelta.x;
        height = (int)rectTransform.sizeDelta.y;
        maskTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);

        Color32[] fillColor = new Color32[width * height];
        for (int i = 0; i < fillColor.Length; i++)
            fillColor[i] = new Color32(0, 0, 0, 255);  // opaque black

        maskTexture.SetPixels32(fillColor);
        maskTexture.Apply(false);

        GetComponent<Image>().material.mainTexture = maskTexture;

        totalPixels = width * height;
        scratchedPixels = 0;
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
                scratchedPixels++;
                return true;
            }
        }
        return false;
    }


    private void Update()
    {
        if (Input.GetMouseButton(0)) // Clic gauche maintenu
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, Camera.main, out Vector2 localPos))
            {
                ScratchAccordingToMousePosition((int)localPos.x, (int)localPos.y);
            }
        }

        // Vérifie le pourcentage gratté
        if (!autoScratchTriggered)
        {
            float scratchedPercentage = (float)scratchedPixels / totalPixels;

            if (scratchedPercentage >= 0.45f)
            {
                autoScratchTriggered = true;
                AutoScratchRemaining();
            }
        }
    }


    void AutoScratchRemaining()
    {
        autoScratchCoroutine = StartCoroutine(ScratchFromCenterOutward());
        StartCoroutine(StopAutoScratchAfterDelay(2f));
    }

    IEnumerator StopAutoScratchAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (autoScratchCoroutine != null)
        {
            StopCoroutine(autoScratchCoroutine);
            autoScratchCoroutine = null;
        }

        ScratchAllRemaining(); // Grattage immédiat du reste
    }


    IEnumerator ScratchFromCenterOutward()
    {
        isScratchDisabled = true;

        Color32[] pixels = maskTexture.GetPixels32();
        Vector2 center = new Vector2(width / 2, height / 2);
        int maxRadius = Mathf.CeilToInt(Mathf.Sqrt(width * width + height * height));

        for (int r = 0; r <= maxRadius; r += 10) 
        {
            // Dessine un disque de rayon `r`
            for (int dx = -r; dx <= r; dx++)
            {
                int dyLimit = Mathf.FloorToInt(Mathf.Sqrt(r * r - dx * dx));
                for (int dy = -dyLimit; dy <= dyLimit; dy++)
                {
                    int x = Mathf.RoundToInt(center.x + dx);
                    int y = Mathf.RoundToInt(center.y + dy);

                    if (x >= 0 && x < width && y >= 0 && y < height)
                    {
                        int index = y * width + x;
                        if (pixels[index].a != 0)
                        {
                            pixels[index].a = 0;
                        }
                    }
                }
            }

            maskTexture.SetPixels32(pixels);
            maskTexture.Apply();

            yield return null; // pause visuelle
        }

        Debug.Log("Auto-scratch complet !");
    }

    void ScratchAllRemaining()
    {
        Color32[] pixels = maskTexture.GetPixels32();

        for (int i = 0; i < pixels.Length; i++)
        {
            if (pixels[i].a != 0)
            {
                pixels[i].a = 0;
            }
        }

        maskTexture.SetPixels32(pixels);
        maskTexture.Apply();

        Debug.Log("Grattage complet effectué !");
    }

}