using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UIButtonSfx : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    IPointerClickHandler, ISelectHandler, ISubmitHandler
{
    [Header("Hover Scale")]
    [Range(0.9f, 2f)] public float scaleMultiplier = 1.1f; // +10%
    [Range(0f, 1f)] public float scaleSpeed = 0.2f; // velocità animazione

    Vector3 originalScale;
    bool isHovered = false;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (AudioManager.I) AudioManager.I.PlayHover();
        isHovered = true;
        StopAllCoroutines();
        StartCoroutine(ScaleTo(originalScale * scaleMultiplier));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        StopAllCoroutines();
        StartCoroutine(ScaleTo(originalScale));
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (AudioManager.I) AudioManager.I.PlayHover();
        // opzionale: puoi anche scalare quando selezionato da tastiera/pad
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (AudioManager.I) AudioManager.I.PlaySelect();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (AudioManager.I) AudioManager.I.PlaySelect();
    }

    System.Collections.IEnumerator ScaleTo(Vector3 target)
    {
        float t = 0f;
        Vector3 start = transform.localScale;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / scaleSpeed;
            transform.localScale = Vector3.Lerp(start, target, t);
            yield return null;
        }
        transform.localScale = target;
    }
}
