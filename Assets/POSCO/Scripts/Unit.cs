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

//������ �Ϲ� ���͵��� ���� Ŭ����
[RequireComponent(typeof(CharacterController))]
//[RequireComponent(typeof(Rigidbody))]
public class Unit : MonoBehaviour
{
    private IUnitState currentState; //�������

    public List<GameObject> monsterPrefabs = new List<GameObject>(); //�������� ���� ����Ʈ
    public List<Monster> ownedMonsterList = new List<Monster>(); //�����ϰ� �ִ� ���� ����Ʈ -> ���� : ������ �ٸ� 3���� ����.  �Ϲݸ��� : ������ ���� 3���� ����

    public bool isBoss; //State������ ���� ����. true : ����, false : �Ϲ� ����
    public GameObject exclamationMarkPrefab; //����ǥ ������
    private GameObject exclamationMark; //���� �״� ������ ����ǥ
    public Transform spawnPosition; //�����Ǵ� ���
    private Player player;

    public string name;            //���� �̸�
    public int level;              //���� ����
    public float moveSpeed;        //������ �ӵ�
    public float moveRange;        //������ ���� (�ϴ��� ���簢���̴�)
    public float sightAngle;       //�þ߰�
    public float detectRange;      //Ž�� ����
    public bool isMove;            //������ �� �ִ���
    public bool hasRandomPosition; //������ ��Ұ� �����ƴ���

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
        //����ǥ ��ũ�� �����صд�.
        InstantiateExclamationMark();

        GameObject canvasObject = Instantiate(canvasPrefab, transform);
        unitInfoText = canvasObject.GetComponentInChildren<TextMeshProUGUI>();

        float monsterHeight = 3f;
        Renderer childRenderer = GetComponentInChildren<Renderer>();

        if (childRenderer != null)
        {
            print("�ڽ� ������Ʈ�� renderer �� �پ��ֽ��ϴ�.");
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
            print($"{level}�� ���� ������");
            ChangeState(new NormalIdleState());
        }

        //�����Ǵ� ������ ������ �״�� �������� �ؾ��Ѵ�

        foreach(GameObject monsterPrefab in monsterPrefabs)
        {
            //���� ��� �ִ� ���͸� ��ȯ�Ѵ�
            GameObject monsterObject = Instantiate(monsterPrefab);
            //�� ��ȯ�� ������ ������ ����ִ� Monster
            Monster originalMonster = monsterObject.GetComponent<Monster>();
            if (originalMonster != null)
            {
                Monster newMonster = monsterObject.GetComponent<Monster>();
                //��ȯ�� ���� ������ ������ newMonster
                newMonster.InitializeFrom(originalMonster);
                //���� �� �Ŀ� ������ ���� �ɷ�ġ �ʱ�ȭ
                newMonster.InitializeLevelInfo(level);
                //��� ��ó�� ��� �Ŀ� �����ֱ�
                ownedMonsterList.Add(newMonster);
            }
        }
        //for (int i = 0; i < monsterPrefab.Count; i++)
        //{
        //    //�ν����Ϳ��� �����ϴ� ����������
        //    GameObject monsterObject = Instantiate(monsterPrefab[i]);
        //    //�� ���� �������� Monster������Ʈ
        //    Monster newMonster = monsterObject.GetComponent<Monster>();
        //    //ownedMonster.level = level;
        //    newMonster.InitializeLevelInfo(level);
        //    ownedMonsterList.Add(newMonster);
        //    //�̰� �� ���� ����Ǵ� ���� ���� ��������
        //    //���⿡�ٰ� ��ȯ�Ǵ� ���͵��� ������ �ʱ�ȭ �ؾ��ϳ�
        //}

        UpdateUnitInfoText();

        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    //Interface�� Update�� ���� �ҷ�����Ѵ�.
    private void Update()
    {
        currentState?.Update(this);

    }

    private void UpdateUnitInfoText()
    {
        unitInfoText.text = $"{name}\n���� : {level}";
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

    //UnitMove, UnitRotation �Ѵ� ��� ������ �Ǽ� �ѹ��� �ǰԲ� �ٲ���
    //�������� �Ķ���ͷ� �־�����
    public void UnitMove(Vector3 destination)
    {
        if (!isMove)
        {
            return;
        }
        //print("UnitMove�Լ��� �����");
        //�����̴� ���� �븻����
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

        //�ӵ� = ���� * ũ��
        //Vector3 moveVelocity = direction * moveSpeed;

        //�߷°�� -> �̰� �� �� �ؾ��ҵ�. ���Ͱ� ��ٶ����ų� ���� ���
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
        //�ٶ� ����
        //print("UnitRotation�Լ��� �����");
        Vector3 targetDirection = new Vector3(target.x - transform.position.x, 0, target.z - transform.position.z).normalized;

        //print($"{targetDirection}");

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);

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

        float currentY = transform.position.y;

        Vector3 randomPos = new Vector3(rangeX, currentY, rangeZ);
        print($"�������� �־��� ������{randomPos}");

        //���� ���� �������� ����
        return randomPos;
    }

    //�÷��̾ ������ �������� ������Ѵ�
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out Player player))
        {
            isMove = false;
        }
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
