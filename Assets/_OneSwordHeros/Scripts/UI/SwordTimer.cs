///-----------------------------------------------------------------
/// Author : Knose1
/// Date : 26/04/2020 03:12
///-----------------------------------------------------------------

using Com.Github.Knose1.OneSwordHeros.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Github.Knose1.OneSwordHeros.UI {
	public class SwordTimer : MonoBehaviour {

		[SerializeField] protected Image round = null;
		[SerializeField] protected Image sword = null;
		[SerializeField] protected Text text = null;

		private void Update () {
			GameManager instance = GameManager.Instance;

			string time = instance.SwordTimeout.ToString();
			string[] time2 = time.Split(',');

			if (time2.Length > 1 && time2[1].Length > 0)
			{
				time2[1] = time2[1].Remove(1, time2[1].Length - 1);
				time = time2[0] + ',' + time2[1];
			}

			//Set 1 when the SwordTimeout is 0. Else set the timeout of the round fillAmount
			round.fillAmount = instance.SwordTimeout / instance.SwordPowerDuration;
			sword.fillAmount = instance.SwordTimeout == 0 ? 1 :  round.fillAmount;
			sword.color = instance.SwordTimeout == 0 ? Color.white : Color.black;

			text.text = time;
		}
	}
}