using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class Menu : NetworkBehaviour
{
    [SerializeField] private UIDocument mainMenu;
    private VisualElement m_RootMenu;
    private Button m_launchButton, m_quitButton;

    private void Awake()
    {
        m_RootMenu = mainMenu.rootVisualElement;
        m_launchButton = m_RootMenu.Q<Button>("LaunchButton");
        m_quitButton = m_RootMenu.Q<Button>("QuitButton");
    }

    private void Start()
    {
        // Only allow the host to launch the game, and others to return to the main menu.
        //if (IsServer)
        //{
            m_launchButton.clicked += LaunchGame;
        //}
        //else
        //{
        //    m_launchButton.SetEnabled(false); // Disable the button for clients.
            m_quitButton.clicked += ReturnToMainMenu;
        //}
    }

    // This method should be executed on the server only.
    private void LaunchGame()
    {
        Debug.Log("Host clicked LaunchButton");

        // Trigger an RPC or network event to start the game on all clients.
        // Example: MyNetworkedGameController.StartGame();

        // If you have a game controller that starts the game, call it here.
        NetworkManager.Singleton.SceneManager.LoadScene("Tests", UnityEngine.SceneManagement.LoadSceneMode.Single);
        PartyManager.LaunchParty();
    }

    private void ReturnToMainMenu()
    {
        if (IsClient)
        {
            Debug.Log("Client clicked QuitButton");

            // Trigger an RPC or network event to return to the main menu on the server.
            // Example: MyNetworkedGameController.ReturnToMainMenu();

            // If you have a game controller that manages returning to the main menu, call it here.
            //MyNetworkedGameController.ReturnToMainMenu();
        }
    }
}
