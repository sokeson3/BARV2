using UnityEngine;

[CreateAssetMenu(fileName = "GameRules", menuName = "Game Rules")]
public class GameRules : ScriptableObject
{
    [Header("Game Settings")]
    public int maxHandSize = 8;
    public int maxSavedMana = 4;

    [Header("Life Points (LP)")]
    public int regionalMatchLP = 4000;
    public int duelLP = 4000;

    [Header("War Campaign LP")]
    public int campaignR1_LP = 2000;
    public int campaignR2_LP = 3000;
    public int campaignR3_LP = 4000;
    public int campaignR4_LP = 4000;
    public int campaignR5_LP = 5000;
}