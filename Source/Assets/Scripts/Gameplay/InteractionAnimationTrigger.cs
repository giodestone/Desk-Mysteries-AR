using UnityEngine;

/// <summary>
/// Sends a to the animator that its a part of, which names are configured by the <see cref="Initialize(string?, string?, string?)"/> function.
/// </summary>
[RequireComponent(typeof(Animator))]
public class InteractionAnimationTrigger : Interactable
{
    // References
    Animator animator;

    // Configuration
#nullable enable
    string? beginTriggerName;
    string? ongoingTriggerName;
    string? stoppingTriggerName;
#nullable disable

    /// <summary>
    /// Initialize the <see cref="InteractionAnimationTrigger"/>
    /// </summary>
    /// <param name="beginTriggerName">Name of trigger to set when the interaction happening is a <see cref="InteractionState.Begin"/>. Set once. Null if nothing should happen.</param>
    /// <param name="ongoingTriggerName">Name of trigger to set when the interaction happening is a <see cref="InteractionState.Ongoing"/>. Set as long as interaction is happening. Null if nothing should happen.</param>
    /// <param name="stoppingTriggerName">Name of trigger to set when the interaction happening is a <see cref="InteractionState.Stopping"/>. Set once. Null if nothing should happen.</param>
    public void Initialize(string? beginTriggerName=null, string? ongoingTriggerName=null, string? stoppingTriggerName=null)
    {
        this.beginTriggerName = beginTriggerName;
        this.ongoingTriggerName = ongoingTriggerName;
        this.stoppingTriggerName = stoppingTriggerName;

        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Override for <see cref="InteractionHappening(InteractionState, RaycastHit)"/>. Calls <see cref="Animator.SetTrigger(string)"/> 
    /// on the <see cref="animator"/> the <see cref="GameObject"/> contains depending the <see cref="InteractionState"/>.
    /// </summary>
    /// <param name="state"></param>
    /// <param name="hit"></param>
    public override void InteractionHappening(InteractionState state, RaycastHit hit)
    {
        
        switch (state)
        {
            case InteractionState.Begin:
                if (beginTriggerName != null)
                    animator.SetTrigger(beginTriggerName);
                break;

            case InteractionState.Ongoing:
                if (ongoingTriggerName != null)
                    animator.SetTrigger(ongoingTriggerName);
                break;

            case InteractionState.Stopping:
                if (stoppingTriggerName != null)
                    animator.SetTrigger(stoppingTriggerName);
                break;
        }
    }
}