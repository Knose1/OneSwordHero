///-----------------------------------------------------------------
/// Author : Knose1
/// Date : 25/04/2020 21:14
///-----------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using Com.Github.Knose1.Common.Pooling;
using System.Collections;
using System;

namespace Com.Github.Knose1.OneSwordHeros.Game.Bullet {
	[RequireComponent(typeof(SpriteRenderer)), RequireComponent(typeof(Collider2D))]
	public class Bullet : MonoBehaviour, IPoolBehaviourDestroy, IPoolBehaviourLateStart {

		[SerializeField] protected bool destroyOnCollisionEnter = true;
		[SerializeField] protected float damage = 1;
		[SerializeField] protected float damageRate = 0.1f;

		protected List<Player> damagedPlayer = new List<Player>();

		protected SpriteRenderer spriteRenderer;
		private Player launcher;

		private void Awake()
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
		}

		public void LatePoolStart()
		{
			gameObject.SetActive(true);
			SetAlpha(1);
		}


		public void PoolDestroy()
		{
			StopAllCoroutines();
			gameObject.SetActive(false);
			SetAlpha(0);
			damagedPlayer = new List<Player>();
		}

		private void OnTriggerStay2D(Collider2D collision)
		{
			Player player = collision.GetComponent<Player>();
			if (player)
			{
				if (damagedPlayer.Contains(player)) return;
				if(player == launcher) return;

				player.Hurt(damage);

				if (!destroyOnCollisionEnter)
				{
					damagedPlayer.Add(player);
					StartCoroutine(DamageRateSystemCoroutine(player));
				}
			}

			if (destroyOnCollisionEnter)
			{
				PoolDestroy();
				return;
			}


		}

		public void SetLauncher(Player player)
		{
			launcher = player;
		}

		public void SetAlpha(float a)
		{
			Color color = spriteRenderer.color;
			color.a = a;
			spriteRenderer.color = color;
		}

		private IEnumerator DamageRateSystemCoroutine(Player playerToRemove)
		{
			yield return new WaitForSeconds(damageRate);
			damagedPlayer.Remove(playerToRemove);
		}
	}
}