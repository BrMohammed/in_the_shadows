using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
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
    public GameObject winobj;
    public float distence;
    public GameObject obj;
    Camera cam;
    public Levels levels;
    public Vector3 rotate;
    public float tolerance = 0.1f;
    Vector3 _rotation;
    public float speed = 1f;

    public LayerMask layerMask;
    private Vector3 tempMousePos;
    private bool isDragging = false;

    public Vector2 offsetofobj ;

    private float rotationX = 0f;
    private float rotationY = 0f;

    bool win = false;

    void Start()
    {
        cam = Camera.main;
        tempMousePos = obj.transform.position;
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
            if (isDragging && !isCtrlDown && isShiftDown && levels == Levels.movement)
            {
                Vector3 worldMousePos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, distence));
                obj.transform.position = new Vector3(worldMousePos.x, worldMousePos.y, 0);

            }
            if (IsRotationCloseToTarget(obj.transform.eulerAngles, rotate, tolerance))
            {
                Debug.Log("Rotation is close to the target!");
                obj.transform.eulerAngles = rotate;
                win = true;
                winobj.SetActive(true);
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
        SceneManager.LoadScene(0);
    }
}
