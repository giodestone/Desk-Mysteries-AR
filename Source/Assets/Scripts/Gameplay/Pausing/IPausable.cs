using System.Collections.Generic;

/// <summary>
/// Provides an interface to be notified of when to be paused or not.
/// </summary>
public interface IPausable
{
    /// <summary>
    /// Register as a pausable item. Call when the object is initialized. Should call 
    /// <see cref="PauseManager.RegisterWithPauseManager(IPausable)"/> in this function to receive pause/unpause notifications.
    /// </summary>
    void RegisterPausable();

    /// <summary>
    /// Unregister as a pausable item. Call this when the object should be destroyed. Should call
    /// <see cref="PauseManager.UnregisterWithPauseManager(IPausable)"/> in this function.
    /// </summary>
    void UnregisterPausable();

    /// <summary>
    /// For logic involving what to do when paused. Called when the game becomes paused.
    /// </summary>
    void Pause();

    /// <summary>
    /// For logic involving what to do when unpaused. Called when the game becomes unpaused.
    /// </summary>
    void UnPause();
}