using EasyTransition;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        //Canvas �ڽ����� Image ������Ʈ�� ����ִ� ������ �ֱ�
        //fadeImage = fadeCanvas.GetComponentInChildren<Image>();

        //fadeCanvas.sortingOrder = 100;

        //fadeImage.raycastTarget = false;
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

    public void HandleCamera(CameraType cameraType)
    {
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
        //yield return null;
    }

    //���⿡�� ī�޶��� ���´� �ϳ��� �����ϰԲ� �Ѵ�.
    private IEnumerator SwitchCameraWithFade(CameraType cameraType)
    {
        //yield return StartCoroutine(Fade(1f));
        //��� ī�޶�� ���� ���·� �ʱ�ȭ
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

    //ī�޶� ����
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
