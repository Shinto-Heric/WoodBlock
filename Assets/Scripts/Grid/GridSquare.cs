using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GridSquare : MonoBehaviour
{
    public Image image;
    public Image hoverImage;
    public Image selectImage;
    public Sprite squareImage;

    public bool selected { get; set; }
    public int gridIndex { get; set; }
    public bool gridLocked { get; set; }
    void Start()
    {
        selected = false;
        gridLocked = false;
    }

    public void PlaceShapeOnBoard()
    {
        ActivateGrid();
    }
    public void ActivateGrid()
    {
        hoverImage.gameObject.SetActive(false);
        selectImage.gameObject.SetActive(true);
        selected = true;
        gridLocked = true;
    }

    public void Deactivate()
    {
        selectImage.gameObject.SetActive(false);

    }
    public void ClearOccupied()
    {
        selected = false;
        gridLocked = false;
    }
    public bool CanWe()
    {
        return hoverImage.gameObject.activeSelf;
    }
    public void SetImage()
    {
        image.GetComponent<Image>().sprite = squareImage;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(gridLocked == false)
        {
            selected = true;
            hoverImage.gameObject.SetActive(true);
        }
        else
        {
            var squareTemp = collision.GetComponent<BlockShape>();
            if (squareTemp != null) squareTemp.SetOccupied();
        }
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        selected = true;
        if (gridLocked == false)
            hoverImage.gameObject.SetActive(true);
        else
        {
            var squareTemp = collision.GetComponent<BlockShape>();
            if (squareTemp != null) squareTemp.SetOccupied();
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (gridLocked == false)
        {
            selected = false;
            hoverImage.gameObject.SetActive(false);
        }
        else
        {
            var squareTemp = collision.GetComponent<BlockShape>();
            if (squareTemp != null) squareTemp.UnSetOccupied();
        }
    }
}
