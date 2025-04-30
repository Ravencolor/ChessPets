using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HistoriqueManager : MonoBehaviour
{
    public TextMeshProUGUI historiqueText;  // Le texte où on affiche l'historique
    private List<string> historique = new List<string>(); // Liste des coups joués

    public void AjouterCoup(string coup)
    {
        historique.Add(coup); // Ajoute le coup dans la liste
        MettreAJourAffichage(); // Met à jour l'affichage
    }

    private void MettreAJourAffichage()
    {
        historiqueText.text = string.Join("\n", historique); // Affiche tous les coups
    }
}
