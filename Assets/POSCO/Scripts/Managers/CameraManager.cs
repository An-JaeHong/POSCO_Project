using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ī�޶� ���¸� ��Ÿ���� Enum
[Serializable]
public enum CameraType
{
    None,
    BattleMap, //������
    FieldMap,  //�ʵ��
    BossMap,   //������
}

public class CameraManager : MonoBehaviour
{
    private static CameraManager instance;
    public static CameraManager Instance { get { return instance; } }

    public Camera battleMapCamera;
    public Camera fieldSceneCamera;
    public Camera bossMapCamera;

    //���� ī�޶� Ÿ��
    private CameraType currentCameraType = CameraType.None;

    public Camera currentCamera;

    private Player player;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        player = FindObjectOfType<Player>();
    }

    private void Start()
    {

        battleMapCamera.enabled = false;
        fieldSceneCamera.enabled = false;
        bossMapCamera.enabled = false;

        //���� ī�޶� ����
        //currentCamera = fieldSceneCamera.GetComponent<Camera>();
        currentCamera = fieldSceneCamera;
        currentCameraType = CameraType.FieldMap;
        HandleCamera(currentCameraType);
    }
    private void Update()
    {

    }

    //���⿡�� ī�޶��� ���´� �ϳ��� �����ϰԲ� �Ѵ�.
    public void HandleCamera(CameraType cameraType)
    {
        //��� ī�޶�� ���� ���·� �ʱ�ȭ
        battleMapCamera.enabled = false;
        fieldSceneCamera.enabled = false;
        bossMapCamera.enabled = false;

        switch (cameraType)
        {
            case CameraType.BattleMap:
                //currentCamera = battleMapCamera.GetComponent<Camera>();
                currentCamera = battleMapCamera;
                Time.timeScale = 1;
                break;
            case CameraType.FieldMap:
                //currentCamera = fieldSceneCamera.GetComponent<Camera>();
                currentCamera = fieldSceneCamera;
                break;
            case CameraType.BossMap:
                //currentCamera = bossMapCamera.GetComponent<Camera>();
                currentCamera = bossMapCamera;
                break;
        }

        currentCamera.enabled = true;

        //if (player != null)
        //{
        //    player.CameraSetting();
        //}
    }
    
    public void SetCanvasEventCamera(Canvas canvas)
    {
        if (canvas != null)
        {
            canvas.worldCamera = currentCamera;
        }
    }
}
