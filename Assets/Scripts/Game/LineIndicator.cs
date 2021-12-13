using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineIndicator : MonoBehaviour
{
    public int[,] lineData = new int[10, 10]
    {
        {0,  1,  2,  3,  4,  5,  6,  7,  8,  9},
        {10, 11, 12, 13, 14, 15, 16, 17, 18, 19 },
        {20, 21, 22, 23, 24, 25, 26, 27, 28, 29 },
        {30, 31, 32, 33, 34, 35, 36, 37, 38, 39 },
        {40, 41, 42, 43, 44, 45, 46, 47, 48, 49 },
        {50, 51, 52, 53, 54, 55, 56, 57, 58, 59 },
        {60, 61, 62, 63, 64, 65, 66, 67, 68, 69 },
        {70, 71, 72, 73, 74, 75, 76, 77, 78, 79 },
        {80, 81, 82, 83, 84, 85, 86, 87, 88, 89 },
        {90, 91, 92, 93, 94, 95, 96, 97, 98, 99 },

    };

    [HideInInspector] public int[] columnIndexes = new int[10]
    {
        0,1,2,3,4,5,6,7,8,9
    };
    private(int,int) GetSquarePos(int cellIndex)
    {
        int row =-1;
        int col =-1;
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (lineData[i, j] == cellIndex)
                {
                    row = i;
                    col = j;
                }
            }
        }
        return (row, col);
    }
    public int[] GetVerticalLine(int cell)
    {
        int[] line = new int[10];
        var pos = GetSquarePos(cell).Item2;
        for (int i = 0; i < 10; i++)
        {
            line[i] = lineData[i, pos];
        }
        return line;
    }
    public int LineIndex(int cell)
    {

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (lineData[i, j] == cell)
                    return i;
            }
        }

        return -1;
    }
}
