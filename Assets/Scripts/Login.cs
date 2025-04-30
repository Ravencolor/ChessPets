using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    // Champs pour les entrées utilisateur : nom d'utilisateur et mot de passe
    public TMP_InputField username;
    public TMP_InputField password;

    // Méthode appelée pour établir une connexion et envoyer une requête
    public void Connect()
    {
        // Démarre une coroutine pour envoyer une requête GET avec les informations utilisateur
        StartCoroutine(GetRequest("http://localhost/chesspets_db/GetUsers.php?username=" + username.text + "&password=" + password.text));
    }

    // Coroutine pour gérer la requête GET
    IEnumerator GetRequest(string uri)
    {
        // Crée une requête GET vers l'URI spécifié
        UnityWebRequest www = UnityWebRequest.Get(uri);
        // Attend que la requête soit terminée
        yield return www.SendWebRequest();

        // Vérifie si la requête a échoué
        if (www.result != UnityWebRequest.Result.Success)
        {
            // Affiche l'erreur dans la console
            Debug.Log(www.error);
        }
        else
        {
            // Affiche la réponse du serveur dans la console
            print(www.downloadHandler.text);
            // Charge la scène "Main Menu" si la requête a réussi
            SceneManager.LoadScene("Main Menu");
        }
    }
}
