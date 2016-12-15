using UnityEngine;
using System.Collections;

public class loopSpriteTexture : MonoBehaviour {
	public int colCount;
	public int rowCount;
	public int totalCells;
	public int stops = 3;
	private int rowNumber = 0;
	private int colNumber = 0;
	private Vector2 offset;
	private int index;

	void Update () {
		animateSprite();
	}

	void animateSprite(){
		if(index >= (totalCells - 1)){
			index = 0;
		}

		if(Time.frameCount % stops == 0){
			index = getNextInLoop(index, totalCells);
		}

		float sizeX = 1.0f / colCount;
		float sizeY = 1.0f / rowCount;
		Vector2 size =  new Vector2(sizeX,sizeY);

		// split into horizontal and vertical index
		var uIndex = index % colCount;
		var vIndex = index / colCount;

		// build offset
		// v coordinate is the bottom of the image in opefjdfkjngl so we need to invert.
		float offsetX = (uIndex+colNumber) * size.x;
		float offsetY = (1.0f - size.y) - (vIndex + rowNumber) * size.y;
		Vector2 offset = new Vector2(offsetX,offsetY);

		GetComponent<Renderer>().material.SetTextureOffset ("_MainTex", offset);
		GetComponent<Renderer>().material.SetTextureScale  ("_MainTex", size);
	}


	int getNextInLoop(int i, int max){
	    int next = i;
	    next++;
	    if(next >= max){
	        next = 0;
	    }
	    return next;
	}

}
