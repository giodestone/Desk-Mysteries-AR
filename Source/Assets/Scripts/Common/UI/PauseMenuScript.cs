using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Script for the pause menu implementing functions for the buttons.
/// </summary>
public class PauseMenuScript : MonoBehaviour
{
    // References
    UIManager uiManager;

    Button resumeButton;
    Button rescanAreaButton;
    Button mainMenuButton;

    void Start()
    {
        SetupReferences();
        
        resumeButton.onClick.AddListener(OnResumeButtonPressed);
        rescanAreaButton.onClick.AddListener(OnRescanAreaButtonpressed);
        mainMenuButton.onClick.AddListener(OnMainMenuButtonPressed);
    }

    void SetupReferences()
    {
        uiManager = GameObject.FindObjectOfType<UIManager>();
        resumeButton = GetButtonInChildOfName("ResumeButton");
        rescanAreaButton = GetButtonInChildOfName("RescanAreaButton");
        mainMenuButton = GetButtonInChildOfName("MainMenuButton");
    }

    /// <summary>
    /// Gets a <see cref="Button"/> component in the immediate child <see cref="GameObject"/>s if the <paramref name="name"/> matches.  
    /// </summary>
    /// <param name="name">Name of the child <see cref="GameObject"/> that should have the Button.</param>
    /// <returns><see cref="Button"/> if found; null if not found or the target <see cref="GameObject"/> doesn't have one.</returns>
    Button GetButtonInChildOfName(string name)
    {
        for (var i = 0; i < this.transform.childCount; ++i)
        {
            var child = this.transform.GetChild(i);
            if (child.name == name)
                return child.GetComponent<Button>();
        }

        return null;
    }

    /// <summary>
    /// Callback for resume button.
    /// </summary>
    void OnResumeButtonPressed()
    {
        PauseManager.IsPaused = false;
    }

    /// <summary>
    /// Callback for rescan area button.
    /// </summary>
    void OnRescanAreaButtonpressed()
    {
        PauseManager.IsPaused = false;
        SceneManager.LoadScene("PlaceMarker", LoadSceneMode.Single);
    }

    /// <summary>
    /// Callback for main menu button.
    /// </summary>
    void OnMainMenuButtonPressed()
    {
        PauseManager.IsPaused = false;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
