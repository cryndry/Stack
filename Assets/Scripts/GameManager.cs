using UnityEngine;

public class GameManager : LazySingleton<GameManager>
{
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private Block baseBlock;

    private Block lastBlock;
    private Block currentBlock;

    private bool isMovingOnX;
    private const float spawnOffset = 3.0f;
    private const float perfectHitThreshold = 0.1f;


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
        if (currentBlock != null)
        {
            newBlock.transform.localScale = lastBlock.transform.localScale;
        }

        currentBlock = newBlock;
    }

    private void HandlePressScreen()
    {
        bool areBlocksIntersecting = SplitBlock();
        if (areBlocksIntersecting)
        {
            SpawnBlock();

            float movementY = currentBlock.transform.localScale.y;
            CameraManager.Instance.MoveCameraUp(movementY);
        }
        else
        {
            Debug.Log("Game Over!");
        }
    }

    private bool SplitBlock()
    {
        if (currentBlock == null) return false;

        // Get Transforms
        Transform currentT = currentBlock.transform;
        Transform lastT = lastBlock.transform;

        // Get Positions and Scales
        Vector3 currentPos = currentT.position;
        Vector3 lastPos = lastT.position;
        Vector3 currentScale = currentT.localScale;

        float delta; // The distance between centers
        float maxDimension; // The width of the platform underneath

        // Determine Axis
        if (isMovingOnX)
        {
            delta = currentPos.x - lastPos.x;
            maxDimension = lastT.localScale.x;
        }
        else
        {
            delta = currentPos.z - lastPos.z;
            maxDimension = lastT.localScale.z;
        }

        // 1. Check for PERFECT HIT
        if (Mathf.Abs(delta) < perfectHitThreshold * maxDimension)
        {
            currentT.position = new Vector3(lastPos.x, currentPos.y, lastPos.z);
            return true;
        }

        // 2. Calculate remaining size
        float remainingSize = maxDimension - Mathf.Abs(delta);

        // 3. GAME OVER check
        if (remainingSize <= 0)
        {
            currentBlock.IsMoving = false;
            FallAndDestroy(currentBlock.gameObject); // Add physics to make it fall
            return false;
        }

        // 4. Update the Current Block (The piece that stays)
        float newPositionAxis = isMovingOnX ? lastPos.x + (delta / 2f) : lastPos.z + (delta / 2f);

        if (isMovingOnX)
        {
            currentT.localScale = new Vector3(remainingSize, currentScale.y, currentScale.z);
            currentT.position = new Vector3(newPositionAxis, currentPos.y, currentPos.z);
        }
        else
        {
            currentT.localScale = new Vector3(currentScale.x, currentScale.y, remainingSize);
            currentT.position = new Vector3(currentPos.x, currentPos.y, newPositionAxis);
        }

        // 5. Spawn Rubble (The piece that falls)
        SpawnRubble(currentPos, currentScale, delta, isMovingOnX);

        return true;
    }

    private void SpawnRubble(Vector3 oldPos, Vector3 oldScale, float delta, bool onX)
    {
        float fallenSize = Mathf.Abs(delta);
        float direction = delta > 0 ? 1 : -1;

        // Calculate rubble position (it sits on the edge of the new block)
        float newPosCoord = onX
            ? oldPos.x + (oldScale.x / 2f * direction) - (fallenSize / 2f * direction)
            : oldPos.z + (oldScale.z / 2f * direction) - (fallenSize / 2f * direction);


        GameObject rubble = Instantiate(currentBlock.gameObject);
        if (onX)
        {
            rubble.transform.localScale = new Vector3(fallenSize, oldScale.y, oldScale.z);
            rubble.transform.position = new Vector3(newPosCoord, oldPos.y, oldPos.z);
        }
        else
        {
            rubble.transform.localScale = new Vector3(oldScale.x, oldScale.y, fallenSize);
            rubble.transform.position = new Vector3(oldPos.x, oldPos.y, newPosCoord);
        }

        FallAndDestroy(rubble); // Add physics to make it fall
    }

    private void FallAndDestroy(GameObject obj, float delay = 2f)
    {
        Rigidbody rb = obj.AddComponent<Rigidbody>();
        rb.mass = 1f;

        Destroy(obj, delay);
    }
}