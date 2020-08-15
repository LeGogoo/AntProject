using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform Ant;

    private void Update()
    {
        transform.position = new Vector3(Ant.position.x, transform.position.y, Ant.position.z);
    }
}
