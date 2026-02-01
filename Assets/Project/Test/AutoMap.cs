using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "World/Auto Tile Set", fileName = "AutoTileSet")]
public class AutoMap : ScriptableObject
{
    [Tooltip("Index = mask (0–15)")]
    public TileBase[] tiles = new TileBase[16];

    public TileBase GetTile(int mask)
    {
        if (mask < 0 || mask >= tiles.Length)
            return null;

        return tiles[mask];
    }
}
