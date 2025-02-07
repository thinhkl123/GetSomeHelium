using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class BallonTileMap : MonoBehaviour
{
    public int xIncrease = 0;
    public int yIncrease = 0;

    [Header(" Other TileMap ")]
    [SerializeField] protected WallTileMap wallTileMap;
    [SerializeField] protected CheckTileMap checkTileMap;
    [SerializeField] protected AirTankTileMap airTankTileMap;

    [Header(" Info ")]
    public Tilemap ballonTileMap;
    [SerializeField] protected Tile ballonTile;

    [Header("Ballon Face")]
    [SerializeField] protected Transform ballonFaceTransform;
    protected Animator ballonAnimator;

    protected List<Vector2Int> listBallonPos;

    public abstract void Scale();

    public abstract void Move(Vector2Int direction);

    public bool CheckEmptyWall(Vector2Int direction)
    {
        bool isEmpty = true;

        for (int i = 0; i < listBallonPos.Count; i++)
        {
            Vector2Int pos = listBallonPos[i];
            pos += direction;
            if (!wallTileMap.CheckEmpty(new Vector3Int(pos.x, pos.y, 0)))
            {
                isEmpty = false;
                break;
            }
        }

        return isEmpty;
    }

    public void ShowPopUp()
    {
        for (int i = 0; i < listBallonPos.Count; i++)
        {
            Vector2Int pos = listBallonPos[i];
            AnimateBallonTileScale(new Vector3Int(pos.x, pos.y, 0));
        }
    }

    private void AnimateBallonTileScale(Vector3Int tilePosition)
    {
        // Initial and target scale
        Vector3 initialScale = new Vector3(0.8f, 0.8f, 1f);
        Vector3 targetScale = new Vector3(1f, 1f, 1f);

        // Get the original transform matrix of the tile
        Matrix4x4 originalMatrix = ballonTileMap.GetTransformMatrix(tilePosition);

        // Animate the scaling using DOTween
        DOTween.To(
            () => initialScale,
            scale =>
            {
                Matrix4x4 animatedMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, scale);
                ballonTileMap.SetTransformMatrix(tilePosition, animatedMatrix);
            },
            targetScale,
            0.2f // Animation duration
        ).SetEase(Ease.OutQuad);
    }

    public void SetTile()
    {
        xIncrease = 0;
        yIncrease = 0;

        for (int i = 0; i < listBallonPos.Count; i++)
        {
            Vector2Int pos = listBallonPos[i];
            checkTileMap.Tick(new Vector3Int(pos.x, pos.y, 0));
            airTankTileMap.CheckNear(pos, this);
        }
    }
}
