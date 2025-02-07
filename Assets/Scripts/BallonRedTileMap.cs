using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public class BallonRedTileMap : BallonTileMap
{
    [Header(" Ballon Blue Tile ")]
    [SerializeField] private BallonBlueTileMap ballonBlueTileMap;

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

    private void Update()
    {
        //if (GameManager.Instance != null)
        {
            if (GameManager.Instance.isWin && SceneController.instance.isLoadingScene)
            {
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Move(new Vector2Int(-1, 0));
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Move(new Vector2Int(1, 0));
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            Move(new Vector2Int(0, 1));
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Move(new Vector2Int(0, -1));
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Scale();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            SceneController.instance.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private bool CheckEmptyBallonBlueTile(Vector3Int pos)
    {
        if (ballonBlueTileMap != null)
        {
            if (ballonBlueTileMap.ballonTileMap.HasTile(pos))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return true;
        }
    }

    public override void Scale()
    {
        if (SoundManager.instance != null)
            SoundManager.instance.Play("Scale");

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
                if (!ballonTileMap.HasTile(new Vector3Int(pos.x, pos.y, 0)) 
                    && CheckEmptyBallonBlueTile(new Vector3Int(pos.x, pos.y, 0))
                )
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
                if (!ballonTileMap.HasTile(new Vector3Int(pos.x, pos.y, 0)) 
                    && CheckEmptyBallonBlueTile(new Vector3Int(pos.x, pos.y, 0))
                )
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

        checkTileMap.ResetTick();
        airTankTileMap.ResetTick();

        xIncrease = 0;
        yIncrease = 0;

        if (ballonBlueTileMap != null)
            ballonBlueTileMap.Scale();

        SetTile();

        ShowPopUp();

        if (ballonBlueTileMap != null)
        {
            ballonBlueTileMap.SetTile();

            ballonBlueTileMap.ShowPopUp();
        }


        if (checkTileMap.isComplete())
        {
            SoundManager.instance.Play("Win");
            GameManager.Instance.isWin = true;
            StartCoroutine(LoadNextLevel(0.3f));
        }
    }

    public override void Move(Vector2Int direction)
    {
        if (!CheckEmptyWall(direction))
        {
            return;
        }

        bool isBlueMove = false;

        if (ballonBlueTileMap != null)
        {
            if (!CheckEmptyBallonBlueTile(direction))
            {
                if (!ballonBlueTileMap.CheckEmptyWall(direction))
                {
                    return;
                }
                else
                {
                    isBlueMove = true;
                }
            }
        }

        if (SoundManager.instance != null)
            SoundManager.instance.Play("Move");

        ballonAnimator.SetTrigger(Constant.SUPRISETRIGGERANI);

        checkTileMap.ResetTick();
        airTankTileMap.ResetTick();

        xIncrease = 0;
        yIncrease = 0;

        if (ballonBlueTileMap != null)
        {
            if (isBlueMove)
                ballonBlueTileMap.Move(direction);
            else
                ballonBlueTileMap.SetTile();
        }

        ballonFaceTransform.position += new Vector3(direction.x * 0.5f, direction.y * 0.5f, 0);

        for (int i = 0; i < listBallonPos.Count; i++)
        {
            Vector2Int pos = listBallonPos[i];
            ballonTileMap.SetTile(new Vector3Int(pos.x, pos.y, 0), null);
        }


        for (int i=0; i<listBallonPos.Count; i++)
        {
            Vector2Int pos = listBallonPos[i];
            pos += direction;
            ballonTileMap.SetTile(new Vector3Int(pos.x, pos.y, 0), ballonTile);
            listBallonPos[i] = pos;
            checkTileMap.Tick(new Vector3Int(pos.x, pos.y, 0));
            airTankTileMap.CheckNear(pos, this);
        }

        if (checkTileMap.isComplete())
        {
            SoundManager.instance.Play("Win");
            GameManager.Instance.isWin = true;
            StartCoroutine(LoadNextLevel(0.3f));
        }
    }

    private bool CheckEmptyBallonBlueTile(Vector2Int direction)
    {
        bool isEmpty = true;

        for (int i = 0; i < listBallonPos.Count; i++)
        {
            Vector2Int pos = listBallonPos[i];
            pos += direction;
            if (ballonBlueTileMap.ballonTileMap.HasTile(new Vector3Int(pos.x, pos.y, 0)))
            {
                isEmpty = false;
                break;
            }
        }

        return isEmpty;
    }

    IEnumerator LoadNextLevel(float time)
    {
        yield return new WaitForSeconds(time);
        SceneController.instance.LoadNextLevel();
    }
}
