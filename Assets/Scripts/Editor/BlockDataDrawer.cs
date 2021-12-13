using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BlockData),false)]
[CanEditMultipleObjects]
[System.Serializable]
public class BlockDataDrawer : Editor
{
    private BlockData blockDataInstance => target as BlockData;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        ClearBoardButton();
        EditorGUILayout.Space();
        DrawColumnsInpuutFields();
        EditorGUILayout.Space();
        if(blockDataInstance.board!= null && blockDataInstance.columns > 0 && blockDataInstance.rows > 0)
        {
            DrawBoardTable();
        }
        serializedObject.ApplyModifiedProperties();
        if(GUI.changed)
        {
            EditorUtility.SetDirty(blockDataInstance);
        }    
    }

    private void ClearBoardButton()
    {
        if(GUILayout.Button("Clear Square"))
        {
            blockDataInstance.Clear();
        }
    }

    private void DrawColumnsInpuutFields()
    {
        var columnTemp = blockDataInstance.columns;
        var rowTemp = blockDataInstance.rows;
        blockDataInstance.columns = EditorGUILayout.IntField("Columns", blockDataInstance.columns);
        blockDataInstance.rows = EditorGUILayout.IntField("Rows", blockDataInstance.rows);
        if((blockDataInstance.columns != columnTemp || blockDataInstance.rows != rowTemp) && (blockDataInstance.columns > 0 && blockDataInstance.rows > 0))
        {
            blockDataInstance.CreateBoard();
        }
    }

    private void DrawBoardTable()
    {
        var boardStyle = new GUIStyle("box");
        boardStyle.padding = new RectOffset(50,50,50,50); 
        boardStyle.margin.left = 32;

        var columnStyle = new GUIStyle();
        columnStyle.fixedHeight = 25;
        columnStyle.alignment = TextAnchor.MiddleCenter;

        var rowStyle = new GUIStyle();
        rowStyle.fixedHeight = 25;
        rowStyle.alignment = TextAnchor.MiddleCenter;

        var buttonStyle = new GUIStyle(EditorStyles.miniButtonMid);
        buttonStyle.normal.background = Texture2D.whiteTexture;
        buttonStyle.onNormal.background = Texture2D.grayTexture;

        for (var row = 0; row <blockDataInstance.rows; row++)
        {
            EditorGUILayout.BeginHorizontal(columnStyle);
            for (var column = 0; column < blockDataInstance.columns; column++)
            {
                EditorGUILayout.BeginHorizontal(rowStyle);
                var square = EditorGUILayout.Toggle(blockDataInstance.board[row].column[column], buttonStyle);
                blockDataInstance.board[row].column[column] = square;
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndHorizontal();
        }

    }
}
