using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPartTrigger : MonoBehaviour
{
    [SerializeField] private TutorialHandler tutorialHandler;
    [SerializeField] private string tutorialPartName;
    
    private void OnValidate()
    {
        if (tutorialHandler == null)
            tutorialHandler = FindObjectOfType<TutorialHandler>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialHandler.StartTutorialPart(tutorialPartName);
            Destroy(gameObject);
        }
    }
}
