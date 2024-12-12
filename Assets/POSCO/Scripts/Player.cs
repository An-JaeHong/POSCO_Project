using EasyTransition;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    public float moveSpeed;//�̵��ӵ�
    private float walkSpeed;
    private float runSpeed;//�⺻�ӵ�
    public float mouseSensivity; //���콺 ����
    public Transform cameraRig;

    //[Range(2f, 10f)]
    //public float zoomSpeed = 2f;
    //public float minZoom = 2f;
    //public float maxZoom = 10f;

    private Item item;
    public UIInventory uiInventory;

    private Rigidbody rb;

    private bool isMouseMove;

    //����ȯ�� �ʿ��� ���
    public TransitionSettings transition;
    public float loadDelay;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        item = FindAnyObjectByType<Item>(); // ���� GameObject�� Item ������Ʈ�� �ִ��� Ȯ��
        uiInventory = FindAnyObjectByType<UIInventory>(); 
        if (item == null)
        {
            Debug.LogError("Item ������Ʈ�� Player ��ü�� �����ϴ�.");
        }
    }

    private void Start()
    {
        currentState = new PlayerIdleState(this);
        currentState.Enter();
        //item = GetComponent<Item>(); // ���� GameObject�� Item ������Ʈ�� �ִ��� Ȯ��
        //if (item == null)
        //{
        //    Debug.LogError("Item ������Ʈ�� Player ��ü�� �����ϴ�.");
        //}
        walkSpeed = moveSpeed;
        runSpeed = moveSpeed*2;
        //�����ϸ� Ŀ�� ���
        //LockCursor();
    }

    public void UseItem(int itemIndex, Monster targetMonster)
    {
        item.Use(itemIndex, targetMonster);
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
            Time.timeScale *= 3f;
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            Time.timeScale /= 3f;
        }

        //�������� ��� ����
        currentState.Update();
        HandleSight();
        //HandleZoom();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnlockCursor();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            LockCursor();
        }

        if (UIInventoryManager.Instance.popupStack.Count > 0 || UIPopupManager.Instance.isPopupUIOpen)
        {
            isMouseMove = false;
            print(isMouseMove);
        }

        else
        {
            isMouseMove = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed = runSpeed;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = walkSpeed;
        }


        //if (UIPopupManager.Instance.isPopupUIOpen)
        //{
        //    isMouseMove = false;
        //}
        //else
        //{
           
        //    isMouseMove = true;
        //    print(isMouseMove);
        //}

        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    ToggleInventory();
        //}


        //if (Input.GetMouseButtonDown(0) && Cursor.lockState == CursorLockMode.None)
        //{
        //    LockCursor();
        //}
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
        //Debug.Log($"Move Direction: {actualMoveDir}, Velocity: {rb.velocity}");
    }

    //�þ� ���� �Լ�
    public void HandleSight()
    {
        if (!isMouseMove)
        {
            print("����");
            return;
        }
        print("�ȸ���");
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(0, mouseX * mouseSensivity * Time.deltaTime, 0);
        cameraRig.Rotate(-mouseY * mouseSensivity * Time.deltaTime, 0, 0);
    }

    //public void HandleZoom()
    //{
    //    float scroll = Input.GetAxis("Mouse ScrollWheel");
    //    Vector3 zoomDirection = cameraRig.forward * scroll * zoomSpeed;
    //    cameraRig.localPosition = Vector3.ClampMagnitude(cameraRig.localPosition + zoomDirection, maxZoom);
    //    cameraRig.localPosition = new Vector3(cameraRig.localPosition.x, Mathf.Clamp(cameraRig.localPosition.y, minZoom, maxZoom), cameraRig.localPosition.z);
    //}

    //Ŀ�� ���
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    //Ŀ�� �������
    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;
    }

    private void ToggleInventory()
    {
        //if (uiInventory != null)
        //{
        //    uiInventory.Toggle(); // �κ��丮 UI�� ����ϴ� �޼��尡 �ִٰ� ����
        //    if (uiInventory.IsOpen()) // �κ��丮�� ���� �ִ��� Ȯ���ϴ� �޼��尡 �ִٰ� ����
        //    {
        //        UnlockCursor();
        //    }
        //    else
        //    {
        //        LockCursor();
        //    }
        //}
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
