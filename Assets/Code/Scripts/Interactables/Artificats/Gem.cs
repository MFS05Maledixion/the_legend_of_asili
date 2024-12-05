using UnityEngine;
public class Gem : MonoBehaviour, IInteractable
{
    private Animator _gemAnimator;
    private bool _hasPickedUpGem = false;

    public void Interact()
    {
        if (!_hasPickedUpGem)
        {
            GemsManager.Instance.GemCollected();
            _hasPickedUpGem = true;
            Invoke(nameof(GetRidOfObject), 1f);
        }
    }

    private void GetRidOfObject()
    {
        Destroy(gameObject);
    }

    public void OnPlayerApproach()
    {
        if (_gemAnimator != null)
        {
            _gemAnimator.SetBool("IsNear", true);
        }
        HintTextManager hintTextManager = FindFirstObjectByType<HintTextManager>();
        if (hintTextManager != null)
        {
            hintTextManager.ShowHint("E to Collect (get close)");
        }
    }

    public void OnPlayerLeave()
    {
        if (_gemAnimator != null)
        {
            _gemAnimator.SetBool("IsNear", false);
        }
        HintTextManager hintTextManager = FindFirstObjectByType<HintTextManager>();
        if (hintTextManager != null)
        {
            hintTextManager.HideHint();
        }
    }
}
