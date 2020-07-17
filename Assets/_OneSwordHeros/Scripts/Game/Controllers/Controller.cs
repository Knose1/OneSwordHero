///-----------------------------------------------------------------
/// Author : Knose1
/// Date : 25/04/2020 15:07
///-----------------------------------------------------------------

using System;
using UnityEngine;

namespace Com.Github.Knose1.OneSwordHeros.Game.Controllers {
	public abstract class Controller : ScriptableObject {
		protected const string ASSET_MENU_FOLDER = OneSwordHeros.ROOT_FOLDER+"/Controller";

		public Vector2 Move { get; protected set; } = Vector2.zero;
		public bool Attack1 { get; protected set; } = false;
		public bool Attack2 { get; protected set; } = false;
		public bool Attack3 { get; protected set; } = false;

		virtual public void Init(){}

		virtual public void UpdateController()
		{
			ResetDefaultValue();
		}

		virtual protected void ResetDefaultValue()
		{
			Move = Vector2.zero;
			Attack1 = false;
			Attack2 = false;
			Attack3 = false;
		}
	}
}