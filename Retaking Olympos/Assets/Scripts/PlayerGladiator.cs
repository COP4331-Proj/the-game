﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGladiator : MonoBehaviour
{
    public static int currentHealth, currentStamina, currentLevel;
    public static int currentPower, currentDefense;
    public static string playerClass;
    public Gladiator player = new Gladiator("Player", 1, 100, 100, 6, 14);
    public HoldPlayerInformation holdPlayerInformation;
    public HealthBar healthBar;
    public StaminaBar staminaBar;
    public float staminaRegenTimer = 0f;
    private int startingHealth;

    // Start is called before the first frame update
    void Start()
    {
        if (holdPlayerInformation != null) 
        {
            player = holdPlayerInformation.gladiatorList[holdPlayerInformation.index];
        }
        
        if (PlayerPrefs.HasKey("GameSceneIsLoaded")) 
            setupPlayerGladiator();

        if (!PlayerPrefs.HasKey("PlayerIsInArena") && PlayerPrefs.HasKey("GameSceneIsLoaded"))
        {
            SavePlayerGladiatorData();
            PlayerPrefs.SetInt("PlayerIsInArena", 1);
        }

        if (PlayerPrefs.HasKey("FightStatus") && PlayerPrefs.GetInt("FightStatus") != 1)
        {
            if (PlayerPrefs.HasKey("playerSetUp")) 
            {
                QuickLoadPlayerGladiator();

                if (PlayerPrefs.HasKey("GameSceneIsLoaded"))
                {
                    useSkill(PlayerPrefs.GetInt("staminaUsed"));
                    takeDamage(PlayerPrefs.GetInt("damageTaken"));
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentStamina < player.GetStamina())
        {
            if (Time.time >= staminaRegenTimer)
            {
                staminaRegen();
                staminaRegenTimer = Time.time + .1f;
            }
        }

        healthBar.setHealth(currentHealth);
        staminaBar.setStamina(currentStamina);

        if (PlayerPrefs.HasKey("GameSceneIsLoaded"))
        {
            PlayerPrefs.SetFloat("playerXPosition", this.transform.position.x);
            PlayerPrefs.SetFloat("playerYPosition", this.transform.position.y);
            PlayerPrefs.SetInt("staminaUsed", (player.GetStamina() - currentStamina));
        }
    }

    // Method to test health bar change
    public void takeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            // If the player dies, we need to reset both player and enemy gladiators for the next fight
            if (PlayerPrefs.HasKey("GameSceneIsLoaded"))
            {
                PlayerPrefs.DeleteKey("playerSetUp");
                PlayerPrefs.DeleteKey("enemySetUp");

                // This flag will tell the game if we need to save gladiator stats at the start of the next battle so that the
                // player can't load the data from this battle
                PlayerPrefs.DeleteKey("PlayerIsInArena");
                PlayerPrefs.DeleteKey("EnemyIsInArena");
            }

            GameObject gameObject = new GameObject();
            gameObject.AddComponent<SceneLoader>();
            gameObject.GetComponent<SceneLoader>().GoToScene("Defeat Screen");
        }

        PlayerPrefs.SetInt("damageTaken", (player.GetHealth() - currentHealth));
    }

    // Method to test stamina bar change
    void useSkill(int stamina)
    {
        if (currentStamina <= 0)
            return;

        currentStamina -= stamina;
    }

    public int getCurrentHealth()
    {
        return currentHealth;
    }


    public void setupPlayerGladiator()
    {
        // Sets up the health bar and stamina bar
        currentHealth = player.GetHealth();
        healthBar.setMaxHealth(player.GetHealth());
        currentStamina = player.GetStamina();
        staminaBar.setMaxStamina(player.GetStamina());

        // Set up the rest of the stats
        currentLevel = player.GetLevel();
        currentPower = player.GetPower();
        currentDefense = player.GetDefense();
        playerClass = player.GetClass();

        // Set up PlayerPref stuff so that the player gladiator doesn't reset when switching to the pause menu
        PlayerPrefs.SetInt("healthValue", currentHealth);
        PlayerPrefs.SetInt("staminaValue", currentStamina);
        PlayerPrefs.SetInt("level", currentLevel);
        PlayerPrefs.SetInt("power", currentPower);
        PlayerPrefs.SetInt("defense", currentDefense);
        PlayerPrefs.SetString("playerClass", playerClass);

        // Create PlayerPref data to keep track of how much damage was taken and stamina was used and player position
        if (!PlayerPrefs.HasKey("playerSetUp"))
        {
            PlayerPrefs.SetInt("damageTaken", 0);
            PlayerPrefs.SetInt("staminaUsed", 0);
            PlayerPrefs.SetFloat("playerXPosition", this.transform.position.x);
            PlayerPrefs.SetFloat("playerYPosition", this.transform.position.y);
        }

        // Use a PlayerPref flag to let the PlayerGladiator class know if the PlayerGladiator is set up
        // The value doesn't actually matter because we can use PlayerPrefs.HasKey to check if the key is present
        PlayerPrefs.SetInt("playerSetUp", 1);
    }

    // This function is for loading playerpref values
    public void QuickLoadPlayerGladiator()
    {
        player.SetHealth(PlayerPrefs.GetInt("healthValue"));
        player.SetStamina(PlayerPrefs.GetInt("staminaValue"));
        currentLevel = PlayerPrefs.GetInt("level");
        currentPower = PlayerPrefs.GetInt("power");
        currentDefense = PlayerPrefs.GetInt("defense");
        playerClass = PlayerPrefs.GetString("playerClass");

        transform.position = new Vector2(PlayerPrefs.GetFloat("playerXPosition"),
        PlayerPrefs.GetFloat("playerYPosition"));
    }

    public void SavePlayerGladiatorData()
    {
        SaveManager.SavePlayerData(this);
    }

    public void staminaRegen()
    {
        if (PlayerPrefs.HasKey("GameSceneIsLoaded"))
            currentStamina++;
    }

    public void LoadPlayerGladiatorData()
    {
        // Load the stats
        PlayerData data = SaveManager.LoadPlayerData();
        currentHealth = data.health;
        currentLevel = data.level;
        currentDefense = data.defense;
        currentStamina = data.stamina;
        currentPower = data.power;

        // The save the stats to PlayerPrefs so that it doesn't disappear on switching off the pause menu
        PlayerPrefs.SetInt("damageTaken", player.GetHealth() - currentHealth);
        PlayerPrefs.SetInt("staminaUsed", player.GetStamina() - currentStamina);
        PlayerPrefs.SetInt("level", currentLevel);
        PlayerPrefs.SetInt("power", currentPower);
        PlayerPrefs.SetInt("defense", currentDefense);

        // Load the position
        PlayerPrefs.SetFloat("playerXPosition", data.xPos);
        PlayerPrefs.SetFloat("playerYPosition", data.yPos);
    }

    public void ConditionalPlayerSave()
    {
        if (PlayerPrefs.HasKey("PlayerIsInArena"))
            SavePlayerGladiatorData();
    }

    public void ConditionalPlayerLoad()
    {
        if (PlayerPrefs.HasKey("PlayerIsInArena"))
            LoadPlayerGladiatorData();
    }

}
