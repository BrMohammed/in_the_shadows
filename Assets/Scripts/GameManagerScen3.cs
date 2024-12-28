using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScen3 : MonoBehaviour
{
    public GameObject winobj;
    public float distence;
    public float zaccesforz;
    public GameObject obj;
    public GameObject obj2;
    Camera cam;
    public Levels levels;
    public Vector3 rotate;
    public Vector3 rotate2;
    public Vector3 transformobj2;
    public Vector3 transformobj;
    public float tolerance = 0.1f;
    public float tolerancetransform = 0.1f;
    Vector3 _rotation;
    public float speed = 1f;

    public LayerMask layerMask;
    public LayerMask layerMaskmovement;
    private Vector3 tempMousePos;
    private bool isDragging = false;

    public Vector2 offsetofobj;

    private float rotationX = 0f;
    private float rotationY = 0f;

    bool win = false;
    bool ismoving = false;


    [Header("UI")]
    public GameObject Guid;
    public GameObject exit;

    void Start()
    {
        cam = Camera.main;
        tempMousePos = obj.transform.position;
        exit.SetActive(false);
        schowGuidtpage();
    }

    void Update()
    {
        if (!win)
        {
            Vector3 mousePos = Input.mousePosition;
            bool isCtrlDown = Input.GetKey(KeyCode.LeftControl);
            bool isShiftDown = Input.GetKey(KeyCode.LeftShift);
            bool isMouse0Down = Input.GetKey(KeyCode.Mouse0);
            bool isMouse0Up = Input.GetMouseButtonUp(0);
            Ray ray = cam.ScreenPointToRay(mousePos);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Exit")))
            {
                print("here");
                SceneManager.LoadScene(0);
            }
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                if (isMouse0Down)
                {
                    isDragging = true;
                }


            }
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMaskmovement))
            {
                if (isMouse0Down)
                {
                    ismoving = true;
                }


            }


            if (isMouse0Up)
            {
                isDragging = false;
                ismoving = false;
            }
            if (!ismoving && isDragging && !isCtrlDown && !isShiftDown)
            {
                float deltaX = tempMousePos.x - mousePos.x;
                obj.transform.Rotate(0, deltaX * speed * Time.deltaTime, 0, Space.World);
            }

            if (!ismoving && isDragging && isCtrlDown && !isShiftDown && (levels == Levels.vertical || levels == Levels.movement))
            {
                float deltaY = -mousePos.y + tempMousePos.y;
                obj.transform.Rotate(0, 0, deltaY * speed * Time.deltaTime, Space.World);
            }
            if (!ismoving && isDragging && !isCtrlDown && isShiftDown && levels == Levels.movement)
            {
                Vector3 worldMousePos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, distence));
                obj.transform.position = new Vector3(zaccesforz, worldMousePos.y, worldMousePos.z);
            }

            if (!isDragging && ismoving && !isCtrlDown && !isShiftDown)
            {
                float deltaX = tempMousePos.x - mousePos.x;
                obj2.transform.Rotate(0, deltaX * speed * Time.deltaTime, 0, Space.World);
            }

            if (!isDragging && ismoving && isCtrlDown && !isShiftDown && (levels == Levels.vertical || levels == Levels.movement))
            {
                float deltaY = - mousePos.y + tempMousePos.y;
                obj2.transform.Rotate(0, 0, deltaY * speed * Time.deltaTime, Space.World);
            }
            if (!isDragging && ismoving && !isCtrlDown && isShiftDown && levels == Levels.movement)
            {
                Vector3 worldMousePos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, distence));
                obj2.transform.position = new Vector3(zaccesforz, worldMousePos.y, worldMousePos.z);
            }
            if (IsRotationCloseToTarget(obj.transform.eulerAngles, rotate, tolerance) 
                && IsRotationCloseToTarget(obj.transform.position, transformobj, tolerancetransform)
                && IsRotationCloseToTarget(obj2.transform.eulerAngles, rotate2, tolerance)
                && IsRotationCloseToTarget(obj2.transform.position, transformobj2, tolerancetransform))
            {
                Debug.Log("Rotation is close to the target!");
                obj.transform.eulerAngles = rotate;
                obj2.transform.eulerAngles = rotate2;
                obj2.transform.position = transformobj2;
                obj.transform.position = transformobj;
                win = true;
                winobj.SetActive(true);
                LeanTween.scale(winobj, new Vector3(1.3f, 1.3f, 1.3f), 1.2f).setEase(LeanTweenType.easeOutElastic);
                int sceneName = int.Parse(SceneManager.GetActiveScene().name);
                if (sceneName > MainManager.init.Getint("Level"))
                    MainManager.init.SetInt("Level", sceneName);
            }
            tempMousePos = mousePos;
        }

    }
    bool IsRotationCloseToTarget(Vector3 currentRotation, Vector3 targetRotation, float tolerance)
    {
        return Mathf.Abs(currentRotation.x - targetRotation.x) <= tolerance &&
               Mathf.Abs(currentRotation.y - targetRotation.y) <= tolerance &&
               Mathf.Abs(currentRotation.z - targetRotation.z) <= tolerance;
    }
    bool IsTransformCloseToTarget(Vector3 curentTransform, Vector3 targetTransform, float tolerance)
    {
        return Mathf.Abs(curentTransform.y - targetTransform.y) <= tolerance &&
               Mathf.Abs(curentTransform.z - targetTransform.z) <= tolerance;
    }

    public void ChengeScene()
    {
        SceneManager.LoadScene(0);
        MainManager.enter = 1;
    }

    public void schowGuidtpage()
    {

        LeanTween.scale(Guid, new Vector3(1, 1, 1), 1.2f).setEase(LeanTweenType.easeOutElastic).setDelay(1.8f);

    }
    public void HideGuidpage()
    {

        LeanTween.scale(Guid, new Vector3(0, 0, 0), 0.5f).setEase(LeanTweenType.easeInBack).setOnComplete(() =>
        {
            exit.SetActive(true);
        });
    }
}
