using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxManager : MonoBehaviour
{
   
    public Transform player;          
    public Material[] skyboxes;        

   
    public float changeEveryMeters = 10f;  
    public float fadeDuration = 3f;         

    private int currentIndex = 0;      
    private bool isFading = false;      

    void Start()
    {
        if (skyboxes != null && skyboxes.Length > 0)
        {
            RenderSettings.skybox = skyboxes[0]; // Imposta skybox iniziale
        }
    }

    void Update()
    {
        // Se non ci sono skybox o player non assegnato, esci
        if (skyboxes == null || skyboxes.Length == 0 || player == null)
            return;

        // Calcola in quale "zona" ci troviamo
        int zoneIndex = Mathf.FloorToInt(player.position.z / changeEveryMeters);


        // Loop ciclico
        zoneIndex = Mathf.Abs(zoneIndex) % skyboxes.Length;

        // Cambia skybox solo se serve e non è già in fade
        if (zoneIndex != currentIndex && !isFading)
        {
            int fromIndex = currentIndex;
           
            int toIndex = zoneIndex;
            
            currentIndex = zoneIndex;
            RenderSettings.skybox = skyboxes[toIndex];
            DynamicGI.UpdateEnvironment();
           
        }
    }

    ////private IEnumerator FadeSkybox(Material fromSky, Material toSky)
    //{
    //    isFading = true;
    //    float t = 0f;

    //    while (t < 1f)
    //    {
    //        t += Time.deltaTime / fadeDuration;

    //        // Interpolazione tra skybox
    //        RenderSettings.skybox.
    //        yield return null;
    //    }

    //    // Alla fine assicura skybox finale corretto
    //    RenderSettings.skybox = toSky;
    //    isFading = false;
    //}
}

