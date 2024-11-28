using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    //�÷��̾ ���۽� ������ �ִ� ����
    public List<GameObject> playerMonsterPrefabList = new List<GameObject>();
    //�÷��̾ ������ ���� ������ ����
    public List<Monster> selectedMonsterList = new List<Monster>();

    //���� ����
    public PlayerStateBase currentState;

    //true : ������ �� ����, false : ������ �� ����
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
                print("�������͸� �����ϼ���");
            }
            else
            {
                print("1");
            }
        }

        //�������� ��� ����
        currentState.Update();
    }

    //�÷��̾��� ���¸� �ٲٴ� �Լ�
    public void ChangeState(PlayerStateBase newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    //�̵� ���� �Լ�
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

    //�����ϴ� ����� ������ ����
    public void OnCollisionEnter(Collision collision)
    {
        //���� ���·� ����
        currentState.HandleCollision(collision);

    }
}
