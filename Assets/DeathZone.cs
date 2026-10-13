using UnityEngine;
using UnityEngine.SceneManagement; // Обязательно для перезагрузки сцен

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.name == "Player")
        {
            // Перезагружаем текущую активную сцену
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}