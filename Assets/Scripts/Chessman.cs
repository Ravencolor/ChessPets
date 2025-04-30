using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chessman : MonoBehaviour
{
    // Références aux objets dans notre scène Unity
    public GameObject controller;
    public GameObject movePlate;

    // Position de cette pièce d'échecs sur le plateau
    // La position correcte sera définie plus tard
    private int xBoard = -1;
    private int yBoard = -1;

    // Variable pour suivre le joueur auquel appartient la pièce ("black" ou "white")
    private string player;

    // Références à tous les sprites possibles que cette pièce d'échecs pourrait avoir
    public Sprite black_cat, black_frog, black_crow, black_human, black_pig, black_dog;
    public Sprite white_cat, white_frog, white_crow, white_human, white_pig, white_dog;

    private void Start()
    {
        // Augmenter la taille du collider
        BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            collider.size = new Vector2(3.25f, 3.25f);
        }
        else
        {
            collider = gameObject.AddComponent<BoxCollider2D>();
            collider.size = new Vector2(3.25f, 3.25f);
        }
    }

    public void Activate()
    {
        // Obtenir le contrôleur de jeu
        controller = GameObject.FindGameObjectWithTag("GameController");

        // Ajuster la position de la pièce
        SetCoords();

        // Choisir le sprite correct en fonction du nom de la pièce
        switch (this.name)
        {
            case "black_cat": this.GetComponent<SpriteRenderer>().sprite = black_cat; player = "black"; break;
            case "black_frog": this.GetComponent<SpriteRenderer>().sprite = black_frog; player = "black"; break;
            case "black_crow": this.GetComponent<SpriteRenderer>().sprite = black_crow; player = "black"; break;
            case "black_human": this.GetComponent<SpriteRenderer>().sprite = black_human; player = "black"; break;
            case "black_pig": this.GetComponent<SpriteRenderer>().sprite = black_pig; player = "black"; break;
            case "black_dog": this.GetComponent<SpriteRenderer>().sprite = black_dog; player = "black"; break;
            case "white_cat": this.GetComponent<SpriteRenderer>().sprite = white_cat; player = "white"; break;
            case "white_frog": this.GetComponent<SpriteRenderer>().sprite = white_frog; player = "white"; break;
            case "white_crow": this.GetComponent<SpriteRenderer>().sprite = white_crow; player = "white"; break;
            case "white_human": this.GetComponent<SpriteRenderer>().sprite = white_human; player = "white"; break;
            case "white_pig": this.GetComponent<SpriteRenderer>().sprite = white_pig; player = "white"; break;
            case "white_dog": this.GetComponent<SpriteRenderer>().sprite = white_dog; player = "white"; break;
        }
    }

    public void SetCoords()
    {
        // Convertir les coordonnées du plateau en coordonnées Unity
        float x = xBoard;
        float y = yBoard;

        // Ajuster avec un décalage variable
        x *= 0.66f;
        y *= 0.66f;

        // Ajouter des constantes pour positionner correctement (position 0,0)
        x += -2.3f;
        y += -2.3f;

        // Définir les valeurs Unity réelles
        this.transform.position = new Vector3(x, y, -0.1f);
        this.transform.localScale = new Vector3(0.2f, 0.2f, -0.1f);
    }

    public int GetXBoard()
    {
        return xBoard;
    }

    public int GetYBoard()
    {
        return yBoard;
    }

    public void SetXBoard(int x)
    {
        xBoard = x;
    }

    public void SetYBoard(int y)
    {
        yBoard = y;
    }

    private void OnMouseUp()
    {
        // Vérifier si le jeu n'est pas terminé et si c'est le tour du joueur actuel
        if (!controller.GetComponent<Game>().IsGameOver() && controller.GetComponent<Game>().GetCurrentPlayer() == player)
        {
            // Supprimer toutes les MovePlates liées à la pièce précédemment sélectionnée
            DestroyMovePlates();

            // Créer de nouvelles MovePlates
            InitiateMovePlates();
        }
    }

    public void DestroyMovePlates()
    {
        // Détruire les anciennes MovePlates
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]); // Attention, la fonction "Destroy" est asynchrone
        }
    }

    public void InitiateMovePlates()
    {
        // Déterminer les mouvements possibles en fonction du type de pièce
        switch (this.name)
        {
            case "black_cat":
            case "white_cat":
                LimitedLineMovePlate(1, 0, 3);
                LimitedLineMovePlate(0, 1, 3);
                LimitedLineMovePlate(1, 1, 3);
                LimitedLineMovePlate(-1, 0, 3);
                LimitedLineMovePlate(0, -1, 3);
                LimitedLineMovePlate(-1, -1, 3);
                LimitedLineMovePlate(-1, 1, 3);
                LimitedLineMovePlate(1, -1, 3);
                break;
            case "black_frog":
            case "white_frog":
                LMovePlate();
                break;
            case "black_crow":
            case "white_crow":
                LimitedLineMovePlate(1, 1, 2);
                LimitedLineMovePlate(1, -1, 2);
                LimitedLineMovePlate(-1, 1, 2);
                LimitedLineMovePlate(-1, -1, 2);
                break;
            case "black_human":
            case "white_human":
                SurroundMovePlate();
                break;
            case "black_pig":
            case "white_pig":
                PigMovePlate();
                break;
            case "black_dog":
                dogMovePlate(xBoard, yBoard - 1);
                break;
            case "white_dog":
                dogMovePlate(xBoard, yBoard + 1);
                break;
        }
    }

    public void LimitedLineMovePlate(int xIncrement, int yIncrement, int maxSteps)
    {
        Game sc = controller.GetComponent<Game>();

        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;
        int steps = 0;

        // Parcourir les cases dans une direction donnée jusqu'à un maximum de pas
        while (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) == null && steps < maxSteps)
        {
            MovePlateSdog(x, y);
            x += xIncrement;
            y += yIncrement;
            steps++;
        }

        // Vérifier si une pièce ennemie est atteinte
        if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) != null && sc.GetPosition(x, y).GetComponent<Chessman>().player != player && steps < maxSteps)
        {
            MovePlateAttackSdog(x, y);
        }
    }

    public void PigMovePlate()
    {
        // Définir les mouvements spécifiques pour le cochon
        PointMovePlate(xBoard, yBoard + 1);
        PointMovePlate(xBoard, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard + 1);
        PointMovePlate(xBoard - 1, yBoard + 1);
        PointMovePlate(xBoard + 1, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard - 1);
    }

    public void LineMovePlate(int xIncrement, int yIncrement)
    {
        Game sc = controller.GetComponent<Game>();

        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        // Parcourir les cases dans une direction donnée jusqu'à rencontrer un obstacle
        while (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) == null)
        {
            MovePlateSdog(x, y);
            x += xIncrement;
            y += yIncrement;
        }

        // Vérifier si une pièce ennemie est atteinte
        if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y).GetComponent<Chessman>().player != player)
        {
            MovePlateAttackSdog(x, y);
        }
    }

    public void LMovePlate()
    {
        // Définir les mouvements en "L" (comme un cavalier)
        PointMovePlate(xBoard + 1, yBoard + 2);
        PointMovePlate(xBoard - 1, yBoard + 2);
        PointMovePlate(xBoard + 2, yBoard + 1);
        PointMovePlate(xBoard + 2, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard - 2);
        PointMovePlate(xBoard - 1, yBoard - 2);
        PointMovePlate(xBoard - 2, yBoard + 1);
        PointMovePlate(xBoard - 2, yBoard - 1);
    }

    public void SurroundMovePlate()
    {
        // Définir les mouvements autour de la pièce (comme un roi)
        PointMovePlate(xBoard, yBoard + 1);
        PointMovePlate(xBoard, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard + 0);
        PointMovePlate(xBoard - 1, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard + 1);
        PointMovePlate(xBoard + 1, yBoard + 0);
        PointMovePlate(xBoard + 1, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard + 1);
    }

    public void PointMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            GameObject cp = sc.GetPosition(x, y);

            // Vérifier si la case est vide ou contient une pièce ennemie
            if (cp == null)
            {
                MovePlateSdog(x, y);
            }
            else if (cp.GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSdog(x, y);
            }
        }
    }

    public void dogMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            // Vérifier si la case est vide
            if (sc.GetPosition(x, y) == null)
            {
                MovePlateSdog(x, y);
            }

            // Vérifier les cases adjacentes pour des pièces ennemies
            if (sc.PositionOnBoard(x + 1, y) && sc.GetPosition(x + 1, y) != null && sc.GetPosition(x + 1, y).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSdog(x + 1, y);
            }

            if (sc.PositionOnBoard(x - 1, y) && sc.GetPosition(x - 1, y) != null && sc.GetPosition(x - 1, y).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSdog(x - 1, y);
            }
        }
    }

    public void MovePlateSdog(int matrixX, int matrixY)
    {
        // Convertir les coordonnées du plateau en coordonnées Unity
        float x = matrixX;
        float y = matrixY;

        // Ajuster avec un décalage variable
        x *= 0.66f;
        y *= 0.66f;

        // Ajouter des constantes pour positionner correctement (position 0,0)
        x += -2.3f;
        y += -2.3f;

        // Créer une MovePlate à la position spécifiée
        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }

    public void MovePlateAttackSdog(int matrixX, int matrixY)
    {
        // Convertir les coordonnées du plateau en coordonnées Unity
        float x = matrixX;
        float y = matrixY;

        // Ajuster avec un décalage variable
        x *= 0.66f;
        y *= 0.66f;

        // Ajouter des constantes pour positionner correctement (position 0,0)
        x += -2.3f;
        y += -2.3f;

        // Créer une MovePlate d'attaque à la position spécifiée
        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = true;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }
}