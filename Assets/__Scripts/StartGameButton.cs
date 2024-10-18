using UnityEngine;
using UnityEngine.SceneManagement;  // Import Scene Management for scene loading

public class StartGameButton : MonoBehaviour
{
    // Function to load the game scene
    public void StartGame()
    {
        SceneManager.LoadScene("__Scene_0");  // Replace "__Scene_0" with the exact name of your game scene
    }
}
