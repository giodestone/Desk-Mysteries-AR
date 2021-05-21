using UnityEngine;
using System.Collections;
using GoogleARCore;
using GoogleARCoreInternal;

/// <summary>
/// Mitigation for when the environmental light as managed by <see cref="EnvironmentalLight"/> is unavailable. Turns on the light its attached to.
/// </summary>
[RequireComponent(typeof(Light))]
public class NoLightingEstimationAvaliableMitigation : MonoBehaviour
{
    Light backupLight;
    Light environmentalLight;

    void Start()
    {
        environmentalLight = GameObjectFindingHelper.GetComponentInInactiveGameObjectsChildren<Light>(GameObject.FindObjectOfType<EnvironmentalLight>().gameObject, true);

        backupLight = GetComponent<Light>();
        backupLight.enabled = !environmentalLight.enabled;
    }

    void Update()
    {
        backupLight.enabled = !environmentalLight.enabled;
    }
}