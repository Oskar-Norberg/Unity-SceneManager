using ringo.SceneSystem;
using UnityEngine;

public class TestSceneSwitcher : MonoBehaviour
{
    [SerializeField] private SceneGroup[] sceneGroups;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene(sceneGroups[0]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene(sceneGroups[1]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SceneManager.LoadScene(sceneGroups[2]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SceneManager.LoadScene(sceneGroups[3]);
        }
    }
}
