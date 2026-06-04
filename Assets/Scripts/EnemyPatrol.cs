using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float speed = 2f;
    public Transform groundCheck; // Objek kosong di depan kaki musuh
    public float checkRadius = 0.2f;
    public LayerMask groundLayer; // Layer tanah

    private bool movingRight = true;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Membuat musuh bergerak maju sesuai arah hadapnya
        if (movingRight)
        {
            rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
            transform.eulerAngles = new Vector3(0, -180, 0); // Hadap kanan
        }
        else
        {
            rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
            transform.eulerAngles = new Vector3(0, 0, 0); // Hadap kiri
        }

        // Deteksi apakah di depan musuh masih ada tanah
        bool isGroundAhead = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        // Jika di depannya jurang (tidak ada tanah), maka musuh berbalik arah
        if (!isGroundAhead)
        {
            movingRight = !movingRight;
        }
    }

    // Menampilkan lingkaran deteksi di jendela Scene untuk mempermudah monitoring
    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ganti deteksi nama menjadi deteksi Tag biar lebih aman
        if (other.gameObject.CompareTag("Bumper"))
        {
            movingRight = !movingRight;
        }
    }
}