using UnityEngine;

public class GlobalAmbientControl : MonoBehaviour
{
    public Material[] objectMaterials;  // List of materials to update
    private bool isDay = true;          // Start with day mode

    void Start()
    {
        // Initialize the materials array if needed
        if (objectMaterials.Length == 0)
        {
            objectMaterials = FindObjectMaterials();
        }
    }

    void Update()
    {
        // Toggle day/night mode with the "T" key
        if (Input.GetKeyDown(KeyCode.T))
        {
            isDay = !isDay;
            float intensity = isDay ? 1.0f : 0.2f;  // Day: Bright, Night: Dim

            // Update ambient intensity for all materials at once
            SetAmbientIntensity(intensity);
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

    // Find all material instances in the scene that use the custom shader
    Material[] FindObjectMaterials()
    {
        Renderer[] renderers = FindObjectsOfType<Renderer>();
        Material[] materials = new Material[renderers.Length];
        
        for (int i = 0; i < renderers.Length; i++)
        {
            materials[i] = renderers[i].sharedMaterial;
        }
        return materials;
    }
}
