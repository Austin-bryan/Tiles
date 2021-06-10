using System;
using UnityEngine;
using ExtensionMethods;
using System.Collections.Generic;
using static PathDebugger;

public class Rotator : MonoBehaviour 		
{
    public float Angle { get; set; }
    public RotateDirection Direction { get; set; }

    public Action OnFinished;
    public List<Coord> Coords;
    public bool HasCenter;
    public int SkippingNumber = 1;

    private bool IsClockwise => Direction == RotateDirection.Clockwise;
    private float speed = 10, totalAngle = 0;
    private bool isDestroying;

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    private void Update()
    {
        if (isDestroying) return;
        
        float z = transform.eulerAngles.z;

        totalAngle += speed * (Angle < 1 ? -1 : 1);
        transform.eulerAngles = new Vector3(0, 0, totalAngle);

        if (totalAngle.Abs() < Angle.Abs()) return;

        if (HasCenter)
        {
            HasCenter = false;
            this.DetatchLastChild();
        }

        int count = transform.childCount;
        int i     = IsClockwise ? 0 : count - 1;
        int tileCount = 0;

        while (tileCount < count)
        {
            int startI = i;
            
            while (true)
            {
                int j = i;
                i += SkippingNumber * (IsClockwise ? 1 : -1);

                if (i < 0 || i > count - 1) i = startI;
                var c = Coords[i];

                tileCount++;

                this.GetChild(j).Get<PlayerTile>().SetCoord(c);

                if (i == startI || tileCount >= count) break;
            }
            
            i += IsClockwise ? 1 : -1;
        }

        transform.DetachChildren();
        isDestroying = true;

        this.Delay(2f, () => Destroy(this));
    }
}