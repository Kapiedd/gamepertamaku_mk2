using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Fungsi umum buat pindah ke level spesifik (dipakai di tombol angka 1, 2, 3)
    public void BukaLevel(string namaLevel)
    {
        SceneManager.LoadScene(namaLevel);
    }

    // Fungsi buat lanjut ke level berikutnya secara otomatis
    // (Misal dari Level 1 langsung ke Level 2)
    public void LanjutLevelBerikutnya()
    {
        int indexSekarang = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(indexSekarang + 1);
    }

    // Fungsi buat balik ke Main Menu
    public void BackToMenu()
    {
        SceneManager.LoadScene("SampleScene"); // Pastikan nama scene menu kamu benar
    }
}