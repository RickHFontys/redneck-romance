using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    private Character chosenCharacter;

    public Character ChosenCharacter
    {
        get {return chosenCharacter;}
        set {chosenCharacter = value;}
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }
}
