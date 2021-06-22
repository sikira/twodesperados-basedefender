using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IHittableObject
{
    int Health { get; }
    void HitMe(int power);
    void HealMe(int power);

}

public interface IFakeTriggerComponent
{
    void ObjectEntered(GameObject obj, int triggerId);
    void ObjectExited(GameObject obj, int triggerId);
    void ObjectStay(GameObject obj, int triggerId);
}
public class FakeTriggerComponent : MonoBehaviour
{
    public List<string> ObjectTag = new List<string> { "Player" };
    public int triggerId = 0;
    public IFakeTriggerComponent receiver;
    // public delegate void ObjectEntered(GameObject obj);
    // public ObjectEntered OnObjectEntered;

    void Start()
    {
        receiver = this.GetComponentInParent<IFakeTriggerComponent>();
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (ObjectTag.Contains(collider.gameObject.tag))
        {
            receiver?.ObjectExited(collider.gameObject, triggerId);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (ObjectTag.Contains(collider.gameObject.tag))
        {
            receiver?.ObjectEntered(collider.gameObject, triggerId);
        }
    }
    void OnTriggerStay2D(Collider2D collider)
    {
        if (ObjectTag.Contains(collider.gameObject.tag))
        {
            receiver?.ObjectStay(collider.gameObject, triggerId);
        }
    }

}
