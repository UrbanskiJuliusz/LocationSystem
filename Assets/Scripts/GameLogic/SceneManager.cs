using Assets.Scripts.Locations;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Assets.Scripts.GameLogic
{
    public class SceneManager
    {
        #region Variables

        internal Dictionary<Vector2Int, AsyncOperationHandle<SceneInstance>> CurrentLoadedScenes { get; private set; } = new Dictionary<Vector2Int, AsyncOperationHandle<SceneInstance>>();
        private HashSet<Vector2Int> sceneCoordsInProcess = new HashSet<Vector2Int>();

        #endregion

        #region Public Methods

        public void LoadScene(Location location, Vector2Int sceneCoords)
        {
            if (CurrentLoadedScenes.ContainsKey(sceneCoords) || sceneCoordsInProcess.Contains(sceneCoords))
                return;

            try
            {
                var locationScenes = location.GetLocationScenes();
                SceneLocationsMap sceneLocationsMap = locationScenes.Find(x => x.coordinates.Equals(sceneCoords));

                if (sceneLocationsMap != null)
                {
                    var operationHandle = Addressables.LoadSceneAsync(sceneLocationsMap.scene, UnityEngine.SceneManagement.LoadSceneMode.Additive);
                    operationHandle.Completed += handle => SceneLoadCompleted(handle, sceneCoords);
                    sceneCoordsInProcess.Add(sceneCoords);
                }
                else
                {
                    Debug.LogError($"Scene with coordinates: {sceneCoords} doesn't exists at location: {location.name}");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Scene load exception - Coordinates: {sceneCoords} - {ex.Message}");
            }
        }

        public void UnloadScene(Vector2Int sceneCoords)
        {
            if (!CurrentLoadedScenes.ContainsKey(sceneCoords) || sceneCoordsInProcess.Contains(sceneCoords))
                return;

            try
            {
                AsyncOperationHandle<SceneInstance> sceneToUnload = CurrentLoadedScenes[sceneCoords];

                var operationHandle = Addressables.UnloadSceneAsync(sceneToUnload, true);
                operationHandle.Completed += handle => SceneUnloadCompleted(handle, sceneCoords);
                sceneCoordsInProcess.Add(sceneCoords);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Scene unload exception - Coordinates: {sceneCoords} - {ex.Message}");
            }
        }

        #endregion

        #region Event callbacks

        private void SceneLoadCompleted(AsyncOperationHandle<SceneInstance> handle, Vector2Int sceneCoords)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                CurrentLoadedScenes[sceneCoords] = handle;
            }

            sceneCoordsInProcess.Remove(sceneCoords);
        }

        private void SceneUnloadCompleted(AsyncOperationHandle<SceneInstance> handle, Vector2Int sceneCoords)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                CurrentLoadedScenes.Remove(sceneCoords);
            }

            sceneCoordsInProcess.Remove(sceneCoords);
        }

        #endregion
    }
}