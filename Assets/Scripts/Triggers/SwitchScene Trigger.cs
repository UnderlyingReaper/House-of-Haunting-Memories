using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchSceneTrigger : MonoBehaviour
{
    [SerializeField] private string sceneName;
    public void SwitchScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}