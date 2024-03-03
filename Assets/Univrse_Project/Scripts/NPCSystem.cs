using System.Collections;
using UnityEngine;
using Inworld;

[RequireComponent(typeof(InworldCharacter))]

public class NPCSystem : MonoBehaviour
{
    #region Vars
    InworldCharacter m_CurrentCharacter;
    bool playerTrigger = false;
    bool conversationEnded = true;
    #endregion

    private enum CharacterTriggers
    {
        greetings,
        goodbye
    }
    #region Methods
    private void Start() {
        m_CurrentCharacter = GetComponent<InworldCharacter>();
    }
    private void Update() {
        if (conversationEnded) {
            InworldController.Audio.StopRecording();
        }
    }
    #region DetectPlayer
    private void OnTriggerEnter(Collider coll) {
        if (coll.GetComponent<Player>() == null)
            return;

        PlayerEnter();

    }
    private void OnTriggerExit(Collider coll) {

        if (coll.GetComponent<Player>() == null)
            return;

        PlayerExit();
    }
    private void PlayerEnter() {
        if (playerTrigger == false) {
            
            conversationEnded = false;
            playerTrigger = true;
            m_CurrentCharacter.SendTrigger(CharacterTriggers.greetings.ToString());

            InworldController.Audio.StartRecording();
        }
    }
    private void PlayerExit() {
        playerTrigger = false;
        m_CurrentCharacter.CancelResponse();
        if (InworldController.Audio.IsPlayerSpeaking) {
            StartCoroutine(WaitLastResponse());
        }
        else {
            InworldController.Audio.StopRecording();
            m_CurrentCharacter.CancelResponse();
            m_CurrentCharacter.SendTrigger(CharacterTriggers.goodbye.ToString());
        }
    }
    IEnumerator WaitLastResponse() {

        yield return new WaitWhile(() => (InworldController.Audio.IsPlayerSpeaking));
        m_CurrentCharacter.SendTrigger(CharacterTriggers.goodbye.ToString());
        conversationEnded = true;
    }
    #endregion
    #endregion
}