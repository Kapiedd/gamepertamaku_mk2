using UnityEngine;
using UnityEngine.SceneManagement; // Baris ini WAJIB ada

public class SceneController : MonoBehaviour
{
    public void BukaLevel1()
    {
        // "Level 1" harus sama persis dengan nama file di folder Scenes kamu
        SceneManager.LoadScene("Level 1");
    }
}