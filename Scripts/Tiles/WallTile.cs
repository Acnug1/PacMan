using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class WallTile : Tile {

    public Sprite Segment, End, Intersection;
    public Sprite m_Preview;

    public override void RefreshTile(Vector3Int location, ITilemap tilemap)
    {
        for (int yd = -1; yd <= 1; yd++)
            for (int xd = -1; xd <= 1; xd++)
            {
                Vector3Int position = new Vector3Int(location.x + xd, location.y + yd, location.z);
                if (HasWallTile(tilemap, position))
                    tilemap.RefreshTile(position);
            }
    }

    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        Vector3Int direction = Vector3Int.zero;

        for (int yd = -1; yd <= 1; yd++)
            for (int xd = -1; xd <= 1; xd++)
            {
                if (yd != 0 && xd != 0) continue;
                if (yd == 0 && xd == 0) continue;

                Vector3Int position = new Vector3Int(location.x + xd, location.y + yd, location.z);
                if (HasWallTile(tilemap, position))
                {
                    direction = new Vector3Int(xd, yd, 0);
                }
            }

        if (IsIntersection(tilemap, location))
        {
            tileData.sprite = Intersection;
        }
        else
        {
            tileData.sprite = IsEnd(tilemap, location, direction) ? End : Segment;
        }

        tileData.color = Color.white;
        var m = tileData.transform;
        m.SetTRS(Vector3.zero, GetRotation(direction), Vector3.one);
        tileData.transform = m;
        tileData.flags = TileFlags.LockTransform;
        tileData.colliderType = ColliderType.None;
    }

    private Quaternion GetRotation(Vector3Int direction)
    {
        var angel = Vector3.Angle(Vector3.right, direction);
        if (direction.y == -1) angel = 270;
        return Quaternion.Euler(0, 0, angel);
    }

    private bool IsEnd(ITilemap tilemap, Vector3Int pos, Vector3Int direction)
    {
        return !(HasWallTile(tilemap, pos + direction) && HasWallTile(tilemap, pos - direction));
    }

    private bool IsIntersection(ITilemap tilemap, Vector3Int pos)
    {
        return (HasWallTile(tilemap, pos + Vector3Int.right) || HasWallTile(tilemap, pos - Vector3Int.right)) &&
               (HasWallTile(tilemap, pos + Vector3Int.up) || HasWallTile(tilemap, pos - Vector3Int.up));
    }

    private bool HasWallTile(ITilemap tilemap, Vector3Int position)
    {
        return tilemap.GetTile(position) == this;
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/WallTile")]
    public static void CreateRoadTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Wall Tile", "New Wall Tile", "Asset", "Save Wall Tile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<WallTile>(), path);
    }
#endif
}
