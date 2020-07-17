///-----------------------------------------------------------------
/// Author : Knose1
/// Date : 26/04/2020 18:58
///-----------------------------------------------------------------

using Com.Github.Knose1.Common.Pooling;
using UnityEngine;

namespace Com.Github.Knose1.OneSwordHeros.Game.Bullet
{
	[RequireComponent(typeof(Animator))]
	public class BulletPaternUsingAnimator : BulletPatern {

		[SerializeField] protected string shootTrigger = "Shoot";
		private Animator animator;

		protected override void Awake()
		{
			animator = GetComponent<Animator>();
			base.Awake();
		}

		public override void PoolStart()
		{
			animator.SetTrigger(shootTrigger);
		}

		virtual public void OnAnimatorEnd()
		{
			PoolManager.Instance.Destroy(gameObject);
		}
	}
}