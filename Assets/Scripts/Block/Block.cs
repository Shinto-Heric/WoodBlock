using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Block : MonoBehaviour, IPointerClickHandler,IPointerUpHandler,IBeginDragHandler,IDragHandler,IEndDragHandler,IPointerDownHandler
{
    public GameObject blockSquareImage;
    public Vector3 blockSelectScale;
    public Vector3 blockPlaceScale;

    public Vector2 offset =  new Vector2(0f,700f);
    public int totalSqaureNumber { get; set; }
    [HideInInspector]
    public BlockData currentBlockData;
    private List<GameObject> currentBlock = new List<GameObject>();
    private RectTransform blockTransform;
    private Canvas gameCanvas;
    private bool blockDraggable;
    private Vector3 startPos;
    private bool gridActive = false;

    public void Awake()
    {
        blockTransform = this.GetComponent<RectTransform>();
        blockDraggable = true;
        gameCanvas = GetComponentInParent<Canvas>();
        startPos = blockTransform.localPosition;
        gridActive = true;
    }
    void Start()
    {

    }
    private void OnEnable()
    {
        GameEvents.MoveShapeToStartPosition += MoveShapeToStartPosition; 
        GameEvents.SetShapeInactive += SetShapeInactive; 

    }
    private void OnDisable()
    {
        GameEvents.MoveShapeToStartPosition -= MoveShapeToStartPosition;
        GameEvents.SetShapeInactive -= SetShapeInactive;


    }
    public bool IsOnStartPos()
    {
        return blockTransform.localPosition == startPos;
    }
    public bool IsAnyOfGridActive()
    {
        foreach(var block in currentBlock)
        {
            if(block.gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    public void DeactivateBlock()
    {
        if(gridActive)
        {
            foreach (var block in currentBlock)
            {
                block?.GetComponent<BlockShape>().DeActivateBlock();
            }
        }
        gridActive = false;
    }
    public void SetShapeInactive()
    {
        if(IsOnStartPos() == false && IsAnyOfGridActive())
        {
            foreach (var block in currentBlock)
            {
                block.gameObject.SetActive(false);
            }
        }
    }
    public void ActivateBlock()
    {
        if (gridActive)
        {
            foreach (var block in currentBlock)
            {
                block?.GetComponent<BlockShape>().ActivateBlock();
            }
        }
        gridActive = false;
    }
    public void CreateNewShapes(BlockData blockData)
    {
        blockTransform.localPosition = startPos;
        CreateBlock(blockData);
    }
    public void CreateBlock(BlockData blockData)
    {
        currentBlockData = blockData;
        totalSqaureNumber = GetNOOFShapes(blockData);
        while(currentBlock.Count <= totalSqaureNumber)
        {
            currentBlock.Add(Instantiate(blockSquareImage, transform) as GameObject);
        }
        foreach(var square in currentBlock)
        {
            square.gameObject.transform.position = Vector3.zero;
            square.gameObject.SetActive(false);
        }
        var squareRect = blockSquareImage.GetComponent<RectTransform>();
        var moveDistace = new Vector2(squareRect.rect.width * squareRect.localScale.x, squareRect.rect.height * squareRect.localScale.y);
        int currectIndexInList = 0;
       for(var row = 0; row < blockData.rows;row++)
        {
            for(var column = 0;column<blockData.columns;column++)
            {
                if(blockData.board[row].column[column])
                {
                    currentBlock[currectIndexInList].SetActive(true);
                    currentBlock[currectIndexInList].GetComponent<RectTransform>().localPosition =
                        new Vector2(GetXPosForBlockSquare(blockData, column, moveDistace), GetYPosForBlockSquare(blockData, row, moveDistace));
                    currectIndexInList++;
                }
            }
        }
    }

    public float GetYPosForBlockSquare(BlockData blockData, int row, Vector2 distance)
    {
        float shiftY = 0;
        if (blockData.rows > 1)
        {
            if (blockData.rows % 2 != 0)
            {
                var midIndex = (blockData.rows - 1) / 2;
                var multiplier = (blockData.rows - 1) / 2;
                if (row < midIndex)
                {
                    shiftY = distance.y * 1;
                    shiftY *= multiplier;
                }
                else if (row > midIndex)
                {
                    shiftY = distance.y * -1;
                    shiftY *= multiplier;
                }
            }
            else
            {
                //var midIndex2 = (blockData.rows == 4) ? 2 : (blockData.rows / 2);
                //var midIndex1 = (blockData.rows == 4) ? 1 : blockData.rows - 1;
                //float multiplier = (float) ((blockData.rows-1) / 2.0f);

                var midIndex2 = (blockData.rows == 2) ? 1 : ((blockData.rows > 3) ? (blockData.rows / 2) : (blockData.rows / 2));
                var midIndex1 = (blockData.rows == 2) ? 0 : ((blockData.rows > 3) ? (blockData.rows/2 - 1) : (blockData.rows - 1));
                var multiplier =   ((blockData.rows > 3) ? (blockData.rows - 1) / 2.0f : (blockData.rows / 2));

                if (row == midIndex1 || row == midIndex2)
                {
                    if (row == midIndex2)
                    {
                        shiftY = (distance.y / 2) * -1;
                    }
                    if (row == midIndex1)
                    {
                        shiftY = (distance.y / 2);
                    }
                }
                if (row < midIndex1 && row < midIndex2)
                {
                    shiftY = distance.y * 1;
                    shiftY *= multiplier;
                }
                else if (row > midIndex1 && row > midIndex2)
                {
                    shiftY = distance.y * -1;
                    shiftY *= multiplier;
                }
            }
        }
        return shiftY;
    }
    public float GetXPosForBlockSquare(BlockData blockData, int column, Vector2 distance)
    {
        float shiftX = 0;
        if (blockData.columns > 1)
        {
            if (blockData.columns % 2 != 0)
            {
                var midIndex = (blockData.columns - 1) / 2;
                var multiplier = (blockData.columns - 1) / 2;
                if (column < midIndex)
                {
                    shiftX = distance.x * -1;
                    shiftX *= multiplier;
                }
                else if(column > midIndex)
                {
                    shiftX = distance.x * 1;
                    shiftX *= multiplier;
                }
            }
            else
            {
                var midIndex2 = (blockData.columns == 2) ? 1 : ((blockData.columns > 3) ? (blockData.columns / 2) : (blockData.columns / 2));
                var midIndex1 = (blockData.columns == 2) ? 0 : ((blockData.columns > 3) ? (blockData.columns / 2 - 1) : (blockData.columns - 1));
                var multiplier = ((blockData.columns > 3) ? (blockData.columns - 1) / 2.0f : (blockData.columns / 2));
                if (column == midIndex1 || column == midIndex2)
                {
                    if (column == midIndex2)
                    {
                        shiftX = (distance.x / 2);
                    }
                    if (column == midIndex1)
                    {
                        shiftX =(distance.x / 2) * -1;
                    }
                }
                if (column < midIndex1 && column < midIndex2)
                {
                    shiftX = distance.x * -1;
                    shiftX *= multiplier;
                }
                else if (column > midIndex1 && column > midIndex2)
                {
                    shiftX = distance.x * 1;
                    shiftX *= multiplier;
                }
            }
        }
            return shiftX;
    }
    private int GetNOOFShapes(BlockData blockData)
    {
        int number = 0;
        foreach(var rowData in blockData.board)
        {
            foreach(var active in rowData.column)
            {
                if (active) number++;
            }
        }
        return number;
    }
    public void OnPointerClick(PointerEventData eventData)
    {

    }
    public void OnPointerUp(PointerEventData eventData)
    {

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        this.GetComponent<RectTransform>().localScale = blockSelectScale;
    }
    public void OnDrag(PointerEventData eventData)
    {
        blockTransform.anchorMin = new Vector2(0f, 0f);
        blockTransform.anchorMax = new Vector2(0f, 0f);
        blockTransform.pivot = new Vector2(0f, 0f);
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(gameCanvas.transform as RectTransform, eventData.position, Camera.main, out position);
        blockTransform.localPosition = position + offset;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        this.GetComponent<RectTransform>().localScale = blockPlaceScale;
        GameEvents.CheckIfBlockCanBePlaced();
    }
    public void OnPointerDown(PointerEventData eventData)
    {

    }
    private void MoveShapeToStartPosition()
    {
        blockTransform.transform.localPosition = startPos;
    }
}
