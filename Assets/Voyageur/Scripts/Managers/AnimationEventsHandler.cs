using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventsHandler : MonoBehaviour
{
	TransitionHandler TransitionHandler;

	GameObject player;
	public InteractionsManager interactionsManager;
	public AdditionalSpritesManager spritesManager;
	public IndividualInteractions individualInteractions;




	private void Awake()
	{
		player = GameObject.Find("Player");
		TransitionHandler = GameObject.Find("Transition Handler").GetComponent<TransitionHandler>();
		
	}
	public void Transition()
	{
		TransitionHandler.Beach();
	}

	public void DestroySprite()
	{
		GameObject.Destroy(gameObject);
	}

	public void ThrowStick()
	{
		player.GetComponent<PlayerController>().ThrowStick();
	}

	public void Launch()
	{
		TransitionHandler.Launch();
	}



    #region SoundFX

    public void FootStepsPlayer()
	{
		player.GetComponent<PlayerSoundManager>().PlayFootsteps();
	}

	public void PaddleSounds()
	{
		GameObject canoeAIO = GameObject.Find("Canoe AIO");
		canoeAIO.GetComponent<PlayerSoundManager>().Paddle();
	}

	public void FootstepsInWater()
	{
		GameObject canoeAIO = GameObject.Find("Canoe AIO");
		canoeAIO.GetComponent<PlayerSoundManager>().PlayExitCanoe();
	}

	public void FootStepsCanoe()
	{
		GameObject canoeAIO = GameObject.Find("Canoe AIO");
		canoeAIO.GetComponent<PlayerSoundManager>().PlayFootsteps();
	}

	public void DragCanoe()
	{
		GameObject canoeAIO = GameObject.Find("Canoe AIO");
		canoeAIO.GetComponent<PlayerSoundManager>().PlayDragCanoe();
	}

	public void MontyFootSteps()
	{
		GameObject monty = GameObject.Find("Monty");
		monty.GetComponent<MontySoundManager>().PlayFootsteps();
	}

	public void Paddle()
	{
		GameObject canoeAIO = GameObject.Find("Canoe AIO");
		canoeAIO.GetComponent<CanoePaddle>().AddPaddleForce();
	}

	public void ChangeTreeSprites()
	{
		interactionsManager.interaction.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = spritesManager.sprites[individualInteractions.chopCount];
		player.GetComponent<PlayerSoundManager>().PlayTreeChop(8 + individualInteractions.chopCount);
	}

	public void DestroyTree()
	{
		Debug.Log("destroy");
		StartCoroutine(player.GetComponent<PlayerController>().EnablePlayerInput(0));

		player.GetComponent<InteractionsManager>().interaction.transform.GetChild(1).gameObject.SetActive(false);
		player.GetComponent<InteractionsManager>().interaction.transform.GetChild(2).gameObject.SetActive(true);
		player.GetComponent<InteractionsManager>().interaction.transform.GetChild(3).gameObject.SetActive(false);
	}

}

#endregion