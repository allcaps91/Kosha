using ComBase.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
namespace ComBase.Controls
{
    public static class clsComboBoxExt
    {
        /// <summary>
        /// 아이템 선택
        /// </summary>
        /// <param name="comboBox"></param>
        /// <param name="data">Value 값</param>
        public static void SetValue(this ComboBox comboBox, object data)
        {
            if(data == null)
            {
                comboBox.SelectedIndex = -1;
                return;
            }

            if(comboBox.DataSource != null)
            {
                for (int i = 0; i < comboBox.Items.Count; i++)
                {
                    var item = comboBox.Items[i];
                    if(item.GetPropertieValue(comboBox.ValueMember).Equals(data))
                    {
                        comboBox.SelectedIndex = i;
                        break;
                    }
                }
            }
            else
            {
                if (comboBox.Tag is ComboBoxOption)
                {
                    ComboBoxOption option = (comboBox.Tag as ComboBoxOption);
                    if (option.Items != null)
                    {
                        for (int i = 0; i < option.Items.Length; i++)
                        {
                            var item = option.Items[i];

                            if (data != null)
                            {
                                if (data.Equals(item.GetPropertieValue(option.ValueMember).Trim()))
                                {
                                    comboBox.SelectedIndex = i;
                                    break;
                                }
                            }
                        }
                    }

                }
                else
                {
                    for (int i = 0; i < comboBox.Items.Count; i++)
                    {
                        string item = comboBox.Items[i].ToString();
                        string[] values = item.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);

                        if (values == null || values.Length < 1)
                        {
                            continue;
                        }
                        if (values[0].Trim().Equals(data))
                        {
                            comboBox.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 선택 값
        /// </summary>
        /// <param name="comboBox"></param>
        /// <returns>String</returns>
        public static string GetValue(this ComboBox comboBox)
        {
            if (comboBox.SelectedIndex < 0)
            {
                return string.Empty;
            }

            if(comboBox.DataSource != null)
            {
                var item = comboBox.SelectedItem;

                if(comboBox.ValueMember.IsNullOrEmpty())
                {
                    return string.Empty;
                }

                return item.GetPropertieValue(comboBox.ValueMember).ToString().Trim();
            }
            else
            {
                if (comboBox.Tag is ComboBoxOption)
                {
                    ComboBoxOption option = (comboBox.Tag as ComboBoxOption);
                    return option.Items[comboBox.SelectedIndex].GetPropertieValue(option.ValueMember).ToString().Trim();
                }
                else
                {
                    string item = comboBox.Items[comboBox.SelectedIndex].ToString();
                    string[] values = item.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);

                    return values[0].Trim();
                }
            }
            //return comboBox.Items[comboBox.SelectedIndex].ToString();
        }

        /// <summary>
        /// 선택 아이템 Class
        /// </summary>
        /// <param name="comboBox"></param>
        /// <returns>Class</returns>
        public static object GetSelectedItem(this ComboBox comboBox)
        {
            if (comboBox.SelectedIndex < 0)
            {
                return null;
            }

            if (comboBox.Tag is ComboBoxOption)
            {
                ComboBoxOption option = (comboBox.Tag as ComboBoxOption);
                return option.Items[comboBox.SelectedIndex];
            }

            return comboBox.SelectedItem;
        }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="comboBox"></param>
        public static void Initialize(this ComboBox comboBox)
        {
            if(comboBox.DataBindings != null)
            {
                comboBox.DataBindings.Clear();
            }
            comboBox.SelectedIndex = -1;
        }

        public static void SetDataSource<T>(this ComboBox comboBox, List<T> list, string displayMember, string valueMember, bool isShowValue, bool isShowDisplay = true) where T : BaseDto, new()
        {
            SetDataSource(true, comboBox, list, displayMember, valueMember, isShowValue, isShowDisplay);
        }

        private static void SetDataSource<T>(bool isPrivete, ComboBox comboBox, List<T> list, string displayMember, string valueMember, bool isShowValue, bool isShowDisplay = true) where T : BaseDto, new()
        {
            if (list == null || list.Count < 1)
            {
                comboBox.DataSource = list;
                return;
            }

            list.ForEach(r =>
            {
                string value = string.Empty;
                if(r.GetPropertieValue(valueMember).NotEmpty())
                {
                    if(r.GetPropertieValue(valueMember).NotEmpty())
                    {
                        value = r.GetPropertieValue(valueMember).ToString().Trim();
                    }
                    r.SetPropertieValue(valueMember, value);
                }

                if(isShowValue && !isShowDisplay)
                {
                    displayMember = valueMember;
                }
                else if(isShowDisplay && isShowValue)
                {
                    //value = string.Concat(r.GetPropertieValue(valueMember).ToString(), ".", r.GetPropertieValue(displayMember).ToString());
                    value = string.Empty;
                    if(r.GetPropertieValue(valueMember).NotEmpty())
                    {
                        string display = string.Empty;
                        if(r.GetPropertieValue(displayMember).NotEmpty())
                        {
                            display = r.GetPropertieValue(displayMember).ToString();
                        }

                        if (r.GetPropertieValue(valueMember).NotEmpty())
                        {
                            value = r.GetPropertieValue(valueMember).ToString().Trim();
                        }

                        if(display.NotEmpty() && value.NotEmpty())
                        {
                            value = string.Concat(value, ".", display);
                        }
                            
                    }
                    r.SetPropertieValue(displayMember, value);
                }

                //r.ComboDisplay = value;
            });

            try
            {
                comboBox.DataSource = list;
                comboBox.ValueMember = valueMember;// "COM_CD";
                comboBox.DisplayMember = displayMember;// "ComboDisplay";
                
            }
            catch(Exception e)
            {

            }
            
        }

        /// <summary>
        /// 콤보박스 매핑
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="comboBox"></param>
        /// <param name="items"></param>
        /// <param name="displayMember"></param>
        /// <param name="valueMember"></param>
        /// <param name="addDisplayMember"></param>
        /// <param name="addValueMember"></param>
        /// <param name="addComboBoxPosition"></param>
        public static void SetItems<T>(this ComboBox comboBox, List<T> list, string displayMember, string valueMember, string addDisplayMember = "", string addValueMember = "", AddComboBoxPosition addComboBoxPosition = AddComboBoxPosition.None, bool IsDisplayValue = true) where T : BaseDto, new()
        {
            SetItems(comboBox, list, displayMember, valueMember, addDisplayMember, addValueMember, addComboBoxPosition, IsDisplayValue, true);
        }

        public static void SetItems<T>(this ComboBox comboBox, List<T> list, string displayMember, string valueMember, bool IsShowValue, bool IsShowDisplay = true) where T : BaseDto, new()
        {
            SetItems(comboBox, list, displayMember, valueMember, string.Empty, string.Empty, AddComboBoxPosition.None , IsShowValue , IsShowDisplay);
        }

        public static void SetItems2<T>(this ComboBox comboBox, List<T> list, string displayMember, string valueMember, bool IsShowValue, bool IsShowDisplay = true, string addDisplayMember = "", string addValueMember = "", AddComboBoxPosition addComboBoxPosition = AddComboBoxPosition.None) where T : BaseDto, new()
        {
            SetItems(comboBox, list, displayMember, valueMember, addDisplayMember, addValueMember, addComboBoxPosition, IsShowValue, IsShowDisplay);
        }

        public static void SetDIsplayItems<T>(this ComboBox comboBox, List<T> list, string displayMember, string valueMember, string addDisplayMember = "", string addValueMember = "", AddComboBoxPosition addComboBoxPosition = AddComboBoxPosition.None) where T : BaseDto, new()
        {
            SetItems(comboBox, list, displayMember, valueMember, addDisplayMember, addValueMember, addComboBoxPosition, false, true); 
        }

        private static void SetItems<T>(ComboBox comboBox, List<T> list, string displayMember, string valueMember, string addDisplayMember = "", string addValueMember = "", AddComboBoxPosition addComboBoxPosition = AddComboBoxPosition.None, bool IsShowValue = true, bool IsShowDisplay = true) where T : BaseDto, new()
        {
            if (list == null || list.Count < 1)
            {
                return;
            }
            List<T> items = new List<T>();
            items = list.ToList();
            comboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            foreach (var item in items)
            {
                object display = item.GetPropertieValue(displayMember);
                object value = item.GetPropertieValue(valueMember);
                string text = string.Empty;

                if (IsShowDisplay && !IsShowValue)
                {
                    text = display.ToString().Trim();
                }
                else if (!IsShowDisplay && IsShowValue)
                {
                    text = value.ToString().Trim();
                }
                else
                {
                    text = string.Concat(value.ToString().Trim(), ".", display.ToString().Trim());
                }

                comboBox.Items.Add(text);
            }

            if (addComboBoxPosition == AddComboBoxPosition.Bottom)
            {
                string text = string.Empty;
                if (IsShowDisplay && IsShowValue)
                {
                    text = string.Concat(valueMember.ToString().Trim(), ".", addDisplayMember.ToString().Trim());
                }
                else if (IsShowDisplay && !IsShowValue)
                {
                    text = addDisplayMember.ToString().Trim();
                }
                else if (!IsShowDisplay && IsShowValue)
                {
                    text = addValueMember.ToString().Trim();
                }
                comboBox.Items.Add(text);

                T t = new T();
                t.SetPropertieValue(valueMember, addValueMember);
                t.SetPropertieValue(displayMember, addDisplayMember);
                items.Add(t);

            }

            if (addComboBoxPosition == AddComboBoxPosition.Top)
            {
                string text = string.Empty;
                if (IsShowDisplay && IsShowValue)
                {
                    text = string.Concat(addValueMember.ToString().Trim(), ".", addDisplayMember.ToString().Trim());
                }
                else if (IsShowDisplay && !IsShowValue)
                {
                    text = addDisplayMember.ToString().Trim();
                }
                else if (!IsShowDisplay && IsShowValue)
                {
                    text = addValueMember.ToString().Trim();
                }
                comboBox.Items.Insert(0, text);

                T t = new T();
                t.SetPropertieValue(valueMember, addValueMember);
                t.SetPropertieValue(displayMember, addDisplayMember);
                //items.Add(t);

                items.Insert(0, t);
            }

            string dataField = string.Empty;
            ComboBoxOption option = new ComboBoxOption();
            if (comboBox.Tag is string)
            {
                dataField = comboBox.Tag.ToString();
            }
            else if (comboBox.Tag is ComboBoxOption)
            {
                option = (comboBox.Tag as ComboBoxOption);
                dataField = option.DataField;
            }

            option.Items = items.ToArray();
            option.DataField = dataField;
            option.ValueMember = valueMember;
            option.DisplayMember = displayMember;
            option.AddValueMember = addValueMember;
            option.AddDisplayMember = addDisplayMember;
            option.ItemPosition = addComboBoxPosition;

            comboBox.Tag = option;
        }

        public static string GetDataField(this ComboBox comboBox)
        {
            if(comboBox.Tag is ComboBoxOption)
            {
                return (comboBox.Tag as ComboBoxOption).DataField;
            }
            if(comboBox.Tag is string)
            {
                return comboBox.Tag.ToString();
            }
            return string.Empty;
        }

        public static ComboBoxOption GetOption(this ComboBox comboBox)
        {
            if (comboBox.Tag is ComboBoxOption)
            {
                return comboBox.Tag as ComboBoxOption;
            }

            return null;
        }

        public static void Clear(this ComboBox comboBox)
        {
            comboBox.Items.Clear();
        }

        public static void SetOptions(this ComboBox comboBox, ComboBoxOption option)
        {
            string dataField = string.Empty;
            if (comboBox.Tag != null)
            {
                if (comboBox.Tag is string)
                {
                    dataField = comboBox.Tag.ToString();
                }
            }

            if (!string.IsNullOrWhiteSpace(dataField))
            {
                option.DataField = dataField;
            }

            comboBox.Tag = option;

            //if(option.Items != null)
            //{
            //    List<BaseDto> list = option.Items.OfType<BaseDto>().ToList();
            //    //private static void SetItems<T>(ComboBox comboBox, List<T> list, string displayMember, string valueMember, string addDisplayMember = "", string addValueMember = "", AddComboBoxPosition addComboBoxPosition = AddComboBoxPosition.None, bool IsShowValue = true, bool IsShowDisplay = true) where T : BaseDto, new()

            //    List<ComComCdDto> comList1 = new List<ComComCdDto>();
            //    comList1.Add(new ComComCdDto { COM_CD = "1", COM_CD_NM = "일" });
            //    comList1.Add(new ComComCdDto { COM_CD = "2", COM_CD_NM = "이" });
            //    comList1.Add(new ComComCdDto { COM_CD = "3", COM_CD_NM = "삼" });
            //    comList1.Add(new ComComCdDto { COM_CD = "4", COM_CD_NM = "사" });
            //    comList1.Add(new ComComCdDto { COM_CD = "5", COM_CD_NM = "오" });
            //    SetDataSource(true, comboBox, list, option.DisplayMember, option.ValueMember, true);
            //}
        }
    }

    public class ComboBoxOption : IControlOption
    {
        public object[] Items { get; set; }
        public string DataField { get; set; }
        public string DisplayMember { get; set; }
        public string ValueMember { get; set; }
        public string AddDisplayMember { get; set; }
        public string AddValueMember { get; set; }
        public AddComboBoxPosition ItemPosition { get; set; }


    }
   
    public enum AddComboBoxPosition
    {
        None,
        Top,
        Bottom
    }
}
