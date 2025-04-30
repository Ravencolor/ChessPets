using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // Méthode appelée lors du clic sur le bouton
    public void LoadmainmenuScene()
    {
        // Vérifie si la scène "main menu" est bien dans les Build main menu
        if (SceneExists("main menu"))
        {
            SceneManager.LoadScene("main menu"); // Charge la scène main menu
        }
        else
        {
            Debug.LogError("La scène 'main menu' n'est pas ajoutée dans les Build Settings !");
        }
    }

    // Vérifie si une scène est dans les Build Settings
    private bool SceneExists(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneFileName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (sceneFileName == sceneName)
            {
                return true;
            }
        }
        return false;
    }
}
