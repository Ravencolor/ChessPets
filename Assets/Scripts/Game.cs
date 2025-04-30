using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    // Référence depuis l'IDE Unity
    public GameObject chesspiece;

    // Matrices nécessaires, positions de chaque GameObject
    // Aussi des tableaux séparés pour les joueurs afin de les suivre facilement
    // Gardez à l'esprit que les mêmes objets seront dans "positions" et "playerBlack"/"playerWhite"
    private GameObject[,] positions = new GameObject[8, 8]; // Tableau 2D pour stocker les positions des pièces sur le plateau
    private GameObject[] playerBlack = new GameObject[16]; // Tableau pour les pièces du joueur noir
    private GameObject[] playerWhite = new GameObject[16]; // Tableau pour les pièces du joueur blanc

    // Tour actuel
    private string currentPlayer = "white"; // Le joueur qui joue actuellement ("white" ou "black")

    // Fin de la partie
    private bool gameOver = false; // Indique si la partie est terminée

    // Unity appelle cette méthode au démarrage du jeu, il existe quelques fonctions intégrées
    public void Start()
    {
        // Initialisation des pièces blanches avec leurs positions de départ
        playerWhite = new GameObject[] { Create("white_pig", 0, 0), Create("white_frog", 1, 0),
            Create("white_crow", 2, 0), Create("white_cat", 3, 0), Create("white_human", 4, 0),
            Create("white_crow", 5, 0), Create("white_frog", 6, 0), Create("white_pig", 7, 0),
            Create("white_dog", 0, 1), Create("white_dog", 1, 1), Create("white_dog", 2, 1),
            Create("white_dog", 3, 1), Create("white_dog", 4, 1), Create("white_dog", 5, 1),
            Create("white_dog", 6, 1), Create("white_dog", 7, 1) };

        // Initialisation des pièces noires avec leurs positions de départ
        playerBlack = new GameObject[] { Create("black_pig", 0, 7), Create("black_frog",1,7),
            Create("black_crow",2,7), Create("black_cat",3,7), Create("black_human",4,7),
            Create("black_crow",5,7), Create("black_frog",6,7), Create("black_pig",7,7),
            Create("black_dog", 0, 6), Create("black_dog", 1, 6), Create("black_dog", 2, 6),
            Create("black_dog", 3, 6), Create("black_dog", 4, 6), Create("black_dog", 5, 6),
            Create("black_dog", 6, 6), Create("black_dog", 7, 6) };

        // Définit toutes les positions des pièces sur le plateau
        for (int i = 0; i < playerBlack.Length; i++)
        {
            SetPosition(playerBlack[i]); // Place les pièces noires sur le plateau
            SetPosition(playerWhite[i]); // Place les pièces blanches sur le plateau
        }
    }

    // Crée une pièce d'échec avec un nom et des coordonnées spécifiques
    public GameObject Create(string name, int x, int y)
    {
        GameObject obj = Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity); // Instancie un objet pièce
        Chessman cm = obj.GetComponent<Chessman>(); // Récupère le script Chessman attaché à l'objet
        cm.name = name; // Définit le nom de la pièce
        cm.SetXBoard(x); // Définit la position X sur le plateau
        cm.SetYBoard(y); // Définit la position Y sur le plateau
        cm.Activate(); // Active la pièce
        return obj;
    }

    // Définit la position d'une pièce sur le plateau
    public void SetPosition(GameObject obj)
    {
        Chessman cm = obj.GetComponent<Chessman>();
        positions[cm.GetXBoard(), cm.GetYBoard()] = obj; // Met à jour la matrice des positions
    }

    // Vide une position spécifique sur le plateau
    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null; // Supprime la pièce à la position donnée
    }

    // Récupère la pièce à une position spécifique
    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
    }

    // Vérifie si une position est valide sur le plateau
    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1)) return false;
        return true;
    }

    // Récupère le joueur actuel
    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }

    // Vérifie si la partie est terminée
    public bool IsGameOver()
    {
        return gameOver;
    }

    // Passe au tour suivant
    public void NextTurn()
    {
        if (currentPlayer == "white")
        {
            currentPlayer = "black"; // Change le joueur actuel en noir
        }
        else
        {
            currentPlayer = "white"; // Change le joueur actuel en blanc
        }
    }

    // Méthode appelée à chaque frame par Unity
    public void Update()
    {
        if (gameOver == true && Input.GetMouseButtonDown(0)) // Si la partie est terminée et qu'un clic est détecté
        {
            gameOver = false;
            SceneManager.LoadScene("Game"); // Redémarre le jeu en rechargeant la scène
        }
    }
    
    // Déclare un gagnant et affiche les messages correspondants
    public void Winner(string playerWinner)
    {
        gameOver = true;

        // Affiche le texte du gagnant
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = true;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = playerWinner + " est le gagnant";

        // Affiche le texte pour redémarrer
        GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = true;
    }
}