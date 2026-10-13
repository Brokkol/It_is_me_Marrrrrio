// Редакторский инструмент: кладёт гриб в три ?-блока, как в оригинальном 1-1.
// Запуск: Mario → Assign Mushrooms (в сцене Level1-1). Потом Cmd+S.
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class AssignMushrooms
{
    // Центры блоков с грибами: после старта, после первой ямы, верхний в «треугольнике»
    static readonly Vector2[] MushroomBlocks =
    {
        new Vector2(21.5f, 3.5f),
        new Vector2(78.5f, 3.5f),
        new Vector2(109.5f, 7.5f),
    };

    [MenuItem("Mario/Assign Mushrooms")]
    public static void Assign()
    {
        GameObject mushroom = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Mushroom.prefab");
        if (mushroom == null) { Debug.LogError("Нет Assets/Prefabs/Mushroom.prefab"); return; }

        int count = 0;
        foreach (QuestionBlock block in Object.FindObjectsOfType<QuestionBlock>())
        {
            Vector2 p = block.transform.position;
            foreach (Vector2 target in MushroomBlocks)
            {
                if (Vector2.Distance(p, target) < 0.3f)
                {
                    Undo.RecordObject(block, "Assign Mushroom");
                    block.mushroomPrefab = mushroom;
                    PrefabUtility.RecordPrefabInstancePropertyModifications(block);
                    count++;
                }
            }
        }
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log("Грибов разложено по блокам: " + count + " из " + MushroomBlocks.Length);
    }
}
