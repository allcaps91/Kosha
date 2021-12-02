using System;
using System.Reflection;    //Assembly 
using System.Windows.Forms;

namespace ComEmrBase
{
    public static class clsEmrFormMap
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strNameSpace">ComEmrLibS</param>
        /// <param name="strForm">폼이름</param>
        /// <param name="po">환자정보</param>
        /// <param name="strEmrNo">EMRNO</param>
        /// <param name="strMode">V:조회,수정, W:신규작성 스프래드 형식의 경우 애매하다.</param>
        /// <returns></returns>
        public static Form EmrFormMappingEx(string strFormNm, string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, FormEmrMessage pEmrCallForm)
        {
            try
            {
                //ActiveFormView = new frmNutritionCounSelRecord(strFormNo,strUpdateNo, po, strEmrNo, strMode);
                Assembly assem = Assembly.GetExecutingAssembly();
                Form objForm = null;

                Type t = assem.GetType("ComEmrBase" + "." + strFormNm);
                objForm = (Form)Activator.CreateInstance(t, strFormNo, strUpdateNo, po, strEmrNo, strMode, pEmrCallForm);
                return objForm;

                //Type t = assembly.GetType(strFormNm);
                ////objForm = (Form)Activator.CreateInstance(t);
                //objForm = (Form)Activator.CreateInstance(t, strFormNo, strUpdateNo, po, strEmrNo, strMode);
                //return objForm;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 19-07-05 신규 생성자 추가
        /// </summary>
        /// <param name="strFormNm"></param>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="po"></param>
        /// <param name="strEmrNo"></param>
        /// <param name="strMode"></param>
        /// <param name="strOrderDate">VB mstrOrderDate 활용위해서 추가.</param>
        /// <param name="pEmrCallForm"></param>
        /// <returns></returns>
        public static Form EmrFormMappingEx(string strFormNm, string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, string strOrderDate, FormEmrMessage pEmrCallForm)
        {
            try
            {
                //ActiveFormView = new frmNutritionCounSelRecord(strFormNo,strUpdateNo, po, strEmrNo, strMode);
                Assembly assem = Assembly.GetExecutingAssembly();
                Form objForm = null;

                Type t = assem.GetType("ComEmrBase" + "." + strFormNm);
                objForm = (Form)Activator.CreateInstance(t, strFormNo, strUpdateNo, po, strEmrNo, strMode, strOrderDate, pEmrCallForm);
                return objForm;

                //Type t = assembly.GetType(strFormNm);
                ////objForm = (Form)Activator.CreateInstance(t);
                //objForm = (Form)Activator.CreateInstance(t, strFormNo, strUpdateNo, po, strEmrNo, strMode);
                //return objForm;
            }
            catch
            {
                return null;
            }
        }

        public static Form EmrFormMappingTemp(string strFormNm, string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode)
        {
            try
            {
                FormEmrMessage pEmrCallForm = null;

                //ActiveFormView = new frmNutritionCounSelRecord(strFormNo,strUpdateNo, po, strEmrNo, strMode);
                Assembly assem = Assembly.GetExecutingAssembly();
                //Assembly assembly = null;
                Form objForm = null;

                Type t = assem.GetType("ComEmrDg" + "." + strFormNm);
                objForm = (Form)Activator.CreateInstance(t, strFormNo, strUpdateNo, po, strEmrNo, strMode, pEmrCallForm);
                return objForm;

                //Type t = assembly.GetType(strFormNm);
                ////objForm = (Form)Activator.CreateInstance(t);
                //objForm = (Form)Activator.CreateInstance(t, strFormNo, strUpdateNo, po, strEmrNo, strMode);
                //return objForm;
            }
            catch
            {
                return null;
            }
        }

        public static Form EmrFormMappingExOld(string strFormNm, string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode)
        {
            try
            {
                //ActiveFormView = new frmNutritionCounSelRecord(strFormNo,strUpdateNo, po, strEmrNo, strMode);
                Assembly assem = Assembly.GetExecutingAssembly();
                //Assembly assembly = null;
                Form objForm = null;

                Type t = assem.GetType("ComEmrDg" + "." + strFormNm);
                objForm = (Form)Activator.CreateInstance(t, strFormNo, strUpdateNo, po, strEmrNo, strMode);
                return objForm;

                //Type t = assembly.GetType(strFormNm);
                ////objForm = (Form)Activator.CreateInstance(t);
                //objForm = (Form)Activator.CreateInstance(t, strFormNo, strUpdateNo, po, strEmrNo, strMode);
                //return objForm;
            }
            catch
            {
                return null;
            }
        }


    }
}
