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
    //플레이어가 시작시 가지고 있는 몬스터
    public List<GameObject> playerMonsterPrefabList = new List<GameObject>();
    //플레이어가 전투를 위해 선택한 몬스터
    public List<Monster> selectedMonsterList = new List<Monster>();

    //현재 상태
    public PlayerStateBase currentState;

    public bool canMove = true;  //true : 움직일 수 있음, false : 움직일 수 없음
    public float moveSpeed;//이동속도
    private float walkSpeed;
    private float runSpeed;//기본속도
    public float mouseSensivity; //마우스 감도
    public Transform cameraRig;

    //[Range(2f, 10f)]
    //public float zoomSpeed = 2f;
    //public float minZoom = 2f;
    //public float maxZoom = 10f;

    private Item item;
    public UIInventory uiInventory;

    private Rigidbody rb;

    private bool isMouseMove;

    //씬전환에 필요한 기능
    public TransitionSettings transition;
    public float loadDelay;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        item = FindAnyObjectByType<Item>(); // 같은 GameObject에 Item 컴포넌트가 있는지 확인
        uiInventory = FindAnyObjectByType<UIInventory>(); 
        if (item == null)
        {
            Debug.LogError("Item 컴포넌트가 Player 객체에 없습니다.");
        }
    }

    private void Start()
    {
        currentState = new PlayerIdleState(this);
        currentState.Enter();
        //item = GetComponent<Item>(); // 같은 GameObject에 Item 컴포넌트가 있는지 확인
        //if (item == null)
        //{
        //    Debug.LogError("Item 컴포넌트가 Player 객체에 없습니다.");
        //}
        walkSpeed = moveSpeed;
        runSpeed = moveSpeed*2;
        //시작하면 커서 잠금
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
                print("전투몬스터를 설정하세요");
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

        //상태패턴 계속 실행
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

    //플레이어의 상태를 바꾸는 함수
    public void ChangeState(PlayerStateBase newState)
    {
        Debug.Log($"before :  {currentState}");
        currentState.Exit();
        currentState = newState;
        Debug.Log($"afters :  {currentState}");
        currentState.Enter();
    }

    //이동 관련 함수
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

    //시야 관련 함수
    public void HandleSight()
    {
        if (!isMouseMove)
        {
            print("멈춤");
            return;
        }
        print("안멈춤");
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

    //커서 잠금
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    //커서 잠금해제
    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;
    }

    private void ToggleInventory()
    {
        //if (uiInventory != null)
        //{
        //    uiInventory.Toggle(); // 인벤토리 UI를 토글하는 메서드가 있다고 가정
        //    if (uiInventory.IsOpen()) // 인벤토리가 열려 있는지 확인하는 메서드가 있다고 가정
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

        //MonsterData에 연동되는 부분임
        onClickSelectButton?.Invoke();
    }

    //접촉하는 대상이 있으면 실행
    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter");
        //만난 상태로 돌입
        currentState.HandleCollision(collision);

    }
}
