using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Levels
{
    horizontal,
    vertical,
    movement
}

public class GameManager : MonoBehaviour
{
   
    public float distence;
    public GameObject obj;
    Camera cam;
    public Levels levels;
    public Vector3 rotate;
    public float tolerance = 0.1f;
    Vector3 _rotation;
    public float speed = 1f;

    public LayerMask layerMask;
    public LayerMask layerMaskmovement;
    private Vector3 tempMousePos;
    private bool isDragging = false;

    public Vector2 offsetofobj ;

    private float rotationX = 0f;
    private float rotationY = 0f;

    bool win = false;

    [Header("UI")]
    public GameObject Guid;
    public GameObject exit;
    public GameObject winobj;

    void Start()
    {
        cam = Camera.main;
        tempMousePos = obj.transform.position;
        exit.SetActive(false);
        schowGuidtpage();
    }

    void Update()
    {
        if(!win)
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
            if (isMouse0Up)
            {
                isDragging = false;
            }

            //if (isDragging && !isCtrlDown && !isShiftDown)
            //{
            //    float deltaX = tempMousePos.x - mousePos.x;
            //    obj.transform.Rotate(Vector3.up * deltaX * speed * Time.deltaTime);
            //}
            //if (isDragging && isCtrlDown && !isShiftDown)
            //{
            //    float deltaX = tempMousePos.y - mousePos.y;
            //    obj.transform.Rotate(Vector3.right * deltaX * speed * Time.deltaTime);
            //}
            if (isDragging && !isCtrlDown && !isShiftDown)
            {
                float deltaX = tempMousePos.x - mousePos.x;
                obj.transform.Rotate(0, deltaX * speed * Time.deltaTime, 0, Space.World);
            }

            if (isDragging && isCtrlDown && !isShiftDown && (levels == Levels.vertical || levels == Levels.movement))
            {
                float deltaY = mousePos.y - tempMousePos.y;
                obj.transform.Rotate(deltaY * speed * Time.deltaTime, 0, 0, Space.World);
            }
            
            if (IsRotationCloseToTarget(obj.transform.eulerAngles, rotate, tolerance))
            {
                Debug.Log("Rotation is close to the target!");
                obj.transform.eulerAngles = rotate;
                win = true;
                winobj.SetActive(true);
                LeanTween.scale(winobj, new Vector3(1.3f, 1.3f, 1.3f), 1.2f).setEase(LeanTweenType.easeOutElastic);
                int sceneName = int.Parse(SceneManager.GetActiveScene().name) ;
                if(sceneName > MainManager.init.Getint("Level" ))
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

    public void ChengeScene()
    {
        AudioManager.instance.PlaySound("click");
        SceneManager.LoadScene(0);
        MainManager.enter = 1;
    }

    public void schowGuidtpage()
    {
            
            LeanTween.scale(Guid, new Vector3(1, 1, 1), 1.2f).setEase(LeanTweenType.easeOutElastic).setDelay(1.8f);

    }
    public void HideGuidpage()
    {
        AudioManager.instance.PlaySound("click");
        LeanTween.scale(Guid, new Vector3(0, 0, 0), 0.5f).setEase(LeanTweenType.easeInBack).setOnComplete(() =>
        {
            exit.SetActive(true);
        });
    }
}
