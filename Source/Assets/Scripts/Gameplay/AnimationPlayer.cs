using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

/// <summary>
/// For playing one shot animations by setting a trigger.
/// </summary>
public class AnimationPlayer : MonoBehaviour
{
    // Properties
    /// <summary>
    /// Whether the animation has been played.
    /// </summary>
    /// <value>True if <see cref="PlayAnimation"/> has been called; false if it hasn't.</value>
    public bool HasPlayed { get; private set; }

    // References
    Animator anim;

    // Variables
    bool startedPlaying = false;
    string triggerName; // You cannot create the state graph in code and setting the animation to legacy seems impossible to play it via code.

    void Start()
    {
        HasPlayed = false;
        SetupReferences();
    }

    void SetupReferences()
    {
        anim = GetComponent<Animator>(); // There is no way to configure the animation clip (and the animation controller) inside of code that would work in release mode. Requires UnityEditor namespace which wouldn't build into Release (i.e. apk).
    }

    /// <summary>
    /// Setup the animation.
    /// </summary>
    /// <param name="triggerName">Name of the trigger to set when <see cref="PlayAnimation"/> is called.</param>
    public void Initialize(string triggerName)
    {
        this.triggerName = triggerName;
    }

    /// <summary>
    /// Play the animation.
    /// </summary>
    public void PlayAnimation()
    {
        startedPlaying = true;
        anim.SetTrigger(triggerName);
        HasPlayed = true;
    }
}
