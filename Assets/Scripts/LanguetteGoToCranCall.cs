using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguetteGoToCranCall : MonoBehaviour
{
    public int index;
    public LanguetteCrantee Languette;

    public void CallLanguetteGotoCran()
    {
        Languette.GoToCran(0);
    }

}
