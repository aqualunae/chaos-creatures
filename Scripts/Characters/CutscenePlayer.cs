using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CutscenePlayer : DialogueWindow
{
    [SerializeField]
    private GameObjectVariable playerRef;

    private List<GameObject> actors;

    private int currentKeyframe;
    private Cutscene cutscene;
    private bool skip;
    private Animator animator;

    public void PlayCutscene(Cutscene cutscene)
    {
        animator = GetComponent<Animator>();
        actors = new List<GameObject>();

        // put us in cutscene mode
        pauzeEvent.Invoke(GameState.Cutscene);
        pauzeEvent.AddListener(Escape);

        // grab the actors and place them inside this object so they can be controlled by the animator
        // note that whether the actors are active should be controlled by the animations
        foreach (GameObject actor in cutscene.Actors)
        {
            GameObject[] instantiatedActors = new GameObject[0]; // PrefabUtility.FindAllInstancesOfPrefab(actor);
            if (instantiatedActors.Length == 0)
            {
                Debug.Log($"Could not find {actor.name}!");
            }
            else
            {
                instantiatedActors[0].transform.parent = transform;
                actors.Add(instantiatedActors[0]);
            }
        }

        // assign text to fields
        nameField.transform.parent.gameObject.SetActive(!string.IsNullOrEmpty(cutscene.Keyframes[0].source));
        nameField.text = cutscene.Keyframes[0].source;
        dialogueField.text = cutscene.Keyframes[0].lines[0];

        // assign variables that will let us progress
        dialogueLines = cutscene.Keyframes[0].lines;
        currentLine = 0;
        currentKeyframe = 0;
        this.cutscene = cutscene;
        skip = false;

        PlayAnimation();
    }

    public override void Next()
    {
        // increase the dialogue line
        currentLine++;
        if (currentLine < dialogueLines.Length)
        {
            // if the line index is valid in the current keyframe, show it
            dialogueField.text = dialogueLines[currentLine];
        }
        else
        {
            // if the line index is not valid in the current keyframe, try the next keyframe
            currentKeyframe++;
            if (currentKeyframe < cutscene.Keyframes.Length)
            {
                // go to the start of the dialogue of the new keyframe
                currentLine = 0;
                dialogueLines = cutscene.Keyframes[currentKeyframe].lines;
                dialogueField.text = dialogueLines[currentLine];

                // play animation
                PlayAnimation();
            }
            else
            {
                // if you're at the end, close the window
                gameObject.SetActive(false);
            }
        }
    }

    private void PlayAnimation()
    {
        // figure out the animation title and whether it exists
        string animation = $"{cutscene.ID}_{currentKeyframe}";
        if (animator.HasState(0, Animator.StringToHash(animation)))
        {
            // play it if it's there
            animator.Play(animation);
        }
    }

    protected override void OnDisable()
    {
        // unpauze the game
        base.OnDisable();
    }

    public override void Escape(GameState state)
    {
        // if the pauze key is pressed during dialogue
        // needs to be pressed twice to skip
        // add confirmation window!
        if (state == GameState.SkipCutscene)
        {
            if (!skip)
            {
                skip = true;
                return;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
