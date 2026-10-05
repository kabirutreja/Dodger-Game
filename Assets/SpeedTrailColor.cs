using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class SpeedTrailColor : MonoBehaviour
{
    [Header("References")]
    public PlayerController player;      // drag your player controller in
    private TrailRenderer trail;
    private Material trailMaterial;      // runtime instance, safe to edit

    [Header("Speed Range")]
    public float minSpeed = 0f;
    public float maxSpeed = 10f;

    [Header("Colors")]
    public Color slowColor = Color.white;
    public Color fastColor = Color.red;

    [Header("Shader Property")]
    public string colorProperty = "_Color"; // use "_BaseColor" for URP Unlit/Lit shaders

    void Awake()
    {
        trail = GetComponent<TrailRenderer>();
        trailMaterial = trail.material; // instantiates a copy automatically
    }

    void Update()
    {
        float speed = player.HorizontalVelocity.magnitude;
        float t = Mathf.InverseLerp(minSpeed, maxSpeed, speed);

        Color current = Color.Lerp(slowColor, fastColor, t);
        trailMaterial.SetColor(colorProperty, current);
    }
}