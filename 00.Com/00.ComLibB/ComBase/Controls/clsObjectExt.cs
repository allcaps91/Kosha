using ComBase.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ComBase.Controls
{
    public static class clsObjectExt
    {
        /// <summary>
        /// Object가 클래스 타입인경우 특정 프로퍼티 값 추출
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object GetPropertieValue(this object obj, string name)
        {
            if(name == "noiseC1")
            {
                string ss = "";
            }
            object dataValue = null;
            if (obj == null || name == null)
            {
                return null;
            }
            PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach (PropertyInfo info in properties)
            {
                
                if (info.Name.ToUpper().Equals(name.ToUpper()))
                {
                    dataValue = info.GetValue(obj, null);
                    break;
                }
            }

            return dataValue;
        }


        /// <summary>
        /// 프로퍼티 값 설정
        /// </summary>
        /// <param name="obj">클래스</param>
        /// <param name="name">프로퍼티명</param>
        /// <param name="value">값</param>
        public static void SetPropertieValue(this object obj, string name, object value)
        {
            if (obj == null)
            {
                return;
            }

            PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach (PropertyInfo info in properties)
            {
                if (info.Name.ToUpper().Equals(name.ToUpper()))
                {
                    if(info.PropertyType.Name == "Int64")
                    {
                       
                        info.SetValue(obj, value.To<long>(0));
                    }
                    else
                    {
                        info.SetValue(obj, value);
                    }
              
                    break;
                }
            }
        }

        public static string Trim(this object obj)
        {
            if(!(obj is string))
            {
                return string.Empty;
            }

            return obj.ToString().Trim();
        }

        /// <summary>
        /// Null Empty, 공백 체크
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this object obj)
        {
            return obj == null || string.IsNullOrWhiteSpace(obj.ToString()) ? true : false;
        }

        public static bool NotEmpty(this object obj)
        {
            if (obj != null)
            {
                if (!string.IsNullOrWhiteSpace(obj.ToString()))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool Empty(this object obj)
        {
            if (obj == null)
            {
                return true;
                
            }
            else
            {
                if (string.IsNullOrWhiteSpace(obj.ToString()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            
        }
        /// <summary>
        /// 설정값이 두 값 사이에 있는지 체크
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static bool Between<T>(this T value, T from, T to) where T : IComparable<T>
        {
            return value.CompareTo(from) >= 0 && value.CompareTo(to) <= 0;
        }

        /// <summary>
        /// 형변형
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T To<T>(this object value)
        {
            try
            {
                Type t = typeof(T);
                Type u = Nullable.GetUnderlyingType(t);

                if (u != null)
                {
                    if (value == null || value.Equals(""))
                    {
                        return default(T);
                    }

                    return (T)Convert.ChangeType(value, u);
                }
                else
                {
                    if (value == null || value.Equals(""))
                    {
                        return default(T);
                    }

                    return (T)Convert.ChangeType(value, t);
                }
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// 형 변환 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="DefaultValue">기본값</param>
        /// <returns></returns>
        public static T To<T>(this object value, T DefaultValue)
        {
            try
            {
                Type t = typeof(T);
                Type u = Nullable.GetUnderlyingType(t);

                if (u != null)
                {
                    if (value == null || value.Equals(""))
                    {
                        return DefaultValue;
                    }

                    return (T)Convert.ChangeType(value, u);
                }
                else
                {
                    if (value == null || value.Equals(""))
                    {
                        return DefaultValue;
                    }

                    return (T)Convert.ChangeType(value, t);
                }
            }
            catch
            {
                return DefaultValue;
            }
        }

        /// <summary>
        /// 한 유형을 다른 유형으로 변환합니다.
        /// 예:
        /// var age = "28";
        /// var intAge = age.To <int> ();
        /// var doubleAge = intAge.To <double> ();
        /// var decimalAge = doubleAge.To <decimal> ();
        /// </ summary>
        /// <typeparam name = "T"> </ typeparam>
        /// <param name = "value"> </ param>
        /// <returns> </ returns>
        public static T To<T>(this IConvertible value)
        {
            try
            {
                Type t = typeof(T);
                Type u = Nullable.GetUnderlyingType(t);

                if (u != null)
                {
                    if (value == null || value.Equals(""))
                    {
                        return default(T);
                    }

                    return (T)Convert.ChangeType(value, u);
                }
                else
                {
                    if (value == null || value.Equals(""))
                    {
                        return default(T);
                    }

                    value = value.ToString();

                    return (T)Convert.ChangeType(value, t);
                }
            }
            catch
            {
                return default(T);
            }
        }

        public static T To<T>(this IConvertible value, T DefaultValue)
        {
            try
            {
                Type t = typeof(T);
                Type u = Nullable.GetUnderlyingType(t);

                if (u != null)
                {
                    if (value == null || value.Equals(""))
                    {
                        return DefaultValue;
                    }

                    return (T)Convert.ChangeType(value, u);
                }
                else
                {
                    if (value == null || value.Equals(""))
                    {
                        return DefaultValue;
                    }

                    return (T)Convert.ChangeType(value, t);
                }
            }
            catch
            {
                return DefaultValue;
            }
        }

        public static T IsNullThen<T>(this object obj, T alt)
        {
            if(obj.IsNullOrEmpty())
            {
                return alt;
            }
            else
            {
                return (T)obj;
            }
        }

        public static string ToString<T>(this T obj)
        {
            StringBuilder sb = new StringBuilder();

            if(obj.IsNullOrEmpty())
            {
                return string.Empty;
            }

            PropertyInfo[] propertyInfos = obj.GetType().GetProperties();
            if (propertyInfos.Length > 0)
            {
                foreach (PropertyInfo info in propertyInfos)
                {
                    sb.Append(info.Name + " : " + (info.GetValue(obj) == null ? "null" : info.GetValue(obj)) + "\t");
                }
            }
            else
            {
                sb.Append(obj);


            }

            return sb.ToString();
        }

        public static Dictionary<string, object> ToDictionary<T>(this T dto)
        {
            Dictionary<string, object> dic = dto.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(dto, null));

            return dic;
        }
    }
}
