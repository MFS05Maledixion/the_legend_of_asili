using UnityEngine;
using TMPro;

public class Portal : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform _newSpawnLocation;
    [SerializeField] private GameObject _prevLevel;
    [SerializeField] private GameObject _nextLevel;
    [SerializeField] private bool _isLastPortal;
    [SerializeField] private AudioClip teleportSound; // teleport sound
    private GameObject _player; // reference player


    private void OnEnable()
    {
        _player = GameObject.FindWithTag("Player");
    }

    private bool CanActivatePortal()
    {
        int gemsCollected = GemsManager.Instance.GemsCollected;
        int gemsToCollect = GemsManager.Instance.GemsToCollect;
        return gemsCollected == gemsToCollect;
    }
    public void Interact()
    {
        if (CanActivatePortal())
        {
            GemsManager.Instance.CanActivatePortalFeedBack(gameObject);
            if (!_isLastPortal)
            {
                TeleportPlayer();
            }
            else
            {
                Debug.Log("This is the last level!");
            }
        }
        else
        {
            GemsManager.Instance.CannotActivatePortalFeedBack(gameObject);
        }
    }

    private void TeleportPlayer()
    {
        if (teleportSound != null && !SoundManager.Instance.interactableAudioSource.isPlaying)
        {
            SoundManager.Instance.interactableAudioSource.pitch = 0.7f; // Normal pitch for running
            SoundManager.Instance.PlaySound(teleportSound);
        }
        if (_player != null)
        {
            _player.transform.position = _newSpawnLocation.position;
            Debug.Log($"Player Teleported to: {_newSpawnLocation.position}");
        }
    }

    private void LevelsHandler()
    {
        // we wont do it here: Just remember to set all levels inactive except Level1. 
        if (_prevLevel != null)
        {
            _prevLevel.SetActive(false);
        }
        if (_nextLevel != null)
        {
            _nextLevel.SetActive(true);
        }
    }


    public void OnPlayerApproach()
    {
        HintTextManager hintTextManager = FindFirstObjectByType<HintTextManager>();
        if (hintTextManager != null)
        {
            if (CanActivatePortal())
            {
                hintTextManager.ShowHint("E to Activate");
            }
            else if (CanActivatePortal() && _isLastPortal)
            {
                hintTextManager.ShowHint("Locked (Final Portal)");
            }
            else
            {
                hintTextManager.ShowHint("Collect all gems to activate");
            }
        }
    }

    public void OnPlayerLeave()
    {
        HintTextManager hintTextManager = FindFirstObjectByType<HintTextManager>();
        if (hintTextManager != null)
        {
            hintTextManager.HideHint();
        }
    }

}
