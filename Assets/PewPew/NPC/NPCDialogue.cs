using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    [Header("NPC")]
    public string npcName = "Wise Duck";
    [TextArea(2, 5)]
    public List<string> lines = new List<string>
    {
        "Defeat the enemies to win.",
        "Press E to equip weapons when you are close to them and press Q to unequip when you have a weapon."
    };

    [Header("Interaction")]
    public KeyCode interactKey = KeyCode.E;
    public string playerTag = "Player";
    public bool rotateTowardPlayer = true;
    public float rotateSpeed = 8f;

    [Header("UI References")]
    public CanvasGroup talkPrompt;
    public CanvasGroup dialoguePanel;
    public TMP_Text nameText;
    public TMP_Text bodyText;

    [Header("Optional: Disable while talking")]
    [Tooltip("Drag components like ThirdPersonController, StarterAssetsInputs here to disable during dialogue.")]
    public MonoBehaviour[] disableWhileTalking;

    public static bool IsAnyDialogueOpen { get; private set; }

    private bool _playerInRange;
    private bool _talking;
    private int _index;
    private Transform _player;

    private void Reset()
    {
        var col = GetComponent<Collider>();
        if (col == null) col = gameObject.AddComponent<SphereCollider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            _playerInRange = true;
            _player = other.transform;
            Show(talkPrompt, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            _playerInRange = false;
            _player = null;
            Show(talkPrompt, false);
            if (_talking) EndDialogue();
        }
    }

    private void Update()
    {
        if (_playerInRange && Input.GetKeyDown(interactKey))
        {
            if (!_talking) StartDialogue();
            else NextLine();
        }

        if (_talking && rotateTowardPlayer && _player != null)
        {
            Vector3 dir = (_player.position - transform.position);
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.0001f)
            {
                Quaternion t = Quaternion.LookRotation(dir.normalized, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, t, Time.deltaTime * rotateSpeed);
            }
        }
    }

    private void StartDialogue()
    {
        if (lines == null || lines.Count == 0) return;

        _talking = true;
        IsAnyDialogueOpen = true;
        _index = 0;

        Show(talkPrompt, false);
        Show(dialoguePanel, true);

        if (nameText) nameText.text = npcName;
        if (bodyText) bodyText.text = lines[_index];

        SetDisabled(true);
        // Optional: unlock cursor if you want a mouse-driven UI
        // Cursor.lockState = CursorLockMode.None; Cursor.visible = true;
    }

    private void NextLine()
    {
        _index++;
        if (_index >= lines.Count)
        {
            EndDialogue();
            return;
        }
        if (bodyText) bodyText.text = lines[_index];
    }

    private void EndDialogue()
    {
        _talking = false;
        IsAnyDialogueOpen = false;

        Show(dialoguePanel, false);
        if (_playerInRange) Show(talkPrompt, true);

        SetDisabled(false);
        // Optional: re-lock cursor
        // Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false;
    }

    private void Show(CanvasGroup cg, bool on)
    {
        if (!cg) return;
        cg.alpha = on ? 1f : 0f;
        cg.interactable = on;
        cg.blocksRaycasts = on;
    }

    private void SetDisabled(bool disabled)
    {
        if (disableWhileTalking == null) return;
        foreach (var m in disableWhileTalking)
        {
            if (m) m.enabled = !disabled;
        }
    }
}