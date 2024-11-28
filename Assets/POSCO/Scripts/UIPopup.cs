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

    //모든 캔버스 닫기
    public void AllCanvasClose()
    {

    }

    //적과 만난 캔버스UI
    public void EnemyContactCanvasOpen()
    {

    }

    //전투중의 UI
    public void BattleCanvasOpen()
    {

    }
}
