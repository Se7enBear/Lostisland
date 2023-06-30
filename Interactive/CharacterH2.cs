using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueController))]
public class CharacterH2 : Interactive
{
    private DialogueController controller;
    private void Awake()
    {
        controller = GetComponent<DialogueController>();
    }
    public override void EmptyClicked()
    {
        if(isDone)
            controller.ShowDialogueFinish();
        else
        controller.ShowDialogueEmpty();
    }
    protected override void OnClickedAction()
    {
       controller.ShowDialogueFinish();
    }
}
