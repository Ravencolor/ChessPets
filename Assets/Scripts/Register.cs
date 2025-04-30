using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Register : MonoBehaviour
{
    public TMP_InputField username; // Champ pour entrer le nom d'utilisateur
    public TMP_InputField password; // Champ pour entrer le mot de passe

    public void CreateAccount()
    {
        // Envoie une requête pour créer un compte avec le nom d'utilisateur et le mot de passe
        StartCoroutine(GetRequest("http://localhost/chesspets_db/AddUser.php?username=" + username.text + "&password=" + password.text));
    }

    IEnumerator GetRequest(string uri)
    {
        UnityWebRequest www = UnityWebRequest.Get(uri); // Crée une requête HTTP GET
        yield return www.SendWebRequest(); // Attend la réponse

        if (www.result != UnityWebRequest.Result.Success)
        {
            // Affiche une erreur si la requête échoue
            Debug.Log(www.error);
        }
        else
        {
            // Affiche la réponse et charge la scène "Main Menu" si la requête réussit
            print(www.downloadHandler.text);
            SceneManager.LoadScene("Main Menu");
        }
    }
}
