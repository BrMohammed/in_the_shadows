using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hover : MonoBehaviour,IPointerEnterHandler , IPointerExitHandler,IPointerClickHandler
{
    public float move;
    private Vector2 enter;
    bool clicked = false;
    void Start()
    {
        LeanTween.scale(this.gameObject, new Vector3(1f, 1f, 1f), 0f).setEase(LeanTweenType.easeOutElastic);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!clicked)
            LeanTween.scale(this.gameObject, new Vector3(1.3f, 1.3f, 1.3f), 0f).setEase(LeanTweenType.easeOutElastic);


    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.scale(this.gameObject, new Vector3(1f, 1f, 1f), 0f).setEase(LeanTweenType.easeOutElastic);

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        clicked = true;
        LeanTween.scale(this.gameObject, new Vector3(1f, 1f, 1f), 0f).setEase(LeanTweenType.easeOutElastic);
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(3);
        clicked = false;
    }
}
