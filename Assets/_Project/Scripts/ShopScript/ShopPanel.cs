using UnityEngine;

public class ShopPanel : MonoBehaviour
{
    // Riferimenti
    // Wallet is no longer required; ShopSlotUI reads coins from GameManager
    [SerializeField] private InventoryModel inventory;
    [SerializeField] private ItemDefinition[] items = new ItemDefinition[5];

    private void Awake()
    {
        // Trova tutti gli slot figli e assegna gli item
        var slots = GetComponentsInChildren<ShopSlotUI>(includeInactive: true);
        int n = Mathf.Min(slots.Length, items.Length);

        for (int i = 0; i < n; i++)
        {
            if (items[i] == null) { Debug.LogWarning($"Item {i} non assegnato."); continue; }
            // pass null for Wallet — ShopSlotUI will use GameManager for coins
            slots[i].Bind(items[i], null, inventory);
        }
    }
}
