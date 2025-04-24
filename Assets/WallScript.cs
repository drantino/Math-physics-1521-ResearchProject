using UnityEngine;

public class WallScript : MonoBehaviour
{
    private GameState _gameState;

    private void Awake()
    {
        _gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
    }
    // Update is called once per frame
    void Update()
    {
        switch (_gameState.physics)
        {
            case GameState.PhysicsType.HardCoded:
                {
                    GetComponent<BoxCollider2D>().isTrigger = true;
                    break;
                }
            case GameState.PhysicsType.CodePhyics:
                {
                    GetComponent<BoxCollider2D>().isTrigger = false;
                    break;
                }
            case GameState.PhysicsType.ForcePhysics:
                {
                    GetComponent<BoxCollider2D>().isTrigger = false;
                    break;
                }
        }
    }
}
