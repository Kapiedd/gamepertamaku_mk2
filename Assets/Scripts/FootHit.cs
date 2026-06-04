using UnityEngine;

public class FootHit : MonoBehaviour
{
    private PlayerMovement playerScript;

    void Start()
    {
        // Mengambil referensi script utama di objek parent (Player)
        playerScript = GetComponentInParent<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Jika trigger sepatu menembus objek bertag Enemy
        if (other.gameObject.CompareTag("Enemy"))
        {
            // 1. Matikan/Hancurkan musuh
            Destroy(other.gameObject);

            // 2. Buat Player memantul
            playerScript.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(playerScript.GetComponent<Rigidbody2D>().linearVelocity.x, playerScript.jumpForce * 0.8f);

            // 3. Tambah Skor
            // score += 20; // Kamu bisa tambahkan fungsi tambah skor di PlayerMovement lalu panggil di sini

            Debug.Log("Musuh diinjak!");
        }
    }
}