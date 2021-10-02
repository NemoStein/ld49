using UnityEngine;

public class DrawerInteractable : Interactable
{
    override protected Vector3 GetFinalPosition()
    {
        return transform.position + Vector3.back * 0.5f;
    }
    
    override protected Quaternion GetFinalRotation()
    {
        return transform.rotation;
    }

    override protected Vector3 GetFinalScale()
    {
        return transform.localScale;
    }
}
