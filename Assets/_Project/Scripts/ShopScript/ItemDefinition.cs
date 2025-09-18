using UnityEngine;

[CreateAssetMenu(fileName = "Item_", menuName = "Shop/ItemDefinition")]
public class ItemDefinition : ScriptableObject
{
    public string id;
    public string itemName;
    [TextArea] public string description;
    public Sprite icon;
    [Min(0)] public int price;
}
