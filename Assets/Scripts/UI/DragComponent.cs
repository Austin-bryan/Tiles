using UnityEngine;
using System.Linq;
using ExtensionMethods;	
using static PathDebugger;

public class DragModule : MonoBehaviour
{
    public CreatorLevelDialog[] MovingObject;
    public RectTransform BaseObject;
    public Vector3 Offset;

    public void Update() => MoveObject();
    public void MoveObject()
    {
        var pos = Input.mousePosition + Offset;
        pos.z   = BaseObject.position.z;

        MovingObject.Where(x   => x.DragEnabled).ToList()
                    .ForEach(x => x.gameObject.transform.position = Camera.main.ScreenToWorldPoint(pos) + x.Offset);
    }
}
