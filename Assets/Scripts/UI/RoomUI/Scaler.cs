using UnityEngine;

public class Scaler : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    public bool scale = true;

    private void OnMouseOver()
    {
        if (scale)
            _animator.SetBool("Pressed", true);
    }
    private void OnMouseExit()
    {
        if (scale)
            _animator.SetBool("Pressed", false);
    }
}
