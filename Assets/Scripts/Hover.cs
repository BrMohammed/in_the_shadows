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
        enter = new Vector2( this.transform.localPosition.x, this.transform.localPosition.y);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!clicked)
            this.transform.localPosition = new Vector2(transform.localPosition.x - move, transform.localPosition.y);


    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.transform.localPosition = new Vector2(enter.x,enter.y);

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        clicked = true;
        this.transform.localPosition = new Vector2(enter.x, enter.y);
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(3);
        clicked = false;
    }
}
