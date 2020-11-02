﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    private Vector2 input;
    private Vector2 rawinput;
    public Transform DbgJ;
    public Transform DbgJr;
    private Vector2 minput;
    private Vector2 mrawinput;
    public Transform DbgM;
    public Transform DbgMr;
    public Transform DbgMoveField;
    public Vector3 MoveField = new Vector3(0, 0, 0);

    private bool paused = false;
    public GameObject PauseMenu;

    public GameObject GroundedIndicator;
    public GameObject cube;
    private Rigidbody cube_rigidbody;
    private MovementRigidBody cube_mov;

    private health Health;
    public Text Health_text;
    public Text Ammo_text;
    private WeaponHolder weapons;

    public GameObject Score;
    public GameMode gameMode;
    public GameObject scoreEntryPrefab;
    public List<GameObject> scoreEntries = new List<GameObject>();

    public bool dbguienabled = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cube_rigidbody = cube.GetComponent<Rigidbody>();
        cube_mov = cube.GetComponent<MovementRigidBody>();
        Health = this.GetComponentInParent<health>();
        weapons = this.transform.parent.GetComponentInChildren<WeaponHolder>();
        gameMode = GameObject.Find("Global").GetComponent<GameMode>();
        dbguisetenabled(dbguienabled);
    }

    void dbguisetenabled(bool t) {
        dbguienabled = t;
        if (t) {
            DbgMoveField.gameObject.SetActive(true);
            DbgJ.gameObject.SetActive(true); ;
            DbgJr.gameObject.SetActive(true); ;
            DbgM.gameObject.SetActive(true); ;
            DbgMr.gameObject.SetActive(true); ;
        }
        else {
            GroundedIndicator.SetActive(false);
            PauseMenu.SetActive(false);
            DbgMoveField.gameObject.SetActive(false);
            DbgJ.gameObject.SetActive(false); ;
            DbgJr.gameObject.SetActive(false); ;
            DbgM.gameObject.SetActive(false); ;
            DbgMr.gameObject.SetActive(false); ;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (dbguienabled) {
            input.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            rawinput.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            MoveField = cube_rigidbody.velocity / cube_mov.spd;
            MoveField = cube.transform.InverseTransformVector(MoveField);
            DbgMoveField.localPosition = 50 * new Vector2(MoveField.x, MoveField.z);
            DbgJ.localPosition = 50 * input;
            DbgJr.localPosition = 50 * rawinput;

            minput.Set(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            mrawinput.Set(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

            DbgM.localPosition = 50 * minput;
            DbgMr.localPosition = 50 * mrawinput;

            if (!paused && Input.GetKeyDown("p"))
            {
                paused = true;
                PauseMenu.SetActive(paused);
                Time.timeScale = 0;
            }
            else if (paused && Input.GetKeyDown("p"))
            {
                paused = false;
                PauseMenu.SetActive(paused);
                Time.timeScale = 1;
            }

            GroundedIndicator.SetActive(cube_mov.isGrounded);
        }



        if (Health != null) {
            Health_text.text = "Health: " + (Health.hp).ToString();
        }
        if (weapons != null)
        {
            Ammo_text.text = "Ammo: " + (weapons.Ammo[weapons.Weapons[weapons.ActiveWeapon].id]).ToString();
        }

        if (gameMode != null) {
            Score.SetActive(Input.GetKey(KeyCode.Tab));
            if (Input.GetKey(KeyCode.Tab)) { UpdateScoreBoardUI(); }
        }


    }

    void UpdateScoreBoardUI() {
        int diff = scoreEntries.Count - gameMode.ScoreTable.Count;
        if (diff < 0) {
            // добавить UI
            for (int i = 0; i < -diff; ++i) {
                GameObject tmp = Instantiate(scoreEntryPrefab);
                tmp.transform.SetParent(Score.transform.GetChild(0),false);
                tmp.transform.position.Set(0,0,0);
                tmp.transform.Translate(23*Vector3.down * (i + scoreEntries.Count));
                scoreEntries.Add(tmp);
            }
        }
        else if (diff > 0) {
            // удалить лишний UI
            for (int i = 0; i < diff; ++i) Destroy(scoreEntries[gameMode.ScoreTable.Count + i]);
            scoreEntries.RemoveRange(gameMode.ScoreTable.Count, diff);
        }
        for (int i=0;i< scoreEntries.Count; ++i) {
            scoreEntries[i].transform.GetChild(0).GetComponent<Text>().text = (gameMode.ScoreTable[i].playerid).ToString();
            scoreEntries[i].transform.GetChild(1).GetComponent<Text>().text = gameMode.ScoreTable[i].nick;
            scoreEntries[i].transform.GetChild(2).GetComponent<Text>().text = (gameMode.ScoreTable[i].score).ToString();
            scoreEntries[i].transform.GetChild(3).GetComponent<Text>().text = (gameMode.ScoreTable[i].K).ToString();
            scoreEntries[i].transform.GetChild(4).GetComponent<Text>().text = (gameMode.ScoreTable[i].D).ToString();
        }

    }
}
