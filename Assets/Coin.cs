using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.name == "Player")
        {
            if (CoinManager.instance != null)
            {
                CoinManager.instance.AddCoin();
            }
            Destroy(gameObject);
        }
    }
}