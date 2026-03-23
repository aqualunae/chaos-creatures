using Assets.Scripts.Creatures;
using UnityEngine;
using UnityEngine.UI;

public class EventWindow : DialogueWindow
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private CreatureRenderer renderer1;

    [SerializeField]
    private CreatureRenderer renderer2;

    [SerializeField]
    private CreatureRenderer renderer3;

    [SerializeField]
    private Image eggRenderer;

    [SerializeField]
    private SpeciesListVariable speciesList;

    [SerializeField]
    private PartyWindow partyWindow;

    private string nextAnimation;
    private string nextLine;
    private bool needsName;
    private Party party;
    private int index;

    public void OnEnable()
    {
        pauzeEvent.Invoke(GameState.DialogueWindow);
        pauzeEvent.AddListener(Escape);
    }

    public void Hatch(Party party, int index)
    {
        this.party = party;
        this.index = index;
        SaveableCreature creature = party.Creatures[index];
        CreatureSpecies species = speciesList.GetSpecies(creature.species);
        eggRenderer.color = species.EggColor;
        renderer3.Initialize(species, creature.details);
        animator.Play("Base.Idle");
        dialogueField.text = $"The { creature.species } egg is ready to hatch!";
        nextAnimation = "Base.Hatch";
        nextLine = $"The { creature.species } hatched from the egg!";
        needsName = true;
    }

    public void Pair(SaveableCreature parent1, SaveableCreature parent2)
    {
        CreatureSpecies species = speciesList.GetSpecies(parent1.species);
        eggRenderer.color = species.EggColor;
        renderer1.Initialize(species, parent1.details);
        renderer2.Initialize(species, parent2.details);
        animator.Play("Base.Pre-Pair");
        dialogueField.text = $"{ parent1.creatureName } and { parent2.creatureName } are ready to pair!";
        nextAnimation = "Base.Pair";
        nextLine = $"{ parent1.creatureName } and { parent2.creatureName } produced an egg!";
    }

    public override void Next()
    {
        if (nextLine != null)
        {
            animator.Play(nextAnimation);
            dialogueField.text = nextLine;
            nextLine = null;
        }
        else if (needsName)
        {
            gameObject.SetActive(false);
            pauzeEvent.Invoke(GameState.PauzeMenu);
            partyWindow.gameObject.SetActive(true);
            partyWindow.DisplayNameField(index);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
