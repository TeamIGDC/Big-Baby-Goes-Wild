using UnityEngine;

public class Banana : MonoBehaviour
{
    [SerializeField] LightingManager lightingManager;

    void Start()
    {
        lightingManager = FindObjectOfType<LightingManager>();
    }

    void Update()
    {
        if(lightingManager.timeOfDay >= 14f)
        {
            RemoveBanana();
        }
    }

    public void RemoveBanana()
    {
        Destroy(gameObject);
    }
}
