using UnityEngine;

public class GameManager : LazySingleton<GameManager>
{
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private Block baseBlock;

    private Block lastBlock;
    private Block currentBlock;

    private bool isMovingOnX;
    private const float spawnOffset = 3.0f;


    private void Awake()
    {
        EventManager.Instance.OnTapTouchLayer += HandlePressScreen;
    }

    private void Start()
    {
        isMovingOnX = Random.value < 0.5f;
        lastBlock = baseBlock;
        SpawnBlock();
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnTapTouchLayer -= HandlePressScreen;
    }

    private void SpawnBlock()
    {
        if (currentBlock != null)
        {
            currentBlock.IsMoving = false;
            lastBlock = currentBlock;
        }

        Vector3 lastBlockScale = lastBlock.transform.localScale;
        Vector3 lastBlockPosition = lastBlock.transform.position;
        float spawnY = lastBlockPosition.y + lastBlockScale.y / 2.0f;

        Vector3 newBlockSpawnPosition = isMovingOnX
            ? new Vector3(lastBlockPosition.x - spawnOffset, spawnY, lastBlockPosition.z)
            : new Vector3(lastBlockPosition.x, spawnY, lastBlockPosition.z - spawnOffset);

        GameObject newBlockGO = BlockGenerator.Instance.GenerateBlock(newBlockSpawnPosition);
        Block newBlock = newBlockGO.GetComponent<Block>();
        newBlock.IsMoving = true;
        newBlock.IsMovingOnX = isMovingOnX;

        currentBlock = newBlock;
    }

    private void HandlePressScreen()
    {
        SpawnBlock();

        float movementY = currentBlock.transform.localScale.y;
        CameraManager.Instance.MoveCameraUp(movementY);
    }
}