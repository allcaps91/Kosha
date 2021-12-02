using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
//using System.Type;
using System.Xml;
using ComBase; //기본 클래스
using ComBase.Controls;
using ComDbB; //DB연결
using Oracle.DataAccess.Client;

namespace ComEmrBase
{
    public class clsXML
    {
        //private static int intItemCnt;
        //private static object gObj = null;

        #region //New Old 공통


        /// <summary>
        /// VB6 컨트롤 배열일 경우 => 컨버젼후 "_"가 있는지 확인한다.
        /// </summary>
        /// <param name="ctlControl"></param>
        /// <returns></returns>
        public static string IsArryCon(Control ctlControl)
        {
            string rtnVal = "";
            int intFind = 0;

            try
            {
                intFind = VB.InStr(ctlControl.Name.ToString(), "_");

                if (intFind == 0)
                {
                    rtnVal = "";
                }
                else
                {
                    //"_"뒤에 있는 숫자를 읽어서 한다.
                    rtnVal = VB.Right(ctlControl.Name.ToString(), ctlControl.Name.ToString().Length - intFind);
                }

                return rtnVal;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return rtnVal;
            }
        }

        /// <summary>
        /// 컨트롤이 패널(그룹)인지 확인
        /// </summary>
        /// <param name="cControl"></param>
        /// <param name="pControl"></param>
        /// <returns></returns>
        public static bool IsParent(Control cControl, Control pControl)
        {
            Control ctlControl = null;

            if (cControl.Parent == null)
            {
                return false;
            }

            if (cControl.Parent == pControl)
            {
                return true;
            }
            else
            {
                if (cControl.Parent is Form)
                {
                    return false;
                }

                ctlControl = cControl.Parent;
                return IsParent(ctlControl, pControl);
            }
        }

        /// <summary>
        /// XML Document를 생성한다
        /// </summary>
        /// <param name="objParent"></param>
        /// <param name="Doc"></param>
        /// <returns></returns>
        public static XmlDocument setXMLCreate(Control objParent, XmlDocument Doc)
        {
            XmlElement DocRoot = null;
            //XmlElement item = null;
            //XmlCDataSection strValue = null;

            try
            {
                DocRoot = Doc.DocumentElement;

                foreach (Control objControl in objParent.Controls)
                {
                    if (objControl.Controls.Count > 0)
                    {
                        if (objControl.Name != "usCommonFrmTop")
                        {
                            Doc = setChild(objControl, Doc, DocRoot);
                        }
                    }

                }

                return Doc;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// XML Document를 생성한다 : Child Node 생성
        /// </summary>
        /// <param name="objParent"></param>
        /// <param name="Doc"></param>
        /// <param name="DocRoot"></param>
        /// <returns></returns>
        private static XmlDocument setChild(Control objParent, XmlDocument Doc, XmlElement DocRoot)
        {
            //Control ctl = null;
            //XmlElement item = null;
            //XmlCDataSection strValue = null;

            try
            {
                DocRoot = Doc.DocumentElement;

                foreach (Control objControl in objParent.Controls)
                {
                    if (objControl.Controls.Count > 0)
                    {
                        Doc = setChild(objControl, Doc, DocRoot);
                    }


                }

                return Doc;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// XML 저장된 데이타를 파싱한다
        /// </summary>
        /// <param name="objParent"></param>
        /// <returns></returns>
        public static XmlDocument setXMLParser(Control objParent)
        {
            XmlDocument Doc = null;
            XmlDeclaration dec = null;
            XmlElement DocRoot = null;

            try
            {
                dec = Doc.CreateXmlDeclaration("1.0", "UTF-8", null);

                Doc.AppendChild(dec);

                DocRoot = Doc.CreateElement("chart");
                Doc.AppendChild(DocRoot);

                Doc = setXMLCreate(objParent, Doc);

                //intItemCnt = 1;

                return Doc;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 폼서식을 저장한다.
        /// </summary>
        /// <param name="frmXmlForm"></param>
        /// <param name="isSpcPanel"></param>
        /// <param name="pControl"></param>
        /// <returns></returns>
        public static string SaveDataToXml(Control frmXmlForm, bool isSpcPanel, Control pControl, bool SaveImage)
        {
            string rtnVal = "";
            string strXML = "";
            //int i = 0;
            //int j = 0;
            string strConIndex = "";

            XmlDocument Doc = new XmlDocument();
            XmlDeclaration dec = null;
            XmlElement DocRoot = null;

            try
            {
                if (isSpcPanel == true)
                {
                    if (pControl == null)
                    {
                        MessageBox.Show(new Form() { TopMost = true }, "선택된 컨테이너가 존재하지 않습니다.");
                        return rtnVal;
                    }
                }

                //해드 선언
                dec = Doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                Doc.AppendChild(dec);
                DocRoot = Doc.CreateElement("chart");
                Doc.AppendChild(DocRoot);

                strXML = Doc.ToString();

                strXML = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + "\r\n" + "<chart>" + "\r\n";

                Control[] controls = null;

                if (isSpcPanel == true)
                {
                    controls = ComFunc.GetAllControls(pControl);
                }
                else
                {
                    controls = ComFunc.GetAllControls(frmXmlForm);
                }


                string strConName = "";

                foreach (Control objControl in controls)
                {
                    strConIndex = "";
                    strConIndex = IsArryCon(objControl);

                    strConName = objControl.Name;

                    //if ((objControl is FarPoint.Win.Spread.FpSpread) == false)
                    if ((objControl is PictureBox) || (objControl is TextBox) || (objControl is ComboBox) || (objControl is CheckBox) || (objControl is RadioButton))
                    {
                        if (objControl is TextBox)
                        {
                            if (((TextBox)objControl).Text.Trim() == "") continue;
                        }

                        if (objControl is ComboBox)
                        {
                            if (((ComboBox)objControl).Text.Trim() == "") continue;
                        }

                        if (objControl is CheckBox)
                        {
                            if (((CheckBox)objControl).Checked == false) continue;
                        }

                        if (objControl is RadioButton)
                        {
                            if (((RadioButton)objControl).Checked == false) continue;
                        }

                        if (VB.InStr(strConName, "_") > 0)
                        {
                            string[] strParams = VB.Split(VB.Trim(strConName), "_", -1);
                            strConName = strParams[0];
                        }

                        //DateTimePicker(DTPicker)
                        if (objControl is DateTimePicker)
                        {
                            if (isSpcPanel == true)
                            {
                                if (IsParent(objControl, pControl) == true)
                                {
                                    strXML = strXML + "<" + strConName + "><![CDATA[" + VB.Format(((DateTimePicker)objControl).Value, "yyyy-MM-dd") + "]]></" + strConName + ">";
                                }
                            }
                            else
                            {
                                strXML = strXML + "<" + strConName + "><![CDATA[" + VB.Format(((DateTimePicker)objControl).Value, "yyyy-MM-dd") + "]]></" + strConName + ">";
                            }
                        }
                        //Pic
                        if (SaveImage == true)
                        {
                            if (objControl is PictureBox)
                            {
                                string strTag = "";

                                if (isSpcPanel == true)
                                {
                                    if (IsParent(objControl, pControl) == true)
                                    {
                                        if (objControl.Tag != null) strTag = VB.Trim(objControl.Tag.ToString());
                                        strXML = strXML + "<" + strConName + "><![CDATA[" + strTag + "]]></" + strConName + ">";
                                    }
                                }
                                else
                                {
                                    if (objControl.Tag != null) strTag = VB.Trim(objControl.Tag.ToString());
                                    strXML = strXML + "<" + strConName + "><![CDATA[" + strTag + "]]></" + strConName + ">";
                                }
                            }
                        }
                        //TextBox
                        if (objControl is TextBox)
                        {
                            if (isSpcPanel == true)
                            {
                                if (IsParent(objControl, pControl) == true)
                                {
                                    strXML = strXML + "<" + strConName + "><![CDATA[" + VB.Trim(objControl.Text.Replace("'", "`")) + "]]></" + strConName + ">";
                                }
                            }
                            else
                            {
                                strXML = strXML + "<" + strConName + "><![CDATA[" + VB.Trim(objControl.Text.Replace("'", "`")) + "]]></" + strConName + ">";
                            }
                        }
                        //ComboBox
                        if (objControl is ComboBox)
                        {
                            if (isSpcPanel == true)
                            {
                                if (IsParent(objControl, pControl) == true)
                                {
                                    strXML = strXML + "<" + strConName + "><![CDATA[" + VB.Trim(objControl.Text) + "]]></" + strConName + ">";
                                }
                            }
                            else
                            {
                                strXML = strXML + "<" + strConName + "><![CDATA[" + VB.Trim(objControl.Text) + "]]></" + strConName + ">";
                            }
                        }
                        //CheckBox
                        if (objControl is CheckBox)
                        {
                            if (isSpcPanel == true)
                            {
                                if (IsParent(objControl, pControl) == true)
                                {
                                    strXML = strXML + "<" + strConName + "><![CDATA[" + (((CheckBox)objControl).Checked == true ? "1" : "0") + "]]></" + strConName + ">";
                                }
                            }
                            else
                            {
                                strXML = strXML + "<" + strConName + "><![CDATA[" + (((CheckBox)objControl).Checked == true ? "1" : "0") + "]]></" + strConName + ">";
                            }
                        }
                        //RadioButton(OptionButton)
                        if (objControl is RadioButton)
                        {
                            if (isSpcPanel == true)
                            {
                                if (IsParent(objControl, pControl) == true)
                                {
                                    strXML = strXML + "<" + strConName + "><![CDATA[" + (((RadioButton)objControl).Checked == true ? "1" : "0") + "]]></" + strConName + ">";
                                }
                            }
                            else
                            {
                                strXML = strXML + "<" + strConName + "><![CDATA[" + (((RadioButton)objControl).Checked == true ? "1" : "0") + "]]></" + strConName + ">";
                            }
                        }
                        //fpSpread
                        //if (objControl is FarPoint.Win.Spread.FpSpread)
                        //{
                        //    strConIndex = "";
                        //    if (strConName != "ssMacroWord")
                        //    {
                        //        if (isSpcPanel == true)
                        //        {
                        //            if (IsParent(objControl, pControl) == true)
                        //            {
                        //                FarPoint.Win.Spread.FpSpread ssSpd = null;
                        //                ssSpd = (FarPoint.Win.Spread.FpSpread)objControl;

                        //                strXML = strXML + "<" + strConName + " Type=\"fpSpreadSheet\"" + " Index=\"" + strConIndex + "\"><![CDATA[" + Convert.ToString(ssSpd.ActiveSheet.RowCount) + "_" + Convert.ToString(ssSpd.ActiveSheet.ColumnCount) + "]]></" + strConName + ">";
                        //                for (i = 0; i < ssSpd.ActiveSheet.RowCount; i++)
                        //                {
                        //                    for (j = 0; j < ssSpd.ActiveSheet.ColumnCount; j++)
                        //                    {
                        //                        strXML = strXML + SaveSpreadData(ssSpd, i, j);
                        //                    }
                        //                }
                        //            }
                        //        }
                        //        else
                        //        {
                        //            FarPoint.Win.Spread.FpSpread ssSpd = null;
                        //            ssSpd = (FarPoint.Win.Spread.FpSpread)objControl;

                        //            strXML = strXML + "<" + strConName + " Type=\"fpSpreadSheet\"" + " Index=\"" + strConIndex + "\"><![CDATA[" + Convert.ToString(ssSpd.ActiveSheet.RowCount) + "_" + Convert.ToString(ssSpd.ActiveSheet.ColumnCount) + "]]></" + strConName + ">";
                        //            for (i = 0; i < ssSpd.ActiveSheet.RowCount; i++)
                        //            {
                        //                for (j = 0; j < ssSpd.ActiveSheet.ColumnCount; j++)
                        //                {
                        //                    strXML = strXML + SaveSpreadData(ssSpd, i, j);
                        //                }
                        //            }
                        //        }
                        //    }
                        //}
                    }


                }

                if (strXML != "")
                {
                    strXML = strXML + "</chart>";
                }
                return strXML;

            }
            catch (Exception ex)
            {
                MessageBox.Show(new Form() { TopMost = true }, ex.Message);
                return "";
            }
        }

        /// <summary>
        /// 폼서식을 저장한다. Old
        /// </summary>
        /// <param name="frmXmlForm"></param>
        /// <param name="isSpcPanel"></param>
        /// <param name="pControl"></param>
        /// <returns></returns>
        public static string SaveDataToXmlOld(Control frmXmlForm, bool isSpcPanel, Control pControl, ref clsEmrType.EmrXmlImage[] pEmrXmlImage, clsEmrType.EmrXmlImageInit[] pEmrXmlImageInit, string strFormNo = "")
        {
            string rtnVal = "";
            string strXML = "";
            //int i = 0;
            string strConIndex = "";

            XmlDocument Doc = new XmlDocument();
            XmlDeclaration dec = null;
            XmlElement DocRoot = null;

            string strContent = GetXML((long) VB.Val(strFormNo));

            try
            {
                if (isSpcPanel == true)
                {
                    if (pControl == null)
                    {
                        ComFunc.MsgBox("선택된 컨테이너가 존재하지 않습니다.");
                        return rtnVal;
                    }
                }

                //해드 선언
                dec = Doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                Doc.AppendChild(dec);
                DocRoot = Doc.CreateElement("chart");
                Doc.AppendChild(DocRoot);

                strXML = Doc.ToString();

                strXML = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + "\r\n" + "<chart>" + "\r\n";

                Control[] controls = null;

                if (isSpcPanel == true)
                {
                    controls = ComFunc.GetAllControls(pControl);
                }
                else
                {
                    controls = ComFunc.GetAllControls(frmXmlForm);
                }


                string strConName = "";

                foreach (Control objControl in controls)
                {
                    strConIndex = "";
                    strConIndex = IsArryCon(objControl);

                    strConName = objControl.Name;

                    if (strConName == "dtMedFrDate" || strConName == "txtMedFrTime") continue;

                    //if ((objControl is FarPoint.Win.Spread.FpSpread) == false)
                    if ((objControl is PictureBox) || (objControl is TextBox) || (objControl is ComboBox) || (objControl is CheckBox) || (objControl is RadioButton))
                    {
                        if (objControl is TextBox)
                        {
                            if (((TextBox)objControl).Text.Trim() == "") continue;
                        }

                        if (objControl is ComboBox)
                        {
                            if (((ComboBox)objControl).Text.Trim() == "") continue;
                        }

                        if (objControl is CheckBox)
                        {
                            if (((CheckBox)objControl).Checked == false) continue;
                        }

                        if (objControl is RadioButton)
                        {
                            if (((RadioButton)objControl).Checked == false) continue;
                        }

                        if (VB.InStr(strConName, "_") > 0)
                        {
                            string[] strParams = VB.Split(VB.Trim(strConName), "_", -1);
                            strConName = strParams[0];
                        }

                        //DateTimePicker(DTPicker)
                        if (objControl is DateTimePicker)
                        {
                            if (isSpcPanel == true)
                            {
                                if (IsParent(objControl, pControl) == true)
                                {
                                    strXML = strXML + "<" + strConName + "><![CDATA[" + VB.Format(((DateTimePicker)objControl).Value, "yyyy-MM-dd") + "]]></" + strConName + ">";
                                }
                            }
                            else
                            {
                                strXML = strXML + "<" + strConName + "><![CDATA[" + VB.Format(((DateTimePicker)objControl).Value, "yyyy-MM-dd") + "]]></" + strConName + ">";
                            }
                        }
                        //Pic
                        if (objControl is PictureBox)
                        {
                            //string strTag = "";

                            //if (isSpcPanel == true)
                            //{
                            //    if (IsParent(objControl, pControl) == true)
                            //    {
                            //        if (objControl.Tag != null) strTag = VB.Trim(objControl.Tag.ToString());
                            //        strXML = strXML + "<" + strConName + "><![CDATA[" + strTag + "]]></" + strConName + ">";
                            //    }
                            //}
                            //else
                            //{
                            //    if (objControl.Tag != null) strTag = VB.Trim(objControl.Tag.ToString());
                            //    strXML = strXML + "<" + strConName + "><![CDATA[" + strTag + "]]></" + strConName + ">";
                            //}


                            if ( ((PictureBox)objControl).Image == null || objControl.BackColor == Color.LightGray)
                            {
                                //strXML = strXML + "<" + strConName + " type=\"image\" label=\"OLD\" src=\"getChartImage.mts?imgNo=0\"></" + strConName + ">";
                                string strNotImgNo = GetImageNo(strContent, objControl.Name);
                                strXML = strXML + "<" + strConName + " type=\"image\" label=\"OLD\" src=\"images/upload/noimage.png?time=" + strNotImgNo + "\"></" + strConName + ">";
                            }
                            else
                            {
                                //기본 이미지 비교
                                bool ChangePicImage = false;
                                //변경해서 저장한 이미지번호 있을경우
                                string strInitImagNo = string.Empty;

                                int j = 0;
                                if (pEmrXmlImageInit != null)
                                {
                                    strInitImagNo = string.Empty;
                                    for (j = 0; j < pEmrXmlImageInit.Length; j++)
                                    {
                                            //if (objControl == pEmrXmlImageInit[j].Pic)
                                            //{
                                            //    if (((PictureBox)objControl).Image != pEmrXmlImageInit[j].Img)
                                            //    {
                                            //        ChangePicImage = true;
                                            //    }
                                            //}
                                        if (((PictureBox)objControl).Name.Equals(pEmrXmlImageInit[j].ContNm))
                                        {
                                            //pEmrXmlImageInit[j].Img.Save(@"C:\YAK_IMAGE\a.jpg");
                                            //((PictureBox)objControl).Image.Save(@"C:\YAK_IMAGE\a2.jpg");

                                            if (((PictureBox)objControl).Image != null && pEmrXmlImageInit[j].Img == null
                                            ||
                                            ((PictureBox)objControl).Image != null && pEmrXmlImageInit[j].Img != null &&
                                                clsEmrFunc.ImageCompare((Bitmap)((PictureBox)objControl).Image, (Bitmap) (pEmrXmlImageInit[j].Img)) == false)
                                            {
                                                ChangePicImage = true;
                                            }
                                            else if(pEmrXmlImageInit[j].ImageNo.Length > 0 && pEmrXmlImageInit[j].ImageNo != GetImageNo(strContent, objControl.Name) &&
                                                    clsEmrFunc.ImageCompare((Bitmap)((PictureBox)objControl).Image, (Bitmap)pEmrXmlImageInit[j].Img))
                                            {
                                                strInitImagNo = pEmrXmlImageInit[j].ImageNo;
                                            } 
                                        }

                                    }
                                }


                                if (ChangePicImage == true)
                                {
                                    string strEMRIMAGENO = "0";
                                    strEMRIMAGENO = (ComQuery.GetSequencesNoEx(clsDB.DbCon, "KOSMOS_EMR.EMRXMLIMAGES_EMRIMAGENO_SEQ")).ToString();

                                    Array.Resize<clsEmrType.EmrXmlImage>(ref pEmrXmlImage, pEmrXmlImage.Length + 1);
                                    pEmrXmlImage[pEmrXmlImage.Length - 1].Pic = objControl;
                                    pEmrXmlImage[pEmrXmlImage.Length - 1].ContNm = ((PictureBox)objControl).Name;
                                    pEmrXmlImage[pEmrXmlImage.Length - 1].ImageNo = strEMRIMAGENO;

                                    strXML = strXML + "<" + strConName + " type=\"image\" label=\"OLD\" src=\"getEmrImage.mts?emrImageNo=" + strEMRIMAGENO + "\" emrimageno=\"" + strEMRIMAGENO + "\"></" + strConName + ">";
                                }
                                else if (strInitImagNo.Length > 0)
                                {
                                    strXML = strXML + "<" + strConName + " type=\"image\" label=\"OLD\" src=\"getEmrImage.mts?emrImageNo=" + strInitImagNo + "\" emrimageno=\"" + strInitImagNo + "\"></" + strConName + ">";
                                }
                                else
                                {
                                    strXML = strXML + "<" + strConName + " type=\"image\" label=\"OLD\" src=\"getChartImage.mts?imgNo=" + GetImageNo(strContent, objControl.Name) + "\"></" + strConName + ">";
                                }
                            }
                        }
                        //TextBox
                        if (objControl is TextBox)
                        {
                            if (isSpcPanel == true)
                            {
                                if (IsParent(objControl, pControl) == true)
                                {
                                    if(( (TextBox)objControl).Multiline == true)
                                    {
                                        strXML = strXML + "<" + strConName + " type=\"textArea\" ><![CDATA[" + objControl.Text.Trim() + "]]></" + strConName + ">";
                                    }
                                    else
                                    {
                                        if (VB.Left(objControl.Name, 2) == "dt")
                                        {
                                            strXML = strXML + "<" + strConName + " type=\"inputDate\"><![CDATA[" + objControl.Text.Trim() + "]]></" + strConName + ">";
                                        }
                                        else
                                        {
                                            strXML = strXML + "<" + strConName + " type=\"inputText\"><![CDATA[" + objControl.Text.Trim() + "]]></" + strConName + ">";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (((TextBox)objControl).Multiline == true)
                                {
                                    strXML = strXML + "<" + strConName + " type=\"textArea\" ><![CDATA[" + objControl.Text.Trim() + "]]></" + strConName + ">";
                                }
                                else
                                {
                                    if (VB.Left(objControl.Name, 2) == "dt")
                                    {
                                        strXML = strXML + "<" + strConName + " type=\"inputDate\"><![CDATA[" + objControl.Text.Trim() + "]]></" + strConName + ">";
                                    }
                                    else
                                    {
                                        strXML = strXML + "<" + strConName + " type=\"inputText\"><![CDATA[" + objControl.Text.Trim() + "]]></" + strConName + ">";
                                    }
                                }
                            }
                        }
                        //ComboBox
                        if (objControl is ComboBox)
                        {
                            if (isSpcPanel == true)
                            {
                                if (IsParent(objControl, pControl) == true)
                                {
                                    strXML = strXML + "<" + strConName + " type=\"inputText\"><![CDATA[" + objControl.Text.Trim() + "]]></" + strConName + ">";
                                }
                            }
                            else
                            {
                                strXML = strXML + "<" + strConName + " type=\"inputText\"><![CDATA[" +  objControl.Text.Trim() + "]]></" + strConName + ">";
                            }
                        }
                        //CheckBox
                        if (objControl is CheckBox)
                        {
                            //if (isSpcPanel == true)
                            //{
                            //    if (IsParent(objControl, pControl) == true)
                            //    {
                            //        strXML = strXML + "<" + strConName + " type=\"inputCheck\"><![CDATA[" + (((CheckBox)objControl).Checked == true ? "1" : "0") + "]]></" + strConName + ">";
                            //    }
                            //}
                            //else
                            //{
                            //    strXML = strXML + "<" + strConName + " type=\"inputCheck\"><![CDATA[" + (((CheckBox)objControl).Checked == true ? "1" : "0") + "]]></" + strConName + ">";
                            //}
                            if (isSpcPanel == true)
                            {
                                if (IsParent(objControl, pControl) == true)
                                {
                                    strXML = strXML + "<" + strConName + " type=\"inputCheck\"><![CDATA[" + (((CheckBox)objControl).Checked == true ? "true" : "false") + "]]></" + strConName + ">";
                                }
                            }
                            else
                            {
                                strXML = strXML + "<" + strConName + " type=\"inputCheck\"><![CDATA[" + (((CheckBox)objControl).Checked == true ? "true" : "false") + "]]></" + strConName + ">";
                            }
                        }
                        //RadioButton(OptionButton)
                        if (objControl is RadioButton)
                        {
                            //if (isSpcPanel == true)
                            //{
                            //    if (IsParent(objControl, pControl) == true)
                            //    {
                            //        strXML = strXML + "<" + strConName + " type=\"inputRadio\"><![CDATA[" + (((RadioButton)objControl).Checked == true ? "1" : "0") + "]]></" + strConName + ">";
                            //    }
                            //}
                            //else
                            //{
                            //    strXML = strXML + "<" + strConName + " type=\"inputRadio\"><![CDATA[" + (((RadioButton)objControl).Checked == true ? "1" : "0") + "]]></" + strConName + ">";
                            //}
                            if (isSpcPanel == true)
                            {
                                if (IsParent(objControl, pControl) == true)
                                {
                                    strXML = strXML + "<" + strConName + " type=\"inputRadio\"><![CDATA[" + (((RadioButton)objControl).Checked == true ? "true" : "false") + "]]></" + strConName + ">";
                                }
                            }
                            else
                            {
                                strXML = strXML + "<" + strConName + " type=\"inputRadio\"><![CDATA[" + (((RadioButton)objControl).Checked == true ? "true" : "false") + "]]></" + strConName + ">";
                            }
                        }

                        #region //Spread 사용안함
                        //fpSpread
                        //if (objControl is FarPoint.Win.Spread.FpSpread)
                        //{
                        //    strConIndex = "";
                        //    if (strConName != "ssMacroWord")
                        //    {
                        //        if (isSpcPanel == true)
                        //        {
                        //            if (IsParent(objControl, pControl) == true)
                        //            {
                        //                FarPoint.Win.Spread.FpSpread ssSpd = null;
                        //                ssSpd = (FarPoint.Win.Spread.FpSpread)objControl;

                        //                strXML = strXML + "<" + strConName + " Type=\"fpSpreadSheet\"" + " Index=\"" + strConIndex + "\"><![CDATA[" + Convert.ToString(ssSpd.ActiveSheet.RowCount) + "_" + Convert.ToString(ssSpd.ActiveSheet.ColumnCount) + "]]></" + strConName + ">";
                        //                for (i = 0; i < ssSpd.ActiveSheet.RowCount; i++)
                        //                {
                        //                    for (j = 0; j < ssSpd.ActiveSheet.ColumnCount; j++)
                        //                    {
                        //                        strXML = strXML + SaveSpreadData(ssSpd, i, j);
                        //                    }
                        //                }
                        //            }
                        //        }
                        //        else
                        //        {
                        //            FarPoint.Win.Spread.FpSpread ssSpd = null;
                        //            ssSpd = (FarPoint.Win.Spread.FpSpread)objControl;

                        //            strXML = strXML + "<" + strConName + " Type=\"fpSpreadSheet\"" + " Index=\"" + strConIndex + "\"><![CDATA[" + Convert.ToString(ssSpd.ActiveSheet.RowCount) + "_" + Convert.ToString(ssSpd.ActiveSheet.ColumnCount) + "]]></" + strConName + ">";
                        //            for (i = 0; i < ssSpd.ActiveSheet.RowCount; i++)
                        //            {
                        //                for (j = 0; j < ssSpd.ActiveSheet.ColumnCount; j++)
                        //                {
                        //                    strXML = strXML + SaveSpreadData(ssSpd, i, j);
                        //                }
                        //            }
                        //        }
                        //    }
                        //}
                        #endregion //Spread 사용안함
                    }


                }

                if (strXML != "")
                {
                    strXML = strXML + "</chart>";
                }
                return strXML;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return "";
            }
        }

        /// <summary>
        /// 기본 이미지 imgno 가져오기 위해서 필요한 함수
        /// 
        /// </summary>
        /// <param name="strFormNo"></param>
        /// <returns></returns>
        public static string GetXML(long FormNo)
        {
            OracleDataReader reader = null;
            string SQL = string.Empty;
            string rtnVal = string.Empty;

            if (FormNo == 0)
                return rtnVal;

            try
            {
                SQL = "SELECT CONTENTS ";
                SQL += ComNum.VBLF + "FROM KOSMOS_EMR.EMRFORM";
                SQL += ComNum.VBLF + "  WHERE formno = " + FormNo;

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBox(sqlErr);
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    return rtnVal;
                }

                if (reader.HasRows && reader.Read())
                {
                    rtnVal = reader.GetValue(0).ToString().Trim();
                }

                reader.Dispose();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        /// <summary>
        /// 이미지번호 파싱함수
        /// </summary>
        /// <param name="strXML"></param>
        /// <param name="img"></param>
        /// <returns></returns>
        public static string GetImageNo(string strXML, string strName)
        {
            string rtnVal = string.Empty;
            if (strXML.IndexOf(" id=" + strName + " class=input_target") != -1)
            {
                string strTemp = " id=" + strName + " class=input_target";
                rtnVal = strXML.Substring(strXML.IndexOf(strTemp) + strTemp.Length);
                rtnVal = rtnVal.Substring(rtnVal.IndexOf("src=") + 5);
                rtnVal = rtnVal.Substring(rtnVal.IndexOf("=") + 1);
                rtnVal = rtnVal.Substring(0, rtnVal.IndexOf((char)34));
            }
            if (VB.IsNumeric(rtnVal) == false)
            {
                rtnVal = string.Empty;
            }
            return rtnVal;
        }

        #endregion ///New Old 공통

        /// <summary>
        /// AEMRCHARTROWHis 의 DATA를 폼에 뿌린다.
        /// </summary>
        /// <param name="frmXmlForm"></param>
        /// <param name="strEmrNo"></param>
        /// <param name="dtp"></param>
        /// <param name="cb"></param>
        public static void LoadDataChartHisRow(PsmhDb pDbCon, Control frmXmlForm, string EmrNoHis, DateTimePicker dtp, ComboBox cb)
        {
            string SQL =  string.Empty;
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;

            string strPRINTTYPE = string.Empty;
            string strFormNo = string.Empty;
            string strUpdateNo = string.Empty;
            string strSAVEGB = string.Empty;
            string strEmrNoHis = string.Empty;
            string strEmrNo = string.Empty;

            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT A.EMRNO, A.EMRNOHIS, A.CHARTDATE, A.CHARTTIME, A.FORMNO, A.UPDATENO, A.SAVEGB";
                SQL = SQL + ComNum.VBLF + "     , R.ITEMCD, R.ITEMINDEX, R.ITEMTYPE, R.ITEMVALUE, R.ITEMVALUE1 , R.ITEMVALUE2";
                SQL = SQL + ComNum.VBLF + "     , B.PRINTTYPE ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AEMRCHARTMSTHIS A";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROWHIS R";
                SQL = SQL + ComNum.VBLF + "       ON R.EMRNO      = A.EMRNO";
                SQL = SQL + ComNum.VBLF + "      AND R.EMRNOHIS   = A.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRFORM B     ";
                SQL = SQL + ComNum.VBLF + "       ON A.FORMNO     = B.FORMNO      ";
                SQL = SQL + ComNum.VBLF + "      AND A.UPDATENO   = B.UPDATENO     ";
                SQL = SQL + ComNum.VBLF + " WHERE A.EMRNOHIS    = " + VB.Val(EmrNoHis);

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }

                strEmrNo = dt.Rows[0]["EMRNO"].ToString().Trim();
                strPRINTTYPE = dt.Rows[0]["PRINTTYPE"].ToString().Trim();
                strFormNo = dt.Rows[0]["FORMNO"].ToString().Trim();
                strUpdateNo = dt.Rows[0]["UPDATENO"].ToString().Trim();
                strSAVEGB = dt.Rows[0]["SAVEGB"].ToString().Trim();
                strEmrNoHis = dt.Rows[0]["EMRNOHIS"].ToString().Trim();

                dtp.Value = DateTime.ParseExact(dt.Rows[0]["CHARTDATE"].ToString().Trim(), "yyyyMMdd", null);
                cb.Text = int.Parse(dt.Rows[0]["CHARTTIME"].ToString().Trim().Substring(0, 4)).ToString("00:00");

                int i = 0;
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    string strITEMCD     = string.Empty;
                    string strITEMVALUE  = string.Empty;
                    string strITEMVALUE1 = string.Empty;
                    string strITEMVALUE2 = string.Empty;
                    string strITEMTYPE   = string.Empty;

                    strITEMCD = dt.Rows[i]["ITEMCD"].ToString().Trim();

                    strITEMTYPE = dt.Rows[i]["ITEMTYPE"].ToString().Trim();
                    strITEMVALUE = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                    strITEMVALUE1 = dt.Rows[i]["ITEMVALUE1"].ToString().Trim();
                    strITEMVALUE2 = dt.Rows[i]["ITEMVALUE2"].ToString().Trim();

                    string strEmrNoOld = strEmrNo;

                    Control[] tx;
                    Control obj;
                    if (strITEMTYPE == "TEXT")
                    {
                        tx = frmXmlForm.Controls.Find(strITEMCD, true);
                        if (tx.Length > 0)
                        {
                            obj = (TextBox) tx[0];
                            //string strText = strITEMVALUE.Trim(); (strITEMVALUE.Replace("\n", "\r\n")).Replace("]]", "");
                            //string strText1 = strITEMVALUE1.Trim(); (strITEMVALUE1.Replace("\n", "\r\n")).Replace("]]", "");
                            string strText = strITEMVALUE.Trim(); strITEMVALUE.Replace("]]", "");
                            string strText1 = strITEMVALUE1.Trim(); strITEMVALUE1.Replace("]]", "");
                            string strText2 = strITEMVALUE2.Trim(); strITEMVALUE2.Replace("]]", "");
                            if (((TextBox) obj).Multiline == false)
                            {
                                obj.Text = strText.Replace("\r\n", " ") + strText1.Replace("\r\n", " ") + strText2.Replace("\r\n", " ");
                            }
                            else
                            {
                                obj.Text = strText + strText1 + strText2;
                            }
                        }
                    }                    
                    else if (strITEMTYPE == "CHECK")
                    {
                        tx = frmXmlForm.Controls.Find(strITEMCD, true);
                        if (tx.Length > 0)
                        {
                            obj = (CheckBox)tx[0];
                            ((CheckBox)obj).Checked = Convert.ToBoolean(VB.Val(strITEMVALUE));
                        }
                    }
                    else if (strITEMTYPE == "RADIO")
                    {
                        tx = frmXmlForm.Controls.Find(strITEMCD, true);
                        if (tx.Length > 0)
                        {
                            obj = (RadioButton)tx[0];
                            ((RadioButton)obj).Checked = Convert.ToBoolean(VB.Val(strITEMVALUE));
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                if (strPRINTTYPE == "0")
                {
                    if (strSAVEGB == "1")
                    {
                        //의사싸인
                        //SetSignImage(pDbCon, frmXmlForm, strEmrNo, strFormNo);
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }


        /// <summary>
        /// AEMRCHARTROW 의 DATA를 폼에 뿌린다.
        /// </summary>
        /// <param name="frmXmlForm"></param>
        /// <param name="strEmrNo"></param>
        /// <param name="blnErrOption"></param>
        /// <param name="OldYn"></param>
        /// <param name="dtp"></param>
        /// <param name="cb"></param>
        public static void LoadDataChartRow1963(PsmhDb pDbCon, Control ctl, string strEmrNo)
        {
            Control[] tx = ctl.Controls.Find("Content", true);
            if (tx.Length <= 0)
            {
                return;
            }

            RichTextBox obj = (RichTextBox)tx[0];
            string strTitle = string.Empty;
            string strValue = string.Empty;

            string SQL = string.Empty;
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "          A1.ITEMVALUE AS TA1";
            SQL += ComNum.VBLF + "       ,  A2.ITEMVALUE AS TA2";
            SQL += ComNum.VBLF + "       ,  A3.ITEMVALUE AS TA3";
            SQL += ComNum.VBLF + "       ,  A4.ITEMVALUE AS TA4";
            SQL += ComNum.VBLF + "  FROM KOSMOS_EMR.AEMRCHARTMST A";
            SQL += ComNum.VBLF + "    LEFT OUTER JOIN KOSMOS_EMR.AEMRCHARTROW A1";
            SQL += ComNum.VBLF + "      ON A1.EMRNO    = A.EMRNO";
            SQL += ComNum.VBLF + "     AND A1.EMRNOHIS = A.EMRNOHIS";
            SQL += ComNum.VBLF + "     AND A1.ITEMCD   = 'I0000002016'";
            SQL += ComNum.VBLF + "    LEFT OUTER JOIN KOSMOS_EMR.AEMRCHARTROW A2";
            SQL += ComNum.VBLF + "      ON A2.EMRNO    = A.EMRNO";
            SQL += ComNum.VBLF + "     AND A2.EMRNOHIS = A.EMRNOHIS";
            SQL += ComNum.VBLF + "     AND A2.ITEMCD   = 'I0000014620'";
            SQL += ComNum.VBLF + "    LEFT OUTER JOIN KOSMOS_EMR.AEMRCHARTROW A3";
            SQL += ComNum.VBLF + "      ON A3.EMRNO    = A.EMRNO";
            SQL += ComNum.VBLF + "     AND A3.EMRNOHIS = A.EMRNOHIS";
            SQL += ComNum.VBLF + "     AND A3.ITEMCD   = 'I0000000190'";
            SQL += ComNum.VBLF + "    LEFT OUTER JOIN KOSMOS_EMR.AEMRCHARTROW A4";
            SQL += ComNum.VBLF + "      ON A4.EMRNO    = A.EMRNO";
            SQL += ComNum.VBLF + "     AND A4.EMRNOHIS = A.EMRNOHIS";
            SQL += ComNum.VBLF + "     AND A4.ITEMCD   = 'I0000014735'";
            SQL += ComNum.VBLF + " WHERE A.EMRNO = " + strEmrNo;

            DataTable dt = null;
            string SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                ComFunc.MsgBox(SqlErr);
                return;
            }

            if( dt.Rows.Count == 0)
            {
                dt.Dispose();
                return;
            }

            #region ta1~4 순서대로
            strValue = dt.Rows[0]["ta1"].ToString().Trim();
            if (strValue.Length > 0)
            {
                strTitle = "S)";
                strValue = strValue.Replace(Environment.NewLine, "\n");
                strValue = strValue.Replace("\n", "   \n");

                using (Font bFont = new Font("serif", 11, FontStyle.Bold))
                {
                    int intStart = obj.TextLength == 0 ? 0 : obj.TextLength;
                    obj.AppendText(strTitle);
                    obj.AppendText(ComNum.VBLF);
                    obj.Select(intStart, strTitle.Length);
                    obj.SelectionFont = bFont;
                }
                obj.AppendText("  " + strValue + ComNum.VBLF);
            }

            strValue = dt.Rows[0]["ta2"].ToString().Trim();
            if (strValue.Length > 0)
            {
                strTitle = "O)";
                strValue = strValue.Replace(Environment.NewLine, "\n");
                strValue = strValue.Replace("\n", "   \n");

                using (Font bFont = new Font("serif", 11, FontStyle.Bold))
                {
                    int intStart = obj.TextLength == 0 ? 0 : obj.TextLength;
                    obj.AppendText(strTitle);
                    obj.AppendText(ComNum.VBLF);
                    obj.Select(intStart, strTitle.Length);
                    obj.SelectionFont = bFont;
                }
                obj.AppendText("  " + strValue + ComNum.VBLF);
            }

            strValue = dt.Rows[0]["ta3"].ToString().Trim();
            if (strValue.Length > 0)
            {
                strTitle = "A)";
                strValue = strValue.Replace(Environment.NewLine, "\n");
                strValue = strValue.Replace("\n", "   \n");

                using (Font bFont = new Font("serif", 11, FontStyle.Bold))
                {
                    int intStart = obj.TextLength == 0 ? 0 : obj.TextLength;
                    obj.AppendText(strTitle);
                    obj.AppendText(ComNum.VBLF);
                    obj.Select(intStart, strTitle.Length);
                    obj.SelectionFont = bFont;
                }
                obj.AppendText("  " + strValue + ComNum.VBLF);
            }

            strValue = dt.Rows[0]["ta4"].ToString().Trim();
            if (strValue.Length > 0)
            {
                strTitle = "P)";
                strValue = strValue.Replace(Environment.NewLine, "\n");
                strValue = strValue.Replace("\n", "   \n");

                using (Font bFont = new Font("serif", 11, FontStyle.Bold))
                {
                    int intStart = obj.TextLength == 0 ? 0 : obj.TextLength;
                    obj.AppendText(strTitle);
                    obj.AppendText(ComNum.VBLF);
                    obj.Select(intStart, strTitle.Length);
                    obj.SelectionFont = bFont;
                }
                obj.AppendText("  " + strValue + ComNum.VBLF);
            }

            #endregion

            dt.Dispose();
            return;
        }

        /// <summary>
        /// 경과기록지(XML + 신규)
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ctl"></param>
        /// <param name="strEmrNo"></param>
        public static void LoadDataChartRow963(PsmhDb pDbCon, Control ctl, string strEmrNo)
        {
            string SQL = string.Empty;
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "        GBN";
            SQL += ComNum.VBLF + "      , CHARTDATE";
            SQL += ComNum.VBLF + "      , CHARTTIME ";
            SQL += ComNum.VBLF + "      , ITEMCD ";
            SQL += ComNum.VBLF + "      , ITEMVALUE ";
            SQL += ComNum.VBLF + "      , ITEMVALUE1 ";
            SQL += ComNum.VBLF + "      , ITEMVALUE2 ";
            SQL += ComNum.VBLF + "FROM ";
            SQL += ComNum.VBLF + "( ";

            SQL += ComNum.VBLF + "  SELECT 'OLD' AS GBN, CHARTDATE, CHARTTIME, 'I0000000981' AS ITEMCD, EXTRACT(CHARTXML, '//ta1').GETCLOBVAL() AS ITEMVALUE, '' AS ITEMVALUE1, '' AS ITEMVALUE2";
            SQL += ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "EMRXML A";
            SQL += ComNum.VBLF + "   WHERE A.EMRNO = " + VB.Val(strEmrNo);

            SQL += ComNum.VBLF + "  UNION ALL";
            SQL += ComNum.VBLF + "  SELECT 'NEW' AS GBN, CHARTDATE, CHARTTIME, R.ITEMCD, TO_CLOB(R.ITEMVALUE) AS ITEMVALUE, R.ITEMVALUE1, R.ITEMVALUE2";
            SQL += ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
            SQL += ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R";
            SQL += ComNum.VBLF + "       ON R.EMRNO = A.EMRNO";
            SQL += ComNum.VBLF + "      AND R.EMRNOHIS = A.EMRNOHIS";
            SQL += ComNum.VBLF + "      AND R.ITEMCD > CHR(0)";
            SQL += ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRFORM B     ";
            SQL += ComNum.VBLF + "       ON A.FORMNO    = B.FORMNO      ";
            SQL += ComNum.VBLF + "      AND A.UPDATENO  = B.UPDATENO     ";
            SQL += ComNum.VBLF + "   WHERE A.EMRNO = " + VB.Val(strEmrNo);
            SQL += ComNum.VBLF + ") ";

            DataTable dt = null;
            string SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                ComFunc.MsgBox(SqlErr);
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                return;
            }

            TextBox obj = null;
            Control tx = null;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string strITEMCD = dt.Rows[i]["ITEMCD"].ToString().Trim();
                string strITEMVALUE = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                string strITEMVALUE1 = dt.Rows[i]["ITEMVALUE1"].ToString().Trim();
                string strITEMVALUE2 = dt.Rows[i]["ITEMVALUE2"].ToString().Trim();

                //List<Control> a = ComFunc.GetAllControls(ctl).Where(d => d is TextBox).ToList();

                tx = ctl.Controls.Find(strITEMCD, true).FirstOrDefault();
                if (tx != null)
                {
                    obj = tx as TextBox;
                    string strText = strITEMVALUE.Trim();
                    string strText1 = strITEMVALUE1.Trim();
                    string strText2 = strITEMVALUE2.Trim();

                    if (dt.Rows[i]["GBN"].ToString().Equals("OLD"))
                    {
                        obj.Text = VB.Replace(GetContent(strText), "\n", "\r\n", 1, -1);
                    }
                    else
                    {
                        obj.Text = strText + strText1 + strText2;
                    }

                }
            }

            dt.Dispose();
            return;
        }

        public static string GetContent(string strXml)
        {
            if (string.IsNullOrWhiteSpace(strXml))
                return string.Empty;

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(strXml);
            return xml.InnerText;
        }

        /// <summary>
        /// AEMRCHARTROW 의 DATA를 폼에 뿌린다.
        /// </summary>
        /// <param name="frmXmlForm"></param>
        /// <param name="strEmrNo"></param>
        /// <param name="blnErrOption"></param>
        /// <param name="OldYn"></param>
        /// <param name="dtp"></param>
        /// <param name="cb"></param>
        public static void LoadDataChartRow(PsmhDb pDbCon, Control frmXmlForm, string strEmrNo, bool blnErrOption, bool OldYn, DateTimePicker dtp, ComboBox cb, FarPoint.Win.Spread.FpSpread SpdWrite = null, string mDirection = "")
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;

           
            try
            {

                SQL = "";
                //SQL = SQL + ComNum.VBLF + "SELECT A.EMRNO, A.EMRNOHIS, A.CHARTDATE, A.CHARTTIME, A.FORMNO, A.UPDATENO, A.SAVEGB     ";
                //SQL = SQL + ComNum.VBLF + "     , R.ITEMCD, R.ITEMINDEX, R.ITEMTYPE, R.ITEMVALUE, R.ITEMVALUE1, R.ITEMVALUE2        ";
                //SQL = SQL + ComNum.VBLF + "     --, B.PRINTTYPE                                                                       ";
                
                SQL = SQL + ComNum.VBLF + "SELECT A.EMRNO, A.EMRNOHIS, A.CHARTDATE, A.CHARTTIME                                     ";
                SQL = SQL + ComNum.VBLF + "     , R.ITEMCD, R.ITEMINDEX, R.ITEMTYPE, R.ITEMVALUE, R.ITEMVALUE1, R.ITEMVALUE2        ";

                if (frmXmlForm.Name.Equals("frmEmrChartNew") == false)
                {
                    SQL = SQL + ComNum.VBLF + " ,   CASE WHEN R.ITEMTYPE = 'IMAGE' THEN                                             ";
                    SQL = SQL + ComNum.VBLF + "     (	SELECT IMAGE                                                                ";
                    SQL = SQL + ComNum.VBLF + "     		  FROM KOSMOS_EMR.AEMRCHARTDRAW                                         ";
                    SQL = SQL + ComNum.VBLF + "       		 WHERE EMRNO      = A.EMRNO                                             ";
                    SQL = SQL + ComNum.VBLF + "      		   AND EMRNOHIS   = A.EMRNOHIS                                          ";
                    SQL = SQL + ComNum.VBLF + "      		   AND ITEMNAME   = R.ITEMCD                                            ";
                    SQL = SQL + ComNum.VBLF + "                AND DRAW IS NOT NULL                                                 ";
                    SQL = SQL + ComNum.VBLF + " 	)                                                                               ";
                    SQL = SQL + ComNum.VBLF + "     END IMAGE                                                                       ";
                }

                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AEMRCHARTMST A                                               ";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R                                       ";
                SQL = SQL + ComNum.VBLF + "       ON R.EMRNO     = A.EMRNO                                                          ";
                SQL = SQL + ComNum.VBLF + "      AND R.EMRNOHIS  = A.EMRNOHIS                                                       ";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRFORM B                                           ";
                SQL = SQL + ComNum.VBLF + "       ON A.FORMNO    = B.FORMNO                                                         ";
                SQL = SQL + ComNum.VBLF + "      AND A.UPDATENO  = B.UPDATENO                                                       ";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRFORM B                                           ";
                SQL = SQL + ComNum.VBLF + "       ON A.FORMNO    = B.FORMNO                                                         ";
                SQL = SQL + ComNum.VBLF + "      AND A.UPDATENO  = B.UPDATENO                                                       ";
                SQL = SQL + ComNum.VBLF + " WHERE A.EMRNO        = " + VB.Val(strEmrNo);

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }

                if (frmXmlForm.Name.Equals("frmEmrForm_Progress_New"))
                {
                    Control txt = frmXmlForm.Controls.Find("txtProgress", true).FirstOrDefault();
                    if (txt != null)
                    {
                        txt.Text = dt.Rows[0]["ITEMVALUE"].ToString().Trim() + dt.Rows[0]["ITEMVALUE1"].ToString().Trim() + dt.Rows[0]["ITEMVALUE2"].ToString().Trim();
                    }
                    return;
                }

                if (OldYn == true)
                {
                    if (dtp != null)
                    {
                        dtp.Value = DateTime.ParseExact(dt.Rows[0]["CHARTDATE"].ToString(), "yyyyMMdd", null);
                    }

                    if (cb != null)
                    {
                        cb.Text = ComFunc.FormatStrToDate(dt.Rows[0]["CHARTTIME"].ToString().Trim(), "M");
                    }
                    //dtp.Enabled = false;
                }

                //string strPRINTTYPE = dt.Rows[0]["PRINTTYPE"].ToString().Trim();
                //string strSAVEGB = dt.Rows[0]["SAVEGB"].ToString().Trim();
                string strEmrNoHis = dt.Rows[0]["EMRNOHIS"].ToString().Trim();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string strITEMCD     = dt.Rows[i]["ITEMCD"].ToString().Trim();

                    string strITEMTYPE   = dt.Rows[i]["ITEMTYPE"].ToString().Trim();
                    string strITEMVALUE  = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                    string strITEMVALUE1 = dt.Rows[i]["ITEMVALUE1"].ToString().Trim();
                    string strITEMVALUE2 = dt.Rows[i]["ITEMVALUE2"].ToString().Trim();

                    string strEmrNoOld = strEmrNo;

                    Control[] tx = null;
                    Control obj = null;

                    if (strITEMTYPE.Equals("DATE"))
                    {
                        if (strITEMCD.Trim() != "dtMedFrDate")
                        {
                            tx = frmXmlForm.Controls.Find(strITEMCD, true);
                            if (tx.Length > 0)
                            {
                                obj = (DateTimePicker)tx[0];
                                if (strITEMVALUE != "")
                                {
                                    ((DateTimePicker)obj).Value = Convert.ToDateTime(strITEMVALUE);
                                }
                            }
                        }
                    }
                    else if (strITEMTYPE.Equals("TEXT"))
                    {
                        tx = frmXmlForm.Controls.Find(strITEMCD, true);
                        if (tx.Length > 0)
                        {
                            obj = (TextBox)tx[0];
                            //string strText = strITEMVALUE.Trim(); (strITEMVALUE.Replace("\n", "\r\n")).Replace("]]", "");
                            //string strText1 = strITEMVALUE1.Trim(); (strITEMVALUE1.Replace("\n", "\r\n")).Replace("]]", "");
                            string strText = strITEMVALUE.Trim(); strITEMVALUE.Replace("]]", "");
                            string strText1 = strITEMVALUE1.Trim(); strITEMVALUE1.Replace("]]", "");
                            string strText2 = strITEMVALUE2.Trim(); strITEMVALUE2.Replace("]]", "");
                            if (((TextBox)obj).Multiline == false)
                            {
                                obj.Text = strText.Replace("\r\n", " ") + strText1.Replace("\r\n", " ") + strText2.Replace("\r\n", " ");
                            }
                            else
                            {
                                obj.Text = (strText + strText1 + strText2);
                            }
                        }
                    }
                    else if (strITEMTYPE.Equals("COMBO"))
                    {
                        if (strITEMCD.Trim() != "txtMedFrTime")
                        {
                            tx = frmXmlForm.Controls.Find(strITEMCD, true);
                            if (tx.Length > 0)
                            {
                                obj = (ComboBox)tx[0];
                                obj.Text = VB.Replace(strITEMVALUE, "", "", 1, -1);
                            }
                        }
                    }
                    else if (strITEMTYPE.Equals("CHECK"))
                    {
                        tx = frmXmlForm.Controls.Find(strITEMCD, true);
                        if (tx.Length > 0)
                        {
                            if (tx[0] is CheckBox)
                            {
                                obj = (CheckBox)tx[0];
                                ((CheckBox)obj).Checked = Convert.ToBoolean(VB.Val(strITEMVALUE));
                            }
                            else if (tx[0] is RadioButton)
                            {
                                obj = (RadioButton)tx[0];
                                ((RadioButton)obj).Checked = Convert.ToBoolean(VB.Val(strITEMVALUE));
                            }

                        }
                    }
                    else if (strITEMTYPE.Equals("RADIO"))
                    {
                        tx = frmXmlForm.Controls.Find(strITEMCD, true);
                        if (tx.Length > 0)
                        {
                            if (tx[0] is RadioButton)
                            {
                                obj = (RadioButton)tx[0];
                                ((RadioButton)obj).Checked = Convert.ToBoolean(VB.Val(strITEMVALUE));
                            }
                            else if (tx[0] is CheckBox)
                            {
                                obj = (CheckBox)tx[0];
                                ((CheckBox)obj).Checked = Convert.ToBoolean(VB.Val(strITEMVALUE));
                            }

                        }
                    }
                    else if (strITEMTYPE.Equals("IMAGE"))
                    {
                        if (frmXmlForm.Name.Equals("frmEmrChartNew"))
                            continue;

                        tx = frmXmlForm.Controls.Find(strITEMCD, true);
                        if (tx.Length > 0 && dt.Rows[i]["IMAGE"].ToString().NotEmpty())
                        {
                            obj = (PictureBox)tx[0];
                            ((PictureBox)obj).Tag = strITEMVALUE;

                            using (MemoryStream mem = new MemoryStream(Convert.FromBase64String(dt.Rows[i]["IMAGE"].ToString())))
                            {
                                ((PictureBox)obj).Image = Image.FromStream(mem);
                            }

                            //추가 이미지 패널 보이기...
                            //if (obj.Parent is mtsPanel15.mPanel)
                            //{
                            //    ImagePanelVisible(frmXmlForm, obj.Parent, true);
                            //}
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                //if (strPRINTTYPE.Equals("0"))
                //{
                //    if (strSAVEGB.Equals("1"))
                //    {
                //        //의사싸인
                //        //SetSignImage(pDbCon, frmXmlForm, strEmrNo, strFormNo);
                //    }
                //}
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }

        /// <summary>
        /// 의사 사인 이미지를 기록지에 붙인다
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="frmXmlForm"></param>
        /// <param name="strEmrNo"></param>
        /// <param name="strFormNo"></param>
        private static void SetSignImage(PsmhDb pDbCon, Control frmXmlForm, string strEmrNo, string strFormNo)
        {
            try
            {
                return;

                if (frmXmlForm == null)
                {
                    return;
                }

                // 싸인 예외처리 기록지 (CPR, 전동)
                if (strFormNo == "831" || strFormNo == "544")
                {
                    return;
                }

                string SQL = "";
                string SqlErr = ""; //에러문 받는 변수
                DataTable dt = null;
                DataTable dt1 = null;
                int j = 0;

                //이전 전과 기록지는 싸인을 의사이름으로 강제로 만들어준다..
                #region //strFormNo == "419"
                if (strFormNo == "419")
                {
                    string strCHARTUSEID1 = "";
                    //=>>기존 이미지 삭제
                    Control[] oldTrsPicFrom1 = null;
                    oldTrsPicFrom1 = frmXmlForm.Controls.Find("I0000029090_3", true);
                    if (oldTrsPicFrom1 != null)
                    {
                        if (oldTrsPicFrom1.Length > 0)
                        {
                            oldTrsPicFrom1[0] = null;
                        }
                    }

                    Control[] oldTrsPicTo1 = null;
                    oldTrsPicTo1 = frmXmlForm.Controls.Find("I0000029090_4", true);
                    if (oldTrsPicTo1 != null)
                    {
                        if (oldTrsPicTo1.Length > 0)
                        {
                            oldTrsPicTo1[0] = null;
                        }
                    }
                    //<<=기존 이미지 삭제

                    Control[] oldTrsTextFrom1 = null;
                    oldTrsTextFrom1 = frmXmlForm.Controls.Find("I0000029090_1", true);
                    if (oldTrsTextFrom1 != null)
                    {
                        if (oldTrsTextFrom1.Length > 0)
                        {
                            if (((TextBox)oldTrsTextFrom1[0]).Text.Trim() != "")
                            {
                                //==> 옆에 싸인 이미지를 표시한다.
                                Control[] mPanelFrom1 = null;
                                mPanelFrom1 = frmXmlForm.Controls.Find("mPanel3", true);
                                if (mPanelFrom1 != null)
                                {
                                    if (mPanelFrom1.Length > 0)
                                    {
                                        SQL = "";
                                        SQL = "SELECT USEID, USENAME FROM " + ComNum.DB_EMR + "AVIEWEMRUSER";
                                        SQL = SQL + ComNum.VBLF + "WHERE USENAME = '" + ((TextBox)oldTrsTextFrom1[0]).Text.Trim() + "' ";
                                        SQL = SQL + ComNum.VBLF + "    AND DRSORT IN ('0','1') ";
                                        SQL = SQL + ComNum.VBLF + "    AND WRITEDATE IS NOT NULL ";
                                        SQL = SQL + ComNum.VBLF + "ORDER BY DRSORT ASC";
                                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                                        if (SqlErr != "")
                                        {
                                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                            return;
                                        }
                                        if (dt.Rows.Count > 0)
                                        {
                                            strCHARTUSEID1 = dt.Rows[0]["USEID"].ToString().Trim();

                                            SQL = "";
                                            SQL = "SELECT USEID, FILESEQ, FEXTENSION, USESIGN FROM " + ComNum.DB_EMR + "AEMRUSERSIGN";
                                            SQL = SQL + ComNum.VBLF + "    WHERE USEID = '" + strCHARTUSEID1 + "' ";
                                            SQL = SQL + ComNum.VBLF + "    ORDER BY FILESEQ ASC";
                                            SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, pDbCon);

                                            if (SqlErr != "")
                                            {
                                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                                return;
                                            }
                                            if (dt1.Rows.Count > 0)
                                            {
                                                string strImage = "";

                                                for (j = 0; j < dt1.Rows.Count; j++)
                                                {
                                                    strImage = strImage + dt1.Rows[j]["USESIGN"].ToString().Trim();
                                                }

                                                byte[] b = Convert.FromBase64String(strImage);
                                                MemoryStream stream = new MemoryStream(b);
                                                Bitmap image1 = new Bitmap(stream);

                                                int intWidth = 100;
                                                int intHeight = 24;

                                                Bitmap newImage;
                                                if (image1.Width > image1.Height)
                                                {
                                                    intWidth = 100;
                                                    intHeight = 24;
                                                }
                                                newImage = new Bitmap(intWidth, intHeight, PixelFormat.Format24bppRgb);
                                                Graphics graphics_1 = Graphics.FromImage(newImage);
                                                graphics_1.CompositingQuality = CompositingQuality.HighQuality;
                                                graphics_1.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                                graphics_1.SmoothingMode = SmoothingMode.HighQuality;
                                                graphics_1.DrawImage(image1, 0, 0, intWidth, intHeight);

                                                System.Windows.Forms.PictureBox I0000029090_3 = new System.Windows.Forms.PictureBox();
                                                I0000029090_3.Name = "I0000029090_3";
                                                I0000029090_3.Size = new System.Drawing.Size(100, 24);
                                                I0000029090_3.Image = Image.FromHbitmap(newImage.GetHbitmap(), IntPtr.Zero);
                                                I0000029090_3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                                                I0000029090_3.TabIndex = 777;
                                                I0000029090_3.TabStop = false;
                                                I0000029090_3.Parent = ((mtsPanel15.mPanel)mPanelFrom1[0]);
                                                I0000029090_3.Location = new System.Drawing.Point(470, 83);
                                            }
                                            dt1.Dispose();
                                            dt1 = null;
                                        }
                                        dt.Dispose();
                                        dt = null;
                                    }
                                }
                                //<== 옆에 싸인 이미지를 표시한다.
                            }
                        }
                    }

                    Control[] oldTrsTextTo1 = null;
                    oldTrsTextTo1 = frmXmlForm.Controls.Find("I0000029090_2", true);
                    if (oldTrsTextTo1 != null)
                    {
                        if (oldTrsTextTo1.Length > 0)
                        {
                            if (((TextBox)oldTrsTextTo1[0]).Text.Trim() != "")
                            {
                                //==> 옆에 싸인 이미지를 표시한다.
                                Control[] mPanelTo1 = null;
                                mPanelTo1 = frmXmlForm.Controls.Find("mPanel7", true);
                                if (mPanelTo1 != null)
                                {
                                    if (mPanelTo1.Length > 0)
                                    {
                                        SQL = "";
                                        SQL = "SELECT USEID, USENAME FROM " + ComNum.DB_EMR + "AVIEWEMRUSER";
                                        SQL = SQL + ComNum.VBLF + "WHERE USENAME = '" + ((TextBox)oldTrsTextTo1[0]).Text.Trim() + "' ";
                                        SQL = SQL + ComNum.VBLF + "    AND DRSORT IN ('0','1')  ";
                                        SQL = SQL + ComNum.VBLF + "    AND WRITEDATE IS NOT NULL ";
                                        SQL = SQL + ComNum.VBLF + "ORDER BY DRSORT ASC";
                                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                                        if (SqlErr != "")
                                        {
                                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                            return;
                                        }
                                        if (dt.Rows.Count > 0)
                                        {
                                            strCHARTUSEID1 = dt.Rows[0]["USEID"].ToString().Trim();

                                            SQL = "";
                                            SQL = "SELECT USEID, FILESEQ, FEXTENSION, USESIGN FROM " + ComNum.DB_EMR + "AEMRUSERSIGN";
                                            SQL = SQL + ComNum.VBLF + "    WHERE USEID = '" + strCHARTUSEID1 + "' ";
                                            SQL = SQL + ComNum.VBLF + "    ORDER BY FILESEQ ASC";
                                            SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, pDbCon);

                                            if (SqlErr != "")
                                            {
                                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                                return;
                                            }
                                            if (dt1.Rows.Count > 0)
                                            {
                                                string strImage = "";

                                                for (j = 0; j < dt1.Rows.Count; j++)
                                                {
                                                    strImage = strImage + dt1.Rows[j]["USESIGN"].ToString().Trim();
                                                }

                                                byte[] b = Convert.FromBase64String(strImage);
                                                MemoryStream stream = new MemoryStream(b);
                                                Bitmap image1 = new Bitmap(stream);

                                                int intWidth = 100;
                                                int intHeight = 24;

                                                Bitmap newImage;
                                                if (image1.Width > image1.Height)
                                                {
                                                    intWidth = 100;
                                                    intHeight = 24;
                                                }
                                                newImage = new Bitmap(intWidth, intHeight, PixelFormat.Format24bppRgb);
                                                Graphics graphics_1 = Graphics.FromImage(newImage);
                                                graphics_1.CompositingQuality = CompositingQuality.HighQuality;
                                                graphics_1.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                                graphics_1.SmoothingMode = SmoothingMode.HighQuality;
                                                graphics_1.DrawImage(image1, 0, 0, intWidth, intHeight);

                                                System.Windows.Forms.PictureBox I0000029090_4 = new System.Windows.Forms.PictureBox();
                                                I0000029090_4.Name = "I0000029090_4";
                                                I0000029090_4.Size = new System.Drawing.Size(100, 24);
                                                I0000029090_4.Image = Image.FromHbitmap(newImage.GetHbitmap(), IntPtr.Zero);
                                                I0000029090_4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                                                I0000029090_4.TabIndex = 777;
                                                I0000029090_4.TabStop = false;
                                                I0000029090_4.Parent = ((mtsPanel15.mPanel)mPanelTo1[0]);
                                                I0000029090_4.Location = new System.Drawing.Point(470, 52);
                                            }
                                            dt1.Dispose();
                                            dt1 = null;
                                        }
                                        dt.Dispose();
                                        dt = null;
                                    }
                                }
                                //<== 옆에 싸인 이미지를 표시한다.
                            }
                        }
                    }
                    return;
                }
                #endregion //strFormNo == "419"
                //=================
                #region //strFormNo == "595"
                if (strFormNo == "595")
                {
                    string strCHARTUSEID2 = "";

                    //=>>기존 이미지 삭제
                    Control[] oldTrsPicFrom1 = null;
                    oldTrsPicFrom1 = frmXmlForm.Controls.Find("I0000029090_3", true);
                    if (oldTrsPicFrom1 != null)
                    {
                        if (oldTrsPicFrom1.Length > 0)
                        {
                            oldTrsPicFrom1[0] = null;
                        }
                    }

                    Control[] oldTrsPicTo1 = null;
                    oldTrsPicTo1 = frmXmlForm.Controls.Find("I0000029090_4", true);
                    if (oldTrsPicTo1 != null)
                    {
                        if (oldTrsPicTo1.Length > 0)
                        {
                            oldTrsPicTo1[0] = null;
                        }
                    }
                    //<<=기존 이미지 삭제

                    Control[] oldTrsTextFrom1 = null;
                    oldTrsTextFrom1 = frmXmlForm.Controls.Find("I0000030675", true);
                    if (oldTrsTextFrom1 != null)
                    {
                        if (oldTrsTextFrom1.Length > 0)
                        {
                            if (((TextBox)oldTrsTextFrom1[0]).Text.Trim() != "")
                            {
                                //==> 옆에 싸인 이미지를 표시한다.
                                Control[] mPanelFrom1 = null;
                                mPanelFrom1 = frmXmlForm.Controls.Find("mPanel14", true);
                                if (mPanelFrom1 != null)
                                {
                                    if (mPanelFrom1.Length > 0)
                                    {
                                        SQL = "";
                                        SQL = "SELECT USEID, USENAME FROM " + ComNum.DB_EMR + "AVIEWEMRUSER";
                                        SQL = SQL + ComNum.VBLF + "WHERE USENAME = '" + ((TextBox)oldTrsTextFrom1[0]).Text.Trim() + "' ";
                                        SQL = SQL + ComNum.VBLF + "    AND DRSORT IN ('0','1') ";
                                        SQL = SQL + ComNum.VBLF + "    AND WRITEDATE IS NOT NULL ";
                                        SQL = SQL + ComNum.VBLF + "ORDER BY DRSORT ASC";

                                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                                        if (SqlErr != "")
                                        {
                                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                            return;
                                        }
                                        if (dt.Rows.Count > 0)
                                        {
                                            strCHARTUSEID2 = dt.Rows[0]["USEID"].ToString().Trim();

                                            SQL = "";
                                            SQL = "SELECT USEID, FILESEQ, FEXTENSION, USESIGN FROM " + ComNum.DB_EMR + "AEMRUSERSIGN";
                                            SQL = SQL + ComNum.VBLF + "    WHERE USEID = '" + strCHARTUSEID2 + "' ";
                                            SQL = SQL + ComNum.VBLF + "    ORDER BY FILESEQ ASC";
                                            SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, pDbCon);

                                            if (SqlErr != "")
                                            {
                                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                                return;
                                            }
                                            if (dt1.Rows.Count > 0)
                                            {
                                                string strImage = "";

                                                for (j = 0; j < dt1.Rows.Count; j++)
                                                {
                                                    strImage = strImage + dt1.Rows[j]["USESIGN"].ToString().Trim();
                                                }

                                                byte[] b = Convert.FromBase64String(strImage);
                                                MemoryStream stream = new MemoryStream(b);
                                                Bitmap image1 = new Bitmap(stream);

                                                int intWidth = 100;
                                                int intHeight = 24;

                                                Bitmap newImage;
                                                if (image1.Width > image1.Height)
                                                {
                                                    intWidth = 100;
                                                    intHeight = 24;
                                                }
                                                newImage = new Bitmap(intWidth, intHeight, PixelFormat.Format24bppRgb);
                                                Graphics graphics_1 = Graphics.FromImage(newImage);
                                                graphics_1.CompositingQuality = CompositingQuality.HighQuality;
                                                graphics_1.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                                graphics_1.SmoothingMode = SmoothingMode.HighQuality;
                                                graphics_1.DrawImage(image1, 0, 0, intWidth, intHeight);

                                                System.Windows.Forms.PictureBox I0000029090_3 = new System.Windows.Forms.PictureBox();
                                                I0000029090_3.Name = "I0000029090_3";
                                                I0000029090_3.Size = new System.Drawing.Size(100, 24);
                                                I0000029090_3.Image = Image.FromHbitmap(newImage.GetHbitmap(), IntPtr.Zero);
                                                I0000029090_3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                                                I0000029090_3.TabIndex = 777;
                                                I0000029090_3.TabStop = false;
                                                I0000029090_3.Parent = ((mtsPanel15.mPanel)mPanelFrom1[0]);
                                                I0000029090_3.Location = new System.Drawing.Point(585, 6);
                                            }
                                            dt1.Dispose();
                                            dt1 = null;
                                        }
                                        dt.Dispose();
                                        dt = null;
                                    }
                                }
                                //<== 옆에 싸인 이미지를 표시한다.
                            }
                        }
                    }

                    Control[] oldTrsTextTo1 = null;
                    oldTrsTextTo1 = frmXmlForm.Controls.Find("I0000030675_1", true);
                    if (oldTrsTextTo1 != null)
                    {
                        if (oldTrsTextTo1.Length > 0)
                        {
                            if (((TextBox)oldTrsTextTo1[0]).Text.Trim() != "")
                            {
                                //==> 옆에 싸인 이미지를 표시한다.
                                Control[] mPanelTo1 = null;
                                mPanelTo1 = frmXmlForm.Controls.Find("mPanel1", true);
                                if (mPanelTo1 != null)
                                {
                                    if (mPanelTo1.Length > 0)
                                    {
                                        SQL = "";
                                        SQL = "SELECT USEID, USENAME FROM " + ComNum.DB_EMR + "AVIEWEMRUSER";
                                        SQL = SQL + ComNum.VBLF + "WHERE USENAME = '" + ((TextBox)oldTrsTextTo1[0]).Text.Trim() + "' ";
                                        SQL = SQL + ComNum.VBLF + "    AND DRSORT IN ('0','1') ";
                                        SQL = SQL + ComNum.VBLF + "    AND WRITEDATE IS NOT NULL ";
                                        SQL = SQL + ComNum.VBLF + "ORDER BY DRSORT ASC";
                                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                                        if (SqlErr != "")
                                        {
                                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                            return;
                                        }
                                        if (dt.Rows.Count > 0)
                                        {
                                            strCHARTUSEID2 = dt.Rows[0]["USEID"].ToString().Trim();

                                            SQL = "";
                                            SQL = "SELECT USEID, FILESEQ, FEXTENSION, USESIGN FROM " + ComNum.DB_EMR + "AEMRUSERSIGN";
                                            SQL = SQL + ComNum.VBLF + "    WHERE USEID = '" + strCHARTUSEID2 + "' ";
                                            SQL = SQL + ComNum.VBLF + "    ORDER BY FILESEQ ASC";
                                            SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, pDbCon);

                                            if (SqlErr != "")
                                            {
                                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                                return;
                                            }
                                            if (dt1.Rows.Count > 0)
                                            {
                                                string strImage = "";

                                                for (j = 0; j < dt1.Rows.Count; j++)
                                                {
                                                    strImage = strImage + dt1.Rows[j]["USESIGN"].ToString().Trim();
                                                }

                                                byte[] b = Convert.FromBase64String(strImage);
                                                MemoryStream stream = new MemoryStream(b);
                                                Bitmap image1 = new Bitmap(stream);

                                                int intWidth = 100;
                                                int intHeight = 24;

                                                Bitmap newImage;
                                                if (image1.Width > image1.Height)
                                                {
                                                    intWidth = 100;
                                                    intHeight = 24;
                                                }
                                                newImage = new Bitmap(intWidth, intHeight, PixelFormat.Format24bppRgb);
                                                Graphics graphics_1 = Graphics.FromImage(newImage);
                                                graphics_1.CompositingQuality = CompositingQuality.HighQuality;
                                                graphics_1.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                                graphics_1.SmoothingMode = SmoothingMode.HighQuality;
                                                graphics_1.DrawImage(image1, 0, 0, intWidth, intHeight);

                                                System.Windows.Forms.PictureBox I0000029090_4 = new System.Windows.Forms.PictureBox();
                                                I0000029090_4.Name = "I0000029090_4";
                                                I0000029090_4.Size = new System.Drawing.Size(100, 24);
                                                I0000029090_4.Image = Image.FromHbitmap(newImage.GetHbitmap(), IntPtr.Zero);
                                                I0000029090_4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                                                I0000029090_4.TabIndex = 777;
                                                I0000029090_4.TabStop = false;
                                                I0000029090_4.Parent = ((mtsPanel15.mPanel)mPanelTo1[0]);
                                                I0000029090_4.Location = new System.Drawing.Point(585, 6);
                                            }
                                            dt1.Dispose();
                                            dt1 = null;
                                        }
                                        dt.Dispose();
                                        dt = null;
                                    }
                                }
                                //<== 옆에 싸인 이미지를 표시한다.
                            }
                        }
                    }
                    return;
                }
                #endregion //strFormNo == "595"
                
                Control[] oldNameLabel = null;
                oldNameLabel = frmXmlForm.Controls.Find("ChartUseLabel", true);
                if (oldNameLabel != null)
                {
                    if (oldNameLabel.Length > 0)
                    {
                        ((Label)oldNameLabel[0]).Visible = false;
                    }
                }

                Control[] oldNameText = null;
                oldNameText = frmXmlForm.Controls.Find("I0000022529", true);
                if (oldNameText != null)
                {
                    if (oldNameText.Length > 0)
                    {
                        ((TextBox)oldNameText[0]).Visible = false;
                    }
                }

                Control[] oldNameText1 = null;
                oldNameText1 = frmXmlForm.Controls.Find("I0000030578", true);
                if (oldNameText1 != null)
                {
                    if (oldNameText1.Length > 0)
                    {
                        ((TextBox)oldNameText1[0]).Visible = false;
                    }
                }

                Control[] oldNameText2 = null;
                oldNameText2 = frmXmlForm.Controls.Find("I0000030612", true);
                if (oldNameText2 != null)
                {
                    if (oldNameText2.Length > 0)
                    {
                        ((TextBox)oldNameText2[0]).Visible = false;
                    }
                }

                Control[] oldNameText3 = null;
                oldNameText3 = frmXmlForm.Controls.Find("I0000000286", true);
                if (oldNameText3 != null)
                {
                    if (oldNameText3.Length > 0)
                    {
                        ((TextBox)oldNameText3[0]).Visible = false;
                    }
                }

                Control[] oldNameText4 = null;
                oldNameText4 = frmXmlForm.Controls.Find("I0000000293", true);
                if (oldNameText4 != null)
                {
                    if (oldNameText4.Length > 0)
                    {
                        ((TextBox)oldNameText4[0]).Visible = false;
                    }
                }

                if (strFormNo != "654") //회복실 제외한다
                {
                    Control[] oldConfLabel = null;
                    oldConfLabel = frmXmlForm.Controls.Find("ConfUseLabel", true);
                    if (oldConfLabel != null)
                    {
                        if (oldConfLabel.Length > 0)
                        {
                            ((Label)oldConfLabel[0]).Visible = false;
                        }
                    }
                }

                Control[] oldConfText1 = null;
                oldConfText1 = frmXmlForm.Controls.Find("I0000030613", true);
                if (oldConfText1 != null)
                {
                    if (oldConfText1.Length > 0)
                    {
                        ((TextBox)oldConfText1[0]).Visible = false;
                    }
                }

                if (strFormNo != "654") //회복실 제외한다
                {
                    Control[] oldConfText2 = null;
                    oldConfText2 = frmXmlForm.Controls.Find("I0000030917", true);
                    if (oldConfText2 != null)
                    {
                        if (oldConfText2.Length > 0)
                        {
                            ((TextBox)oldConfText2[0]).Visible = false;
                        }
                    }
                }
                //==> 기존 작성된 작성자, 확인자를 없앤다.

                Control[] signPenel = null;
                signPenel = frmXmlForm.Controls.Find("IG00249", true);
                if (signPenel != null)
                {
                    if (signPenel.Length > 0)
                    {
                        //없애고 다시 만들까나..??
                        return; //이미 사인패널이 있으면 나간다.
                    }
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT A.EMRNO,  A.CHARTUSEID, A.COMPUSEID, ";
                SQL = SQL + ComNum.VBLF + "        T.USENAME AS CHARTUSENAME, P.USENAME AS COMPUSENAME ";
                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
                SQL = SQL + ComNum.VBLF + "    LEFT OUTER JOIN " + ComNum.DB_EMR + "AVIEWEMRUSER T";
                SQL = SQL + ComNum.VBLF + "        ON A.CHARTUSEID = T.USEID";
                SQL = SQL + ComNum.VBLF + "    LEFT OUTER JOIN " + ComNum.DB_EMR + "AVIEWEMRUSER P";
                SQL = SQL + ComNum.VBLF + "        ON A.COMPUSEID = P.USEID";
                SQL = SQL + ComNum.VBLF + "    WHERE A.EMRNO = " + VB.Val(strEmrNo);
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }
                string strCHARTUSEID = dt.Rows[0]["CHARTUSEID"].ToString().Trim();
                string strCOMPUSEID = dt.Rows[0]["COMPUSEID"].ToString().Trim();
                string strCHARTUSENAME = dt.Rows[0]["CHARTUSENAME"].ToString().Trim();
                string strCOMPUSENAME = dt.Rows[0]["COMPUSENAME"].ToString().Trim();
                dt.Dispose();
                dt = null;

                Control cParent = null;
                Control[] tx = null;

                string strPanChart = "panChart";

                tx = frmXmlForm.Controls.Find(strPanChart, true);
                if (tx != null)
                {
                    if (tx.Length > 0)
                    {
                        if (tx[0] is mtsPanel15.mPanel) cParent = (mtsPanel15.mPanel)tx[0];
                        if (tx[0] is Panel) cParent = (Panel)tx[0];

                        mtsPanel15.mPanel IG00249 = new mtsPanel15.mPanel();
                        cParent.Controls.Add(IG00249);

                        IG00249.Dock = System.Windows.Forms.DockStyle.Top;
                        //IG00249.Location = new System.Drawing.Point(0, 2718);
                        IG00249.Name = "IG00249";
                        IG00249.Size = new System.Drawing.Size(669, 31);
                        IG00249.TabIndex = 777;
                        IG00249.TabStop = true;
                        IG00249.BringToFront();

                        // lblCHARTUSENAME
                        System.Windows.Forms.Label lblCHARTUSENAME = new System.Windows.Forms.Label();
                        lblCHARTUSENAME.AutoSize = true;
                        lblCHARTUSENAME.BackColor = System.Drawing.Color.Transparent;
                        lblCHARTUSENAME.Cursor = System.Windows.Forms.Cursors.Default;
                        lblCHARTUSENAME.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                        lblCHARTUSENAME.ForeColor = System.Drawing.SystemColors.ControlText;
                        lblCHARTUSENAME.Name = "lblCHARTUSENAME";
                        lblCHARTUSENAME.RightToLeft = System.Windows.Forms.RightToLeft.No;
                        lblCHARTUSENAME.Size = new System.Drawing.Size(49, 13);
                        lblCHARTUSENAME.TabIndex = 999;
                        lblCHARTUSENAME.Text = "작성자 :";
                        lblCHARTUSENAME.Parent = IG00249;
                        lblCHARTUSENAME.Location = new System.Drawing.Point(12, 9);

                        System.Windows.Forms.Label I0000031497_1 = new System.Windows.Forms.Label();
                        I0000031497_1.AutoSize = true;
                        I0000031497_1.BackColor = System.Drawing.Color.Transparent;
                        I0000031497_1.Cursor = System.Windows.Forms.Cursors.Default;
                        I0000031497_1.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                        I0000031497_1.ForeColor = System.Drawing.SystemColors.ControlText;
                        I0000031497_1.Name = "I0000031497_1";
                        I0000031497_1.RightToLeft = System.Windows.Forms.RightToLeft.No;
                        I0000031497_1.Size = new System.Drawing.Size(78, 13);
                        I0000031497_1.TabIndex = 999;
                        I0000031497_1.Text = strCHARTUSENAME;
                        I0000031497_1.Parent = IG00249;
                        I0000031497_1.Location = new System.Drawing.Point(70, 9);

                        //TODO AEMRUSERSIGN
                        //SQL = "";
                        //SQL = "SELECT USEID, FILESEQ, FEXTENSION, USESIGN FROM " + ComNum.DB_EMR + "AEMRUSERSIGN";
                        //SQL = SQL + ComNum.VBLF + "    WHERE USEID = '" + strCHARTUSEID + "' ";
                        //SQL = SQL + ComNum.VBLF + "    ORDER BY FILESEQ ASC";
                        //SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, pDbCon);

                        //if (SqlErr != "")
                        //{
                        //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        //    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        //}
                        //if (dt1.Rows.Count > 0)
                        //{
                        //    string strImage = "";

                        //    for (j = 0; j < dt1.Rows.Count; j++)
                        //    {
                        //        strImage = strImage + dt1.Rows[j]["USESIGN"].ToString().Trim();
                        //    }

                        //    byte[] b = Convert.FromBase64String(strImage);
                        //    MemoryStream stream = new MemoryStream(b);
                        //    Bitmap image1 = new Bitmap(stream);

                        //    int intWidth = 100;
                        //    int intHeight = 24;

                        //    Bitmap newImage;
                        //    if (image1.Width > image1.Height)
                        //    {
                        //        intWidth = 100;
                        //        intHeight = 24;
                        //    }
                        //    newImage = new Bitmap(intWidth, intHeight, PixelFormat.Format24bppRgb);
                        //    Graphics graphics_1 = Graphics.FromImage(newImage);
                        //    graphics_1.CompositingQuality = CompositingQuality.HighQuality;
                        //    graphics_1.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        //    graphics_1.SmoothingMode = SmoothingMode.HighQuality;
                        //    graphics_1.DrawImage(image1, 0, 0, intWidth, intHeight);

                        //    // I0000031497
                        //    System.Windows.Forms.PictureBox I0000031497_0 = new System.Windows.Forms.PictureBox();
                        //    I0000031497_0.Name = "I0000031497_0";
                        //    I0000031497_0.Size = new System.Drawing.Size(100, 24);
                        //    I0000031497_0.Image = Image.FromHbitmap(newImage.GetHbitmap(), IntPtr.Zero);
                        //    I0000031497_0.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                        //    I0000031497_0.TabIndex = 777;
                        //    I0000031497_0.TabStop = false;
                        //    I0000031497_0.Parent = IG00249;
                        //    I0000031497_0.Location = new System.Drawing.Point(150, 3);
                        //}
                        //dt1.Dispose();
                        //dt1 = null;

                        //if (strFormNo != "654") //회복실 제외한다
                        //{
                        //    if ((strCOMPUSEID != "") && (strCHARTUSEID != strCOMPUSEID))
                        //    {
                        //        // lblCOMPUSENAME
                        //        System.Windows.Forms.Label lblCOMPUSENAME = new System.Windows.Forms.Label();
                        //        lblCOMPUSENAME.AutoSize = true;
                        //        lblCOMPUSENAME.BackColor = System.Drawing.Color.Transparent;
                        //        lblCOMPUSENAME.Cursor = System.Windows.Forms.Cursors.Default;
                        //        lblCOMPUSENAME.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                        //        lblCOMPUSENAME.ForeColor = System.Drawing.SystemColors.ControlText;
                        //        lblCOMPUSENAME.Name = "lblCOMPUSENAME";
                        //        lblCOMPUSENAME.RightToLeft = System.Windows.Forms.RightToLeft.No;
                        //        lblCOMPUSENAME.Size = new System.Drawing.Size(49, 13);
                        //        lblCOMPUSENAME.TabIndex = 242;
                        //        lblCOMPUSENAME.Text = "확인자 :";
                        //        lblCOMPUSENAME.Parent = IG00249;
                        //        lblCOMPUSENAME.Location = new System.Drawing.Point(393, 9);

                        //        System.Windows.Forms.Label I0000031498_1 = new System.Windows.Forms.Label();
                        //        I0000031498_1.AutoSize = true;
                        //        I0000031498_1.BackColor = System.Drawing.Color.Transparent;
                        //        I0000031498_1.Cursor = System.Windows.Forms.Cursors.Default;
                        //        I0000031498_1.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                        //        I0000031498_1.ForeColor = System.Drawing.SystemColors.ControlText;
                        //        I0000031498_1.Name = "I0000031497_1";
                        //        I0000031498_1.RightToLeft = System.Windows.Forms.RightToLeft.No;
                        //        I0000031498_1.Size = new System.Drawing.Size(78, 13);
                        //        I0000031498_1.TabIndex = 999;
                        //        I0000031498_1.Text = strCOMPUSENAME;
                        //        I0000031498_1.Parent = IG00249;
                        //        I0000031498_1.Location = new System.Drawing.Point(453, 9);

                        //        SQL = "";
                        //        SQL = "SELECT USEID, FILESEQ, FEXTENSION, USESIGN FROM " + ComNum.DB_EMR + "AEMRUSERSIGN";
                        //        SQL = SQL + ComNum.VBLF + "    WHERE USEID = '" + strCOMPUSEID + "' ";
                        //        SQL = SQL + ComNum.VBLF + "    ORDER BY FILESEQ ASC";
                        //        SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, pDbCon);

                        //        if (SqlErr != "")
                        //        {
                        //            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        //            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        //        }
                        //        if (dt1.Rows.Count > 0)
                        //        {
                        //            string strImage = "";

                        //            for (j = 0; j < dt1.Rows.Count; j++)
                        //            {
                        //                strImage = strImage + dt1.Rows[j]["USESIGN"].ToString().Trim();
                        //            }

                        //            byte[] b = Convert.FromBase64String(strImage);
                        //            MemoryStream stream = new MemoryStream(b);
                        //            Bitmap image1 = new Bitmap(stream);

                        //            int intWidth = 100;
                        //            int intHeight = 24;

                        //            Bitmap newImage;
                        //            if (image1.Width > image1.Height)
                        //            {
                        //                intWidth = 100;
                        //                intHeight = 24;
                        //            }
                        //            newImage = new Bitmap(intWidth, intHeight, PixelFormat.Format24bppRgb);
                        //            Graphics graphics_1 = Graphics.FromImage(newImage);
                        //            graphics_1.CompositingQuality = CompositingQuality.HighQuality;
                        //            graphics_1.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        //            graphics_1.SmoothingMode = SmoothingMode.HighQuality;
                        //            graphics_1.DrawImage(image1, 0, 0, intWidth, intHeight);

                        //            // I0000031497
                        //            System.Windows.Forms.PictureBox I0000031498_0 = new System.Windows.Forms.PictureBox();
                        //            I0000031498_0.Name = "I0000031498_0";
                        //            I0000031498_0.Size = new System.Drawing.Size(100, 24);
                        //            I0000031498_0.Image = Image.FromHbitmap(newImage.GetHbitmap(), IntPtr.Zero);
                        //            I0000031498_0.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                        //            I0000031498_0.TabIndex = 999;
                        //            I0000031498_0.TabStop = false;
                        //            I0000031498_0.Parent = IG00249;
                        //            I0000031498_0.Location = new System.Drawing.Point(533, 3);
                        //        }
                        //        dt1.Dispose();
                        //        dt1 = null;
                        //    }
                        //}

                    }
                }
                else
                {
                    return;
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// 의사 싸인 이미지 패널
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="objPanel"></param>
        /// <param name="blnVisible"></param>
        private static void ImagePanelVisible(Control frm, Control objPanel, bool blnVisible)
        {
            if (VB.Left(objPanel.Name, 7) != "IG00170") return;
            objPanel.Visible = true;
        }
        
        /// <summary>
        /// 이전 차트 내역을 불러와서 보여준다.
        /// </summary>
        /// <param name="frmXmlForm"></param>
        /// <param name="strEmrNo"></param>
        public static void LoadDataChartRowHis(PsmhDb pDbCon, Control frmXmlForm, string strEmrNo, string strOldGb)
        {
            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            //try
            //{
            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT A.EMRNO, A.CHARTDATE, A.CHARTTIME, A.FORMNO, A.UPDATENO, ";
            SQL = SQL + ComNum.VBLF + "        R.ITEMCD, R.ITEMINDEX, R.ITEMTYPE, R.ITEMVALUE, R.ITEMVALUE1, R.ITEMVALUE2";
            SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
            SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R";
            SQL = SQL + ComNum.VBLF + "        ON R.EMRNO = A.EMRNO";
            SQL = SQL + ComNum.VBLF + "    WHERE A.EMRNO = " + VB.Val(strEmrNo);

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                try
                {
                    string strITEMCD = dt.Rows[i]["ITEMCD"].ToString().Trim();

                    string strITEMTYPE = dt.Rows[i]["ITEMTYPE"].ToString().Trim();
                    string strITEMVALUE = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                    string strITEMVALUE1 = dt.Rows[i]["ITEMVALUE1"].ToString().Trim();
                    string strITEMVALUE2 = dt.Rows[i]["ITEMVALUE2"].ToString().Trim();

                    string strEmrNoOld = strEmrNo;

                    Control[] tx = null;
                    Control obj = null;

                    if (strITEMTYPE == "DATE")
                    {
                        if (strITEMCD.Trim() != "dtMedFrDate")
                        {
                            tx = frmXmlForm.Controls.Find(strITEMCD, true);
                            if (tx.Length > 0)
                            {
                                obj = (DateTimePicker)tx[0];
                                if (strITEMVALUE != "")
                                {
                                    ((DateTimePicker)obj).Value = Convert.ToDateTime(strITEMVALUE);
                                }
                            }
                        }
                    }
                    else if (strITEMTYPE == "TEXT")
                    {
                        tx = frmXmlForm.Controls.Find(strITEMCD, true);
                        if (tx.Length > 0)
                        {
                            obj = (TextBox)tx[0];
                            string strText = strITEMVALUE.Trim(); strITEMVALUE.Replace("]]", "");
                            string strText1 = strITEMVALUE1.Trim(); strITEMVALUE1.Replace("]]", "");
                            string strText2 = strITEMVALUE2.Trim(); strITEMVALUE2.Replace("]]", "");
                            if (((TextBox)obj).Multiline == false)
                            {
                                obj.Text = strText.Replace("\r\n", " ") + strText1.Replace("\r\n", " ") + strText2.Replace("\r\n", " ");
                            }
                            else
                            {
                                obj.Text = strText + strText1 + strText2;
                            }
                        }
                    }
                    else if (strITEMTYPE == "COMBO")
                    {
                        if (strITEMCD.Trim() != "txtMedFrTime")
                        {
                            tx = frmXmlForm.Controls.Find(strITEMCD, true);
                            if (tx.Length > 0)
                            {
                                obj = (ComboBox)tx[0];
                                obj.Text = VB.Replace(strITEMVALUE, "", "", 1, -1);
                            }
                        }
                    }
                    else if (strITEMTYPE == "CHECK")
                    {
                        tx = frmXmlForm.Controls.Find(strITEMCD, true);
                        if (tx.Length > 0)
                        {
                            obj = (CheckBox)tx[0];
                            ((CheckBox)obj).Checked = Convert.ToBoolean(VB.Val(strITEMVALUE));
                        }
                    }
                    else if (strITEMTYPE == "RADIO")
                    {
                        tx = frmXmlForm.Controls.Find(strITEMCD, true);
                        if (tx.Length > 0)
                        {
                            obj = (RadioButton)tx[0];
                            ((RadioButton)obj).Checked = Convert.ToBoolean(VB.Val(strITEMVALUE));
                        }
                    }
                    else if (strITEMTYPE == "IMAGE")
                    {
                        
                    }
                }
                catch { }
            }

            dt.Dispose();
            dt = null;
            //}
            //catch (Exception ex)
            //{
            //    ComFunc.MsgBox(ex.Message);
            //}
        }

        /// <summary>
        /// EMR DATA를 삭제한다
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strEmrNo"></param>
        /// <param name="strUseId"></param>
        /// <returns></returns>
        public static bool gDeleteEmrXml(PsmhDb pDbCon, string strEmrNo, string strUseId)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(pDbCon);
            
            try
            {
                SQL = "SELECT ";
                SQL = SQL + ComNum.VBLF + "                A.EMRNO, A.CHARTUSEID, A.MEDFRDATE, A.PTNO, A.FORMNO";
                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
                SQL = SQL + ComNum.VBLF + "    WHERE A.EMRNO = " + strEmrNo;

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("삭제할 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                string strUseId1 = (dt.Rows[0]["CHARTUSEID"].ToString() + "").Trim();
                string MEDFRDATE = (dt.Rows[0]["MEDFRDATE"].ToString() + "").Trim();
                string PTNO = (dt.Rows[0]["PTNO"].ToString() + "").Trim();
                string FORMNO = (dt.Rows[0]["FORMNO"].ToString() + "").Trim();


                dt.Dispose();
                dt = null;

                //if (strUseId1 != strUseId)
                //{
                //    clsDB.setRollbackTran(pDbCon);
                //    ComFunc.MsgBox("기존 작성자와 다릅니다." + ComNum.VBLF + "삭제할 수 없습니다.");
                //    Cursor.Current = Cursors.Default;
                //    return false;
                //}
                //else
                //{
                    if (ComFunc.MsgBoxQ("기존내용을 삭제하시겠습니까?", "EMR 삭제", MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        clsDB.setRollbackTran(pDbCon);
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                //}

                #region /CheckMapping
                switch (CheckMapping(pDbCon, strEmrNo, ref SQL))
                {
                    case "0":
                        break;

                    case "1":
                        clsDB.setRollbackTran(pDbCon);
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("매칭된 기록지가 있습니다. 먼저 삭제 하시고 삭제해 주세요");
                        return false;

                    case "2":
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return false;
                        }
                        break;
                }
                #endregion /CheckMapping

                #region 입퇴원요약지 검수 관련 데이터 정리
                //if (FORMNO.Equals("1647"))
                //{
                //    SQL = "INSERT INTO KOSMOS_EMR.EMRXML_COMPLETE_HISTORY(";
                //    SQL += ComNum.VBLF + " EMRNO, CDATE, CSABUN, DELDATE, DELSABUN, MEDFRDATE, PTNO, INDATE) ";
                //    SQL += ComNum.VBLF + " SELECT EMRNO, CDATE, CSABUN, SYSDATE, " + clsType.User.IdNumber + ", MEDFRDATE, PTNO, INDATE";
                //    SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMRXML_COMPLETE ";
                //    SQL += ComNum.VBLF + "  WHERE EMRNO = " + VB.Val(strEmrNo);

                //    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                //    if (SqlErr.Length > 0)
                //    {
                //        clsDB.setRollbackTran(pDbCon);
                //        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                //        return false;
                //    }

                //    SQL = " DELETE KOSMOS_EMR.EMRXML_COMPLETE ";
                //    SQL += ComNum.VBLF + "  WHERE PTNO = '" + PTNO + "'";
                //    SQL += ComNum.VBLF + "    AND MEDFRDATE = '" + MEDFRDATE + "'";

                //    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                //    if (SqlErr.Length > 0)
                //    {
                //        clsDB.setRollbackTran(pDbCon);
                //        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                //        return false;
                //    }
                //}
                #endregion

                string strCurDateTime = ComQuery.CurrentDateTime(pDbCon, "A");
                string strCurDate = VB.Left(strCurDateTime, 8);
                string strCurTime = VB.Right(strCurDateTime, 6);
                //AEMRCHARTMST
                //double dblEmrHisNo = ComQuery.GetSequencesNo(pDbCon, "" + ComNum.DB_EMR + "GETSEQ_AEMRCHARTMSTHIS");
                double dblEmrHisNo = -1;

                //AEMRCHARTMST
                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTMSTHIS ";
                SQL = SQL + ComNum.VBLF + "    (EMRNO , EMRNOHIS, ACPNO, FORMNO, UPDATENO, CHARTDATE   ,";
                SQL = SQL + ComNum.VBLF + "    CHARTTIME   ,CHARTUSEID  ,WRITEDATE   ,WRITETIME   ,COMPUSEID   ,";
                SQL = SQL + ComNum.VBLF + "    COMPDATE    ,COMPTIME    ,PRNTYN      ,SAVEGB      ,SAVECERT    ,FORMGB,";
                SQL = SQL + ComNum.VBLF + "    PTNO, INOUTCLS, OP_DEPT, DEPTCDNOW, DRCDNOW, ROOM_NO, ";
                SQL = SQL + ComNum.VBLF + "    DCEMRNOHIS, ";
                SQL = SQL + ComNum.VBLF + "    DCUSEID, DCDATE, DCTIME , CURDEPT , CURGRADE, CODEUSEID, CODEDATE, CODETIME, CERTNO)";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "    EMRNO , EMRNOHIS, ACPNO  ,FORMNO      ,UPDATENO    ,CHARTDATE   ,";
                SQL = SQL + ComNum.VBLF + "    CHARTTIME   ,CHARTUSEID  ,WRITEDATE   ,WRITETIME   ,COMPUSEID   ,";
                SQL = SQL + ComNum.VBLF + "    COMPDATE    ,COMPTIME    ,PRNTYN      ,SAVEGB      ,SAVECERT    ,FORMGB ,";
                SQL = SQL + ComNum.VBLF + "    PTNO, INOUTCLS, OP_DEPT, DEPTCDNOW, DRCDNOW, ROOM_NO, ";
                SQL = SQL + ComNum.VBLF + dblEmrHisNo + " AS DCEMRNOHIS, ";
                SQL = SQL + ComNum.VBLF + "    '" + clsType.User.IdNumber + "',";   //DCUSEID
                SQL = SQL + ComNum.VBLF + "    '" + strCurDate + "',";   //DCDATE
                SQL = SQL + ComNum.VBLF + "    '" + strCurTime + "', CURDEPT ,  CURGRADE, CODEUSEID, CODEDATE, CODETIME, CERTNO ";   //DCTIME
                SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_EMR + "AEMRCHARTMST";
                SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + strEmrNo;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + "DELETE FROM  " + ComNum.DB_EMR + "AEMRCHARTMST";
                SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + strEmrNo;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                //AEMRCHARTROW
                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTROWHIS ";
                SQL = SQL + ComNum.VBLF + "    (EMRNO, EMRNOHIS, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1, INPUTGB)";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "    EMRNO, EMRNOHIS, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1, INPUTGB";
                SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_EMR + "AEMRCHARTROW";
                SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + strEmrNo;
                //SQL = SQL + ComNum.VBLF + "   AND EMRNOHIS = " + dblEmrHisNoOld;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                SQL = "";
                SQL = SQL + ComNum.VBLF + "DELETE FROM  " + ComNum.DB_EMR + "AEMRCHARTROW";
                SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + strEmrNo;
                //SQL = SQL + ComNum.VBLF + "   AND EMRNOHIS = " + dblEmrHisNoOld;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                //AEMRCHARTFORM
                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTFORMHIS ";
                SQL = SQL + ComNum.VBLF + "    (";
                SQL = SQL + ComNum.VBLF + "    EMRNO,EMRNOHIS,CONTROLNAME,CONTROTYPE,CONTROLPARENT,LOCATIONX,LOCATIONY,";
                SQL = SQL + ComNum.VBLF + "    SIZEWIDTH,SIZEHEIGHT,TAG,CHILDINDEX,BACKCOLOR,FORECOLOR,BOARDSTYLE,";
                SQL = SQL + ComNum.VBLF + "    DOCK,ENABLED,VISIBLED,TEXT,FONTS,MULTILINE,SCROLLBARS,TEXTALIGN,";
                SQL = SQL + ComNum.VBLF + "    IMAGE,IMAGESIZEMODE,CHECKED";
                SQL = SQL + ComNum.VBLF + "    )";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "    EMRNO,EMRNOHIS,CONTROLNAME,CONTROTYPE,CONTROLPARENT,LOCATIONX,LOCATIONY,";
                SQL = SQL + ComNum.VBLF + "    SIZEWIDTH,SIZEHEIGHT,TAG,CHILDINDEX,BACKCOLOR,FORECOLOR,BOARDSTYLE,";
                SQL = SQL + ComNum.VBLF + "    DOCK,ENABLED,VISIBLED,TEXT,FONTS,MULTILINE,SCROLLBARS,TEXTALIGN,";
                SQL = SQL + ComNum.VBLF + "    IMAGE,IMAGESIZEMODE,CHECKED";
                SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_EMR + "AEMRCHARTFORM";
                SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + strEmrNo;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                SQL = "";
                SQL = SQL + ComNum.VBLF + "DELETE FROM  " + ComNum.DB_EMR + "AEMRCHARTFORM";
                SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + strEmrNo;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                //AEMRCHARTIMAGE
                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTIMAGEHIS ";
                SQL = SQL + ComNum.VBLF + "    (EMRNO, EMRNOHIS, ITEMCD, IMAGENO, IMGSVR, IMGPATH, IMGEXTENSION)";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "    EMRNO, EMRNOHIS, ITEMCD, IMAGENO, IMGSVR, IMGPATH, IMGEXTENSION";
                SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_EMR + "AEMRCHARTIMAGE";
                SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + strEmrNo;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                SQL = "";
                SQL = SQL + ComNum.VBLF + "DELETE FROM  " + ComNum.DB_EMR + "AEMRCHARTIMAGE";
                SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + strEmrNo;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                string strMiBiCd = string.Empty;
                string strGrp = clsEmrChart.GetEmrGrp(clsDB.DbCon, FORMNO, ref strMiBiCd);


                if (FORMNO == "1647" || strGrp == "C" || strGrp == "A")
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_EMR + "EMRMIBI";
                    SQL = SQL + ComNum.VBLF + "SET";
                    SQL = SQL + ComNum.VBLF + " WRITEDATE = '" + strCurDate + "',";
                    SQL = SQL + ComNum.VBLF + " WRITETIME = '" + strCurTime + "'";
                    SQL = SQL + ComNum.VBLF + "  WHERE PTNO = '" + PTNO + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND MEDFRDATE = '" + MEDFRDATE + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND MEDDRCD = '" + clsType.User.IdNumber + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND MIBICLS = 1";
                    SQL = SQL + ComNum.VBLF + "  AND MIBIGRP = '" + strGrp + "'";
                    //입퇴원(1647) = A13 삭제, 입원기록지(C10)
                    SQL = SQL + ComNum.VBLF + "  AND MIBICD = '" + (FORMNO.Equals("1647") ? "A13" : "C10") + "'";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }

                clsDB.setCommitTran(pDbCon);
                ComFunc.MsgBox("삭제하였습니다.");
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        public static bool gDeleteEmrXmlOld(PsmhDb pDbCon, string strEmrNo, string strUseId, string strFormNo)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strChartUseId = string.Empty;
            string strPtno = string.Empty;
            string strMedFrDate = string.Empty;
            double dblEmrNo = VB.Val(strEmrNo);

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "SELECT ";
                SQL = SQL + ComNum.VBLF + "        A.PTNO, A.MEDFRDATE, A.EMRNO, A.USEID";
                SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_EMR.EMRXML A";
                SQL = SQL + ComNum.VBLF + "    WHERE A.EMRNO = " + dblEmrNo;

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                strPtno = dt.Rows[0]["PTNO"].ToString().Trim();
                strMedFrDate = dt.Rows[0]["MEDFRDATE"].ToString().Trim();
                strChartUseId = dt.Rows[0]["USEID"].ToString().Trim();
                dt.Dispose();
                dt = null;

                //if (clsType.User.IdNumber.Equals("52517") && strChartUseId.Equals("21225"))
                //{
                //}
                //else
                //{
                //    if (clsType.User.IdNumber != strChartUseId)
                //    {
                //        clsDB.setRollbackTran(clsDB.DbCon);
                //        ComFunc.MsgBox("기존 작성자와 다릅니다." + ComNum.VBLF + "삭제할 수 없습니다.");
                //        Cursor.Current = Cursors.Default;
                //        return false;
                //    }
                //}
                
                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
                strCurDateTime = strCurDateTime.Replace("-", "").Replace(":", "");

                #region 입퇴원요약지 검수 관련 데이터 정리
                if (strFormNo.Equals("1647"))
                {
                    SQL = "INSERT INTO KOSMOS_EMR.EMRXML_COMPLETE_HISTORY(";
                    SQL += ComNum.VBLF + " EMRNO, CDATE, CSABUN, DELDATE, DELSABUN, MEDFRDATE, PTNO, INDATE) ";
                    SQL += ComNum.VBLF + " SELECT EMRNO, CDATE, CSABUN, SYSDATE, " + clsType.User.IdNumber + ", MEDFRDATE, PTNO, INDATE";
                    SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMRXML_COMPLETE ";
                    SQL += ComNum.VBLF + "  WHERE EMRNO = " + dblEmrNo;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr.Length > 0)
                    {
                        clsDB.setRollbackTran(pDbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        return false;
                    }

                    SQL = " DELETE KOSMOS_EMR.EMRXML_COMPLETE ";
                    SQL += ComNum.VBLF + "  WHERE PTNO = '" + strPtno + "'";
                    SQL += ComNum.VBLF + "    AND MEDFRDATE = '" + strMedFrDate + "'";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr.Length > 0)
                    {
                        clsDB.setRollbackTran(pDbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        return false;
                    }
                }
                #endregion

                double dblEmrHisNo = ComQuery.GetSequencesNo(clsDB.DbCon, "KOSMOS_EMR.EMRXMLHISNO");
                               
                SQL = "";
                SQL = SQL + ComNum.VBLF + " INSERT INTO KOSMOS_EMR.EMRXMLHISTORY";
                SQL = SQL + ComNum.VBLF + "      (HISTORYNO, EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,UPDATENO,HISTORYWRITEDATE,HISTORYWRITETIME,DELUSEID,CERTNO)";
                SQL = SQL + ComNum.VBLF + " SELECT  " + dblEmrHisNo + ",";
                SQL = SQL + ComNum.VBLF + "      EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,UPDATENO,";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Left(strCurDateTime, 8) + "',";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Right(strCurDateTime, 6) + "', '" + clsType.User.IdNumber + "',CERTNO";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMRXML";
                SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + dblEmrNo;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + " DELETE FROM KOSMOS_EMR.EMRXML";
                SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + dblEmrNo;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + " DELETE FROM KOSMOS_EMR.EMRXMLMST";
                SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + dblEmrNo;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                string strMiBiCd = string.Empty;
                string strGrp = clsEmrChart.GetEmrGrp(clsDB.DbCon, strFormNo, ref strMiBiCd);


                if (strFormNo == "1647" || strGrp == "C" || strGrp == "A")
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_EMR + "EMRMIBI";
                    SQL = SQL + ComNum.VBLF + "SET";
                    SQL = SQL + ComNum.VBLF + " WRITEDATE = '" + VB.Left(strCurDateTime, 8) + "',";
                    SQL = SQL + ComNum.VBLF + " WRITETIME = '" + VB.Right(strCurDateTime, 6) + "'";
                    SQL = SQL + ComNum.VBLF + "  WHERE PTNO = '" + strPtno + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND MEDFRDATE = '" + strMedFrDate + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND MEDDRCD = '" + clsType.User.IdNumber + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND MIBICLS = 1";
                    SQL = SQL + ComNum.VBLF + "  AND MIBIGRP = '" + strGrp + "'";
                    //입퇴원(1647) = A13 삭제, 입원기록지(C10)
                    SQL = SQL + ComNum.VBLF + "  AND MIBICD = '" + (strFormNo.Equals("1647") ? "A13" : "C10") + "'";                    

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                Log.Error(ex.ToString());
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        /// <summary>
        /// 기존 저장을 한 사용자인지 확인
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strEmrNo"></param>
        /// <param name="strUseId"></param>
        /// <returns></returns>
        public static bool gDeleteEmrXmlNotAuth(PsmhDb pDbCon, string strEmrNo, string strUseId)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수
            //int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(pDbCon);
            
            try
            {
                SQL = "SELECT ";
                SQL = SQL + ComNum.VBLF + "                A.EMRNO, A.CHARTUSEID";
                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
                SQL = SQL + ComNum.VBLF + "    WHERE A.EMRNO = " + strEmrNo;

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("삭제할 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                string strUseId1 = (dt.Rows[0]["CHARTUSEID"].ToString() + "").Trim();

                dt.Dispose();
                dt = null;
                
                string strCurDateTime = ComQuery.CurrentDateTime(pDbCon, "A");
                string strCurDate = VB.Left(strCurDateTime,8);
                string strCurTime = VB.Right(strCurDateTime, 6);
                double dblEmrHisNo = -1; //삭제할 경우

                #region //과거기록 백업
                SqlErr = clsEmrQuery.SaveChartMastHis(pDbCon, strEmrNo, dblEmrHisNo, strCurDate, strCurTime, "C", "", clsType.User.IdNumber);
                if (SqlErr != "OK")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                #endregion

                clsDB.setCommitTran(pDbCon);
                ComFunc.MsgBox("삭제하였습니다.");
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        /// <summary>
        /// text Type => Spread type
        /// </summary>
        /// <param name="strType"></param>
        /// <returns></returns>
        public static string GetType(string strType)
        {
            string rtnVal = "";

            switch (strType)
            {
                case "CHECK":
                    rtnVal = "CheckBoxCellType";
                    break;
                case "COMBO":
                    rtnVal = "ComboBoxCellType";
                    break;
                default:
                    rtnVal = "TypeText";
                    break;
            }

            return rtnVal;
        }

        public static string GetTypeEx(string strType)
        {
            string rtnVal = "";

            switch (strType)
            {
                case "CheckBoxCellType":
                case "CHECK":
                    rtnVal = "CheckBoxCellType";
                    break;
                case "ComboBoxCellType":
                    rtnVal = "ComboBoxCellType";
                    break;
                default: 
                    rtnVal = "TextCellType";
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// 기록지가 삭제 될때 매핑 되는 서식지의 매핑 정보를 같이 삭제 한다.
        /// </summary>
        /// <param name="strEmrNo">리턴값이 = 0 : 없거나 실패 , 1 : 매칭된 기록지가 있어 보내는 기록지는 삭제 불가, 2 : 성공</param>
        /// <param name="SQL"></param>
        /// <returns></returns>
        private static string CheckMapping(PsmhDb pDbCon, string strEmrNo, ref string SQL)
        {
            //string strReturn = "0";
            //string SqlErr = ""; //에러문 받는 변수
            ////int intRowAffected = 0; //변경된 Row 받는 변수
            //DataTable dt = null;
            return "0";

            //TODO CheckMapping
            //SQL = "";
            //SQL = "SELECT *      ";
            //SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRMAPPING      ";
            //SQL = SQL + ComNum.VBLF + "WHERE (FROMEMRNO = " + strEmrNo + " OR TOEMRNO = " + strEmrNo + ")     ";

            //SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            //if (SqlErr != "")
            //{
            //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
            //    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
            //    return strReturn;
            //}
            //if (dt.Rows.Count == 0)
            //{
            //    dt.Dispose();
            //    dt = null;
            //    return strReturn;
            //}

            //if (dt.Rows[0]["FROMEMRNO"].ToString().Trim() == strEmrNo.Trim())
            //{
            //    if (dt.Rows[0]["TOEMRNO"].ToString().Trim() == "" || dt.Rows[0]["TOEMRNO"].ToString().Trim() == "0")
            //    {
            //        strReturn = "2";

            //        SQL = "";
            //        SQL = "DELETE " + ComNum.DB_EMR + "AEMRMAPPING      ";
            //        SQL = SQL + ComNum.VBLF + "WHERE ACPNO = '" + dt.Rows[0]["ACPNO"].ToString().Trim() + "'      ";
            //        SQL = SQL + ComNum.VBLF + "AND FROMEMRNO = '" + strEmrNo + "'      ";
            //        SQL = SQL + ComNum.VBLF + "AND FORMGB = '" + dt.Rows[0]["FORMGB"].ToString().Trim() + "'      ";
            //    }
            //    else
            //    {
            //        strReturn = "1";
            //    }
            //}
            //else if (dt.Rows[0]["TOEMRNO"].ToString().Trim() == strEmrNo.Trim())
            //{
            //    strReturn = "2";

            //    SQL = "";
            //    SQL = "UPDATE " + ComNum.DB_EMR + "AEMRMAPPING      ";
            //    SQL = SQL + ComNum.VBLF + "SET TOEMRNO = ''        ";
            //    SQL = SQL + ComNum.VBLF + "WHERE ACPNO = '" + dt.Rows[0]["ACPNO"].ToString().Trim() + "'      ";
            //    SQL = SQL + ComNum.VBLF + "AND TOEMRNO = '" + strEmrNo + "'      ";
            //    SQL = SQL + ComNum.VBLF + "AND FORMGB = '" + dt.Rows[0]["FORMGB"].ToString().Trim() + "'      ";
            //}

            //dt.Dispose();
            //dt = null;


            //return strReturn;
        }

        /// <summary>
        /// 사용자 템플릿을 불러와 폼에 표시한다
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="frmXmlForm"></param>
        /// <param name="strMACRONO"></param>
        /// <param name="blnErrOption"></param>
        public static void LoadDataUserChartRow(PsmhDb pDbCon, Control frmXmlForm, string strMACRONO, bool blnErrOption)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            
            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "        R.ITEMCD, R.ITEMINDEX, R.ITEMTYPE, R.ITEMVALUE, R.ITEMVALUE1";
                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRUSERCHARTROW R";
                SQL = SQL + ComNum.VBLF + "    WHERE R.MACRONO = " + VB.Val(strMACRONO);

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }
                
                int i = 0;
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    string strITEMCD = "";
                    //string strITEMINDEX = "";
                    string strITEMVALUE = "";
                    string strITEMVALUE1 = "";
                    string strITEMTYPE = "";

                    strITEMCD = dt.Rows[i]["ITEMCD"].ToString().Trim();

                    strITEMTYPE = dt.Rows[i]["ITEMTYPE"].ToString().Trim();
                    strITEMVALUE = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                    strITEMVALUE1 = dt.Rows[i]["ITEMVALUE1"].ToString().Trim();

                    string strEmrNoOld = strMACRONO;

                    Control[] tx = null;
                    Control obj = null;

                    if (strITEMTYPE == "DATE")
                    {
                        //if (strITEMCD.Trim() != "dtMedFrDate")
                        //{
                        //    tx = frmXmlForm.Controls.Find(strITEMCD, true);
                        //    if (tx.Length > 0)
                        //    {
                        //        obj = (DateTimePicker)tx[0];
                        //        if (strITEMVALUE != "")
                        //        {
                        //            ((DateTimePicker)obj).Value = Convert.ToDateTime(strITEMVALUE);
                        //        }
                        //    }
                        //}
                    }
                    else if (strITEMTYPE == "TEXT")
                    {
                        tx = frmXmlForm.Controls.Find(strITEMCD, true);
                        if (tx.Length > 0)
                        {
                            obj = (TextBox)tx[0];
                            //string strText = strITEMVALUE.Trim(); (strITEMVALUE.Replace("\n", "\r\n")).Replace("]]", "");
                            //string strText1 = strITEMVALUE1.Trim(); (strITEMVALUE1.Replace("\n", "\r\n")).Replace("]]", "");
                            string strText = strITEMVALUE.Trim(); strITEMVALUE.Replace("]]", "");
                            string strText1 = strITEMVALUE1.Trim(); strITEMVALUE1.Replace("]]", "");
                            if (((TextBox)obj).Multiline == false)
                            {
                                obj.Text = strText.Replace("\r\n", " ") + strText1.Replace("\r\n", " ");
                            }
                            else
                            {
                                obj.Text = strText + strText1;
                            }
                        }
                    }
                    else if (strITEMTYPE == "COMBO")
                    {
                        if (strITEMCD.Trim() != "txtMedFrTime")
                        {
                            tx = frmXmlForm.Controls.Find(strITEMCD, true);
                            if (tx.Length > 0)
                            {
                                obj = (ComboBox)tx[0];
                                obj.Text = VB.Replace(strITEMVALUE, "", "", 1, -1);
                            }
                        }
                    }
                    else if (strITEMTYPE == "CHECK")
                    {
                        tx = frmXmlForm.Controls.Find(strITEMCD, true);
                        if (tx.Length > 0)
                        {
                            obj = (CheckBox)tx[0];
                            ((CheckBox)obj).Checked = Convert.ToBoolean(VB.Val(strITEMVALUE));
                        }
                    }
                    else if (strITEMTYPE == "RADIO")
                    {
                        tx = frmXmlForm.Controls.Find(strITEMCD, true);
                        if (tx.Length > 0)
                        {
                            obj = (RadioButton)tx[0];
                            ((RadioButton)obj).Checked = Convert.ToBoolean(VB.Val(strITEMVALUE));
                        }
                    }
                    else if (strITEMTYPE == "IMAGE")
                    {
                        #region //이미지는 사용하지 않는다
                        //tx = frmXmlForm.Controls.Find(strITEMCD, true);
                        //if (tx.Length > 0)
                        //{
                        //    obj = (PictureBox)tx[0];
                        //    ((PictureBox)obj).Tag = strITEMVALUE;

                        //    if (strITEMVALUE != "")
                        //    {
                        //        string strFoldJob = "";
                        //        string strFoldBase = "";
                        //        clsEmrFunc.CheckImageJobFold(ref strFoldJob, ref strFoldBase, strFormNo, strUpdateNo, strEmrNoOld, strITEMCD);

                        //        //서버의 이미지를 다운로드한다.
                        //        SQL = "";
                        //        SQL = SQL + ComNum.VBLF + "SELECT A.EMRNO, A.ITEMCD, A.IMAGENO, A.IMGSVR, A.IMGPATH, A.IMGEXTENSION ";
                        //        SQL = SQL + ComNum.VBLF + "        , B.BASNAME , B.BASEXNAME , B.REMARK1, B.REMARK2, B.VFLAG1 ";
                        //        SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRCHARTIMAGE A";
                        //        SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD B";
                        //        SQL = SQL + ComNum.VBLF + "        ON B.BSNSCLS = '기록지관리'";
                        //        SQL = SQL + ComNum.VBLF + "        AND B.UNITCLS = '이미지PATH'";
                        //        SQL = SQL + ComNum.VBLF + "        AND B.BASCD = A.IMGSVR";
                        //        SQL = SQL + ComNum.VBLF + "    WHERE A.EMRNO = " + VB.Val(strMACRONO);
                        //        SQL = SQL + ComNum.VBLF + "        AND A.ITEMCD = '" + strITEMCD + "'";

                        //        DataTable dt1 = null;
                        //        SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, pDbCon);

                        //        if (SqlErr != "")
                        //        {
                        //            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        //            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        //            return;
                        //        }
                        //        if (dt1.Rows.Count > 0)
                        //        {
                        //            string strImageNameJpg = dt1.Rows[0]["IMAGENO"].ToString().Trim() + ".jpg";
                        //            string strImageNameDgr = dt1.Rows[0]["IMAGENO"].ToString().Trim() + ".dgr";
                        //            string strServerImgPath = dt1.Rows[0]["IMGPATH"].ToString().Trim();

                        //            string ServerAddress = dt1.Rows[0]["BASNAME"].ToString().Trim();
                        //            string strServerPath = dt1.Rows[0]["BASEXNAME"].ToString().Trim();
                        //            string ServerPort = "21";
                        //            string UserName = dt1.Rows[0]["REMARK1"].ToString().Trim();
                        //            string Password = dt1.Rows[0]["REMARK2"].ToString().Trim();
                        //            string HomePath = dt1.Rows[0]["VFLAG1"].ToString().Trim();

                        //            try
                        //            {
                        //                //clsWinScp.ConWinScp("Ftp", ServerAddress, UserName, Password, HomePath, "");

                        //                //if (clsWinScp.gWinScp.FileExists(strServerPath + "/" + strServerImgPath + "/" + strImageNameJpg) == true)
                        //                //{
                        //                //    clsWinScp.gTrsResult = clsWinScp.gWinScp.GetFiles(strServerPath + "/" + strServerImgPath + "/" + strImageNameJpg, strFoldJob + "\\" + strImageNameJpg, false, clsWinScp.gTrsOptions);
                        //                //}
                        //            }
                        //            catch
                        //            {

                        //            }

                        //            //이미지를 표시한다.
                        //            int intWidth = ((PictureBox)obj).Width;
                        //            int intHeight = ((PictureBox)obj).Height;

                        //            Bitmap image1 = (Bitmap)Image.FromFile(strFoldJob + "\\" + strImageNameJpg, true);
                        //            Bitmap newImage = new Bitmap(intWidth, intHeight, PixelFormat.Format24bppRgb);
                        //            Graphics graphics_1 = Graphics.FromImage(newImage);
                        //            graphics_1.CompositingQuality = CompositingQuality.HighQuality;
                        //            graphics_1.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        //            graphics_1.SmoothingMode = SmoothingMode.HighQuality;
                        //            graphics_1.DrawImage(image1, 0, 0, intWidth, intHeight);

                        //            ((PictureBox)obj).Image = newImage;
                        //            image1.Dispose();
                        //            image1 = null;
                        //        }
                        //        dt1.Dispose();
                        //        dt1 = null;
                        //        //추가 이미지 패널 보이기...
                        //        if (obj.Parent is mtsPanel15.mPanel)
                        //        {
                        //            ImagePanelVisible(frmXmlForm, obj.Parent, true);
                        //        }
                        //    }
                        //}
                        #endregion //이미지는 사용하지 않는다
                    }
                }

                dt.Dispose();
                dt = null;
                
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }


    }
}
