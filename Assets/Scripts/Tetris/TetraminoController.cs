using UnityEngine;

public class TetraminoController : MonoBehaviour
{
    [SerializeField] private Board board;
    [Header("Fall Parameters")]
    [SerializeField] private float fallTime;
    [SerializeField] private float constFallMult;
    private float fallMult;
    private float levelMult;
    private float fallTimer;
    private Tetramino currentTetra;

    private void Update()
    {
        if (currentTetra == null) return;
        HandleInput();
        fallTimer += Time.deltaTime;
        if (fallTimer >= fallTime * fallMult * levelMult)
        {
            fallTimer = 0;
            board.TryMoveDown(currentTetra);
        }
    }

    public void ResetController()
    {
        currentTetra = null;
        levelMult = 1;
        fallTimer = 0;
    }

    public void SetCurrentTetra(Tetramino tetramino)
    {
        currentTetra = tetramino;
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            board.TryMoveHorizontal(currentTetra, -1);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            board.TryMoveHorizontal(currentTetra, 1);
        else if (Input.GetKeyDown(KeyCode.Return))
            board.TryRotate(currentTetra);
        fallMult = Input.GetKey(KeyCode.DownArrow) ? constFallMult : 1;
    }
}