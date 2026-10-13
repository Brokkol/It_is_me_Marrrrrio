using UnityEngine;

public class QuestionBlock : MonoBehaviour
{
    public GameObject mushroomPrefab;   // ← внутри класса
    private bool used = false;

    public void Hit()
    {
        if (used) return;
        used = true;

        GetComponent<SpriteRenderer>().color = Color.gray;

        if (mushroomPrefab != null)
            Instantiate(mushroomPrefab, transform.position + Vector3.up, Quaternion.identity);
        else
            CoinManager.instance.AddCoin();
    }
}