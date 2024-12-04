using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using System.Xml.Serialization;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

//������ �Ϲ� ���͵��� ���� Ŭ����
[RequireComponent(typeof(CharacterController))]
public class Unit : MonoBehaviour
{
    private IUnitState currentState; //�������
    public List<Monster> ownedMonsterList = new List<Monster>(); //�����ϰ� �ִ� ���� ����Ʈ -> ���� : ������ �ٸ� 3���� ����.  �Ϲݸ��� : ������ ���� 3���� ����
    public bool isBoss; //State������ ���� ����. true : ����, false : �Ϲ� ����
    public GameObject exclamationMark; //����ǥ ������Ʈ
    public float moveSpeed;

    private CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (isBoss)
        {
            SetState(new BossIdleState());
        }
        else
        {
            SetState(new NormalIdleState());
        }
    }

    //Interface�� Update�� ���� �ҷ�����Ѵ�.
    private void Update()
    {
        currentState?.Update(this);
    }

    //���¸� �ٲٴ� �Լ�
    public void SetState(IUnitState newState)
    {
        //���� ���´� ����
        currentState?.Exit(this);
        print($"Before State : {currentState}");
        currentState = newState;
        //�ٽ� ����
        currentState.Enter(this);
        print($"After State : {currentState}");
    }

    //�������� �Ķ���ͷ� �־�����
    public void UnitMove(Vector3 destination)
    {
        //�����̴� ���� �븻����
        Vector3 direction = (destination - transform.position).normalized;

        //�ӵ� = ���� * ũ��
        Vector3 velocity = direction * moveSpeed;

        characterController.Move(velocity * Time.deltaTime);
    }

    public void SwitchExclamationMark(bool isActive)
    {
        exclamationMark.SetActive(isActive);
    }
}
