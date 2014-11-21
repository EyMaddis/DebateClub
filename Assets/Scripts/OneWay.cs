using UnityEditor;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class OneWay : MonoBehaviour
{

    public int FromLayer = 8;
    public int Push2Layer = 11;
    

    void OnTriggerEnter2D(Collider2D col)
    {
        GameObject go = col.gameObject;
        if (go.tag == "Player")
        {
            //Debug.Log("CollisionEnter!");
            go.layer = Push2Layer;
            
            go.GetComponent<Character>().InWall = true;
            
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {

        GameObject go = col.gameObject;
        if (go.tag == "Player")
        {
           // Debug.Log("CollisionExit!");
            go.layer = FromLayer;
            go.GetComponent<Character>().InWall = false;
        }
    }
}
