using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CCC.UI.Animation;
using UnityEngine.Events;

public class Shack_CallToAction : MonoBehaviour
{
    [Header("Optional Components")]
    [SerializeField] FloatingAnimation _floatingAnimation;
    [SerializeField] Image[] _images;
    [SerializeField] GameObject[] _gameObjects;

    [Header("Events")]
    [SerializeField] UnityEvent _onEnable = new UnityEvent();
    [SerializeField] UnityEvent _onDisable = new UnityEvent();

    public UnityEvent OnEnableEvent
    {
        get { return _onEnable; }
    }
    public UnityEvent OnDisableEvent
    {
        get { return _onDisable; }
    }

    void Awake()
    {
        if (!enabled)
            OnDisable();
    }

    void OnEnable()
    {
        SetActiveComponents(true);
        _onEnable.Invoke();
    }

    void OnDisable()
    {
        SetActiveComponents(false);
        _onDisable.Invoke();
    }

    void SetActiveComponents(bool state)
    {
        if (_images != null)
            foreach (var image in _images)
            {
                if (image)
                    image.enabled = state;
            }

        if (_gameObjects != null)
            foreach (var gameObj in _gameObjects)
            {
                if (gameObj)
                    gameObj.SetActive(state);
            }

        if (_floatingAnimation)
            _floatingAnimation.enabled = state;
    }
}
