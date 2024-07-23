using Game.Entity;
using UnityEngine;

public class Health : MonoBehaviour
{
   /*
   public float health;

    void Update()
    {
        
        if(health<=0)
        {
            Debug.Log("I am dead, boi :(");
            Destroy(gameObject);
        }
        
        if(health > 0f)
        {
            Player player = gameObject.GetComponent<Player>();
            //player._health += health;
            health = 0f;
        }
    }*/

    void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("banana"))
        {
            Debug.Log("yummy banana");
            //health += 10f;
            Player player = gameObject.GetComponent<Player>();
            player._health += 10;
            collider.GetComponent<Banana>().RemoveBanana();
        }
    }
}
