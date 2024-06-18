using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURL : MonoBehaviour
{
    public void OpenEfrogLink()
    {
        Application.OpenURL("https://element.market/collections/ethereum-frogs");
    }

    public void OpenCroakLink()
    {
        Application.OpenURL("https://app.lynex.fi/swap");
    }

    public void OpenTwitterLink()
    {
        Application.OpenURL("https://x.com/solbrandom");
    }
}
