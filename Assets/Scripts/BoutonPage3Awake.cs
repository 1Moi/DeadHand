using System.Collections;
using UnityEngine;

public class BoutonPage3Awake : MonoBehaviour
{
    private int direction = 1;

    void OnEnable()
    {
        StartCoroutine(AnimateButton());
    }

    IEnumerator AnimateButton()
    {
        transform.localScale += new Vector3(0.25f * direction, 0.25f, 0);
        transform.position += new Vector3(-0.65f * direction, 1, 0);

        yield return new WaitForSeconds(1.5f);

        transform.localScale += new Vector3(-0.25f * direction, -0.25f, 0);
        transform.position += new Vector3(0.65f * direction, -1, 0);
    }
}
