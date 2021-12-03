using ComBase;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace ComEmrBase
{
    public static class clsOldChart
    {
        /// <summary>
        /// 기록내용을 화면에 뿌려준다(정형외과 S.O.A.P)
        /// </summary>
        /// <param name="frmXmlForm">기록지폼</param>
        /// <param name="strEmrNo"></param>
        public static void LoadDataXMLOldChart1963(Control frmXmlForm, string strEmrNo)
        {
            XmlDocument Doc = new XmlDocument();
            OracleDataReader reader = null;

            //try
            //{
            string SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT CHARTXML";
            SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "EMRXML";  //VIEWEMRXML
            SQL = SQL + ComNum.VBLF + "    WHERE EMRNO = " + VB.Val(strEmrNo);

            string SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
            if (reader.HasRows == false)
            {
                reader.Dispose();
                return;
            }

            reader.Read();

            string strXml = reader.GetValue(0).ToString().Trim();
            strXml = strXml.Replace("type = \"image\" label = \"undefined\"", "type = \"image\" label = \"PictureBox\"");
            strXml = strXml.Replace("undefined", "");
            reader.Dispose();

            Doc.LoadXml(strXml);

            SetUserXmlValueOldChart1963(frmXmlForm, Doc);
            //}
            //catch (Exception ex)
            //{
            //    ComFunc.MsgBox(ex.Message);
            //}
        }

        /// <summary>
        /// 정형외과 S.O.A.P 연속보기에서 데이터 뿌려줌.
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="Doc"></param>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        public static void SetUserXmlValueOldChart1963(Control ctl, XmlDocument Doc)
        {
            XmlNodeList nodeList = Doc.SelectNodes("chart");
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            string strValue = string.Empty;
            foreach (XmlNode node in nodeList)
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    string strName = childNode.Name.ToString();
                    strValue = (childNode.InnerText.ToString() + "").ToString();
                    string strType = childNode.Attributes.GetNamedItem("type").Value;

                    dictionary.Add(strName, strValue);
                }
            }

            Control[] tx = ctl.Controls.Find("Content", true);
            if (tx.Length <= 0)
            {
                return;
            }

            RichTextBox obj = (RichTextBox)tx[0];
            string strTitle = string.Empty;
            strValue = string.Empty;

            #region ta1~4 순서대로
            if (dictionary.TryGetValue("ta1", out strValue))
            {
                if(strValue.Length > 0)
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
            }

            if (dictionary.TryGetValue("ta2", out strValue))
            {
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
            }

            if (dictionary.TryGetValue("ta3", out strValue))
            {
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
            }

            if (dictionary.TryGetValue("ta4", out strValue))
            {
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
            }

            #endregion
            //}
            //catch (Exception ex)
            //{
            //    ComFunc.MsgBox(ex.Message);
            //}
        }

        /// <summary>
        /// 기록내용을 화면에 뿌려준다
        /// </summary>
        /// <param name="frmXmlForm">기록지폼</param>
        /// <param name="strEmrNo"></param>
        /// <param name="blnOption"> true : 에러 표시, false : 에러 무시</param>
        /// <param name="OldYn">이전내역일 경우</param>
        /// <param name="dtp">작성일자</param>
        /// <param name="cb">작성시간</param>
        public static void LoadDataXMLOldChart(Control frmXmlForm, string strEmrNo, bool blnErrOption, bool OldYn, DateTimePicker dtp, ComboBox cb)
        {
            XmlDocument Doc = new XmlDocument();

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            string strXml = "";
            string strFormNo = "0";
            string strUpdateNo = "0";

            //try
            //{
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT FORMNO, UPDATENO, CHARTXML, CHARTDATE, CHARTTIME ";
                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "EMRXML";  //VIEWEMRXML
                SQL = SQL + ComNum.VBLF + "    WHERE EMRNO = " + VB.Val(strEmrNo);

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }

                strXml = (dt.Rows[0]["CHARTXML"].ToString()).Trim();
                strXml = strXml.Replace("type = \"image\" label = \"undefined\"", "type = \"image\" label = \"PictureBox\"");
                strXml = strXml.Replace("undefined", "");
                if (OldYn == true)
                {
                    strFormNo = dt.Rows[0]["FORMNO"].ToString().Trim();
                    //strUpdateNo = dt.Rows[0]["UPDATENO"].ToString().Trim();
                    if(dtp != null)
                    {
                        dtp.Value = Convert.ToDateTime(ComFunc.FormatStrToDate((dt.Rows[0]["CHARTDATE"].ToString()).Trim(), "D"));
                        if(clsType.User.AuAMANAGE != "1")
                        {
                            //dtp.Enabled = false;

                        }
                    }

                    if (cb != null)
                    {   
                        cb.Text = ComFunc.FormatStrToDate((dt.Rows[0]["CHARTTIME"].ToString()).Trim(), "M");
                    }
                }
                dt.Dispose();
                dt = null;

                Doc.LoadXml(strXml);

                SetUserXmlValueOldChart(frmXmlForm, Doc, strFormNo, strUpdateNo);
            //}
            //catch (Exception ex)
            //{
            //    ComFunc.MsgBox(ex.Message);
            //}
        }

        /// <summary>
        /// 경과이미지를 화면에 뿌려준다
        /// </summary>
        /// <param name="frmXmlForm">기록지폼</param>
        /// <param name="strEmrNo"></param>
        /// <param name="blnOption"> true : 에러 표시, false : 에러 무시</param>
        /// <param name="OldYn">이전내역일 경우</param>
        /// <param name="dtp">작성일자</param>
        /// <param name="cb">작성시간</param>
        public static void LoadImageChart(Control frmXmlForm, string strEmrNo, bool blnErrOption, bool OldYn, DateTimePicker dtp, ComboBox cb)
        {
            string SqlErr = string.Empty; //에러문 받는 변수
       
            OracleDataReader reader = null;
            Image tmpImg = null;

            try
            {
                string SQL = string.Empty;
                SQL += ComNum.VBLF + "SELECT CHARTDATE, CHARTTIME, EMRIMAGEMERGE";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMRXMLIMAGES";
                SQL += ComNum.VBLF + "WHERE EMRNO = " + VB.Val(strEmrNo);


                SQL += ComNum.VBLF + "UNION ALL";
                SQL += ComNum.VBLF + "SELECT CHARTDATE, CHARTTIME, EMRIMAGEMERGE";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMRXMLIMAGES";
                SQL += ComNum.VBLF + "WHERE EMRNO = " + VB.Val(strEmrNo);

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    return;
                }

                if (reader.HasRows && reader.Read())
                {
                    if (dtp != null)
                    {
                        dtp.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(reader.GetValue(0).ToString().Trim(), "D"));
                        dtp.Enabled = false;
                    }

                    if (cb != null)
                    {
                        cb.Text = ComFunc.FormatStrToDate((reader.GetValue(0).ToString().Trim()).Trim(), "T");
                    }


                    byte[] buffer = (byte[])reader.GetValue(2);
                    using (MemoryStream memStream = new MemoryStream(buffer))
                    {
                        tmpImg = Image.FromStream(memStream, true);
                    }

                }

                reader.Dispose();

                Control[] controls = frmXmlForm.Controls.Find("img", true);
                if(controls.Length > 0)
                {
                    ((PictureBox)controls[0]).Image = tmpImg;
                    ((PictureBox)controls[0]).SizeMode = PictureBoxSizeMode.AutoSize;
                    frmXmlForm.Height = controls[0].Height + 10;
                }


            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx((Form) frmXmlForm, ex.Message);
            }
        }

        public static void LoadDataXMLOldChartEx(Control frmXmlForm, string strEmrNo, bool blnErrOption)
        {
            XmlDocument Doc = new XmlDocument();

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            string strXml = "";
            string strFormNo = "0";
            string strUpdateNo = "0";

            //try
            //{
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT FORMNO, CHARTXML, CHARTDATE, CHARTTIME ";
                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "EMRXML";
                SQL = SQL + ComNum.VBLF + "    WHERE EMRNO = " + VB.Val(strEmrNo);
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")       
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }

                strXml = dt.Rows[0]["CHARTXML"].ToString().Trim().Replace(">undefined<", "><");
                strFormNo = dt.Rows[0]["FORMNO"].ToString().Trim();
                //strUpdateNo = dt.Rows[0]["UPDATENO"].ToString().Trim();
                
                dt.Dispose();
                dt = null;

                Doc.LoadXml(strXml);

                SetUserXmlValueOldChart(frmXmlForm, Doc, strFormNo, strUpdateNo);
            //}
            //catch (Exception ex)
            //{
            //    ComFunc.MsgBox(ex.Message);
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="strName"></param>
        /// <param name="strValue"></param>
        /// <param name="strType"></param>
        private static void FixOldFormItemWrong(string strFormNo , string strUpdateNo, ref string strName, ref string strValue, ref string strType)
        {
            if (strFormNo == "2486")
            {
                if (strName == "ik72") strType = "inputCheck"; //inputText
                if (strName == "it136") strType = "inputText"; //inputCheck
                if (strName == "it137") strType = "inputText"; //inputCheck
            }
            if (strFormNo == "2678")
            {
                if (strName == "dt1") strType = "inputDate"; //inputCheck
                if (strName == "ir3") strType = "inputRadio"; //inputCheck
                if (strName == "ir7") strType = "inputRadio"; //inputCheck
            }
        }

        /// <summary>
        /// 잘못된 컨트롤 되돌리기 : 저장할 경우 다시 되돌려야 함..
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="Doc"></param>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        private static void SetUserXmlValueOldChart(Control ctl, XmlDocument Doc, string strFormNo, string strUpdateNo)
        {
            XmlNodeList nodeList = Doc.SelectNodes("chart");

            //try
            //{
            foreach (XmlNode node in nodeList)
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    string strName = "";
                    string strValue = "";
                    string strType = "";

                    //19-08-22 경과기록지 폼 변경으로 수정함.
                    if(ctl.Name == "frmEmrForm_Progress_New")
                    {
                        strName = "txtProgress";
                    }
                    else
                    {
                        strName = childNode.Name.ToString();
                    }
                    strType = childNode.Attributes.GetNamedItem("type").Value;
                    strValue = (childNode.InnerText.ToString() + "").ToString();
                    Control[] tx = null;
                    Control obj = null;
                    string strRtype = "";
                    tx = ctl.Controls.Find(strName, true);
                    if (tx.Length <= 0)
                    {
                        continue; 
                        //ComFunc.MsgBox("컨트롤이 존재하지 않습니다.");
                        //return;
                    }

                    strRtype = tx[0].GetType().ToString();
                    strRtype = strRtype.Replace("System.Windows.Forms.", "");

                    if (strRtype == "CheckBox")
                    {
                        obj = (CheckBox)tx[0];
                        if (strValue == "true")
                        {
                            ((CheckBox)obj).Checked = true;
                        }
                        else
                        {
                            ((CheckBox)obj).Checked = false;
                        }
                    }
                    else if (strRtype == "RadioButton")
                    {
                        obj = (RadioButton)tx[0];
                        if (strValue == "true")
                        {
                            ((RadioButton)obj).Checked = true;
                        }
                        else
                        {
                            ((RadioButton)obj).Checked = false;
                        }
                    }
                    else if (strRtype == "PictureBox")
                    {
                        //기본이미지는 서식 생성시 이미 들어가 있음
                        //string strFormImage = childNode.Attributes.GetNamedItem("imgNo").Value;   //필요없음
                        if (childNode.Attributes.GetNamedItem("emrimageno") != null)
                        {
                            
                            string strSrc = childNode.Attributes.GetNamedItem("src").Value;
                            string strEmrimageno = childNode.Attributes.GetNamedItem("emrimageno").Value;
                            if (strSrc.IndexOf("noimage.png") > 0)
                            {
                                obj = (PictureBox)tx[0];
                                if (((PictureBox)obj).Image == null)
                                {
                                    obj.BackColor = Color.LightGray;
                                    ((PictureBox)obj).Image = Properties.Resources.noimage;
                                    ((PictureBox)obj).SizeMode = PictureBoxSizeMode.Zoom;
                                }
                                //이미지 없음
                            }
                            else
                            {
                                //Image pic = clsEmrQueryOld.GetImage_EMRXMLIMAGES(strEmrimageno);
                                Image pic = clsEmrQueryOld.GetImage_EMRXMLIMAGES_Ex(strEmrimageno);

                                obj = (PictureBox)tx[0];
                                obj.BackColor = Color.White;
                                ((PictureBox)obj).Image = pic;
                                ((PictureBox)obj).SizeMode = PictureBoxSizeMode.Zoom;
                                ((PictureBox)obj).Tag = strEmrimageno;
                            }

                        }
                        //string strSrc = childNode.Attributes.GetNamedItem("src").Value;
                        //string strEmrImage = childNode.Attributes.GetNamedItem("emrimageno").Value;
                        //string strFormImage = childNode.Attributes.GetNamedItem("imgNo").Value;
                        ////emrimageno : 작성된 이미지 번호 : ADMIN.EMRXMLIMAGES
                        ////ftp Path : /emr1/mento/tomcat/webapps/Emr/images/mts/emrimages
                        ////imgNo : 기본이미지 : ADMIN.EMRFORMIMAGES
                        ////ftp Path : /emr1/mento/tomcat/webapps/Emr/images/mts/formimages

                        //if (strSrc.IndexOf("emrimageno") > 0)
                        //{
                        //    //TODO 작성된 이미지를 불러 온다
                        //    string strEmrimageno = childNode.Attributes.GetNamedItem("emrimageno").Value;
                        //    obj = (PictureBox)tx[0];
                        //    ((PictureBox)obj).Image = null;
                        //    ((PictureBox)obj).SizeMode = PictureBoxSizeMode.StretchImage;
                        //    //obj.Text = VB.Replace(strValue, "\n", "\r\n", 1, -1);
                        //}
                    }
                    else
                    {
                        obj = (TextBox)tx[0];
                        obj.Text = VB.Replace(strValue, "\n", "\r\n", 1, -1);
                        if(((TextBox)obj).Multiline == true)
                        {
                            //TODO 텍스트 박스 높이 조절 : 여기서 하기 힘들 듯
                        }
                    }
                    
                    #region // 이전 루틴 : 주석
                    //FixOldFormItemWrong(strFormNo, strUpdateNo, ref strName , ref strValue , ref strType);
                    //if (strType == "inputText")
                    //{
                    //    tx = ctl.Controls.Find(strName, true);
                    //    if (tx.Length > 0)
                    //    {
                    //        obj = (TextBox)tx[0];
                    //        obj.Text = VB.Replace(strValue, "\n", "\r\n", 1, -1);
                    //    }
                    //}
                    //else if (strType == "textArea")
                    //{
                    //    tx = ctl.Controls.Find(strName, true);
                    //    if (tx.Length > 0)
                    //    {
                    //        obj = (TextBox)tx[0];
                    //        obj.Text = VB.Replace(strValue, "\n", "\r\n", 1, -1);
                    //    }
                    //}
                    //else if (strType == "inputDate")
                    //{
                    //    tx = ctl.Controls.Find(strName, true);
                    //    if (tx.Length > 0)
                    //    {
                    //        obj = (TextBox)tx[0];
                    //        obj.Text = VB.Replace(strValue, "\n", "\r\n", 1, -1);
                    //    }
                    //}
                    //else if (strType == "inputCheck")
                    //{
                    //    tx = ctl.Controls.Find(strName, true);
                    //    if (tx.Length > 0)
                    //    {
                    //        obj = (CheckBox)tx[0];
                    //        if (strValue == "true")
                    //        {
                    //            ((CheckBox)obj).Checked = true;
                    //        }
                    //        else
                    //        {
                    //            ((CheckBox)obj).Checked = false;
                    //        }
                    //    }
                    //}
                    //else if (strType == "inputRadio")
                    //{
                    //    tx = ctl.Controls.Find(strName, true);
                    //    if (tx.Length > 0)
                    //    {
                    //        obj = (RadioButton)tx[0];
                    //        if (strValue == "true")
                    //        {
                    //            ((RadioButton)obj).Checked = true;
                    //        }
                    //        else
                    //        {
                    //            ((RadioButton)obj).Checked = false;
                    //        }
                    //    }
                    //}
                    #endregion // 이전 루틴 : 주석

                }
            }
            //}
            //catch (Exception ex)
            //{
            //    ComFunc.MsgBox(ex.Message);
            //}
        }


        /// <summary>
        /// 폼서식을 저장한다. - VB6에서 컨버젼
        /// </summary>
        /// <param name="frmXmlForm"></param>
        /// <param name="isSpcPanel"></param>
        /// <param name="pControl"></param>
        /// <returns></returns>
        public static string SaveDataToXmlOld(Form frmXmlForm, bool isSpcPanel, Control pControl)
        {
            string rtnVal = "";
            string strXML = "";
            string strConIndex = "";

            string strTextType = "inputText";

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


                foreach (Control objControl in controls)
                {
                    strConIndex = "";
                    //strConIndex = clsXML.IsArryCon(objControl);

                    strTextType = "inputText";

                    //TextBox
                    if (objControl is TextBox)
                    {
                        if (objControl.Name.Substring(0, 2).Trim() == "dt")
                        {
                            strTextType = "inputDate";
                        }
                        else if (objControl.Name.Substring(0, 2).Trim() == "ta")
                        {
                            strTextType = "textArea";
                        }
                        else
                        {
                            strTextType = "inputText";
                        }
                        //strTextType = objControl.Name.Substring(0, 2).Trim();  
                        strXML = strXML + "<" + objControl.Name + " type=\"" + strTextType + "\"" + " Index=\"" + strConIndex + "\"><![CDATA[" + VB.Trim(objControl.Text) + "]]></" + objControl.Name + ">";
                    }

                    //CheckBox
                    if (objControl is CheckBox)
                    {
                        strXML = strXML + "<" + objControl.Name + " type=\"inputCheck\"" + "\"><![CDATA[" + (((CheckBox)objControl).Checked == true ? "true" : "false") + "]]></" + objControl.Name + ">";
                    }

                    //RadioButton
                    if (objControl is RadioButton)
                    {
                        strXML = strXML + "<" + objControl.Name + " type=\"inputRadio\"" + "\"><![CDATA[" + (((RadioButton)objControl).Checked == true ? "true" : "false") + "]]></" + objControl.Name + ">";
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
    }
}
