/// <summary>
/// Interface for making a class an event receiver.
/// </summary>
public interface IGameplayEventReceiver
{
    /// <summary>
    /// Notify the receiver that an event has happened.
    /// </summary>
    /// <param name="gameplayEvent"></param>
    void NotifyOfEvent(GameplayEvents gameplayEvent);
}