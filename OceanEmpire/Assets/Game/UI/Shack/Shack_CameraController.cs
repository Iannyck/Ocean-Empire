using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shack_CameraController : MonoBehaviour
{
    public enum Section { Recolte = -1, Hub = 0, Shop = 1 }

    public GuidedAccelerationHandler cameraAcceleration;

    [SerializeField] private Transform _calendarDestination;
    [SerializeField] private Transform _recolteDestination;
    [SerializeField] private Transform _shopDestination;
    [SerializeField] Section startSection;

    [Header("C'est le bordel!"), SerializeField]
    QuestPanel questPanel;

    public const int SECTION_MAX = 1;
    public const int SECTION_MIN = -1;
    private Section _currentSection;
    private Vector3 _currentDestination;
    private Transform _cameraT;
    private float _currentSpeed;

    void Awake()
    {
        PersistentLoader.LoadIfNotLoaded(() =>
        {
            _cameraT = PersistentCamera.GetCamera().transform;
            GoTo(startSection, true);
        });
    }

    public void MoveRight(bool instantaneous = false)
    {
        _currentSection = (Section)Mathf.Min(SECTION_MAX, (int)_currentSection + 1);
        UpdateCameraDestination();

        if (instantaneous)
            InstantaneousMoveToDestination();
    }

    public void MoveLeft(bool instantaneous = false)
    {
        _currentSection = (Section)Mathf.Max(SECTION_MIN, (int)_currentSection - 1);
        UpdateCameraDestination();

        if (instantaneous)
            InstantaneousMoveToDestination();
    }

    public void GoTo(Section section, bool instantaneous = false)
    {
        _currentSection = (Section)Mathf.Clamp((int)section, SECTION_MIN, SECTION_MAX);
        UpdateCameraDestination();

        if (instantaneous)
            InstantaneousMoveToDestination();
    }

    void UpdateCameraDestination()
    {
        _currentDestination = GetSectionPosition(_currentSection);

        if (_currentSection == Section.Hub)
            questPanel.UpdateContent();
    }

    void InstantaneousMoveToDestination()
    {
        _cameraT.position = _currentDestination;
    }

    public Section CurrentSection
    {
        get { return _currentSection; }
    }

    public float CameraSectionPosition
    {
        get
        {
            float camX = _cameraT.position.x;
            float previousSectionX = float.PositiveInfinity;

            for (int i = SECTION_MIN; i <= SECTION_MAX; i++)
            {
                float sectionX = GetSectionPosition((Section)i).x;
                if (camX < sectionX)
                {
                    if (previousSectionX == float.PositiveInfinity)
                        return SECTION_MIN;
                    return (camX - previousSectionX) / (sectionX - previousSectionX) + i - 1;
                }
                else
                {
                    previousSectionX = sectionX;
                }
            }
            return SECTION_MAX;
        }
    }

    public Vector3 GetSectionPosition(Section section)
    {
        switch (section)
        {
            case Section.Hub:
                return _calendarDestination.position;
            case Section.Recolte:
                return _recolteDestination.position;
            case Section.Shop:
                return _shopDestination.position;
            default:
                return Vector3.zero;
        }
    }

    void Update()
    {
        if (_cameraT == null)
            return;

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
        pos.y = _currentDestination.y;
        _cameraT.position = pos;
    }
}
