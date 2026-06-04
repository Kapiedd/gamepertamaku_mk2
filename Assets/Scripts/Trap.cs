using UnityEngine;

public class Trap : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Cek apakah yang menyentuh jebakan adalah Player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Ambil komponen PlayerMovement dari objek yang menabrak
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();

            if (player != null)
            {
                // Panggil fungsi TakeDamage yang sudah kita buat kemarin
                // Fungsi ini otomatis mengurangi nyawa dan merespawn player ke tempat aman
                player.TakeDamage();

                Debug.Log("Player terkena jebakan!");
            }
        }
    }

    // Gunakan ini jika jebakan kamu di-set sebagai "Is Trigger" (bisa ditembus)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.TakeDamage();
                Debug.Log("Player masuk ke area jebakan!");
            }
        }
    }
}