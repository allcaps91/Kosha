using ComBase.Controls;
using ComBase.Mvc.Exceptions;
using ComBase.Mvc.Validation;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ComBase.Mvc
{
    public class BaseDto : INotifyPropertyChanged
    {
        public System.Action OnPropertyChangedReceived;
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if(OnPropertyChangedReceived != null)
            {
                OnPropertyChangedReceived();
            }
        }

        public RowStatus RowStatus { get; set; }

        public string ComboDisplay { get; set; }

        public string zTemp1 { get; set; }
        public string zTemp2 { get; set; }
        public string zTemp3 { get; set; }
        public string zTemp4 { get; set; }
        public string zTemp5 { get; set; }
        public string zTemp6 { get; set; }
        public string zTemp7 { get; set; }
        public string zTemp8 { get; set; }
        public string zTemp9 { get; set; }
        public string zTemp10 { get; set; }
        public string zTemp11 { get; set; }
        public string zTemp12 { get; set; }
        public string zTemp13 { get; set; }
        public string zTemp14 { get; set; }
        public string zTemp15 { get; set; }
        public string zTemp16 { get; set; }
        public string zTemp17 { get; set; }
        public string zTemp18 { get; set; }
        public string zTemp19 { get; set; }
        public string zTemp20 { get; set; }

        public BaseDto()
        {
            
        }
        public void Validate()
        {
            List<MTSValidationResult> list = new List<MTSValidationResult>();
            PropertyInfo[] propertys = this.GetType().GetProperties();
            foreach (PropertyInfo pi in propertys)
            {
                object[] attributes = pi.GetCustomAttributes(true);
                foreach (Attribute attribute in attributes)
                {
                    if (attribute is IMTSValidation)
                    {
                        IMTSValidation validation = attribute as IMTSValidation;
                        if (validation.DoValidation(pi.GetValue(this)) == false)
                        {
                            list.Add(new MTSValidationResult(validation, pi.Name, pi.GetValue(this)));
                        }
                    }
                }
            }
            if (list.Count > 0)
            {
                throw new MTSValidationException(list);
            }
        }
        /// <summary>
        /// 스프레드에서 사용하는 Validate입니다
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public void Validate(string propertyName, object value)
        {
            List<MTSValidationResult> list = new List<MTSValidationResult>();

            PropertyInfo[] propertys = this.GetType().GetProperties();
            foreach (PropertyInfo pi in propertys)
            {
                if (pi.Name.Equals(propertyName))
                {
                    object[] attributes = pi.GetCustomAttributes(true);
                    List<IMTSValidation> validations = new List<IMTSValidation>();
                    foreach (Attribute attribute in attributes)
                    {
                        if (attribute is IMTSValidation)
                        {

                            IMTSValidation validation = attribute as IMTSValidation;
                            validations.Add(validation);
                        }
                    }

                    IEnumerable<IMTSValidation> sortList = from v in validations
                                                           orderby v.Order
                                                           select v;
                    foreach (IMTSValidation validation in sortList)
                    {
                        if (validation.DoValidation(value) == false)
                        {
                            list.Add(new MTSValidationResult(validation, pi.Name, value));
                            break;
                        }
                    }
                }

            }
            if (list.Count > 0)
            {
                throw new MTSValidationException(list);
            }
        }
    }

    public enum RowStatus
    {
        None, Update, Insert, Delete
    }
}
