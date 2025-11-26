using UnityEngine;

public class BlockGenerator : LazySingleton<BlockGenerator>
{
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private GameObject blocksParent;


    public GameObject GenerateBlock(Vector3 bottomPosition)
    {
        Vector3 position = new Vector3(
            bottomPosition.x,
            bottomPosition.y + blockPrefab.transform.localScale.y / 2.0f,
            bottomPosition.z
        );

        GameObject newBlock = Instantiate(blockPrefab, position, Quaternion.identity, blocksParent.transform);
        return newBlock;
    }
}
