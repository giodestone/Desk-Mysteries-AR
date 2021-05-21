using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Player controller for the Gameplay scene. Calls the <see cref="Interactable.InteractionHappening(InteractionState, UnityEngine.RaycastHit)"/>
/// function on <see cref="Interactable"/>s.
/// </summary>
public class GameplayController : MonoBehaviour, IPausable
{
    // Constants
    const float playerReach = 5f;
    const string interactableTag = "Interactable";

    // References
    new Camera camera;
    ButtonClickedStateHandler interactButtonState;


    // Variables
    ComponentToGameObjectLookup<Interactable> interactableLookup;

    bool isPaused;

    bool wasButtonClicked;
    Collider previouslyHitCollider; // Use collider because the hit is on the collider not the GameObject.
    RaycastHit previousHit;


    void Start()
    {
        RegisterPausable();

        camera = GameObject.FindObjectOfType<Camera>();
        if (camera.GetComponent<AudioSource>() == null) // Allow the player to listen to the beautifully synced up sounds.
            camera.gameObject.AddComponent<AudioListener>();

        interactableLookup = new ComponentToGameObjectLookup<Interactable>();
        var interactButton = GameObject.Find("InteractButton").GetComponent<Button>();
        interactButtonState = interactButton.GetComponent<ButtonClickedStateHandler>();
        wasButtonClicked = false;
    }

    void Update()
    {
        if (isPaused)
            return;

        InteractionRaycast();
    }

    void OnDestroy()
    {
        UnregisterPausable();
    }

    /// <summary>
    /// Perform the raycast which determines on which object the <see cref="Interactable.InteractionHappening(InteractionState, RaycastHit)"/> is
    /// called on. The <see cref="Interactable"/> must be in <see cref="interactableLookup"/> for it to be called. Make sure all objects are
    /// indexed.
    /// </summary>
    void InteractionRaycast()
    {
        // Tell the last thing that was clicked that it no longer is.
        if (!interactButtonState.IsButtonClicked)
        {
            SendStoppedInteractingToPreviousInteractable();
        }

        // On read things on key down, not while pressed.
        if (interactButtonState.IsButtonClicked && wasButtonClicked)
        {
            wasButtonClicked = interactButtonState.IsButtonClicked;
            return;
        }

        if (interactButtonState.IsButtonClicked && Physics.Raycast(camera.transform.position, camera.transform.forward, out var hit, playerReach))
        {
            if (interactableLookup.GetComponentsIfExists(hit.collider.gameObject, out var interactables))
            {
                InteractionState state;

                // If this is the first hit on this collider.
                if (previouslyHitCollider != hit.collider)
                {
                    state = InteractionState.Begin;
                }
                else
                {
                    state = InteractionState.Ongoing;
                }

                foreach (var interactable in interactables)
                {
                    interactable.InteractionHappening(state, hit);
                }

                previouslyHitCollider = hit.collider;
                previousHit = hit;
            }
        }

        wasButtonClicked = interactButtonState.IsButtonClicked;
    }

    /// <summary>
    /// Inform <see cref="previouslyHitCollider"/> that it is no longer interacted with.
    /// </summary>
    void SendStoppedInteractingToPreviousInteractable()
    {
        if (previouslyHitCollider != null)
        {
            // Inform the previously collided thing that the collision has stopped.
            foreach (var interactable in interactableLookup.GetComponentsFromGameObject(previouslyHitCollider.gameObject))
            {
                interactable.InteractionHappening(InteractionState.Stopping, previousHit);
            }
            previouslyHitCollider = null;
        }
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
