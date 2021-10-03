using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string Name;
    public Animator Animator;

    public bool Interacting;
    public bool Given;

    int SelectionLayer;
    int ActivationLayer;
    int InteractionLayer;

    protected string SelectionLayerName = "Selection";
    protected string ActivationLayerName = "Activation";
    protected string InteractionLayerName = "Interaction";

    protected string IdleState = "Idle";
    protected string SelectState = "Select";
    protected string DeselectState = "Deselect";
    protected string InteractState = "Interact";
    protected string ReleaseState = "Release";
    protected string GiveState = "Give";
    protected string TakeState = "Take";
    protected string ActivateState = "Activate";

    public float MinIdleTime = 0;
    public float MaxIdleTime = 9;
    public float MinActivationChance = 0;
    public float MaxActivationChance = 1;

    float CurrentTime;
    float ElapsedTime;
    float LastActivationTime;

    bool Active;

    void Start()
    {
        SelectionLayer = Animator.GetLayerIndex(SelectionLayerName);
        ActivationLayer = Animator.GetLayerIndex(ActivationLayerName);
        InteractionLayer = Animator.GetLayerIndex(InteractionLayerName);
    }

    public void Select()
    {
        Animator.Play(SelectState, SelectionLayer);
    }

    public void Deselect()
    {
        Animator.Play(DeselectState, SelectionLayer);
    }

    public void Interact()
    {
        Interacting = true;
        Animator.Play(InteractState, InteractionLayer);
        Animator.Play(DeselectState, SelectionLayer);
        Animator.Play(IdleState, ActivationLayer);
    }

    public void Release()
    {
        Active = false;
        Interacting = false;
        LastActivationTime = CurrentTime;
        Animator.Play(ReleaseState, InteractionLayer);
    }

    public void Give()
    {
        Given = true;
        Animator.Play(GiveState, InteractionLayer);
    }

    public void Take()
    {
        Given = false;
        Animator.Play(TakeState, InteractionLayer);
    }

    public void Activate()
    {
        Active = true;
        Animator.Play(ActivateState, ActivationLayer);
    }

    public void UpdateTime(float currentTime)
    {
        UpdateTime(currentTime, false);
    }

    public void UpdateTime(float currentTime, bool setup)
    {
        CurrentTime = currentTime;
        ElapsedTime = CurrentTime - LastActivationTime;

        if (setup)
        {
            LastActivationTime = CurrentTime;
        }
    }

    public float GetActivationChance()
    {
        if (Active) return 0;

        if (ElapsedTime <= MinIdleTime) return MinActivationChance;
        if (ElapsedTime >= MaxIdleTime) return MaxActivationChance;

        return (ElapsedTime - MinIdleTime) / (MaxIdleTime - MinIdleTime) * (MaxActivationChance - MinActivationChance) + MinActivationChance;
    }
}
