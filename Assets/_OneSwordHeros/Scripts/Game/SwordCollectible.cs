///-----------------------------------------------------------------
/// Author : Knose1
/// Date : 25/04/2020 21:12
///-----------------------------------------------------------------

using UnityEngine;
using Com.Github.Knose1.Common.Pooling;
using System;

namespace Com.Github.Knose1.OneSwordHeros.Game {
	public class SwordCollectible : MonoBehaviour, IPoolBehaviourDestroy {

		public event Action<Player> OnCollect;
		
		[SerializeField] protected string playerTag = "Player";

		private void OnTriggerEnter2D(Collider2D collision)
		{
			Debug.Log($"[{nameof(SwordCollectible)}] TriggerEnter2D");

			if (collision.CompareTag(playerTag))
			{
				Debug.Log($"[{nameof(SwordCollectible)}] it's the player");

				OnCollect?.Invoke(collision.GetComponent<Player>());

				PoolManager.Instance.Destroy(gameObject);
			}
		}

		public void PoolDestroy() 
		{
			OnCollect = null;
		}
	}
}