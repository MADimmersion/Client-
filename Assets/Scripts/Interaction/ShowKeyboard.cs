using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class ShowKeyboard : MonoBehaviour
{
    public TMPro.TMP_InputField inputField; // Référence au champ de saisie où vous voulez afficher le texte

    private string currentText = ""; // Texte actuellement saisi
    private bool isKeyboardActive = false; // Indique si le clavier est actif

    private void Update()
    {
        // Vérifiez si le bouton A du contrôleur Oculus est enfoncé
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            if (isKeyboardActive)
            {
                AddLetter("A");
            }
        }

        // Ajoutez d'autres boutons Oculus (B, X, Y, etc.) selon vos besoins
    }

    // Fonction pour activer/désactiver le clavier
    public void ToggleKeyboard()
    {
        isKeyboardActive = !isKeyboardActive;

        if (isKeyboardActive)
        {
            // Affichez le clavier virtuel ou effectuez d'autres actions nécessaires
            // selon votre implémentation spécifique.
        }
        else
        {
            // Masquez le clavier virtuel ou effectuez d'autres actions nécessaires.
        }
    }

    // Fonction pour ajouter une lettre au texte
    public void AddLetter(string letter)
    {
        currentText += letter;
        UpdateInputField();
    }

    // Fonction pour supprimer la dernière lettre du texte
    public void DeleteLetter()
    {
        if (currentText.Length > 0)
        {
            currentText = currentText.Substring(0, currentText.Length - 1);
            UpdateInputField();
        }
    }

    // Fonction pour mettre à jour le champ de saisie
    private void UpdateInputField()
    {
        if (inputField != null)
        {
            inputField.text = currentText;
        }
    }
}
