using ringo.SceneSystem;
using ringo.SceneSystem.Helpers;
using UnityEngine;

public class TestSceneSwitcher : PersistentSingleton<TestSceneSwitcher>
{
    [SerializeField] private SceneGroup[] sceneGroups;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.Instance.LoadScene(sceneGroups[0]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.Instance.LoadScene(sceneGroups[1]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SceneManager.Instance.LoadScene(sceneGroups[2]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SceneManager.Instance.LoadScene(sceneGroups[3]);
        }
    }
}
