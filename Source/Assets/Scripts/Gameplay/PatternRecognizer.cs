using System;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

/// <summary>
/// Provides basic pattern recognition for strings and informs a <see cref="IGameplayEventReceiver"/> when the pattern is correct.
/// </summary>
public class PatternRecognizer
{
    // References
    GameplayEventSender onPatternRecognisedEventSender;

    // Configuration
    string targetPattern;

    // Variables
    string input;

    /// <summary>
    /// Create a new instance of <see cref="PatternRecognizer"/>.
    /// </summary>
    /// <param name="targetPattern">What pattern should be found before notifying <paramref name="onPatternRecognisedEventSender"/>.
    /// Consider using <see cref="GenerateRandomPattern(char[], int)"/> to generate the pattern.</param>
    /// <param name="onPatternRecognisedEventSender">A configured sender of events when the correct pattern is found.</param>
    public PatternRecognizer(string targetPattern, GameplayEventSender onPatternRecognisedEventSender)
    {
        this.targetPattern = targetPattern;
        if (targetPattern.Length == 0)
            throw new ArgumentException(message: "No target pattern!", paramName: nameof(targetPattern));

        this.onPatternRecognisedEventSender = onPatternRecognisedEventSender;
    }

    /// <summary>
    /// Generate a pattern with random letters using the values provided in <paramref name="possibleCharacters"/> of <paramref name="legnth"/>.
    /// </summary>
    /// <param name="possibleCharacters"></param>
    /// <param name="legnth"></param>
    /// <returns></returns>
    public static string GenerateRandomPattern(char[] possibleCharacters, int legnth)
    {
        var returnString = "";

        for (var i = 0; i < legnth; ++i)
        {
            returnString += possibleCharacters[Random.Range(0, possibleCharacters.Length)];
        }

        return returnString;
    }

    /// <summary>
    /// Add a character to the internal check.
    /// </summary>
    /// <param name="c">Character to add.</param>
    public void AddCharacter(char c)
    {
        input += c;
        TrimInput();
        MatchPattern();
    }

    /// <summary>
    /// Trim the input to be same size of the target pattern by removing the last character.
    /// </summary>
    void TrimInput()
    {
        if (input.Length > targetPattern.Length)
        {
            input = input.Remove(0, input.Length - targetPattern.Length);
        }
    }

    /// <summary>
    /// Notify <see cref="onPatternRecognisedEventSender"/> when <see cref="input"/> matches <see cref="targetPattern"/>.
    /// </summary>
    void MatchPattern()
    {
        if (input == targetPattern)
            onPatternRecognisedEventSender.NotifyReciever();
    }
}