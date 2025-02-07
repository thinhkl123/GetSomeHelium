using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AirTankTileMap : MonoBehaviour
{
    [SerializeField] private Tilemap airTankTileMap;
    [SerializeField] private Tilemap airTankOnTileMap;
    [SerializeField] private Tile airTankOnTile;

    List<Vector3Int> airTankTileList;
    List<Vector3Int> airTankTakeList = new List<Vector3Int>();

    private void Start()
    {
        airTankTileList = new List<Vector3Int>();

        for (int i = Constant.BOTTOMLEFT.x; i <= Constant.TOPRIGHT.x; i++)
        {
            for (int j = Constant.BOTTOMLEFT.y; j <= Constant.TOPRIGHT.y; j++)
            {
                if (airTankTileMap.HasTile(new Vector3Int(i, j, 0)))
                {
                    airTankTileList.Add(new Vector3Int(i, j, 0));
                }
            }
        }
    }

    public void ResetTick()
    {
        for (int i=0; i < airTankTileList.Count; ++i)
        {
            airTankOnTileMap.SetTile(airTankTileList[i], null);
        }

        airTankTakeList.Clear(); 
    }

    public void CheckNear(Vector2Int pos, BallonTileMap ballonTileMap)
    {
        Vector3Int newpos = new Vector3Int(pos.x + 1, pos.y, 0);
        CheckTick(newpos, ballonTileMap);
        newpos = new Vector3Int(pos.x - 1, pos.y, 0);
        CheckTick(newpos, ballonTileMap);
        newpos = new Vector3Int(pos.x, pos.y + 1, 0);
        CheckTick(newpos, ballonTileMap);
        newpos = new Vector3Int(pos.x, pos.y - 1, 0);
        CheckTick(newpos, ballonTileMap);
    }

    private void CheckTick(Vector3Int pos, BallonTileMap ballonTileMap)
    {
        if (airTankTileMap.HasTile(pos))
        {
            airTankOnTileMap.SetTile(pos, airTankOnTile);
            if (airTankTileMap.GetTransformMatrix(pos).rotation.eulerAngles.z == 270)
            {
                ballonTileMap.yIncrease -= 1;
            }
            else if (airTankTileMap.GetTransformMatrix(pos).rotation.eulerAngles.z == 0)
            {
                ballonTileMap.xIncrease += 1;
            }
            else if (airTankTileMap.GetTransformMatrix(pos).rotation.eulerAngles.z == 90)
            {
                ballonTileMap.yIncrease += 1;
            }
            else
            {
                ballonTileMap.xIncrease -= 1;
            }

            airTankTakeList.Add(pos);
        }
    }

    public void TakeAirTank()
    {
        for (int i = 0; i < airTankTakeList.Count; i++)
        {
            AnimateAirTankTileScale(airTankTakeList[i]);
        }
    }

    private void AnimateAirTankTileScale(Vector3Int tilePosition)
    {
        // Initial and target scale
        Vector3 initialScale = new Vector3(1f, 0.6f, 1f);
        Vector3 targetScale = new Vector3(1f, 1f, 1f);

        // Get the original transform matrix of the tile
        Matrix4x4 originalMatrix = airTankTileMap.GetTransformMatrix(tilePosition);
        Matrix4x4 originalMatrixOn = airTankOnTileMap.GetTransformMatrix(tilePosition);

        // Extract the current rotation from the matrix
        Quaternion originalRotation = originalMatrix.rotation;

        // Animate the scaling using DOTween
        DOTween.To(
            () => initialScale,
            scale =>
            {
                Matrix4x4 animatedMatrix = Matrix4x4.TRS(Vector3.zero, originalRotation, scale);
                airTankTileMap.SetTransformMatrix(tilePosition, animatedMatrix);

                Matrix4x4 animatedMatrixOn = Matrix4x4.TRS(Vector3.zero, originalRotation, scale);
                airTankOnTileMap.SetTransformMatrix(tilePosition, animatedMatrix);
            },
            targetScale,
            0.2f // Animation duration
        ).SetEase(Ease.OutQuad);
    }
}
