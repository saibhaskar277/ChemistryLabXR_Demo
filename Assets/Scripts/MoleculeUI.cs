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
        EventManager.ListenEvent<OnMoleculeCreatedEvent>(Show);
        EventManager.ListenEvent<OnMoleculeDelectedEvent>(e => Hide());
    }

    private void Show(OnMoleculeCreatedEvent e)
    {
        var data = e.moleculeData;

        infoPanel.SetActive(true);

        canShowMoleculeInfo = false;

        nameText.text = $"Molecule name is {data.moleculeName}";
        formulaText.text = $"Chemical formula is {data.formula}";
        descriptionText.text = $"Description: {data.description}";
        bondTypeText.text = $"The bond type is {data.bondType}";
        bondAngleText.text = $"The bond angle is {data.bondAngle}";
    }

    private void Hide()
    {
        infoPanel.SetActive(false);
        canShowMoleculeInfo = true;
    }

    private void OnDisable()
    {
        EventManager.StopListening<OnMoleculeCreatedEvent>(Show);
        EventManager.StopListening<OnMoleculeDelectedEvent>(e => Hide());
    }

}
