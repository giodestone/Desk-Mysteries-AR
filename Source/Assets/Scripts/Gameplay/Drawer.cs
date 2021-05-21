using UnityEngine;

/// <summary>
/// Implements logic for a drawer.
/// </summary>
public class Drawer : Interactable
{
    // Constants
    const float transitionSpeedMultiplier = 2f;

    // Properties

    /// <summary>
    /// Whether the drawer can be opened or not.
    /// </summary>
    /// <value>True if the drawer cannot be opened; false if it can.</value>
    public bool IsLocked { get => isLocked; set => isLocked = value; }

    // References
    new Camera camera;

    // Configuration
    float outDistance;
    bool isLocked;

    // Variables
    Vector3 originalPosition;
    bool isTransitioning;
    float transitionDirection; // Should be either -1 or 1 depending whether it should be opening or closing.
    float transitionProgress = 1f;

    /// <summary>
    /// Initialize the drawer.
    /// </summary>
    /// <param name="outDistance">How far should go out.</param>
    /// <param name="isLocked">Whether the drawer can be opened or not.</param>
    public void Initialize(float outDistance, bool isLocked = false) 
    {
        this.outDistance = outDistance;
        this.isLocked = isLocked;

        camera = GameObject.FindObjectOfType<Camera>();
        transitionProgress = 0f;
        originalPosition = transform.localPosition;

        isTransitioning = false;
        transitionDirection = 1f;
    }

    /// <summary>
    /// Override for when an interation is happening. Opens/closes the drawer. Rejects additional interactions
    /// if a drawer transition is taking place.
    /// </summary>
    /// <param name="state"></param>
    /// <param name="hit"></param>
    public override void InteractionHappening(InteractionState state, RaycastHit hit)
    {
        if (isTransitioning || isLocked)
            return;
        
        switch (state)
        {
            case InteractionState.Begin:
                isTransitioning = true;
                break;
        }
    }

    void Update()
    {
        if (!isTransitioning)
            return;

        var localForward = transform.worldToLocalMatrix.MultiplyVector(transform.forward); // transform.forward is in global space.

        transitionProgress += Time.deltaTime * transitionSpeedMultiplier * transitionDirection;
            transform.localPosition = Vector3.Lerp(originalPosition, originalPosition + (localForward * outDistance), transitionProgress);

        if (transitionProgress >= 1f || transitionProgress <= 0f)
        {
            transitionProgress = Mathf.Clamp01(transitionProgress);
            transitionDirection *= -1f; // Nicer than making an if statement.
            isTransitioning = false;
        }
    }
}