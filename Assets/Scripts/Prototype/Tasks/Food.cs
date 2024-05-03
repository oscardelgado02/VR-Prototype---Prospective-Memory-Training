using UnityEngine;

public class Food : TaskObject
{
    // Methods
    private void EatFood()
    {
        if (GetIfTaskCanBeDone())
        {
            FinishTask();

            Destroy(gameObject);    // Destroy the fruit once it has been eaten
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag.Equals("HeadCollider"))
        {
            EatFood();
        }
    }
}
