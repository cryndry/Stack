using UnityEngine;

public class GameManager : LazySingleton<GameManager>
{
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private Block baseBlock;

    private Block lastBlock;
    private Block currentBlock;

    private bool isMovingOnX;
    private const float spawnOffset = 3.0f;


    void Start()
    {
        isMovingOnX = Random.value < 0.5f;
        lastBlock = baseBlock;
        SpawnBlock();
    }

    private void SpawnBlock()
    {
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

        if (currentBlock != null)
        {
            currentBlock.IsMoving = false;

            lastBlock = currentBlock;
            currentBlock = newBlock;
        }
    }
}