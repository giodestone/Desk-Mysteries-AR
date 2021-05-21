using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Adds logic for the menu button on the top left of the screen to pause the game by interacting with <see cref="PauseManager.IsPaused"/>.
/// </summary>
[RequireComponent(typeof(Button))]
public class MenuButtonScript : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnMenuButtonClicked);
    }

    /// <summary>
    /// Callback for when the menu button is clicked. Sets <see cref="PauseManager.IsPaused"/> to true.
    /// </summary>
    void OnMenuButtonClicked()
    {
        PauseManager.IsPaused = true;
    }
}
