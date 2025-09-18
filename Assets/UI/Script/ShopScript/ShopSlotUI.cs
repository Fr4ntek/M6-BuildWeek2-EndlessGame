using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSlotUI : MonoBehaviour
{
    [Header("UI Refs")]
    [SerializeField] private TextMeshProUGUI currentInventory; // Q
    [SerializeField] private Image imgItem;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI cost;
    [SerializeField] private Image imgShop;      // deve avere anche Button
    [SerializeField] private Button shopButton;  // metti qui il Button di ImgShop

    [Header("Config (runtime)")]
    public ItemDefinition item;      // assegnato da ShopPanel.Bind
    private Wallet wallet;
    private InventoryModel inventory;

    public void Bind(ItemDefinition def, Wallet w, InventoryModel inv)
    {
        item = def;
        wallet = w;
        inventory = inv;

        // UI statiche
        if (imgItem) imgItem.sprite = def.icon;
        if (description) description.text = $"{def.itemName}";
        if (cost) cost.text = $"{def.price}";

        // listeners
        if (shopButton != null)
        {
            shopButton.onClick.RemoveAllListeners();
            shopButton.onClick.AddListener(Buy);
        }

        // sync iniziale
        RefreshCount(inventory.GetCount(item.id));
        RefreshAffordability();

        // eventi
        if (wallet != null) wallet.OnCoinsChanged += _ => RefreshAffordability();
        if (inventory != null) inventory.OnItemCountChanged += OnAnyItemCountChanged;
    }

    private void OnDestroy()
    {
        if (wallet != null) wallet.OnCoinsChanged -= _ => RefreshAffordability();
        if (inventory != null) inventory.OnItemCountChanged -= OnAnyItemCountChanged;
    }

    private void OnAnyItemCountChanged(string id, int newCount)
    {
        if (item != null && id == item.id) RefreshCount(newCount);
    }

    private void RefreshCount(int count)
    {
        if (currentInventory) currentInventory.text = count.ToString();
    }

    private void RefreshAffordability()
    {
        if (wallet == null || item == null || shopButton == null) return;
        bool can = wallet.Coins >= item.price;
        shopButton.interactable = can;

        // opzionale: cambia colore quando non puoi comprare
        if (imgShop != null)
            imgShop.color = can ? Color.white : new Color(1, 1, 1, 0.4f);
    }

    private void Buy()
    {
        if (wallet == null || inventory == null || item == null) return;
        if (!wallet.TrySpend(item.price)) return;

        inventory.Add(item.id, 1);
        // feedback opzionale: piccola animazione/flash, suono, ecc.
        RefreshAffordability();
    }
}
