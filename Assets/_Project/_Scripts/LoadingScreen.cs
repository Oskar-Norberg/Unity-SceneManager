using System;
using UnityEngine;
using UnityEngine.UI;
using SceneManager = ringo.SceneSystem.SceneManager;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;

    private void OnEnable()
    {
        SceneManager.OnSceneLoadingStarted += OnSceneLoadingStarted;
        SceneManager.OnSceneLoadingFinished += OnSceneLoadingFinished;
    }

    private void OnDisable()
    {
        SceneManager.OnSceneLoadingStarted -= OnSceneLoadingStarted;
        SceneManager.OnSceneLoadingFinished -= OnSceneLoadingFinished;
    }

    private void OnSceneLoadingStarted()
    {
        backgroundImage.enabled = true;
    }
    
    private void OnSceneLoadingFinished()
    {
        backgroundImage.enabled = false;
    }
}
