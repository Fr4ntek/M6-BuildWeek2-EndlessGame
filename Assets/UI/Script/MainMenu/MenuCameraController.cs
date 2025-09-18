using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuCameraController : MonoBehaviour
{
    // Riferimento alla telecamera di gioco
    [SerializeField] private Camera gameCamera;
    [SerializeField] private string sceneToLoad;

    // Configurazioni di rotazione
    [SerializeField] private float rotationDuration = 0.5f; // tempo in secondi
    [SerializeField] private float angleStep = 90f;

    private bool isRotating = false;

    private void Start()
    {
        if (gameCamera == null) gameCamera = Camera.main;
    }

    // Metodo pubblico per avviare la rotazione della telecamera
    public void RotateCamera()
    {
        if (gameCamera != null && !isRotating)
        {
            StartCoroutine(RotateOverTime(Quaternion.Euler(0f, angleStep, 0f)));
        }
    }

    // Coroutine per ruotare la telecamera nel tempo
    private IEnumerator RotateOverTime(Quaternion stepRotation)
    {
        isRotating = true;

        Quaternion startRot = gameCamera.transform.rotation;
        Quaternion endRot = startRot * stepRotation;

        float elapsed = 0f;

        while (elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / rotationDuration);

            gameCamera.transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        gameCamera.transform.rotation = endRot;
        isRotating = false;
    }

    // Metodo per caricare la scena di gioco
    public void LoadGameScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
            SceneManager.LoadScene(sceneToLoad);
    }
}
