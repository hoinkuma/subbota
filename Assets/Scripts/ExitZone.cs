using UnityEngine;

public class ExitZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (GameManager.instance != null)
            GameManager.instance.OpenExitMenu();
    }
}