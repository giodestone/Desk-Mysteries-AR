using UnityEngine;

/// <summary>
/// Plays a sound when an interaction is either <see cref="InteractionState.Begin"/> or <see cref="InteractionState.Stopping"/>.
/// </summary>
/// <remarks>
/// Will not play sounds if it is already playing one.
/// </remarks>
public class InteractionSound : Interactable
{
    // Properties
    /// <summary>
    /// Whether a sound should be played when an interaction happens.
    /// </summary>
    /// <value>True if a sound should be played; false if it shouldn't.</value>
    public bool ShouldPlaySound { get; set; }

    // References
    bool isPlayingSound = false;

    AudioSource audioSource;
    AudioClip beginAudioClip;
    AudioClip stoppingAudioClip;

    /// <summary>
    /// Initialize the <see cref="InteractionSound"/>. Creates a 3D <see cref="AudioSource"/> in the <see cref="GameObject"/> its attached to.
    /// </summary>
    /// <param name="volume">Volume to play the clips back at.</param>
    /// <param name="beginAudioClip">Audio clip to play when <see cref="InteractionState.Begin"/> happens. Can be <see langword="null"/> (no sound will be played).</param>
    /// <param name="stoppingAudioClip">Audio clip to play when <see cref="InteractionState.Stopping"/> happens. Can be <see langword="null"/> (no sound will be played).</param>
    /// <param name="isEnabled">Sets value of <see cref="ShouldPlaySound"/>.</param>
    public void Initialize(float volume, AudioClip beginAudioClip, AudioClip stoppingAudioClip, bool isEnabled=true)
    {
        AddAudioSource(volume);
        this.beginAudioClip = beginAudioClip;
        this.stoppingAudioClip = stoppingAudioClip;
        ShouldPlaySound = isEnabled;
    }

    /// <summary>
    /// Creates a 3D audio source and sets its volume.
    /// </summary>
    /// <param name="volume"></param>
    void AddAudioSource(float volume)
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1f; // Make sound 3D.
        audioSource.volume = volume;
    }

    /// <summary>
    /// Override for <see cref="Interactable.InteractionHappening(InteractionState, RaycastHit)"/>. Plays the begin/stopping audio clips if they
    /// are not null, and if <see cref="ShouldPlaySound"/> is true.
    /// </summary>
    /// <param name="state"></param>
    /// <param name="hit"></param>
    public override void InteractionHappening(InteractionState state, RaycastHit hit)
    {
        if (!ShouldPlaySound || isPlayingSound)
            return;
        
        switch (state)
        {
            case InteractionState.Begin:
                if (beginAudioClip != null)
                {
                    audioSource.PlayOneShot(beginAudioClip);
                    isPlayingSound = true;
                    Invoke(nameof(OnStopPlayingSound), beginAudioClip.length);
                }

                break;

            case InteractionState.Stopping:
                if (stoppingAudioClip != null)
                {
                    audioSource.PlayOneShot(stoppingAudioClip);
                    isPlayingSound = true;
                    Invoke(nameof(OnStopPlayingSound), beginAudioClip.length);
                }
                break;
        }
    }

    /// <summary>
    /// Callback for whether the item is playing sound.
    /// </summary>
    void OnStopPlayingSound()
    {
        isPlayingSound = false;
    }
}