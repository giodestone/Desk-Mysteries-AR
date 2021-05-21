using UnityEngine;

/// <summary>
/// Remove the component from the lookup when this component is destroyed.
/// </summary>
public class RemoveFromComponentToGameObjectLookupWhenDestroyed : MonoBehaviour
{
    IComponentLookup owningComponentLookup;

    /// <summary>
    /// Initialize <see cref="RemoveFromComponentToGameObjectLookupWhenDestroyed"/>.
    /// </summary>
    /// <param name="componentLookup">Component lookup to tell if the <see cref="GameObject"/> this component is attached to is destroyed.</param>
    public void Initialize(IComponentLookup componentLookup)
    {
        this.owningComponentLookup = componentLookup;
    }

    void OnDestroy()
    {
        owningComponentLookup?.UnregisterObject(this.gameObject); // null check because when quitting game owning component may have been destroyed.
    }
}