using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Responsible for setting up and managing logic and progression of the Gameplay scene.
/// </summary>
public class GameplayManager : MonoBehaviour, IGameplayEventReceiver
{
    // Constants
    const float largeDrawerOutDistance = 0.35f;
    const float smallDrawerOutDistance = 0.16f;
    const float objectiveMessageDisplayTimeSeconds = 8f;

    // References
    UIManager uiManager;
    MoveDeskToMarker deskMover;
    AudioSource ownAudioSource;
    AudioClip keyPickupSound;

    Drawer leftLargeDrawer;
    Drawer rightLargeDrawer;
    Drawer bottomSmallDrawer;
    Drawer topSmallDrawer;
    InteractionSound leftLargeDrawerInteractionSound;
    InteractionSound rightLargeDrawerInteractionSound;
    InteractionSound bottomSmallDrawerInteractionSound;
    InteractionSound topSmallDrawerInteractionSound;
    
    AnimationPlayer penCompartmentAnimationPlayer;
    AnimationPlayer keySectionAnimationPlayer;
    AudioSource penCompartmentAudioSource;
    AudioClip penCompartmentSound;

    InteractionNotifier keyInteractionNotifier;
    InteractionNotifier keySectionInteractionNotifier;
    AudioSource keySectionAudioSource;
    AudioClip keySectionSound;

    InteractionNotifier xButtonInteractionNotifier;
    InteractionNotifier oButtonInteractionNotifier;
    InteractionAnimationTrigger xButtonAnimationTrigger;
    InteractionAnimationTrigger oButtonAnimationTrigger;
    InteractionSound xButtonInteractionSound;
    InteractionSound oButtonInteractionSound;

    GameplayProgressStore gameplayProgressStore;

    // Variables
    bool isRestoringFromGameplayProgressStore = false;

    bool isKeyFound = false;
    bool allowPatternInput = false;
    PatternRecognizer patternRecognizer;

    void Start()
    {
        SetupReferences();
        SetupDrawers();
        SetupPenCompartment();
        SetupKey();
        SetupButtons();
        SetupSounds();
        SetupGameplayProgressStore();
    }

    /// <summary>
    /// Change the <see cref="uiManager"/>'s group to <see cref="UIManagerCanvasGroups.Objective"/> and set it back
    /// after <see cref="objectiveMessageDisplayTimeSeconds"/>.
    /// </summary>
    void DisplayObjective()
    {
        uiManager.ChangeGroup(UIManagerCanvasGroups.Objective);
        Invoke(nameof(ChangeBackToUIGameplayState), objectiveMessageDisplayTimeSeconds);
    }

    void SetupReferences()
    {
        uiManager = GameObject.FindObjectOfType<UIManager>();

        deskMover = GameObject.FindObjectOfType<MoveDeskToMarker>();

        leftLargeDrawer = GameObject.Find("LeftLargeDrawer").GetComponent<Drawer>();
        rightLargeDrawer = GameObject.Find("RightLargeDrawer").GetComponent<Drawer>();

        bottomSmallDrawer = GameObject.Find("BottomSmallDrawer").GetComponent<Drawer>();
        topSmallDrawer = GameObject.Find("TopSmallDrawer").GetComponent<Drawer>();

        leftLargeDrawerInteractionSound = leftLargeDrawer.gameObject.GetComponent<InteractionSound>();
        rightLargeDrawerInteractionSound = rightLargeDrawer.gameObject.GetComponent<InteractionSound>();

        bottomSmallDrawerInteractionSound = bottomSmallDrawer.gameObject.GetComponent<InteractionSound>();
        topSmallDrawerInteractionSound = topSmallDrawer.gameObject.GetComponent<InteractionSound>();

        penCompartmentAnimationPlayer = GameObject.Find("PenCompartment").GetComponent<AnimationPlayer>();
        penCompartmentAudioSource = penCompartmentAnimationPlayer.gameObject.AddComponent<AudioSource>();

        keyInteractionNotifier = GameObject.Find("KeyModel").GetComponent<InteractionNotifier>();
        
        keySectionAnimationPlayer = GameObject.Find("KeySection").GetComponent<AnimationPlayer>();
        keySectionInteractionNotifier = GameObject.Find("KeySection").GetComponent<InteractionNotifier>();
        keySectionAudioSource = keySectionInteractionNotifier.gameObject.AddComponent<AudioSource>();

        xButtonInteractionNotifier = GameObject.Find("ButtonX").GetComponent<InteractionNotifier>();
        oButtonInteractionNotifier = GameObject.Find("ButtonO").GetComponent<InteractionNotifier>();

        xButtonAnimationTrigger = xButtonInteractionNotifier.gameObject.GetComponent<InteractionAnimationTrigger>();
        oButtonAnimationTrigger = oButtonInteractionNotifier.gameObject.GetComponent<InteractionAnimationTrigger>();

        xButtonInteractionSound = xButtonInteractionNotifier.GetComponent<InteractionSound>();
        oButtonInteractionSound = oButtonInteractionNotifier.GetComponent<InteractionSound>();

        gameplayProgressStore = GameObject.FindObjectOfType<GameplayProgressStore>();
    }

    /// <summary>
    /// Setup the various components to do with the drawers in the scene.
    /// </summary>
    void SetupDrawers()
    {
        leftLargeDrawer.Initialize(largeDrawerOutDistance);
        rightLargeDrawer.Initialize(largeDrawerOutDistance);

        bottomSmallDrawer.Initialize(smallDrawerOutDistance, true);
        topSmallDrawer.Initialize(smallDrawerOutDistance, true);

        leftLargeDrawerInteractionSound.Initialize(0.75f, Resources.Load<AudioClip>("Audio/Drawer"), null);
        rightLargeDrawerInteractionSound.Initialize(0.75f, Resources.Load<AudioClip>("Audio/Drawer"), null);
        bottomSmallDrawerInteractionSound.Initialize(0.75f, Resources.Load<AudioClip>("Audio/Drawer"), null);
        bottomSmallDrawerInteractionSound.ShouldPlaySound = false;
        topSmallDrawerInteractionSound.Initialize(0.75f, Resources.Load<AudioClip>("Audio/Drawer"), null);
        topSmallDrawerInteractionSound.ShouldPlaySound = false;
    }

    /// <summary>
    /// Setup the pen compartment sound and animation.
    /// </summary>
    void SetupPenCompartment()
    {
        penCompartmentAnimationPlayer.Initialize("OpenBox");
        penCompartmentSound = Resources.Load<AudioClip>("Audio/PenCompartment");
    }

    /// <summary>
    /// Setup the components on the collectible key.
    /// </summary>
    void SetupKey()
    {
        keyInteractionNotifier.Initialize(new GameplayEventSender(this, GameplayEvents.KeyFound));
        keySectionInteractionNotifier.Initialize(new GameplayEventSender(this, GameplayEvents.KeySectionClicked));
        keySectionAnimationPlayer.Initialize("Unlock");
        keySectionSound = Resources.Load<AudioClip>("Audio/Lock");
    }

    /// <summary>
    /// Setup the components on the X and O buttons.
    /// </summary>
    void SetupButtons()
    {
        xButtonInteractionNotifier.Initialize(new GameplayEventSender(this, GameplayEvents.XButtonPressed));
        oButtonInteractionNotifier.Initialize(new GameplayEventSender(this, GameplayEvents.OButtonPressed));

        xButtonAnimationTrigger.Initialize(beginTriggerName: "PressButton");
        oButtonAnimationTrigger.Initialize(beginTriggerName: "PressButton");

        patternRecognizer = new PatternRecognizer("XXOXOXOO", new GameplayEventSender(this, GameplayEvents.CorrectCodeEntered));
        
        xButtonInteractionSound.Initialize(0.5f, Resources.Load<AudioClip>("Audio/Button"), null);

        oButtonInteractionSound.Initialize(0.5f, Resources.Load<AudioClip>("Audio/Button"), null);
    }

    /// <summary>
    /// Adds an <see cref="AudioSource"/> to this <see cref="GameObject"/> (which ignores its spatial position) and loads a sound for <see cref="keyPickupSound"/>.
    /// </summary>
    void SetupSounds()
    {
        ownAudioSource = gameObject.AddComponent<AudioSource>();
        keyPickupSound = Resources.Load<AudioClip>("Audio/Key");
    }

    /// <summary>
    /// Add a <see cref="GameplayProgressStore"/> to the scene if not present and mark it to not be destroyed between scenes (so if player rescans the area their progress isn't lost).
    /// Otherwise updates the progress of the <see cref="GameplayManager"/> with what was stored.
    /// </summary>
    void SetupGameplayProgressStore()
    {
        if (gameplayProgressStore == null)
        {
            var gameplayObject = new GameObject("GamplayProgressStore");
            gameplayProgressStore = gameplayObject.AddComponent<GameplayProgressStore>();
            gameplayProgressStore.Initialize();
            DisplayObjective();
        }
        else
        {
            LoadProgressFromGameplayProgressStore();
        }
    }

    /// <summary>
    /// Calls <see cref="NotifyOfEvent(GameplayEvents)"/> with the events stored inside of <see cref="GameplayProgressStore"/>.
    /// </summary>
    void LoadProgressFromGameplayProgressStore()
    {
        while (gameplayProgressStore.AreGameplayEventsLeft)
        {
            isRestoringFromGameplayProgressStore = true;
            NotifyOfEvent(gameplayProgressStore.GetEvent());
            isRestoringFromGameplayProgressStore = false;
        }
    }

    /// <summary>
    /// Implementation of <see cref="IGameplayEventReceiver"/>. Provides logic for when these events are recieved and stores them in
    /// <see cref="GameplayProgressStore"/>.
    /// </summary>
    /// <param name="gameplayEvent"></param>
    public void NotifyOfEvent(GameplayEvents gameplayEvent)
    {
        if (!isRestoringFromGameplayProgressStore)
            gameplayProgressStore.LogEvent(gameplayEvent);

        switch (gameplayEvent)
        {
            case GameplayEvents.KeyFound:
                if (!isRestoringFromGameplayProgressStore)
                    ownAudioSource.PlayOneShot(keyPickupSound);
                
                keyInteractionNotifier.gameObject.SetActive(false);
                isKeyFound = true; // Make it possible to unlock the top drawers.
                break;

            case GameplayEvents.KeySectionClicked:
                if (!isKeyFound)
                    break;

                allowPatternInput = true; // make it possible to insert the pattern.

                if (!keySectionAnimationPlayer.HasPlayed)
                {
                    keySectionAnimationPlayer.PlayAnimation();
                    if (!isRestoringFromGameplayProgressStore)
                        keySectionAudioSource.PlayOneShot(keySectionSound);
                }

                topSmallDrawer.IsLocked = false;
                topSmallDrawerInteractionSound.ShouldPlaySound = true;
                bottomSmallDrawer.IsLocked = false;
                bottomSmallDrawerInteractionSound.ShouldPlaySound = true;
                break;

            case GameplayEvents.XButtonPressed:
                if (!allowPatternInput)
                    break;

                patternRecognizer.AddCharacter('X');
                break;

            case GameplayEvents.OButtonPressed:
                if (!allowPatternInput)
                    break;

                patternRecognizer.AddCharacter('O');
                break;

            case GameplayEvents.CorrectCodeEntered:
                allowPatternInput = false;
                penCompartmentAnimationPlayer.PlayAnimation();
                Invoke(nameof(ChangeToUIWinState), 1f); // Let the animation play out!

                if (!isRestoringFromGameplayProgressStore)
                    penCompartmentAudioSource.PlayOneShot(penCompartmentSound);
                break;
        }
    }

    /// <summary>
    /// Change the UI state to the win state.
    /// </summary>
    void ChangeToUIWinState()
    {
        uiManager.ChangeGroup(UIManagerCanvasGroups.Win);
    }

    /// <summary>
    /// Change the UI state to the gameplay state, hiding the objective before.
    /// </summary>
    void ChangeBackToUIGameplayState()
    {
        uiManager.ChangeGroup(UIManagerCanvasGroups.PlayerInteraction);
    }
}