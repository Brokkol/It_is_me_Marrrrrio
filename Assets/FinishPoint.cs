using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishPoint : MonoBehaviour
{
    public GameObject winText;          // сюда перетащим WinText
    public float restartDelay = 3f;

    private bool finished = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (finished) return;           // чтобы не сработало дважды
        if (!other.CompareTag("Player")) return;
        finished = true;

        other.GetComponent<PlayerController>().enabled = false;
        other.attachedRigidbody.velocity = Vector2.zero;
        
        CoinManager.instance.StopTimer();

        if (winText != null) winText.SetActive(true);

        Invoke(nameof(Restart), restartDelay);
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
