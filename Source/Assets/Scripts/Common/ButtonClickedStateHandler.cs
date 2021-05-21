using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

/// <summary>
/// Tracks whether the button is clicked or not by providing access to a boolean.
/// </summary>
public class ButtonClickedStateHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool IsButtonClicked { get; private set; }

    /// <summary>
    /// <see cref="IPointerDownHandler"/> implementation.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        IsButtonClicked = true;
    }

    /// <summary>
    /// <see cref="IPointerUpHandler"/> implementation.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        IsButtonClicked = false;
    }
}