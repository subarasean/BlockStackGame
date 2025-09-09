using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField]
    private MovingCube cubePrefab;
    [SerializeField]
    private MoveDirection moveDirection;

    public void SpawnCube()
    {
        var cube = Instantiate(cubePrefab);

        if (MovingCube.LastCube != null && MovingCube.LastCube.gameObject != GameObject.Find("StartCube"))
        {
            float newY = MovingCube.LastCube.transform.position.y + cubePrefab.transform.localScale.y;

            if (moveDirection == MoveDirection.X)
            {
                cube.transform.position = new Vector3(transform.position.x, newY, MovingCube.LastCube.transform.position.z);
            }
            else
            {
                cube.transform.position = new Vector3(MovingCube.LastCube.transform.position.x, newY, transform.position.z);
            }

            cube.transform.localScale = new Vector3(MovingCube.LastCube.transform.localScale.x, cubePrefab.transform.localScale.y, MovingCube.LastCube.transform.localScale.z);
        }
        else
        {
            cube.transform.position = transform.position;
            cube.transform.localScale = cubePrefab.transform.localScale;
        }
        cube.MoveDirection = moveDirection; 
    }
}
