using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    [SerializeField] private int numberofscens = 5;
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject Levelcam;
    [SerializeField] private GameObject startbtn;
    [SerializeField] private float timeoftransactioncofcam = 2;
    [SerializeField] private float TimeofMoveCardes = 2;
    [SerializeField] private float floatHeightForcard = 0.5f;
    public LayerMask layerMask;
    [SerializeField] private Vector3 OfssetOfCamMovement;
    Camera _cam;
    [SerializeField] private GameObject _fadeui;
    public GameObject[] AllCards;
    public GameObject[] CardsPosetion;

    

    static public MainManager init;
    bool animation = false;

    private void Awake()
    {
        if (init == null)
            init = this;
    }
    private void Start()
    {
        print("Animated :" + Getint("Animated") + "Level :" + Getint("Level"));
        _cam = Camera.main;
        int Animated = Getint("Animated");
        int Level = Getint("Level");
        if (Animated != 0)
        {
            for (int i = 0; i < Animated; i++)
            {
                AllCards[i].transform.position = CardsPosetion[i].transform.position;
            }
            for (int i = 0; i < Animated - 1; i++)
            {
                AllCards[i].transform.rotation = Quaternion.Euler(270, 180, 0);
            }
            if(Animated == numberofscens)
            {
                if(Getint("End") == 0)
                {
                    AllCards[numberofscens - 1].GetComponent<Animator>().enabled = !AllCards[numberofscens - 1].GetComponent<Animator>().enabled;
                    SetInt("End",1);
                }
                else
                    AllCards[Level - 1].transform.rotation = Quaternion.Euler(270, 180, 0);

            }
        }
    }



    private void Update()
    {
        if(Levelcam.activeSelf && !animation)
        {
            int level = Getint("Level");
            int Animated = Getint("Animated");
            if (AllCards.Length > 0 && CardsPosetion.Length > 0)
            {
               
                if (Animated == level && level < numberofscens)
                {
                    if(level != 0 && level != numberofscens)
                        AllCards[level - 1].GetComponent<Animator>().enabled = !AllCards[level - 1].GetComponent<Animator>().enabled;
                    StartCoroutine(Flipcard());
                }
               
                
            }
            animation = true;
        }
            
            Vector3 mousePos = Input.mousePosition;
            bool isMouse0Up = Input.GetMouseButtonUp(0);
            Ray ray = Levelcam.GetComponent<Camera>().ScreenPointToRay(mousePos);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {

                if (isMouse0Up)
                {
                    Vector3 targetPosition = hit.transform.position + OfssetOfCamMovement;
                    _fadeui.GetComponent<Animator>().enabled = !_fadeui.GetComponent<Animator>().enabled;
                    LeanTween.move(Levelcam, targetPosition, timeoftransactioncofcam).setOnComplete(() =>
                    {
                        SceneManager.LoadScene(hit.transform.gameObject.name);
                    });
                }
            }
     
        
    }

    IEnumerator Flipcard()
    {
        yield return new WaitForSeconds(1);
        int level = Getint("Level");
        MoveCardWithCurveEffect(AllCards[level], CardsPosetion[level].transform.position);
    }


    void MoveCardWithCurveEffect(GameObject card, Vector3 targetPosition)
    {


        Vector3 startPoint = card.transform.position;
        Vector3 midPoint = Vector3.Lerp(startPoint, targetPosition, 0.5f);
        midPoint.y += floatHeightForcard;
        Vector3 endPoint = targetPosition;
        Vector3[] path = new Vector3[] { startPoint, startPoint, midPoint, endPoint, endPoint };
        LeanTween.moveSpline(card, path, TimeofMoveCardes).setEase(LeanTweenType.easeOutQuad).setOnComplete(()=>
        {
            int level = Getint("Level");
            int animated = Getint("Animated");
            if(level == animated)
            {
                //card.GetComponent<Animator>().enabled = !card.GetComponent<Animator>().enabled;
                SetInt("Animated",level + 1);
            }
        });
    }
    public void SetInt(string KeyName, int Value)
    {
        PlayerPrefs.SetInt(KeyName, Value);
    }

    public int Getint(string KeyName)
    {
        return PlayerPrefs.GetInt(KeyName,0);
    }




    public void GoToMenue()
    {
        cam.GetComponent<Animator>().enabled = !cam.GetComponent<Animator>().enabled;
        startbtn.SetActive(false);
        StartCoroutine(EnableLevelCam());
    }


    public void Resetlevels()
    {
        SetInt("Animated", 0 );
        SetInt("Level", 0 );
    }
    IEnumerator EnableLevelCam()
    {
        yield return new WaitForSeconds(3);
        Levelcam.SetActive(true);
        cam.SetActive(false);
    }
}
