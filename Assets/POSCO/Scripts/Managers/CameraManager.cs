using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//카메라 상태를 나타내는 Enum
[Serializable]
public enum CameraType
{
    None,
    BattleMap, //전투씬
    FieldMap,  //필드씬
    BossMap,   //보스씬
}

public class CameraManager : MonoBehaviour
{
    private static CameraManager instance;
    public static CameraManager Instance { get { return instance; } }

    public Camera battleMapCamera;
    public Camera fieldSceneCamera;
    public Camera bossMapCamera;

    //현재 카메라 타입
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

        //현재 카메라 세팅
        //currentCamera = fieldSceneCamera.GetComponent<Camera>();
        currentCamera = fieldSceneCamera;
        currentCameraType = CameraType.FieldMap;
        HandleCamera(currentCameraType);
    }
    private void Update()
    {

    }

    //여기에서 카메라의 상태는 하나만 존재하게끔 한다.
    public void HandleCamera(CameraType cameraType)
    {
        //계속 카메라는 꺼진 상태로 초기화
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
