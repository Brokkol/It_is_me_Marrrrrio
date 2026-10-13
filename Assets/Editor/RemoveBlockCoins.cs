// Редакторский инструмент: удаляет монетки, висящие над ?-блоками (их теперь выдают сами блоки).
// Монетки над первой ямой (X 68–72) остаются. Запуск: Mario → Remove Coins Above Blocks, потом Cmd+S.
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class RemoveBlockCoins
{
    [MenuItem("Mario/Remove Coins Above Blocks")]
    public static void Remove()
    {
        QuestionBlock[] blocks = Object.FindObjectsOfType<QuestionBlock>();
        int removed = 0;

        foreach (Coin coin in Object.FindObjectsOfType<Coin>())
        {
            Vector2 c = coin.transform.position;
            foreach (QuestionBlock b in blocks)
            {
                Vector2 p = b.transform.position;
                // монетка ровно над блоком: тот же X, чуть выше
                if (Mathf.Abs(c.x - p.x) < 0.3f && c.y > p.y && c.y - p.y < 2f)
                {
                    Undo.DestroyObjectImmediate(coin.gameObject);
                    removed++;
                    break;
                }
            }
        }
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log("Удалено монеток над блоками: " + removed);
    }
}
