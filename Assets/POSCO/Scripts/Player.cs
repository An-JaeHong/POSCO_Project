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

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 moveDir = new Vector3(x, 0, z);
        rb.MovePosition(new Vector3() * moveSpeed * Time.deltaTime);
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
        Debug.Log("OnCollisionEnter");
        //���� ���·� ����
        currentState.HandleCollision(collision);

    }
}
