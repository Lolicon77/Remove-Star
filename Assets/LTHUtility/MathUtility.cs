﻿// Author:
// [LongTianhong]
//
// Copyright (C) 2014 Nanjing Xiaoxi Network Technology Co., Ltd. (http://www.mogoomobile.com)

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LTHUtility {

	public class MathUtility : MonoBehaviour {

		public static Vector3 UniformLerp(Vector3 from, Vector3 to, ref float timer, float time) {
			timer += Time.deltaTime;
			return Vector3.Lerp(from, to, timer / time);
		}

		public static float UniformLerp(float from, float to, ref float timer, float time) {
			timer += Time.deltaTime;
			return Mathf.Lerp(from, to, timer / time);
		}

		public static Quaternion UniformLerp(Quaternion from, Quaternion to, ref float timer, float time) {
			timer += Time.deltaTime;
			return Quaternion.Lerp(from, to, timer / time);
		}


	}
}