///-----------------------------------------------------------------
/// Author : Knose1
/// Date : 26/04/2020 14:26
///-----------------------------------------------------------------

using UnityEngine;
using Com.Github.Knose1.Common.Pooling;

namespace Com.Github.Knose1.OneSwordHeros.Game.Bullet {

	public interface ITargetPatern
	{
		void SetTarget(Vector3 position);
	}

	public interface IPaternRotation
	{
		void SetRotation(Quaternion rotation);
	}

	public abstract class BulletPatern : MonoBehaviour, IPoolBehaviourStart {

		protected Bullet[] bullets;
		protected Vector3[] bulletsStartPosition;

		virtual protected void Awake () {
			bullets = GetComponentsInChildren<Bullet>(true);

			bulletsStartPosition = new Vector3[bullets.Length];

			for (int i = bullets.Length - 1; i >= 0; i--)
			{
				bulletsStartPosition.SetValue(bullets[i].transform.position - transform.position, i);
			}
		}

		virtual public void PoolStart()
		{
			for (int i = bullets.Length - 1; i >= 0; i--)
			{
				bullets[i].transform.position = transform.position + bulletsStartPosition[i];
			}
		}

		public void SetLauncher(Player player)
		{
			for (int i = bullets.Length - 1; i >= 0; i--)
			{
				bullets[i].SetLauncher(player);
			}
		}
	}
}