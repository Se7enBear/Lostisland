using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour
{
    public RectTransform hand;
    private bool canClick;
    private Vector3 mouseWorldPos => Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
    private ItemName currentItem;
    private bool holdItem;
    private void OnEnable()
    {
        EventHandler.ItemSelectedEvent += OnItemSelectedEvent;
        EventHandler.ItemUsedEvent += OnItemUsedEvent;
    }
    private void Update()
    {
        canClick = ObjectAtMousePostion();
        if (hand.gameObject.activeInHierarchy)
        {
            hand.position = Input.mousePosition;
        }
        if (InteractWithUI()) return;
        if (canClick && Input.GetMouseButtonDown(0))
        {
            ClickAction(ObjectAtMousePostion().gameObject);
        }
    }
    private void OnDisable()
    {
        EventHandler.ItemSelectedEvent -= OnItemSelectedEvent;
        EventHandler.ItemUsedEvent -= OnItemUsedEvent;
    }

    private void OnItemUsedEvent(ItemName name)
    {
        currentItem=ItemName.None;
        holdItem=false;
        hand.gameObject.SetActive(false);
    }

    private void OnItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
    {
        holdItem=isSelected;
        if (isSelected)
        {
            currentItem = itemDetails.itemName;
        }
        hand.gameObject.SetActive(holdItem);
    }

    private void ClickAction(GameObject clickObject)
    {
        switch (clickObject.tag) {
            case "Tp":
                var teleport=clickObject.GetComponent<Tp>();
                teleport?.TpToScene();
                    break;
            case "Item":
                var item=clickObject.GetComponent<Item>();
                item?.ItemClicked();
                break;
            case "Interactive":
                var interactive=clickObject.GetComponent<Interactive>();
                if(holdItem)
                interactive?.CheckItem(currentItem);
                else interactive?.EmptyClicked();
                break;
        }
    }

    private Collider2D ObjectAtMousePostion()
    {
        return Physics2D.OverlapPoint(mouseWorldPos);

    }
    private bool InteractWithUI()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return true;
        return false;
    }
}
