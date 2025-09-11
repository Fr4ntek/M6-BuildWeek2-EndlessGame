using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
public class DistanceCounter : MonoBehaviour
{
    public Transform player;            // riferimento al player
    public TextMeshProUGUI distanceText; // riferimento al testo UI
    public float scale = 1f;            // quanto vale 1 unità di Unity in "metri di gioco"

    private Vector3 startPos;
    private float distance;

    void Start()
    {
        if (player == null) player = transform;
        startPos = player.position;
    }

    void Update()
    {
        distance = Vector3.Distance(startPos, player.position);

        
        float metri = distance * scale;

       
        distanceText.text = "Metri: " + Mathf.FloorToInt(metri).ToString();
    }
}

