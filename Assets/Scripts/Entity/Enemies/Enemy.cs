using UnityEngine;

public class Enemy : MonoBehaviour
{
    // This is just to test

    [SerializeField] float speed;

    float countdown = 5f;

    void Update()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime);
        countdown -= Time.deltaTime;
        if(countdown == 0f)
        {
            Destroy(gameObject);
        }
    }
}
