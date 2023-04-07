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

            SceneManager sceneManager = new SceneManager();
            await LoadInitialScene(playerData, sceneManager);

            SpawnPlayer(playerData, ref sceneManager);
        }

        private async Task<bool> LoadInitialScene(PlayerData playerData, SceneManager sceneManager)
        {
            sceneManager.LoadScene(playerData.Location, playerData.SceneCoords);

            while (sceneManager.CurrentLoadedScenes.Count <= 0)
                await Task.Yield();

            return true;
        }

        private void SpawnPlayer(PlayerData playerData, ref SceneManager sceneManager)
        {
            GameObject player = Instantiate(playerPrefab);
            player.transform.position = playerData.Position;

            if (player.TryGetComponent(out WorldPositionController worldPositionController))
            {
                worldPositionController.Location = playerData.Location;
            }
            else
            {
                player.AddComponent<WorldPositionController>().Location = playerData.Location;
            }

            var sceneLoaderController = SceneLoaderController.CreateComponentWithSceneManager(player, sceneManager);
            worldPositionController.sceneLoaderController = sceneLoaderController;
        }

        #endregion
    }
}