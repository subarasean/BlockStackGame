using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovingCube : MonoBehaviour
{
    public static MovingCube CurrentCube {get; private set;}
    public static MovingCube LastCube { get; private set; }
    public MoveDirection MoveDirection { get; set; }

    [SerializeField]
    float moveSpeed = 1f;
    [SerializeField]
    private ParticleSystem stackEffect;

    private void OnEnable()
    {
        CurrentCube = this;

        if (LastCube == null)
        {
            LastCube = GameObject.Find("StartCube").GetComponent<MovingCube>() ;
        }


        GetComponent<Renderer>().material.color = GetRandomColor();

        transform.localScale = new Vector3(LastCube.transform.localScale.x, transform.localScale.y, LastCube.transform.localScale.z);
    }

    private Color GetRandomColor()
    {
        return new Color(UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f));
    }

    internal void Stop()
    {
        moveSpeed = 0;
        float cutCube = GetCutCube();
        UnityEngine.Debug.Log(cutCube);

        float max = MoveDirection == MoveDirection.Z ? LastCube.transform.localScale.z : LastCube.transform.localScale.x;
        if (Mathf.Abs(cutCube) >= max)
        {
            LastCube = null;
            CurrentCube = null;
            FindObjectOfType<GameManager>().GameOver();
        }


        float direction = cutCube > 0 ? 1 : -1;

        if (MoveDirection == MoveDirection.Z)
            ZSplit(cutCube, direction);
        else
            XSplit(cutCube, direction);

        if(stackEffect != null)
        {
            ParticleSystem effect = Instantiate(stackEffect, transform.position + new Vector3(0, transform.localScale.y / 2f, 0), Quaternion.Euler(-90,0,0));
            effect.Play();
            Destroy(effect.gameObject, effect.main.duration);
        }

        LastCube = this;
    }

    private float GetCutCube()
    {
        if(MoveDirection == MoveDirection.Z)
            return transform.position.z - LastCube.transform.position.z;
        else
            return transform.position.x - LastCube.transform.position.x;
    }

    private void XSplit(float cutCube, float direction)
    {
        float newCubeX = LastCube.transform.localScale.x - Mathf.Abs(cutCube);
        float fallingBlockSize = transform.localScale.x - newCubeX;

        float newXPosition = LastCube.transform.position.x + (cutCube / 2f);
        transform.localScale = new Vector3(newCubeX, transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);

        float cubeEdge = transform.position.x + (newCubeX / 2f * direction);
        float fallingBlockPosition = cubeEdge + fallingBlockSize / 2f * direction;

        SpawnDroppedCube(fallingBlockPosition, fallingBlockSize);
    }

    private void ZSplit(float cutCube, float direction)
    {
        float newCubeZ = LastCube.transform.localScale.z - Mathf.Abs(cutCube);
        float fallingBlockSize = transform.localScale.z - newCubeZ;

        float newZPosition = LastCube.transform.position.z + (cutCube / 2f);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newCubeZ);
        transform.position = new Vector3(transform.position.x, transform.position.y, newZPosition);

        float cubeEdge = transform.position.z + (newCubeZ / 2f * direction);
        float fallingBlockPosition = cubeEdge + fallingBlockSize / 2f * direction;

        SpawnDroppedCube(fallingBlockPosition, fallingBlockSize);
    }

    private void SpawnDroppedCube(float fallingBlockPosition, float fallingBlockSize)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        if (MoveDirection == MoveDirection.Z)
        {
            cube.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, fallingBlockSize);
            cube.transform.position = new Vector3(transform.position.x, transform.position.y, fallingBlockPosition);
        }
        else
        {
            cube.transform.localScale = new Vector3(fallingBlockSize, transform.localScale.y, transform.localScale.z);
            cube.transform.position = new Vector3(fallingBlockPosition, transform.position.y, transform.position.z);
        }
        cube.AddComponent<Rigidbody>();
        cube.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;

        Destroy(cube.gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (MoveDirection == MoveDirection.Z)
            transform.position += transform.forward * Time.deltaTime * moveSpeed;
        else
            transform.position += transform.right * Time.deltaTime * moveSpeed;
       
    }
}
