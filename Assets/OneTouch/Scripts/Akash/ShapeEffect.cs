using System.Collections.Generic;
using UnityEngine;

public class ShapeEffect : MonoBehaviour
{
    public List<GameObject> Shapes = new List<GameObject>();
    public List<Transform> StartPositions = new List<Transform>();


    void Start()
    {
        currentShapeCounter = 0;
        currentPositionCounter = 0;
        ShapeEffects();
        ShapeEffects();
    }

    int currentPositionCounter;
    int currentShapeCounter;

    void ShapeEffects()
    {
        Shapes[currentShapeCounter].transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        Shapes[currentShapeCounter].transform.position = StartPositions[currentPositionCounter].position;
        TweenShape(Shapes[currentShapeCounter]);
        currentShapeCounter++;
        currentPositionCounter++;

        if (currentShapeCounter >= Shapes.Count)
        {
            currentShapeCounter = 0;
        }

        if (currentPositionCounter >= StartPositions.Count)
        {
            currentPositionCounter = 0;
        }
    }

    void TweenShape(GameObject obj)
    {
        float time = Random.Range(2, 5);
        LeanTween.moveY(obj, -5f, time);
        LeanTween.rotateAround(obj, Vector3.forward, 360, 1).setRepeat(-1);
        LeanTween.scale(obj, Vector3.zero, time).setOnComplete(() =>
        {
            ShapeEffects();
        });
    }
}
