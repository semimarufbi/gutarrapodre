using UnityEngine;
using UnityEngine.SceneManagement;

public class scripttemp : MonoBehaviour
{
    public void CarregarCena(string cenaprincipal)
    {
        SceneManager.LoadScene(cenaprincipal);
    }
}
