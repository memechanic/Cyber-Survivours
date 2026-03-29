using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public CharacterDataBase characterDB;


    public TMP_Text characterName;
    public GameObject characterSprite;
    public TMP_Text characterHealth;
    public TMP_Text characterArmor;
    public TMP_Text characterSpeed;
    public TMP_Text characterDamage;



    private int selectedCharacter = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateCharacter();
    }

    public void SelectCharacter(int index = 0)
    {
        if (index > characterDB.characterCount || index < 0)
            selectedCharacter = 0;
        else selectedCharacter = index;
        UpdateCharacter();
        Save();
    }

    public void UpdateCharacter()
    {
        Character character = characterDB.GetCharacter(selectedCharacter);
        characterName.text = character.name;
        characterHealth.text = character.maxHealth.ToString();
        characterSpeed.text = character.movementSpeed.ToString();
        characterArmor.text = character.armor.ToString();
        characterDamage.text = character.baseDamage.ToString();
        characterSprite.GetComponent<Image>().sprite = character.face;
    }

    private void Save()
    {
        PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
