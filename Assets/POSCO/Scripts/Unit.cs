using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Transactions;
using System.Xml.Serialization;
using TMPro;
using UnityEditor.Animations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.TextCore.Text;

//보스와 일반 몬스터들을 묶을 클래스
[RequireComponent(typeof(CharacterController))]
//[RequireComponent(typeof(Rigidbody))]
public class Unit : MonoBehaviour
{
    private IUnitState currentState; //현재상태

    public List<GameObject> monsterPrefabs = new List<GameObject>(); //프리팹을 받을 리스트
    public List<Monster> ownedMonsterList = new List<Monster>(); //소지하고 있는 몬스터 리스트 -> 보스 : 종류가 다른 3개의 몬스터.  일반몬스터 : 종류가 같은 3개의 몬스터

    public bool isBoss; //State패턴을 위한 변수. true : 보스, false : 일반 몬스터
    public GameObject exclamationMarkPrefab; //느낌표 프리팹
    private GameObject exclamationMark; //실제 켰다 껐다할 느낌표
    public Transform spawnPosition; //스폰되는 장소
    private Player player;

    public string name;            //유닛 이름
    public int level;              //유닛 레벨
    public float moveSpeed;        //움직임 속도
    public float moveRange;        //움직임 범위 (일단은 정사각형이다)
    public float sightAngle;       //시야각
    public float detectRange;      //탐지 범위
    public bool isMove;            //움직일 수 있는지
    public bool hasRandomPosition; //랜덤한 장소가 생성됐는지

    public CharacterController characterController;

    public GameObject canvasPrefab;
    private TextMeshProUGUI unitInfoText;

    public Vector3 velocity;
    private Rigidbody rb;
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        player = FindAnyObjectByType<Player>();
    }

    private void Start()
    {
        //느낌표 마크를 생성해둔다.
        InstantiateExclamationMark();

        GameObject canvasObject = Instantiate(canvasPrefab, transform);
        unitInfoText = canvasObject.GetComponentInChildren<TextMeshProUGUI>();

        float monsterHeight = 3f;
        Renderer childRenderer = GetComponentInChildren<Renderer>();

        if (childRenderer != null)
        {
            print("자식 컴포넌트에 renderer 가 붙어있습니다.");
            monsterHeight = childRenderer.bounds.size.y;
        }

        canvasObject.transform.localPosition = new Vector3(0, monsterHeight + 0.5f, 0);

        if (isBoss)
        {
            level = 16;
            ChangeState(new BossIdleState());
        }
        else
        {
            level = Random.Range(1, 16);
            print($"{level}로 레벨 정해짐");
            ChangeState(new NormalIdleState());
        }

        //생성되는 몬스터의 레벨이 그대로 가져가게 해야한다

        foreach(GameObject monsterPrefab in monsterPrefabs)
        {
            //내가 들고 있는 몬스터를 소환한다
            GameObject monsterObject = Instantiate(monsterPrefab);
            //그 소환된 몬스터의 정보를 들고있는 Monster
            Monster originalMonster = monsterObject.GetComponent<Monster>();
            if (originalMonster != null)
            {
                Monster newMonster = monsterObject.GetComponent<Monster>();
                //소환된 몬스터 정보를 복사할 newMonster
                newMonster.InitializeFrom(originalMonster);
                //복사 한 후에 레벨에 따른 능력치 초기화
                newMonster.InitializeLevelInfo(level);
                //모든 후처리 기능 후에 더해주기
                ownedMonsterList.Add(newMonster);
            }
        }
        //for (int i = 0; i < monsterPrefab.Count; i++)
        //{
        //    //인스펙터에서 참조하는 몬스터프리팹
        //    GameObject monsterObject = Instantiate(monsterPrefab[i]);
        //    //그 몬스터 프리팹의 Monster컴포넌트
        //    Monster newMonster = monsterObject.GetComponent<Monster>();
        //    //ownedMonster.level = level;
        //    newMonster.InitializeLevelInfo(level);
        //    ownedMonsterList.Add(newMonster);
        //    //이거 또 값이 복사되는 일이 생김 수정하자
        //    //여기에다가 소환되는 몬스터들의 정보를 초기화 해야하나
        //}

        UpdateUnitInfoText();

        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    //Interface라서 Update를 직접 불러줘야한다.
    private void Update()
    {
        currentState?.Update(this);

    }

    private void UpdateUnitInfoText()
    {
        unitInfoText.text = $"{name}\n레벨 : {level}";
    }

    //상태를 바꾸는 함수
    public void ChangeState(IUnitState newState)
    {
        //현재 상태는 종료
        currentState?.Exit(this);
        print($"Before State : {currentState}");
        currentState = newState;
        //다시 시작
        currentState.Enter(this);
        print($"After State : {currentState}");
    }

    //UnitMove, UnitRotation 둘다 계속 실행이 되서 한번만 되게끔 바꾸자
    //목적지를 파라미터로 넣어주자
    public void UnitMove(Vector3 destination)
    {
        if (!isMove)
        {
            return;
        }
        //print("UnitMove함수가 실행됨");
        //움직이는 방향 노말벡터
        float distanceToDestination = Vector3.Distance(transform.position, destination);
        if (distanceToDestination > 0.1f)
        {
            Vector3 direction = (destination - transform.position).normalized;
            Vector3 moveVelocity = direction * moveSpeed;

            if (characterController.isGrounded && velocity.y < 0)
            {
                moveVelocity.y = 0;
            }
            moveVelocity.y += Physics.gravity.y * Time.deltaTime;
            //Vector3 finalVelocity = moveVelocity + velocity;
            characterController.Move(moveVelocity * Time.deltaTime);
        }
        //Vector3 direction = (destination - transform.position).normalized;

        //속도 = 방향 * 크기
        //Vector3 moveVelocity = direction * moveSpeed;

        //중력계산 -> 이거 좀 잘 해야할듯. 몬스터가 고꾸라지거나 위로 뜬다
        //moveVelocity.y = Physics.gravity.y * Time.deltaTime;

        //if (characterController.isGrounded && velocity.y < 0)
        //{
        //    moveVelocity.y = 0;
        //}
        //moveVelocity.y += Physics.gravity.y * Time.deltaTime;
        ////Vector3 finalVelocity = moveVelocity + velocity;
        //characterController.Move(moveVelocity * Time.deltaTime);
        //rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
    }

    public void UnitRotation(Vector3 target)
    {
        //바라볼 방향
        //print("UnitRotation함수가 실행됨");
        Vector3 targetDirection = new Vector3(target.x - transform.position.x, 0, target.z - transform.position.z).normalized;

        //print($"{targetDirection}");

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);

    }


    //느낌표를 소환해줄 함수
    public void InstantiateExclamationMark()
    {
        exclamationMark = Instantiate(exclamationMarkPrefab, transform);
    }

    //느낌표를 띄워줄 함수
    public void ShowExclamationMark()
    {
        //자식으로 소환
        exclamationMark.SetActive(true);
    }

    //느낌표를 숨겨줄 함수
    public void HideExclamationMark()
    {
        exclamationMark.SetActive(false);
    }

    //정해진 포지션에 도달하면 다른 포지션을 생성하게끔 하자
    public Vector3 SetRandomPosition()
    {
        //함수 실행될때 마다 랜덤한 x,z값 생성하고 새로운 벡터를 생성
        float rangeX = Random.Range(spawnPosition.position.x - moveRange, spawnPosition.position.x + moveRange);
        float rangeZ = Random.Range(spawnPosition.position.z - moveRange, spawnPosition.position.z + moveRange);

        float currentY = transform.position.y;

        Vector3 randomPos = new Vector3(rangeX, currentY, rangeZ);
        print($"랜덤으로 주어진 포지션{randomPos}");

        //새로 만든 포지션을 리턴
        return randomPos;
    }

    //플레이어를 만나면 움직임을 멈춰야한다
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out Player player))
        {
            isMove = false;
        }
    }

    //공격범위 그리는 함수
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 forward = transform.forward;
        //오른쪽 각도
        Vector3 rightBoundary = Quaternion.Euler(0, sightAngle / 2, 0) * forward;
        //왼쪽 각도
        Vector3 leftBoundary = Quaternion.Euler(0, -sightAngle / 2, 0) * forward;

        //Unit의 포지션
        Vector3 position = transform.position;
        //기즈모의 Y축 포지션을 0.1만큼 띄운다 0이면 바닥에 가려질 수도 있어서 0.1만큼 띄워서 생성할것이다
        position.y = 0.1f;

        //반지름 = 탐지범위
        float radius = detectRange;

        //좌우로 반지름 만큼의 선분을 그린다.
        Gizmos.DrawLine(position, position + rightBoundary * radius);
        Gizmos.DrawLine(position, position + leftBoundary * radius);

        //segments가 클수록 부드러운 호를 그린다.
        int segments = 20;
        //angleStep = 시야각 / n; 각도를 쪼개는 변수
        float angleStep = sightAngle / segments;
        //시작포인트는 왼쪽에서부터이다. 그리고 쭉 그려줄것이다.
        Vector3 previousPoint = position + leftBoundary * radius;

        for (int i = 1; i <= segments; i++)
        {
            //왼쪽 각 부터 계속 더하면서 angle을 업데이트
            float angle = -sightAngle / 2 + angleStep * i;
            //호에 그려지는 포인트
            Vector3 direction = Quaternion.Euler(0, angle, 0) * forward;
            //호에 그려질 다음 포인트
            Vector3 nextPoint = position + direction * radius;
            //그 포인트들을 계속 이어간다.
            Gizmos.DrawLine(previousPoint, nextPoint);
            //그리고 이전 포인트는 다음 포인트로 초기화 -> 계속 이어나가면 호처럼 된다.
            previousPoint = nextPoint;
        }
    }

}
