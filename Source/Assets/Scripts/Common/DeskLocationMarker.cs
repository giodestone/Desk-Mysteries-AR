using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for providing access to the desk location marker.
/// </summary>
public class DeskLocationMarker : MonoBehaviour
{
    // Properties

    /// <summary>
    /// Whether the visual marker(s) are displayed.
    /// </summary>
    /// <value></value>
    public bool IsVisualMarkerActive { get; private set; }

    // Variables
    List<GameObject> visualMarkers;

    void Awake()
    {
        // By default the original child GameObject's are simply visual. Later other GameObjects may be attached
        // so this collection should stay constant. Using in Awake because it gets called regardless whether the 
        // object is on or off.
        if (visualMarkers == null)
        {
            visualMarkers = new List<GameObject>();

            for (var i = 0; i < transform.childCount; ++i)
            {
                // Shouldn't matter whether the children are active or not.
                var child = transform.GetChild(i).gameObject;
                visualMarkers.Add(child);
                child.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Set whether the visual markers are displayed or not. User <see cref="IsVisualMarkerActive"/> for whether the marker is already on.
    /// </summary>
    public void SetEnabledOfVisualMarker(bool state)
    {
        IsVisualMarkerActive = state;

        foreach (var marker in visualMarkers)
        {
            marker.SetActive(IsVisualMarkerActive);
        }
    }

}
