using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace L7 {
	public static class ReflectionExtensionMethods {

		/// <summary>
		/// 得到私有字段的值
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="instance"></param>
		/// <param name="fieldname"></param>
		/// <returns></returns>
		public static T GetPrivateField<T>(this object instance, string fieldname) {
			BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
			Type type = instance.GetType();
			FieldInfo field = type.GetField(fieldname, flag);
			return (T)field.GetValue(instance);
		}

		/// <summary>
		/// 得到私有属性的值
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="instance"></param>
		/// <param name="propertyname"></param>
		/// <returns></returns>
		public static T GetPrivateProperty<T>(this object instance, string propertyname) {
			BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
			Type type = instance.GetType();
			PropertyInfo field = type.GetProperty(propertyname, flag);
			return (T)field.GetValue(instance, null);
		}

		/// <summary>
		/// 设置私有成员的值
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="fieldname"></param>
		/// <param name="value"></param>
		public static void SetPrivateField(this object instance, string fieldname, object value) {
			BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
			Type type = instance.GetType();
			FieldInfo field = type.GetField(fieldname, flag);
			field.SetValue(instance, value);
		}

		/// <summary>
		/// 设置私有属性的值
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="propertyname"></param>
		/// <param name="value"></param>
		public static void SetPrivateProperty(this object instance, string propertyname, object value) {
			BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
			Type type = instance.GetType();
			PropertyInfo field = type.GetProperty(propertyname, flag);
			field.SetValue(instance, value, null);
		}

		/// <summary>
		/// 调用私有方法
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="instance"></param>
		/// <param name="name"></param>
		/// <param name="param"></param>
		/// <returns></returns>
		public static T CallPrivateMethod<T>(this object instance, string name, params object[] param) {
			BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
			Type type = instance.GetType();
			MethodInfo method = type.GetMethod(name, flag);
			return (T)method.Invoke(instance, param);
		}



		/// <summary>
		/// 通过反射找到所以实现指定接口的类并存入集合
		/// </summary>
		/// <param name="interfaceType"></param>
		/// <returns></returns>
		public static List<Type> FindTypesImplementInterface(Type interfaceType) {
			List<Type> types = new List<Type>();
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
				foreach (var type in assembly.GetTypes()) {
					foreach (var t in type.GetInterfaces()) {
						if (t == interfaceType) {
							types.Add(type);
						}
					}
				}
			}
			return types;
		}


	}
}