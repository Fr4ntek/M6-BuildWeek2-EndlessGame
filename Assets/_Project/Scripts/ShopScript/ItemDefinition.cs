using UnityEngine;

[CreateAssetMenu(fileName = "Item_", menuName = "Shop/ItemDefinition")]

// Definizione di un item acquistabile nello shop
public class ItemDefinition : ScriptableObject
{
    public string id;   // identificatore univoco
    public string itemName; // nome visualizzato
    [TextArea] public string description;   // descrizione visualizzata
    public Sprite icon; // icona visualizzata
    [Min(0)] public int price; // costo in monete
}
