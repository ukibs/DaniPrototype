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

    BV,
    ZCS,
    GJ,

    Count
}

public class GameManager : MonoBehaviour {
    public class L
    {
        public int currentLevel = 0;
        public int maxLevel = 0;
        public List<LevelDataToSave> levels = new List<LevelDataToSave>();

        public LevelDataToSave CurrentLevelData
        {
            get
            {
                return levels[currentLevel];
            }
        }

        public int CurrentLevel
        {
            get { return currentLevel; }
            set
            {
                currentLevel = value;
                if(levels.Count < currentLevel)
                {
                    levels.Add(new LevelDataToSave());
                }
            }
        }
    }

    #region Public Attributes
    public static GameManager instance = null;

    public Dictionary<ChallengeType, L> infoType = new Dictionary<ChallengeType, L>();

    public float avgWordScreen = 3;

    #endregion

    #region Private Attributes
    private SaveLoad saveLoad;
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
            if (infoType[ChallengeType.BV].maxLevel > infoType[ChallengeType.GJ].maxLevel && infoType[ChallengeType.BV].maxLevel > infoType[ChallengeType.ZCS].maxLevel) return infoType[ChallengeType.BV].maxLevel;
            else if (infoType[ChallengeType.GJ].maxLevel > infoType[ChallengeType.BV].maxLevel && infoType[ChallengeType.GJ].maxLevel > infoType[ChallengeType.ZCS].maxLevel) return infoType[ChallengeType.GJ].maxLevel;
            return infoType[ChallengeType.ZCS].maxLevel;
        }
    }

    public float RestTimeLastLevel
    {
        set
        {
            LevelSelectedData.restTimeLastLevel = value;
        }
    }

    public L ModeSelected
    {
        get
        {
            return infoType[challengeType];
        }
        set
        {
            infoType[challengeType] = value;
        }
    }

    public LevelDataToSave LevelSelectedData
    {
        get { return infoType[challengeType].CurrentLevelData; }
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
        //Debug.Log("Estoy en debug");
        saveLoad = SaveLoad.Instance;

        for (int i = 0; i < (int)ChallengeType.Count; i++)
        {
            infoType.Add((ChallengeType)i, new L());
        }

        //
        challengeType = ChallengeType.BV;
    }
    #endregion

    #region Methods

    public void SetPoints(ref LevelDataToSave l, ChallengeType ct)
    {
        LevelDataToSave prevLevel = infoType[ct].currentLevel - 1 >= 0 ? infoType[ct].levels[infoType[ct].currentLevel - 1] : infoType[ct].levels[0];
        l.maxPoints = prevLevel.amountWords * (3 - prevLevel.bestTimeRespond) * prevLevel.difficulty + avgWordScreen * -(prevLevel.difficulty * 2 / 100) + prevLevel.restTimeLastLevel/2;
        l.minPoints = prevLevel.amountWords * (3 - prevLevel.worstTimeRespond) * prevLevel.difficulty + avgWordScreen * -(prevLevel.difficulty * 2 / 100) + prevLevel.restTimeLastLevel/2;
    }

    public void NextLevel(float points)
    {
        LevelDataToSave data = LevelSelectedData;
        data.points = points;
        saveLoad.AddData(Challenge_Type, data);
        float avg = (data.maxPoints - data.minPoints) / 2;

        if (data.level == ModeSelected.maxLevel && data.points > data.minPoints + avg)
        {
            ModeSelected.maxLevel++;
            ModeSelected.CurrentLevel++;
            data = LevelSelectedData;
            if (data.points > data.minPoints + avg)
            {
                LevelSelectedData.difficulty = LevelSelectedData.difficulty + 1;
            }
            else if (data.points > data.minPoints)
            {
                LevelSelectedData.difficulty = LevelSelectedData.difficulty + 0.5f;
            }
            SetPoints(ref data, challengeType);
        }          
    }

    /// <summary>
    /// Compara el tiempo de respuesta y actualiza el mejor y el peor
    /// </summary>
    public void TimeRespond(float value)
    {
        if (LevelSelectedData.bestTimeRespond > value) LevelSelectedData.bestTimeRespond = value;
        if (LevelSelectedData.worstTimeRespond < value) LevelSelectedData.worstTimeRespond = value;
    }
    #endregion
}
