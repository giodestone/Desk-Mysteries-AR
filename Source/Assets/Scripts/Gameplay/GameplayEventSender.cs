
/// <summary>
/// Responsible for sending gameplay events to a receiver, which implements the <see cref="IGameplayEventReceiver"/> interface.
/// </summary>
public class GameplayEventSender
{
    IGameplayEventReceiver receiver;
    GameplayEvents actionToNotifyReceiverOf;

    /// <summary>
    /// Responsible for sending gameplay events to a receiver.
    /// </summary>
    /// <param name="receiver">What will 'receiver' the event.</param>
    /// <param name="actionToNotifyReceiverOf">Action to notify the receiver of.</param>
    public GameplayEventSender(IGameplayEventReceiver receiver, GameplayEvents actionToNotifyReceiverOf)
    {
        this.receiver = receiver;
        this.actionToNotifyReceiverOf = actionToNotifyReceiverOf;
    }

    /// <summary>
    /// Notify the receiver of the action to notify the receiver of.
    /// </summary>
    public void NotifyReciever()
    {
        receiver.NotifyOfEvent(actionToNotifyReceiverOf);
    }
}