using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LauchGameButton : MonoBehaviour
{
    public void LaunchGame(string GameSceneName)
    {
        // Assurez-vous que vous avez une référence valide à un objet NetworkManager dans votre scène
        NetworkManager networkManager = FindObjectOfType<NetworkManager>();

        if (networkManager != null)
        {
            Debug.LogError("c'est trouvé");
            // Utilisez l'objet NetworkManager pour accéder à SceneManager
            UnityEngine.SceneManagement.SceneManager.LoadScene(GameSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
        else
        {
            // Si l'objet NetworkManager n'est pas trouvé, affichez un message d'erreur ou effectuez un autre traitement approprié.
            Debug.LogError("NetworkManager not found in the scene.");
        }
    }
}