using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inworld;

public class NPCSystem : MonoBehaviour
{
    bool playerTrigger = false;

    [SerializeField] InworldCharacter m_CurrentCharacter;

    bool conversationEnded = true;

    private void Update() {
        if (conversationEnded) {
            InworldController.Audio.StopRecording();         
            
        }
    }

    private void OnTriggerEnter(Collider coll) {
        if (playerTrigger == false) {

            conversationEnded = false;
            playerTrigger = true;
            m_CurrentCharacter.SendTrigger("greatings");

            InworldController.Audio.StartRecording();
        }

    }

    private void OnTriggerExit(Collider coll) {
        playerTrigger = false;

        m_CurrentCharacter.CancelResponse();
        if (InworldController.Audio.IsPlayerSpeaking) {
            StartCoroutine(WaitLastResponse());
        }
        else {
            InworldController.Audio.StopRecording();
            m_CurrentCharacter.CancelResponse();
            m_CurrentCharacter.SendTrigger("goodbye");
        }
    }

    IEnumerator WaitLastResponse() {

        yield return new WaitWhile(() => (InworldController.Audio.IsPlayerSpeaking));
        m_CurrentCharacter.SendTrigger("goodbye");
        conversationEnded = true;
    }
}
