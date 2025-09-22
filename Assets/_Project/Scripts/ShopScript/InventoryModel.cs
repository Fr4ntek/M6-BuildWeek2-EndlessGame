using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryModel : MonoBehaviour
{
    // Dizionario che mappa itemId a quantit�
    private readonly Dictionary<string, int> _counts = new();
    public event Action<string, int> OnItemCountChanged; // (itemId, newCount)

    // Se il dizionario non contiene l'id, ritorna 0
    public int GetCount(string id) => _counts.TryGetValue(id, out var c) ? c : 0;

    // Aggiunge qty all'item con id specificato, se non esiste lo crea
    public void Add(string id, int qty = 1)
    {
        if (!_counts.ContainsKey(id)) _counts[id] = 0;
        _counts[id] = Mathf.Max(0, _counts[id] + qty);
        OnItemCountChanged?.Invoke(id, _counts[id]);
    }
}
