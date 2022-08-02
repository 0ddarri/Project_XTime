using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    //public Tilemap Tile;
    public List<Tilemap> TilemapList = new List<Tilemap>();

    [SerializeField] List<TileData> TileDatas = new List<TileData>();

    Dictionary<TileBase, TileData> DataFromTiles;



    private void Awake()
    {
        DataFromTiles = new Dictionary<TileBase, TileData>();
        for(int i = 0; i < TileDatas.Count; i++)
        {
            for(int j = 0; j < TileDatas[i].Tiles.Length; j++)
            {
                DataFromTiles.Add(TileDatas[i].Tiles[j], TileDatas[i]);
            }
        }
    }

    public Vector3Int GetCellPos(Vector3 pos, int num)
    {
        Vector3Int cellPos = TilemapList[num].WorldToCell(pos);
        //Tile.SetTileFlags(cellPos, TileFlags.None);
        //Tile.SetColor(cellPos, Color.clear);
        return cellPos;
    }

    public bool GetMovable(Vector3Int tilePos, int num)
    {
        TileBase clickedTile = TilemapList[num].GetTile(tilePos);
        return DataFromTiles[clickedTile].MoveAble;
    }

    public Vector3 GetCellWorldPos(Vector3Int cellpos, int num)
    {
        Vector3 worldPos = TilemapList[num].CellToWorld(cellpos);
        return worldPos;
    }

    List<Vector3Int> GetNearMovableCellPos(Vector3 CurPos, int num)
    {
        List<Vector3Int> Movetilelist = new List<Vector3Int>();

        Vector3Int CurTilePos = GetCellPos(CurPos, num);

        Vector3Int[] CheckTilePos = new Vector3Int[4];
        CheckTilePos[0] = CurTilePos + Vector3Int.up;
        CheckTilePos[1] = CurTilePos + Vector3Int.left;
        CheckTilePos[2] = CurTilePos + Vector3Int.right;
        CheckTilePos[3] = CurTilePos + Vector3Int.down;

        //for(int i = 0; i < 4; i++)
        //{
        //    Tile.SetTileFlags(CheckTilePos[i], TileFlags.None);
        //    Color color = Tile.GetColor(CheckTilePos[i]);
        //    color.r -= 0.1f;
        //    Tile.SetColor(CheckTilePos[i], color);
        //}

        bool[] Movable = new bool[4];
        for(int i = 0; i < 4; i++)
        {
            if(TilemapList[num].HasTile(CheckTilePos[i]))
            {
                Movable[i] = GetMovable(CheckTilePos[i], num);
                if (Movable[i].Equals(true))
                    Movetilelist.Add(CheckTilePos[i]);
            }
        }

        return Movetilelist;
    }

    public List<Vector3> GetNearMovableWorldCellPos(Vector3 pos, int num)
    {
        List<Vector3> nearPos = new List<Vector3>();
        List<Vector3Int> nearIntPos = GetNearMovableCellPos(pos, num);

        for(int i = 0; i < nearIntPos.Count; i++)
            nearPos.Add(GetCellWorldPos(nearIntPos[i], num));
        
        return nearPos;
    }
}
