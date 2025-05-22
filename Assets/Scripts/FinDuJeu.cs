using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class FinDuJeu : MonoBehaviour
{
    public AutoFlip autoFlip;
    public Camera pageCamera;
    public NewBehaviourScript NewBehaviourScript;
    public GameObject ClickBlocker;
    public Canvas CanvaLivre;

    public SceneFader sceneFader; // Référence au SceneFader dans l’inspecteur

    public IEnumerator AnimationFinal()
    {
        ClickBlocker.SetActive(true);
        yield return autoFlip.FlipToEnd();
        pageCamera.transform.position = new Vector3(600, 0, -18);
        NewBehaviourScript.IsDissolving = true;
        yield return new WaitForSeconds(10f);
        CanvaLivre.gameObject.SetActive(false);

        // Démarrer fondu au noir avant le chargement
        yield return sceneFader.FadeOut();

        // Charger la nouvelle scène
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Outro", LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
            yield return null;

        // Faire apparaître progressivement la nouvelle scène
        yield return sceneFader.FadeIn();

        ClickBlocker.SetActive(false);
    }

}
