using UnityEngine;

[ExecuteInEditMode]
public class HorizontalCameraController : MonoBehaviour
{
    private Camera Camera;
    private float LastAspect;

    [SerializeField] float m_fieldOfView = 60f;
    [SerializeField] float m_orthographicSize = 5f;
    public float FieldOfView
    {
        get { return m_fieldOfView; }
        set
        {
            if (m_fieldOfView != value)
            {
                m_fieldOfView = value;
                RefreshCamera();
            }
        }
    }

    public float OrthographicSize
    {
        get { return m_orthographicSize; }
        set
        {
            if (m_orthographicSize != value)
            {
                m_orthographicSize = value;
                RefreshCamera();
            }
        }
    }

    private void OnEnable()
    {
        RefreshCamera();
    }
    private void Update()
    {
        float aspect = Camera.aspect;
        if (aspect != LastAspect) AdjustCamera(aspect);
    }
    private void AdjustCamera(float aspect)
    {
        LastAspect = aspect;

        // Credit: https://forum.unity.com/threads/how-to-calculate-horizontal-field-of-view.16114/#post-2961964
        float _1OverAspect = 1f / aspect;
        Camera.fieldOfView = 2f * Mathf.Atan(Mathf.Tan(m_fieldOfView * Mathf.Deg2Rad * 0.5f) * _1OverAspect) * Mathf.Rad2Deg;
        Camera.orthographicSize = m_orthographicSize * _1OverAspect;
    }
    public void RefreshCamera()
    {
        if (Camera == null) Camera = GetComponent<Camera>();
        AdjustCamera(Camera.aspect);
    }


#if UNITY_EDITOR
    private void OnValidate()
    {
        RefreshCamera();
    }
#endif
}