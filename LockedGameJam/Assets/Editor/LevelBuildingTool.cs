using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class LevelBuildingTool : EditorWindow
{
    private string prefabsFolder = "Assets/Prefabs";

    private List<GameObject> prefabs = new List<GameObject>();
    private Vector2[] prefabLocations;

    private Vector2Int gridSize = new Vector2Int();
    private GameObject wallPrefab;
    private GameObject floorPrefab;
    private Camera mainCam;
    private Vector2 scrollPos;

    [MenuItem("Tools/LevelBuilder")]
    static void OpenWindow()
    {
        LevelBuildingTool window = (LevelBuildingTool)GetWindow(typeof(LevelBuildingTool));
        window.Show();
    }

    private void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(700));
        GUILayout.Label("Create the grid", EditorStyles.boldLabel);
        gridSize = EditorGUILayout.Vector2IntField("Grid Size", gridSize);
        if (gridSize.x < 1)
            gridSize.x = 1;
        if (gridSize.y < 1)
            gridSize.y = 1;

        mainCam = EditorGUILayout.ObjectField("Camera", mainCam, typeof(Camera), true) as Camera;
        wallPrefab = EditorGUILayout.ObjectField("Wall prefab", wallPrefab, typeof(GameObject), false) as GameObject;
        floorPrefab = EditorGUILayout.ObjectField("Floor prefab", floorPrefab, typeof(GameObject), false) as GameObject;
        if (GUILayout.Button("Create Grid"))
            CreateGrid();

        EditorGUILayout.Space(25);
        GUILayout.Label("Find Prefabs", EditorStyles.boldLabel);
        prefabsFolder = EditorGUILayout.TextField("Prefabs path", prefabsFolder);
        if (GUILayout.Button("Find Prefabs"))
            FindPrefabs();

        ShowPrefabs();
        EditorGUILayout.EndScrollView();
    }

    private void CreateGrid()
    {
        int biggerSize = gridSize.x > gridSize.y ? gridSize.x : gridSize.y;
        float cameraSize = biggerSize / 2f + 1.5f;
        mainCam.orthographicSize = cameraSize;

        SpriteRenderer floor = Instantiate(floorPrefab, Vector3.zero, Quaternion.identity).GetComponent<SpriteRenderer>();
        floor.size = new Vector2(gridSize.x + 2, gridSize.y + 2);

        float xOffset = gridSize.x/2f + 0.5f;
        float yOffset = gridSize.y/2f + 0.5f;
        SpriteRenderer leftWall = Instantiate(wallPrefab, Vector3.left * xOffset, Quaternion.identity).GetComponent<SpriteRenderer>();
        SpriteRenderer rightWall = Instantiate(wallPrefab, Vector3.right * xOffset, Quaternion.identity).GetComponent<SpriteRenderer>();
        SpriteRenderer topWall = Instantiate(wallPrefab, Vector3.up * yOffset, Quaternion.identity).GetComponent<SpriteRenderer>();
        SpriteRenderer bottomWall = Instantiate(wallPrefab, Vector3.down * yOffset, Quaternion.identity).GetComponent<SpriteRenderer>();
        leftWall.size = new Vector2(1, gridSize.y + 2);
        rightWall.size = new Vector2(1, gridSize.y + 2);
        topWall.size = new Vector2(gridSize.x + 2, 1);
        bottomWall.size = new Vector2(gridSize.x + 2, 1);

        floor.GetComponent<GridNumbers>().GridSize = gridSize;
    }

    private void FindPrefabs()
    {
        prefabs.Clear();

        string[] assetsFound = AssetDatabase.FindAssets("t:GameObject", new[] { prefabsFolder });

        for (int i = 0; i < assetsFound.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(assetsFound[i]);
            prefabs.Add(AssetDatabase.LoadAssetAtPath<GameObject>(path));
        }

        prefabLocations = new Vector2[prefabs.Count];
    }

    private void ShowPrefabs()
    {
        for (int i = 0; i < prefabs.Count; i++)
        {
            GUILayout.BeginHorizontal();
            SpriteRenderer spriteRenderer = prefabs[i]?.GetComponent<SpriteRenderer>();
            prefabLocations[i] = EditorGUILayout.Vector2Field(prefabs[i].name + " Location", prefabLocations[i], GUILayout.Width(300));
            GUILayout.EndHorizontal();
            if (spriteRenderer != null)
            {
                if (GUILayout.Button(spriteRenderer.sprite.texture, GUILayout.Width(100), GUILayout.Height(100)))
                {
                    PreparePrefabToPaint(prefabs[i], prefabLocations[i]);
                }
            }
            else
            {
                if (GUILayout.Button("Create " + prefabs[i].name, GUILayout.Width(100), GUILayout.Height(100)))
                {
                    PreparePrefabToPaint(prefabs[i], prefabLocations[i]);
                }
            }
        }
    }

    private void PreparePrefabToPaint(GameObject prefabToPaint, Vector3 location)
    {
        Instantiate(prefabToPaint, location, Quaternion.identity);
    }
}
