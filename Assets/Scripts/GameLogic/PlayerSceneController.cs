using Assets.Scripts.Locations;
using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.GameLogic
{
    public class PlayerSceneController : MonoBehaviour
    {
        #region Variables

        public SceneManager sceneManager;

        private WorldPositionController playerWorldPositionController;
        internal WorldPositionController PlayerWorldPositionController
        {
            get { return playerWorldPositionController; }
            set
            {
                if (playerWorldPositionController != value)
                {
                    playerWorldPositionController = value;
                    enabled = (value != null);
                }
            }
        }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            enabled = PlayerWorldPositionController != null;
            return;
        }

        private void Update()
        {
            CheckForSceneToLoad(PlayerWorldPositionController.Location,
                                PlayerWorldPositionController.SceneCoords,
                                PlayerWorldPositionController.PosXOnCurrentSceneInPercent,
                                PlayerWorldPositionController.PosZOnCurrentSceneInPercent);

            CheckForSceneToUnload(PlayerWorldPositionController.SceneCoords);
        }

        #endregion

        #region Private Methods

        private void CheckForSceneToLoad(Location location, Vector2Int playerCurrentSceneCoords, float playerPosXOnCurrentSceneInPercent, float playerPosZOnCurrentSceneInPercent)
        {
            if (PlayerWorldPositionController.IsObjectInSceneCorner(playerPosXOnCurrentSceneInPercent, playerPosZOnCurrentSceneInPercent))
            {
                int xCoordShift = (playerPosXOnCurrentSceneInPercent >= 0.75f) ? 1 : -1;
                int yCoordShift = (playerPosZOnCurrentSceneInPercent >= 0.75f) ? 1 : -1;

                Vector2Int coordsSceneToLoad = new Vector2Int(playerCurrentSceneCoords.x + xCoordShift, playerCurrentSceneCoords.y + yCoordShift);
                sceneManager.LoadScene(location, coordsSceneToLoad);
            }

            if (playerPosXOnCurrentSceneInPercent >= 0.75f || playerPosXOnCurrentSceneInPercent <= 0.25f)
            {
                int xCoordShift = (playerPosXOnCurrentSceneInPercent >= 0.75f) ? 1 : -1;

                Vector2Int coordsSceneToLoad = new Vector2Int(playerCurrentSceneCoords.x + xCoordShift, playerCurrentSceneCoords.y);
                sceneManager.LoadScene(location, coordsSceneToLoad);
            }

            if (playerPosZOnCurrentSceneInPercent >= 0.75f || playerPosZOnCurrentSceneInPercent <= 0.25f)
            {
                int yCoordShift = (playerPosZOnCurrentSceneInPercent >= 0.75f) ? 1 : -1;

                Vector2Int coordsSceneToLoad = new Vector2Int(playerCurrentSceneCoords.x, playerCurrentSceneCoords.y + yCoordShift);
                sceneManager.LoadScene(location, coordsSceneToLoad);
            }
        }

        private void CheckForSceneToUnload(Vector2Int playerCurrentSceneCoords)
        {
            var currentLoadedScenes = sceneManager.CurrentLoadedScenes;

            if (currentLoadedScenes != null && currentLoadedScenes.Count > 0)
            {
                foreach (Vector2Int sceneCoords in currentLoadedScenes.Keys)
                {
                    if (sceneCoords.x >= (playerCurrentSceneCoords.x + 2) || sceneCoords.x <= (playerCurrentSceneCoords.x - 2) ||
                        sceneCoords.y >= (playerCurrentSceneCoords.y + 2) || sceneCoords.y <= (playerCurrentSceneCoords.y - 2))
                    {
                        sceneManager.UnloadScene(sceneCoords);
                        break;
                    }
                }
            }
        }

        #endregion
    }
}