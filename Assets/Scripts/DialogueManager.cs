using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Image characterImage;
    public GameFlowManager gfm;

    public GameObject dialoguePanel;
    public GameObject agafarComandaButton;
    public GameObject botoParlarClient;
    public bool isDialogueInici = true;

    private Dialogue dialogue;
    private int index = 0;

    public Button choiceButtonA;
    public Button choiceButtonB;

    // Assigna aquí el botó gran transparent des de l'inspector
    public Button dialegClickButton;

    void Start()
    {
        // Vinculem directament el botó transparent perquè passi el text
        if (dialegClickButton != null)
        {
            dialegClickButton.onClick.AddListener(NextSentence);
        }
    }

    public void StartDialogue(Dialogue d)
    {
        if (d == null)
        {
            Debug.LogWarning("[DialogueManager] StartDialogue called with null dialogue!");
            EndDialogue();
            return;
        }
        dialogue = d;
        index = 0;

        dialoguePanel.SetActive(true);
        agafarComandaButton.SetActive(false);

        if (botoParlarClient != null) botoParlarClient.SetActive(false);

        choiceButtonA.gameObject.SetActive(false);
        choiceButtonB.gameObject.SetActive(false);

        // Activem el botó invisible al principi perquè l'usuari pugui fer click per passar el text
        if (dialegClickButton != null) dialegClickButton.gameObject.SetActive(true);

        nameText.text = dialogue.characterName;
        characterImage.sprite = dialogue.characterPixel;

        ShowSentence();
    }

    public void InteractuarAmbClient()
    {
        if (botoParlarClient != null) botoParlarClient.SetActive(false);
        gfm.TalkClients();
    }

    public void NextSentence()
    {
        index++;

        if (index >= dialogue.sentences.Length)
        {
            if (dialogue.choices != null && dialogue.choices.Length > 0)
            {
                // Si hi ha opcions, desactivem el botó transparent per deixar prémer choice1 i choice2
                if (dialegClickButton != null) dialegClickButton.gameObject.SetActive(false);
                ShowSimpleChoices();
                return;
            }

            EndDialogue();
            return;
        }

        ShowSentence();
    }

    void ShowSentence()
    {
        string s = dialogue.sentences[index];
        s = s.Replace("{playerName}", PlayerPrefs.GetString("playerName"));
        dialogueText.text = s;
    }

    void EndDialogue()
    {
        if (dialegClickButton != null) dialegClickButton.gameObject.SetActive(false);
        dialoguePanel.SetActive(false);
        agafarComandaButton.SetActive(true);

        gfm.OnDialogueEnded();
    }

    void ShowSimpleChoices()
    {
        dialogueText.text = "";

        choiceButtonA.gameObject.SetActive(true);
        choiceButtonA.GetComponentInChildren<TMP_Text>().text = dialogue.choices[0].choiceText;

        choiceButtonA.onClick.RemoveAllListeners();
        choiceButtonA.onClick.AddListener(() => SelectChoice(0));

        if (dialogue.choices.Length > 1)
        {
            choiceButtonB.gameObject.SetActive(true);
            choiceButtonB.GetComponentInChildren<TMP_Text>().text = dialogue.choices[1].choiceText;

            choiceButtonB.onClick.RemoveAllListeners();
            choiceButtonB.onClick.AddListener(() => SelectChoice(1));
        }
        else
        {
            choiceButtonB.gameObject.SetActive(false);
        }
    }

    void SelectChoice(int choiceIndex)
    {
        choiceButtonA.gameObject.SetActive(false);
        choiceButtonB.gameObject.SetActive(false);

        StartDialogue(dialogue.choices[choiceIndex].nextDialogue);
    }
}
