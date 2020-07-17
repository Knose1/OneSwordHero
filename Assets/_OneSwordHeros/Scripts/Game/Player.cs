///-----------------------------------------------------------------
/// Author : Knose1
/// Date : 25/04/2020 20:10
///-----------------------------------------------------------------

using UnityEngine;
using Com.Github.Knose1.Common.Pooling;
using Com.Github.Knose1.OneSwordHeros.Game.Controllers;
using Com.Github.Knose1.OneSwordHeros.Game.Bullet;

namespace Com.Github.Knose1.OneSwordHeros.Game {
	public class Player : MonoBehaviour, IPoolBehaviour {

		public Controller controller;
		[SerializeField] protected Transform sword;
		[SerializeField] protected float speed;

		[SerializeField] protected float hp = 300;

		[SerializeField] protected Attack paternAtk1;
		[SerializeField] protected Attack paternAtk2;
		[SerializeField] protected Attack paternAtk3;

		protected float reloadTimestamp = 0;
		private Vector3 opponentPosition;

		public void PoolStart()
		{
			//throw new System.NotImplementedException();
		}

		public void Move()
		{
			transform.position += new Vector3(controller.Move.x, controller.Move.y) * speed * Time.deltaTime;
		}

		public void ActivateSword(bool active) => sword.gameObject.SetActive(active);

		public void SetOpponentPosition(Vector3 pos)
		{
			opponentPosition = pos;
		}
		public void SwordLookToward(Vector3 lookVector)
		{
			Debug.DrawRay(transform.position, lookVector);
			sword.up = lookVector;
		}

		public void DoAttack()
		{
			if (Time.time < reloadTimestamp) return;

			if		(controller.Attack1)	paternAtk1.Send(transform.position, this);
			else if (controller.Attack2)	paternAtk2.Send(transform.position, this);
			else if (controller.Attack3)	paternAtk3.Send(transform.position, this);
		}

		public void PoolDestroy()
		{
			throw new System.NotImplementedException();
		}

		public void Hurt(float atk)
		{
			Debug.Log($"[{nameof(Player)}] playerHurt, {hp} -> {hp - atk}");
			hp = Mathf.Max(0, hp - atk);

		}

		[System.Serializable]
		public struct Attack
		{
			[SerializeField] public PoolConfig config;
			[SerializeField] public float reloadTime;

			public Attack(PoolConfig config = null, float reloadTime = 0.1f)
			{
				this.config = config;
				this.reloadTime = reloadTime;
			}

			public void Send(Vector3 position, Player player)
			{
				GameObject bulletPaternGO = PoolManager.Instance.Instantiate(config);
				bulletPaternGO.transform.position = position;

				player.reloadTimestamp = Time.time + reloadTime;

				BulletPatern bulletPatern = bulletPaternGO.GetComponent<BulletPatern>();
				

				if (bulletPatern)
				{
					bulletPatern.SetLauncher(player);

					if (bulletPatern is ITargetPatern)
					{
						(bulletPatern as ITargetPatern).SetTarget(player.opponentPosition);
					}
					if (bulletPatern is IPaternRotation)
					{
						(bulletPatern as IPaternRotation).SetRotation(player.sword.rotation);
					}
				}
				else
				{
					Debug.LogError($"{config} is not a {nameof(BulletPatern)}");
				}
			}


		}
	}
}