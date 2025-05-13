using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguetteGoToCranCall : MonoBehaviour
{
    public LanguetteCrantee Languette;

    public void CallLanguetteGotoCran(int index)
    {
        Languette.GoToCran(index);
    }

}
