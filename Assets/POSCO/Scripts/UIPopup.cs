using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPopup : MonoBehaviour
{
    private static UIPopup instance;
    public static UIPopup Instance { get { return instance; } }

    public GameObject enemyContactCanvas;
    public GameObject battleCanvas;
    public GameObject chooseBattleStateCanvas;

    //�ൿ�� ���� �ؽ�Ʈ
    private TextMeshProUGUI chooseStateText;

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

        chooseStateText = chooseBattleStateCanvas.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        AllCanvasClose();
    }

    //��� ĵ���� �ݱ�
    public void AllCanvasClose()
    {
        enemyContactCanvas.SetActive(false);
        battleCanvas.SetActive(false);
    }

    //���� ���� ĵ����UI
    public void EnemyContactCanvasOpen()
    {
        enemyContactCanvas.SetActive(true);
    }

    //�������� UI
    public void BattleCanvasOpen()
    {
        battleCanvas.SetActive(true);
    }

    public void ChooseBattleStateCanvasOpen(Monster playerMonster)
    {
        
        chooseBattleStateCanvas.SetActive(true);
        chooseStateText.text = $"{playerMonster.name}'Turn. Choose your state!";
    }

    //���ݹ�ư Ŭ��
    public void OnClickDoBattleButton()
    {
        player.ChangeState(new PlayerBattleState(player));
    }

    //���������ư Ŭ��
    public void OnClickDoRunButton()
    {
        player.ChangeState(new PlayerIdleState(player));
    }

    //Ÿ�� ���� ��ư Ŭ��
    public void OnClickTargetNum(int targetNum)
    {

    }

    public void OnClickDoAttackButton()
    {

    }

    public void onClickDoHealButton()
    {

    }
}
