///-----------------------------------------------------------------
/// Author : Knose1
/// Date : 25/04/2020 15:08
///-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.Serialization;

namespace Com.Github.Knose1.OneSwordHeros.Game.Controllers {
	[CreateAssetMenu(menuName = ASSET_MENU_FOLDER + "/" + nameof(ControllerKeybord), fileName = nameof(ControllerKeybord), order = 0)]
	public class ControllerKeybord : Controller {
		[SerializeField] protected KeyCode left  = KeyCode.LeftArrow;
		[SerializeField] protected KeyCode up	 = KeyCode.UpArrow;
		[SerializeField] protected KeyCode right = KeyCode.RightArrow;
		[SerializeField] protected KeyCode down  = KeyCode.DownArrow;

		[SerializeField, FormerlySerializedAs("Attack_1")] protected KeyCode atk1  = KeyCode.W;
		[SerializeField, FormerlySerializedAs("Attack_2")] protected KeyCode atk2  = KeyCode.X;
		[SerializeField, FormerlySerializedAs("Attack_3")] protected KeyCode atk3  = KeyCode.C;

		public override void UpdateController()
		{
			base.UpdateController();
			
			int left    = Input.GetKey(this.left)	? 1 : 0; 
			int up		= Input.GetKey(this.up)		? 1 : 0;
			int right	= Input.GetKey(this.right)	? 1 : 0;
			int down	= Input.GetKey(this.down)	? 1 : 0;

			Attack1 = Input.GetKey(atk1);
			Attack2 = Input.GetKey(atk2);
			Attack3 = Input.GetKey(atk3);

			Move = Vector2.ClampMagnitude(new Vector2(right - left, up - down), 1);
		}
	}
}