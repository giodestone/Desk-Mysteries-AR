using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Provides a lookup for which <see cref="GameObject"/>s contain which components in a performant way. Adds all of the
/// <see cref="GameObject"/>'s children (and its children, recursively) too.
/// </summary>
/// <typeparam name="ComponentType">Type of component that the lookup should hold.</typeparam>
public class ComponentToGameObjectLookup<ComponentType> : IComponentLookup
    where ComponentType : UnityEngine.Component
{
    Dictionary<GameObject, HashSet<ComponentType>> gameObjectToComponent;

    /// <summary>
    /// Initialize the lookup from new.
    /// </summary>
    /// <param name="shouldIndexComponentsInScene">Whether to index items in scene. Will only add active ones.</param>
    public ComponentToGameObjectLookup(bool shouldIndexComponentsInScene=true)
    {
        gameObjectToComponent = new Dictionary<GameObject, HashSet<ComponentType>>();

        if (shouldIndexComponentsInScene)
        {
            IndexComponentsFromScene();
        }
    }

    /// <summary>
    /// Add active components from scene to the lookup.
    /// </summary>
    void IndexComponentsFromScene()
    {
        var components = GameObject.FindObjectsOfType<ComponentType>();
        foreach (var component in components)
        {
            AddLookup(component.gameObject, component);

            AddChildrenRecursive(component.gameObject, component);
        }
    }

    /// <summary>
    /// Add all the children of <paramref name="gameObject"/> to the lookup recursively getting all of the children to the lookup.
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="component"></param>
    void AddChildrenRecursive(GameObject gameObject, ComponentType component)
    {
        for (var i = 0; i < gameObject.transform.childCount; ++i)
        {
            var child = gameObject.transform.GetChild(i).gameObject;

            // Child has its own interactable, don't touch it.
            if (child.GetComponent<ComponentType>() != null)
                return;

            AddLookup(child, component);
            
            AddChildrenRecursive(child, component);
        }
    }

    /// <summary>
    /// Add a lookup pair.
    /// </summary>
    /// <remarks>
    /// Adds a <see cref="RemoveFromComponentToGameObjectLookupWhenDestroyed"/> component to the <paramref name="gameObject"/> if not present.
    /// </remarks>
    /// <param name="gameObject"></param>
    /// <param name="component"></param>
    public void AddLookup(GameObject gameObject, ComponentType component)
    {
        if (!gameObjectToComponent.ContainsKey(gameObject))
            gameObjectToComponent.Add(gameObject, new HashSet<ComponentType>());

        gameObjectToComponent[gameObject].Add(component);

        // Notify this on destroy to remove it.
        if (gameObject.GetComponents<RemoveFromComponentToGameObjectLookupWhenDestroyed>().Length == 0)
        {
            gameObject.AddComponent<RemoveFromComponentToGameObjectLookupWhenDestroyed>().Initialize(this);
        }
    }

    /// <summary>
    /// Unregister an object from the lookup.
    /// </summary>
    /// <remarks>
    /// Called by <see cref="RemoveFromComponentToGameObjectLookupWhenDestroyed"/>.
    /// </remarks>
    /// <param name="gameObject"></param>
    public void UnregisterObject(GameObject gameObject)
    {
        gameObjectToComponent.Remove(gameObject);
    }

    /// <summary>
    /// Get the components from the gameobject.
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    public ComponentType[] GetComponentsFromGameObject(GameObject gameObject)
    {
        // If debugger breaks or error happens here (probably something cryptic) then add lock(gameObjectToComponent) when using it and inside of UnregisterObject(): void.
        ComponentType[] array = new ComponentType[gameObjectToComponent[gameObject].Count];
        gameObjectToComponent[gameObject].CopyTo(array);
        return array;
    }

    /// <summary>
    /// Return the component, if it exists.
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="components"></param>
    /// <returns>True if it exists, false if it doesn't.</returns>
    public bool GetComponentsIfExists(GameObject gameObject, out ComponentType[] components)
    {
        // If debugger breaks or error happens here (probably something cryptic) then add lock(gameObjectToComponent) when using it and inside of UnregisterObject(): void.
        if (!gameObjectToComponent.ContainsKey(gameObject))
        {
            components = null;
            return false;
        }

        components = new ComponentType[gameObjectToComponent[gameObject].Count];
        gameObjectToComponent[gameObject].CopyTo(components);
        return true;
    }
}