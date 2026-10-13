// Редакторский инструмент: превращает ?-тайлы на Ground в отдельные объекты-префабы с тегом Block.
// Запуск: Mario → Make Question Blocks (в сцене Level1-1).
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class MakeQuestionBlocks
{
    const string PrefabPath = "Assets/Prefabs/QuestionBlock.prefab";
    const string SpritePath = "Assets/tile_14_256x256.png";
    const string QuestionTilePath = "Assets/Tiles/QuestionBlock.asset";

    [MenuItem("Mario/Make Question Blocks")]
    public static void Make()
    {
        AddTag("Block");
        if (!Directory.Exists("Assets/Prefabs")) AssetDatabase.CreateFolder("Assets", "Prefabs");

        // 1. Префаб блока: картинка + коллайдер 1×1 + тег Block. Rigidbody не нужен — блок стоит на месте.
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);
        if (prefab == null)
        {
            GameObject temp = new GameObject("QuestionBlock");
            temp.tag = "Block";
            SpriteRenderer sr = temp.AddComponent<SpriteRenderer>();
            sr.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(SpritePath);
            temp.AddComponent<BoxCollider2D>();
            prefab = PrefabUtility.SaveAsPrefabAsset(temp, PrefabPath);
            Object.DestroyImmediate(temp);
        }

        // 2. Находим ?-тайлы на Ground, стираем их и ставим на их место объекты
        Tilemap ground = GameObject.Find("Ground").GetComponent<Tilemap>();
        TileBase qTile = AssetDatabase.LoadAssetAtPath<TileBase>(QuestionTilePath);

        GameObject old = GameObject.Find("QuestionBlocks");
        if (old != null) Object.DestroyImmediate(old);
        GameObject group = new GameObject("QuestionBlocks");

        int count = 0;
        foreach (Vector3Int cell in ground.cellBounds.allPositionsWithin)
        {
            if (ground.GetTile(cell) != qTile) continue;
            ground.SetTile(cell, null);
            GameObject b = (GameObject)PrefabUtility.InstantiatePrefab(prefab, group.transform);
            b.transform.position = ground.GetCellCenterWorld(cell);
            count++;
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        Debug.Log("?-блоков превращено в объекты: " + count);
    }

    static void AddTag(string tag)
    {
        SerializedObject tm = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tags = tm.FindProperty("tags");
        for (int i = 0; i < tags.arraySize; i++)
            if (tags.GetArrayElementAtIndex(i).stringValue == tag) return;
        tags.InsertArrayElementAtIndex(tags.arraySize);
        tags.GetArrayElementAtIndex(tags.arraySize - 1).stringValue = tag;
        tm.ApplyModifiedProperties();
    }
}
