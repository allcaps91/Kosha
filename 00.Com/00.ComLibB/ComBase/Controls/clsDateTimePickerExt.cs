using System;
using System.Windows.Forms;

namespace ComBase.Controls
{
    /// <summary>
    /// <history>[2019.09.18]dhkim option TextAlign 추가</history>
    /// <history>[2019.10.24]SetOPtion datafiled 빠지는 현상 수정</history>
    /// </summary>
    public static class clsDateTimePickerExt
    {
        public static void SetValue(this DateTimePicker dateTimePicker, object data)
        {
            if (dateTimePicker.Name == "DtpDECLAREDAY")
            {
                string x = "";
            }
            if (data is DateTime)
            {
                if(DateTime.MinValue.CompareTo(data) == 0)
                {
                    dateTimePicker.Checked = false;
                    dateTimePicker.CustomFormat = " ";
                    return;
                }

                dateTimePicker.Checked = true;
                if (dateTimePicker.CustomFormat == " ")
                {
                    if (dateTimePicker.Tag is DateTimePickerOption)
                    {
                        DateTimePickerOption option = dateTimePicker.Tag as DateTimePickerOption;

                        DateTimeType dateTimeType = option.DataBaseFormat;

                        dateTimePicker.CustomFormat = option.DisplayFormat.GetEnumDescription();

                    }
                }

                dateTimePicker.Value = (DateTime)data;
            }
            else
            {
                if (data.IsNullOrEmpty())
                {
                    if(dateTimePicker.ShowCheckBox)
                    {
                        dateTimePicker.Checked = false;
                    }
                    dateTimePicker.CustomFormat = " ";
                    return;
                }

                if(dateTimePicker.ShowCheckBox)
                {
                    dateTimePicker.Checked = true;
                }

                if (dateTimePicker.Tag is DateTimePickerOption)
                {
                    DateTimePickerOption option = dateTimePicker.Tag as DateTimePickerOption;

                    DateTimeType dateTimeType = option.DataBaseFormat;

                    dateTimePicker.CustomFormat = option.DisplayFormat.GetEnumDescription();
                    try
                    {
                        dateTimePicker.Value = DateTime.ParseExact(data.ToString(), dateTimeType.GetEnumDescription(), null);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                    }
                  
                }
                else if(data is string)
                {
                    if (dateTimePicker.CustomFormat.Trim() == "")
                    {
                        dateTimePicker.CustomFormat = "yyyy-MM-dd";
                    }

                    dateTimePicker.Value = DateTime.ParseExact(data.ToString(), dateTimePicker.CustomFormat, null);
                    
                }
            }
        }

        public static string GetValue(this DateTimePicker dateTimePicker)
        {
        

            string format = "yyyy";
            if (dateTimePicker.Tag is IControlOption)
            {
                if (dateTimePicker.ShowCheckBox)
                {
                    if (!dateTimePicker.Checked)
                    {
                        return null;
                    }
                }
                
                DateTimeType dateTimeType = (dateTimePicker.Tag as DateTimePickerOption).DataBaseFormat;
                format = dateTimeType.GetEnumDescription();
                return dateTimePicker.Value.ToString(format);
            }
            else
            {
                if (dateTimePicker.CustomFormat.IsNullOrEmpty())
                {
                    return dateTimePicker.Value.ToString();
                }
                else
                {
                    return dateTimePicker.Value.ToString(dateTimePicker.CustomFormat);
                }
                
            }
          
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTimePicker"></param>
        /// <returns></returns>
        public static bool IsValueTypeString(this DateTimePicker dateTimePicker)
        {
            if (dateTimePicker.Tag is IControlOption)
            {
                DateTimeType dateTimeType = (dateTimePicker.Tag as DateTimePickerOption).DataBaseFormat;
                return dateTimeType != DateTimeType.None;

            }
            return true;
        }


        public static void SetOptions(this DateTimePicker dateTimePicker, DateTimePickerOption option)
        {
            string dataField = string.Empty;
            if (dateTimePicker.Tag is DateTimePickerOption)
            {
                dataField = (dateTimePicker.Tag as DateTimePickerOption).DataField;
            }
            else if(dateTimePicker.Tag is string)
            {
                dataField = dateTimePicker.Tag.ToString();
            }
            if (dataField.IsNullOrEmpty())
            {
                dataField = option.DataField;

            }

            //  표시포멧 설정
            dateTimePicker.Format = DateTimePickerFormat.Custom;
            dateTimePicker.CustomFormat = option.DisplayFormat.GetEnumDescription();
            option.DataField = dataField;

     //       option.DataField = dataField;
            dateTimePicker.Tag = option;

            dateTimePicker.ValueChanged += (s, e) =>
            {
                if (dateTimePicker.Name == "DtpDECLAREDAY")
                {
                    string x = "";
                }
                if (dateTimePicker.Tag == null || !(dateTimePicker.Tag is DateTimePickerOption))
                {
                    return;
                }

                //초기화
                if (dateTimePicker.ShowCheckBox)
                {
                    if (!dateTimePicker.Checked)
                    {
                        dateTimePicker.CustomFormat = " ";
                        return;
                    }
                }
               
               

                option = dateTimePicker.Tag as DateTimePickerOption;
                if (dateTimePicker.Checked)
                {
                    dateTimePicker.Format = DateTimePickerFormat.Custom;
                    dateTimePicker.CustomFormat = option.DisplayFormat.GetEnumDescription();
                }
            };
        }

        /// <summary>
        /// DateTimePicker 초기화 Clear
        /// </summary>
        /// <param name="dateTimePicker"></param>
        public static void Initialize(this DateTimePicker dateTimePicker)
        {
            if(dateTimePicker.Name == "DtpDECLAREDAY")
            {
                string x = "";
            }
            if (dateTimePicker.DataBindings != null)
            {
                dateTimePicker.DataBindings.Clear();
            }

            if (dateTimePicker.CustomFormat == " ")
            {
                dateTimePicker.Checked = false;
                dateTimePicker.CustomFormat = " ";
            }
            else
            {
                if (dateTimePicker.Value.IsNullOrEmpty())
                {
                    dateTimePicker.Checked = false;
                    dateTimePicker.CustomFormat = " ";

                }
                else
                {
                    dateTimePicker.Value = DateTime.Now;
                }
            }
      
            
        }

        public static string GetDataField(this DateTimePicker dateTimePicker)
        {
            if (dateTimePicker.Tag is DateTimePickerOption)
            {
                return (dateTimePicker.Tag as DateTimePickerOption).DataField;
            }

            return string.Empty;
        }

        public static DateTimePickerOption GetOption(this DateTimePicker dateTimePicker)
        {
            if (dateTimePicker.Tag is DateTimePickerOption)
            {
                return dateTimePicker.Tag as DateTimePickerOption;
            }

            return null;
        }
    }

    public class DateTimePickerOption : IControlOption
    {
        public DateTimeType DisplayFormat = DateTimeType.YYYY_MM_DD;
        public DateTimeType DataBaseFormat = DateTimeType.YYYY_MM_DD;
        public string DataField { get; set; }
    }
}
