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
    public Transform spawnPosition; //�����Ǵ� ���

    public float moveSpeed;   //������ �ӵ�
    public float moveRange;   //������ ���� (�ϴ��� ���簢���̴�)
    public float sightAngle;  //�þ߰�
    public float detectRange; //Ž�� ����

    private CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        if (isBoss)
        {
            ChangeState(new BossIdleState());
        }
        else
        {
            ChangeState(new NormalIdleState());
        }
    }

    //Interface�� Update�� ���� �ҷ�����Ѵ�.
    private void Update()
    {
        currentState?.Update(this);
    }

    //���¸� �ٲٴ� �Լ�
    public void ChangeState(IUnitState newState)
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

    //����ǥ�� ����� �Լ�
    public void ShowExclamationMark()
    {
        exclamationMark.SetActive(true);
    }

    //����ǥ�� ������ �Լ�
    public void HideExclamationMark()
    {
        exclamationMark.SetActive(false);
    }

    //������ �����ǿ� �����ϸ� �ٸ� �������� �����ϰԲ� ����
    public Vector3 SetRandomPosition()
    {
        //�Լ� ����ɶ� ���� ������ x,z�� �����ϰ� ���ο� ���͸� ����
        float rangeX = Random.Range(spawnPosition.position.x - moveRange, spawnPosition.position.x + moveRange);
        float rangeZ = Random.Range(spawnPosition.position.z - moveRange, spawnPosition.position.z + moveRange);

        Vector3 randomPos = new Vector3(rangeX, 0 , rangeZ);

        print($"�������� ������ ������ : {randomPos}");
        //���� ���� �������� ����
        return randomPos;
    }
}
