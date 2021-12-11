using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSquare : MonoBehaviour
{
    public Image image;
    public Sprite squareImage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetImage()
    {
        image.GetComponent<Image>().sprite = squareImage;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
