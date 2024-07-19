using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [SerializeField] Light directonalLight;
    [SerializeField] LightingPreset preset;

    [SerializeField, Range(0, 24)] float timeOfDay;

    void Update()
    {
        if(preset==null) return;

        if(Application.isPlaying) 
        {
            timeOfDay += Time.deltaTime;
            timeOfDay %= 24;
            UpdateLighting(timeOfDay/24f);
        } else {
            UpdateLighting(timeOfDay / 24f);
        }
    }

    void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = preset.ambientColor.Evaluate(timePercent);
        RenderSettings.fogColor = preset.fogColor.Evaluate(timePercent);

        if(directonalLight != null)
        {
            directonalLight.color = preset.directionalColor.Evaluate(timePercent);
            directonalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0f));
        }
    }

    void OnValidate()
    {
        if(directonalLight!=null) return;

        if(RenderSettings.sun != null)
        {
            directonalLight = RenderSettings.sun;
        } else {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach(Light light in lights)
            {
                if(light.type == LightType.Directional)
                {
                    directonalLight = light;
                    return;
                }
            }
        }
    }
}
