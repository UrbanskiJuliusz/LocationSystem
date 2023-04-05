using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Assets.Scripts.Locations
{
    [CreateAssetMenu(fileName = "Location", menuName = "Locations/New Location")]
    public class Location : ScriptableObject
    {
        [SerializeField] private LocationName LocationName;
        [SerializeField] private List<SceneLocationsMap> LocationScenes;

        public List<SceneLocationsMap> GetLocationScenes() => LocationScenes;


#if UNITY_EDITOR

        [ContextMenu(nameof(LoadScenes))]
        public void LoadScenes()
        {
            LocationScenes = new List<SceneLocationsMap>();
            Addressables.LoadResourceLocationsAsync("locations").Completed += SetLocationScenes;
        }

        private void SetLocationScenes(AsyncOperationHandle<IList<IResourceLocation>> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded && obj.Result != null && obj.Result.Count > 0)
            {
                foreach (var resourceLocation in obj.Result)
                {
                    if (resourceLocation.ToString().ToLower().Contains(LocationName.ToString().ToLower()))
                    {
                        GUID guid = AssetDatabase.GUIDFromAssetPath(resourceLocation.ToString());
                        string[] pathSplit = resourceLocation.ToString().ToLower().Split('/');
                        string xData = pathSplit[^1].Split('-')[0].Replace("x_", "").Replace(".unity", "").Trim();
                        string zData = pathSplit[^1].Split('-')[1].Replace("z_", "").Replace(".unity", "").Trim();

                        Vector2 coords = new Vector2(0, 0);

                        if (int.TryParse(xData, out int x))
                            coords.x = x;
                        else
                            Debug.LogError($"You have to correct scene name format - {pathSplit[^1]}");

                        if (int.TryParse(zData, out int z))
                            coords.y = z;
                        else
                            Debug.LogError($"You have to correct scene name format - {pathSplit[^1]}");

                        SceneLocationsMap locationScenesMap = new SceneLocationsMap(coords, new AssetReference(guid.ToString()));

                        LocationScenes.Add(locationScenesMap);
                    }
                }
            }

            Addressables.Release(obj);
        }

#endif
    }
}