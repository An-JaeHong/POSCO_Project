using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopup : MonoBehaviour
{
    private static UIPopup instance;
    public static UIPopup Instance { get { return instance; } }

    public GameObject enemyContactCanvas;
    public GameObject battleCanvas;

    private Player player;

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

    private void Start()
    {
        player = FindObjectOfType<Player>();
        AllCanvasClose();
    }

    //모든 캔버스 닫기
    public void AllCanvasClose()
    {
        enemyContactCanvas.SetActive(false);
        battleCanvas.SetActive(false);
    }

    //적과 만난 캔버스UI
    public void EnemyContactCanvasOpen()
    {
        enemyContactCanvas.SetActive(true);
    }

    //전투중의 UI
    public void BattleCanvasOpen()
    {
        battleCanvas.SetActive(true);
    }

    public void OnClickDoBattleButton()
    {
        player.ChangeState(new PlayerBattleState(player));
    }

    public void OnClickDoRunButton()
    {
        player.ChangeState(new PlayerIdleState(player));
    }
}
