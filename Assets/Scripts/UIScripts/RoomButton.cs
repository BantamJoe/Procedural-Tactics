using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class RoomButton : MonoBehaviour {

    private Button button;

    public GameObject player;
    public GameObject playerMech;
    public GameObject weapon;
    public GameObject room;

    public GameObject destinationPrefab;

    public string roomType;

    
    void Start()
    {
        player = GameObject.Find("Player");
        playerMech = GameObject.Find("PlayerMech(Clone)");

        button = GetComponent<Button>();
        button.onClick.AddListener(() => OnButtonClick(player.GetComponent<Player>().selectedEnemy));
    }

    void OnButtonClick(GameObject targetEnemy)
    {
        if (playerMech == null)
        {
            playerMech = GameObject.Find("PlayerMech(Clone)");
        }

        Mech mech = playerMech.GetComponent<Mech>();

        if (mech.selectedWeapon != null)
        {
            weapon = mech.selectedWeapon;
            weapon.GetComponent<Weapon>().targetSelected = true;

            if (targetEnemy != null)
            {
                if (targetEnemy.transform.Find(roomType) != null)
                {
                    room = targetEnemy.transform.Find(roomType).gameObject;
                }
                else
                {
                    room = targetEnemy.transform.Find("Torso").Find(roomType).gameObject;
                }

                weapon.GetComponent<Weapon>().enemyRoom = room;

                if (mech.rotationDestination != null)
                {
                    Destroy(mech.rotationDestination);
                }

                destinationPrefab = mech.destinationPrefab;
                mech.rotationDestination = Instantiate(destinationPrefab, room.transform.position, Quaternion.identity);
                mech.rotationDestination.transform.SetParent(room.transform);
                mech.rotationDestination.layer = 2;
                mech.rotationCompleted = false;

            }

            mech.selectedWeapon = null;
            weapon.GetComponent<Weapon>().weaponSelected = false;
        }
    }

    void Destroy()
    {
        button.onClick.RemoveListener(() => OnButtonClick(player.GetComponent<Player>().selectedEnemy));
    }

}
