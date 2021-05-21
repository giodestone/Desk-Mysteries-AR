using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves the desk to be a child of <see cref="DeskLocationMarker"/> on start.
/// </summary>
public class MoveDeskToMarker : MonoBehaviour
{
    // References
    GameObject deskRootObject; // Desk model etc.
    GameObject deskLocationMarker; // Where the desk should be.

    void Start()
    {
        deskRootObject = GameObject.Find("DeskRootObject");
        deskLocationMarker = GameObject.Find("DeskLocationMarker");

        MoveToRootObject();
    }

    /// <summary>
    /// Move the desk to the root object.
    /// </summary>
    public void MoveToRootObject()
    {
        deskRootObject.transform.SetParent(deskLocationMarker.transform, false);
    }
}
