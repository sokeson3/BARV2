using UnityEngine;
using System.Collections.Generic;

public enum CardType { Unit, Spell, Trap, Landmark, Token }
public enum Region { Northern, Eastern, Southern, Western, Neutral }
public enum Keyword { Rush, Pierce, Token } // We will populate this from the description

[CreateAssetMenu(fileName = "New Card", menuName = "Card Data")]
public class CardData : ScriptableObject
{
    [Header("Card Identity")]
    public string cardID; // Unique ID from the CSV (e.g., "N001")
    public string cardName;
    public CardType cardType;
    public Region region;

    [Header("Visuals")]
    public Sprite cardImage;

    [Header("Gameplay Stats")]
    public int manaCost;
    public int attack;
    public int defense; // Mapped from the "HP" column in the CSV

    [Header("Text & Effects")]
    [TextArea(3, 5)]
    public string cardDescription;
    public List<Keyword> keywords;
}