using ComBase.Mvc;
using ComBase.Mvc.Exceptions;
using ComBase.Mvc.Utils;
using ComBase.Mvc.Validation;
using FarPoint.Win.Spread;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

namespace ComBase.Controls
{

    public static class clsPanelExt
    {
        /// <summary>
        /// 패널 초기화
        /// </summary>
        /// <param name="p"></param>
        /// <history>[2019.07.09 김민철]컨트롤 종류 추가 NumericUpDown</history>
        /// <history>[2019.09.19 김동훈]DtpDatePicker 초기화시 빈문자열로 초기화 되도록 변경, 
        /// 값을 준상태로 초기화할경우는 Dto 또는 Model의 생성자에서 기본값을 세팅하여 Initialize 대신 panel.setData(dto)를 사용하도록 한다.
        /// </history>
        public static void Initialize(this Panel panel)
        {
            if (panel.Tag is PanelOption)
            {
                (panel.Tag as PanelOption).Data = null;
            }

            List<Control> list = new List<Control>();
            FindControls(ref list, panel);

            foreach (Control control in list)
            {
                BaseForm form = control.TopLevelControl as BaseForm;
                if (form != null)
                {
                    form.errorProvider1.SetIconAlignment(control, ErrorIconAlignment.MiddleRight);
                    form.errorProvider1.SetError(control, "");
                }
                if (control is TextBox)
                {
                    (control as TextBox).Initialize();
                }
                else if (control is MaskedTextBox)
                {
                    (control as MaskedTextBox).Initialize();
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
                else if (control is NumericUpDown)
                {
                    (control as NumericUpDown).Initialize();
                }
                else if (control is Label)
                {
                    if ((control as Label).AutoSize == false)
                    {
                        (control as Label).TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    }
                }
                else if (control is CodeSearchText)
                {
                    (control as CodeSearchText).Initialize();
                }
            }
        }

        /// <summary>
        /// 패널 안의 컨트롤 엔터키 바인딩
        /// </summary>
        /// <param name="panel"></param>
        public static void SetEnterKey(this Panel panel)
        {
            List<Control> list = new List<Control>();
            FindControls(ref list, panel);

            foreach (Control control in list)
            {
                if (control is TextBox)
                {
                    if (!(control as TextBox).Multiline)
                    {
                        (control as TextBox).KeyDown -= PanelExt_KeyDown;
                        (control as TextBox).KeyDown += PanelExt_KeyDown;
                    }
                }
                else if (control is ComboBox)
                {
                    (control as ComboBox).KeyDown -= PanelExt_KeyDown;
                    (control as ComboBox).KeyDown += PanelExt_KeyDown;
                }
                else if (control is DateTimePicker)
                {
                    (control as DateTimePicker).KeyDown -= PanelExt_KeyDown;
                    (control as DateTimePicker).KeyDown += PanelExt_KeyDown;
                }
                else if (control is MaskedTextBox)
                {
                    (control as MaskedTextBox).KeyDown -= PanelExt_KeyDown;
                    (control as MaskedTextBox).KeyDown += PanelExt_KeyDown;
                }
            }
        }

        public static void SetChangeValueClearSpd(this Panel panel, FpSpread fpSpread)
        {
            List<Control> list = new List<Control>();
            FindControls(ref list, panel);

            foreach (Control control in list)
            {
                if (control is RadioButton)
                {
                    (control as RadioButton).CheckedChanged += (s, e) =>
                    {
                        fpSpread.RowsClear();
                    };
                }
                if (control is CheckBox)
                {
                    (control as CheckBox).CheckedChanged += (s, e) =>
                    {
                        fpSpread.RowsClear();
                    };
                }
                else if (control is ComboBox)
                {
                    (control as ComboBox).SelectedValueChanged += (s, e) =>
                    {
                        fpSpread.RowsClear();
                    };
                }
                else if (control is DateTimePicker)
                {
                    (control as DateTimePicker).ValueChanged += (s, e) =>
                    {
                        fpSpread.RowsClear();
                    };
                }
            }
        }

        public static SearchParameter GetParameters(this Panel panel)
        {
            List<Control> list = new List<Control>();
            FindControls(ref list, panel);

            SearchParameter parameter = new SearchParameter();
            foreach (Control control in list)
            {
                if (control is ComboBox)
                {
                    parameter.Add((control.Tag as ComboBoxOption).DataField, GetControlValue(control));
                }
                else
                {
                    if (!control.Tag.IsNullOrEmpty())
                    {
                        parameter.Add(control.Tag.ToString(), GetControlValue(control));
                    }
                }
            }

            return parameter;
        }

        public static void SetEnterExecuteButton(this Panel panel, Button btn)
        {
            List<Control> list = new List<Control>();
            FindControls(ref list, panel);

            foreach (Control control in list)
            {
                if (control is TextBox || control is DateTimePicker ||
                    control is ComboBox)
                {
                    control.KeyDown += (s, e) =>
                    {
                        if (e.KeyCode == Keys.Enter)
                        {
                            btn.PerformClick();
                        }
                    };
                }
                else if (control is CodeSearchText)
                {
                    (control as CodeSearchText).ExecuteButton = btn;
                }
                else if (control is MedieineCodeSearchText)
                {
                    (control as MedieineCodeSearchText).ExecuteButton = btn;
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
                if (item is NumericUpDown)
                {
                    list.Add(item);
                    continue;
                }

                if (item is CodeSearchText)
                {
                    list.Add(item);
                }
                else if (item is MedieineCodeSearchText)
                {
                    list.Add(item);
                }
                else if (item.HasChildren)
                {
                    FindControls(ref list, item);
                }
                else
                {
                    list.Add(item);
                }
            }
        }

        private static void PanelExt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        /// <summary>
        /// 데이터 바인딩
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static void SetData(this Panel panel, object data)
        {
            if (panel.Tag is PanelOption)
            {
                (panel.Tag as PanelOption).Data = data;
            }
            else
            {
                panel.Tag = new PanelOption { Data = data };
            }

            List<Control> list = new List<Control>();
            FindControls(ref list, panel);
            SetControlDataBind(list, data);
        }

        public static void SetDataBinding(this Panel panel, object data)
        {
            if (panel.Tag is PanelOption)
            {
                (panel.Tag as PanelOption).Data = data;
            }
            else
            {
                panel.Tag = new PanelOption { Data = data };
            }

            List<Control> list = new List<Control>();
            FindControls(ref list, panel);
            SetControlDataBinding(list, data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="panel"></param>
        /// <returns></returns>
        public static T GetData<T>(this Panel panel) where T : new()
        {
            T t = new T();

            if (panel.Tag is PanelOption)
            {
                if ((panel.Tag as PanelOption).Data != null)
                {
                    t = (T)(panel.Tag as PanelOption).Data;
                }
            }

            List<Control> list = new List<Control>();
            FindControls(ref list, panel);

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
                string dataField = string.Empty;

                if (item is CodeSearchText)
                {
                    dataField = (item as CodeSearchText).ValueDataField;
                    if (dataField.NotEmpty())
                    {
                        PropertyInfo pi = t.GetType().GetProperty(dataField);
                        if (pi == null)
                        {
                            continue;
                        }

                        pi.SetValue(t, (item as CodeSearchText).GetValue());
                    }

                    dataField = (item as CodeSearchText).DisplayDataField;
                    if (dataField.NotEmpty())
                    {
                        PropertyInfo pi = t.GetType().GetProperty(dataField);
                        if (pi == null)
                        {
                            continue;
                        }

                        pi.SetValue(t, (item as CodeSearchText).GetDisplay());
                    }

                    continue;
                }
                else if (item is MedieineCodeSearchText)
                {
                    dataField = (item as MedieineCodeSearchText).ValueDataField;
                    if (dataField.NotEmpty())
                    {
                        PropertyInfo pi = t.GetType().GetProperty(dataField);
                        if (pi == null)
                        {
                            continue;
                        }

                        pi.SetValue(t, (item as MedieineCodeSearchText).GetValue());
                    }

                    dataField = (item as MedieineCodeSearchText).DisplayDataField;
                    if (dataField.NotEmpty())
                    {
                        PropertyInfo pi = t.GetType().GetProperty(dataField);
                        if (pi == null)
                        {
                            continue;
                        }

                        pi.SetValue(t, (item as MedieineCodeSearchText).GetDisplay());
                    }

                    continue;
                }
                else if (item.Tag == null || string.IsNullOrWhiteSpace(item.Tag.ToString()) || !item.Enabled)
                {
                    continue;
                }

                if (item.Tag is IControlOption)
                {
                    dataField = (item.Tag as IControlOption).DataField;
                }
                else
                {
                    dataField = item.Tag.ToString();
                }
                if (dataField == "SENDMAILDATE")
                {
                    string x = "";
                }
                if (dataField == "ESTIMATEDATE")
                {
                    string x = "";
                }
                if (dataField == "STARTDATE")
                {
                    string x = "";
                }
                if (dataField == "PRINTDATE")
                {
                    string x = "";
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
                    PropertyInfo pi = t.GetType().GetProperty(dataField);
                    if (pi == null)
                    {
                        continue;
                    }

                    //  값
                    object value = GetControlValue(item);
                    Type type = pi.PropertyType;


                    //  NumericUpDown 오류인한 추가
                    if (pi.PropertyType == typeof(Int16) ||
                        pi.PropertyType == typeof(Int32) ||
                        pi.PropertyType == typeof(Int64) ||
                        pi.PropertyType == typeof(decimal) ||
                        pi.PropertyType == typeof(double) ||
                        pi.PropertyType == typeof(long)
                       )
                    {
                        if (value == null || value.IsNullOrEmpty())
                        {
                            value = 0;
                        }
                        value = Convert.ChangeType(value.ToString().Replace(",", ""), type);
                    }
                    else if (pi.PropertyType == typeof(DateTime?) && value.IsNullOrEmpty())
                    {
                        value = null;
                    }
                    else if (pi.PropertyType == typeof(DateTime?) && value is string)
                    {
                        value = DateTime.Parse(value.ToString());
                    }
                    else if (pi.PropertyType == typeof(string) && value is DateTime)
                    {
                        throw new MTSException(pi.Name + " DateTimePickerOption의 Databaseformat이 설정 되지 않았습니다");
                    }
                    else if (pi.PropertyType.IsEnum && value is string)
                    {
                        value = Enum.Parse(pi.PropertyType, value.ToString());
                    }
                    else if (pi.PropertyType == typeof(string) && value is decimal)
                    {
                        value = value.ToString();
                    }
                    else if (pi.PropertyType == typeof(bool))
                    {
                        bool isCheck = false;
                        if (!bool.TryParse(value.ToString(), out isCheck))
                        {
                            continue;
                        }
                    }
                    try
                    {
                        pi.SetValue(t, value);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                        MessageBox.Show(ex.Message);
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
                string value = (item as TextBox).GetValue();
                if (value.IsNullOrEmpty())
                {
                    return null;
                }
                else
                {
                    TextBox textBox = item as TextBox;
                    if (textBox.Tag is TextBoxOption)
                    {
                        TextBoxOption option = textBox.Tag as TextBoxOption;
                        if (option.DisplayFormat != DateTimeType.None)
                        {
                            return DateUtil.stringToDateTime(value, option.DisplayFormat);
                        }
                        else
                        {
                            return value;
                        }
                    }
                    else
                    {
                        return value;
                    }
                }
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
                DateTimePicker dateTimePicker = item as DateTimePicker;
                if (dateTimePicker.Tag is DateTimePickerOption)
                {
                    if (dateTimePicker.IsValueTypeString())
                    {
                        return dateTimePicker.GetValue();
                    }
                    else
                    {
                        if (dateTimePicker.Checked == false)
                        {
                            return null;
                        }

                        return dateTimePicker.Value;
                    }
                }
                //return (item as DateTimePicker).GetValue();
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
            else if (item is NumericUpDown)
            {
                return (item as NumericUpDown).GetValue();
            }
            else if (item is MaskedTextBox)
            {
                return (item as MaskedTextBox).GetValue();
            }

            return null;
        }

        /// <summary>
        /// 데이터 바인딩
        /// </summary>
        /// <param name="list"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        private static void SetControlDataBind(List<Control> list, object dto)
        {
            PropertyInfo[] properties = dto.GetType().GetProperties();

            Dictionary<string, PropertyInfo> dictionary = new Dictionary<string, PropertyInfo>();
            foreach (PropertyInfo propertyInfo in properties)
            {
                dictionary.Add(propertyInfo.Name, propertyInfo);
            }

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

                    if (item is CodeSearchText)
                    {
                        if (dataField.Equals((item as CodeSearchText).ValueDataField))
                        {
                            object obj = dto.GetPropertieValue(dataField);
                            (item as CodeSearchText).TxtCode.DataBindings.Clear();
                            (item as CodeSearchText).TxtCode.DataBindings.Add("Text", dto, dataField, true, DataSourceUpdateMode.OnPropertyChanged);

                            if ((item as CodeSearchText).DisplayDataField.NotEmpty())
                            {
                                (item as CodeSearchText).TxtName.DataBindings.Clear();
                                (item as CodeSearchText).TxtName.DataBindings.Add("Text", dto, (item as CodeSearchText).DisplayDataField, true, DataSourceUpdateMode.OnPropertyChanged);
                            }
                        }
                        //if ((item as CodeSearchText).ValueDataField.Equals(info.Name))
                        //{
                        //    object obj = dto.GetPropertieValue(dataField);
                        //    (item as CodeSearchText).SetValueData(obj);
                        //    break;
                        //}
                        //if ((item as CodeSearchText).DisplayDataField.Equals(info.Name))
                        //{
                        //    object obj = dto.GetPropertieValue(dataField);
                        //    (item as CodeSearchText).SetDisplayData(obj);
                        //    break;
                        //}
                    }
                    else if (item is MedieineCodeSearchText)
                    {
                        if ((item as MedieineCodeSearchText).ValueDataField.Equals(info.Name))
                        {
                            object obj = dto.GetPropertieValue(dataField);
                            (item as MedieineCodeSearchText).SetValueData(obj);
                            break;
                        }
                        if ((item as MedieineCodeSearchText).DisplayDataField.Equals(info.Name))
                        {
                            object obj = dto.GetPropertieValue(dataField);
                            (item as MedieineCodeSearchText).SetDisplayData(obj);
                            break;
                        }
                    }
                    else if (info.Name.Equals(dataField))
                    {
                        object obj = dto.GetPropertieValue(dataField);
                        SetControlValue(item, obj);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 데이터 바인딩
        /// </summary>
        /// <param name="list"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        private static void SetControlDataBinding(List<Control> list, object dto)
        {
            PropertyInfo[] properties = dto.GetType().GetProperties();
            foreach (Control item in list)
            {
                if ((item is MedieineCodeSearchText))
                {
                }

                if ((!(item is CodeSearchText) && !(item is MedieineCodeSearchText)) && (item.Tag == null || string.IsNullOrWhiteSpace(item.Tag.ToString())))
                {
                    continue;
                }

                string dataField = string.Empty;
                if (item.Tag is IControlOption)
                {
                    dataField = (item.Tag as IControlOption).DataField;
                }
                else if (item is CodeSearchText)
                {
                    dataField = (item as CodeSearchText).ValueDataField;
                }
                else if (item is MedieineCodeSearchText)
                {
                    dataField = (item as MedieineCodeSearchText).ValueDataField;
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
                                        item.DataBindings.Add("Text", obj, dataField, true, DataSourceUpdateMode.OnPropertyChanged);
                                    }
                                }
                            }
                        }
                        continue;
                    }

                    if (item is CodeSearchText)
                    {
                        if (dataField.Equals((item as CodeSearchText).ValueDataField))
                        {
                            object obj = dto.GetPropertieValue(dataField);
                            (item as CodeSearchText).TxtCode.DataBindings.Clear();
                            (item as CodeSearchText).TxtCode.DataBindings.Add("Text", dto, dataField, true, DataSourceUpdateMode.OnPropertyChanged);

                            if ((item as CodeSearchText).DisplayDataField.NotEmpty())
                            {
                                (item as CodeSearchText).TxtName.DataBindings.Clear();
                                (item as CodeSearchText).TxtName.DataBindings.Add("Text", dto, (item as CodeSearchText).DisplayDataField, true, DataSourceUpdateMode.OnPropertyChanged);
                            }
                        }
                    }
                    else if (item is MedieineCodeSearchText)
                    {
                        if (dataField.Equals((item as MedieineCodeSearchText).ValueDataField))
                        {
                            object obj = dto.GetPropertieValue(dataField);
                            (item as MedieineCodeSearchText).TxtCode.DataBindings.Clear();
                            (item as MedieineCodeSearchText).TxtCode.DataBindings.Add("Text", dto, dataField, true, DataSourceUpdateMode.OnPropertyChanged);

                            if ((item as MedieineCodeSearchText).DisplayDataField.NotEmpty())
                            {
                                (item as MedieineCodeSearchText).TxtName.DataBindings.Clear();
                                (item as MedieineCodeSearchText).TxtName.DataBindings.Add("Text", dto, (item as MedieineCodeSearchText).DisplayDataField, true, DataSourceUpdateMode.OnPropertyChanged);
                            }
                        }
                    }
                    else if (info.Name.Equals(dataField))
                    {
                        object obj = dto.GetPropertieValue(dataField);
                        item.DataBindings.Clear();

                        item.DataBindings.CollectionChanged += (s, e) =>
                        {
                            (s as ControlBindingsCollection).Control.Refresh();
                        };
                        if (item is DateTimePicker)
                        {
                            (item as DateTimePicker).SetValue(null);
                            Binding DateTimePickerBinding = new Binding("Value", dto, dataField, true, DataSourceUpdateMode.OnPropertyChanged);
                            if (dto.GetPropertieValue(dataField).NotEmpty())
                            {
                                (item as DateTimePicker).SetValue(dto.GetPropertieValue(dataField));
                            }

                            (item as DateTimePicker).MouseUp += (s, e) =>
                            {
                                if (!(s as DateTimePicker).Checked)
                                {
                                    (s as DateTimePicker).SetValue(null);
                                }
                            };

                            item.DataBindings.Add(DateTimePickerBinding);

                            DateTimePickerBinding.Format += DateTimePickerBinding_Format;
                            DateTimePickerBinding.Parse += DateTimePickerBinding_Parse;
                        }
                        else if (item is ComboBox)
                        {
                            Binding ComboBoxBinding = new Binding("SelectedValue", dto, dataField, true, DataSourceUpdateMode.OnPropertyChanged);
                            //ComboBoxBinding.Format += (s, e) =>
                            //{

                            //};
                            //ComboBoxBinding.Parse += (s, e) =>
                            //{

                            //};
                            item.DataBindings.Add(ComboBoxBinding);
                        }
                        else if (item is CheckBox)
                        {
                            Binding CheckBoxBinding = new Binding("Checked", dto, dataField, true, DataSourceUpdateMode.OnPropertyChanged);

                            CheckBoxBinding.Format += CheckBoxBinding_Format;
                            CheckBoxBinding.Parse += CheckBoxBinding_Parse;
                            item.DataBindings.Add(CheckBoxBinding);
                        }
                        else if (item is RadioButton)
                        {
                            //if (RadioBinding.Control != null)
                            {
                                RadioButton rdo = (item as RadioButton);
                                if (rdo.GetOption() != null)
                                {
                                    rdo.Checked = rdo.GetOption().CheckValue.Equals(dto.GetPropertieValue(dataField));
                                }
                            }

                            Binding RadioBinding = new Binding("Checked", dto, dataField, true, DataSourceUpdateMode.OnPropertyChanged);
                            item.DataBindings.Add(RadioBinding);
                            RadioBinding.Format += (s, e) =>
                            {
                                Binding bind = s as Binding;
                                if (bind.Control != null)
                                {
                                    string controlValue = (bind.Control as RadioButton).GetOption().CheckValue;
                                    //Console.WriteLine($"ForMat : { controlValue } == {e.Value}");
                                    e.Value = controlValue.Equals(e.Value);
                                }
                            };

                            RadioBinding.Parse += (s, e) =>
                            {
                                Binding bind = s as Binding;

                                if (bind.Control != null)
                                {
                                    if ((bind.Control as RadioButton).Checked)
                                    {
                                        e.Value = (bind.Control as RadioButton).GetOption().CheckValue;
                                    }
                                    else
                                    {
                                        e.Value = string.Empty;
                                    }

                                    //Console.WriteLine($"Parse : {e.Value}       {(bind.Control as RadioButton).Name}");
                                }
                            };

                            (RadioBinding.Control as RadioButton).Click += (s, e) =>
                            {
                                (s as RadioButton).Checked = true;
                            };
                        }
                        else if (item is NumericUpDown)
                        {
                            item.DataBindings.Add("Value", dto, dataField, true, DataSourceUpdateMode.OnPropertyChanged);
                        }
                        else
                        {
                            if (dto.GetPropertieValue(dataField) != null)
                            {
                                //string value = dto.GetPropertieValue(dataField).ToString();
                                dto.SetPropertieValue(dataField, dto.GetPropertieValue(dataField));
                            }
                            item.DataBindings.Add("Text", dto, dataField, true, DataSourceUpdateMode.OnPropertyChanged);
                        }

                        break;
                    }
                }
            }
        }

        private static void CheckBoxBinding_Parse(object sender, ConvertEventArgs e)
        {
            Binding binding = sender as Binding;
            if (binding != null)
            {
                CheckBox checkBox = (binding.Control as CheckBox);
                e.Value = checkBox.Checked ? checkBox.GetOption().CheckValue : checkBox.GetOption().UnCheckValue;
            }
        }

        private static void CheckBoxBinding_Format(object sender, ConvertEventArgs e)
        {
            Binding binding = sender as Binding;
            if (binding != null)
            {
                CheckBox checkBox = (binding.Control as CheckBox);
                e.Value = checkBox.GetOption().CheckValue.Equals(e.Value);
            }
        }


        private static void DateTimePickerBinding_Parse(object sender, ConvertEventArgs e)
        {
            Binding binding = sender as Binding;
            try
            {
                if (binding != null)
                {
                    DateTimePicker dtp = (binding.Control as DateTimePicker);

                    if (dtp.Name == "DtpJOBSTIME")
                    {

                    }

                    if (dtp != null)
                    {
                        if (dtp.Checked == false)
                        {
                            dtp.ShowCheckBox = true;
                            dtp.Checked = false;
                            dtp.SetValue(null);

                        }
                        else
                        {

                            DateTime val = Convert.ToDateTime(e.Value);
                            if (dtp.GetOption() != null)
                            {
                                e.Value = val.ToString(dtp.GetOption().DataBaseFormat.GetEnumDescription());
                            }
                            else
                            {
                                e.Value = val;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }
        private static void DateTimePickerBinding_Format(object sender, ConvertEventArgs e)
        {
            Binding binding = sender as Binding;
            if (binding != null)
            {
                try
                {
                    if (binding.Control != null)
                    {
                        DateTimePicker dtp = (binding.Control as DateTimePicker);

                        if (dtp.Name == "DtpJOBSTIME")
                        {

                        }

                        if (dtp.ShowCheckBox)
                        {
                            if (dtp.Checked == false)
                            {
                                dtp.ShowCheckBox = true;
                                dtp.Checked = false;
                                dtp.SetValue(null);
                            }
                        }
                        else
                        {
                            if (dtp.GetOption() != null && e.Value.NotEmpty())
                            {
                                DateTime dtm = DateTime.ParseExact(e.Value.ToString(), dtp.GetOption().DataBaseFormat.GetEnumDescription(), null);
                                dtp.CustomFormat = dtp.GetOption().DisplayFormat.GetEnumDescription();
                                e.Value = dtm;
                            }
                            else
                            {
                                if (dtp.ShowCheckBox)
                                {
                                    dtp.Checked = false;
                                }

                                dtp.SetValue(null);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }

            }
        }

        /// <summary>
        /// 컨트롤 값넣기
        /// </summary>
        /// <param name="item"></param>
        /// <param name="data"></param>
        private static void SetControlValue(Control item, object data)
        {
            if (item.Name == "txtWorkTime1")
            {
                string xx = "";
            }
            if (item is TextBox)
            {
                string value = string.Empty;
                if (data != null)
                {
                    value = data.ToString().Trim();
                }

                (item as TextBox).SetValue(value);
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
            else if (item is NumericUpDown)
            {
                (item as NumericUpDown).SetValue(data);
            }
            else if (item is MaskedTextBox)
            {
                (item as MaskedTextBox).SetValue(data);
            }
        }

        public static void AddRequiredControl(this Panel panel, Control control)
        {
            PanelOption option = null;
            if (panel.Tag == null)
            {
                option = new PanelOption();
                panel.Tag = option;
            }
            else
            {
                option = panel.Tag as PanelOption;
            }

            control.TextChanged += (s, e) =>
            {
                BaseForm form = (s as Control).TopLevelControl as BaseForm;

                if (form != null)
                {
                    form.errorProvider1.SetError(control, "");
                }
            };
            option.AddRequiredControl(control);
        }

        /// <summary>
        /// 필수입력
        /// </summary>
        /// <param name="panel"></param>
        /// <returns></returns>
        public static bool RequiredValidate(this Panel panel)
        {
            if (panel.Tag == null || !(panel.Tag is PanelOption))
            {
                return true;
            }

            List<Control> list = new List<Control>();
            bool result = true;
            foreach (Control control in (panel.Tag as PanelOption).RequiredControls)
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
                if (form != null)
                {
                    form.errorProvider1.SetIconAlignment(control, ErrorIconAlignment.MiddleRight);
                    form.errorProvider1.SetError(control, "필수 입력 항목입니다.");
                }
            }
            return result;
        }

        public static PanelOption GetOption(this Panel panel)
        {
            if (panel.Tag is PanelOption)
            {
                return panel.Tag as PanelOption;
            }

            return null;
        }
        /// <summary>
        /// dto 유효성 검사하여 panel의 컨트롤에 에러롤 표시합니다(반드시 BaseForm을 상속받아야 합니다)
        /// 
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="dto"></param>
        public static bool Validate<T>(this Panel panel) where T : new()
        {
            bool isValid = true;
            try
            {
                T t = new T();

                //BaseDto dto = (panel.Tag as PanelOption).Data as BaseDto;
                if (t.GetType().BaseType.Name == "BaseDto")
                {
                    BaseDto dto = GetData<T>(panel) as BaseDto;
                    dto.Validate();
                }
                else
                {
                    throw new Exception("BaseDto를 상속해야 합니다");
                }
            }
            catch (MTSValidationException ex)
            {
                isValid = false;
                panel.SetValidationResult(ex.ValidationResult);
            }

            return isValid;

        }
        /// <summary>
        /// 유효성 검사결과를 에러표시(반드시 BaseForm을 상속받아야 합니다)
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="validationResultList"></param>
        public static void SetValidationResult(this Panel panel, List<MTSValidationResult> validationResultList)
        {
            bool isError = false;
            List<Control> list = new List<Control>();
            FindControls(ref list, panel);

            BaseForm form = panel.TopLevelControl as BaseForm;
            if (form == null)
            {
                foreach (Control item in panel.TopLevelControl.Controls)
                {
                    if (item.GetType().FullName == "System.Windows.Forms.MdiClient")
                    {
                        foreach (Control item2 in item.Controls)
                        {
                            //if(item2 is BaseForm)
                            //{
                            //    Log.Debug("베이스 대시보드이다");
                            //}
                            BaseForm tmp = item2 as BaseForm;
                            if (tmp != null)
                            {
                                form = tmp;
                            }

                        }
                    }
                }
            }

            if (form == null)
            {
                throw new MTSException("BaseForm을 상속받아야 합니다");
            }
            foreach (Control control in list)
            {
                control.TextChanged -= Control_TextChanged;
            }
            foreach (Control control in list)
            {
                control.TextChanged += Control_TextChanged;
                form.errorProvider1.SetError(control, string.Empty);
                foreach (MTSValidationResult result in validationResultList)
                {
                    if (control.Tag != null)
                    {
                        string name = control.Name;
                        if (result.PropertyName == "FEETYPE")
                        {
                            string xxx = string.Empty;
                        }
                        if (name == "TxtFEETYPE")
                        {
                            string xxx = string.Empty;
                        }
                        string dataField = string.Empty;
                        //IControlOption xx = (control.Tag as IControlOption);
                        if (control.Tag is IControlOption)
                        {
                            dataField = (control.Tag as IControlOption).DataField;
                        }
                        else
                        {
                            dataField = control.Tag.ToString();

                        }

                        if (dataField != null)
                        {
                            if (dataField.Equals(result.PropertyName))
                            {

                                isError = true;
                                form.errorProvider1.SetIconAlignment(control, ErrorIconAlignment.MiddleRight);
                                form.errorProvider1.SetError(control, result.Message);

                            }
                        }
                    }
                }
            }
            if (!isError && validationResultList.Count > 0)
            {
                foreach (MTSValidationResult result in validationResultList)
                {
                    MessageUtil.Error(result.PropertyName + " " + result.Message);

                }
            }
        }

        private static void Control_TextChanged(object sender, EventArgs e)
        {
            Control control = sender as Control;
            BaseForm form = control.TopLevelControl as BaseForm;
            if (form != null)
            {
                form.errorProvider1.SetError(control, "");
            }

        }
    }

    public class PanelOption
    {
        public List<Control> RequiredControls;
        public object Data;

        public PanelOption()
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
