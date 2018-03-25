using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour {

    #region Variables
    private Button button;

    public GameObject player;
    public GameObject playerMech;
    public GameObject room;

    public string roomType;

    bool initDone;
    #endregion

    void Start() 
	{
        player = GameObject.Find("Player");
        
        button = GetComponent<Button>();
    }

    void OnEnable()
    {
        player = GameObject.Find("Player");
    }

    void OnButtonClick(GameObject roomToUpgrade)
    {
        if (Player.resources >= roomToUpgrade.GetComponent<Room>().upgradeCost)
        {
            Player.resources -= roomToUpgrade.GetComponent<Room>().upgradeCost;
            roomToUpgrade.GetComponent<Room>().LevelUp();
        }
    }

    public void Init()
    {
        player = GameObject.Find("Player");
        playerMech = GameObject.Find("PlayerMech(Clone)");

        if (playerMech != null)
        {
            if (playerMech.transform.Find(roomType) != null)
            {
                room = playerMech.transform.Find(roomType).gameObject;
            }
            else
            {
                room = playerMech.transform.Find("Torso").Find(roomType).gameObject;
            }

            button.onClick.AddListener(() => OnButtonClick(room));
        }
    }

    void Destroy()
    {
        button.onClick.RemoveListener(() => OnButtonClick(room));
    }

    void Update()
    {
        if (playerMech == null || player == null)
        {
            initDone = false;
        }

        if (initDone == false)
        {
            Init();
            initDone = true;
        }
    }

}
