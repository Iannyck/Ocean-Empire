using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shack_CameraController : MonoBehaviour
{
    public enum Section { Calendar = -1, Recolte = 0, Shop = 1 }

    public GuidedAccelerationHandler cameraAcceleration;

    [SerializeField] private Transform _calendarDestination;
    [SerializeField] private Transform _recolteDestination;
    [SerializeField] private Transform _shopDestination;

    private const int SECTION_MAX = 1;
    private const int SECTION_MIN = -1;
    private Section _currentSection;
    private Vector3 _currentDestination;
    private Transform _cameraT;
    private float _currentSpeed;

    void Awake()
    {
        PersistentLoader.LoadIfNotLoaded(() =>
        {
            _cameraT = PersistentCamera.GetCamera().transform;
        });
    }

    public void MoveRight()
    {
        _currentSection = (Section)Mathf.Min(SECTION_MAX, (int)_currentSection + 1);
        UpdateCameraDestination();
    }

    public void MoveLeft()
    {
        _currentSection = (Section)Mathf.Min(SECTION_MAX, (int)_currentSection - 1);
        UpdateCameraDestination();
    }

    public void GoTo(Section section)
    {
        _currentSection = section;
        UpdateCameraDestination();
    }

    void UpdateCameraDestination()
    {
        switch (_currentSection)
        {
            case Section.Calendar:
                _currentDestination = _calendarDestination.position;
                break;
            case Section.Recolte:
                _currentDestination = _recolteDestination.position;
                break;
            case Section.Shop:
                _currentDestination = _shopDestination.position;
                break;
        }
    }

    void Update()
    {
        var deltaTime = Time.deltaTime;

        // Update acceleration
        cameraAcceleration.UpdateAcceleration(_currentDestination.x, _cameraT.position.x, _currentSpeed, deltaTime);

        // Update speed
        _currentSpeed += cameraAcceleration.CurrentAcceleration * deltaTime;
        // Update acceleration
        cameraAcceleration.UpdateAcceleration(_currentDestination.x, _cameraT.position.x, _currentSpeed, deltaTime);

        // Update position
        var pos = _cameraT.position;
        pos.x += _currentSpeed * deltaTime;
        _cameraT.position = pos;
    }
}
