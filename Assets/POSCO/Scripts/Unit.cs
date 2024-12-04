using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
    public GameObject exclamationMarkPrefab; //����ǥ ������
    private GameObject exclamationMark; //���� �״� ������ ����ǥ
    public Transform spawnPosition; //�����Ǵ� ���

    public string name; //���� �̸�
    public float moveSpeed;   //������ �ӵ�
    public float moveRange;   //������ ���� (�ϴ��� ���簢���̴�)
    public float sightAngle;  //�þ߰�
    public float detectRange; //Ž�� ����
    public bool iscontactedPlayer; //�÷��̾ ��������

    public CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        InstantiateExclamationMark();
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
        if (iscontactedPlayer)
        {
            return;
        }
        //�����̴� ���� �븻����
        Vector3 direction = (destination - transform.position).normalized;

        //�ӵ� = ���� * ũ��
        Vector3 velocity = direction * moveSpeed;

        characterController.Move(velocity * Time.deltaTime);
    }


    //����ǥ�� ��ȯ���� �Լ�
    public void InstantiateExclamationMark()
    {
        exclamationMark = Instantiate(exclamationMarkPrefab, transform);
    }

    //����ǥ�� ����� �Լ�
    public void ShowExclamationMark()
    {
        //�ڽ����� ��ȯ
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

    //���ݹ��� �׸��� �Լ�
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 forward = transform.forward;
        //������ ����
        Vector3 rightBoundary = Quaternion.Euler(0, sightAngle / 2, 0) * forward;
        //���� ����
        Vector3 leftBoundary = Quaternion.Euler(0, -sightAngle / 2, 0) * forward;

        //Unit�� ������
        Vector3 position = transform.position;
        //������� Y�� �������� 0.1��ŭ ���� 0�̸� �ٴڿ� ������ ���� �־ 0.1��ŭ ����� �����Ұ��̴�
        position.y = 0.1f;

        //������ = Ž������
        float radius = detectRange;

        //�¿�� ������ ��ŭ�� ������ �׸���.
        Gizmos.DrawLine(position, position + rightBoundary * radius);
        Gizmos.DrawLine(position, position + leftBoundary * radius);

        //segments�� Ŭ���� �ε巯�� ȣ�� �׸���.
        int segments = 20;
        //angleStep = �þ߰� / n; ������ �ɰ��� ����
        float angleStep = sightAngle / segments;
        //��������Ʈ�� ���ʿ��������̴�. �׸��� �� �׷��ٰ��̴�.
        Vector3 previousPoint = position + leftBoundary * radius;

        for (int i = 1; i <= segments; i++)
        {
            //���� �� ���� ��� ���ϸ鼭 angle�� ������Ʈ
            float angle = -sightAngle / 2 + angleStep * i;
            //ȣ�� �׷����� ����Ʈ
            Vector3 direction = Quaternion.Euler(0, angle, 0) * forward;
            //ȣ�� �׷��� ���� ����Ʈ
            Vector3 nextPoint = position + direction * radius;
            //�� ����Ʈ���� ��� �̾��.
            Gizmos.DrawLine(previousPoint, nextPoint);
            //�׸��� ���� ����Ʈ�� ���� ����Ʈ�� �ʱ�ȭ -> ��� �̾���� ȣó�� �ȴ�.
            previousPoint = nextPoint;
        }
    }
}
