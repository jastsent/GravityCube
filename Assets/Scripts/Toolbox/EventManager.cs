//using System;
//using System.Collections.Generic;
//using UnityEngine;
//using JastSent;
//
//namespace JastSent {
//	public static class EventManager {
//		public delegate void Callback();
//		public delegate void Callback<T>(T arg1);
//		public delegate void Callback<T, U>(T arg1, U arg2);
//		public delegate void Callback<T, U, V>(T arg1, U arg2, V arg3);
//
//		//all listeners and callbacks
//		static private Dictionary<string, List<Delegate>> listeners = new Dictionary<string, List<Delegate>>();
//		static private Dictionary<string, List<Delegate>> listeners1 = new Dictionary<string, List<Delegate>>();
//		static private Dictionary<string, List<Delegate>> listeners2 = new Dictionary<string, List<Delegate>>();
//		static private Dictionary<string, List<Delegate>> listeners3 = new Dictionary<string, List<Delegate>>();
//		//triggered events saves here until all messages is sent
//		static private Dictionary<string, List<List<Delegate>>> triggered = new Dictionary<string, List<List<Delegate>>>();
//		static private Dictionary<string, List<List<Delegate>>> triggered1 = new Dictionary<string, List<List<Delegate>>>();
//		static private Dictionary<string, List<List<Delegate>>> triggered2 = new Dictionary<string, List<List<Delegate>>>();
//		static private Dictionary<string, List<List<Delegate>>> triggered3 = new Dictionary<string, List<List<Delegate>>>();
//
//		//ADD LISTENER FUNCTIONS
//		/////
//		public static void AddListener(string eventType, Callback ev){
//			if (!listeners.ContainsKey(eventType)){
//				List<Delegate> list = new List<Delegate> ();
//				list.Add (ev);
//				listeners.Add(eventType, list);
//			} else {
//				if (!listeners [eventType].Contains (ev)) {
//					listeners [eventType].Add (ev);
//				}
//			}
//		}
//		// 1 argument
//		public static void AddListener<T>(string eventType, Callback<T> ev){
//			if (!listeners1.ContainsKey(eventType)){
//				List<Delegate> list = new List<Delegate> ();
//				list.Add (ev);
//				listeners1.Add(eventType, list);
//			} else {
//				if (!listeners1 [eventType].Contains (ev)) {
//					listeners1 [eventType].Add (ev);
//				}
//			}
//		}
//		// 2 arguments
//		public static void AddListener<T, U>(string eventType, Callback<T, U> ev){
//			if (!listeners2.ContainsKey(eventType)){
//				List<Delegate> list = new List<Delegate> ();
//				list.Add (ev);
//				listeners2.Add(eventType, list);
//			} else {
//				if (!listeners2 [eventType].Contains (ev)) {
//					listeners2 [eventType].Add (ev);
//				}
//			}
//		}
//		// 3 arguments
//		public static void AddListener<T, U, V>(string eventType, Callback<T, U, V> ev){
//			if (!listeners3.ContainsKey(eventType)){
//				List<Delegate> list = new List<Delegate> ();
//				list.Add (ev);
//				listeners3.Add(eventType, list);
//			} else {
//				if (!listeners3 [eventType].Contains (ev)) {
//					listeners3 [eventType].Add (ev);
//				}
//			}
//		}
//
//		//REMOVE LISTENER FUNCTIONS
//		/////
//
//
//		public static void RemoveListener(string eventType, Callback ev){
//			if (!listeners.ContainsKey(eventType)){
//				return;
//			} else {
//				if(listeners[eventType].Contains(ev)) listeners[eventType].Remove(ev);
//
//				if (!triggered.ContainsKey(eventType)){
//					return;
//				} else {
//					for (int i = 0; i < triggered [eventType].Count; i++) {
//						if(triggered[eventType][i].Contains(ev))
//							triggered[eventType][i].Remove(ev);
//					}
//				}
//			}
//		}
//
//		//1 argument
//		public static void RemoveListener<T>(string eventType, Callback<T> ev){
//			if (!listeners1.ContainsKey(eventType)){
//				return;
//			} else {
//				if(listeners1[eventType].Contains(ev)) listeners1[eventType].Remove(ev);
//
//				if (!triggered1.ContainsKey(eventType)){
//					return;
//				} else {
//					for (int i = 0; i < triggered1 [eventType].Count; i++) {
//						if(triggered1[eventType][i].Contains(ev))
//							triggered1[eventType][i].Remove(ev);
//					}
//				}
//			}
//		}
//
//		//2 arguments
//		public static void RemoveListener<T, U>(string eventType, Callback<T, U> ev){
//			if (!listeners2.ContainsKey(eventType)){
//				return;
//			} else {
//				if(listeners2[eventType].Contains(ev)) listeners2[eventType].Remove(ev);
//
//				if (!triggered2.ContainsKey(eventType)){
//					return;
//				} else {
//					for (int i = 0; i < triggered2 [eventType].Count; i++) {
//						if(triggered2[eventType][i].Contains(ev))
//							triggered2[eventType][i].Remove(ev);
//					}
//				}
//			}
//		}
//
//		//3 arguments
//		public static void RemoveListener<T, U, V>(string eventType, Callback<T, U, V> ev){
//			if (!listeners3.ContainsKey(eventType)){
//				return;
//			} else {
//				if(listeners3[eventType].Contains(ev)) listeners3[eventType].Remove(ev);
//
//				if (!triggered3.ContainsKey(eventType)){
//					return;
//				} else {
//					for (int i = 0; i < triggered3 [eventType].Count; i++) {
//						if(triggered3[eventType][i].Contains(ev))
//							triggered3[eventType][i].Remove(ev);
//					}
//				}
//			}
//		}
//
//		//BROADCAST FUNCTIONS
//		/////
//
//		public static void Trigger(string eventType){
//			if (listeners.ContainsKey (eventType)) {
//				/*if (listeners.ContainsKey (eventType)) {
//					List<Delegate> l = new List<Delegate> (listeners [eventType]);
//					while (l.Count > 0) {
//						Callback call = l [0] as Callback;
//						call ();
//						l.RemoveAt (0);
//					}
//				}*/
//				int iteration = 0;
//				if (triggered.ContainsKey (eventType)) {
//					iteration = triggered [eventType].Count;
//					List<Delegate> list = new List<Delegate>(listeners [eventType]);
//					triggered [eventType].Add (list);
//				} else {
//					List<List<Delegate>> lists = new List<List<Delegate>> ();
//					List<Delegate> list = new List<Delegate>(listeners [eventType]);
//					lists.Add (list);
//					triggered.Add (eventType, lists);
//				}
//				while (triggered[eventType][iteration].Count > 0) {
//					Callback call = triggered[eventType][iteration] [0] as Callback;
//					triggered[eventType][iteration].RemoveAt (0);
//					call ();
//				}
//				if(iteration == 0)
//					triggered.Remove (eventType);
//			}
//		}
//
//		//1 argument
//		public static void Trigger<T>(string eventType, T param1){
//			if (listeners1.ContainsKey (eventType)) {
//				/*List<Delegate> l = new List<Delegate>(listeners1[eventType]);
//				while (l.Count > 0) {
//					Callback<T> call = l [0] as Callback<T>;
//					call (param1);
//					l.RemoveAt (0);
//				}*/
//				int iteration = 0;
//				if (triggered1.ContainsKey (eventType)) {
//					iteration = triggered1 [eventType].Count;
//					List<Delegate> list = new List<Delegate>(listeners1 [eventType]);
//					triggered1 [eventType].Add (list);
//				} else {
//					List<List<Delegate>> lists = new List<List<Delegate>> ();
//					List<Delegate> list = new List<Delegate>(listeners1 [eventType]);
//					lists.Add (list);
//					triggered1.Add (eventType, lists);
//				}
//				while (triggered1[eventType][iteration].Count > 0) {
//					Callback<T> call = triggered1[eventType][iteration] [0] as Callback<T>;
//					triggered1[eventType][iteration].RemoveAt (0);
//					call (param1);
//				}
//				if(iteration == 0)
//					triggered1.Remove (eventType);
//			}
//		}
//
//		//2 arguments
//		public static void Trigger<T, U>(string eventType, T param1, U param2){
//			if (listeners2.ContainsKey (eventType)) {
//				/*List<Delegate> l = new List<Delegate> (listeners2 [eventType]);
//				while (l.Count > 0) {
//					Callback<T, U> call = l [0] as Callback<T, U>;
//					call (param1, param2);
//					l.RemoveAt (0);
//				}*/
//				int iteration = 0;
//				if (triggered2.ContainsKey (eventType)) {
//					iteration = triggered2 [eventType].Count;
//					List<Delegate> list = new List<Delegate>(listeners2 [eventType]);
//					triggered2 [eventType].Add (list);
//				} else {
//					List<List<Delegate>> lists = new List<List<Delegate>> ();
//					List<Delegate> list = new List<Delegate>(listeners2 [eventType]);
//					lists.Add (list);
//					triggered2.Add (eventType, lists);
//				}
//				while (triggered2[eventType][iteration].Count > 0) {
//					Callback<T, U> call = triggered2[eventType][iteration] [0] as Callback<T, U>;
//					triggered2[eventType][iteration].RemoveAt (0);
//					call (param1, param2);
//				}
//				if(iteration == 0)
//					triggered2.Remove (eventType);
//			}		
//		}
//
//		//3 arguments
//		public static void Trigger<T, U, V>(string eventType, T param1, U param2, V param3){
//			if (listeners3.ContainsKey (eventType)) {
//				/*List<Delegate> l = new List<Delegate> (listeners3 [eventType]);
//				while (l.Count > 0) {
//					Callback<T, U, V> call = l [0] as Callback<T, U, V>;
//					call (param1, param2, param3);
//					l.RemoveAt (0);
//				}*/
//				int iteration = 0;
//				if (triggered3.ContainsKey (eventType)) {
//					iteration = triggered3 [eventType].Count;
//					List<Delegate> list = new List<Delegate>(listeners3 [eventType]);
//					triggered3 [eventType].Add (list);
//				} else {
//					List<List<Delegate>> lists = new List<List<Delegate>> ();
//					List<Delegate> list = new List<Delegate>(listeners3 [eventType]);
//					lists.Add (list);
//					triggered3.Add (eventType, lists);
//				}
//				while (triggered3[eventType][iteration].Count > 0) {
//					Callback<T, U, V> call = triggered3[eventType][iteration] [0] as Callback<T, U, V>;
//					triggered3[eventType][iteration].RemoveAt (0);
//					call (param1, param2, param3);
//				}
//				if(iteration == 0)
//					triggered3.Remove (eventType);	
//			}
//		}
//
//		/// OTHER
//		/*public static bool Contains(string eventType, Callback ev){
//			if (listeners.ContainsKey (eventType)) {
//				for (int i = 0; i < listeners [eventType].Co; i++) {
//					if (listeners [eventType] .Contains (ev))
//						return true;
//				}
//				return false;
//			} else
//				return false;
//		}*/
//	}
//}

using System;
using System.Collections.Generic;

namespace JastSent {
	public static class EventManager {
		public delegate void Callback();
		public delegate void Callback<T>(T arg1);
		public delegate void Callback<T, U>(T arg1, U arg2);
		public delegate void Callback<T, U, V>(T arg1, U arg2, V arg3);

		//all listeners and callbacks
		static private Dictionary<int, List<Delegate>> listeners = new Dictionary<int, List<Delegate>>();
		static private Dictionary<int, List<Delegate>> listeners1 = new Dictionary<int, List<Delegate>>();
		static private Dictionary<int, List<Delegate>> listeners2 = new Dictionary<int, List<Delegate>>();
		static private Dictionary<int, List<Delegate>> listeners3 = new Dictionary<int, List<Delegate>>();
		//triggered events saves here until all messages is sent
		static private Dictionary<int, List<List<Delegate>>> triggered = new Dictionary<int, List<List<Delegate>>>();
		static private Dictionary<int, List<List<Delegate>>> triggered1 = new Dictionary<int, List<List<Delegate>>>();
		static private Dictionary<int, List<List<Delegate>>> triggered2 = new Dictionary<int, List<List<Delegate>>>();
		static private Dictionary<int, List<List<Delegate>>> triggered3 = new Dictionary<int, List<List<Delegate>>>();

		//ADD LISTENER FUNCTIONS
		/////
		public static void AddListener(int eventType, Callback ev){
			if (!listeners.ContainsKey(eventType)){
				List<Delegate> list = new List<Delegate> ();
				list.Add (ev);
				listeners.Add(eventType, list);
			} else {
				if (!listeners [eventType].Contains (ev)) {
					listeners [eventType].Add (ev);
				}
			}
		}
		// 1 argument
		public static void AddListener<T>(int eventType, Callback<T> ev){
			if (!listeners1.ContainsKey(eventType)){
				List<Delegate> list = new List<Delegate> ();
				list.Add (ev);
				listeners1.Add(eventType, list);
			} else {
				if (!listeners1 [eventType].Contains (ev)) {
					listeners1 [eventType].Add (ev);
				}
			}
		}
		// 2 arguments
		public static void AddListener<T, U>(int eventType, Callback<T, U> ev){
			if (!listeners2.ContainsKey(eventType)){
				List<Delegate> list = new List<Delegate> ();
				list.Add (ev);
				listeners2.Add(eventType, list);
			} else {
				if (!listeners2 [eventType].Contains (ev)) {
					listeners2 [eventType].Add (ev);
				}
			}
		}
		// 3 arguments
		public static void AddListener<T, U, V>(int eventType, Callback<T, U, V> ev){
			if (!listeners3.ContainsKey(eventType)){
				List<Delegate> list = new List<Delegate> ();
				list.Add (ev);
				listeners3.Add(eventType, list);
			} else {
				if (!listeners3 [eventType].Contains (ev)) {
					listeners3 [eventType].Add (ev);
				}
			}
		}

		//REMOVE LISTENER FUNCTIONS
		/////


		public static void RemoveListener(int eventType, Callback ev){
			if (!listeners.ContainsKey(eventType)){
				return;
			} else {
				if(listeners[eventType].Contains(ev)) listeners[eventType].Remove(ev);

				if (!triggered.ContainsKey(eventType)){
					return;
				} else {
					for (int i = 0; i < triggered [eventType].Count; i++) {
						if(triggered[eventType][i].Contains(ev))
							triggered[eventType][i].Remove(ev);
					}
				}
			}
		}

		//1 argument
		public static void RemoveListener<T>(int eventType, Callback<T> ev){
			if (!listeners1.ContainsKey(eventType)){
				return;
			} else {
				if(listeners1[eventType].Contains(ev)) listeners1[eventType].Remove(ev);

				if (!triggered1.ContainsKey(eventType)){
					return;
				} else {
					for (int i = 0; i < triggered1 [eventType].Count; i++) {
						if(triggered1[eventType][i].Contains(ev))
							triggered1[eventType][i].Remove(ev);
					}
				}
			}
		}

		//2 arguments
		public static void RemoveListener<T, U>(int eventType, Callback<T, U> ev){
			if (!listeners2.ContainsKey(eventType)){
				return;
			} else {
				if(listeners2[eventType].Contains(ev)) listeners2[eventType].Remove(ev);

				if (!triggered2.ContainsKey(eventType)){
					return;
				} else {
					for (int i = 0; i < triggered2 [eventType].Count; i++) {
						if(triggered2[eventType][i].Contains(ev))
							triggered2[eventType][i].Remove(ev);
					}
				}
			}
		}

		//3 arguments
		public static void RemoveListener<T, U, V>(int eventType, Callback<T, U, V> ev){
			if (!listeners3.ContainsKey(eventType)){
				return;
			} else {
				if(listeners3[eventType].Contains(ev)) listeners3[eventType].Remove(ev);

				if (!triggered3.ContainsKey(eventType)){
					return;
				} else {
					for (int i = 0; i < triggered3 [eventType].Count; i++) {
						if(triggered3[eventType][i].Contains(ev))
							triggered3[eventType][i].Remove(ev);
					}
				}
			}
		}

		//BROADCAST FUNCTIONS
		/////

		public static void Trigger(int eventType){
			if (listeners.ContainsKey (eventType)) {
				/*if (listeners.ContainsKey (eventType)) {
					List<Delegate> l = new List<Delegate> (listeners [eventType]);
					while (l.Count > 0) {
						Callback call = l [0] as Callback;
						call ();
						l.RemoveAt (0);
					}
				}*/
				int iteration = 0;
				if (triggered.ContainsKey (eventType)) {
					iteration = triggered [eventType].Count;
					List<Delegate> list = new List<Delegate>(listeners [eventType]);
					triggered [eventType].Add (list);
				} else {
					List<List<Delegate>> lists = new List<List<Delegate>> ();
					List<Delegate> list = new List<Delegate>(listeners [eventType]);
					lists.Add (list);
					triggered.Add (eventType, lists);
				}
				while (triggered[eventType][iteration].Count > 0) {
					Callback call = triggered[eventType][iteration] [0] as Callback;
					triggered[eventType][iteration].RemoveAt (0);
					call ();
				}
				if (iteration == 0)
					triggered [eventType].Clear ();
			}
		}

		//1 argument
		public static void Trigger<T>(int eventType, T param1){
			if (listeners1.ContainsKey (eventType)) {
				/*List<Delegate> l = new List<Delegate>(listeners1[eventType]);
				while (l.Count > 0) {
					Callback<T> call = l [0] as Callback<T>;
					call (param1);
					l.RemoveAt (0);
				}*/
				int iteration = 0;
				if (triggered1.ContainsKey (eventType)) {
					iteration = triggered1 [eventType].Count;
					List<Delegate> list = new List<Delegate>(listeners1 [eventType]);
					triggered1 [eventType].Add (list);
				} else {
					List<List<Delegate>> lists = new List<List<Delegate>> ();
					List<Delegate> list = new List<Delegate>(listeners1 [eventType]);
					lists.Add (list);
					triggered1.Add (eventType, lists);
				}
				while (triggered1[eventType][iteration].Count > 0) {
					Callback<T> call = triggered1[eventType][iteration] [0] as Callback<T>;
					triggered1[eventType][iteration].RemoveAt (0);
					call (param1);
				}
				if(iteration == 0)
					triggered1[eventType].Clear ();
			}
		}

		//2 arguments
		public static void Trigger<T, U>(int eventType, T param1, U param2){
			if (listeners2.ContainsKey (eventType)) {
				/*List<Delegate> l = new List<Delegate> (listeners2 [eventType]);
				while (l.Count > 0) {
					Callback<T, U> call = l [0] as Callback<T, U>;
					call (param1, param2);
					l.RemoveAt (0);
				}*/
				int iteration = 0;
				if (triggered2.ContainsKey (eventType)) {
					iteration = triggered2 [eventType].Count;
					List<Delegate> list = new List<Delegate>(listeners2 [eventType]);
					triggered2 [eventType].Add (list);
				} else {
					List<List<Delegate>> lists = new List<List<Delegate>> ();
					List<Delegate> list = new List<Delegate>(listeners2 [eventType]);
					lists.Add (list);
					triggered2.Add (eventType, lists);
				}
				while (triggered2[eventType][iteration].Count > 0) {
					Callback<T, U> call = triggered2[eventType][iteration] [0] as Callback<T, U>;
					triggered2[eventType][iteration].RemoveAt (0);
					call (param1, param2);
				}
				if(iteration == 0)
					triggered2[eventType].Clear ();
			}		
		}

		//3 arguments
		public static void Trigger<T, U, V>(int eventType, T param1, U param2, V param3){
			if (listeners3.ContainsKey (eventType)) {
				/*List<Delegate> l = new List<Delegate> (listeners3 [eventType]);
				while (l.Count > 0) {
					Callback<T, U, V> call = l [0] as Callback<T, U, V>;
					call (param1, param2, param3);
					l.RemoveAt (0);
				}*/
				int iteration = 0;
				if (triggered3.ContainsKey (eventType)) {
					iteration = triggered3 [eventType].Count;
					List<Delegate> list = new List<Delegate>(listeners3 [eventType]);
					triggered3 [eventType].Add (list);
				} else {
					List<List<Delegate>> lists = new List<List<Delegate>> ();
					List<Delegate> list = new List<Delegate>(listeners3 [eventType]);
					lists.Add (list);
					triggered3.Add (eventType, lists);
				}
				while (triggered3[eventType][iteration].Count > 0) {
					Callback<T, U, V> call = triggered3[eventType][iteration] [0] as Callback<T, U, V>;
					triggered3[eventType][iteration].RemoveAt (0);
					call (param1, param2, param3);
				}
				if(iteration == 0)
					triggered3[eventType].Clear ();
			}
		}

		/// OTHER
		/*public static bool Contains(string eventType, Callback ev){
			if (listeners.ContainsKey (eventType)) {
				for (int i = 0; i < listeners [eventType].Co; i++) {
					if (listeners [eventType] .Contains (ev))
						return true;
				}
				return false;
			} else
				return false;
		}*/
	}
}