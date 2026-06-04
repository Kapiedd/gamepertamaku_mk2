using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Kamera")]
    public Transform target; // Tarik objek Player ke sini nanti

    [Header("Pengaturan Posisi & Kecepatan")]
    public float smoothSpeed = 0.125f; // Kehalusan gerakan kamera
    public Vector3 offset = new Vector3(0, 0, -10); // Jarak aman kamera (Z harus minus!)

    [Header("Pengaturan Jarak Zoom Kamera")]
    [Range(1f, 20f)]
    public float zoomSize = 5f; // Mengatur ukuran zoom kamera (Makin kecil angka = Makin Zoom)

    private Camera cam;

    void Start()
    {
        // Mengambil komponen kamera utama
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // 1. Logika Mengikuti Karakter dengan Halus (Smooth Follow)
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            // 2. Logika Mengatur Ukuran Zoom
            if (cam != null)
            {
                cam.orthographicSize = zoomSize;
            }
        }
    }
}