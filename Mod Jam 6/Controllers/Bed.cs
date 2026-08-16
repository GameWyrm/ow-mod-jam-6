using System;
using UnityEngine;

namespace Mod_Jam_6
{
    class Bed : MonoBehaviour
    {
		public enum State
		{
			NORMAL = 0,
			VOIDED = 1,
		}

		public enum Floor
		{
			THIRD = 0,
			SECOND = 2,
			FIRST = 4
		}

		public Floor floor;

		private const float SLEEPING_DISTANCE = 2f;

		[SerializeField]
		private State _initialState = State.NORMAL;

		[SerializeField]
		private Sector _sector;

		[Space]
		[SerializeField]
		private bool _lookUpWhileSleeping = true;

		[Space]
		[SerializeField]
		protected OWAudioSource _audio;

		[SerializeField]
		protected OWAudioSource _oneShotAudio;

		[Space]
		[SerializeField]
		private SingleInteractionVolume _interactVolume;

		[SerializeField]
		private PlayerAttachPoint _attachPoint;

		private PlayerLockOnTargeting _lockOnTargeting;

		protected State _state;

		private bool _isPlayerSleeping;

		private bool _isTimeFastForwarding;

		private bool _playerInSector;

		private float _fastForwardStartTime;

		private float _fastForwardMultiplier = 1f;

		private ScreenPrompt _wakePrompt;

		private void Awake()
		{
			if (_interactVolume != null)
			{
				_interactVolume.OnPressInteract += OnPressInteract;
				_wakePrompt = new ScreenPrompt(InputLibrary.interact, UITextLibrary.GetString(UITextType.WakeUpPrompt));
			}
			if (_sector != null)
			{
				_sector.OnSectorOccupantsUpdated += new OWEvent.OWCallback(OnSectorOccupantsUpdated);
			}
		}

		private void Start()
		{
			TimeManager.instance.timeEvents[(int)floor].AddListener(OnFloorVoiding);
			_interactVolume?.ChangePrompt(UITextType.CampfireDozeOff);
			if (Locator.GetPlayerTransform() != null)
			{
				_lockOnTargeting = Locator.GetPlayerTransform().GetRequiredComponent<PlayerLockOnTargeting>();
			}
			_audio?.SetLocalVolume(0f);
			base.enabled = false;
			SetState(_initialState, forceStateUpdate: true);
		}

		private void OnDestroy()
		{
			if (_interactVolume != null)
			{
				_interactVolume.OnPressInteract -= OnPressInteract;
			}
			if (_sector != null)
			{
				_sector.OnSectorOccupantsUpdated -= new OWEvent.OWCallback(OnSectorOccupantsUpdated);
			}
		}

		public Sector GetSector()
		{
			return _sector;
		}

		public State GetState()
		{
			return _state;
		}

		public void SetInteractionEnabled(bool enabled)
		{
			if (enabled)
			{
				_interactVolume?.EnableInteraction();
			}
			else
			{
				_interactVolume?.DisableInteraction();
			}
		}

		public void PlayOneShot(AudioType audioType)
		{
			_oneShotAudio.PlayOneShot(audioType);
		}

		public void SetState(State newState, bool forceStateUpdate = false)
		{
			if (_state == newState && !forceStateUpdate) { return; }

			_state = newState;	
			base.enabled = true;
		}

		private void OnFloorVoiding()
        {
			SetState(State.VOIDED);
        }

		private void OnPressInteract()
		{
			if (!_isPlayerSleeping && CanSleepHereNow() && OWInput.IsInputMode(InputMode.Character))
			{
				StartSleeping();
			}
		}

		private void UpdateDisplayState()
        {
			_interactVolume?._screenPrompt.SetDisplayState((!CanSleepHereNow()) ? ScreenPrompt.DisplayState.GrayedOut : ScreenPrompt.DisplayState.Normal);
		}

		private void StartSleeping()
		{
			Locator.GetToolModeSwapper().UnequipTool();

			if (_attachPoint != null)
			{
				_attachPoint.transform.position = Locator.GetPlayerBody().transform.position;
				_attachPoint.AttachPlayer();
			}
			_interactVolume.DisableInteraction();
			if (_lookUpWhileSleeping)
			{
				_lockOnTargeting.LockOn(base.transform, Vector3.up * 10f, 1f, useZoom: true);
			}
			else
			{
				_lockOnTargeting.LockOn(base.transform, Vector3.up * 0.75f, 1f, useZoom: true);
			}
			Locator.GetPlayerCamera().GetComponent<PlayerCameraEffectController>().CloseEyes(3f);
			Locator.GetAudioMixer().MixSleepAtCampfire(3f);
			Locator.GetPlayerAudioController().OnStartSleepingAtCampfire(isDreamCampfire: false);
			_fastForwardStartTime = Time.timeSinceLevelLoad + 3f;
			_isPlayerSleeping = true;
			Locator.GetPromptManager().AddScreenPrompt(_wakePrompt, PromptPosition.Center);
			_wakePrompt.SetVisibility(isVisible: false);
			OWInput.ChangeInputMode(InputMode.None);
			if (Locator.GetPlayerSuit().IsWearingSuit())
			{
				Locator.GetPlayerSuit().RemoveHelmet();
			}
			Locator.GetFlashlight().TurnOff(playAudio: false);
			GlobalMessenger<bool>.FireEvent("StartSleepingAtCampfire", false);
		}

		public void StopSleeping(bool sudden = false)
		{
			if (_isPlayerSleeping)
			{
				if (_isTimeFastForwarding)
				{
					StopFastForwarding();
				}
				if (_attachPoint != null)
				{
					_attachPoint.DetachPlayer();
				}
				_lockOnTargeting.BreakLock();
				_interactVolume.EnableInteraction();
				if (_lookUpWhileSleeping || PlayerState.InZeroG())
				{
					Locator.GetPlayerCamera().GetComponent<PlayerCameraController>().CenterCamera(50f, smoothStep: true);
				}
				Locator.GetPlayerCamera().GetComponent<PlayerCameraEffectController>().OpenEyes(1f, sudden);
				Locator.GetAudioMixer().UnmixSleepAtCampfire(sudden ? 1f : 3f);
				Locator.GetPlayerAudioController().OnStopSleepingAtCampfire(sudden || Time.timeSinceLevelLoad - _fastForwardStartTime > 60f, sudden);
				_isPlayerSleeping = false;
				Locator.GetPromptManager().RemoveScreenPrompt(_wakePrompt);
				OWInput.ChangeInputMode(InputMode.Character);
				if (Locator.GetPlayerSuit().IsWearingSuit())
				{
					Locator.GetPlayerSuit().PutOnHelmetAfterDelay(2f);
				}
				GlobalMessenger.FireEvent("StopSleepingAtCampfire");
			}
		}

		private void StartFastForwarding()
		{
			Locator.GetPlayerCamera().enabled = false;
			_isTimeFastForwarding = true;
			_fastForwardMultiplier = 1f;
			OWTime.SetMaxDeltaTime(1f / 30f);
			GlobalMessenger.FireEvent("StartFastForward");
		}

		private void StopFastForwarding()
		{
			Locator.GetPlayerCamera().enabled = true;
			_isTimeFastForwarding = false;
			_fastForwardMultiplier = 1f;
			OWTime.SetTimeScale(1f);
			OWTime.SetMaxDeltaTime(1f / 15f);
			GlobalMessenger.FireEvent("EndFastForward");
		}

		private void Update()
		{
			if (_isPlayerSleeping && !_isTimeFastForwarding && Time.timeSinceLevelLoad > _fastForwardStartTime)
			{
				StartFastForwarding();
			}
			if (_isTimeFastForwarding)
			{
				_wakePrompt.SetVisibility(OWInput.IsInputMode(InputMode.None) && Time.timeSinceLevelLoad - _fastForwardStartTime > GetWakePromptDelay());
				if (ShouldWakeUp())
				{
					StopSleeping();
				}
				else if (!OWTime.IsPaused())
				{
					_fastForwardMultiplier = Mathf.MoveTowards(_fastForwardMultiplier, 10f, 2f * Time.unscaledDeltaTime);
					OWTime.SetTimeScale(_fastForwardMultiplier);
				}
			}
			UpdateDisplayState(); // Probably not the best place to put this (in term of performance) but lazy
		}

		private float GetWakePromptDelay()
		{			
			return 10f; // How long before the prompt shows up on the sleep timer window
		}

		private bool CanSleepHereNow()
		{
			// If bed state okay, if enough time in loop + if normal controls -> can sleep
			return _state == State.NORMAL && TimeLoop.IsTimeFlowing() && TimeLoop.GetSecondsRemaining() > 85f && OWInput.IsInputMode(InputMode.Character);
		}

		private bool ShouldWakeUp()
		{
			// If one of the 3 cancel button is pressed (input mode None when asleep) then wake up
			if (OWInput.IsInputMode(InputMode.None) && (OWInput.IsNewlyPressed(InputLibrary.interact) || OWInput.IsNewlyPressed(InputLibrary.cancel) || OWInput.IsNewlyPressed(InputLibrary.interactSecondary)))
            {
                return true;
            }
			// Also wake up if bed becoming voided
			if(_state == State.VOIDED)
            {
				return true;
            }
			// Also wake up if late
            return TimeLoop.GetSecondsRemaining() < 85f;
        }

		private void OnSectorOccupantsUpdated()
		{
			bool flag = _sector.ContainsOccupant(DynamicOccupant.Player);
			if (_playerInSector != flag)
			{
				_playerInSector = flag;
				base.enabled = _playerInSector;
			}
		}
	}
}

