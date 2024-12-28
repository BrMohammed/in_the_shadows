using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    [SerializeField] private int numberofscens = 5;
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject Levelcam;

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


    static public int enter = 0;
    [Header("Menu")]
    public  GameObject menu;
    [SerializeField] private GameObject menuto;
    [SerializeField] private GameObject menenubegin;
    [SerializeField] private float animationmenuetime = 0.8f;
    [SerializeField] private GameObject Aboutpage;
    [SerializeField] private GameObject ReturnBtn;




    private void Awake()
    {
        if (init == null)
            init = this;
    }
    private void Start()
    {
        menu.transform.localPosition = menuto.transform.localPosition;
        if (enter == 1)
        {
            Levelcam.SetActive(true);
            menu.SetActive(false);
            ReturnBtn.SetActive(true);
            cam.SetActive(false);
            cam.transform.position = Levelcam.transform.position;
            cam.transform.rotation = Levelcam.transform.rotation;
        }
        else
            LeanTween.moveLocal(menu, new Vector3(menenubegin.transform.localPosition.x, menenubegin.transform.localPosition.y, menenubegin.transform.localPosition.z), animationmenuetime).setEase(LeanTweenType.easeOutBack).setDelay(1f);

        print("Animated :" + Getint("Animated") + "Level :" + Getint("Level"));
        _cam = Camera.main;
        int Animated = Getint("Animated");
        int Level = Getint("Level");
        if (Animated != 0)
        {
            for (int i = 0; i < Animated; i++)
            {
                int LayerIgnoreRaycast = LayerMask.NameToLayer("click");
                AllCards[i].layer = LayerIgnoreRaycast;
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
                else if(Getint("End") == 1)
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
                    {
                        AllCards[level - 1].GetComponent<Animator>().enabled = !AllCards[level - 1].GetComponent<Animator>().enabled;
                        int LayerIgnoreRaycast = LayerMask.NameToLayer("click");
                        AllCards[level - 1].layer = LayerIgnoreRaycast;
                    }
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
                int LayerIgnoreRaycast = LayerMask.NameToLayer("click");
                card.layer = LayerIgnoreRaycast;
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




    public void GoToleveles()
    {
        AudioManager.instance.PlaySound("click");
        LeanTween.moveLocal(menu, new Vector3(menuto.transform.localPosition.x, menuto.transform.localPosition.y, menuto.transform.localPosition.z), animationmenuetime)
        .setEase(LeanTweenType.easeInBack).setOnComplete(() =>
        {
            LeanTween.moveLocal(cam, new Vector3(0, 4.81f, -10.03f), 0.9f).setOnComplete(() =>
            {
                LeanTween.moveLocal(cam, new Vector3(0, 5.67000008f, -12.1899996f), 0.6f).setOnComplete(() =>
                {
                    LeanTween.moveLocal(cam, new Vector3(0, 5.25f, -11.2299995f), 0.5f);
                });
               
            });
            LeanTween.rotateLocal(cam, new Vector3(344.953003f, 0, 0), 0.9f).setOnComplete(() =>
            {
                LeanTween.rotateLocal(cam, new Vector3(47.6199989f, 0, 0), 0.6f).setOnComplete(() =>
                {
                    LeanTween.rotateLocal(cam, new Vector3(63.045002f, 0, 0), 0.5f).setOnComplete(() =>
                    {
                        ReturnBtn.SetActive(true);
                        Levelcam.SetActive(true);
                        cam.SetActive(false);
                    });
                });

            });

        });
        enter = 1;
    }
    public void returntomenu()
    {
        ReturnBtn.SetActive(false);
        Levelcam.SetActive(false);
        menu.SetActive(true);
        cam.SetActive(true);
        LeanTween.moveLocal(cam, new Vector3(0, 5.67000008f, -12.1899996f), 0.5f).setOnComplete(() =>
            {
                LeanTween.moveLocal(cam, new Vector3(0, 4.80999994f, -10.0389996f), 0.6f).setOnComplete(() =>
                {
                    LeanTween.moveLocal(cam, new Vector3(0, 4, -8.93999958f), 0.9f);
                });

            });
            LeanTween.rotateLocal(cam, new Vector3(47.6199989f, 0, 0), 0.5f).setOnComplete(() =>
            {
                LeanTween.rotateLocal(cam, new Vector3(344.953003f, 0, 0), 0.6f).setOnComplete(() =>
                {
                    LeanTween.rotateLocal(cam, new Vector3(315.580017f, 0, 0),0.9f).setOnComplete(() =>
                    {

                        LeanTween.moveLocal(menu, new Vector3(menenubegin.transform.localPosition.x, menenubegin.transform.localPosition.y, menenubegin.transform.localPosition.z), animationmenuetime)
                        .setEase(LeanTweenType.easeOutBack);
                    });
                });


        });
        enter = 0;
    }

    public void Resetlevels()
    {
        SetInt("Animated",0 );
        SetInt("Level",0 );
        SetInt("End", 0);
    }

    public void schowaboutpage()
    {
        AudioManager.instance.PlaySound("click");
        LeanTween.moveLocal(menu, new Vector3(menuto.transform.localPosition.x, menuto.transform.localPosition.y, menuto.transform.localPosition.z), animationmenuetime)
            .setEase(LeanTweenType.easeInBack).setOnComplete(() =>
            {
                LeanTween.scale(Aboutpage, new Vector3(1, 1, 1), 1f).setEase(LeanTweenType.easeOutElastic);
            });
       
    }
    public void Hideaboutpage()
    {
        AudioManager.instance.PlaySound("click");
        LeanTween.scale(Aboutpage, new Vector3(0, 0, 0), 0.5f).setEase(LeanTweenType.easeInBack).setOnComplete(() =>
        {
            LeanTween.moveLocal(menu, new Vector3(menenubegin.transform.localPosition.x, menenubegin.transform.localPosition.y, menenubegin.transform.localPosition.z), animationmenuetime).setEase(LeanTweenType.easeOutBack);

        });
    }
    public void CloseGame()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    IEnumerator EnableLevelCam()
    {
        yield return new WaitForSeconds(3);
        Levelcam.SetActive(true);
        cam.SetActive(false);
    }
}
