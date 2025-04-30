using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    // Référence au GameController et à l'Historique
    public GameObject controller;
    public HistoriqueManager historiqueManager; // Ajout de l'historique des coups

    // La pièce qui a créé cette MovePlate
    GameObject reference = null;

    // Position sur l'échiquier
    int matrixX;
    int matrixY;

    // false: mouvement normal, true: attaque
    public bool attack = false;

    public void Start()
    {
        if (attack)
        {
            // Couleur rouge pour l'attaque
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        historiqueManager = GameObject.FindObjectOfType<HistoriqueManager>(); // Récupération du script d'historique

        // Suppression de la pièce ennemie si c'est une attaque
        if (attack)
        {
            GameObject cp = controller.GetComponent<Game>().GetPosition(matrixX, matrixY);

            if (cp.name == "white_human") controller.GetComponent<Game>().Winner("black");
            if (cp.name == "black_human") controller.GetComponent<Game>().Winner("white");

            Destroy(cp);
        }

        // Sauvegarde de la position d'origine
        int fromX = reference.GetComponent<Chessman>().GetXBoard();
        int fromY = reference.GetComponent<Chessman>().GetYBoard();

        // Suppression de l'ancienne position sur l'échiquier
        controller.GetComponent<Game>().SetPositionEmpty(fromX, fromY);

        // Déplacement de la pièce
        reference.GetComponent<Chessman>().SetXBoard(matrixX);
        reference.GetComponent<Chessman>().SetYBoard(matrixY);
        reference.GetComponent<Chessman>().SetCoords();

        // Mise à jour du plateau
        controller.GetComponent<Game>().SetPosition(reference);

        // Enregistrement du coup dans l'historique
        string piece = reference.name;
        string move = $"{piece} : {fromX},{fromY} → {matrixX},{matrixY}";

        // Vérification si une pièce est détruite
        if (attack)
        {
            string destroyedPiece = controller.GetComponent<Game>().GetPosition(matrixX, matrixY).name;
            move = $"{destroyedPiece} : {fromX},{fromY} → {matrixX},{matrixY}  <color=red>MANGE</color>";
        }

        historiqueManager.AjouterCoup(move);

        // Changement de joueur
        controller.GetComponent<Game>().NextTurn();

        // Suppression des MovePlates
        reference.GetComponent<Chessman>().DestroyMovePlates();
    }

    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    public void SetReference(GameObject obj)
    {
        reference = obj;
    }

    public GameObject GetReference()
    {
        return reference;
    }
}
