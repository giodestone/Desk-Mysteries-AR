using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages buttons in the Main Menu scene. Initializes them and raycasts to see if the player has interacted with them.
/// </summary>
public class MainMenuButtonManager : MonoBehaviour
{
    // Constants
    const float maxRaycastDistance = 10f;
    const string startButtonName = "Start Button";
    const string exitButtonName = "Exit Button";

    // References
    new Camera camera;

    // Variables
    Dictionary<GameObject, Button3DScript> buttons;
    bool rejectTouches = false;

    void Start()
    {
        camera = GameObject.FindObjectOfType<Camera>();

        // Setup and index all buttons by gameobject to make invoking the script reusable.
        buttons = new Dictionary<GameObject, Button3DScript>();
        foreach (var buttonScript in GameObject.FindObjectsOfType<Button3DScript>())
        {
            switch(buttonScript.gameObject.name)
            {
                case startButtonName:
                    buttonScript.Initialize("PlaceMarker");
                    break;
                case exitButtonName:
                    buttonScript.Initialize("Exit");
                    break;
                default:
                    throw new System.Exception($"Button {buttonScript.gameObject.name} is not setup!");
            }

            buttons.Add(buttonScript.gameObject, buttonScript);
        }

        RemoveGameplayProgressStore();
    }

    void Update()
    {
        if (Input.touchCount < 1 || rejectTouches)
            return;

        RaycastTouches();
    }

    /// <summary>
    /// Remove the <see cref="GameObject"/> with a <see cref="GameplayProgressStore"/> if it exists in the scene.
    /// </summary>
    void RemoveGameplayProgressStore()
    {
        var gameplayStore = GameObject.FindObjectOfType<GameplayProgressStore>();
        if (gameplayStore != null)
            Destroy(gameplayStore.gameObject);
    }

    /// <summary>
    /// Creates a ray at the touch and if it hits a button then invokes the <see cref="Button3DScript.SwitchScene"/>.
    /// </summary>
    void RaycastTouches()
    {
        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            var worldRay = camera.ScreenPointToRay(touch.position);
            if (Physics.Raycast(worldRay, out var hit, maxRaycastDistance))
            {
                if (buttons.ContainsKey(hit.collider.gameObject))
                {
                    buttons[hit.collider.gameObject].SwitchScene();
                    rejectTouches = true;
                }
            }
        }
    }
}
