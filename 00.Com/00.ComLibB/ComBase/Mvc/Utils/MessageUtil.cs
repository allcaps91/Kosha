using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComBase.Mvc.Utils
{
    public class MessageUtil
    {
        /// <summary>
        /// 경고메세지 (확인버튼)
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static DialogResult Alert(string message, Form ownerForm = null)
        {
            if(ownerForm != null)
            {
                //Form msgF = new Form();
                //msgF.Owner = ownerForm;
                //msgF.TopMost = true;
                //msgF.StartPosition = FormStartPosition.CenterParent;
                return MessageBox.Show(ownerForm, message, "경고", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                return MessageBox.Show(new Form() { TopMost = true, StartPosition = FormStartPosition.CenterParent }, message, "경고", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        /// <summary>
        /// 정보메세지 (확인버튼)
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static DialogResult Info(string message, Form ownerForm = null)
        {
            if (ownerForm != null)
            {
                //Form msgF = new Form();
                //msgF.Owner = ownerForm;
                //msgF.TopMost = true;
                //msgF.StartPosition = FormStartPosition.CenterParent;
                return MessageBox.Show(ownerForm, message, "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                return MessageBox.Show(new Form() { TopMost = true, StartPosition = FormStartPosition.CenterParent }, message, "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 오류메세지 (확인버튼)
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static DialogResult Error(string message, Form ownerForm = null)
        {
            if (ownerForm != null)
            {
                //Form msgF = new Form();
                //msgF.Owner = ownerForm;
                //msgF.TopMost = true;
                //msgF.StartPosition = FormStartPosition.CenterParent;
                return MessageBox.Show(ownerForm, message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                return MessageBox.Show(new Form() { TopMost = true, StartPosition = FormStartPosition.CenterParent }, message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// 확인메세지 Yes or No
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static DialogResult Confirm(string message, Form ownerForm = null, MessageBoxDefaultButton btn2 = MessageBoxDefaultButton.Button2)
        {
            if(ownerForm != null)
            {
                //Form msgF = new Form();
                //msgF.Owner = ownerForm;
                //msgF.StartPosition = FormStartPosition.CenterParent;
                //msgF.TopMost = true;
                return MessageBox.Show(ownerForm, message, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question, btn2);
            }
            else
            {
                return MessageBox.Show(new Form() { TopMost = true, StartPosition = FormStartPosition.CenterParent }, message, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question, btn2);
            }
        }
        
    }
}
