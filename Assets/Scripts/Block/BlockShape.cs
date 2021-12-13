using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BlockShape : MonoBehaviour
{
    public Image blockImage;
    void Start()
    {
        blockImage.gameObject.SetActive(false);
    }
    
    public void ActivateBlock()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        gameObject.SetActive(true);
    }
    public void DeActivateBlock()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.SetActive(false);
    }

    public void SetOccupied()
    {
        blockImage.gameObject.SetActive(false);
    }
    public void UnSetOccupied()
    {
        blockImage.gameObject.SetActive(true);
    }

   
}
