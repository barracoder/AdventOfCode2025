using System;
using System.Numerics;

float Difference3D(Vector3 a, Vector3 b)
{
    return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z);
}