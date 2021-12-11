using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    // Start is called before the first frame update
    public int columns = 10;
    public int rows = 10;
    public GameObject square;

    public float squareOffset = 0.5f;
    public float squareScale = 0.5f;
    public float squareGap = 0.1f;
    public Vector2 startPos = new Vector2(0.0f, 0.0f);


    private Vector2 offset = new Vector2(0.0f, 0.0f);
    private List<GameObject> gridSquares = new List<GameObject>();
    void Start()
    {
        CreateGrids();
    }

    private void CreateGrids()
    {
        SpawnGrids();
        SetGridPositions();
    }

    private void SpawnGrids()
    {
        for(int row = 0; row < rows; row++)
        {
            for(int column = 0; column < columns; columns++)
            {
                gridSquares.Add(Instantiate(square));
                gridSquares[gridSquares.Count - 1].transform.SetParent(this.transform);
                gridSquares[gridSquares.Count - 1].GetComponent<GridSquare>().SetImage();
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
            var posYOffset = offset.y * columnNumber + (squareGapNum.y * squareGap);

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
            square.GetComponent<RectTransform>().localPosition = new Vector3(startPos.x + posXOffset, startPos.y + posYOffset);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
