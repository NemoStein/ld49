using System.Collections;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string Name;

    readonly float SelectionDuration = 0.25f;
    Coroutine SelectionAnimating;

    Vector3 TargetPosition;
    Quaternion TargetRotation;
    Vector3 TargetScale;

    Vector3 InitialPosition;
    Quaternion InitialRotation;
    Vector3 InitialScale;

    Vector3 FinalPosition;
    Quaternion FinalRotation;
    Vector3 FinalScale;

    protected abstract Vector3 GetFinalPosition();
    protected abstract Quaternion GetFinalRotation();
    protected abstract Vector3 GetFinalScale();

    void Start()
    {
        InitialPosition = transform.position;
        InitialRotation = transform.rotation;
        InitialScale = transform.localScale;

        FinalPosition = GetFinalPosition();
        FinalRotation = GetFinalRotation();
        FinalScale = GetFinalScale();
    }

    public void Interact()
    {
        Deselect();
    }

    public void Give()
    {
        Deselect();
    }

    public void Select()
    {
        TargetPosition = FinalPosition;
        TargetRotation = FinalRotation;
        TargetScale = FinalScale;

        SelectionAnimating = StartCoroutine(SelectionAnimation());
    }

    public void Deselect()
    {
        TargetPosition = InitialPosition;
        TargetRotation = InitialRotation;
        TargetScale = InitialScale;

        SelectionAnimating = StartCoroutine(SelectionAnimation());
    }

    IEnumerator InteractionAnimation()
    {
        yield return null;
    }

    IEnumerator SelectionAnimation()
    {
        if (SelectionAnimating != null)
        {
            StopCoroutine(SelectionAnimating);
        }

        var initialPosition = transform.position;
        var initialRotation = transform.rotation;
        var initialScale = transform.localScale;

        var time = 0f;
        while (time < 1f)
        {
            time += 1 / SelectionDuration * Time.deltaTime;
            var ease = EaseOutCubic(0, 1, time);

            transform.position = Vector3.Lerp(initialPosition, TargetPosition, ease);
            transform.rotation = Quaternion.Lerp(initialRotation, TargetRotation, ease);
            transform.localScale = Vector3.Lerp(initialScale, TargetScale, ease);

            yield return null;
        }

        SelectionAnimating = null;
    }

    static float EaseOutCubic(float start, float end, float value)
    {
        value--;
        end -= start;
        return end * (value * value * value + 1) + start;
    }
}
