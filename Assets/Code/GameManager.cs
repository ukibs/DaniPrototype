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
    public class L
    {
        public int currentLevel = 0;
        private int difficulty = 10;
        public float bestTimeRespond = 1;
        public float worstTimeRespond = 2;
        public float restTimeLastLevel = 15;
        public float amountWords = 20;

        public int Difficulty
        {
            get { return difficulty-9; }
            set { difficulty = value; }
        }
    }

    #region Public Attributes
    public static GameManager instance = null;

    public Dictionary<ChallengeType, L> infoType = new Dictionary<ChallengeType, L>();

    
    public float avgWordScreen = 3;
    
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
            if (infoType[ChallengeType.BV].currentLevel > infoType[ChallengeType.GJ].currentLevel && infoType[ChallengeType.BV].currentLevel > infoType[ChallengeType.ZCS].currentLevel) return infoType[ChallengeType.BV].currentLevel;
            else if (infoType[ChallengeType.GJ].currentLevel > infoType[ChallengeType.BV].currentLevel && infoType[ChallengeType.GJ].currentLevel > infoType[ChallengeType.ZCS].currentLevel) return infoType[ChallengeType.GJ].currentLevel;
            return infoType[ChallengeType.ZCS].currentLevel;
        }
    }

    public float RestTimeLastLevel
    {
        set
        {
            infoType[challengeType].restTimeLastLevel = value;
        }
    }
    #endregion

    #region Monobehavoiur Methods

    void Awake()
    {
        //Check if there is already an instance
        if (instance == null)
            //if not, set it to this.
            instance = this;
        //If instance already exists:
        else if (instance != this)
            //Destroy this, this enforces our singleton pattern so there can only be one instance.
            Destroy(gameObject);

        //Set Manager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        infoType.Add(ChallengeType.BV, new L());
        infoType.Add(ChallengeType.GJ, new L());
        infoType.Add(ChallengeType.ZCS, new L());
    }

    #endregion

    #region Methods

    public void SetPoints(LevelData l)
    {
        l.maxPoints = infoType[l.type].amountWords * (3 - infoType[l.type].bestTimeRespond) * infoType[l.type].Difficulty + avgWordScreen * -(infoType[l.type].Difficulty * 2 / 100) + infoType[l.type].restTimeLastLevel;
        l.minPoints = infoType[l.type].amountWords * (3 - infoType[l.type].worstTimeRespond) * infoType[l.type].Difficulty + avgWordScreen * -(infoType[l.type].Difficulty * 2 / 100) + infoType[l.type].restTimeLastLevel;
    }

    public void NextLevel()
    {
        infoType[challengeType].currentLevel++;
    }

    /// <summary>
    /// Compara el tiempo de respuesta y actualiza el mejor y el peor
    /// </summary>
    public void TimeRespond(float value)
    {
        if (infoType[challengeType].bestTimeRespond > value) infoType[challengeType].bestTimeRespond = value;
        if (infoType[challengeType].worstTimeRespond < value) infoType[challengeType].worstTimeRespond = value;
    }
    #endregion
}
