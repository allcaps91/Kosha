using ComBase.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ComBase.Controls
{
    public static class clsCollectionExt
    {
        /// <summary>
        /// List<T> 객체에서 특정 프로퍼티를 string[]배열로 변경
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">리스트</param>
        /// <param name="name">프로퍼티명</param>
        /// <returns>string[]</returns>
        public static string[] GetStringArray<T>(this List<T> list, string name)
        {
            if(list == null)
            {
                return null;
            }
            List<string> lt = new List<string>();
            foreach(var item in list)
            {
                if(item.GetPropertieValue(name) != null)
                {
                    lt.Add(item.GetPropertieValue(name).ToString());
                    continue;
                }
            }

            return lt.ToArray();
        }

        /// <summary>
        /// 컬렉션 합치기
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="collection"></param>
        public static void AddRange<T>(this List<T> list, IEnumerable<T> collection)
        {
            if (collection == null)
            {
                return;
            }

            foreach (var item in collection)
            {
                list.Add(item);
            }
        }

        /// <summary>
        /// 컬렉션 합치기
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="collection"></param>
        public static void AddRange<T>(this BindingList<T> list, IEnumerable<T> collection)
        {
            if (collection == null)
            {
                return;
            }

            foreach (var item in collection)
            {
                list.Add(item);
            }
        }

        /// <summary>
        /// 컬렉션으로 변경
        /// var somearray = new int[] {1,2,3,4,5,6,7,8}
        /// Collection<int> numbers = (from item in somearray
        ///                           where item > 4
        ///                           select item).ToCollection();
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns>Collection<T></returns>
        public static Collection<T> ToCollection<T>(this IEnumerable<T> enumerable)
        {
            var collection = new Collection<T>();
            foreach (T i in enumerable)
            {
                collection.Add(i);
            }
                
            return collection;
        }

        /// <summary>
        /// 정렬
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="keySelector"></param>
        /// <param name="descending"></param>
        /// <returns></returns>
        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> enumerable, Func<TSource, TKey> keySelector, bool descending)
        {
            if (enumerable == null)
            {
                return null;
            }

            if (descending)
            {
                return enumerable.OrderByDescending(keySelector);
            }

            return enumerable.OrderBy(keySelector);
        }

        /// <summary>
        /// 정렬 다수
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="keySelector1"></param>
        /// <param name="keySelector2"></param>
        /// <param name="keySelectors"></param>
        /// <returns></returns>
        public static IOrderedEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> enumerable, Func<TSource, IComparable> keySelector1, Func<TSource, IComparable> keySelector2, params Func<TSource, IComparable>[] keySelectors)
        {
            if (enumerable == null)
            {
                return null;
            }

            IEnumerable<TSource> current = enumerable;

            if (keySelectors != null)
            {
                for (int i = keySelectors.Length - 1; i >= 0; i--)
                {
                    current = current.OrderBy(keySelectors[i]);
                }
            }

            current = current.OrderBy(keySelector2);

            return current.OrderBy(keySelector1);
        }

        /// <summary>
        /// 정렬
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="descending"></param>
        /// <param name="keySelector"></param>
        /// <param name="keySelectors"></param>
        /// <returns></returns>
        public static IOrderedEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> enumerable, bool descending, Func<TSource, IComparable> keySelector, params Func<TSource, IComparable>[] keySelectors)
        {
            if (enumerable == null)
            {
                return null;
            }

            IEnumerable<TSource> current = enumerable;

            if (keySelectors != null)
            {
                for (int i = keySelectors.Length - 1; i >= 0; i--)
                {
                    current = current.OrderBy(keySelectors[i], descending);
                }
            }

            return current.OrderBy(keySelector, descending);
        }

        /// <summary>
        /// 기본 동등 비교자를 사용하여 시퀀스에서 첫 번째 항목의 인덱스를 반환합니다.
        /// EX)
        /// int[] numbers = new int[] { 5, 3, 12, 56, 43 };
        /// int index = numbers.IndexOf(123);
        /// </ summary>
        /// <typeparam name = "TSource"> 소스 요소의 유형입니다. </ typeparam>
        /// <param name = "list"> 값을 찾을 시퀀스. </ param>
        /// <param name = "value"> 시퀀스에서 찾을 객체 </ param>
        /// <returns> 전체 시퀀스에서 첫 번째로 발견 된 값의 0부터 시작하는 인덱스 (있는 경우). 그렇지 않으면 -1입니다. </ returns>
        public static int IndexOf<TSource>(this IEnumerable<TSource> list, TSource value) where TSource : IEquatable<TSource>
        {

            return list.IndexOf<TSource>(value, EqualityComparer<TSource>.Default);

        }

        /// <summary>
        /// 지정된 IEqualityComparer를 사용하여 시퀀스에서 첫 번째 항목의 인덱스를 반환합니다.
        /// </ summary>
        /// <typeparam name = "TSource"> 소스 요소의 유형입니다. </ typeparam>
        /// <param name = "list"> 값을 찾을 시퀀스. </ param>
        /// <param name = "value"> 시퀀스에서 찾을 객체 </ param>
        /// <param name = "comparer"> 값을 비교하는 동등 비교 자입니다. </ param>
        /// <returns> 전체 시퀀스에서 첫 번째로 발견 된 값의 0부터 시작하는 인덱스 (있는 경우). 그렇지 않으면 -1입니다. </ returns>
        public static int IndexOf<TSource>(this IEnumerable<TSource> list, TSource value, IEqualityComparer<TSource> comparer)
        {
            int index = 0;
            foreach (var item in list)
            {
                if (comparer.Equals(item, value))
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        /// <summary>
        /// 이 확장 메서드는 IList 인터페이스를 구현하는 컬렉션의 항목을 대체합니다.
        /// Ex)
        /// List<string> strg = new List<string> { "test", "tesssssst2" };
        /// strg.Replace(1,"test2");
        /// </ summary>
        /// <typeparam name = "T"> 조작중인 필드의 유형 </ typeparam>
        /// <param name = "thisList"> 입력 목록 </ param>
        /// <param name = "position"> 이전 항목의 위치 </ param>
        /// <param name = "item"> 장소에 집어 넣을 수있는 아이템 </ param>
        /// <returns> 대체 할 경우 True, 실패하면 false </ returns>
        public static bool Replace<T>(this IList<T> thisList, int position, T item)
        {
            if (position > thisList.Count - 1)
            {
                return false;
            }

            //  이리스트의 범위 안에있는 경우에만 처리한다.
            thisList.RemoveAt(position);
            // 이전 항목을 제거합니다.
            thisList.Insert(position, item);
            // 새 위치를 해당 위치에 삽입합니다.
            return true;
        }

        /// <summary>
        /// 지정된 열거 형의 각 요소 사이에 지정된 구분 문자열을 연결하여 연결된 단일 문자열을 생성합니다.
        /// </ summary>
        /// <typeparam name = "T"> 모든 개체 </ typeparam>
        /// <param name = "list"> 열거 형 </ param>
        /// <param name = "separator"> 문자열 </ param>
        /// <returns> 구분 기호 문자열에 산재 된 값의 요소로 구성된 문자열 </ returns>
        public static string ToString<T>(this IEnumerable<T> list, string separator = ",")
        {
            StringBuilder sb = new StringBuilder();

            foreach (var obj in list)
            {
                PropertyInfo[] propertyInfos = obj.GetType().GetProperties();
                if (propertyInfos.Length > 0)
                {
                    foreach (PropertyInfo info in propertyInfos)
                    {
                        sb.Append(info.Name + " : " + (info.GetValue(obj) == null ? "null" : info.GetValue(obj)) + "\t");
                    }

                    sb.AppendLine(separator);
                }
                else
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(separator);
                    }
                    sb.Append(obj);
                }


            }
            return sb.ToString();
        }

        public static IEnumerable<T> GetEditbleData<T>(this BindingList<T> list)
        {
            return list.Where(r => !r.GetPropertieValue("RowStatus").Equals(RowStatus.None));
        }

        public static IEnumerable<T> GetDeleteData<T>(this BindingList<T> list)
        {
            return list.Where(r => !r.GetPropertieValue("RowStatus").Equals(RowStatus.Delete));
        }

        public static IEnumerable<T> GetDeleteInsert<T>(this BindingList<T> list)
        {
            return list.Where(r => !r.GetPropertieValue("RowStatus").Equals(RowStatus.Insert));
        }

        public static List<T> Copy<T>(this List<T> list)
        {
            T[] copyList = new T[list.Count];
            list.CopyTo(copyList);

            return copyList.OfType<T>().ToList();
        }

        public static List<T> Clone<T>(this List<T> list) where T : new()
        {
            List<T> newList = new List<T>();
            foreach(var item in list)
            {
                PropertyInfo[] propertyInfos = item.GetType().GetProperties();

                T t = new T();
                foreach (PropertyInfo info in propertyInfos)
                {
                    t.SetPropertieValue(info.Name, item.GetPropertieValue(info.Name));
                }

                newList.Add(t);
            }

            return newList;
        }

        /// <summary>
        /// list.DistinctBy(r => r.SITE_ID).ToList();
        /// list.DistinctBy(r => new { r.id, r.name }).ToList();
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
