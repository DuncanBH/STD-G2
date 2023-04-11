using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Parallaxer : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    [SerializeField] private float[] layerSpeeds;


    void Start()
    {
        Vector3 position = transform.position;

        var children1 = new List<Transform>();
        var children2 = new List<Transform>();

        int index = 0;
        
        Vector3 childRendererSize = GetComponentInChildren<SpriteRenderer>().bounds.size;

        foreach (Transform child in transform)
        {
            Debug.Log("bading");

            child.gameObject.AddComponent<ParallaxerUnit>().speed = layerSpeeds[index];
            
            children1.Add(Instantiate(child));
            children2.Add(Instantiate(child));
            
            index++;
        }

        index = 0;
        
        foreach (var child in children1)
        {
            child.parent = transform;
            child.transform.localScale = Vector3.one;
            child.position = new Vector3(position.x + childRendererSize.x, position.y);
            child.gameObject.AddComponent<ParallaxerUnit>().speed = layerSpeeds[index];

            index++;
        }


        index = 0;
        foreach (var child in children2)
        {
            child.parent = transform;
            child.transform.localScale = Vector3.one;
            child.position = new Vector3(position.x - childRendererSize.x, position.y);
            child.gameObject.AddComponent<ParallaxerUnit>().speed = layerSpeeds[index];

            index++;
        }
    }
}