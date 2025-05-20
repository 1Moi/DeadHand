using UnityEngine;

public class BlackHoleAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void PlayBlackHole()
    {
        if (animator != null)
            animator.Play("TrouNoir");
        else
            Debug.LogError("Animator non assigné dans BlackHoleAnimator !");
    }
}
