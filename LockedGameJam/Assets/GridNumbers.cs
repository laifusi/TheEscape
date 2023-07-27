using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GridNumbers : MonoBehaviour
{
#if UNITY_EDITOR
    public Vector2Int GridSize { get; set; }

    private void OnDrawGizmos()
    {
        if (GridSize.x == 0 || GridSize.y == 0)
            return;

        GUIStyle style = new GUIStyle();
        style.fixedHeight = 1;
        style.fixedWidth = 1;
        style.alignment = TextAnchor.MiddleCenter;
        style.richText = true;

        for (float i = -(GridSize.x/2f + 0.5f); i <= GridSize.x/2f + 0.5; i += 1)
        {
            for (float j = -(GridSize.y/2f + 0.5f); j <= GridSize.y/2f + 0.5f; j += 1)
            {
                Handles.Label(new Vector3(i, j, 0), "<color=red>" + i + ", " + j + "</color>", style);
            }
        }
    }

    [ContextMenu("GetGridSize")]
    private void GetGridSize()
    {
        Vector2 size = GetComponent<SpriteRenderer>().size;
        GridSize = new Vector2Int((int)size.x - 2, (int)size.y - 2);
    }
#endif
}
