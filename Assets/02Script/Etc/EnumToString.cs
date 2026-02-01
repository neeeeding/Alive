using System;
using System.ComponentModel;
using UnityEngine;

namespace _02Script.Etc
{
    public class EnumToString : MonoBehaviour
    {
        public static string Name<T>(T wantName)
        {
            var field = wantName.GetType().GetField(wantName.ToString());
            var attr = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attr?.Description??wantName.ToString();
        }
    }
}