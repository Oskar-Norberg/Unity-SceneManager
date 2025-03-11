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
            SceneManager.LoadSceneGroup(sceneGroups[0]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadSceneGroup(sceneGroups[1]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SceneManager.LoadSceneGroup(sceneGroups[2]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SceneManager.LoadSceneGroup(sceneGroups[3]);
        }
    }
}
