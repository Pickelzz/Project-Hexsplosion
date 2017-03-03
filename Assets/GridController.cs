using UnityEngine;
using System.Collections;

public class GridController : MonoBehaviour {
    public GameObject hexPrefab;

    int width = 12;
    int height = 20;

    float xOffset = 0.47f;
    float yOffset = 0.332f;

	// Use this for initialization
	void Start () {
	
        for(int x = 0; x < width; x++) { 
            for(int y = 0; y < height; y++)
            {
                float xPos = (transform.localPosition.x + x) * xOffset;
                float yPos = (transform.localPosition.y + y) * yOffset;
             
                if(y % 2 == 1)
                {
                    xPos += xOffset / 2f;    
                }
                GameObject hex_go = (GameObject)Instantiate(hexPrefab, new Vector3(xPos, yPos,-5), Quaternion.identity);
                hex_go.name = "hex " + x + "_" + y;
                hex_go.transform.SetParent(this.transform); 
       
            }     
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
