using UnityEngine;
using UnityEngine.UIElements;

public class H2AReset : Interactive
{
    private Transform gearSprite;
    private void Awake()
    {
        gearSprite = transform.GetChild(0);
    }
    public override void EmptyClicked()
    {
        GameController.Instance.Reset();
    }
}
