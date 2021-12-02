using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
//using System.Type;
using System.Xml;
using ComBase; //기본 클래스
using ComDbB; //DB연결
using Oracle.DataAccess.Client;

namespace ComEmrBase
{
    public class clsXMLOld
    {
        /// <summary>
        /// 기록내용을 화면에 뿌려준다
        /// </summary>
        /// <param name="frmXmlForm">기록지폼</param>
        /// <param name="strEmrNo"></param>
        /// <param name="blnOption"> true : 에러 표시, false : 에러 무시</param>
        /// <param name="OldYn">이전내역일 경우</param>
        /// <param name="dtp">작성일자</param>
        /// <param name="cb">작성시간</param>
        public static void LoadDataXML(PsmhDb pDbCon, Control frmXmlForm, string strEmrNo, bool blnErrOption, bool OldYn, DateTimePicker dtp, ComboBox cb)
        {
            XmlDocument Doc = new XmlDocument();

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            string strXml = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT CHARTXML, CHARTDATE, CHARTTIME ";
                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "VIEWEMRXML";
                SQL = SQL + ComNum.VBLF + "    WHERE EMRNO = " + VB.Val(strEmrNo);
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

                strXml = (dt.Rows[0]["CHARTXML"].ToString()).Trim();
                if (OldYn == true)
                {
                    dtp.Value = Convert.ToDateTime(ComFunc.FormatStrToDate((dt.Rows[0]["CHARTDATE"].ToString()).Trim(), "D"));
                    cb.Text = ComFunc.FormatStrToDate((dt.Rows[0]["CHARTTIME"].ToString()).Trim(), "M");
                    //dtp.Enabled = false;
                }
                dt.Dispose();
                dt = null;

                Doc.LoadXml(strXml);

                SetUserXmlValue(frmXmlForm, Doc, blnErrOption);

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }


        /// <summary>
        /// 스프래드에 값을 뿌린다
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="Doc"></param>
        /// <param name="blnErrOption">true : 에러 표시, false : 에러 무시</param>
        private static void SetUserXmlValue(Control ctl, XmlDocument Doc, bool blnErrOption)
        {
            XmlNodeList nodeList = null;

            nodeList = Doc.SelectNodes("chart");

            try
            {
                foreach (XmlNode node in nodeList)
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        string strName = "";
                        string strIndex = "";
                        string strValue = "";
                        string strType = "";
                        //string strMacro = "";

                        strName = childNode.Name.ToString();

                        strType = childNode.Attributes.GetNamedItem("Type").Value;
                        strValue = (childNode.InnerText.ToString() + "").ToString();
                        strIndex = "";
                        if (strType != "fpSpread")
                        {
                            strIndex = childNode.Attributes.GetNamedItem("Index").Value;
                        }

                        if (strIndex.Trim() != "")
                        {
                            if (VB.InStr(strName, "_") == 0)
                            {
                                strName = strName + "_" + strIndex;
                            }
                        }
                        Control[] tx = null;
                        Control obj = null;

                        if (strType == "DATE")
                        {
                            if (strName.Trim() != "dtMedFrDate")
                            {
                                tx = ctl.Controls.Find(strName, true);
                                if (tx.Length > 0)
                                {
                                    obj = (DateTimePicker)tx[0];
                                    if (strValue != "")
                                    {
                                        ((DateTimePicker)obj).Value = Convert.ToDateTime(strValue);
                                    }
                                }
                            }
                        }
                        else if (strType == "TEXT")
                        {
                            tx = ctl.Controls.Find(strName, true);
                            if (tx.Length > 0)
                            {
                                obj = (TextBox)tx[0];
                                string strText = (strValue.Replace("\n", "\r\n")).Replace("]]", "");
                                if (((TextBox)obj).Multiline == false)
                                {
                                    obj.Text = strText.Replace("\r\n", " ");
                                }
                                else
                                {
                                    obj.Text = strText;
                                }
                            }
                        }
                        else if (strType == "COMBO")
                        {
                            if (strName.Trim() != "txtMedFrTime")
                            {
                                tx = ctl.Controls.Find(strName, true);
                                if (tx.Length > 0)
                                {
                                    obj = (ComboBox)tx[0];
                                    obj.Text = VB.Replace(strValue, "", "", 1, -1);
                                }
                            }
                        }
                        else if (strType == "CHECK")
                        {
                            tx = ctl.Controls.Find(strName, true);
                            if (tx.Length > 0)
                            {
                                obj = (CheckBox)tx[0];
                                ((CheckBox)obj).Checked = Convert.ToBoolean(VB.Val(strValue));
                            }
                        }
                        else if (strType == "RADIO")
                        {
                            tx = ctl.Controls.Find(strName, true);
                            if (tx.Length > 0)
                            {
                                obj = (RadioButton)tx[0];
                                ((RadioButton)obj).Checked = Convert.ToBoolean(VB.Val(strValue));
                            }
                        }
                        //else if (strType == "fpSpreadSheet")
                        //{
                        //    setSpread(childNode, ctl);
                        //}
                        //else if (strType == "fpSpread")
                        //{
                        //    setSpread(childNode, ctl);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }


        /// <summary>
        /// 기록지 스프래드를 초기화 한다
        /// </summary>
        /// <param name="spreadNode"></param>
        /// <param name="ctl"></param>
        private static void setSpread(XmlNode spreadNode, Control ctl)
        {
            string strName = "";
            string strSpdName = "";
            string strIndex = "";
            FarPoint.Win.Spread.FpSpread spd = null;
            int intRow = -1;
            int intCol = -1;
            string strValue = "";
            string strType = "";
            string strCellType = "";
            int intPlus = -1;

            try
            {
                strType = spreadNode.Attributes.GetNamedItem("Type").Value;

                if (strType == "fpSpreadSheet")
                {
                    strName = spreadNode.Name.ToString();
                    strIndex = spreadNode.Attributes.GetNamedItem("Index").Value;
                    strType = spreadNode.Attributes.GetNamedItem("Type").Value;
                    strValue = (spreadNode.InnerText.ToString() + "").ToString();

                    if (strIndex.Trim() != "")
                    {
                        if (VB.InStr(strName, "_") == 0)
                        {
                            strSpdName = strName + "_" + strIndex;
                        }
                    }
                    else
                    {
                        strSpdName = strName;
                    }
                    Control[] tx = null;

                    tx = ctl.Controls.Find(strSpdName, true);
                    spd = (FarPoint.Win.Spread.FpSpread)tx[0];

                    spd.ActiveSheet.RowCount = Convert.ToInt32(VB.Val(VB.Split(strValue, "_")[0].ToString()));
                    spd.ActiveSheet.ColumnCount = Convert.ToInt32(VB.Val(VB.Split(strValue, "_")[1].ToString()));
                    spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);
                }
                else //fpSpread
                {
                    strName = spreadNode.Name.ToString();
                    //strIndex = spreadNode.Attributes.GetNamedItem("Index").Value;
                    strType = spreadNode.Attributes.GetNamedItem("Type").Value;
                    strValue = (spreadNode.InnerText.ToString() + "").ToString();
                    strCellType = spreadNode.Attributes.GetNamedItem("CellType").Value;

                    strSpdName = VB.Split(strName, "_")[0].ToString();

                    Control[] tx = null;

                    tx = ctl.Controls.Find(strSpdName, true);
                    spd = (FarPoint.Win.Spread.FpSpread)tx[0];

                    intRow = Convert.ToInt32(VB.Val(VB.Split(strName, "_")[1].ToString())) + intPlus;
                    intCol = Convert.ToInt32(VB.Val(VB.Split(strName, "_")[2].ToString())) + intPlus;
                    //CellType : 10:ChexkBox, 0:Date, 1:Edit, 7:Button, 8:ComboBox, 12:Currency, 13:Number, 11:OwnerDrawn, 14:Percent, 4:Pic, 9:Picture, 15:Scientific, 5:StaticText, 6:Time
                    if (strCellType == "10")
                    {
                        spd.ActiveSheet.Cells[intRow, intCol].Value = Convert.ToBoolean(VB.Val(strValue));
                    }
                    else
                    {
                        spd.ActiveSheet.Cells[intRow, intCol].Text = strValue.Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }


        /// <summary>
        /// 기록지 스프래드에 값을 뿌린다 : 대량으로
        /// </summary>
        /// <param name="strITEMCD"></param>
        /// <param name="strITEMNO"></param>
        /// <param name="strITEMINDEX"></param>
        /// <param name="strITEMTYPE"></param>
        /// <param name="strITEMVALUE"></param>
        /// <param name="strITEMVALUE1"></param>
        /// <returns></returns>
        public static string SaveDataToXmlBatch(string[] strITEMCD, string[] strITEMNO, string[] strITEMINDEX, string[] strITEMTYPE, string[] strITEMVALUE, string[] strITEMVALUE1)
        {
            string strXML = "";
            int i = 0;
            //int j = 0;

            XmlDocument Doc = new XmlDocument();
            XmlDeclaration dec = null;
            XmlElement DocRoot = null;
            try
            {
                //해드 선언
                dec = Doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                Doc.AppendChild(dec);
                DocRoot = Doc.CreateElement("chart");
                Doc.AppendChild(DocRoot);

                strXML = Doc.ToString();

                strXML = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + "\r\n" + "<chart>" + "\r\n";

                for (i = 0; i < strITEMCD.Length; i++)
                {
                    strXML = strXML + "<" + strITEMCD[i].Trim() + "><![CDATA[" + strITEMVALUE[i].Trim() + strITEMVALUE1[i].Trim() + "]]></" + strITEMCD[i].Trim() + ">";
                }

                if (strXML != "")
                {
                    strXML = strXML + "</chart>";
                }
                return strXML;
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        /// 폼서식을 저장한다. - VB6에서 컨버젼
        /// </summary>
        /// <param name="frmXmlForm"></param>
        /// <param name="isSpcPanel"></param>
        /// <param name="pControl"></param>
        /// <returns></returns>
        public static string SaveDataToXmlOld(Control frmXmlForm, bool isSpcPanel, Control pControl)
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
                    strConIndex = clsXML.IsArryCon(objControl);

                    strConName = objControl.Name;

                    if ((objControl is FarPoint.Win.Spread.FpSpread) == false)
                    {
                        if (VB.InStr(strConName, "_") > 0)
                        {
                            string[] strParams = VB.Split(VB.Trim(strConName), "_", -1);
                            strConName = strParams[0];
                        }
                    }

                    //DateTimePicker(DTPicker)
                    if (objControl is DateTimePicker)
                    {
                        if (isSpcPanel == true)
                        {
                            if (clsXML.IsParent(objControl, pControl) == true)
                            {
                                strXML = strXML + "<" + strConName + " Type=\"DATE\"" + " Index=\"" + strConIndex + "\"><![CDATA[" + VB.Format(((DateTimePicker)objControl).Value, "yyyy-MM-dd") + "]]></" + strConName + ">";
                            }
                        }
                        else
                        {
                            strXML = strXML + "<" + strConName + " Type=\"DATE\"" + " Index=\"" + strConIndex + "\"><![CDATA[" + VB.Format(((DateTimePicker)objControl).Value, "yyyy-MM-dd") + "]]></" + strConName + ">";
                        }
                    }
                    //TextBox
                    if (objControl is TextBox)
                    {
                        if (isSpcPanel == true)
                        {
                            if (clsXML.IsParent(objControl, pControl) == true)
                            {
                                strXML = strXML + "<" + strConName + " Type=\"TEXT\"" + " Index=\"" + strConIndex + "\"><![CDATA[" + VB.Trim(objControl.Text) + "]]></" + strConName + ">";
                            }
                        }
                        else
                        {
                            strXML = strXML + "<" + strConName + " Type=\"TEXT\"" + " Index=\"" + strConIndex + "\"><![CDATA[" + VB.Trim(objControl.Text) + "]]></" + strConName + ">";
                        }
                    }
                    //ComboBox
                    if (objControl is ComboBox)
                    {
                        if (isSpcPanel == true)
                        {
                            if (clsXML.IsParent(objControl, pControl) == true)
                            {
                                strXML = strXML + "<" + strConName + " Type=\"COMBO\"" + " Index=\"" + strConIndex + "\"><![CDATA[" + VB.Trim(objControl.Text) + "]]></" + strConName + ">";
                            }
                        }
                        else
                        {
                            strXML = strXML + "<" + strConName + " Type=\"COMBO\"" + " Index=\"" + strConIndex + "\"><![CDATA[" + VB.Trim(objControl.Text) + "]]></" + strConName + ">";
                        }
                    }
                    //CheckBox
                    if (objControl is CheckBox)
                    {
                        if (isSpcPanel == true)
                        {
                            if (clsXML.IsParent(objControl, pControl) == true)
                            {
                                strXML = strXML + "<" + strConName + " Type=\"CHECK\"" + " Index=\"" + strConIndex + "\"><![CDATA[" + (((CheckBox)objControl).Checked == true ? "1" : "0") + "]]></" + strConName + ">";
                            }
                        }
                        else
                        {
                            strXML = strXML + "<" + strConName + " Type=\"CHECK\"" + " Index=\"" + strConIndex + "\"><![CDATA[" + (((CheckBox)objControl).Checked == true ? "1" : "0") + "]]></" + strConName + ">";
                        }
                    }
                    //RadioButton(OptionButton)
                    if (objControl is RadioButton)
                    {
                        if (isSpcPanel == true)
                        {
                            if (clsXML.IsParent(objControl, pControl) == true)
                            {
                                strXML = strXML + "<" + strConName + " Type=\"RADIO\"" + " Index=\"" + strConIndex + "\"><![CDATA[" + (((RadioButton)objControl).Checked == true ? "1" : "0") + "]]></" + strConName + ">";
                            }
                        }
                        else
                        {
                            strXML = strXML + "<" + strConName + " Type=\"RADIO\"" + " Index=\"" + strConIndex + "\"><![CDATA[" + (((RadioButton)objControl).Checked == true ? "1" : "0") + "]]></" + strConName + ">";
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
        /// 스프래드 서식을 저장한다.
        /// </summary>
        /// <param name="SpdWrite"></param>
        /// <returns></returns>
        public static string SaveDataToXmlSpd(FarPoint.Win.Spread.FpSpread SpdWrite)
        {
            //string rtnVal = "";
            string strXML = "";
            int i = 0;
            int j = 0;

            string strConName = SpdWrite.Name;
            string strConIndex = "";

            XmlDocument Doc = new XmlDocument();
            XmlDeclaration dec = null;
            XmlElement DocRoot = null;

            try
            {

                //해드 선언
                dec = Doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                Doc.AppendChild(dec);
                DocRoot = Doc.CreateElement("chart");
                Doc.AppendChild(DocRoot);

                strXML = Doc.ToString();

                strXML = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + "\r\n" + "<chart>" + "\r\n";

                //fpSpread
                FarPoint.Win.Spread.FpSpread ssSpd = null;
                ssSpd = SpdWrite;

                strXML = strXML + "<" + strConName + " Type=\"fpSpreadSheet\"" + " Index=\"" + strConIndex + "\"><![CDATA[" + Convert.ToString(ssSpd.ActiveSheet.RowCount) + "_" + Convert.ToString(ssSpd.ActiveSheet.ColumnCount) + "]]></" + strConName + ">";
                for (i = 0; i < ssSpd.ActiveSheet.RowCount; i++)
                {
                    for (j = 0; j < ssSpd.ActiveSheet.ColumnCount; j++)
                    {
                        strXML = strXML + SaveSpreadData(ssSpd, i, j);
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
        /// 스프래드의 Data를 저장한다
        /// </summary>
        /// <param name="spd"></param>
        /// <param name="Row"></param>
        /// <param name="Col"></param>
        /// <returns></returns>
        public static string SaveSpreadData(FarPoint.Win.Spread.FpSpread spd, int Row, int Col)
        {
            string rtnVal = "";
            string strXmlX = "";
            //string CellType = "";
            //string TypeVAlign = "";
            //string TypeHAlign = "";
            string strHead = "";
            string strTail = "";
            string strMid = "";
            string strValue = "";

            strHead = "<" + spd.Name + "_" + Convert.ToString(Row + 1) + "_" + Convert.ToString(Col + 1) + " Type=\"fpSpread\"";
            strMid = "><![CDATA[";
            strTail = "]]></" + spd.Name + "_" + Convert.ToString(Row + 1) + "_" + Convert.ToString(Col + 1) + ">";

            strXmlX = strHead + " RowHeight=\"" + spd.ActiveSheet.Rows[Row].Height + "\"" + " ColWidth=\"" + spd.ActiveSheet.Columns[Col].Width + "\"";
            //TypeVAlign="2" TypeHAlign="2" CellType="10" TypeCheckTextAlign="1" TypeCheckText=""
            //strXmlX = strXmlX + " TypeVAlign=\"" + spd.ActiveSheet.Cells[Row, Col].VerticalAlignment.ToString() + "\"";
            //strXmlX = strXmlX + " TypeHAlign=\"" + spd.ActiveSheet.Cells[Row, Col].HorizontalAlignment.ToString() + "\"";
            //strXmlX = strXmlX + " CellType=\"" + spd.ActiveSheet.Cells[Row, Col].CellType.ToString() + "\"";

            if (spd.ActiveSheet.Cells[Row, Col].VerticalAlignment == FarPoint.Win.Spread.CellVerticalAlignment.Top)
            {
                strXmlX = strXmlX + " TypeVAlign=\"" + "0" + "\"";
            }
            else if (spd.ActiveSheet.Cells[Row, Col].VerticalAlignment == FarPoint.Win.Spread.CellVerticalAlignment.Bottom)
            {
                strXmlX = strXmlX + " TypeVAlign=\"" + "1" + "\"";
            }
            else
            {
                strXmlX = strXmlX + " TypeVAlign=\"" + "2" + "\"";  //Center
            }
            if (spd.ActiveSheet.Cells[Row, Col].HorizontalAlignment == FarPoint.Win.Spread.CellHorizontalAlignment.Left)
            {
                strXmlX = strXmlX + " TypeHAlign=\"" + "0" + "\"";
            }
            else if (spd.ActiveSheet.Cells[Row, Col].HorizontalAlignment == FarPoint.Win.Spread.CellHorizontalAlignment.Right)
            {
                strXmlX = strXmlX + " TypeHAlign=\"" + "1" + "\"";
            }
            else
            {
                strXmlX = strXmlX + " TypeHAlign=\"" + "2" + "\"";  //Center
            }
            //CellType : 10:ChexkBox, 0:Date, 1:Edit, 7:Button, 8:ComboBox, 12:Currency, 13:Number, 11:OwnerDrawn, 14:Percent, 4:Pic, 9:Picture, 15:Scientific, 5:StaticText, 6:Time
            //TypeVAlign="2" TypeHAlign="2" CellType="10" TypeCheckTextAlign="1" TypeCheckText=""
            if (spd.ActiveSheet.Cells[Row, Col].CellType != null)
            {
                if (spd.ActiveSheet.Cells[Row, Col].CellType.ToString() == "CheckBoxCellType")
                {
                    strXmlX = strXmlX + " CellType=\"" + "10" + "\"";
                    //변환이 어려움으로 기본적으로 스프래드에 세팅된 값을 사용하도록 한다.
                    //FarPoint.Win.Spread.CellType.CheckBoxCellType ck = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
                    strXmlX = strXmlX + " TypeCheckTextAlign=\"" + "1" + "\"" + " TypeCheckText=\"" + "" + "\"";
                    if (Convert.ToBoolean(spd.ActiveSheet.Cells[Row, Col].Value) == true)
                    {
                        strValue = "1";
                    }
                    else
                    {
                        strValue = "0";
                    }
                }
                else if (spd.ActiveSheet.Cells[Row, Col].CellType.ToString() == "ComboBoxCellType")
                {
                    strXmlX = strXmlX + " CellType=\"" + "8" + "\"";
                    strValue = spd.ActiveSheet.Cells[Row, Col].Text.Trim();
                }
                else  //나머지 TEXTBOX
                {
                    strXmlX = strXmlX + " CellType=\"" + "1" + "\"";
                    strValue = spd.ActiveSheet.Cells[Row, Col].Text.Trim();
                }
            }
            else
            {
                strXmlX = strXmlX + " CellType=\"" + "1" + "\"";
                strValue = spd.ActiveSheet.Cells[Row, Col].Text.Trim();
            }

            rtnVal = strXmlX + strMid + strValue + strTail;
            return rtnVal;
        }


        /// <summary>
        /// EMR DATA를 저장한다.
        /// </summary>
        /// <param name="po"></param>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="strEmrNo"></param>
        /// <returns>0:에러</returns>
        public static double gSaveEmrXml(PsmhDb pDbCon, EmrPatient po, string strFormNo, string strUpdateNo, string strEmrNo, string strChartDate, string strChartTime, 
                                        string strXml, clsEmrType.EmrXmlImage[] pEmrXmlImage, clsEmrType.EmrXmlImageInit[] pEmrXmlImageInit, string strSaveFlag)
        {
            double rtnVal = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            double dblEmrHisNo = 0;
            double dblEmrNoNew = 0;

            if (strChartTime.Length < 6)
            {
                strChartTime = strChartTime + "00";
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(pDbCon);


            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(pDbCon, "A");

                string strUseId = string.Empty;
                string strWriteDate = string.Empty;
                string strWriteTime = string.Empty;

                //이전 작성한 내역이 있으면 백업하고
                if (VB.Val(strEmrNo) != 0)
                {
                    if (strSaveFlag == "SAVE")
                    {
                        #region 코딩 관련 EMR 작성시간, 작성자 가져오기
                        OracleDataReader dataReader = null;

                        SQL = "SELECT USEID, WRITEDATE, WRITETIME";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMRXMLMST";
                        SQL = SQL + ComNum.VBLF + " WHERE EMRNO = " + strEmrNo;

                        SqlErr = clsDB.GetAdoRs(ref dataReader, SQL, pDbCon);
                        if(SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        if(dataReader.HasRows && dataReader.Read())
                        {
                            strUseId = dataReader.GetValue(0).ToString().Trim();
                            strWriteDate = dataReader.GetValue(1).ToString().Trim();
                            strWriteTime = dataReader.GetValue(2).ToString().Trim();
                        }

                        dataReader.Dispose();
                        #endregion

                        #region 권한으로 변경내역 저장
                        SQL = " INSERT INTO KOSMOS_EMR.EMRXML_MODIFY(UDATE , USABUN, EMRNO, WRITEDATE, WRITETIME, GUBUN) VALUES (";
                        SQL += ComNum.VBLF + "SYSDATE, '" + clsType.User.IdNumber + "','" + strEmrNo + "','" + strWriteDate + "',";
                        SQL += ComNum.VBLF + "'" + strWriteTime + "','" + "" + "')";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                        #endregion

                        if (string.IsNullOrWhiteSpace(strUseId) || strUseId.Equals("0"))
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox("기존 저장 내역을 읽어오는 도중 오류가 발생했습니다. 다시 저장해주세요");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        #region 입퇴원요약지 검수 관련 데이터 정리
                        if (strFormNo.Equals("1647"))
                        {
                            SQL = "INSERT INTO KOSMOS_EMR.EMRXML_COMPLETE_HISTORY(";
                            SQL += ComNum.VBLF + " EMRNO, CDATE, CSABUN, DELDATE, DELSABUN, MEDFRDATE, PTNO, INDATE) ";
                            SQL += ComNum.VBLF + " SELECT EMRNO, CDATE, CSABUN, SYSDATE, " + clsType.User.IdNumber + ", MEDFRDATE, PTNO, INDATE";
                            SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMRXML_COMPLETE ";
                            SQL += ComNum.VBLF + "  WHERE EMRNO = " + VB.Val(strEmrNo);

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                            if (SqlErr.Length > 0)
                            {
                                clsDB.setRollbackTran(pDbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                                return rtnVal;
                            }

                            SQL = " DELETE KOSMOS_EMR.EMRXML_COMPLETE ";
                            SQL += ComNum.VBLF + "  WHERE PTNO = '" + po.ptNo + "'";
                            SQL += ComNum.VBLF + "    AND MEDFRDATE = '" + po.medFrDate + "'";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                            if (SqlErr.Length > 0)
                            {
                                clsDB.setRollbackTran(pDbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                                return rtnVal;
                            }
                        }
                        #endregion
                    }

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_EMR + "EMRXMLMST_HISTORY";
                    SQL = SQL + ComNum.VBLF + "      ( ";
                    SQL = SQL + ComNum.VBLF + "      EMRNO, PTNO, GBEMR,";
                    SQL = SQL + ComNum.VBLF + "      FORMNO, USEID, CHARTDATE, CHARTTIME, ";
                    SQL = SQL + ComNum.VBLF + "      INOUTCLS, MEDFRDATE, MEDFRTIME, MEDENDDATE, MEDENDTIME, ";
                    SQL = SQL + ComNum.VBLF + "      MEDDEPTCD, MEDDRCD, WRITEDATE, WRITETIME";
                    SQL = SQL + ComNum.VBLF + "      ) ";
                    SQL = SQL + ComNum.VBLF + " SELECT  ";
                    SQL = SQL + ComNum.VBLF + "      EMRNO, PTNO, GBEMR,";
                    SQL = SQL + ComNum.VBLF + "      FORMNO, USEID, CHARTDATE, CHARTTIME, ";
                    SQL = SQL + ComNum.VBLF + "      INOUTCLS, MEDFRDATE, MEDFRTIME, MEDENDDATE, MEDENDTIME, ";
                    SQL = SQL + ComNum.VBLF + "      MEDDEPTCD, MEDDRCD, WRITEDATE, WRITETIME";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMRXMLMST";
                    SQL = SQL + ComNum.VBLF + " WHERE EMRNO = " + VB.Val(strEmrNo);

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " DELETE FROM " + ComNum.DB_EMR + "EMRXMLMST";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + VB.Val(strEmrNo);
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    dblEmrHisNo = VB.Val(ComQuery.gGetSequencesNo(pDbCon, ComNum.DB_EMR + "EMRXMLHISNO"));

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_EMR + "EMRXMLHISTORY";
                    SQL = SQL + ComNum.VBLF + "      (HISTORYNO, EMRNO, FORMNO,                        ";
                    SQL = SQL + ComNum.VBLF + "      USEID, CHARTXML, CHARTDATE,                      ";
                    SQL = SQL + ComNum.VBLF + "      CHARTTIME, ACPNO, PTNO,                          ";
                    SQL = SQL + ComNum.VBLF + "      INOUTCLS, MEDFRDATE, MEDFRTIME,                  ";
                    SQL = SQL + ComNum.VBLF + "      MEDENDDATE, MEDENDTIME, MEDDEPTCD,               ";
                    SQL = SQL + ComNum.VBLF + "      MEDDRCD, MIBICHECK, WRITEDATE,                   ";
                    SQL = SQL + ComNum.VBLF + "      WRITETIME, CONTENTS, HISTORYWRITEDATE,           ";
                    SQL = SQL + ComNum.VBLF + "      HISTORYWRITETIME, UPDATENO, EMRSIGNED,           ";
                    SQL = SQL + ComNum.VBLF + "      EMRXMLHASH, CERTDATE, CERTTIME,                  ";
                    SQL = SQL + ComNum.VBLF + "      CERTUSEID, DELUSEID, CERTNO  )                    ";
                    SQL = SQL + ComNum.VBLF + " SELECT  " + dblEmrHisNo + ",";
                    SQL = SQL + ComNum.VBLF + "      EMRNO, FORMNO,                        ";
                    SQL = SQL + ComNum.VBLF + "      USEID, CHARTXML, CHARTDATE,                      ";
                    SQL = SQL + ComNum.VBLF + "      CHARTTIME, ACPNO, PTNO,                          ";
                    SQL = SQL + ComNum.VBLF + "      INOUTCLS, MEDFRDATE, MEDFRTIME,                  ";
                    SQL = SQL + ComNum.VBLF + "      MEDENDDATE, MEDENDTIME, MEDDEPTCD,               ";
                    SQL = SQL + ComNum.VBLF + "      MEDDRCD, MIBICHECK, WRITEDATE,                   ";
                    SQL = SQL + ComNum.VBLF + "      WRITETIME, CONTENTS, ";
                    SQL = SQL + ComNum.VBLF + "      '" + VB.Left(strCurDateTime, 8) + "',";        //HISTORYWRITEDATE
                    SQL = SQL + ComNum.VBLF + "      '" + VB.Right(strCurDateTime, 6) + "',";        //HISTORYWRITETIME
                    SQL = SQL + ComNum.VBLF + "      UPDATENO, EMRSIGNED,           ";
                    SQL = SQL + ComNum.VBLF + "      EMRXMLHASH, CERTDATE, CERTTIME,                  ";
                    SQL = SQL + ComNum.VBLF + "      CERTUSEID, ";
                    if(strSaveFlag == "SAVE")
                    {
                        SQL = SQL + ComNum.VBLF + "      '" + VB.Val(strUseId) + "',";  // DELUSEID, ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "      '" + VB.Val(clsType.User.IdNumber) + "',";  // DELUSEID, ";
                    }
                    SQL = SQL + ComNum.VBLF + "      CERTNO                      ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMRXML";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + VB.Val(strEmrNo);
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " DELETE FROM " + ComNum.DB_EMR + "EMRXML";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + VB.Val(strEmrNo);
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }

                dblEmrNoNew = VB.Val(ComQuery.gGetSequencesNo(pDbCon, ComNum.DB_EMR + "GETEMRXMLNO"));

                SQL = "";
                SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_EMR + "EMRXMLMST";
                SQL = SQL + ComNum.VBLF + "      ( ";
                SQL = SQL + ComNum.VBLF + "      EMRNO, PTNO, GBEMR, ";
                SQL = SQL + ComNum.VBLF + "      FORMNO, USEID, CHARTDATE, CHARTTIME, ";
                SQL = SQL + ComNum.VBLF + "      INOUTCLS, MEDFRDATE, MEDFRTIME, MEDENDDATE, MEDENDTIME, ";
                SQL = SQL + ComNum.VBLF + "      MEDDEPTCD, MEDDRCD, WRITEDATE, WRITETIME";
                SQL = SQL + ComNum.VBLF + "      ) ";
                SQL = SQL + ComNum.VBLF + "      VALUES (";
                SQL = SQL + ComNum.VBLF + "      " + dblEmrNoNew + ",";
                SQL = SQL + ComNum.VBLF + "      '" + po.ptNo + "',";
                SQL = SQL + ComNum.VBLF + "      '1' ,";
                SQL = SQL + ComNum.VBLF + "      " + strFormNo + ",";
                if (strSaveFlag == "SAVE")
                {
                    SQL = SQL + ComNum.VBLF + "      '" + VB.Val(strUseId) + "',";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "      '" + VB.Val(clsType.User.IdNumber) + "',";
                }
                SQL = SQL + ComNum.VBLF + "      '" + strChartDate.Trim() + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strChartTime.Trim() + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.inOutCls + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.medFrDate + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.medFrTime + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.medEndDate + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.medEndTime + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.medDeptCd + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.medDrCd + "',";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Left(strCurDateTime, 8) + "',";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Right(strCurDateTime, 6) + "'";
                SQL = SQL + ComNum.VBLF + "      )";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_EMR + "EMRXML";
                SQL = SQL + ComNum.VBLF + "      (EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,UPDATENO) "; //, CONTENTS) ";
                SQL = SQL + ComNum.VBLF + "      VALUES (";
                SQL = SQL + ComNum.VBLF + "      " + dblEmrNoNew + ",";
                SQL = SQL + ComNum.VBLF + "      " + strFormNo + ",";
                if (strSaveFlag == "SAVE")
                {
                    SQL = SQL + ComNum.VBLF + "      '" + VB.Val(strUseId) + "',";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "      '" + VB.Val(clsType.User.IdNumber) + "',";
                }
                SQL = SQL + ComNum.VBLF + "      '" + strChartDate.Trim() + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strChartTime.Trim() + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.acpNo + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.ptNo + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.inOutCls + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.medFrDate + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.medFrTime + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.medEndDate + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.medEndTime + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.medDeptCd + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.medDrCd + "',";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Left(strCurDateTime, 8) + "',";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Right(strCurDateTime, 6) + "',";
                SQL = SQL + ComNum.VBLF + "      :1,";
                SQL = SQL + ComNum.VBLF + "      '" + strUpdateNo + "'";
                //SQL = SQL + ComNum.VBLF + "      (SELECT CONTENTS FROM " + ComNum.DB_EMR + "EMRFORM WHERE FORMNO = " + strFormNo + ")";
                SQL = SQL + ComNum.VBLF + "      )";
                SqlErr = clsDB.ExecuteXmlQueryEx(SQL, strXml, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                
                if (pEmrXmlImageInit != null && pEmrXmlImage != null)
                {
                    if (pEmrXmlImage.Length > 0)
                    {
                        for (int i = 0; i < pEmrXmlImage.Length; i++)
                        {
                            for (int j = 0; j < pEmrXmlImageInit.Length; j++)
                            {
                                //if (clsEmrFunc.CompareBitmapsFast((Bitmap) ((PictureBox)pEmrXmlImage[i].Pic).Image, (Bitmap)pEmrXmlImageInit[j].Img) == false)
                                if (clsEmrFunc.ImageCompare((Bitmap)((PictureBox)pEmrXmlImage[i].Pic).Image, (Bitmap)pEmrXmlImageInit[j].Img) == false)
                                {
                                    string strBIGFILE = VB.Left(strCurDateTime, 4) + "/" + VB.Left(strCurDateTime, 8) + "/" + VB.Left(strCurDateTime, 8) + VB.Left(strCurDateTime, 6) + ".jpg";
                                    string strSMALLFILE = VB.Left(strCurDateTime, 4) + "/" + VB.Left(strCurDateTime, 8) + "/s" + VB.Left(strCurDateTime, 8) + VB.Left(strCurDateTime, 6) + ".jpg";

                                    //string[] arryXmlImage = new string[2];
                                    //arryXmlImage[0] = clsFuncImage.ImageToBase64(((PictureBox)pEmrXmlImage[i].Pic).Image, ImageFormat.Png);
                                    //arryXmlImage[1] = clsFuncImage.ImageToBase64(((PictureBox)pEmrXmlImage[i].Pic).Image, ImageFormat.Png);

                                    byte[] arryXmlImage1 = clsFuncImage.ImageToByte2(((PictureBox)pEmrXmlImage[i].Pic).Image, ImageFormat.Png);
                                    byte[] arryXmlImage2 = clsFuncImage.ImageToByte2(((PictureBox)pEmrXmlImage[i].Pic).Image, ImageFormat.Png);

                                    SQL = "";
                                    SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_EMR + "EMRXMLIMAGES";
                                    SQL = SQL + ComNum.VBLF + "(	";
                                    SQL = SQL + ComNum.VBLF + "		EMRIMAGENO, EMRIMAGEFILE, IMGNO,         ";
                                    SQL = SQL + ComNum.VBLF + "		EMRNO, WRITEDATE, WRITETIME,             ";
                                    SQL = SQL + ComNum.VBLF + "		USEID, EMRIMAGEMERGE, BIGFILE,           ";
                                    SQL = SQL + ComNum.VBLF + "		SMALLFILE, PTNO, CHARTDATE, CHARTTIME    ";
                                    SQL = SQL + ComNum.VBLF + ")	";
                                    SQL = SQL + ComNum.VBLF + "VALUES	";
                                    SQL = SQL + ComNum.VBLF + "(	";
                                    SQL = SQL + ComNum.VBLF + "		" + pEmrXmlImage[i].ImageNo + ",";     //EMRIMAGENO, 
                                    SQL = SQL + ComNum.VBLF + "		:1,";     //EMRIMAGEFILE, 
                                    SQL = SQL + ComNum.VBLF + "		" + "0" + ",";     //IMGNO, 
                                    SQL = SQL + ComNum.VBLF + "		" + dblEmrNoNew.ToString() + ",";     //EMRNO, 
                                    SQL = SQL + ComNum.VBLF + "		'" + VB.Left(strCurDateTime, 8) + "',";     //WRITEDATE, 
                                    SQL = SQL + ComNum.VBLF + "		'" + VB.Right(strCurDateTime, 6) + "',";     //WRITETIME, 
                                    if (strSaveFlag == "SAVE")
                                    {
                                        SQL = SQL + ComNum.VBLF + "		'" + VB.Val(strUseId) + "',";     //USEID, 
                                    }
                                    else
                                    {
                                        SQL = SQL + ComNum.VBLF + "		'" + VB.Val(clsType.User.IdNumber) + "',";     //USEID, 
                                    }
                                    SQL = SQL + ComNum.VBLF + "		:2,";     //EMRIMAGEMERGE, 
                                    SQL = SQL + ComNum.VBLF + "		'" + strBIGFILE + "',";     //BIGFILE, 
                                    SQL = SQL + ComNum.VBLF + "		'" + strSMALLFILE + "',";     //SMALLFILE, 
                                    SQL = SQL + ComNum.VBLF + "		'" + po.ptNo + "',";     //PTNO, 
                                    SQL = SQL + ComNum.VBLF + "		'" + strChartDate + "',";     //CHARTDATE, 
                                    SQL = SQL + ComNum.VBLF + "		'" + strChartTime + "'";     //CHARTTIME
                                    SQL = SQL + ComNum.VBLF + ")	";

                                    OracleCommand Cmd = null;
                                    if (Cmd == null)
                                    {
                                        Cmd = pDbCon.Con.CreateCommand();
                                    }

                                    Cmd.CommandText = SQL;
                                    Cmd.CommandTimeout = 60;
                                    if (pDbCon.Trs != null)
                                    {
                                        Cmd.Transaction = pDbCon.Trs;
                                    }
                                    Cmd.Parameters.Add(":1", OracleDbType.Blob).Value = arryXmlImage1;
                                    Cmd.Parameters.Add(":2", OracleDbType.Blob).Value = arryXmlImage2;

                                    intRowAffected = Cmd.ExecuteNonQuery();
                                    break;
                                }
                            }
                        }
                    }
                }

                string strMiBiCd = string.Empty;

                SQL = "";
                SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_EMR + "EMRMIBI";
                SQL = SQL + ComNum.VBLF + "SET";
                SQL = SQL + ComNum.VBLF + " WRITEDATE = '" + VB.Left(strCurDateTime, 8) + "',";
                SQL = SQL + ComNum.VBLF + " WRITETIME = '" + VB.Right(strCurDateTime, 6) + "'";
                SQL = SQL + ComNum.VBLF + "  WHERE PTNO = '" + po.ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "  AND MEDFRDATE = '" + po.medFrDate + "' ";
                if (strSaveFlag == "SAVE")
                {
                    SQL = SQL + ComNum.VBLF + "  AND MEDDRCD = '" + VB.Val(strUseId) + "' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND MEDDRCD = '" + clsType.User.IdNumber + "' ";
                }
                SQL = SQL + ComNum.VBLF + "  AND MIBICLS = 1";
                SQL = SQL + ComNum.VBLF + "  AND MIBIGRP = '" + clsEmrChart.GetEmrGrp(clsDB.DbCon, strFormNo, ref strMiBiCd) + "'";
                SQL = SQL + ComNum.VBLF + "  AND MIBICD <> 'A13'"; //입퇴원요약지 삭제 아이템
                SQL = SQL + ComNum.VBLF + "  AND MIBICD <> 'C10'"; //입원기록지 삭제  아이템

                if (strMiBiCd.Length > 0)
                {
                    SQL = SQL + ComNum.VBLF + "  AND MIBICD IN(" + strMiBiCd + ")";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                clsDB.setCommitTran(pDbCon);
                //ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                return dblEmrNoNew;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }


        /// <summary>
        /// EMR DATA를 저장한다. ==> 종속된 저장시 사용
        /// </summary>
        /// <param name="po"></param>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="strEmrNoNew"></param>
        /// <param name="strEmrNoOld"></param>
        /// <param name="strChartDate"></param>
        /// <param name="strChartTime"></param>
        /// <param name="strXml"></param>
        /// <returns></returns>
        public static bool gSaveEmrXmlEx(PsmhDb pDbCon, EmrPatient po, string strFormNo, string strUpdateNo, double strEmrNoNew, double dblEmrNoOld,
                                        string strChartDate, string strChartTime, string strXml, clsTrans TRS)
        {
            bool rtnVal = false;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            double dblEmrHisNo = 0;
            //double dblEmrNoNew = 0;

            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(pDbCon, "A");

                //이전 작성한 내역이 있으면 백업하고
                if (dblEmrNoOld != 0)
                {
                    dblEmrHisNo = VB.Val(ComQuery.gGetSequencesNo(pDbCon, ComNum.DB_EMR + "EMRXMLHISNO"));

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_EMR + "EMRXMLHISTORY";
                    SQL = SQL + ComNum.VBLF + "      (HISTORYNO, EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                    SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                    SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,UPDATENO, CONTENTS, HISTORYWRITEDATE,HISTORYWRITETIME)";
                    SQL = SQL + ComNum.VBLF + " SELECT  " + dblEmrHisNo + ",";
                    SQL = SQL + ComNum.VBLF + "      EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                    SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                    SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML, UPDATENO, CONTENTS, ";
                    SQL = SQL + ComNum.VBLF + "      '" + VB.Left(strCurDateTime, 8) + "',";
                    SQL = SQL + ComNum.VBLF + "      '" + VB.Right(strCurDateTime, 6) + "'";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMRXML";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + dblEmrNoOld.ToString();
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " DELETE FROM " + ComNum.DB_EMR + "EMRXML";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + dblEmrNoOld.ToString();
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_EMR + "EMRXML";
                SQL = SQL + ComNum.VBLF + "      (EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,UPDATENO) ";
                SQL = SQL + ComNum.VBLF + "      VALUES (";
                SQL = SQL + ComNum.VBLF + "      " + strEmrNoNew + ",";
                SQL = SQL + ComNum.VBLF + "      " + strFormNo + ",";
                SQL = SQL + ComNum.VBLF + "      '" + clsType.User.IdNumber + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strChartDate + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strChartTime + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.acpNo + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.ptNo + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.inOutCls + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.medFrDate + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.medFrTime + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.medEndDate + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.medEndTime + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.medDeptCd + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.medDrCd + "',";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Left(strCurDateTime, 8) + "',";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Left(strCurDateTime, 6) + "',";
                SQL = SQL + ComNum.VBLF + "      :1,";
                SQL = SQL + ComNum.VBLF + "      '" + strUpdateNo + "')";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        /// <summary>
        /// EMR DATA를 저장한다. ==> 하나만
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="po"></param>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="strEmrNo"></param>
        /// <param name="strChartDate"></param>
        /// <param name="strChartTime"></param>
        /// <param name="strCurDateTime"></param>
        /// <param name="strUseId"></param>
        /// <param name="TRS"></param>
        /// <returns></returns>
        public static bool gSaveEmrXmlOnly(PsmhDb pDbCon, EmrPatient po, string strFormNo, string strUpdateNo, double strEmrNo, string strChartDate, string strChartTime,
                                string strCurDateTime, string strUseId, clsTrans TRS)
        {
            bool rtnVal = false;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_EMR + "EMRXML";
                SQL = SQL + ComNum.VBLF + "      (EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,UPDATENO) ";
                SQL = SQL + ComNum.VBLF + "      VALUES (";
                SQL = SQL + ComNum.VBLF + "      " + strEmrNo + ",";
                SQL = SQL + ComNum.VBLF + "      " + strFormNo + ",";
                SQL = SQL + ComNum.VBLF + "      '" + strUseId + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strChartDate + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strChartTime + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.acpNo + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.ptNo + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.inOutCls + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.medFrDate + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.medFrTime + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.medEndDate + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.medEndTime + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.medDeptCd + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.medDrCd + "',";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Left(strCurDateTime, 8) + "',";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Left(strCurDateTime, 6) + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strUpdateNo + "')";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }

        }


        /// <summary>
        /// EMR DATA를 삭제한다
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strEmrNo"></param>
        /// <param name="strUseId"></param>
        /// <param name="TRS"></param>
        /// <returns></returns>
        public static bool gDeleteEmrXmlEx(PsmhDb pDbCon, string strEmrNo, string strUseId, clsTrans TRS)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(pDbCon, "A");

                double dblEmrHisNo = VB.Val(ComQuery.gGetSequencesNo(pDbCon, ComNum.DB_EMR + "EMRXMLHISNO"));

                SQL = "";
                SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_EMR + "EMRXMLHISTORY";
                SQL = SQL + ComNum.VBLF + "      (HISTORYNO, EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,UPDATENO, CONTENTS, HISTORYWRITEDATE,HISTORYWRITETIME)";
                SQL = SQL + ComNum.VBLF + " SELECT  " + dblEmrHisNo + ",";
                SQL = SQL + ComNum.VBLF + "      EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML, UPDATENO, CONTENTS, ";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Left(strCurDateTime, 8) + "',";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Right(strCurDateTime, 6) + "'";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMRXML";
                SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + VB.Val(strEmrNo);
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + " DELETE FROM " + ComNum.DB_EMR + "EMRXML";
                SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + VB.Val(strEmrNo);
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return false;
            }
        }

        /// <summary>
        /// EMR DATA를 저장한다. ==> 종속된 저장시 사용
        /// </summary>
        /// <param name="po"></param>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="strEmrNoNew"></param>
        /// <param name="strEmrNoOld"></param>
        /// <param name="strChartDate"></param>
        /// <param name="strChartTime"></param>
        /// <param name="strXml"></param>
        /// <returns></returns>
        public static bool gSaveEmrXmlEx(PsmhDb pDbCon, EmrPatient po, string strFormNo, string strUpdateNo, double strEmrNoNew, double strEmrNoOld, string strChartDate, string strChartTime, string strXml)
        {
            bool rtnVal = false;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            double dblEmrHisNo = 0;
            //double dblEmrNoNew = 0;

            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(pDbCon, "A");

                //이전 작성한 내역이 있으면 백업하고
                if (strEmrNoOld != 0)
                {
                    dblEmrHisNo = VB.Val(ComQuery.gGetSequencesNo(pDbCon, ComNum.DB_EMR + "EMRXMLHISNO"));

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_EMR + "EMRXMLHISTORY";
                    SQL = SQL + ComNum.VBLF + "      (HISTORYNO, EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                    SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                    SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,UPDATENO, CONTENTS, HISTORYWRITEDATE,HISTORYWRITETIME)";
                    SQL = SQL + ComNum.VBLF + " SELECT  " + dblEmrHisNo + ",";
                    SQL = SQL + ComNum.VBLF + "      EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                    SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                    SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML, UPDATENO, CONTENTS, ";
                    SQL = SQL + ComNum.VBLF + "      '" + VB.Left(strCurDateTime, 8) + "',";
                    SQL = SQL + ComNum.VBLF + "      '" + VB.Right(strCurDateTime, 6) + "'";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMRXML";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + strEmrNoOld;
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        return false;
                    }

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " DELETE FROM " + ComNum.DB_EMR + "EMRXML";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + strEmrNoOld;
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        return false;
                    }
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_EMR + "EMRXML";
                SQL = SQL + ComNum.VBLF + "      (EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,UPDATENO) ";
                SQL = SQL + ComNum.VBLF + "      VALUES (";
                SQL = SQL + ComNum.VBLF + "      " + strEmrNoNew + ",";
                SQL = SQL + ComNum.VBLF + "      " + strFormNo + ",";
                SQL = SQL + ComNum.VBLF + "      '" + clsType.User.IdNumber + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strChartDate + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strChartTime + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.acpNo + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.ptNo + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.inOutCls + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.medFrDate + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.medFrTime + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.medEndDate + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.medEndTime + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.medDeptCd + "',";
                SQL = SQL + ComNum.VBLF + "      '" + po.medDrCd + "',";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Left(strCurDateTime, 8) + "',";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Left(strCurDateTime, 6) + "',";
                SQL = SQL + ComNum.VBLF + "      :1,";
                SQL = SQL + ComNum.VBLF + "      '" + strUpdateNo + "')";

                clsDB.ExecuteXmlQuery(SQL, strXml, ref intRowAffected, clsDB.DbCon);

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }
    }
}
