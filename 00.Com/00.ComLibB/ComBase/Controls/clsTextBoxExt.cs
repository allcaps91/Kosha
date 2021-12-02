using ComBase.Mvc.Exceptions;
using ComBase.Mvc.Utils;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ComBase.Controls
{
    /// <summary>
    /// <history>2019.09.20 김동훈 SetValue 날짜형식추가</history>
    /// </summary>
    public static class clsTextBoxExt
    {
        public static void SetValue(this TextBox textBox, object data)
        {
            if (data.IsNullOrEmpty())
            {
                textBox.Text = string.Empty;
            }
            else
            {
                if (textBox.Tag is TextBoxOption)
                {
                    TextBoxOption option = textBox.Tag as TextBoxOption;
                    if(data is System.DateTime && option.DisplayFormat != DateTimeType.None)
                    {

                        textBox.Text = DateUtil.DateTimeToStrig(DateTime.Parse(data.ToString()), option.DisplayFormat);
                    }
                    else if (data is System.DateTime && option.DisplayFormat == DateTimeType.None)
                    {
                         throw new MTSException(" 값이 날짜형입니다. DisplayFormat을 설정하세요");
                    }
                    else
                    {
                        textBox.Text = data.ToString();
                    }
                    
                }
                else
                {
                    if(textBox.Tag is TextBoxOption)
                    {
                        TextBoxOption option = textBox.Tag as TextBoxOption;
                        if (option.TextFormat == TextType.Number)
                        {
                            textBox.TextAlign = HorizontalAlignment.Right;
                            textBox.Text = string.Format("{0:#,##0;(#,##0);}", data.To<long>());
                        }
                    }
                    else
                    {
                        textBox.Text = data.ToString();
                    }
                }
            }
        }

        public static string GetValue(this TextBox textBox)
        {
            return textBox.Text;
        }

        public static void Initialize(this TextBox textBox)
        {
            if(textBox.DataBindings != null)
            {
                textBox.DataBindings.Clear();
            }
            textBox.Text = string.Empty;
        }

        /// <summary>
        /// 입력 텍스트가 숫자인지 확인하는 확장 메서드입니다.
        /// Ex)
        /// private void txtRollNo_KeyPress(object sender, KeyPressEventArgs e)
        /// {
        ///     (sender as TextBox).ValidateNumber(e, false);
        /// }
        /// </ summary>
        /// <param name = "e"> 키 이벤트 초기화를 누릅니다. </ param>
        /// <param name = "IsCalculation"> 참이면 소수점 (.)을 허용하고, 거짓이면 소수점 (.)을 사용할 수 없습니다. </ param>
        public static void ValidateNumber(this TextBox txt, KeyPressEventArgs e, bool IsCalculation)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                if (e.KeyChar == '.' && IsCalculation)
                {
                    if (txt.Text.IndexOf('.') > -1)
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        e.Handled = false;
                    }
                }
                else
                {
                    e.Handled = true;
                }
            }
        }

        public static void SetAutoComplete<T>(this TextBox textBox, List<T> list, string columnName, AutoCompleteMode  autoCompleteMode = AutoCompleteMode.SuggestAppend)
        {
            AutoCompleteStringCollection autoCompleteStringCollection = new AutoCompleteStringCollection();

            List<string> items = new List<string>();
            foreach (object item in list)
            {
                if (item.GetPropertieValue(columnName) != null)
                {
                    items.Add(item.GetPropertieValue(columnName).ToString());
                }
            }

            autoCompleteStringCollection.AddRange(items.ToArray());

            textBox.AutoCompleteMode = autoCompleteMode;
            textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBox.AutoCompleteCustomSource = autoCompleteStringCollection;
        }

        public static void SetOptions(this TextBox textBox, TextBoxOption option)
        {
            string dataField = string.Empty;
            if (textBox.Tag != null)
            {
                if (textBox.Tag is string)
                {
                    dataField = textBox.Tag.ToString();
                   
                }
            }

            if (!string.IsNullOrWhiteSpace(dataField))
            {
                option.DataField = dataField;
            }

            textBox.Tag = option;
            //textBox.ReadOnly = option.ReadOnly;

            if(option.TextFormat == TextType.Number)
            {
                textBox.TextAlign = HorizontalAlignment.Right;

                textBox.TextChanged += (s, e) =>
                {
                    string result = string.Empty;
                    char[] validChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-' };
                    foreach (char c in (s as TextBox).Text) // check each character in the user's input
                    {
                        if (Array.IndexOf(validChars, c) != -1)
                            result += c; // if this is ok, then add it to the result
                    }

                    if(result.Empty() || result.Equals("-"))
                    {
                        (s as TextBox).Text = result;
                        (s as TextBox).SelectionStart = (s as TextBox).Text.Length;
                    }
                    else
                    {
                        (s as TextBox).Text = string.Format("{0:#,##0;-#,##0;}", result.To<long>());
                        (s as TextBox).SelectionStart = (s as TextBox).Text.Length;
                    }
                };
            }
            else if(option.TextFormat == TextType.Decimal)
            {
                textBox.TextAlign = HorizontalAlignment.Right;
                textBox.KeyPress += (s, e) =>
                {
                    if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
                    {
                        e.Handled = true;
                    }

                    // only allow one decimal point
                    if ((e.KeyChar == '.') && ((s as TextBox).Text.IndexOf('.') > -1))
                    {
                        e.Handled = true;
                    }
                };
                //textBox.TextChanged += TextBox_TextChanged;
                //textBox.KeyPress += TextBox_KeyPress;
            }
        }

        private static void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            //////TextBox txtRecieve = sender as TextBox;
            //////var text = txtRecieve.Text.Trim();
            //////if (Regex.IsMatch(text, @"^\d{1,2}(\.\d{1,2})?$"))
            //////{
            //////    // Do something here
            //////}
            //////else
            ////// {
            //////    MessageBox.Show("Doesn't match pattern");
            //////}

            //////if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != ',')
            //////{
            //////    e.Handled = true;
            //////}

            ////////check if '.' , ',' pressed
            //////char sepratorChar = 's';
            //////if (e.KeyChar == '.' || e.KeyChar == ',')
            //////{
            //////    // check if it's in the beginning of text not accept
            //////    if (txtRecieve.Text.Length == 0) e.Handled = true;
            //////    // check if it's in the beginning of text not accept
            //////    if (txtRecieve.SelectionStart == 0) e.Handled = true;
            //////    // check if there is already exist a '.' , ','
            //////    if (alreadyExist(txtRecieve.Text, ref sepratorChar)) e.Handled = true;
            //////    //check if '.' or ',' is in middle of a number and after it is not a number greater than 99
            //////    if (txtRecieve.SelectionStart != txtRecieve.Text.Length && e.Handled == false)
            //////    {
            //////        // '.' or ',' is in the middle
            //////        string AfterDotString = txtRecieve.Text.Substring(txtRecieve.SelectionStart);

            //////        if (AfterDotString.Length > 2)
            //////        {
            //////            e.Handled = true;
            //////        }
            //////    }
            //////}
            ////////check if a number pressed

            //////if (Char.IsDigit(e.KeyChar))
            //////{
            //////    //check if a coma or dot exist
            //////    if (alreadyExist(txtRecieve.Text, ref sepratorChar))
            //////    {
            //////        int sepratorPosition = txtRecieve.Text.IndexOf(sepratorChar);
            //////        string afterSepratorString = txtRecieve.Text.Substring(sepratorPosition + 1);
            //////        if (txtRecieve.SelectionStart > sepratorPosition && afterSepratorString.Length > 1)
            //////        {
            //////            e.Handled = true;
            //////        }
            //////    }
            //////}
        }

        private static void TextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string result = string.Empty;
            char[] validChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.', '-' };
            foreach (char c in textBox.Text) // check each character in the user's input
            {
                if (Array.IndexOf(validChars, c) != -1)
                    result += c; // if this is ok, then add it to the result
            }

            //textBox.TextChanged -= TextBox_TextChanged;
            textBox.Text = string.Format("{0:0.##}", result.To<long>());
            //(s as TextBox).SelectionStart = (s as TextBox).Text.Length;
            //textBox.TextChanged += TextBox_TextChanged;
        }

        public static string GetDataField(this TextBox textBox)
        {
            if (textBox.Tag is TextBoxOption)
            {
                return (textBox.Tag as TextBoxOption).DataField;
            }

            return string.Empty;
        }

        public static TextBoxOption GetOption(this TextBox textBox)
        {
            if (textBox.Tag is TextBoxOption)
            {
                return textBox.Tag as TextBoxOption;
            }

            return null;
        }
        /// <summary>
        /// 엔터키로 실행할 버튼 지정
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="button"></param>
        public static void SetExecuteButton(this TextBox textBox, Button button)
        {
            textBox.KeyDown += (sender, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    button.PerformClick();
                }
            };
        }
    }

    public class TextBoxOption : IControlOption
    {
        public string DataField { get; set; }
        public bool ReadOnly { get; set; }
        /// <summary>
        /// 값이 DateTime일경우 보여질 날짜형식
        /// </summary>
        public DateTimeType DisplayFormat;

        public TextType TextFormat = TextType.Text;

        public string DecimalType = "n0";
    }

    public enum TextType
    {
        Text,
        Number,
        Decimal
    }
}
