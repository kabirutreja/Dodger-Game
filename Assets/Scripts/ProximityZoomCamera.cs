using UnityEngine;

public class ProximityZoomCamera : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform target;          // the "lava" / interactable object
    private Camera cam;

    [Header("Trigger Distance")]
    public float triggerDistance = 5f;   // start zooming in once closer than this
    public float releaseDistance = 6f;   // must move this far away before zooming back out (hysteresis)

    [Header("Zoom Settings")]
    public float maxZoomMultiplier = 2f;
    public float zoomSpeed = 1.5f;       // higher = faster transition

    private float baseFOV;
    private float baseOrthoSize;
    private float currentZoomT;          // 0 = normal (1x), 1 = fully zoomed (max zoom)
    private bool isZooming;

    void Awake()
    {
        cam = GetComponent<Camera>();
        baseFOV = cam.fieldOfView;
        baseOrthoSize = cam.orthographicSize;
    }

    void Update()
    {
        if (player == null || target == null) return;

        float dist = Vector3.Distance(player.position, target.position);

        // Hysteresis: enter zoom below triggerDistance, only exit once beyond releaseDistance
        if (dist <= triggerDistance)
            isZooming = true;
        else if (dist >= releaseDistance)
            isZooming = false;

        float targetT = isZooming ? 1f : 0f;
        currentZoomT = Mathf.MoveTowards(currentZoomT, targetT, zoomSpeed * Time.deltaTime);

        ApplyZoom(currentZoomT);
    }

    void ApplyZoom(float t)
    {
        if (cam.orthographic)
        {
            // Zooming in = smaller orthographic size
            float targetSize = baseOrthoSize / maxZoomMultiplier;
            cam.orthographicSize = Mathf.Lerp(baseOrthoSize, targetSize, t);
        }
        else
        {
            // Zooming in = smaller FOV (narrower view = "closer" feeling)
            float targetFOV = baseFOV / maxZoomMultiplier;
            cam.fieldOfView = Mathf.Lerp(baseFOV, targetFOV, t);
        }
    }
}