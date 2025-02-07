using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallonBlueTileMap : BallonTileMap
{
    [Header(" Ballon Red Tile ")]
    [SerializeField] private BallonRedTileMap ballonRedTileMap;

    private void Awake()
    {
        ballonAnimator = ballonFaceTransform.GetComponent<Animator>();
    }

    private void Start()
    {
        listBallonPos = new List<Vector2Int>();

        for (int i = Constant.BOTTOMLEFT.x; i <= Constant.TOPRIGHT.x; i++)
        {
            for (int j = Constant.BOTTOMLEFT.y; j <= Constant.TOPRIGHT.y; j++)
            {
                if (ballonTileMap.HasTile(new Vector3Int(i, j, 0)))
                {
                    listBallonPos.Add(new Vector2Int(i, j));
                }
            }
        }
    }

    public override void Move(Vector2Int direction)
    {
        ballonAnimator.SetTrigger(Constant.SUPRISETRIGGERANI);
        ballonFaceTransform.position += new Vector3(direction.x * 0.5f, direction.y * 0.5f, 0);

        xIncrease = 0;
        yIncrease = 0;

        for (int i = 0; i < listBallonPos.Count; i++)
        {
            Vector2Int pos = listBallonPos[i];
            ballonTileMap.SetTile(new Vector3Int(pos.x, pos.y, 0), null);
        }

        for (int i = 0; i < listBallonPos.Count; i++)
        {
            Vector2Int pos = listBallonPos[i];
            pos += direction;
            ballonTileMap.SetTile(new Vector3Int(pos.x, pos.y, 0), ballonTile);
            listBallonPos[i] = pos;
            checkTileMap.Tick(new Vector3Int(pos.x, pos.y, 0));
            airTankTileMap.CheckNear(pos, this);
        }
    }

    public override void Scale()
    {
        ballonAnimator.SetTrigger(Constant.SCALETRIGGERANI);
        airTankTileMap.TakeAirTank();

        List<Vector2Int> list = new List<Vector2Int>();

        for (int i = 0; i < listBallonPos.Count; i++)
        {
            Vector2Int pos = listBallonPos[i];
            int mul;
            if (xIncrease != 0)
            {
                pos.x += xIncrease > 0 ? 1 : -1;
                mul = xIncrease > 0 ? 1 : -1;
                if (!ballonTileMap.HasTile(new Vector3Int(pos.x, pos.y, 0)) && !ballonRedTileMap.ballonTileMap.HasTile(new Vector3Int(pos.x, pos.y, 0)))
                {
                    for (int j = 1; j <= Mathf.Abs(xIncrease); j++)
                    {
                        pos = listBallonPos[i];
                        pos.x += j * mul;
                        if (wallTileMap.CheckEmpty(new Vector3Int(pos.x, pos.y, 0)))
                        {
                            ballonTileMap.SetTile(new Vector3Int(pos.x, pos.y, 0), ballonTile);
                            //Debug.Log(pos);
                            list.Add(pos);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            if (yIncrease != 0)
            {
                pos = listBallonPos[i];
                pos.y += yIncrease > 0 ? 1 : -1;
                mul = yIncrease > 0 ? 1 : -1;
                if (!ballonTileMap.HasTile(new Vector3Int(pos.x, pos.y, 0)) && !ballonRedTileMap.ballonTileMap.HasTile(new Vector3Int(pos.x, pos.y, 0)))
                {
                    for (int j = 1; j <= Mathf.Abs(yIncrease); j++)
                    {
                        pos = listBallonPos[i];
                        pos.y += j * mul;
                        //Debug.Log(pos);
                        if (wallTileMap.CheckEmpty(new Vector3Int(pos.x, pos.y, 0)))
                        {
                            ballonTileMap.SetTile(new Vector3Int(pos.x, pos.y, 0), ballonTile);
                            //Debug.Log(pos);
                            list.Add(pos);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }

        listBallonPos.AddRange(list);
    }
}
