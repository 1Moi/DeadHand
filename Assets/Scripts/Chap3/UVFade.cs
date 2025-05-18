using UnityEngine;

public class UVFade : MonoBehaviour
{
    [Header("Materials")]
    public Material fadeMaterial;     // Fait : A × fadeFactor
    public Material addMaterial;      // Fait : A + B

    [Header("RenderTextures")]
    public RenderTexture uvSource;    // La texture du halo (UVRevealMask)
    public RenderTexture fadeBuffer;  // Stocke la mémoire qui s’efface lentement
    public RenderTexture finalMask;   // Résultat final utilisé dans ton shader RevealUV_Shader

    [Header("Settings")]
    [Range(0, 1)] public float fadeSpeed = 0.97f;

    private RenderTexture temp;

    void Update()
    {
        // 1. Appliquer le fade (diminue progressivement)
        fadeMaterial.SetFloat("_FadeFactor", fadeSpeed);
        temp = RenderTexture.GetTemporary(fadeBuffer.width, fadeBuffer.height, 0, fadeBuffer.format);
        Graphics.Blit(fadeBuffer, temp, fadeMaterial);

        // 2. Ajouter les nouvelles zones éclairées
        Graphics.Blit(uvSource, temp, addMaterial);

        // 3. Copier le résultat dans le mask final
        Graphics.Blit(temp, finalMask);

        // 4. Mettre à jour le buffer
        Graphics.Blit(finalMask, fadeBuffer);

        RenderTexture.ReleaseTemporary(temp);
    }
}
