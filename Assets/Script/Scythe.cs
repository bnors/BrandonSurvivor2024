using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scythe : MonoBehaviour
{
    void Update()
    {
        transform.position += Vector3.right * 5f * Time.deltaTime;
    }
}
