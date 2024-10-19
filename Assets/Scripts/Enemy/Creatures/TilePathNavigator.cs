using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class TilePathNavigator : MonoBehaviour
{
    private WorldGenerator worldGenerator;

    private bool isNavigating = false;

    private Vector3Int lastTileDestination = Vector3Int.one * -1;
    private Vector3Int currentTileDestination = Vector3Int.one * -1;

    private Quaternion targetRotation = Quaternion.identity;

    public float speed;
    public float rotationSpeed;

    private void Start()
    {
        worldGenerator = WorldGenerator.instance;

        if (worldGenerator.tilesGenerated) initiate();
        else worldGenerator.onFinishedGenerating += initiate;
    }


    private void Update()
    {
        if (!isNavigating) return;

        moveToDestination();
        rotateForward();
    }

    private void findNextDestination()
    {

        Vector3Int destinationDiff = lastTileDestination - currentTileDestination;

        int originSide = 0;

        switch(destinationDiff.x)
        {
            case 1:
                originSide = 0b0100;
                break;
            case -1:
                originSide = 0b0001;
                break;
            default:
                break;
        }

        switch(destinationDiff.z)
        {
            case 1:
                originSide = 0b1000;
                break;
            case -1:
                originSide = 0b0010;
                break;
            default:
                break;
        }

        if(lastTileDestination == (Vector3Int.one * -1)) originSide = 0;

        Tile currentTile = worldGenerator.getTile(currentTileDestination.x, currentTileDestination.z);

        List<Vector3Int> possibleDestinations = new List<Vector3Int>();

        int allowedSides = originSide ^ currentTile.connectingSides;

        if(allowedSides == 0)
        {
            Vector3Int tmp = currentTileDestination;
            currentTileDestination = lastTileDestination;
            lastTileDestination = tmp;
            return;
        }

        if((0b1000 & allowedSides) > 0) possibleDestinations.Add(currentTileDestination + Vector3Int.forward);
        if((0b0100 & allowedSides) > 0) possibleDestinations.Add(currentTileDestination + Vector3Int.right);
        if((0b0010 & allowedSides) > 0) possibleDestinations.Add(currentTileDestination + Vector3Int.back);
        if((0b0001 & allowedSides) > 0) possibleDestinations.Add(currentTileDestination + Vector3Int.left);

        lastTileDestination = currentTileDestination;

        currentTileDestination = possibleDestinations[Random.Range(0, possibleDestinations.Count)];

        targetRotation = Quaternion.Euler(0, -90, 0) * Quaternion.LookRotation(currentTileDestination - lastTileDestination);
    }

    private void moveToDestination()
    {
        Vector3 worldDestination = tileToWorldPos(currentTileDestination);

        if(Vector3.Distance(transform.position, worldDestination) < 0.1f)
        {
            transform.position = worldDestination;
            findNextDestination();
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, worldDestination, speed * Time.deltaTime);
    }

    public void initiate()
    {
        isNavigating = true;

        Vector3Int startTilePos = worldGenerator.getRandomTile().tilePos;

        currentTileDestination = startTilePos;

        transform.position = tileToWorldPos(startTilePos);

        findNextDestination();
    }

    private Vector3 tileToWorldPos(Vector3Int tilePos)
    {
        return new Vector3(tilePos.x * WorldGenerator.tileScale, 0, tilePos.z * WorldGenerator.tileScale);
    }

    private void rotateForward()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(tileToWorldPos(lastTileDestination), 1);
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(tileToWorldPos(currentTileDestination), 1);
    }
}
