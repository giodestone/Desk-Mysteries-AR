using System.Collections;
using System.Collections.Generic;
using GoogleARCore;
using UnityEngine;

/// <summary>
/// Player controller for the place marker scenes. Handles placing the object and displaying marker at where the location of
/// the desk will be.
/// </summary>
public class DeskLocationPickerController : MonoBehaviour, IPausable
{
    // Properties

    /// <summary>
    /// Whether to allow the player to place things or not. Will not update UI.
    /// </summary>
    /// <value></value>
    public bool EnablePlacement { get; set; } = false;

    /// <summary>
    /// Determines whether the desk location object has been placed by checking whether <see cref="DeskLocationMarker.IsVisualMarkerActive"/> is true.
    /// </summary>
    /// <value></value>
    public bool IsDeskLocationObjectPlaced
    {
        get
        {
            if (deskLocationMarker == null)
                return false;
            else
                return deskLocationMarker.IsVisualMarkerActive;
        }
    }

    // References
    new Camera camera;
    DeskLocationMarker deskLocationMarker;
    ButtonClickedStateHandler interactButton;

    // Variables
    bool isPaused = false;

    Anchor deskObjectAnchor;

    void Awake()
    {
        Application.targetFrameRate = 60;
        RegisterPausable();
        SetupReferences();
    }

    void OnDestroy()
    {
        UnregisterPausable();
    }

    void SetupReferences()
    {
        camera = GameObject.FindObjectOfType<Camera>();

        deskLocationMarker = GameObject.FindObjectOfType<DeskLocationMarker>();

        var playerInteractonCanvasGroup = GameObjectFindingHelper.FindInactiveCanvasChildGameObject("PlayerInteractionCanvasGroup");
        interactButton = playerInteractonCanvasGroup.transform.Find("InteractButton").GetComponent<ButtonClickedStateHandler>();
    }

    void Update()
    {
        if (!EnablePlacement || !interactButton.IsButtonClicked || isPaused)
            return;

        var middleOfScreen = new Vector2(Screen.width / 2f, Screen.height / 2f);

        // Make sure on an existing plane so the table can be placed reliably.
        TrackableHitFlags filter = TrackableHitFlags.PlaneWithinPolygon;
        if (Frame.Raycast(middleOfScreen.x, middleOfScreen.y, filter, out var hit))
        {
            if (!(hit.Trackable is DetectedPlane))
                return;

            if (((DetectedPlane)hit.Trackable).PlaneType != DetectedPlaneType.HorizontalUpwardFacing)
                return;

            deskLocationMarker.SetEnabledOfVisualMarker(true);

            // Make desk location obj face the camera in the y axis.
            var deskLocMarkerToCamera = (deskLocationMarker.transform.position - camera.transform.position).normalized;
            var toCameraRotation = Quaternion.LookRotation(deskLocMarkerToCamera);
            deskLocationMarker.transform.rotation =
            Quaternion.Euler(deskLocationMarker.transform.rotation.x,
            toCameraRotation.eulerAngles.y,
            deskLocationMarker.transform.rotation.z);

            // Delete any previous anchors.
            if (deskObjectAnchor != null)
            {
                deskLocationMarker.transform.SetParent(null, false);
                Destroy(deskObjectAnchor.gameObject);
            }

            // Create new anchor and attach
            deskObjectAnchor = hit.Trackable.CreateAnchor(hit.Pose);
            deskLocationMarker.transform.SetParent(deskObjectAnchor.transform, false);
        }
    }

    /// <summary>
    /// Set whether the desk placement marker(s) are visible or not.
    /// See Also: <seealso cref="DeskLocationMarker.SetEnabledOfVisualMarker(bool)"/>
    /// </summary>
    /// <param name="state"></param>
    public void EnableVisualMarker(bool state)
    {
        deskLocationMarker.SetEnabledOfVisualMarker(state);
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
