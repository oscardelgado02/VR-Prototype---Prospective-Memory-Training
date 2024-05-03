using UnityEngine;

public class Food : TaskObject
{
    // Attributes
    [StringInList(typeof(PropertyDrawersHelper), "FoodTagList")]
    [SerializeField] private string foodTag;

    // Methods
    private void EatFood()
    {
        if (GetIfTaskCanBeDone())
        {
            FinishTask();

            // In case the eaten food is a coffee
            if (foodTag.Equals(labels.Instance._foodTags[(int)foodIds.coffee]))
            {
                // We get the empty coffee material from the coffee MeshRenderer
                MeshRenderer meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
                Material mat = meshRenderer.material;

                // We change the materials so it only remains the empty coffee material
                Material[] newMaterials = new Material[] { mat };
                meshRenderer.materials = newMaterials;
            }
            // In other case
            else
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
