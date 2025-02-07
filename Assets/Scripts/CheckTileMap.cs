using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CheckTileMap : MonoBehaviour
{
    [SerializeField] private Tilemap checkTileMap;
    [SerializeField] private Tile yesTileOff;
    [SerializeField] private Tile noTileOff;
    [SerializeField] private Tile yesTileOn;
    [SerializeField] private Tile noTileOn;

    private List<Vector3Int> yesTileList;
    private List<Vector3Int> noTileList;

    private int checkYesCount = 0;
    private int checkNoCount = 0;

    private void Start()
    {
        yesTileList = new List<Vector3Int>();
        noTileList = new List<Vector3Int>();

        for (int i = Constant.BOTTOMLEFT.x; i <= Constant.TOPRIGHT.x; i++)
        {
            for (int j = Constant.BOTTOMLEFT.y; j <= Constant.TOPRIGHT.y; j++)
            {
                if (checkTileMap.HasTile(new Vector3Int(i, j, 0)))
                {
                    if (checkTileMap.GetTile(new Vector3Int(i, j, 0)).name.StartsWith(Constant.YESTILENAME))
                    {
                        yesTileList.Add(new Vector3Int(i, j, 0));
                    }
                    else
                    {
                        noTileList.Add(new Vector3Int(i, j, 0));
                    }
                }
            }
        }
    }

    public void ResetTick()
    {
        for (int i = 0; i < yesTileList.Count; i++)
        {
            checkTileMap.SetTile(yesTileList[i], yesTileOff);
        }

        for (int i = 0; i < noTileList.Count; i++)
        {
            checkTileMap.SetTile(noTileList[i], noTileOff);
        }

        checkYesCount = 0;
        checkNoCount = 0;
    }

    public void Tick(Vector3Int pos)
    {
        if (checkTileMap.HasTile(pos))
        {
            if (checkTileMap.GetTile(pos).name.StartsWith(Constant.YESTILENAME))
            {
                checkTileMap.SetTile(pos, yesTileOn);
                checkYesCount++;
            }
            else
            {
                checkTileMap.SetTile(pos, noTileOn);
                checkNoCount++;
            }
        }
    }

    public bool isComplete()
    {
        return checkYesCount == yesTileList.Count && checkNoCount == 0;
    }
}
