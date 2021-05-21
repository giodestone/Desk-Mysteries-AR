using UnityEngine;
using System;
using System.Collections;
using GoogleARCore;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manager for the PlaceMarker scene. Handles setting up the scene by creating prefabs for recognized planes,
/// and scene specific logic such as chaning to different states and updating the UI.
/// </summary>
public class AreaScanningManager : MonoBehaviour, IPausable
{
    // Constants
    const string gameplaySceneName = "GameplayScene";

    // References
    UIManager uiManager;
    DeskLocationPickerController deskLocationPickerController;

    // Prefabs
    GameObject newPlanePrefab;

    // Variables
    bool isPaused;

    AreaScanningManagerState state;
    AreaScanningManagerState previousState;
    List<DetectedPlane> newPlanes = new List<DetectedPlane>();
    List<GameObject> createdPlanes = new List<GameObject>();


    void Start()
    {
        RegisterPausable();
        SetupReferences();

        newPlanePrefab = Resources.Load<GameObject>("Prefabs/Plane");
        deskLocationPickerController.EnablePlacement = false;
        GameObjectFindingHelper.GetComponentInInactiveGameObjectsChildren<Button>(GameObjectFindingHelper.FindInactiveCanvasChildGameObject("ContinueToGameCanvasGroup")).onClick.AddListener(OnContinueButtonClicked);

        state = AreaScanningManagerState.FindingPlane;
        previousState = state;
        uiManager.ChangeGroup(UIManagerCanvasGroups.FindingPlane);
    }

    void OnDestroy()
    {
        UnregisterPausable();
    }

    void SetupReferences()
    {
        uiManager = GameObject.FindObjectOfType<UIManager>();
        deskLocationPickerController = gameObject.GetComponent<DeskLocationPickerController>();
    }

    void Update()
    {
        if (isPaused)
            return;

        switch (state)
        {
            case AreaScanningManagerState.FindingPlane:
                if (Session.Status != SessionStatus.Tracking)
                    return;

                // Add new planes that have been detected.
                Session.GetTrackables<DetectedPlane>(newPlanes, TrackableQueryFilter.New);

                foreach (var plane in newPlanes)
                {
                    var newPlaneObj = GameObject.Instantiate(newPlanePrefab);
                    createdPlanes.Add(newPlaneObj);

                    newPlaneObj.GetComponent<PlaneVisualizer>().Initialize(plane);
                }
                
                if (state == AreaScanningManagerState.FindingPlane)
                {
                    deskLocationPickerController.EnablePlacement = false;

                    if (newPlanes.Count > 0) // Found some planes so transition.
                    {
                        uiManager.ChangeGroup(UIManagerCanvasGroups.WaitingForUserToPlaceObject);
                        state = AreaScanningManagerState.WaitingForUserToPlaceObject;
                    }
                }
                else
                {
                    deskLocationPickerController.EnablePlacement = true;
                }
                break;

            case AreaScanningManagerState.WaitingForUserToPlaceObject:
                if (deskLocationPickerController.IsDeskLocationObjectPlaced)
                {
                    state = AreaScanningManagerState.PlaneTrackedAndObjectPlaced;
                    uiManager.ChangeGroup(UIManagerCanvasGroups.ContinueToGame);
                }
                goto case AreaScanningManagerState.FindingPlane; // Keep searching for planes.

            case AreaScanningManagerState.PlaneTrackedAndObjectPlaced:
                goto case AreaScanningManagerState.WaitingForUserToPlaceObject; // Keep searching for planes.
        }

        previousState = state;
    }

    /// <summary>
    /// Callback for continue button.
    /// </summary>
    void OnContinueButtonClicked()
    {
        StopScanning();
    }

    /// <summary>
    /// Tell the manager to stop scanning for new planes.
    /// </summary>
    public void StopScanning()
    {
        createdPlanes.RemoveAll(go => go == null); // Not all created planes will necessarily exist.

        foreach (var plane in createdPlanes)
        {
            plane.SetActive(false);
        }

        state = AreaScanningManagerState.DoNotSearch;

        SwitchToGameplay();
    }

    /// <summary>
    /// Tell the manager to start scanning for new planes.
    /// </summary>
    public void StartScanning()
    {
        foreach (var plane in createdPlanes)
        {
            plane.SetActive(true);
        }

        state = AreaScanningManagerState.FindingPlane;
    }

    /// <summary>
    /// Switches to the gameplay scene and disables markers and desk placement.
    /// </summary>
    void SwitchToGameplay()
    {
        deskLocationPickerController.EnablePlacement = false;
        deskLocationPickerController.EnableVisualMarker(false);

        uiManager.ChangeGroup(UIManagerCanvasGroups.PlayerInteraction);
        SceneManager.LoadScene(gameplaySceneName, LoadSceneMode.Additive);
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
        isPaused = true;
    }

    public void UnPause()
    {
        isPaused = false;
    }
}