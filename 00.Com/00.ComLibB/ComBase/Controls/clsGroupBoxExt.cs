using ComBase.Mvc;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace ComBase.Controls
{
    public static class clsGroupBoxExt
    {
        /// <summary>
        /// 그룹박스 초기화
        /// </summary>
        /// <param name="p"></param>
        public static void Initialize(this GroupBox group)
        {
            List<Control> list = new List<Control>();
            FindControls(ref list, group);

            foreach (Control control in list)
            {
                if (control is TextBox)
                {
                    (control as TextBox).Initialize();
                }
                else if (control is ComboBox)
                {
                    (control as ComboBox).Initialize();
                }
                else if (control is CheckBox)
                {
                    (control as CheckBox).Initialize();
                }
                else if (control is RadioButton)
                {
                    (control as RadioButton).Initialize();
                }
                else if (control is RichTextBox)
                {
                    (control as RichTextBox).Initialize();
                }
                else if (control is DateTimePicker)
                {
                    (control as DateTimePicker).Initialize();
                }
            }
        }

        /// <summary>
        /// 그룹박스 안의 컨트롤 엔터키 바인딩
        /// </summary>
        /// <param name="groupBox"></param>
        public static void SetEnterKey(this GroupBox groupBox)
        {
            List<Control> list = new List<Control>();
            FindControls(ref list, groupBox);

            foreach (Control control in list)
            {
                if (control is TextBox)
                {
                    if (!(control as TextBox).Multiline)
                    {
                        (control as TextBox).KeyDown -= GroupBoxExt_KeyDown;
                        (control as TextBox).KeyDown += GroupBoxExt_KeyDown;
                    }
                }
                else if (control is ComboBox)
                {
                    (control as ComboBox).KeyDown -= GroupBoxExt_KeyDown;
                    (control as ComboBox).KeyDown += GroupBoxExt_KeyDown;
                }
                else if (control is DateTimePicker)
                {
                    (control as DateTimePicker).KeyDown -= GroupBoxExt_KeyDown;
                    (control as DateTimePicker).KeyDown += GroupBoxExt_KeyDown;
                }
            }
        }

        /// <summary>
        /// 컨트롤 찾기
        /// </summary>
        /// <param name="list"></param>
        /// <param name="control"></param>
        private static void FindControls(ref List<Control> list, Control control)
        {
            foreach (Control item in control.Controls)
            {
                if (item.HasChildren)
                {
                    FindControls(ref list, item);
                }
                else
                {
                    list.Add(item);
                }
            }
        }

        private static void GroupBoxExt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        /// <summary>
        /// 데이터 바인딩
        /// </summary>
        /// <param name="groupBox"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool SetData(this GroupBox groupBox, object data)
        {
            if (groupBox.Tag is GroupBoxOption)
            {
                (groupBox.Tag as GroupBoxOption).Data = data;
            }
            else
            {
                groupBox.Tag = new GroupBoxOption { Data = data };
            }

            List<Control> list = new List<Control>();
            FindControls(ref list, groupBox);
            return SetControlDataBind(list, data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="groupBox"></param>
        /// <returns></returns>
        public static T GetData<T>(this GroupBox groupBox) where T : new()
        {
            T t = new T();
            List<Control> list = new List<Control>();
            FindControls(ref list, groupBox);

            return GetData<T>(list, t);
        }

        /// <summary>
        /// 컨트롤 테이터 class 반환
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private static T GetData<T>(List<Control> list, T t)
        {
            PropertyInfo[] properties = t.GetType().GetProperties();
            foreach (var item in list)
            {
                if (item.Tag == null || string.IsNullOrWhiteSpace(item.Tag.ToString()))
                {
                    continue;
                }

                string dataField = string.Empty;
                if (item.Tag is IControlOption)
                {
                    dataField = (item.Tag as IControlOption).DataField;
                }
                else
                {
                    dataField = item.Tag.ToString();
                }
                if(item.Name == "NumCheckCount1")
                {
                    string xx = "sssssd";
                }
                if(dataField == "CheckCount1")
                {
                    string xx = "d";
                }
                if (dataField.Contains("."))
                {
                    string[] temp = dataField.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                    string fieldName = temp[1];

                    object obj = t;
                    PropertyInfo pInfo = null;

                    for (int i = 0; i < temp.Length; i++)
                    {
                        string part = temp[i];

                        Type type = obj.GetType();
                        pInfo = type.GetProperty(part);

                        if (i == 0)
                        {
                            obj = pInfo.GetValue(obj, null);
                        }
                        else
                        {
                            pInfo.SetValue(obj, GetControlValue(item));
                        }
                    }

                    foreach (PropertyInfo info in obj.GetType().GetProperties())
                    {
                        if (pInfo.GetValue(obj) != null)
                        {
                            continue;
                        }
                        if (fieldName.Equals(info.Name))
                        {
                            pInfo.SetValue(obj, GetControlValue(item));
                        }
                    }
                }
                else
                {
                    foreach (PropertyInfo info in properties)
                    {
                        if (info.GetValue(t) != null)
                        {
                            continue;
                        }

                        if (info.Name.Equals(dataField))
                        {
                            info.SetValue(t, GetControlValue(item));
                            break;
                        }
                    }
                }
            }
            return t;
        }

        /// <summary>
        /// 컨트롤 값가져오기
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private static object GetControlValue(Control item)
        {
            if (item is TextBox)
            {
                return (item as TextBox).GetValue();
            }
            else if (item is CheckBox)
            {
                return (item as CheckBox).GetValue();
            }
            else if (item is ComboBox)
            {
                return (item as ComboBox).GetValue();
            }
            else if (item is DateTimePicker)
            {
                return (item as DateTimePicker).GetValue();
            }
            else if (item is PictureBox)
            {
                return (item as PictureBox).GetValue();
            }
            else if (item is RadioButton)
            {
                return (item as RadioButton).GetValue();
            }
            else if (item is RichTextBox)
            {
                return (item as RichTextBox).GetValue();
            }

            return null;
        }

        /// <summary>
        /// 데이터 바인딩
        /// </summary>
        /// <param name="list"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        private static bool SetControlDataBind(List<Control> list, object dto)
        {
            PropertyInfo[] properties = dto.GetType().GetProperties();
            try
            {
                foreach (Control item in list)
                {
                    if (item.Tag == null || string.IsNullOrWhiteSpace(item.Tag.ToString()))
                    {
                        continue;
                    }

                    string dataField = string.Empty;
                    if (item.Tag is IControlOption)
                    {
                        dataField = (item.Tag as IControlOption).DataField;
                    }
                    else
                    {
                        dataField = item.Tag.ToString();
                    }

                    foreach (PropertyInfo info in properties)
                    {
                        if (dataField.Contains("."))
                        {
                            string[] temp = dataField.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                            string field = temp[1];

                            foreach (PropertyInfo pInfo in info.PropertyType.GetRuntimeProperties())
                            {
                                if (pInfo.Name.Equals(field))
                                {
                                    object obj = dto;
                                    PropertyInfo pinfo = null;

                                    for (int i = 0; i < temp.Length; i++)
                                    {
                                        string part = temp[i];

                                        Type type = obj.GetType();
                                        pinfo = type.GetProperty(part);

                                        if (i == 0)
                                        {
                                            obj = pinfo.GetValue(obj, null);
                                        }
                                        else
                                        {
                                            SetControlValue(item, info.GetValue(dto));
                                        }
                                    }
                                }
                            }
                            continue;
                        }

                        if (info.Name.Equals(dataField))
                        {
                            SetControlValue(item, info.GetValue(dto));
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 컨트롤 값넣기
        /// </summary>
        /// <param name="item"></param>
        /// <param name="data"></param>
        private static void SetControlValue(Control item, object data)
        {
            if (item is TextBox)
            {
                (item as TextBox).SetValue(data);
            }
            else if (item is CheckBox)
            {
                (item as CheckBox).SetValue(data);
            }
            else if (item is ComboBox)
            {
                (item as ComboBox).SetValue(data);
            }
            else if (item is DateTimePicker)
            {
                (item as DateTimePicker).SetValue(data);
            }
            else if (item is PictureBox)
            {
                (item as PictureBox).SetValue(data);
            }
            else if (item is RadioButton)
            {
                (item as RadioButton).SetValue(data);
            }
            else if (item is RichTextBox)
            {
                (item as RichTextBox).SetValue(data);
            }
        }

        public static void AddRequiredControl(this GroupBox groupBox, Control control)
        {
            GroupBoxOption option = null;
            if (groupBox.Tag == null)
            {
                option = new GroupBoxOption();
                groupBox.Tag = option;
            }
            else
            {
                option = groupBox.Tag as GroupBoxOption;
            }

            control.TextChanged += (s, e) =>
            {
                BaseForm form = (s as Control).TopLevelControl as BaseForm;
                form.errorProvider1.SetError(control, "");
            };
            option.AddRequiredControl(control);
        }

        /// <summary>
        /// 필수입력
        /// </summary>
        /// <param name="groupBox"></param>
        /// <returns></returns>
        public static bool RequiredValidate(this GroupBox groupBox)
        {
            if (groupBox.Tag == null || !(groupBox.Tag is GroupBoxOption))
            {
                return false;
            }

            List<Control> list = new List<Control>();
            bool result = true;
            foreach (Control control in (groupBox.Tag as GroupBoxOption).RequiredControls)
            {
                if (control is TextBox)
                {
                    if (string.IsNullOrWhiteSpace((control as TextBox).GetValue()))
                    {
                        list.Add(control);
                        result = false;
                    }
                }
                else if (control is ComboBox)
                {
                    if (string.IsNullOrWhiteSpace((control as ComboBox).GetValue()))
                    {
                        list.Add(control);
                        result = false;
                    }
                }
                else if (control is DateTimePicker)
                {
                    if (string.IsNullOrWhiteSpace((control as DateTimePicker).GetValue()))
                    {
                        list.Add(control);
                        result = false;
                    }
                }
            }

            foreach (Control control in list)
            {
                BaseForm form = control.TopLevelControl as BaseForm;
                form.errorProvider1.SetIconAlignment(control, ErrorIconAlignment.MiddleRight);
                form.errorProvider1.SetError(control, "필수 입력 항목입니다.");
            }
            return result;
        }

        public static GroupBoxOption GetOption(this GroupBox groupBox)
        {
            if (groupBox.Tag is GroupBoxOption)
            {
                return groupBox.Tag as GroupBoxOption;
            }

            return null;
        }
    }

    public class GroupBoxOption
    {
        public List<Control> RequiredControls;
        public object Data;

        public GroupBoxOption()
        {
            RequiredControls = new List<Control>();
        }

        public void AddRequiredControl(Control control)
        {
            if (!RequiredControls.Contains(control))
            {
                RequiredControls.Add(control);
            }
        }
    }
}
