using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class ShowKeyboard : MonoBehaviour
{
    public TMPro.TMP_InputField inputField; // R�f�rence au champ de saisie o� vous voulez afficher le texte

    private string currentText = ""; // Texte actuellement saisi
    private bool isKeyboardActive = false; // Indique si le clavier est actif

    private void Update()
    {
        // V�rifiez si le bouton A du contr�leur Oculus est enfonc�
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            if (isKeyboardActive)
            {
                AddLetter("A");
            }
        }

        // Ajoutez d'autres boutons Oculus (B, X, Y, etc.) selon vos besoins
    }

    // Fonction pour activer/d�sactiver le clavier
    public void ToggleKeyboard()
    {
        isKeyboardActive = !isKeyboardActive;

        if (isKeyboardActive)
        {
            // Affichez le clavier virtuel ou effectuez d'autres actions n�cessaires
            // selon votre impl�mentation sp�cifique.
        }
        else
        {
            // Masquez le clavier virtuel ou effectuez d'autres actions n�cessaires.
        }
    }

    // Fonction pour ajouter une lettre au texte
    public void AddLetter(string letter)
    {
        currentText += letter;
        UpdateInputField();
    }

    // Fonction pour supprimer la derni�re lettre du texte
    public void DeleteLetter()
    {
        if (currentText.Length > 0)
        {
            currentText = currentText.Substring(0, currentText.Length - 1);
            UpdateInputField();
        }
    }

    // Fonction pour mettre � jour le champ de saisie
    private void UpdateInputField()
    {
        if (inputField != null)
        {
            inputField.text = currentText;
        }
    }
}
