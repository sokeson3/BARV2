using UnityEngine;
using UnityEngine.SceneManagement; // Required for loading scenes

public class MainMenuUI : MonoBehaviour
{
    // This function will be called by our button
    public void LoadGameScene()
    {
        // "Game" must match the scene name in your Build Settings
        SceneManager.LoadScene("Game");
    }
}