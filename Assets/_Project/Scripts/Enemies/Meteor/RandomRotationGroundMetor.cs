using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotationGroundMetor : MonoBehaviour
{
    private void Start()
    {
        float randomRotationY = Random.Range(-15, 15);
        float randomRotationZ = Random.Range(-15, 15);

        transform.rotation = Quaternion.Euler(Random.Range(0,360), transform.rotation.y + randomRotationY, transform.rotation.z + randomRotationZ);
    }
}
