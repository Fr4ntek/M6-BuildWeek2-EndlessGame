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
    private InventoryModel inventory;
    [Header("Purchase Rules")]
    [SerializeField] private bool singlePurchaseOnly = false; // per gestire se limitare acquisto a 1
    private System.Action<int> coinsChangedHandler;
    public SaveData SaveData { get; private set; }

    public void Bind(ItemDefinition itm, Wallet w, InventoryModel inv)
    {
        itemDef = itm;
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

        // Initialize SaveData reference from GameManager so Buy won't hit null
        SaveData = GameManager.instance != null ? GameManager.instance.SaveData : null;

        // sync iniziale
        RefreshCount(inventory.GetCount(itemDef.id));
        RefreshAffordability();

        // eventi: subscribe to GameManager coin changes and inventory changes
        // unsubscribe previous handler if any (in case of re-bind)
        if (coinsChangedHandler != null && GameManager.instance != null)
            GameManager.instance.OnCoinsChanged -= coinsChangedHandler;

        coinsChangedHandler = _ => RefreshAffordability();
        if (GameManager.instance != null) GameManager.instance.OnCoinsChanged += coinsChangedHandler;
        if (inventory != null) inventory.OnItemCountChanged += OnAnyItemCountChanged;
    }

    // Pulizia eventi
    private void OnDestroy()
    {
        if (coinsChangedHandler != null && GameManager.instance != null)
            GameManager.instance.OnCoinsChanged -= coinsChangedHandler;
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

    // Aggiorna lo stato del bottone in base alla disponibilità economica
    private void RefreshAffordability()
    {
        // Controlla se puoi permetterti l'item
        if (itemDef == null || shopButton == null) return;

        int coins = GameManager.instance != null ? GameManager.instance.SaveData.totalCoins : 0;
        bool can = coins >= itemDef.price;

        // se singlePurchaseOnly, disabilita se già posseduto
        if (singlePurchaseOnly && inventory != null)
        {
            int current = inventory.GetCount(itemDef.id);
            if (current >= 1) can = false;
        }
        shopButton.interactable = can;

        // cambia colore quando non puoi comprare
        if (imgShop != null)
            imgShop.color = can ? Color.white : new Color(1, 1, 1, 0.4f);
    }

    // Logica di acquisto
    private void Buy()
    {
        if (GameManager.instance == null || inventory == null || itemDef == null) return;

        // Respect single-purchase rule
        if (singlePurchaseOnly && inventory.GetCount(itemDef.id) >= 1)
        {
            return;
        }

        // Prova a spendere i soldi, esci se non ci riesci
        if (!GameManager.instance.TrySpend(itemDef.price)) return;

        // Safely get SaveData (use local SaveData initialized in Bind or fallback to GameManager)
        var sd = SaveData ?? (GameManager.instance != null ? GameManager.instance.SaveData : null);
        if (sd != null)
        {
            // Aggiunge l'item in base all'id
            if (itemDef.id == "EL")
                sd.extraLife++;
            else if (itemDef.id == "GA")
                sd.perdiPeso = true;
            else if (itemDef.id == "TI")
                sd.temporaryInvincibility++;
        }

        // Aggiorna il modello di inventario così che gli ascoltatori ricevano l'evento
        if (inventory != null)
        {
            inventory.Add(itemDef.id, 1);
            Debug.Log($"Added item {itemDef.id} to inventory via ShopSlotUI");
        }

        // feedback opzionale: piccola animazione/flash, suono, ecc.
        RefreshAffordability();

    }
}
