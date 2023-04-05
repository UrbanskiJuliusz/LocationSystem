using Assets.Scripts.GameLogic;
using Assets.Scripts.Locations;
using Assets.Scripts.Settings;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class WorldPositionController : MonoBehaviour
    {
        #region Variables

        private const float MinCornerThreshold = 0.25f;
        private const float MaxCornerThreshold = 0.75f;

        #endregion

        #region Properties

        internal Location Location { get; set; }
        internal Vector2Int SceneCoords { get; private set; }
        internal int PosXOnCurrentScene { get; private set; }
        internal int PosZOnCurrentScene { get; private set; }
        internal float PosXOnCurrentSceneInPercent { get; private set; }
        internal float PosZOnCurrentSceneInPercent { get; private set; }

        #endregion

        #region Unity Methods

        private void Update()
        {
            if (transform.hasChanged)
            {
                if (Physics.Raycast(transform.position, Vector3.down * 100f, out RaycastHit hit, 100f, (int)GameLayers.Terrain))
                {
                    SceneCoords = new Vector2Int(Mathf.FloorToInt(transform.position.x / GameSettings.TERRAIN_WIDTH),
                                                 Mathf.FloorToInt(transform.position.z / GameSettings.TERRAIN_LENGTH));

                    PosXOnCurrentScene = Mathf.FloorToInt(transform.position.x - hit.transform.position.x);
                    PosZOnCurrentScene = Mathf.FloorToInt(transform.position.z - hit.transform.position.z);

                    PosXOnCurrentSceneInPercent = PosXOnCurrentScene / (float)GameSettings.TERRAIN_WIDTH;
                    PosZOnCurrentSceneInPercent = PosZOnCurrentScene / (float)GameSettings.TERRAIN_LENGTH;
                }

                transform.hasChanged = false;
            }
        }

        #endregion

        #region Public Methods

        public bool IsObjectInSceneCorner(float posXOnCurrentSceneInPercent, float posZOnCurrentSceneInPercent)
        {
            bool isXInCorner = posXOnCurrentSceneInPercent <= MinCornerThreshold || posXOnCurrentSceneInPercent >= MaxCornerThreshold;
            bool isZInCorner = posZOnCurrentSceneInPercent <= MinCornerThreshold || posZOnCurrentSceneInPercent >= MaxCornerThreshold;

            return isXInCorner && isZInCorner;
        }

        #endregion
    }
}