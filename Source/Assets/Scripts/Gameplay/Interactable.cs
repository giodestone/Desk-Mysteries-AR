using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Base class for an interactable object.
/// </summary>
public class Interactable : MonoBehaviour
{
    /// <summary>
    /// Override this method to get Interaction calls.
    /// </summary>
    /// <param name="state">The state of the interaction.</param>
    /// <param name="hit">When the <paramref name="state"/> is <see cref="InteractionState.Stopping"/> the previous hit is passed back as no hit actually happens.</param>
    public virtual void InteractionHappening(InteractionState state, RaycastHit hit)
    {

    }
}