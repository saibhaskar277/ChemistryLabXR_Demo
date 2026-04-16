using TMPro;
using UnityEngine;

public class MoleculeUI : MonoBehaviour
{

    public static MoleculeUI Instance { get; private set; } 

    [Header("Panel")]
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text formulaText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text bondTypeText;
    [SerializeField] private TMP_Text bondAngleText;


    public bool canShowMoleculeInfo { get; private set; }

    private void Awake()
    {
        canShowMoleculeInfo = true;
        if (Instance == null)
            Instance = this;
    }

    private void OnEnable()
    {
        EventManager.ListenEvent<OnMoleculeHoveredEvent>(Show);
        EventManager.ListenEvent<OnMoleculeDelectedEvent>(e => Hide());
    }

    private void Show(OnMoleculeHoveredEvent e)
    {
        var data = e.moleculeData;
        var loc = LocalizationManager.Instance;

        infoPanel.SetActive(true);
        canShowMoleculeInfo = false;

        nameText.text =
            loc.GetText(LocalizationKey.MoleculeName) + " " + data.moleculeName;

        formulaText.text =
            loc.GetText(LocalizationKey.ChemicalFormula) + " " + data.formula;

        descriptionText.text =
            loc.GetText(LocalizationKey.MoleculeDescription) + ": " + data.description;

        bondTypeText.text =
            loc.GetText(LocalizationKey.BondType) + " " + data.bondType;

        bondAngleText.text =
            loc.GetText(LocalizationKey.BondAngle) + " " + data.bondAngle;

        string narration =
            $"{nameText.text}. " +
            $"{formulaText.text}. " +
            $"{descriptionText.text}. " +
            $"{bondTypeText.text}. " +
            $"{bondAngleText.text}.";

        EventManager.RaiseEvent(new OnSpeechPlayEvent(narration));
    }

    private void Hide()
    {
        infoPanel.SetActive(false);
        canShowMoleculeInfo = true;
        EventManager.RaiseEvent(new OnSpeechStopEvent());
    }

    private void OnDisable()
    {
        EventManager.StopListening<OnMoleculeHoveredEvent>(Show);
        EventManager.StopListening<OnMoleculeDelectedEvent>(e => Hide());
    }

}
