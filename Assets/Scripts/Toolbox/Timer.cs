using System;
using UnityEngine;

namespace JastSent {

	public class Timer {

		public delegate void Callback();

		public float time = 0f;
		public float endTime = 0f;
		public bool end = false;
		public bool stop = false;
		public bool onPause = false;
		public Delegate callback = null;
		//public Delegate methodUpdate = null;

		void FrameUpdate(){
			if (onPause || (!Game.pause && !onPause)) {
				time += Time.deltaTime;
				if (time >= endTime) {						
					end = true;
					stop = true;
					if (callback != null) {
						Callback call = callback as Callback;
						call ();
					}
					EventManager.RemoveListener (Keys.Messages.Update, FrameUpdate);
				} /*else {
					if (methodUpdate != null) {
						Callback call = methodUpdate as Callback;
						call ();
					}
				}*/
			}
		}

		public Timer(float timeCount, bool OnPause, bool stopOnStart, Callback callbackEnd){
			if (time < timeCount) {
				end = false;
				if (stopOnStart)
					stop = true;
				else
					EventManager.AddListener (Keys.Messages.Update, FrameUpdate);
			} else {
				end = true;
				stop = true;
				if (callbackEnd != null)
					callbackEnd ();
			}
			endTime = timeCount;
			onPause = OnPause;
			callback = callbackEnd;
		}

		public Timer(float timeCount, bool OnPause, bool stopOnStart){
			if (time < timeCount) {
				end = false;
				if (stopOnStart)
					stop = true;
				else
					EventManager.AddListener (Keys.Messages.Update, FrameUpdate);
			} else {
				end = true;
				stop = true;
			}
			endTime = timeCount;
			onPause = OnPause;
		}

		public void Reset(){
			SetTime (0f);
		}

		public void Replay(){
			SetTime (0f);
			Play ();
		}

		public void SetTime(float newTime){
			if (newTime < endTime) {
				end = false;
			} else {
				end = true;
				if (!stop)
					Remove ();
			}
			time = newTime;
		}

		public void SetEndTime(float newEndTime){
			if (time < newEndTime) {
				end = false;
			} else {
				end = true;
				if (!stop)
					Remove ();
			}
			endTime = newEndTime;
		}

		public void SetTimes(float newTime, float newEndTime){	
			if (newTime < newEndTime) {
				end = false;
			} else {
				end = true;
				if (!stop)
					Remove ();
			}
			time = newTime;
			endTime = newEndTime;
		}

		public void Stop(){
			if (!stop) {
				Remove ();
			}
		}

		public void Play(){
			if (stop && !end) {
				Add ();
			}
		}

		private void Remove(){
			EventManager.RemoveListener (Keys.Messages.Update, FrameUpdate);
			stop = true;
		}
		private void Add(){
			EventManager.AddListener (Keys.Messages.Update, FrameUpdate);
			stop = false;
		}
	}
}
