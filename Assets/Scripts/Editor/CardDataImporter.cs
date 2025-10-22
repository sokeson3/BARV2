using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CardDataImporter
{
    private static string csvPath = "Assets/Data/card_data.csv";

    [MenuItem("Tools/Import Card Data")]
    public static void ImportCards()
    {
        string[] allLines = File.ReadAllLines(Application.dataPath + csvPath.Replace("Assets", ""));

        // Loop through all lines, starting from the second line (index 1) to skip the header.
        for (int i = 1; i < allLines.Length; i++)
        {
            string line = allLines[i];
            if (string.IsNullOrWhiteSpace(line)) continue; // Skip empty lines

            // Use a robust method to split the CSV line to handle commas within quotes
            string[] columns = SplitCsvLine(line);

            if (columns.Length < 8)
            {
                Debug.LogWarning($"Skipping line {i + 1}: Not enough columns. Found {columns.Length}.");
                continue;
            }

            // --- Create the ScriptableObject Asset ---
            CardData cardData = ScriptableObject.CreateInstance<CardData>();

            try
            {
                // --- Populate Data from Columns ---
                cardData.cardID = columns[0];
                cardData.cardName = columns[1];

                // Clean up region name typos before parsing
                string regionString = columns[3];
                if (regionString.Equals("Nothern", StringComparison.OrdinalIgnoreCase))
                {
                    regionString = "Northern";
                }
                cardData.region = (Region)Enum.Parse(typeof(Region), regionString, true);
                cardData.cardType = (CardType)Enum.Parse(typeof(CardType), columns[2], true);

                cardData.attack = int.Parse(columns[4]);
                cardData.defense = int.Parse(columns[5]); // Mapping HP to defense
                cardData.manaCost = int.Parse(columns[6]);
                cardData.cardDescription = columns[7];

                // --- Extract Keywords from Description ---
                cardData.keywords = new List<Keyword>();
                if (cardData.cardDescription.IndexOf("Rush", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    cardData.keywords.Add(Keyword.Rush);
                }
                if (cardData.cardDescription.IndexOf("Pierce", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    cardData.keywords.Add(Keyword.Pierce);
                }
                if (cardData.cardType == CardType.Token)
                {
                    cardData.keywords.Add(Keyword.Token);
                }


                // --- Link Card Image based on ID ---
                // IMPORTANT: Assumes your image files are named like "N001.png", "E014a.png", etc.
                string imagePath = $"Assets/Art/Cards/{cardData.cardID}.png";
                cardData.cardImage = AssetDatabase.LoadAssetAtPath<Sprite>(imagePath);

                if (cardData.cardImage == null)
                {
                    Debug.LogWarning($"Image not found for '{cardData.cardName}' (ID: {cardData.cardID}). Expected path: {imagePath}");
                }

                // --- Save the Asset ---
                // Ensure the target directory exists
                if (!AssetDatabase.IsValidFolder("Assets/Data/CardData"))
                {
                    AssetDatabase.CreateFolder("Assets/Data", "CardData");
                }
                string assetPath = $"Assets/Data/CardData/{cardData.cardID}_{cardData.cardName}.asset";
                AssetDatabase.CreateAsset(cardData, assetPath);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to parse line {i + 1} for card '{columns[1]}'. Error: {e.Message}");
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh(); // Refresh the asset database to show new files
        Debug.Log("Card import complete!");
    }

    // A simple but more robust CSV line splitter that handles quoted fields.
    private static string[] SplitCsvLine(string line)
    {
        // This regex splits by commas, but ignores commas inside double quotes.
        return Regex.Split(line, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
    }
}