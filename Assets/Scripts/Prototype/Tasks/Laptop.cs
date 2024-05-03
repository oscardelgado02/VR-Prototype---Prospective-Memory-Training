using UnityEngine;

public class Laptop : TaskObject
{
    [SerializeField] private GameObject emailScreen;
    [SerializeField] private GameObject sentScreen;

    // Methods
    private void SendEmail()
    {
        if (GetIfTaskCanBeDone())
        {
            ChangeScreenContent(true);

            FinishTask();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        // Left Hand case
        if (collider.gameObject.tag.Equals("LeftHandCollider"))
            SendEmail();

        // Right Hand case
        if (collider.gameObject.tag.Equals("RightHandCollider"))
            SendEmail();
    }

    private void ChangeScreenContent(bool emailSent)
    {
        // Show/hide the email screen
        emailScreen.SetActive(!emailSent);

        // Show/hide the email screen
        sentScreen.SetActive(emailSent);
    }

    private void Start() { ChangeScreenContent(false); }
}
