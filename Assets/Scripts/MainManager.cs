using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject startbtn;

    public void GoToMenue()
    {
        cam.GetComponent<Animator>().enabled = !cam.GetComponent<Animator>().enabled;
        startbtn.SetActive(false);
    }
}
