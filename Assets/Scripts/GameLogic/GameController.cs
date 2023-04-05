using Assets.Scripts.Locations;
using Assets.Scripts.PersistenceData;
using Assets.Scripts.Player;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GameLogic
{
    public class GameController : MonoBehaviour
    {
        #region Variables

        [field: Header("Player Data")]
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Location playerLocation;
        [SerializeField] private Vector3 playerPosition;

        [field: Header("Controllers")]
        [SerializeField] private PlayerSceneController playerSceneController;

        #endregion

        #region Unity Methods

        private void Start()
        {
            _ = StartGame();
        }

        #endregion

        #region Private Methods

        private async Task StartGame()
        {
            /* ... Load Saved Player Data ... */

            PlayerData playerData = new PlayerData(playerLocation, playerPosition);

            await LoadInitialScene(playerData);

            SpawnPlayer(playerData);
        }

        private async Task<bool> LoadInitialScene(PlayerData playerData)
        {
            playerSceneController.sceneManager = new SceneManager();
            playerSceneController.sceneManager.LoadScene(playerData.Location, playerData.SceneCoords);

            while (playerSceneController.sceneManager.CurrentLoadedScenes.Count <= 0)
                await Task.Yield();

            return true;
        }

        private void SpawnPlayer(PlayerData playerData)
        {
            GameObject player = Instantiate(playerPrefab);
            player.transform.position = playerData.Position;

            WorldPositionController worldPositionController = player.GetComponent<WorldPositionController>();

            worldPositionController.Location = playerData.Location;
            playerSceneController.PlayerWorldPositionController = worldPositionController;
        }

        #endregion
    }
}