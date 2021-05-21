using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script for a button interacted by with a raycast. Bobs when idle, spins when clicks. Loads a new scene after a small delay;
/// </summary>
public class Button3DScript : MonoBehaviour
{
    // Constants
    const float maxRotationSpeed = 360f * 2f; // per second
    float secondsToReachMaxRotationSpeed = 0.3f;
    const float passiveRotationStrengthMultiplier = 10f;

    // Configuration
    string sceneName;
    float delay;

    // Variables
    Vector3 originalRotation;
    float pressedRotation;
    bool isPressed = false;
    float timePressed;


    void Start()
    {
        originalRotation = transform.rotation.eulerAngles;
    }

    /// <summary>
    /// Setup the button by providing the name of the scene to switch to.
    /// </summary>
    /// <param name="nameOfSceneToSwitchTo">If the name is 'exit' the application will exit.</param>
    /// <param name="delay">Delay before starting to load the new scene.</param>
    public void Initialize(string nameOfSceneToSwitchTo, float delay=0.4f)
    {
        SceneManager.GetSceneByName(nameOfSceneToSwitchTo); // Basically an assertion - will crash if doesn't exist.
        this.sceneName = nameOfSceneToSwitchTo;
        this.delay = delay;
        secondsToReachMaxRotationSpeed = Mathf.Clamp(secondsToReachMaxRotationSpeed, 0f, delay);
    }

    /// <summary>
    /// Begin switching scene to one set by <see cref="Initialize"/> in the background after the specified <see cref="delay"/>.
    /// </summary>
    public void SwitchScene()
    {
        Invoke(nameof(BeginLoadingScene), delay);
        isPressed = true;
        timePressed = Time.time;
    }

    /// <summary>
    /// Begin loading the scene as defined by <see cref="sceneName"/>.
    /// </summary>
    void BeginLoadingScene()
    {
        if (sceneName.ToLower() == "exit")
        {
            Application.Quit();
            return;
        }

        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    /// <summary>
    /// Update position in physics fixed update function.
    /// </summary>
    void FixedUpdate()
    {
        if (isPressed)
        {
            // Rotate based on pressed rotation
            pressedRotation += Mathf.Lerp(0, maxRotationSpeed, Time.time - timePressed / timePressed + secondsToReachMaxRotationSpeed) * Time.fixedDeltaTime;
            transform.rotation = Quaternion.Euler(
                originalRotation.x + (Mathf.Cos(timePressed) * passiveRotationStrengthMultiplier),
            originalRotation.y + pressedRotation + (Mathf.Sin(timePressed) * passiveRotationStrengthMultiplier),
            originalRotation.z);
        }
        else
            transform.rotation = Quaternion.Euler(
                originalRotation.x + (Mathf.Cos(Time.time) * passiveRotationStrengthMultiplier),
            originalRotation.y + (Mathf.Sin(Time.time) * passiveRotationStrengthMultiplier),
            originalRotation.z);
    }
}
