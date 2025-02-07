using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallTileMap : MonoBehaviour
{
    [SerializeField] private Tilemap bgTileMap;

    public bool CheckEmpty(Vector3Int pos)
    {
        return !bgTileMap.HasTile(pos);
    }
}
