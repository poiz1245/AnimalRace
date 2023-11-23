using Fusion;
using System.Numerics;

enum Buttons
{
    forward = 0,
    back = 1,
    right = 2,
    left = 3,
    drift = 4,
}
public struct NetworkInputData : INetworkInput
{
    public NetworkButtons buttons;
    //public Angle yaw;
    public float yaw; //수평각도    
}
