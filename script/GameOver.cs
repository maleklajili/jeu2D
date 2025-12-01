using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    // Bouton pour revenir au menu principal
    public void Menu()
    {
        Debug.Log("Menu button clicked");
        SceneManager.LoadScene("Menu"); // Nom exact de la scène Menu
    }

    // Bouton pour recommencer le niveau actuel
    public void Retry()
    {
        Debug.Log("Retry button clicked");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Bouton pour aller vers LEVEL1
    public void GoToLevel1()
    {
        Debug.Log("Go to LEVEL1 button clicked");
        SceneManager.LoadScene("LEVEL2 1"); // Nom exact de ta scène LEVEL1
        // Si tu préfères utiliser l'index dans Build Settings :
        // SceneManager.LoadScene(1);
    }
}
