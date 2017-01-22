 using UnityEngine;
 using System.Collections;
 
 public class SwordPiece1 : MonoBehaviour {
 
     // Use this for initialization
     void Start () {
     
     }
     
     // Update is called once per frame
     void Update () {
     
     }
 
 
     public void OnTriggerEnter(Collider other)
     {       
             //If the one colliding have the tag prey it
             //will get destroyed
 
         if(other.tag == "Player")
         {
             Destroy(other.gameObject);
         }
     }
 }
