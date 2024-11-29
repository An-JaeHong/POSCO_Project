using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UIPopup : MonoBehaviour
{
    //UI의 Text
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI contentText;
    //버튼 띄울 위치 => Horizontal Layout을 들고 있어야함
    [SerializeField] private Transform buttonContainer;

    public Transform ButtonContainer => buttonContainer;

    private static UIPopup instance;
    public static UIPopup Instance { get { return instance; } }

    //적을 만났을때 뜨는 UI
    public GameObject enemyContactCanvas;
    //전투에 들어갔을때 공격할껀지 아니면 힐 할껀지 뜨는 UI
    public GameObject chooseBattleStateCanvas;
    //공격하기 누르면 누굴 선택할껀지
    public GameObject chooseTargetCanvas;

    //행동을 고르는 텍스트
    private TextMeshProUGUI chooseStateText;

    private Player player;
    public Monster currentTurnMonster = new Monster();

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

        //몬스터의 턴이 바뀔때 마다 현재턴의 몬스터가 계속 바뀐다.
        TurnManager.Instance.monsterTurnChange += SetCurrentTurnMonster;
    }

    public void SetTitle(string content)
    {
        contentText.text = content;
    }

    //public void SetContent(string content)
    //{
    //    contentText.text = content;
    //}

    //모든 캔버스 닫기
    public void AllCanvasClose()
    {
        enemyContactCanvas.SetActive(false);
        chooseBattleStateCanvas.SetActive(false);
        chooseTargetCanvas.SetActive(false);
    }

    //적과 만난 캔버스UI
    public void EnemyContactCanvasOpen()
    {
        enemyContactCanvas.SetActive(true);
    }

    //전투중의 UI
    public void ChooseBattleStateCanvasOpen()
    {
        chooseBattleStateCanvas.SetActive(true);
    }

    public void ChooseTargetCanvasOpen()
    {
        chooseTargetCanvas.SetActive(true);
        chooseStateText.text = $"{currentTurnMonster.name}'Turn. Choose your state!";
    }

    //공격하기 버튼을 클릭하면 플레이어 상태를 전투상태로 만들어야한다.
    public void OnClickDoBattleButton()
    {
        player.ChangeState(new PlayerBattleState(player));
    }

    //도망가기버튼 클릭
    public void OnClickDoRunButton()
    {
        player.ChangeState(new PlayerIdleState(player));
    }

    //타겟 고르는 버튼 클릭
    public void OnClickTargetNum(int targetNum)
    {
        Monster target = new Monster();
        switch (targetNum)
        {
            case 1:
                target = GameManager.Instance.enemyMonsterInBattleList[targetNum - 1];
                break;
            case 2:
                target = GameManager.Instance.enemyMonsterInBattleList[targetNum - 1];
                break;
            case 3:
                target = GameManager.Instance.enemyMonsterInBattleList[targetNum - 1];
                break;
        }

        GameManager.Instance.ExecutePlayerAttackAction(target);
    }

    public void OnClickDoAttackButton()
    {
        print("공격하기를 선택했다!");
        chooseBattleStateCanvas.SetActive(false);
        ChooseTargetCanvasOpen();
    }

    public void onClickDoHealButton()
    {

    }

    //현재 턴인 몬스터를 불러온다.
    public void SetCurrentTurnMonster(Monster currentTurnMonster)
    {
        this.currentTurnMonster = currentTurnMonster;
    }
}
