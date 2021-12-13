using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockStorage : MonoBehaviour
{
    public List<BlockData> blockDatas;
    public List<Block> blocks;
    private void OnEnable()
    {
        GameEvents.RequestNewBlock += RequestNewBlock;
    }
    private void OnDisable()
    {
        GameEvents.RequestNewBlock -= RequestNewBlock;
    }
    void Start()
    {
        foreach(var block in blocks)
        {
            int blockNum = UnityEngine.Random.Range(0, blockDatas.Count);
            block.CreateBlock(blockDatas[blockNum]);
        }
    }

    public Block GetCurrentSelectedBlock()
    {
        foreach(var block in blocks)
        {
            if (block.IsOnStartPos() == false && block.IsAnyOfGridActive())
                return block;
        }
        return null;
    }
    private void RequestNewBlock()
    {
        foreach (var block in blocks)
        {
            var index = UnityEngine.Random.Range(0, blockDatas.Count);
            block.CreateNewShapes(blockDatas[index]);
        }
    }
}
