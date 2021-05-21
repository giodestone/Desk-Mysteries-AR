using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

/// <summary>
/// Provides helpers for finding <see cref="GameObject"/>s.
/// </summary>
public static class GameObjectFindingHelper
{
    private static string canvasName = "Canvas";

    /// <summary>
    /// Workaround for <see cref="GameObject.Find(string)"/> only finding active <see cref="GameObject"/>s.
    /// </summary>
    /// <param name="name"></param>
    /// <returns>The found GameObject in the Canvas GameObject, null if not found.</returns>
    public static GameObject FindInactiveCanvasChildGameObject(string name)
    {
        var canvas = GameObject.Find(canvasName);
        return canvas.transform.Find(name).gameObject;
    }

    /// <summary>
    /// Finds both active and inactive <see cref="GameObject"/>s in the scene root level by <paramref name="name"/>. 
    /// Workaround for <see cref="GameObject.Find(string)"/> only finding active <see cref="GameObject"/>s.
    /// </summary>
    /// <remarks>
    /// Performance demanding due to way the object is found.
    /// Only finds the items in the <paramref name="callingGameObject"/>s scene. Scenes must be merged if 
    /// <see cref="LoadSceneMode.Additive"/> is selected and the <see cref="GameObject"/> to be found is from the other scene..
    /// </remarks>
    /// <param name="name"></param>
    /// <returns>The found GameObject, or null if not found.</returns>
    public static GameObject FindRootLevelInactiveGameObjectInScene(GameObject callingGameObject, string name)
    {
        var gameObjects = new List<GameObject>(callingGameObject.scene.GetRootGameObjects());

        var rootLevelFind = gameObjects.Find(go => go.name == name);

        if (rootLevelFind != null)
            return rootLevelFind;

        foreach (var rootGameObj in gameObjects)
        {
            for (int i = 0; i < rootGameObj.transform.childCount; ++i)
            {
                var currentChild = rootGameObj.transform.GetChild(i).gameObject;
                if (currentChild.name == name)
                    return currentChild;
            }
        }

        // Not found :(
        return null;
    }

    /// <summary>
    /// Alias for <see cref="FindRootLevelInactiveGameObjectInScene(GameObject, string)"/>.
    /// </summary>
    /// <param name="callingGameObject"></param>
    /// <param name="name"></param>
    public static GameObject Find(GameObject callingGameObject, string name)
    {
        return FindRootLevelInactiveGameObjectInScene(callingGameObject, name);
    }

    /// <summary>
    /// Workaround for getting in/active components in the in/active GameObject's in/active children.
    /// </summary>
    /// <param name="gameObject">Game Object its located in.</param>
    /// <param name="searchChildrensChildren">Whether to search the children's children too for the component.</param>
    /// <typeparam name="ComponentType"></typeparam>
    /// <returns><typeparamref name="ComponentType"/> if found; null, or the default value of <typeparamref name="ComponentType"/> if not found.</returns>
    public static ComponentType GetComponentInInactiveGameObjectsChildren<ComponentType>(GameObject gameObject, bool searchChildrensChildren=false)
    {
        ComponentType targetComponent;
        // Look in children
        for (var i = 0; i < gameObject.transform.childCount; i++)
        {
            var childGameObject = gameObject.transform.GetChild(i);

            if (childGameObject.TryGetComponent<ComponentType>(out targetComponent))
                return targetComponent;

            // Look in children's children.
            if (!searchChildrensChildren)
                continue;
            
            for (var j = 0; j < childGameObject.transform.childCount; ++j)
            {
                var childOfChildGameObject = childGameObject.GetChild(j).gameObject;

                if (childOfChildGameObject.TryGetComponent<ComponentType>(out targetComponent))
                    return targetComponent;
            }
        }

        return default(ComponentType); // aka null.
    }

}