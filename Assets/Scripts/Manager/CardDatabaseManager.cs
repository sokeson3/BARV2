using System.Collections.Generic;
using UnityEngine;

public class CardDatabaseManager : MonoBehaviour
{
    // --- Singleton Pattern ---
    // This ensures there is only one instance of this manager in the game.
    public static CardDatabaseManager Instance { get; private set; }

    // Dictionaries provide very fast lookups by key (in this case, the card's name).
    public Dictionary<string, CardData> allCards = new Dictionary<string, CardData>();

    private void Awake()
    {
        // Implement the singleton pattern.
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            // DontDestroyOnLoad keeps this manager alive when changing scenes.
            DontDestroyOnLoad(this.gameObject);
            LoadCardDatabase();
        }
    }

    private void LoadCardDatabase()
    {
        // Load all CardData objects from the "Resources/Data/CardData" folder.
        // The path is relative to any "Resources" folder.
        var loadedCards = Resources.LoadAll<CardData>("Data/CardData");

        foreach (var card in loadedCards)
        {
            if (!allCards.ContainsKey(card.cardName))
            {
                allCards.Add(card.cardName, card);
            }
            else
            {
                Debug.LogWarning($"Duplicate card name found: {card.cardName}.");
            }
        }
        Debug.Log($"Loaded {allCards.Count} cards into the database.");
    }

    // Example function to get a card by its name.
    public CardData GetCardByName(string cardName)
    {
        if (allCards.TryGetValue(cardName, out CardData card))
        {
            return card;
        }
        Debug.LogError($"Card '{cardName}' not found in the database!");
        return null;
    }
}