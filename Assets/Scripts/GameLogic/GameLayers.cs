namespace Assets.Scripts.GameLogic
{
    public enum GameLayers
    {
        Default = 1 << 0,
        Water = 1 << 4,
        UI = 1 << 5,
        Terrain = 1 << 6
    }
}