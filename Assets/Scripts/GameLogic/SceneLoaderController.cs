using Assets.Scripts.Locations;
using Assets.Scripts.Settings;
using UnityEngine;

namespace Assets.Scripts.GameLogic
{
    public class SceneLoaderController : MonoBehaviour
    {
        public static SceneLoaderController CreateComponentWithSceneManager(GameObject gameObject, SceneManager sceneManager = null)
        {
            SceneLoaderController sceneLoaderController = gameObject.AddComponent<SceneLoaderController>();
            sceneLoaderController.sceneManager = sceneManager ?? new SceneManager();

            return sceneLoaderController;
        }

        #region Variables

        private SceneManager sceneManager;

        //Import data from settings in Awake
        private float lowThresholdToSceneLoad;
        private float highThresholdToSceneLoad;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            lowThresholdToSceneLoad = GameSettings.LOW_THRESHOLD_TO_SCENE_LOAD;
            highThresholdToSceneLoad = GameSettings.HIGH_THRESHOLD_TO_SCENE_LOAD;
        }

        #endregion

        #region Public Methods

        public void CheckForSceneToLoad(Location location, Vector2Int playerCurrentSceneCoords, bool isObjectInSceneCorner,
                                        float playerPosXOnCurrentSceneInPercent, float playerPosZOnCurrentSceneInPercent)
        {
            if (isObjectInSceneCorner)
            {
                int xCoordShift = (playerPosXOnCurrentSceneInPercent >= highThresholdToSceneLoad) ? 1 : -1;
                int yCoordShift = (playerPosZOnCurrentSceneInPercent >= highThresholdToSceneLoad) ? 1 : -1;

                Vector2Int coordsSceneToLoad = new Vector2Int(playerCurrentSceneCoords.x + xCoordShift, playerCurrentSceneCoords.y + yCoordShift);
                sceneManager.LoadScene(location, coordsSceneToLoad);
            }

            if (playerPosXOnCurrentSceneInPercent >= highThresholdToSceneLoad || playerPosXOnCurrentSceneInPercent <= lowThresholdToSceneLoad)
            {
                int xCoordShift = (playerPosXOnCurrentSceneInPercent >= highThresholdToSceneLoad) ? 1 : -1;

                Vector2Int coordsSceneToLoad = new Vector2Int(playerCurrentSceneCoords.x + xCoordShift, playerCurrentSceneCoords.y);
                sceneManager.LoadScene(location, coordsSceneToLoad);
            }

            if (playerPosZOnCurrentSceneInPercent >= highThresholdToSceneLoad || playerPosZOnCurrentSceneInPercent <= lowThresholdToSceneLoad)
            {
                int yCoordShift = (playerPosZOnCurrentSceneInPercent >= highThresholdToSceneLoad) ? 1 : -1;

                Vector2Int coordsSceneToLoad = new Vector2Int(playerCurrentSceneCoords.x, playerCurrentSceneCoords.y + yCoordShift);
                sceneManager.LoadScene(location, coordsSceneToLoad);
            }
        }

        public void CheckForSceneToUnload(Vector2Int playerCurrentSceneCoords)
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