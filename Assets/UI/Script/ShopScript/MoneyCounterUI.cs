// Assets/Scripts/Shop/MoneyCounterUI.cs
using UnityEngine;
using TMPro;
using System.Collections;

public class MoneyCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label; // assegna il TMP_Text del contatore
    [SerializeField] private Wallet wallet;         // se lasci vuoto, lo cerco io
    [SerializeField] private bool smooth = true;    // conteggio animato
    [SerializeField, Range(0.05f, 1.5f)] private float animDuration = 0.25f;

    private int shownValue;
    private Coroutine animCo;

    private void Awake()
    {
        if (label == null) label = GetComponent<TextMeshProUGUI>();
        if (wallet == null) wallet = FindObjectOfType<Wallet>();
    }

    private void OnEnable()
    {
        if (wallet != null)
            wallet.OnCoinsChanged += HandleCoinsChanged;

        // sync iniziale
        if (wallet != null)
            SetValueImmediate(wallet.Coins);
        else
            SetValueImmediate(0);
    }

    private void OnDisable()
    {
        if (wallet != null)
            wallet.OnCoinsChanged -= HandleCoinsChanged;
    }

    private void HandleCoinsChanged(int newValue)
    {
        if (!smooth) { SetValueImmediate(newValue); return; }
        if (animCo != null) StopCoroutine(animCo);
        animCo = StartCoroutine(AnimateTo(newValue));
    }

    private void SetValueImmediate(int v)
    {
        shownValue = v;
        if (label) label.text = shownValue.ToString("N0");
    }

    private IEnumerator AnimateTo(int target)
    {
        int start = shownValue;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / animDuration;
            shownValue = Mathf.RoundToInt(Mathf.Lerp(start, target, Mathf.SmoothStep(0,1,t)));
            if (label) label.text = shownValue.ToString("N0");
            yield return null;
        }
        SetValueImmediate(target);
        animCo = null;
    }
}
