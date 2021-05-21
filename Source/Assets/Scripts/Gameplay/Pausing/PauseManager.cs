using System.Collections.Generic;

/// <summary>
/// Manager class for <see cref="IPausable"/>s, calling <see cref="IPausable.Pause"/> and <see cref="IPausable.UnPause"/>.
/// </summary>
/// <remarks>
/// Provides the extensions <see cref="RegisterWithPauseManager(IPausable)"/> and <see cref="UnregisterWithPauseManager(IPausable)"/>.
/// </remarks>
public static class PauseManager
{
    static bool isPaused = false;

    /// <summary>
    /// Whether everything is paused or not. Will pause/unpause when value is set.
    /// </summary>
    /// <value></value>
    public static bool IsPaused
    {
        get
        {
            return isPaused;
        }
        set
        {
            if (value == false && isPaused == true)
            {
                isPaused = value;
                UnPauseAll();
            }
            else if (value == true && isPaused == false)
            {
                isPaused = value;
                PauseAll();
            }
        }
    }

    static List<IPausable> pausables = new List<IPausable>();

    /// <summary>
    /// Register the <paramref name="pausable"/> with the pause manager. Should be done in <see cref="IPausable.RegisterPausable"/>.
    /// Will call relevant function relating to paused status after being added.
    /// </summary>
    /// <param name="pausable"></param>
    public static void RegisterWithPauseManager(this IPausable pausable)
    {
        pausables.Add(pausable);
        
        if (isPaused)
            pausable.Pause();
        else
            pausable.UnPause();
    }

    /// <summary>
    /// Unregister the <paramref name="pausable"/> with the pause manager. Should be done in <see cref="IPausable.UnregisterPausable"/>.
    /// </summary>
    /// <param name="pausable"></param>
    public static void UnregisterWithPauseManager(this IPausable pausable)
    {
        pausables.Remove(pausable);
    }

    /// <summary>
    /// Calls <see cref="IPausable.Pause"/> on all <see cref="pausables"/>.
    /// </summary>
    static void PauseAll()
    {
        foreach (var pausable in pausables)
            pausable.Pause();
    }

    /// <summary>
    /// Calls <see cref="IPausable.UnPause"/> on all <see cref="pausables"/>.
    /// </summary>
    static void UnPauseAll()
    {
        foreach (var pausable in pausables)
            pausable.UnPause();
    }
}