using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProceduralGenerationAlgorithms 
{ 

    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPosition, int walkLength)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();

        path.Add(startPosition);
        var previousPosition = startPosition;

        for(int i=0; i<walkLength; i++)
        {
            var newPosition = previousPosition + Direction2D.GetRandomDirection();
            path.Add(newPosition);
            previousPosition = newPosition;
        }

        return path;
    }
}

public static class Direction2D
{
    public static List<Vector2Int> cardinalDirectionsList = new List<Vector2Int>
    {
        Vector2Int.up,
        Vector2Int.right, 
        Vector2Int.down, 
        Vector2Int.left
    }; 

    public static Vector2Int GetRandomDirection()
    {
    return cardinalDirectionsList[Random.Range(0, cardinalDirectionsList.Count)];
    }   
}
