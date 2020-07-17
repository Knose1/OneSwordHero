///-----------------------------------------------------------------
/// Author : Knose1
/// Date : 26/04/2020 16:07
///-----------------------------------------------------------------

using Com.Github.Knose1.Common.Pooling;
using System;
using UnityEngine;

namespace Com.Github.Knose1.OneSwordHeros.Game.Bullet {
	public class BulletPatern2Sword : BulletPatern, ITargetPatern, IPaternRotation, IPoolBehaviourDestroy
	{
		private Vector3 target;
		private Quaternion rotation;
		[SerializeField] public float lerpTime = 1;
		[SerializeField] public AnimationCurve moveCurve = AnimationCurve.Linear(0,0,1,1);
		[SerializeField] public AnimationCurve alphaCurve = AnimationCurve.Constant(0,1,1);
		
		protected Vector3[] bulletsPoolStartPosition;

		private float timestamp;

		public Action DoAction { get; private set; }

		protected override void Awake()
		{
			base.Awake();
			bulletsPoolStartPosition = new Vector3[bullets.Length];
		}

		public override void PoolStart()
		{
			for (int i = bullets.Length - 1; i >= 0; i--)
			{
				Bullet bullet = bullets[i];

				bullet.transform.rotation = rotation;

				bulletsPoolStartPosition[i] = bullet.transform.position = rotation * (bulletsStartPosition[i]) + transform.position;
			}

			timestamp = Time.time + lerpTime;

			DoAction = DoActionNormal;
		}

		public void SetTarget(Vector3 position)
		{
			target = position;
		}

		public void SetRotation(Quaternion rotation)
		{
			this.rotation = rotation;
		}

		private void Update () {
			DoAction?.Invoke();
		}

		public void PoolDestroy()
		{
			DoAction = null;
		}

		public void DoActionNormal()
		{
			if (timestamp < 0) return;

			float linearRatio = Mathf.Clamp(1 - (timestamp - Time.time) / lerpTime, 0, 1);

			float ratio = moveCurve.Evaluate(linearRatio);

			if (ratio > 0.99) ratio = 1;
			
			for (int i = bullets.Length - 1; i >= 0; i--)
			{
				Bullet bullet = bullets[i];
				Vector3 toReach = Vector3.LerpUnclamped(bulletsPoolStartPosition[i], target, ratio);

				bullet.SetAlpha(alphaCurve.Evaluate(linearRatio));

				bullet.transform.position = toReach;
			}
			if (ratio == 1)
			{
				timestamp = -1;
				PoolManager.Instance.Destroy(gameObject);
			}
		}
	}
}