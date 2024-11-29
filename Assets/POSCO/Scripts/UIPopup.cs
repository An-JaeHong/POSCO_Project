using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIPopup : MonoBehaviour
{
    //UI�� Text
    [SerializeField] private TextMeshProUGUI titleText;
    //[SerializeField] private TextMeshProUGUI contentText;
    //��ư ��� ��ġ => Horizontal Layout�� ��� �־����
    [SerializeField] private RectTransform buttonContainer;

    public RectTransform ButtonContainer => buttonContainer;

    //private static UIPopup instance;
    //public static UIPopup Instance { get { return instance; } }

    ////���� �������� �ߴ� UI
    //public GameObject enemyContactCanvas;
    ////������ ������ �����Ҳ��� �ƴϸ� �� �Ҳ��� �ߴ� UI
    //public GameObject chooseBattleStateCanvas;
    ////�����ϱ� ������ ���� �����Ҳ���
    //public GameObject chooseTargetCanvas;

    ////�ൿ�� ������ �ؽ�Ʈ
    //private TextMeshProUGUI chooseStateText;

    //private Player player;
    //public Monster currentTurnMonster = new Monster();

    private void Awake()
    {
        //if (instance == null)
        //{
        //    instance = this;
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}
        //DontDestroyOnLoad(gameObject);

        //chooseStateText = chooseBattleStateCanvas.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        //player = FindObjectOfType<Player>();
        //AllCanvasClose();

        ////������ ���� �ٲ� ���� �������� ���Ͱ� ��� �ٲ��.
        //TurnManager.Instance.monsterTurnChange += SetCurrentTurnMonster;
    }

    public void SetTitle(string content)
    {
        titleText.text = content;
    }

    //public void SetContent(string content)
    //{
    //    contentText.text = content;
    //}

    //��� ĵ���� �ݱ�
    //public void AllCanvasClose()
    //{
    //    enemyContactCanvas.SetActive(false);
    //    chooseBattleStateCanvas.SetActive(false);
    //    chooseTargetCanvas.SetActive(false);
    //}

    ////���� ���� ĵ����UI
    //public void EnemyContactCanvasOpen()
    //{
    //    enemyContactCanvas.SetActive(true);
    //}

    ////�������� UI
    //public void ChooseBattleStateCanvasOpen()
    //{
    //    chooseBattleStateCanvas.SetActive(true);
    //}

    //public void ChooseTargetCanvasOpen()
    //{
    //    chooseTargetCanvas.SetActive(true);
    //    chooseStateText.text = $"{currentTurnMonster.name}'Turn. Choose your state!";
    //}

    ////�����ϱ� ��ư�� Ŭ���ϸ� �÷��̾� ���¸� �������·� �������Ѵ�.
    //public void OnClickDoBattleButton()
    //{
    //    player.ChangeState(new PlayerBattleState(player));
    //}

    ////���������ư Ŭ��
    //public void OnClickDoRunButton()
    //{
    //    player.ChangeState(new PlayerIdleState(player));
    //}

    ////Ÿ�� ������ ��ư Ŭ��
    //public void OnClickTargetNum(int targetNum)
    //{
    //    Monster target = new Monster();
    //    switch (targetNum)
    //    {
    //        case 1:
    //            target = GameManager.Instance.enemyMonsterInBattleList[targetNum - 1];
    //            break;
    //        case 2:
    //            target = GameManager.Instance.enemyMonsterInBattleList[targetNum - 1];
    //            break;
    //        case 3:
    //            target = GameManager.Instance.enemyMonsterInBattleList[targetNum - 1];
    //            break;
    //    }

    //    GameManager.Instance.ExecutePlayerAttackAction(target);
    //}

    //public void OnClickDoAttackButton()
    //{
    //    print("�����ϱ⸦ �����ߴ�!");
    //    chooseBattleStateCanvas.SetActive(false);
    //    ChooseTargetCanvasOpen();
    //}

    //public void onClickDoHealButton()
    //{

    //}

    ////���� ���� ���͸� �ҷ��´�.
    //public void SetCurrentTurnMonster(Monster currentTurnMonster)
    //{
    //    this.currentTurnMonster = currentTurnMonster;
    //}
}
