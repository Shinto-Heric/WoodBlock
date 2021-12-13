using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName ="BlockData")]
[CreateAssetMenu]
[System.Serializable]
public class BlockData : ScriptableObject
{
    [System.Serializable]
    public class Row
    {
        public bool[] column;
        private int rowSize;
        Row() { }
        public Row(int size)
        {
            CreateRow(size);
        }

        public void CreateRow(int size)
        {
            rowSize = size;
            column = new bool[rowSize];
            ClearRow();
        }
        public void ClearRow()
        {
            for(int i = 0; i<rowSize;i++)
            {
                column[i] = false;
            }
        }
    }
    public int columns = 0;
    public int rows = 0;
    public Row[] board;

    public void Clear()
    {
        for (int i = 0; i < rows; i++)
        {
            board[i].ClearRow();
        }
    }
    public void CreateBoard()
    {
        board = new Row[rows];

        for(var i =0;i<rows;i++)
        {
            board[i] = new Row(columns);
        }
    }
}
