using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSlotUI : MonoBehaviour
{
    [Header("UI Refs")]
    [SerializeField] private TextMeshProUGUI currentInventory;  // testo che mostra quante copie dell'item sono in inventario
    [SerializeField] private Image imgItem; // immagine dell'item
    [SerializeField] private TextMeshProUGUI description;   // nome o descrizione dell'item
    [SerializeField] private TextMeshProUGUI cost;   // testo che mostra il costo dell'item
    [SerializeField] private Image imgShop; // immagine di sfondo del bottone
    [SerializeField] private Button shopButton; // Bottone ImgShop

    [Header("Config (runtime)")]
    public ItemDefinition itemDef;
    private Wallet wallet;
    private InventoryModel inventory;

    public void Bind(ItemDefinition itm, Wallet w, InventoryModel inv)
    {
        itemDef = itm;
        wallet = w;
        inventory = inv;

        // UI statiche
        if (imgItem) imgItem.sprite = itm.icon;
        if (description) description.text = $"{itm.itemName}";
        if (cost) cost.text = $"{itm.price}";

        // listeners
        if (shopButton != null)
        {
            shopButton.onClick.RemoveAllListeners();
            shopButton.onClick.AddListener(Buy);
        }

        // sync iniziale
        RefreshCount(inventory.GetCount(itemDef.id));
        RefreshAffordability();

        // eventi
        if (wallet != null) wallet.OnCoinsChanged += _ => RefreshAffordability();
        if (inventory != null) inventory.OnItemCountChanged += OnAnyItemCountChanged;
    }

    // Pulizia eventi
    private void OnDestroy()
    {
        if (wallet != null) wallet.OnCoinsChanged -= _ => RefreshAffordability();
        if (inventory != null) inventory.OnItemCountChanged -= OnAnyItemCountChanged;
    }

    // Callback quando cambia il conteggio di un item in inventario
    private void OnAnyItemCountChanged(string id, int newCount)
    {
        if (itemDef != null && id == itemDef.id) RefreshCount(newCount);
    }

    // Aggiorna il testo del conteggio
    private void RefreshCount(int count)
    {
        if (currentInventory) currentInventory.text = count.ToString();
    }

    // Aggiorna lo stato del bottone in base alla disponibilit� economica
    private void RefreshAffordability()
    {
        // Controlla se puoi permetterti l'item
        if (wallet == null || itemDef == null || shopButton == null) return;
        bool can = wallet.Coins >= itemDef.price;
        shopButton.interactable = can;

        // cambia colore quando non puoi comprare
        /*
            if (imgShop != null)
                imgShop.color = can ? Color.white : new Color(1, 1, 1, 0.4f);
        */
    }

    // Logica di acquisto
    private void Buy()
    {
        if (wallet == null || inventory == null || itemDef == null) return;
        if (!wallet.TrySpend(itemDef.price)) return;

        inventory.Add(itemDef.id, 1);
        // feedback opzionale: piccola animazione/flash, suono, ecc.
        RefreshAffordability();
    }
}
