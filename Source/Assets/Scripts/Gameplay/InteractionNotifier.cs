using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Notifies an <see cref="IGameplayEventReceiver"/> when an <see cref="InteractionState.Begin"/> is occurring.
/// </summary>
public class InteractionNotifier : Interactable
{
    // References
    GameplayEventSender gameplayEventSender;

    /// <summary>
    /// Initialize the interaction notifier.
    /// </summary>
    /// <param name="gameplayEventSender">A configured event sender which will be used to inform of interactions when they happen.</param>
    public void Initialize(GameplayEventSender gameplayEventSender)
    {
        this.gameplayEventSender = gameplayEventSender;
    }

    /// <summary>
    /// Notifies an <see cref="IGameplayEventReceiver"/> when an <see cref="InteractionState.Begin"/> is occurring.
    /// </summary>
    /// <param name="state"></param>
    /// <param name="hit"></param>
    public override void InteractionHappening(InteractionState state, RaycastHit hit)
    {
        switch (state)
        {
            case InteractionState.Begin:
                gameplayEventSender.NotifyReciever();
                return;
        }
    }
}
