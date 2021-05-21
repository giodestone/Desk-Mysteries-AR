using UnityEngine;

/// <summary>
/// Provides a type independent access for <see cref="ComponentToGameObjectLookup{ComponentType}"/>.
/// </summary>
public interface IComponentLookup
{
    public void UnregisterObject(GameObject gameObject);
}