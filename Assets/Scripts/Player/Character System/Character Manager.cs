using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public CharacterDataBase characterDB;

    [Header("Character Info")]

    public TMP_Text characterName;
    public GameObject characterSprite;
    public TMP_Text characterHealth;
    public TMP_Text characterArmor;
    public TMP_Text characterSpeed;
    public TMP_Text characterDamage;

    [Header("Character Select")]

    public GameObject selectButton;
    public GameObject selectContainer;

    [Header("Unlock Character")]
    public GameObject unlockDialog;
    public TMP_Text priceText;
    public TMP_Text youHaveText;
    public Button unlockButton;

    private int selectedCharacter = 0;

    public List<Button> characterButtons = new List<Button>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateCharacterInfo();
        RefreshCharacterSelect();
    }

    public void SelectCharacter(int index = 0)
    {
        if (index > characterDB.CharacterCount || index < 0)
            selectedCharacter = 0;
        else selectedCharacter = index;
        UpdateCharacterInfo();
        SaveSelectedCharacter();
    }

    public void UpdateCharacterInfo()
    {
        Character character = characterDB.GetCharacter(selectedCharacter);
        characterName.text = character.name;
        characterHealth.text = character.maxHealth.ToString();
        characterSpeed.text = character.movementSpeed.ToString();
        characterArmor.text = character.armor.ToString();
        characterDamage.text = character.baseDamage.ToString();
        characterSprite.GetComponent<Image>().sprite = character.face;
        characterSprite.GetComponent<Image>().SetNativeSize();
    }

    private void SaveSelectedCharacter()
    {
        PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
    }

    public void UnlockCharacterDialog(int index)
    {
        unlockDialog.SetActive(true);
        priceText.text = "Price: " + characterDB.GetCharacter(index).cost.ToString();
        youHaveText.text = "You have: " + SaveManager.Instance.data.coins.ToString();
        unlockButton.onClick.RemoveAllListeners();
        unlockButton.onClick.AddListener(() => UnlockCharacter(index));
    }

    public void UnlockCharacter(int index)
    {
        if (!SaveManager.Instance.data.unlockedCharacters.Contains(index)
            && SaveManager.Instance.data.coins >= characterDB.GetCharacter(index).cost)
        {
            SaveManager.Instance.data.coins -= characterDB.GetCharacter(index).cost;
            SaveManager.Instance.data.unlockedCharacters.Add(index);
            SaveManager.Instance.Save();
            RefreshCharacterSelect();
            CloseUnlockDialog();
        }
        else
        {
            priceText.GetComponent<Animator>().Play("ErrorUnlock");
        }
    }

    // private void BuildCharacterSelect()
    // {
    //     for (int i = selectContainer.transform.childCount - 1; i >= 0; i--)
    //         Destroy(selectContainer.transform.GetChild(i).gameObject);

    //     characterButtons.Clear();

    //     for (int i = 0; i < characterDB.CharacterCount; i++)
    //     {
    //         GameObject btnGO = Instantiate(selectButton, selectContainer.transform);
    //         Button btn = btnGO.GetComponent<Button>();
    //         characterButtons.Add(btn);
    //         btnGO.transform.GetChild(0).GetComponent<Image>().sprite = characterDB.GetCharacter(i).face;
    //     }

    //     RefreshCharacterSelect();
    // }

    private void RefreshCharacterSelect()
    {
        for (int i = 0; i < characterButtons.Count; i++)
        {
            int index = i;
            Button btn = characterButtons[i];
            GameObject btnGO = btn.gameObject;
            Image faceImage = btnGO.transform.GetChild(0).GetComponent<Image>();
            Character character = characterDB.GetCharacter(i);

            faceImage.sprite = character.face;
            bool unlocked = SaveManager.Instance.data.unlockedCharacters.Contains(i);
            faceImage.color = unlocked ? Color.white : Color.black;

            btn.onClick.RemoveAllListeners();
            if (unlocked)
                btn.onClick.AddListener(() => SelectCharacter(index));
            else
                btn.onClick.AddListener(() => UnlockCharacterDialog(index));
        }
    }

    public void CloseUnlockDialog()
    {
        priceText.GetComponent<Animator>().Play("Default");
        unlockDialog.SetActive(false);
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
