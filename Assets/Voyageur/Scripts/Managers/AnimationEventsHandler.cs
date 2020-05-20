using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventsHandler : MonoBehaviour
{
	TransitionHandler TransitionHandler;

	public GameObject player;
	public GameObject door;
	public GameObject logPile;
	public GameObject fire;
	public InteractionsManager interactionsManager;
	public AdditionalSpritesManager spritesManager;
	public IndividualInteractions individualInteractions;
	public MyGrid grid;

	public GameObject bats;
	public GameObject batsAudio;


	#region Functions



	public void FinishMine()
	{
		bats.SetActive(true);
		batsAudio.SetActive(true);
		//individualInteractions.animTriggered = false;
		StartCoroutine(player.GetComponent<PlayerController>().EnablePlayerInput(0f));
		interactionsManager.interaction.MarkAsComplete();
		player.GetComponent<PlayerController>().targetFound = false;
		player.GetComponent<PlayerController>().usingHands = false;
		Debug.Log("Fin");

	}

	public void ChangeSpriteLayer()
	{
		GetComponent<SpriteRenderer>().sortingOrder = GameObject.Find("CanoeGFX").GetComponent<SpriteRenderer>().sortingOrder - 1;
	}
	public void MontyInCanoe()
	{
		transform.GetComponentInParent<MontyStateVariables>().montyInCanoe = true;
	}

	public void SignalMontyToGetIn()
	{
		GameObject.Find("Monty").GetComponent<MontyStateVariables>().montyReadyToGetIn = true;
	}

	private void Awake()
	{
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

	public void Door()
	{
		TransitionHandler.Door();
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
		player.GetComponent<PlayerController>().usingAxe = false;
		player.GetComponent<PlayerController>().targetFound = false;
		individualInteractions.chopCount = 0;
		player.GetComponent<InteractionsManager>().interaction.MarkAsComplete();
	}
	public void ChoppingBlock()
	{
		logPile.SetActive(true);
		player.GetComponent<InteractionsManager>().interaction.gameObject.SetActive(false);

		individualInteractions.chopCount = 0;
		player.GetComponent<InteractionsManager>().interaction.MarkAsComplete();
		player.GetComponent<PlayerInventory>().AddWood();
		player.GetComponent<PlayerController>().usingAxe = false;
		player.GetComponent<PlayerController>().targetFound = false;
		StartCoroutine(player.GetComponent<PlayerController>().EnablePlayerInput(0));
	}

	public void SetHasLogs()
	{
		individualInteractions.hasLogs = true;
	}

	public void DeleteLogs()
	{
		player.GetComponent<InteractionsManager>().interaction.transform.GetChild(4).gameObject.SetActive(false);
		player.GetComponent<InteractionsManager>().interaction.transform.GetChild(3).gameObject.SetActive(false);
	}

	public void StartFire()
	{
		fire.SetActive(true);
	}

	public void FinishFire()
	{
		player.GetComponent<InteractionsManager>().interaction.MarkAsComplete();
		player.GetComponent<PlayerController>().usingHands = false;
		individualInteractions.hasLogs = false;
		player.GetComponent<BoxCollider2D>().enabled = true;
		player.GetComponent<PlayerController>().targetFound = false;

		StartCoroutine(player.GetComponent<PlayerController>().EnablePlayerInput(0));

	}

	public void FallTree()
	{
		player.GetComponent<InteractionsManager>().interaction.transform.GetChild(2).gameObject.SetActive(false);
		player.GetComponent<InteractionsManager>().interaction.MarkAsComplete();
		StartCoroutine(player.GetComponent<PlayerController>().EnablePlayerInput(0));

		individualInteractions.chopCount = 0;
		grid.CreateGrid();



	}

	public void FinishFishing()
	{

		individualInteractions.failed = false;
		individualInteractions.lineCast = false;
		individualInteractions.timer = 0;
		individualInteractions.generated = false;
		individualInteractions.random = 0;
		individualInteractions.fishing = false;
		individualInteractions.fishStage = 0;
		player.GetComponent<PlayerController>().usingRod = false;



		//player.GetComponent<PlayerController>().RevertSprite();
		StartCoroutine(player.GetComponent<PlayerController>().EnablePlayerInput(0));
	}

	public void TriggerTreeFall()
	{
		player.GetComponent<InteractionsManager>().interaction.transform.GetChild(1).GetComponent<Animator>().SetTrigger("fall");

	}


	public void EnablePlayerMovement(float time)
	{

		StartCoroutine(player.GetComponent<PlayerController>().EnablePlayerInput(time));

	}

	#endregion

	#region SoundFX

	public void PlayLightFire()
	{
		player.GetComponent<PlayerSoundManager>().PlayLightFire ();
	}

	public void PlayRockThrow()
	{
		player.GetComponent<PlayerSoundManager>().PlayRockThrow();
	}

	public void PlayCameraShutter()
	{
		player.GetComponent<PlayerSoundManager>().PlayCameraShutter();
	}
	public void PlayLogChop1()
	{
		player.GetComponent<PlayerSoundManager>().PlayChopWood1();
	}

	public void PlayChop2()
	{
		player.GetComponent<PlayerSoundManager>().PlayChopWood2();
	}

	public void PlayDropLogs()
	{
		player.GetComponent<PlayerSoundManager>().PlayDropWood();
	}

	public void PlaySharpenAxe()
	{
		player.GetComponent<PlayerSoundManager>().PlaySharpenAxe();

	}


	public void PlayItemSwitch()
	{
		player.GetComponent<PlayerSoundManager>().PlayItemSwitch();
	}

	public void PlayExtendRod()
	{
		player.GetComponent<PlayerSoundManager>().PlayExtendRod();
	}

	public void PlayCastRod()
	{
		player.GetComponent<PlayerSoundManager>().PlayCastRod();
	}

	public void PlayFloatSplash()
	{
		player.GetComponent<PlayerSoundManager>().PlayFloatSplash();
	}
	public void PlayBite()
	{
		player.GetComponent<PlayerSoundManager>().PlayBite();
	}
	public void PlayReelNoFish()
	{
		player.GetComponent<PlayerSoundManager>().PlayReelNoFish();
	}

	public void PlayReelFish()
	{
		player.GetComponent<PlayerSoundManager>().PlayReelFish();
	}


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
		player.GetComponent<PlayerSoundManager>().PlayMontyFootSteps();
	}

	

	public void OpenDoor()
	{
		door.GetComponent<PlayerSoundManager>().PlayDoorOpen();
	}

	public void CloseDoor()
	{
		door.GetComponent<PlayerSoundManager>().PlayDoorShut();
	}

	public void PutDownCanoe()
	{
		player.GetComponent<PlayerSoundManager>().PlayPutDownCanoe();
	}

}

#endregion