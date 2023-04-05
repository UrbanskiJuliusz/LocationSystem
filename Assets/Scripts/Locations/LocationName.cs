using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.Locations
{
    public enum LocationName
    {
        DarkForest,
        NorthIsland
    }

    [Serializable]
    public class SceneLocationsMap
    {
        public SceneLocationsMap(Vector2 coordinates, AssetReference scene)
        {
            this.coordinates = coordinates;
            this.scene = scene;
        }

        public Vector2 coordinates;
        public AssetReference scene;
    }
}