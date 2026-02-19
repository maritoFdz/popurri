using UnityEngine;

public class TetraminoController : MonoBehaviour
{
    [SerializeField] private Board board;
    [Header("Fall Parameters")]
    [SerializeField] private float fallTime;
    [SerializeField] private float constFallMult;
    private float fallMult;
    private float fallTimer;
    private Tetramino currentTetra;

    private void Awake()
    {
        fallMult = 1.0f;
    }

    private void Update()
    {
        if (currentTetra == null) return;
        HandleInput();
        fallTimer += Time.deltaTime;
        if (fallTimer >= fallTime * fallMult)
        {
            fallTimer = 0;
            board.TryMoveDown(currentTetra);
        }
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