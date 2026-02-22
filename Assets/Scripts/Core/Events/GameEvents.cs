using UnityEngine;
using System;

public static class GameEvents {

    public static Action<Card> OnCardSpawned;
    public static Action<Card> OnCardEnteredConveyor;
    public static Action<Card> OnCardCollected;
    public static Action<GeneratorCrate> OnGeneratorTapped;
}