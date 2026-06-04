using UnityEngine;
using TMPro; // Wajib ada untuk membaca TextMeshPro UI
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Pergerakan Karakter")]
    public float moveSpeed = 7f;
    public float jumpForce = 10f;
    private float horizontalInput;
    private bool isGrounded;
    private int jumpCount;
    public int maxJumps = 2; // Mendukung Double Jump

    [Header("Komponen Fisika & Animasi")]
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    [Header("Sistem UI & Game Logic")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    private int score = 0;
    private int lives = 3;
    private Vector3 respawnPoint;

    void Start()
    {
        // Mengambil semua komponen yang menempel di objek Player automaticamente
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Mencatat posisi awal player sebagai titik lahir kembali (respawn)
        respawnPoint = transform.position;

        // Update tampilan UI di awal game
        UpdateUI();
    }

    void Update()
    {
        // 1. Mengambil Input Gerakan Horizontal (Kanan/Kiri)
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // 2. Mengatur Kecepatan Jalan
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        // 3. Mengatur Animasi Jalan (isRunning)
        if (horizontalInput != 0)
        {
            anim.SetBool("isRunning", true);
            // Membalik arah hadap badan karakter sprite
            spriteRenderer.flipX = horizontalInput < 0;
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

        // 4. Input Lompat & Double Jump
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpCount++;

            // Memicu trigger doubleJump di Animator jika ini lompatan kedua
            if (jumpCount > 1)
            {
                anim.SetTrigger("doubleJump");
            }
        }

        // 5. Mengirim Status Melayang ke Animator (isJumping)
        anim.SetBool("isJumping", !isGrounded);

        // 6. Mengirim Kecepatan Vertikal untuk Animasi Jatuh (yVelocity)
        anim.SetFloat("yVelocity", rb.linearVelocity.y);

        // 7. Mekanik & Animasi Menunduk (Crouch) - Tombol S atau Panah Bawah
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            anim.SetBool("isCrouching", true);
            // (Opsional) Kamu bisa kurangi kecepatan jalan di sini kalau mau pas nunduk jalannya lambat
        }
        else
        {
            anim.SetBool("isCrouching", false);
        }
    }

    // Mendeteksi apakah kaki karakter menyentuh tanah padat
    // Mendeteksi apakah badan karakter menabrak benda padat
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = 0; // Reset jatah lompat saat menginjak tanah
        }

        // LOGIKA BARU: Jika menabrak musuh
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Cek apakah posisi kaki Player berada di atas kepala musuh
            // collision.contacts[0].normal.y mengecek arah tabrakan. 
            // Jika nilainya mendekati -1, artinya Player menginjak objek tersebut dari atas.
            if (collision.contacts[0].normal.y <= -0.5f)
            {
                // 1. Beri gaya dorong ke atas sedikit biar si rubah memantul setelah menginjak musuh
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * 0.8f);

                // 2. Hancurkan/Matikan objek musuh tersebut
                Destroy(collision.gameObject);

                // 3. (Opsional) Tambah skor bonus karena berhasil bunuh musuh
                score += 20;
                UpdateUI();

                Debug.Log("Musuh berhasil dikalahkan!");
            }
            else
            {
                // Jika menabrak dari samping atau bawah, Player yang kena damage
                TakeDamage();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    // Mendeteksi Trigger (Benda yang bisa ditembus seperti Koin/Item)
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Logika untuk Koin
        if (other.gameObject.CompareTag("Coin"))
        {
            score += 10;
            UpdateUI();
            Destroy(other.gameObject); // Hapus koin dari layar
        }

        // Logika untuk Garis Finish dengan Syarat Poin
        // Logika untuk Garis Finish dengan Syarat Poin
        if (other.gameObject.CompareTag("Finish"))
        {
            if (score >= 50)
            {
                // Ambil nama scene yang sedang dimainkan saat ini
                string currentSceneName = SceneManager.GetActiveScene().name;

                // JIKA SEKARANG LAGI DI LEVEL 3, LANGSUNG BALIK KE LOBBY
                // (Pastikan tulisan "Level 3" dan "Lobby" di bawah ini sama persis dengan nama file Scene kamu)
                if (currentSceneName == "Level 3")
                {
                    Debug.Log("Selamat! Kamu menamatkan game. Kembali ke Lobby...");
                    SceneManager.LoadScene("Lobby");
                }
                else
                {
                    // JIKA BUKAN LEVEL 3 (Misal Level 1 atau Level 2), LANJUT KE LEVEL BERIKUTNYA
                    Debug.Log("Lanjut ke level berikutnya...");
                    int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

                    if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
                    {
                        SceneManager.LoadScene(nextSceneIndex);
                    }
                    else
                    {
                        Debug.Log("Anda Berhasil!!, kembali ke Lobby.");
                        SceneManager.LoadScene("Lobby");
                    }
                }
            }
            else
            {
                Debug.Log("Poin kamu belum cukup! Kumpulkan sampai 50 poin dulu. Poin sekarang: " + score);
            }
        }
    }

    // Fungsi saat karakter terluka atau menabrak musuh
    // Fungsi saat karakter terluka atau menabrak musuh/jebakan
    public void TakeDamage()
    {
        anim.SetTrigger("hurt"); // Memicu animasi kaget/terluka
        lives--;
        UpdateUI();

        if (lives <= 0)
        {
            Debug.Log("Game Over! Nyawa habis, kembali ke Lobby...");

            // KODE BARU: Jika nyawa habis, langsung load scene Lobby
            // (Pastikan nama "Lobby" di bawah ini sama persis dengan nama scene menu utamamu)
            SceneManager.LoadScene("Lobby");
        }
        else
        {
            // Jika nyawa masih ada, kembalikan ke titik aman awal level
            transform.position = respawnPoint;
            rb.linearVelocity = Vector2.zero; // Menghentikan sisa gaya dorong fisika
        }
    }

    // Fungsi pembantu untuk memperbarui teks UI di layar secara aman
    void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
        if (livesText != null)
        {
            livesText.text = "Health: " + lives;
        }
    }
}