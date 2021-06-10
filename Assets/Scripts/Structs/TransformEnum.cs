using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

public struct Transform
{
    public Vector3 Location;
    public Quaternion Rotation;
    public Vector3 Scale;

    public Transform(Vector3 location, Quaternion rotation, Vector3 scale)
    {
        Location = location;
        Rotation = rotation;
        Scale    = scale;
    }
    public Transform (int locX, int locY, Quaternion rotation, int scaleX, int scaleY)
    {
        Location = new Vector3(locX, locY);
        Rotation = rotation;
        Scale    = new Vector3(scaleX, scaleY);
    }

    public override string ToString() => $"Location: {Location} Rotation: {Rotation} Scale: {Scale}";
    public static Transform operator *(Transform a, float b) => new Transform(a.Location * b, a.Rotation, a.Scale * b);

    public Transform AddLocX(float x) { Location.AddX(x); return this; }
    public Transform AddLocY(float y) { Location.AddY(y); return this; }
    public Transform AddLocZ(float z) { Location.AddZ(z); return this; }
    public Transform SubLocX(float x) { Location.SubX(x); return this; }
    public Transform SubLocY(float y) { Location.SubY(y); return this; }
    public Transform SubLocZ(float z) { Location.SubZ(z); return this; }

    public Transform AddRotX(float x) { Rotation.AddX(x); return this; }
    public Transform AddRotY(float y) { Rotation.AddY(y); return this; }
    public Transform AddRotZ(float z) { Rotation.AddZ(z); return this; }
    public Transform SubRotX(float x) { Rotation.SubX(x); return this; }
    public Transform SubRotY(float y) { Rotation.SubY(y); return this; }
    public Transform SubRotZ(float z) { Rotation.SubZ(z); return this; }

    public Transform AddScaleX(float x) { Location.AddX(x); return this; }
    public Transform AddScaleY(float y) { Location.AddY(y); return this; }
    public Transform AddScaleZ(float z) { Location.AddZ(z); return this; }
    public Transform SubScaleX(float x) { Location.SubX(x); return this; }
    public Transform SubScaleY(float y) { Location.SubY(y); return this; }
    public Transform SubScaleZ(float z) { Location.SubZ(z); return this; }
}
