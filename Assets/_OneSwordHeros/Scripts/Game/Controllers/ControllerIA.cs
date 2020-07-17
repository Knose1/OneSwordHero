///-----------------------------------------------------------------
/// Author : Knose1
/// Date : 25/04/2020 15:08
///-----------------------------------------------------------------

using System;
using UnityEngine;

namespace Com.Github.Knose1.OneSwordHeros.Game.Controllers {
	[CreateAssetMenu(menuName = ASSET_MENU_FOLDER + "/" + nameof(ControllerIA), fileName = nameof(ControllerIA), order = 0)]
	public class ControllerIA : Controller
	{
		[System.NonSerialized] public Player player;
		protected Action doAction;

		public override void Init()
		{
			doAction = DoActionVoid;
		}

		public override void UpdateController()
		{
			base.UpdateController();

			doAction();
		}

		protected void DoActionVoid() {}

		protected void DoActionNoSword() {}
		protected void DoActionSword() {}
	}
}