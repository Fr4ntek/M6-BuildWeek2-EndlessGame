using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryModel : MonoBehaviour
{
    private readonly Dictionary<string, int> _counts = new();
    public event Action<string, int> OnItemCountChanged; // (itemId, newCount)

    public int GetCount(string id) => _counts.TryGetValue(id, out var c) ? c : 0;

    public void Add(string id, int qty = 1)
    {
        if (!_counts.ContainsKey(id)) _counts[id] = 0;
        _counts[id] = Mathf.Max(0, _counts[id] + qty);
        OnItemCountChanged?.Invoke(id, _counts[id]);
    }
}
