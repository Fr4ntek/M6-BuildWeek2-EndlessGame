using UnityEngine;

public class ShopPanel : MonoBehaviour
{
    [SerializeField] private Wallet wallet;
    [SerializeField] private InventoryModel inventory;
    [SerializeField] private ItemDefinition[] items = new ItemDefinition[5];

    private void Awake()
    {
        var slots = GetComponentsInChildren<ShopSlotUI>(includeInactive: true);
        int n = Mathf.Min(slots.Length, items.Length);

        for (int i = 0; i < n; i++)
        {
            if (items[i] == null) { Debug.LogWarning($"Item {i} non assegnato."); continue; }
            slots[i].Bind(items[i], wallet, inventory);
        }
    }
}
