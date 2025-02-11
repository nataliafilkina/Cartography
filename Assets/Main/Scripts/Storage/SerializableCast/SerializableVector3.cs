using UnityEngine;

public struct SerializableVector3
{
    public float x;
    public float y;
    public float z;

    public SerializableVector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static implicit operator Vector3(SerializableVector3 s) => new Vector3(s.x, s.y, s.z);
    public static implicit operator SerializableVector3(Vector3 v) => new SerializableVector3(v.x, v.y, v.z);
}

