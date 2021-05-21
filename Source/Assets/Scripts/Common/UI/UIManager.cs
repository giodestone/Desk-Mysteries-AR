using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Provides an interface to chance the currently displayed UI.
/// </summary>
public class UIManager : MonoBehaviour, IPausable
{
    // References
    GameObject menuCanvasGroup; // Item containing the burger menu.

    // Variables
    bool isInitialized = false;

    UIManagerCanvasGroups currentGroup = UIManagerCanvasGroups.None;
    UIManagerCanvasGroups canvasGroupBeforePause;
    Dictionary<UIManagerCanvasGroups, List<GameObject>> groupsToToggle;

    void Initialize()
    {
        RegisterPausable();

        menuCanvasGroup = GameObjectFindingHelper.FindInactiveCanvasChildGameObject("MenuCanvasGroup");

        SetupGroups();

        isInitialized = true;
    }

    /// <summary>
    /// Sets up the <see cref="groupsToToggle"/> by effectively creating a lookup from <see cref="UIManagerCanvasGroups"/> to a list of <see cref="GameObject"/>s.
    /// </summary>
    void SetupGroups()
    {
        groupsToToggle = new Dictionary<UIManagerCanvasGroups, List<GameObject>>();
        groupsToToggle.Add(UIManagerCanvasGroups.None, new List<GameObject>()); // Nothing in none after all its none.
        groupsToToggle.Add(UIManagerCanvasGroups.FindingPlane, new List<GameObject>() { GameObjectFindingHelper.FindInactiveCanvasChildGameObject("FindingPlaneCanvasGroup") });
        groupsToToggle.Add(UIManagerCanvasGroups.WaitingForUserToPlaceObject, new List<GameObject>() { GameObjectFindingHelper.FindInactiveCanvasChildGameObject("WaitingForUserToPlaceObjectCanvasGroup"), GameObjectFindingHelper.FindInactiveCanvasChildGameObject("PlayerInteractionCanvasGroup") });
        groupsToToggle.Add(UIManagerCanvasGroups.ContinueToGame, new List<GameObject>() { GameObjectFindingHelper.FindInactiveCanvasChildGameObject("ContinueToGameCanvasGroup"), GameObjectFindingHelper.FindInactiveCanvasChildGameObject("PlayerInteractionCanvasGroup") });
        groupsToToggle.Add(UIManagerCanvasGroups.PlayerInteraction, new List<GameObject>() { GameObjectFindingHelper.FindInactiveCanvasChildGameObject("PlayerInteractionCanvasGroup") });
        groupsToToggle.Add(UIManagerCanvasGroups.Objective, new List<GameObject>() { GameObjectFindingHelper.FindInactiveCanvasChildGameObject("PlayerInteractionCanvasGroup"), GameObjectFindingHelper.FindInactiveCanvasChildGameObject("ObjectiveCanvasGroup") });
        
        groupsToToggle.Add(UIManagerCanvasGroups.Win, new List<GameObject>() { GameObjectFindingHelper.FindInactiveCanvasChildGameObject("PlayerInteractionCanvasGroup"), GameObjectFindingHelper.FindInactiveCanvasChildGameObject("WinCanvasGroup") });
        groupsToToggle.Add(UIManagerCanvasGroups.Paused, new List<GameObject>() { GameObjectFindingHelper.FindInactiveCanvasChildGameObject("PausedCanvasGroup") });
    }

    void OnDestroy()
    {
        UnregisterPausable();
    }

    /// <summary>
    /// Change the currently displayed group. Groups are setup in <see cref="SetupGroups"/>
    /// </summary>
    /// <param name="newGroup"></param>
    public void ChangeGroup(UIManagerCanvasGroups newGroup)
    {
        if (newGroup == currentGroup)
            return;

        if (!isInitialized)
            Initialize();

        foreach (var item in groupsToToggle[currentGroup])
        {
            item.SetActive(false);
        }

        foreach (var item in groupsToToggle[newGroup])
        {
            item.SetActive(true);
        }

        currentGroup = newGroup;
    }

    // IPausable implementation.

    public void RegisterPausable()
    {
        this.RegisterWithPauseManager();
    }

    public void UnregisterPausable()
    {
        this.UnregisterWithPauseManager();
    }

    public void Pause()
    {
        // Keep a memory of what was on the screen before a pause.
        canvasGroupBeforePause = currentGroup;
        ChangeGroup(UIManagerCanvasGroups.Paused);
        menuCanvasGroup.SetActive(false);
    }

    public void UnPause()
    {
        // Restore what was before the game was paused.
        ChangeGroup(canvasGroupBeforePause);
        menuCanvasGroup?.SetActive(true);
    }
}