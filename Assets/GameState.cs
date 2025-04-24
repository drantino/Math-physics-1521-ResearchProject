using UnityEngine;

public class GameState : MonoBehaviour
{
    public enum PhysicsType
    {
        HardCoded,
        ForcePhysics,
        CodePhyics
    }
    public PhysicsType physics;
}
