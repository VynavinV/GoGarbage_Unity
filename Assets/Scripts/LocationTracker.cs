using UnityEngine;
using TMPro; // Add this line

public class LocationTracker : MonoBehaviour
{
    public float smoothSpeed = 2.0f;

    // Add TextMeshProUGUI fields
    public TextMeshProUGUI compassRotationText;
    public TextMeshProUGUI objectRotationText;
    public MapLoader mapLoader;
    public double lat;
    public double lon;

    void FixedUpdate()
    {
        // Get the compass direction
        float compassDirection = Input.compass.trueHeading;

        // Calculate the target rotation
        Quaternion targetRotation = Quaternion.Euler(0, compassDirection, 0);

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * smoothSpeed);

        // Update the TextMeshProUGUI components
        if (compassRotationText != null)
        {
            compassRotationText.text = $"Compass Rotation: {compassDirection:F2}°";
        }
        if (objectRotationText != null)
        {
            objectRotationText.text = $"Object Rotation: {transform.rotation.eulerAngles.y:F2}°";
        }


    }

    void Start()
    {
        // Enable the compass
        Input.compass.enabled = true;

        // Set fixed timestep to achieve approximately 30 FPS
        Time.fixedDeltaTime = 1.0f / 30.0f;
    }
}
