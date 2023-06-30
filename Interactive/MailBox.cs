using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailBox : Interactive
{
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    public Sprite openSprite;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void OnEnable()
    {
        EventHandler.AfterSceneUnloadEvent += OnAfterSceneUnloadEvent;
    }
    private void OnDisable()
    {
        EventHandler.AfterSceneUnloadEvent -= OnAfterSceneUnloadEvent;
    }

    private void OnAfterSceneUnloadEvent()
    {
        if (!isDone)
        {
           transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            spriteRenderer.sprite = openSprite;
            boxCollider.enabled = false;
        }
    }

    protected override void OnClickedAction()
    {
        spriteRenderer.sprite = openSprite;
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
