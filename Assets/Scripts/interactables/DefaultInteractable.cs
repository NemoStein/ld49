using UnityEngine;

public class DefaultInteractable : Interactable
{
    override protected Vector3 GetFinalPosition()
    {
        return transform.position + Vector3.up * 0.25f;
    }
    
    override protected Quaternion GetFinalRotation()
    {
        return transform.rotation * Quaternion.Euler(Vector3.forward * -15f);
    }

    override protected Vector3 GetFinalScale()
    {
        return transform.localScale * 1.1f;
    }
}
