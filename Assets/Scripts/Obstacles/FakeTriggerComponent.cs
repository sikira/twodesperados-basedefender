using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFakeTriggerComponent
{
    void ObjectEntered(GameObject obj, int triggerId);
    void ObjectExited(GameObject obj, int triggerId);
}
public class FakeTriggerComponent : MonoBehaviour
{
    public string ObjectTag = "Player";
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
        if (collider.gameObject.tag == ObjectTag)
        {
            receiver?.ObjectExited(collider.gameObject, triggerId);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == ObjectTag)
        {
            receiver?.ObjectEntered(collider.gameObject, triggerId);
        }
    }

}
