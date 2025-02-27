using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        Input.backButtonLeavesApp = true;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void LoadNewScene(int index)
    {
        AsyncOperation loadOperation;
        loadOperation = SceneManager.LoadSceneAsync(index);
        loadOperation.completed += _ => SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(index));
    }
}
