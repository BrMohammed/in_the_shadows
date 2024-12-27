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
        enter = new Vector2( this.transform.position.x, this.transform.position.y);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!clicked)
            this.transform.position = new Vector2(transform.position.x - move, transform.position.y);


    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.transform.position = new Vector2(enter.x,enter.y);

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        clicked = true;
        this.transform.position = new Vector2(enter.x, enter.y);
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(3);
        clicked = false;
    }
}
