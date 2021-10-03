using System.Collections;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string Name;
    public Animator Animator;

    protected string SelectState = "Select";
    protected string DeselectState = "Deselect";
    protected string InteractState = "Interact";
    protected string ReleaseState = "Release";
    protected string GiveState = "Give";
    protected string TakeState = "Take";

    public void Interact()
    {
        Animator.CrossFade(InteractState, 1f);
    }

    public void Release()
    {
        Animator.Play(ReleaseState);
    }

    public void Give()
    {
        Animator.Play(GiveState);
    }

    public void Take()
    {
        Animator.Play(TakeState);
    }

    public void Select()
    {
        Animator.Play(SelectState);
    }

    public void Deselect()
    {
        Animator.Play(DeselectState);
    }
}
