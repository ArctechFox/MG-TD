using UnityEngine;

public abstract class Buildable : MonoBehaviour
{
    public Transform AnchorPoint;

    public virtual void InitializeComponents(Transform AnchorPoint)
    {
        this.AnchorPoint = AnchorPoint;
    }
}
