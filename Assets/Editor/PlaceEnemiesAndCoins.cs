// Редакторский инструмент: делает префабы из Enemy и Coin и расставляет их по уровню 1-1.
// Запуск: Mario → Place Enemies and Coins (в открытой сцене Level1-1).
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class PlaceEnemiesAndCoins
{
    const string Folder = "Assets/Prefabs";

    [MenuItem("Mario/Place Enemies and Coins")]
    public static void Place()
    {
        if (!Directory.Exists(Folder)) AssetDatabase.CreateFolder("Assets", "Prefabs");

        GameObject enemyPrefab = MakePrefab("Enemy");
        GameObject coinPrefab = MakePrefab("Coin");
        if (enemyPrefab == null || coinPrefab == null) return;

        // Папки в Hierarchy, чтобы не было каши
        GameObject enemies = Group("Enemies");
        GameObject coins = Group("Coins");

        // Враги: x, на сколько клеток ходит в каждую сторону (чтобы не врезаться в трубы)
        float[,] enemyData =
        {
            { 22f, 4f },                       // у первых блоков
            { 42.5f, 2f },                     // между 2-й и 3-й трубой
            { 51.5f, 2f }, { 53.5f, 2f },      // между 3-й и 4-й трубой
            { 80f, 3f },                       // после первой ямы
            { 97f, 3f }, { 99f, 3f },
            { 114f, 3f }, { 116f, 3f },
            { 124f, 3f }, { 126f, 3f },
            { 172f, 3f }, { 174f, 3f },        // перед последней трубой
        };
        for (int i = 0; i < enemyData.GetLength(0); i++)
        {
            Spawn(enemyPrefab, new Vector3(enemyData[i, 0], 0.5f, 0f), enemies.transform);
        }

        // Монетки над каждым ?-блоком (пока блоки не умеют выбивать монеты)
        Vector2Int[] qBlocks =
        {
            new Vector2Int(16, 3), new Vector2Int(21, 3), new Vector2Int(23, 3), new Vector2Int(22, 7),
            new Vector2Int(78, 3), new Vector2Int(94, 7), new Vector2Int(106, 3), new Vector2Int(109, 3),
            new Vector2Int(112, 3), new Vector2Int(109, 7), new Vector2Int(129, 7), new Vector2Int(130, 7),
            new Vector2Int(170, 3),
        };
        foreach (Vector2Int q in qBlocks)
            Spawn(coinPrefab, new Vector3(q.x + 0.5f, q.y + 1.6f, 0f), coins.transform);

        // Дуга монеток над первой ямой
        for (int i = 0; i < 4; i++)
            Spawn(coinPrefab, new Vector3(68.5f + i, 3f + (i == 1 || i == 2 ? 1f : 0f), 0f), coins.transform);

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        Debug.Log("Расставлено врагов: " + enemyData.GetLength(0) + ", монет: " + (qBlocks.Length + 4));
    }

    // Превращает объект из сцены в префаб (сам объект становится его копией) и убирает его со сцены
    static GameObject MakePrefab(string name)
    {
        string path = Folder + "/" + name + ".prefab";
        GameObject existing = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        GameObject inScene = GameObject.Find(name);

        if (existing == null)
        {
            if (inScene == null) { Debug.LogError("Не нашёл " + name + " в сцене"); return null; }
            existing = PrefabUtility.SaveAsPrefabAsset(inScene, path);
        }
        if (inScene != null && inScene.transform.parent == null) Object.DestroyImmediate(inScene);
        return existing;
    }

    static GameObject Group(string name)
    {
        GameObject g = GameObject.Find(name);
        if (g != null) Object.DestroyImmediate(g);   // при повторном запуске пересоздаём
        return new GameObject(name);
    }

    static GameObject Spawn(GameObject prefab, Vector3 pos, Transform parent)
    {
        GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent);
        go.transform.position = pos;
        return go;
    }
}
