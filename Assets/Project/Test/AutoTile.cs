// using UnityEngine;
// using UnityEngine.Tilemaps;
// using System.Collections.Generic;

// public class AutoTile : MonoBehaviour
// {
//     [Header("References")]
//     [SerializeField] private Camera mainCamera;
//     [SerializeField] private Tilemap tilemap;

//     [Header("Auto Tile Data")]
//     [SerializeField] private AutoTileSet autoTileSet;

//     // ===== MAP DATA =====
//     private Dictionary<Vector2Int, bool> filledCells = new();

//     private static readonly Vector2Int[] Neighbors =
//     {
//         Vector2Int.up,
//         Vector2Int.right,
//         Vector2Int.down,
//         Vector2Int.left
//     };

//     // ================== RUNTIME INIT ==================
//     void Start()
//     {
//         BuildFromExistingTilemap();
//         RebuildAll();
//     }

//     void Update()
//     {
//         if (Input.GetMouseButtonDown(0))
//         {
//             Vector3 worldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
//             Vector3Int cell3D = tilemap.WorldToCell(worldPos);
//             Vector2Int cell = new(cell3D.x, cell3D.y);

//             ToggleCell(cell);
//         }
//     }

//     // ================= CORE =================

//     void BuildFromExistingTilemap()
//     {
//         filledCells.Clear();

//         BoundsInt bounds = tilemap.cellBounds;
//         TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

//         int index = 0;
//         for (int y = bounds.yMin; y < bounds.yMax; y++)
//         {
//             for (int x = bounds.xMin; x < bounds.xMax; x++)
//             {
//                 TileBase tile = allTiles[index++];
//                 if (tile != null)
//                 {
//                     filledCells[new Vector2Int(x, y)] = true;
//                 }
//             }
//         }
//     }

//     void ToggleCell(Vector2Int pos)
//     {
//         if (filledCells.ContainsKey(pos))
//             filledCells.Remove(pos);
//         else
//             filledCells[pos] = true;

//         RefreshAround(pos);
//     }

//     void RebuildAll()
//     {
//         foreach (var cell in filledCells.Keys)
//         {
//             RefreshCell(cell);
//         }
//     }

//     void RefreshAround(Vector2Int center)
//     {
//         RefreshCell(center);
//         foreach (var dir in Neighbors)
//         {
//             RefreshCell(center + dir);
//         }
//     }

//     void RefreshCell(Vector2Int pos)
//     {
//         if (!filledCells.ContainsKey(pos))
//         {
//             tilemap.SetTile((Vector3Int)pos, null);
//             return;
//         }

//         int mask = CalculateMask(pos);
//         TileBase tile = autoTileSet.GetTile(mask);
//         tilemap.SetTile((Vector3Int)pos, tile);
//     }

//     // ================= AUTO TILE =================

//     int CalculateMask(Vector2Int pos)
//     {
//         int mask = 0;
//         if (IsFilled(pos + Vector2Int.up)) mask |= 1;
//         if (IsFilled(pos + Vector2Int.right)) mask |= 2;
//         if (IsFilled(pos + Vector2Int.down)) mask |= 4;
//         if (IsFilled(pos + Vector2Int.left)) mask |= 8;
//         return mask;
//     }

//     bool IsFilled(Vector2Int pos)
//     {
//         return filledCells.ContainsKey(pos);
//     }

// #if UNITY_EDITOR
//     void OnDrawGizmos()
//     {
//         if (filledCells == null) return;

//         Gizmos.color = Color.yellow;
//         foreach (var cell in filledCells.Keys)
//         {
//             Gizmos.DrawWireCube(
//                 new Vector3(cell.x + 0.5f, cell.y + 0.5f, 0),
//                 Vector3.one
//             );
//         }
//     }
    
// #endif
// }
