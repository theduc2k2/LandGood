#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AutoTileToolWindow : EditorWindow
{
    AutoTileSet tileSet;
    Tilemap targetTilemap;

    const int GRID_SIZE = 4;
    const int CELL_SIZE = 64;

    [MenuItem("Tools/Auto Tile Tool")]
    static void Open()
    {
        GetWindow<AutoTileToolWindow>("Auto Tile Tool");
    }

    void OnGUI()
    {
        GUILayout.Label("AUTO TILE MAP BUILDER", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        tileSet = (AutoTileSet)EditorGUILayout.ObjectField(
            "Auto Tile Set",
            tileSet,
            typeof(AutoTileSet),
            false
        );

        targetTilemap = (Tilemap)EditorGUILayout.ObjectField(
            "Target Tilemap",
            targetTilemap,
            typeof(Tilemap),
            true
        );

        EditorGUILayout.Space();

        if (tileSet == null)
        {
            EditorGUILayout.HelpBox("Assign AutoTileSet asset", MessageType.Warning);
            return;
        }

        DrawTileGrid();

        EditorGUILayout.Space();

        if (GUILayout.Button("Apply Auto-Tile To Tilemap", GUILayout.Height(40)))
        {
            if (targetTilemap != null)
                ApplyAutoTile();
            else
                Debug.LogWarning("No Tilemap assigned");
        }
    }

    void DrawTileGrid()
    {
        GUILayout.Label("Tile Mask Grid (0–15)", EditorStyles.boldLabel);

        for (int y = 0; y < GRID_SIZE; y++)
        {
            EditorGUILayout.BeginHorizontal();

            for (int x = 0; x < GRID_SIZE; x++)
            {
                int mask = y * GRID_SIZE + x;

                TileBase current = tileSet.GetTile(mask);

                EditorGUI.BeginChangeCheck();
                TileBase tile = (TileBase)EditorGUILayout.ObjectField(
                    current,
                    typeof(TileBase),
                    false,
                    GUILayout.Width(CELL_SIZE),
                    GUILayout.Height(CELL_SIZE)
                );

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(tileSet, "Set Auto Tile");
                    tileSet.SetTile(mask, tile);
                    EditorUtility.SetDirty(tileSet);
                }
            }

            EditorGUILayout.EndHorizontal();
        }
    }

    void ApplyAutoTile()
    {
        BoundsInt bounds = targetTilemap.cellBounds;

        foreach (var pos in bounds.allPositionsWithin)
        {
            TileBase existing = targetTilemap.GetTile(pos);
            if (existing == null)
                continue;

            int mask = CalculateMask(pos);
            TileBase tile = tileSet.GetTile(mask);

            if (tile != null)
                targetTilemap.SetTile(pos, tile);
        }

        Debug.Log("Auto-tile applied to Tilemap");
    }

    int CalculateMask(Vector3Int pos)
    {
        int mask = 0;

        if (HasTile(pos + Vector3Int.up)) mask |= 1;
        if (HasTile(pos + Vector3Int.right)) mask |= 2;
        if (HasTile(pos + Vector3Int.down)) mask |= 4;
        if (HasTile(pos + Vector3Int.left)) mask |= 8;

        return mask;
    }

    bool HasTile(Vector3Int pos)
    {
        return targetTilemap.GetTile(pos) != null;
    }
}
#endif
