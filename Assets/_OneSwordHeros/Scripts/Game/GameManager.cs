///-----------------------------------------------------------------
/// Author : Knose1
/// Date : 25/04/2020 21:12
///-----------------------------------------------------------------

using UnityEngine;
using Cinemachine;
using Com.Github.Knose1.Common.Pooling;
using Com.Github.Knose1.OneSwordHeros.Game.Controllers;
using System.Collections.Generic;

namespace Com.Github.Knose1.OneSwordHeros.Game {
	public class GameManager : MonoBehaviour
	{
		private static GameManager _instance;
		public static GameManager Instance => _instance ? _instance : (_instance = FindObjectOfType<GameManager>());

		/*/////////////////////////////////////////////////////*/

		[Header("Config")]
		[SerializeField] protected PoolConfig playerConfig = null;
		[SerializeField] protected PoolConfig swordConfig = null;

		[Header("Controller")]
		[SerializeField] protected List<Controller> controllers = null;

		[Header("Arena")]
		[SerializeField] protected float arenaSize = 5;
		[SerializeField] protected Transform arena = null;
		[SerializeField, Range(0, 1)] protected float playerStartPosition = 0.8f;
		[SerializeField, Range(0, 1)] protected float playableArea = 0.9f;


		[Header("Sword")]
		[SerializeField] protected float _swordPowerDuration = 10;
		public float SwordPowerDuration => _swordPowerDuration;

		[Header("Camera")]
		[SerializeField] protected CinemachineTargetGroup targetGroup = null;
		[SerializeField] protected float playerRadius = 1;
		[SerializeField] protected float playerWeight = 1;
		[SerializeField] protected float swordWeight = 1;
		[SerializeField] protected float swordRadius = 1;

		/*/////////////////////////////////////////////////////*/

		protected SwordCollectible sword = null;

		private Player _playerWithTheSword = null;
		public Player PlayerWithTheSword => _playerWithTheSword;

		private List<Player> players = new List<Player>();

		private float swordTimestamp = 0;
		public float SwordTimeout { get; protected set; }

		private void Start()
		{
			swordTimestamp = -_swordPowerDuration;

			arena.localScale = new Vector3(arenaSize * 2, arenaSize * 2, 1);

			int count = controllers.Count;
			for (int i = count - 1; i >= 0; i--)
			{
				Quaternion quaternion = Quaternion.AngleAxis(i * 360 / count, Vector3.forward);

				Player player = PoolManager.Instance.Instantiate(playerConfig).GetComponent<Player>();

				targetGroup.AddMember(player.transform, playerWeight, playerRadius);

				player.transform.position = quaternion * new Vector2(arenaSize * playerStartPosition, 0);
				player.controller = controllers[i];
				
				controllers[i].Init();
				players.Add(player);
			}


			DropSword(Vector2.zero);
		}

		private void Update()
		{
			SwordTimeout = Mathf.Max(0, swordTimestamp + _swordPowerDuration - Time.time);
			if (SwordTimeout <= 0 && _playerWithTheSword)
			{
				_playerWithTheSword.ActivateSword(false);
				_playerWithTheSword = null;
				DropSword();
			}

			for (int i = controllers.Count - 1; i >= 0; i--)
			{
				controllers[i].UpdateController();
			}

			for (int i = players.Count - 1; i >= 0; i--)
			{
				Player player = players[i];
				player.Move();

				if (player.transform.position.sqrMagnitude > playableArea * playableArea * arenaSize * arenaSize)
				{
					player.transform.position = Vector3.ClampMagnitude(player.transform.position, playableArea * arenaSize);
				}
			}

			if (players.Count > 1)
			{
				Player anotherPlayer = players[(players.IndexOf(_playerWithTheSword) + 1) % players.Count];
				_playerWithTheSword?.SwordLookToward(anotherPlayer.transform.position - _playerWithTheSword.transform.position);

				_playerWithTheSword?.SetOpponentPosition(anotherPlayer.transform.position);
			}

			_playerWithTheSword?.DoAttack();

		}

		private void DropSword() => DropSword(RandomInArena());

		private void DropSword(Vector2 position)
		{
			sword = PoolManager.Instance.Instantiate(swordConfig).GetComponent<SwordCollectible>();
			sword.transform.position = position;
			sword.OnCollect += Sword_OnCollect;

			targetGroup.AddMember(sword.transform, swordWeight, swordRadius);

		}

		private void Sword_OnCollect(Player obj)
		{
			targetGroup.RemoveMember(sword.transform);

			swordTimestamp = Time.time;
			sword.OnCollect -= Sword_OnCollect;
			obj.ActivateSword(true);
			_playerWithTheSword = obj;
		}

		private Vector2 RandomInArena() => Random.insideUnitCircle * arenaSize * playableArea;
	}
}