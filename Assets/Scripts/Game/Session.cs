using Models.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using Toolbar;
using TwitchIntegration;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Session : MonoBehaviour
{
    public static Session instance { get; private set; }

    public static LevelData lastLevelData;

    public bool paused { private set; get; } = false;

    public int money { private set; get; } = -1;
    public int quota { private set; get; } = -1;
    public int dueIn { private set; get; } = -1;

    private readonly int startingDueIn = 3;

    [SerializeField]
    private AudioSource audioSource;

    public UserMaster userMaster;
    public TwitchUser lastUser;

    void Start()
    {
        instance = this;

        userMaster = new UserMaster();

        GameInputs.initiate();

        Cursor.lockState = CursorLockMode.Confined;

        //TwitchManager.OnTwitchCommandReceived += onCommand;

        if (lastLevelData == null)
        {
            lastLevelData = new LevelData(0, 500, startingDueIn, new List<(BaseItem, Vector3)>());
        }

        WorldGenerator.instance.onFinishedGenerating += loadLevelData;

        togglePaused(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            togglePauseMenu(!paused);
        }
    }

    private void onCommand(TwitchUser user, TwitchCommand command)
    {
        lastUser = user;
    }

    private Team teamToJoin = Team.ONE;
    public Team getTeamToJoin()
    {
        Team team = teamToJoin;
        teamToJoin++;
        if (teamToJoin > Team.FOUR) teamToJoin = Team.ONE;

        return team;
    }

    private void togglePauseMenu(bool state)
    {
        UIMenuLogic.instance.toggleMenu(state);
        togglePaused(state);
    }

    public void togglePaused(bool state)
    {
        if(state)
        {
            Time.timeScale = 0f;
            paused = true;
        }
        else
        {
            Time.timeScale = 1.0f;
            paused = false;
        }
    }

    private void loadLevelData()
    {
        money = lastLevelData.money;
        quota = lastLevelData.quota;
        dueIn = lastLevelData.dueIn;

        foreach((BaseItem, Vector3) storedItem in lastLevelData.storedItems)
        {
            BaseItem item = storedItem.Item1;
            Vector3 pos = storedItem.Item2;

            Instantiate(item.DroppedItemPrefab, pos, Quaternion.identity);
        }

        InfoDisplay.instance.updateText();

        if (dueIn == startingDueIn && money > 0)
        {
            audioSource.playSound("ka_ching");
        }
    }

    public void finalizeLevel(bool playerDied)
    {
        if(dueIn == 1)
        {
            lastLevelData.dueIn = startingDueIn;

            int storedValue = 0;
            if (!playerDied) storedValue = StorageZone.instance.getStoredValue() + ToolbarController.instance.getAllItemsValue();
            lastLevelData.storedItems.Clear();

            lastLevelData.quota += 500;
            lastLevelData.money += storedValue;

            if(quota > storedValue)
            {
                lastLevelData = new LevelData(0, 500, startingDueIn, new List<(BaseItem, Vector3)>());

                SceneManager.LoadScene("GameOver");
            }
            else
            {
                SceneManager.LoadScene("Main");
            }
        }
        else
        {
            lastLevelData.quota = quota;
            lastLevelData.money = money;
            lastLevelData.dueIn = dueIn - 1;

            if (playerDied)
            {
                lastLevelData.storedItems.Clear();
            }
            else
            {
                List<(BaseItem, Vector3)> storedItems = StorageZone.instance.getStoredItemsAndPositions();
                BaseItem[] toolbarItems = ToolbarController.instance.getAllItems();

                for(int i = 0; i < toolbarItems.Length; i++)
                {
                    if(toolbarItems[i] != null)
                    {
                        storedItems.Add((toolbarItems[i], new Vector3(1, 0.5f + 0.5f * i, -1 + (WorldGenerator.worldSize / 2 * WorldGenerator.tileScale))));
                    }
                }

                lastLevelData.storedItems = storedItems;
            }

            if(playerDied) SceneManager.LoadScene("DeathScreen");
            else SceneManager.LoadScene("Main");
        }
    }
}
