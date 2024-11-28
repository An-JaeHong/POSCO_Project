using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    //플레이어가 시작시 가지고 있는 몬스터
    public List<GameObject> playerMonsterPrefabList = new List<GameObject>();
    //플레이어가 전투를 위해 선택한 몬스터
    public List<Monster> selectedMonsterList = new List<Monster>();

    //현재 상태
    public PlayerStateBase currentState;

    //true : 움직일 수 있음, false : 움직일 수 없음
    public bool canMove = true;
    public float moveSpeed;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        currentState = new PlayerIdleState(this);
        currentState.Enter();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.U))
        {
            //nt($"{selectedMonsters[0].name}");
            if (selectedMonsterList.Count == 0)
            {
                print("전투몬스터를 설정하세요");
            }
            else
            {
                print("1");
            }
        }

        //상태패턴 계속 실행
        currentState.Update();
    }

    //플레이어의 상태를 바꾸는 함수
    public void ChangeState(PlayerStateBase newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    //이동 관련 함수
    public void HandleMovement()
    {
        if (!canMove) return;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 moveDir = new Vector3(x, 0, z);
        rb.MovePosition(transform.position + moveDir * moveSpeed * Time.deltaTime);
    }

    public List<GameObject> GetMonsterPrefabList()
    {
        return playerMonsterPrefabList;
    }

    public List<Monster> GetSeletedMonsterList()
    {
        return selectedMonsterList;
    }

    public void SetSelectedMonsters(List<Monster> selecedMonater)
    {
        //List<Monster> temp = new List<Monster>();
        //temp = selecedMonater;
        this.selectedMonsterList = selecedMonater;
        print(selectedMonsterList[1].name);
    }

    //접촉하는 대상이 있으면 실행
    public void OnCollisionEnter(Collision collision)
    {
        //만난 상태로 돌입
        currentState.HandleCollision(collision);

    }
}
