using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public BlockStorage blockStorage;
    public int columns = 10;
    public int rows = 10;
    public GameObject square;

    public float squareOffset = 0.5f;
    public float squareScale = 0.5f;
    public float squareGap = 0.1f;
    public Vector2 startPos = new Vector2(0.0f, 0.0f);


    private Vector2 offset = new Vector2(0.0f, 0.0f);
    private List<GameObject> gridSquares = new List<GameObject>();
    private LineIndicator lineIndicator;
    private void OnEnable()
    {
        GameEvents.CheckIfBlockCanBePlaced += CheckIfBlockCanBePlaced;
    }
    private void OnDisable()
    {
        GameEvents.CheckIfBlockCanBePlaced -= CheckIfBlockCanBePlaced;
    }
    void Start()
    {
        lineIndicator = GetComponent<LineIndicator>();
        CreateGrids();
    }

    private void CreateGrids()
    {
        SpawnGrids();
        SetGridPositions();
    }

    private void SpawnGrids()
    {
        int squareIndex = 0;
        for(var row = 0; row < rows; row++)
        {
            for(var column = 0; column < columns; column++)
            {
                gridSquares.Add(Instantiate(square));
                gridSquares[gridSquares.Count - 1].transform.SetParent(this.transform);
                gridSquares[gridSquares.Count - 1].transform.localScale =new Vector3(squareScale, squareScale, squareScale);
                var gridsq = gridSquares[gridSquares.Count - 1].GetComponent<GridSquare>();
                gridsq.SetImage();
                gridsq.gridIndex = squareIndex++;
                 
            }
        }
    }

    private void SetGridPositions()
    {
        int columnNumber = 0;
        int rowNumber = 0;
        Vector2 squareGapNum = new Vector2(0.0f, 0.0f);

        var squareRect = gridSquares[0].GetComponent<RectTransform>();
        bool rowMoved = false;

        offset.x = squareRect.rect.width * squareRect.transform.localScale.x + squareOffset;
        offset.y = squareRect.rect.height * squareRect.transform.localScale.y + squareOffset;
        foreach (GameObject square in gridSquares)
        {
            if(columnNumber +1 > columns)
            {
                squareGapNum.x = 0;
                columnNumber = 0;
                rowNumber++;
                rowMoved = false;
            }
            var posXOffset = offset.x * columnNumber + (squareGapNum.x * squareGap);
            var posYOffset = offset.y * rowNumber + (squareGapNum.y * squareGap);

            if(columnNumber > 0)
            {
                squareGapNum.x++;
                posXOffset += squareGap;
            }

            if(rowNumber > 0 && rowMoved == false)
            {
                rowMoved = true;
                squareGapNum.y++;
                posYOffset += squareGap;
            }
            square.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPos.x + posXOffset, startPos.y + posYOffset);
            square.GetComponent<RectTransform>().localPosition = new Vector3(startPos.x + posXOffset, startPos.y + posYOffset,0.0f);
            columnNumber++;
        }
    }

    private void CheckIfBlockCanBePlaced()
    {
        var squareIndex = new List<int>();
        foreach(var grid in gridSquares)
        {
            var gridSq = grid.GetComponent<GridSquare>();
            if(gridSq.selected && !gridSq.gridLocked)
            {
                squareIndex.Add(gridSq.gridIndex);
                gridSq.selected = false;
                //gridSq.ActivateGrid();
            }
        }

        var currentBlock = blockStorage.GetCurrentSelectedBlock();
        if (currentBlock == null) return;

        if(currentBlock.totalSqaureNumber == squareIndex.Count)
        {
            foreach(var sq in squareIndex)
            {
                gridSquares[sq].GetComponent<GridSquare>().PlaceShapeOnBoard();
            }

            var shapeLeft = 0;
            foreach (var block in blockStorage.blocks)
            {
                if (block.IsOnStartPos() && block.IsAnyOfGridActive())
                    shapeLeft++;
            }

            if(shapeLeft == 0)
            {
                GameEvents.RequestNewBlock();
            }
            else
            {
                GameEvents.SetShapeInactive();
            }
            CheckIfCompleted();
        }
        else
        {
            GameEvents.MoveShapeToStartPosition();
        }
    }
    public void CheckIfCompleted()
    {
        List<int[]> lines = new List<int[]>();
        foreach(var column in lineIndicator.columnIndexes)
        {
            lines.Add(lineIndicator.GetVerticalLine(column));
        }
        
        for(int row = 0; row < 10; row++)
        {
            List<int> data = new List<int>(10);
            for(var index = 0; index < 10; index++)
            {
                data.Add(lineIndicator.lineData[row, index]);
            }
            lines.Add(data.ToArray());
        }
        var linesCompleted = CheckIfSquaresCompleted(lines);

        var totalScores = linesCompleted;
        GameEvents.AddScores(totalScores);
        CheckIfPlayerLost();
    }
    private int CheckIfSquaresCompleted(List<int[]> data)
    {
        List<int[]> completedLines = new List<int[]>();
        var linesCompleted = 0;
        foreach(var line in data)
        {
            var lineCompleted = true;
            foreach(var squareIndex in line)
            {
                var comp = gridSquares[squareIndex].GetComponent<GridSquare>();
                if(comp.gridLocked == false)
                {
                    lineCompleted = false;
                }
            }
            if(lineCompleted)
            {
                completedLines.Add(line);
            }
        }
        foreach (var line in completedLines)
        {
            var completed = false;
            foreach (var squareIndex in line)
            {
                var comp = gridSquares[squareIndex].GetComponent<GridSquare>();
                comp.Deactivate();
                completed = true;
            }
            foreach (var squareIndex in line)
            {
                var comp = gridSquares[squareIndex].GetComponent<GridSquare>();
                comp.ClearOccupied();
            }
            if (completed)
                linesCompleted++;
        }
        return linesCompleted;
    }

    private void CheckIfPlayerLost()
    {
        var validShapes = 0;
        for(var index =0;index<blockStorage.blocks.Count;index++)
        {
            var isShapeActive = blockStorage.blocks[index].IsAnyOfGridActive();
            if(CheckIfShapeCanBePlaced(blockStorage.blocks[index]) && isShapeActive)
            {
                blockStorage.blocks[index]?.ActivateBlock();
                validShapes++;
            }
        }
        if(validShapes == 0)
        {
            //GameEvents.GameOver(false);
            Debug.Log("GameOver");
        }
    }

    private bool CheckIfShapeCanBePlaced(Block currentBlock)
    {
        var currentBlockData = currentBlock.currentBlockData;
        var columns = currentBlockData.columns;
        var rows = currentBlockData.rows;

        List<int> filledUpSquares = new List<int>();
        var blockIndex = 0;
        for(var rowIndex = 0;rowIndex< rows;rowIndex++)
        {
            for(var columnIndex =0; columnIndex<columns;columnIndex++)
            {
                if(currentBlockData.board[rowIndex].column[columnIndex])
                {
                    filledUpSquares.Add(blockIndex);
                }
                blockIndex++;
            }
        }
        if(currentBlock.totalSqaureNumber != filledUpSquares.Count)
        {
            Debug.LogError("Something went wrong while counting");
        }

        var squareList = GetAllCombinations(columns, rows);

        bool canBePlaced = false;
        foreach(var number in squareList)
        {
            bool shapeCanBePlaced = true;
            foreach(var squareIndexToCheck in filledUpSquares)
            {
                var comp = gridSquares[number[squareIndexToCheck]].GetComponent<GridSquare>();
                if(comp.gridLocked)
                {
                    shapeCanBePlaced = false;
                }
            }
            if(shapeCanBePlaced)
            {
                canBePlaced = true;
            }
        }
        return canBePlaced;
    }

    private List<int[]> GetAllCombinations(int columns, int rows)
    {
        var blockList = new List<int[]>();
        var lastColumnIndex = 0;
        var lastRowIndex = 0;

        int safeIndex = 0;
        while(lastRowIndex+(rows-1)<10)
        {
            var rowData = new List<int>();
            for(var row = lastRowIndex;row<lastRowIndex+rows;row++)
            {
                for (var column = lastColumnIndex; column < lastColumnIndex + columns; column++)
                {
                    rowData.Add(lineIndicator.lineData[row, column]);
                }

            }
            blockList.Add(rowData.ToArray());
            lastColumnIndex++;
            if(lastColumnIndex +(columns - 1)>=10)
            {
                lastRowIndex++;
                lastColumnIndex = 0;
            }
            safeIndex++;
            if(safeIndex > 100)
            {
                break;
            }
        }
        return blockList;
    }
}
