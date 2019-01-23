using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CamLevelSelectorMovement : MonoBehaviour {

    //
    public float rowTransitionTime = 0.5f;
    public Text type;

    //
    private float previousMouseX;
    private float previousMouseY;

    private int currentRow = 0;
    private int rowNumber;

    private Vector3 previousRowPosition;
    private Vector3 nextRowPosition;

    LevelsGenerator levelsGenerator;
    GameManager gameManager;

    private bool lerping;

    private float rowTransitionProgress = 0;

    // Use this for initialization
    void Start () {
        levelsGenerator = FindObjectOfType<LevelsGenerator>();
        gameManager = GameManager.instance;
        rowNumber = (int)ChallengeType.Count;
        nextRowPosition = -Vector3.forward * 10;
	}
	
	// Update is called once per frame
	void Update () {
        //
        float dt = Time.deltaTime;
        //
        if (Input.GetMouseButtonDown(0))
        {
            previousMouseX = Input.mousePosition.x;
            previousMouseY = Input.mousePosition.y;
        }
        //
        if (Input.GetMouseButton(0))
        {
            Vector3 newPosition = transform.position;

            //newPosition.x += (previousMouseX - Input.mousePosition.x) * Time.deltaTime;
            newPosition.y += (previousMouseY - Input.mousePosition.y) * Time.deltaTime;

            if(newPosition.y >= 0)
            {
                transform.position = newPosition;
            }

            //previousMouseX = Input.mousePosition.x;
            previousMouseY = Input.mousePosition.y;
        }
        //
        if (Input.GetMouseButtonUp(0))
        {
            float offsetX = Input.mousePosition.x - previousMouseX;
            // Chequeamos si hay bastante offset
            if (Mathf.Abs(offsetX) > 100)
            {
                if (offsetX > 0)
                    MoveCam(1);
                else
                    MoveCam(-1);
            }
        }
        //
        if (lerping)
        {
            //
            //Debug.Log(lerping);
            //
            rowTransitionProgress += dt;
            //
            //float originalDistance = Vector3.Magnitude(previousRowPosition - nextRowPosition);
            //float currentDistance = Vector3.Magnitude(transform.position - nextRowPosition);
            transform.position = Vector3.Lerp(previousRowPosition, nextRowPosition, Mathf.Sqrt(rowTransitionProgress / rowTransitionTime));
            if(rowTransitionProgress > rowTransitionTime)
            {
                lerping = false;
            }
        }
	}

    //
    public void MoveCam(int direction)
    {
            // Lo hacemos al revés
            gameManager.Challenge_Type = (ChallengeType)(int)gameManager.Challenge_Type - direction;
            //
            if (gameManager.Challenge_Type == ChallengeType.Invalid) gameManager.Challenge_Type = ChallengeType.BV;
            if (gameManager.Challenge_Type == ChallengeType.Count) gameManager.Challenge_Type = ChallengeType.GJ;
            //
            currentRow = (int)gameManager.Challenge_Type;
            previousRowPosition = transform.position;

            // Tenemos en cuenta el offset de las columnas
            nextRowPosition.x = currentRow * 5 - 5;
            nextRowPosition.y = gameManager.infoType[(ChallengeType)currentRow].CurrentLevel * 3;//num nivel

            Debug.Log(nextRowPosition + ", " + gameManager.Challenge_Type + ", " + direction);
            rowTransitionProgress = 0;
            lerping = true;

        type.text = gameManager.Challenge_Type+"";
    }
    
}
