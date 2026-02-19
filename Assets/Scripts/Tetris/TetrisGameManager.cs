using UnityEngine;

public class TetrisGameManager : MonoBehaviour
{
    public static TetrisGameManager instance;
    [SerializeField] private TetraminoController controller;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;
    }

    public void GameOver()
    {
        controller.gameObject.SetActive(false);
        Debug.Log("Game Over penco");
    }
}
