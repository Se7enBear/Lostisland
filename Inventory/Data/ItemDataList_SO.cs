using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ItemDataList_SO",menuName ="Inventory/ItemDataList_SO")]
public class ItemDataList_SO:ScriptableObject
{
    public List<ItemDetails> ItemDetailsList;
    public ItemDetails GetItemDetails(ItemName itemName)
    {
        return ItemDetailsList.Find(i=>i.itemName == itemName);
    }
}
[System.Serializable]

public class ItemDetails
{
    public ItemName itemName;
    public Sprite itemSprite;
}