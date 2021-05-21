using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Stores the progress between the play instances. Should've been a static class but it was too late when I noticed :(
/// </summary>
public class GameplayProgressStore : MonoBehaviour
{
    // Properties
    /// <summary>
    /// Whether there are gameplay events in the internal queue to process.
    /// </summary>
    /// <value></value>
    public bool AreGameplayEventsLeft { get => gameplayEventQueue.Count > 0; }

    // Variables
    Queue<GameplayEvents> gameplayEventQueue;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void Initialize()
    {
        gameplayEventQueue = new Queue<GameplayEvents>();
    }

    /// <summary>
    /// Get rid of any events.
    /// </summary>
    public void ClearProgressStore()
    {
        gameplayEventQueue.Clear();
    }

    /// <summary>
    /// Store an event that has happened.
    /// </summary>
    /// <param name="eventToLog"></param>
    public void LogEvent(GameplayEvents eventToLog)
    {
        gameplayEventQueue.Enqueue(eventToLog);
    }

    /// <summary>
    /// Get the first event that has happened.
    /// </summary>
    /// <returns></returns>
    public GameplayEvents GetEvent()
    {
        return gameplayEventQueue.Dequeue();
    }
}