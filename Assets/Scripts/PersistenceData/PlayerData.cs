using Assets.Scripts.Locations;
using Assets.Scripts.Settings;
using UnityEngine;

namespace Assets.Scripts.PersistenceData
{
    [System.Serializable]
    public class PlayerData
    {
        #region Constructors

        public PlayerData() { }

        public PlayerData(Location location, Vector3 position)
        {
            Location = location;
            Position = position;

            SceneCoords = new Vector2Int(Mathf.FloorToInt(position.x / GameSettings.TERRAIN_WIDTH),
                                         Mathf.FloorToInt(position.z / GameSettings.TERRAIN_LENGTH));
        }

        #endregion

        #region Properties

        private Location location;
        private Vector3 position;
        private Vector2Int sceneCoords;

        public Location Location 
        {
            get => location;
            set => location = value;
        }

        public Vector3 Position
        {
            get => position;
            set
            {
                if (position != value)
                {
                    position = value;
                    SceneCoords = new Vector2Int(Mathf.FloorToInt(position.x / GameSettings.TERRAIN_WIDTH),
                                                 Mathf.FloorToInt(position.z / GameSettings.TERRAIN_LENGTH));
                }
            }
        }

        public Vector2Int SceneCoords
        {
            get => sceneCoords;
            private set => sceneCoords = value;
        }

        #endregion
    }
}