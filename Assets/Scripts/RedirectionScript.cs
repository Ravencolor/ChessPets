using UnityEngine;
using UnityEngine.SceneManagement;

public class RedirectionScript : MonoBehaviour
{
    // Charge une nouvelle scène en fonction de son nom
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Charge la scène suivante dans la liste des scènes
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    // Recharge la scène actuelle
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Quitte l'application
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitter le jeu");
    }
}
