// Assets/Scripts/Shop/MoneyCounterUI.cs
using UnityEngine;
using TMPro;
using System.Collections;

public class MoneyCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label; // assegna il TMP_Text del contatore
    // Wallet removed: MoneyCounterUI now reads coins from GameManager
    [SerializeField] private bool smooth = true;    // conteggio animato o istantaneo
    [SerializeField, Range(0.05f, 1.5f)] private float animDuration = 0.25f;    // durata animazione

    private int shownValue;
    private Coroutine animCo;

    // Inizializzazioni
    private void Awake()
    {
        // riferimenti
        if (label == null) label = GetComponent<TextMeshProUGUI>();
        // no wallet lookup required
    }

    // Eventi
    private void OnEnable()
    {
        // iscrizione eventi
        if (GameManager.instance != null)
        {
            GameManager.instance.OnCoinsChanged += HandleCoinsChanged;
            SetValueImmediate(GameManager.instance.SaveData.totalCoins);
        }
        else
            SetValueImmediate(0);
    }

    // Pulizia eventi
    private void OnDisable()
    {
        if (GameManager.instance != null)
            GameManager.instance.OnCoinsChanged -= HandleCoinsChanged;
    }

    // Callback quando cambia il numero di monete
    private void HandleCoinsChanged(int newValue)
    {
        // aggiorna il contatore
        if (!smooth) { SetValueImmediate(newValue); return; }
        if (animCo != null) StopCoroutine(animCo);
        animCo = StartCoroutine(AnimateTo(newValue));
    }

    // Aggiorna il contatore senza animazione
    private void SetValueImmediate(int v)
    {
        shownValue = v;
        if (label) label.text = shownValue.ToString("N0");
    }

    // Animazione del contatore
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
