using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LauchGameButton : MonoBehaviour
{
    public void LaunchGame(string GameSceneName)
    {
        // Assurez-vous que vous avez une r�f�rence valide � un objet NetworkManager dans votre sc�ne
        NetworkManager networkManager = FindObjectOfType<NetworkManager>();

        if (networkManager != null)
        {
            Debug.LogError("c'est trouv�");
            // Utilisez l'objet NetworkManager pour acc�der � SceneManager
            UnityEngine.SceneManagement.SceneManager.LoadScene(GameSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
        else
        {
            // Si l'objet NetworkManager n'est pas trouv�, affichez un message d'erreur ou effectuez un autre traitement appropri�.
            Debug.LogError("NetworkManager not found in the scene.");
        }
    }
}