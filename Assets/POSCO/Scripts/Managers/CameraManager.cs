using EasyTransition;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Canvas fadeCanvas;
    private Image fadeImage;

    public TransitionSettings transition;
    public float startDelay;
    public GameObject settingMenu;
    TransitionManager manager;

    //public TransitionSettings transition;
    //public float loadDelay;

    

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

        //Canvas 자식으로 Image 컴포넌트를 들고있는 프리팹 넣기
        //fadeImage = fadeCanvas.GetComponentInChildren<Image>();

        //fadeCanvas.sortingOrder = 100;

        //fadeImage.raycastTarget = false;
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

    //카메라 바뀔때
    public void HandleCamera(CameraType cameraType)
    {
        //일단 모든걸 다 끄고
        battleMapCamera.enabled = false;
        fieldSceneCamera.enabled = false;
        bossMapCamera.enabled = false;
        OpenSettingMenu();

        //현재 카메라가 뭔지 정해준다
        switch (cameraType)
        {
            case CameraType.BattleMap:
                //currentCamera = battleMapCamera.GetComponent<Camera>();
                currentCamera = battleMapCamera;
                //Time.timeScale = 1;
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
        //OpenSettingMenu();
        //currentCamera.enabled = true;
        InActiveSettingMenuPanel();
        Invoke("Temp", 1.2f);
        //InActiveSettingMenuPanel();

        //AdjustCanvasDirection();
        //yield return StartCoroutine(Fade(0f));

        //if (player != null)
        //{
        //    player.CameraSetting();
        //}
        //yield return null;
    }

    private void Temp()
    {
        currentCamera.enabled = true;

    }

    //여기에서 카메라의 상태는 하나만 존재하게끔 한다.
    private IEnumerator SwitchCameraWithFade(CameraType cameraType)
    {
        //yield return StartCoroutine(Fade(1f));
        //계속 카메라는 꺼진 상태로 초기화
        battleMapCamera.enabled = false;
        fieldSceneCamera.enabled = false;
        bossMapCamera.enabled = false;
        OpenSettingMenu();

        switch (cameraType)
        {
            case CameraType.BattleMap:
                //currentCamera = battleMapCamera.GetComponent<Camera>();
                currentCamera = battleMapCamera;
                //Time.timeScale = 1;
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
        //OpenSettingMenu();
        currentCamera.enabled = true;
        InActiveSettingMenuPanel();

        //AdjustCanvasDirection();
        //yield return StartCoroutine(Fade(0f));

        //if (player != null)
        //{
        //    player.CameraSetting();
        //}
        yield return null;
    }

    //카메라 조정
    private void AdjustCanvasDirection()
    {
        if (fadeCanvas.renderMode == RenderMode.WorldSpace)
        {
            fadeCanvas.transform.LookAt(currentCamera.transform);
            fadeCanvas.transform.Rotate(0, 180f, 0);
        }
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float fadeSpeed = 1f;
        Color color = fadeImage.color;
        while (!Mathf.Approximately(color.a, targetAlpha))
        {
            color.a = Mathf.MoveTowards(color.a, targetAlpha, fadeSpeed * Time.deltaTime);
            fadeImage.color = color;
            yield return null;
        }
    }

    public void SetCanvasEventCamera(Canvas canvas)
    {
        if (canvas != null)
        {
            canvas.worldCamera = currentCamera;
        }
    }

    public void OpenSettingMenu()
    {
        manager = TransitionManager.Instance();
        manager.onTransitionCutPointReached += ActivateSettingMenuPanel;
        manager.Transition(transition, startDelay);
    }

    public void ActivateSettingMenuPanel()
    {
        settingMenu.SetActive(true);
        manager.onTransitionCutPointReached -= ActivateSettingMenuPanel;
    }

    public void InActiveSettingMenuPanel()
    {
        settingMenu.SetActive(false);
        manager.onTransitionCutPointReached -= ActivateSettingMenuPanel;
    }
}
