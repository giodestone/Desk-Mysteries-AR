using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Destroys the GameObject its attached to when <see cref="Start()"/> is called by Unity.
/// </summary>
public class DestroyOnStart : MonoBehaviour
{
    void Start()
    {
        Destroy(this);
    }
}
