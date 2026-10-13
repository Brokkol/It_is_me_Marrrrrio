// Редакторский инструмент: рисует уровень по мотивам Super Mario Bros 1-1.
// Запуск: верхнее меню Unity → Mario → Build World 1-1.
// Твоя текущая сцена сохраняется, уровень строится в КОПИИ Assets/Scenes/Level1-1.unity.
// Этот файл в игру не попадает (папка Editor), его можно удалить, когда уровень готов.
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public static class BuildWorld1_1
{
    const string BrickTilePath = "Assets/Tiles/tile_05_256x256.asset";
    const string QuestionSpritePath = "Assets/tile_14_256x256.png";
    const string QuestionTilePath = "Assets/Tiles/QuestionBlock.asset";
    const string NewScenePath = "Assets/Scenes/Level1-1.unity";

    static Tilemap map;
    static TileBase brick, question;

    [MenuItem("Mario/Build World 1-1")]
    public static void Build()
    {
        // 1. Сохраняем текущую сцену и делаем её копию
        Scene current = SceneManager.GetActiveScene();
        EditorSceneManager.SaveScene(current);
        EditorSceneManager.SaveScene(current, NewScenePath, true);
        EditorSceneManager.OpenScene(NewScenePath, OpenSceneMode.Single);

        // 2. Тайлы
        brick = AssetDatabase.LoadAssetAtPath<TileBase>(BrickTilePath);
        question = AssetDatabase.LoadAssetAtPath<TileBase>(QuestionTilePath);
        if (question == null)
        {
            Sprite s = AssetDatabase.LoadAssetAtPath<Sprite>(QuestionSpritePath);
            Tile t = ScriptableObject.CreateInstance<Tile>();
            t.sprite = s;
            AssetDatabase.CreateAsset(t, QuestionTilePath);
            question = t;
        }

        GameObject groundGO = GameObject.Find("Ground");
        if (groundGO == null || brick == null)
        {
            Debug.LogError("Не нашёл объект Ground или тайл кирпича");
            return;
        }
        map = groundGO.GetComponent<Tilemap>();
        map.ClearAllTiles();

        // 3. Земля: 2 ряда (y = -2 и -1), верх земли на y = 0. Ямы как в 1-1.
        Ground(0, 68); Ground(71, 85); Ground(89, 152); Ground(155, 211);
        Column(-1, 0, 14);                          // стенка слева, чтобы не уйти за край

        // 4. Блоки: нижний ряд на y = 3, верхний на y = 7
        Q(16, 3);
        B(20, 3); Q(21, 3); B(22, 3); Q(23, 3); B(24, 3); Q(22, 7);
        B(77, 3); Q(78, 3); B(79, 3);
        for (int x = 80; x <= 87; x++) B(x, 7);
        for (int x = 91; x <= 93; x++) B(x, 7); Q(94, 7); B(94, 3);
        B(100, 3); B(101, 3);
        Q(106, 3); Q(109, 3); Q(112, 3); Q(109, 7);
        B(118, 3);
        for (int x = 121; x <= 123; x++) B(x, 7);
        B(128, 7); Q(129, 7); Q(130, 7); B(131, 7); B(129, 3); B(130, 3);
        B(168, 3); B(169, 3); Q(170, 3); B(171, 3);

        // 5. Трубы (пока из кирпичей, 2 клетки шириной)
        Pipe(28, 2); Pipe(38, 3); Pipe(46, 4); Pipe(57, 4); Pipe(163, 2); Pipe(179, 2);

        // 6. Лестницы
        for (int i = 0; i < 4; i++) Column(134 + i, 0, i + 1);   // вверх
        for (int i = 0; i < 4; i++) Column(140 + i, 0, 4 - i);   // вниз
        for (int i = 0; i < 4; i++) Column(148 + i, 0, i + 1);
        Column(152, 0, 4);
        for (int i = 0; i < 4; i++) Column(155 + i, 0, 4 - i);
        for (int i = 0; i < 8; i++) Column(181 + i, 0, i + 1);   // финальная лестница
        Column(189, 0, 8);

        // 7. Расставляем объекты
        Move("Player", new Vector3(3f, 1.2f, 0f));
        Move("Main Camera", new Vector3(3f, 2f, -10f));
        Move("Enemy", new Vector3(22f, 0.5f, 0f));
        Move("Coin", new Vector3(16.5f, 5f, 0f));
        GameObject finish = Move("Finish", new Vector3(198.5f, 5f, 0f));
        if (finish) finish.transform.localScale = new Vector3(0.3f, 10f, 1f);   // флагшток
        GameObject death = Move("DeathZone", new Vector3(105f, -7f, 0f));
        if (death) death.transform.localScale = new Vector3(240f, 2f, 1f);

        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        Debug.Log("World 1-1 построен в " + NewScenePath);
    }

    static void B(int x, int y) => map.SetTile(new Vector3Int(x, y, 0), brick);
    static void Q(int x, int y) => map.SetTile(new Vector3Int(x, y, 0), question);

    static void Ground(int from, int to)
    {
        for (int x = from; x <= to; x++) { B(x, -2); B(x, -1); }
    }

    // Столбик из кирпичей высотой h, начиная с yStart
    static void Column(int x, int yStart, int h)
    {
        for (int y = yStart; y < yStart + h; y++) B(x, y);
    }

    static void Pipe(int x, int h) { Column(x, 0, h); Column(x + 1, 0, h); }

    static GameObject Move(string name, Vector3 pos)
    {
        GameObject go = GameObject.Find(name);
        if (go != null) go.transform.position = pos;
        return go;
    }
}
