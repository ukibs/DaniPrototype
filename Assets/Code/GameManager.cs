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
        private float difficulty = 10;
        public float bestTimeRespond = 1;
        public float worstTimeRespond = 2;
        public float restTimeLastLevel = 15;
        public float amountWords = 9;
        public List<LevelDataToSave> levels = new List<LevelDataToSave>();

        public float Difficulty
        {
            get { return difficulty-9; }
            set { difficulty = value; }
        }

        public LevelDataToSave CurrentLevelData
        {
            get
            {
                return levels[currentLevel];
            }
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

    public void SetPoints(ref LevelDataToSave l)
    {
        l.maxPoints = infoType[Challenge_Type].amountWords * (3 - infoType[Challenge_Type].bestTimeRespond) * infoType[Challenge_Type].Difficulty + avgWordScreen * -(infoType[Challenge_Type].Difficulty * 2 / 100) + infoType[Challenge_Type].restTimeLastLevel;
        l.minPoints = infoType[Challenge_Type].amountWords * (3 - infoType[Challenge_Type].worstTimeRespond) * infoType[Challenge_Type].Difficulty + avgWordScreen * -(infoType[Challenge_Type].Difficulty * 2 / 100) + infoType[Challenge_Type].restTimeLastLevel;
    }

    public void NextLevel(float points)
    {
        LevelDataToSave data = infoType[Challenge_Type].CurrentLevelData;
        data.points = points;

        float avg = (data.maxPoints - data.minPoints) / 2;

        if (data.points > data.minPoints+avg)
        {
            infoType[challengeType].Difficulty = infoType[challengeType].Difficulty + 10;
        }
        else if(data.points > data.minPoints)
        {
            infoType[challengeType].Difficulty = infoType[challengeType].Difficulty + 9.5f;
        }
        infoType[challengeType].currentLevel++;
        data = infoType[Challenge_Type].CurrentLevelData;
        SetPoints(ref data);
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
