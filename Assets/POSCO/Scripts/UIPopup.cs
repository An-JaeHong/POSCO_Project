using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopup : MonoBehaviour
{
    private static UIPopup instance;
    public static UIPopup Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    //¸ðµç Äµ¹ö½º ´Ý±â
    public void AllCanvasClose()
    {

    }
}
