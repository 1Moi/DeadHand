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

    public IEnumerator AnimationFinal()
    {
        ClickBlocker.SetActive(true);
        yield return autoFlip.FlipToEnd();
        pageCamera.transform.position = new Vector3(600, 0, -18);
        NewBehaviourScript.IsDissolving = true;
        yield return new WaitForSeconds(2f);
        CanvaLivre.gameObject.SetActive(false);
        SceneManager.LoadScene("Outro", LoadSceneMode.Additive);
        yield return new WaitForSeconds(14f);
        ClickBlocker.SetActive(false);
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
