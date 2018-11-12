using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameMode
{
    Invalid = -1,

    Type1,
    Type2,

    //...
    Count,

}

public enum ChallengeType
{
    Invalid = -1,

    ZCS,
    BV,
    GJ,

    Count
}

public class GameManager : MonoBehaviour {


    #region Public Attributes
    public static GameManager instance = null;

    public int BVCurrentLevel = 5;
    public int GJCurrentLevel = 12;
    public int ZCSCurrentLevel = 23;
    #endregion

    #region Private Attributes
    private GameMode gameMode;
    private ChallengeType challengeType;
    #endregion

    #region Properties
    public GameMode Game_Mode
    {
        get { return gameMode; }
        set { gameMode = value; }
    }

    public ChallengeType Challenge_Type
    {
        get { return challengeType; }
        set { challengeType = value; }
    }

    public int MaxLevel
    {
        get
        {
            if (BVCurrentLevel > GJCurrentLevel && BVCurrentLevel > ZCSCurrentLevel) return BVCurrentLevel;
            else if (GJCurrentLevel > BVCurrentLevel && GJCurrentLevel > ZCSCurrentLevel) return GJCurrentLevel;
            return ZCSCurrentLevel;
        }
    }
    #endregion

    #region Monobehavoiur Methods

    void Awake()
    {
        //Check if there is already an instance of SoundManager
        if (instance == null)
            //if not, set it to this.
            instance = this;
        //If instance already exists:
        else if (instance != this)
            //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
            Destroy(gameObject);

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        
    }

    #endregion

    #region Methods
    #endregion
}
