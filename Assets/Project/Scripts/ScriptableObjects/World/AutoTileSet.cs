using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "World/Auto Tile Set")]
public class AutoTileSet : ScriptableObject
{
    [Tooltip("Index = mask (0–15)")]
    public TileBase[] tiles = new TileBase[16];

    public TileBase GetTile(int mask)
    {
        if (mask < 0 || mask >= tiles.Length)
            return null;

        return tiles[mask];
    }

    // ✅ THÊM HÀM NÀY
    public void SetTile(int mask, TileBase tile)
    {
        if (mask < 0 || mask >= tiles.Length)
            return;

        tiles[mask] = tile;
    }
}
