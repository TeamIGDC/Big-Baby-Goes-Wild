using UnityEngine;

public class Health : MonoBehaviour
{
    public int health;

    void Update()
    {
        if(health<=0)
        {
            Debug.Log("I am dead, boi :(");
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("banana"))
        {
            Debug.Log("yummy banana");
            health += 10;
            collider.GetComponent<Banana>().RemoveBanana();
        }
    }
}
