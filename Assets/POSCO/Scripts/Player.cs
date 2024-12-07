using System;
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

    public bool canMove = true;  //true : ������ �� ����, false : ������ �� ����
    public float moveSpeed;      //�̵��ӵ�
    public float mouseSensivity; //���콺 ����
    public Transform cameraRig;

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

        if (Input.GetKeyUp(KeyCode.Z))
        {
            Time.timeScale = 3f;
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            Time.timeScale = 1f;
        }

        //�������� ��� ����
        currentState.Update();
        HandleSight();
    }

    //�÷��̾��� ���¸� �ٲٴ� �Լ�
    public void ChangeState(PlayerStateBase newState)
    {
        Debug.Log($"before :  {currentState}");
        currentState.Exit();
        currentState = newState;
        Debug.Log($"afters :  {currentState}");
        currentState.Enter();
    }

    //�̵� ���� �Լ�
    public void HandleMovement()
    {
        if (!canMove) return;

        Vector3 inputValue = Vector3.zero;
        inputValue.x = Input.GetAxis("Horizontal");
        inputValue.z = Input.GetAxis("Vertical");

        Vector3 inputMoveDir = inputValue.normalized * moveSpeed;
        Vector3 actualMoveDir = transform.TransformDirection(inputMoveDir);
        rb.velocity = new Vector3(actualMoveDir.x, rb.velocity.y, actualMoveDir.z);
        Debug.Log($"Move Direction: {actualMoveDir}, Velocity: {rb.velocity}");
    }

    //�þ� ���� �Լ�
    public void HandleSight()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(0, mouseX * mouseSensivity * Time.deltaTime, 0);
        cameraRig.Rotate(-mouseY * mouseSensivity * Time.deltaTime, 0, 0);
    }

    public List<GameObject> GetMonsterPrefabList()
    {
        return playerMonsterPrefabList;
    }

    public List<Monster> GetSeletedMonsterList()
    {
        return selectedMonsterList;
    }

    public event Action onClickSelectButton;
    public void SetSelectedMonsters(List<Monster> selecedMonater)
    {
        //List<Monster> temp = new List<Monster>();
        //temp = selecedMonater;
        this.selectedMonsterList = selecedMonater;
        print(selectedMonsterList[1].name);

        //MonsterData�� �����Ǵ� �κ���
        onClickSelectButton?.Invoke();
    }

    //�����ϴ� ����� ������ ����
    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter");
        //���� ���·� ����
        currentState.HandleCollision(collision);

    }
}
