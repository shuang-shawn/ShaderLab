using UnityEngine;

public class GlobalAmbientControl : MonoBehaviour
{
    public Material[] objectMaterials;  // List of materials to update
    public UnityEngine.KeyCode fogToggleKey = KeyCode.F;
    public UnityEngine.KeyCode dayNightToggleKey = KeyCode.T;
    public UnityEngine.KeyCode flashlightToggleKey = KeyCode.Y;
    public Transform playerTransform;



    private bool isDay = true;
    private bool isFogOn = false;          
    private bool isFlashlightOn = false;

    void Start()
    {
        // Initialize the materials array if needed
        
    }

    void Update()
    {
        // Toggle day/night mode with the "T" key
        if (Input.GetKeyDown(dayNightToggleKey))
        {
            isDay = !isDay;
            float intensity = isDay ? 1.0f : 0.2f;  // Day: Bright, Night: Dim

            // Update ambient intensity for all materials at once
            SetAmbientIntensity(intensity);
        }

        if (Input.GetKeyDown(fogToggleKey))
        {
            isFogOn = !isFogOn;

            // Update fog setting for all materials
            SetFogEffect(isFogOn);
        }

        if (Input.GetKeyDown(flashlightToggleKey))
        {
            isFlashlightOn = !isFlashlightOn;
            SetFlashlightState(isFlashlightOn);
        }
        if (isFlashlightOn) {
            foreach (var mat in objectMaterials)
            {
                mat.SetVector("_PlayerPosition", playerTransform.position);
            }
        }


    }

        void SetFlashlightState(bool isOn)
    {

        float flashlightValue = isOn ? 1.0f : 0.0f;
        foreach (var mat in objectMaterials)
        {
            mat.SetFloat("_UseFlashlight", flashlightValue);
        }
    }

    void SetFogEffect(bool enableFog)
    {
        float fogValue = enableFog ? 1.0f : 0.0f;
        foreach (var mat in objectMaterials)
        {
            mat.SetFloat("_UseFog", fogValue);
        }
    }

    // Update the ambient intensity of all material instances
    void SetAmbientIntensity(float intensity)
    {
        foreach (var mat in objectMaterials)
        {
            mat.SetFloat("_AmbientIntensity", intensity);
        }
    }


}
