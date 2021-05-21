using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes the desk rotate around like the buttons.
/// </summary>
public class DeskSwivel : MonoBehaviour
{
    // Constants
    const float passiveRotationStrengthMultiplier = 10f;

    // Variables
    Vector3 originalRotation;

    float randomTimeOffset; // To not match the bobbing with other buttons.

    void Start()
    {
        originalRotation = transform.rotation.eulerAngles;
        randomTimeOffset = Random.Range(0.5f, 0.75f);
    }

    
    void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(
                originalRotation.x + (Mathf.Cos(Time.time) * passiveRotationStrengthMultiplier),
            originalRotation.y + (Mathf.Sin(Time.time) * passiveRotationStrengthMultiplier),
            originalRotation.z);
    }
}
