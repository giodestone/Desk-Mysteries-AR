/// <summary>
/// The progress of the interaction, whether its beginning, in progress etc.
/// </summary>
public enum InteractionState
{
    Begin, // Called once when an interaction begins.
    Ongoing, // Called during the interaction repeatedly.
    Stopping // Called once when the item is no longer being interacted with.
}