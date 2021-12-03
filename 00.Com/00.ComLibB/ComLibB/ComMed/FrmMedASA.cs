using ComBase;
using ComEmrBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : FrmMedASA.cs
    /// Description     : 마취 신체등급(ASA)
    /// Author          : 안정수
    /// Create Date     : 2017-11-28
    /// Update History  : 
    /// TODO : SaveErInfoXML 함수에서 데이터 생성부분
    /// <history>       
    /// d:\psmh\Ocs\Frm마취신체등급_ASA.frm(Frm마취신체등급_ASA) => FrmMedASA.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\Ocs\Frm마취신체등급_ASA.frm(Frm마취신체등급_ASA)
    /// </seealso>
    /// </summary>
    public partial class FrmMedASA : Form
    {
        clsSpread CS = new clsSpread();

        string SQL = "";
        string SqlErr = "";
        DataTable dt = null;
        //int intRowAffected = 0;

        int mnJobSabun = 0;

        public FrmMedASA()
        {
            InitializeComponent();
            setEvent();
        }

        public FrmMedASA(int GnJobSabun)
        {
            InitializeComponent();
            setEvent();
            mnJobSabun = GnJobSabun;
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnOk.Click += new EventHandler(eBtnEvent);
            this.btnExit.Click += new EventHandler(eBtnEvent);            
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }

            else if (sender == this.btnOk)
            {
                //
                //if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                //{
                //    return; //권한 확인
                //}
                btnOk_Click();
            }         
        }

        void eFormLoad(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}

            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            READ_ASA_Code();
        }

        void READ_ASA_Code()
        {
            int i = 0;
            int nREAD = 0;
            string strOK = "";
            int nOK = 0;

            CS.Spread_All_Clear(ssList);

            cboASA.Items.Clear();
            cboASA.Items.Add("");

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  NAME,CODE ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND Gubun ='마취_신체등급(ASA)' ";
            SQL += ComNum.VBLF + "      AND (DelDate IS NULL OR DelDate ='')";
            SQL += ComNum.VBLF + "ORDER BY CODE";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    nREAD = dt.Rows.Count;
                    ssList.ActiveSheet.Rows.Count = nREAD;

                    strOK = "";

                    for(i = 0; i < nREAD; i++)
                    {
                        ssList.ActiveSheet.Cells[i, 0].Text = "Class " + dt.Rows[i]["Code"].ToString().Trim();
                        ssList.ActiveSheet.Cells[i, 1].Text = "Class " + dt.Rows[i]["Name"].ToString().Trim();

                        if (clsPublic.Gstr마취신체등급 != "")
                        {
                            if (clsPublic.Gstr마취신체등급 == dt.Rows[i]["Code"].ToString().Trim())
                            {
                                nOK = i;
                                strOK = "OK";
                            }
                        }

                        cboASA.Items.Add(dt.Rows[i]["Code"].ToString().Trim() + ".(Class " 
                                                                              + dt.Rows[i]["Code"].ToString().Trim() + ") "
                                                                              + dt.Rows[i]["Name"].ToString().Trim());
                    }
                }
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt.Dispose();
            dt = null;

            if (strOK == "OK")
            {
                cboASA.SelectedIndex = nOK + 1;
            }
            else
            {
                cboASA.SelectedIndex = 0;
            }
        }

        void btnOk_Click()
        {
            clsPublic.Gstr마취신체등급 = "";

            if(cboASA.SelectedItem.ToString().Trim() != "")
            {
                clsPublic.Gstr마취신체등급 = VB.Left(cboASA.SelectedItem.ToString().Trim(), 1);
            }

            SaveErInfoNEW(cboASA.SelectedItem.ToString().Trim());
            //SaveErInfoXML(cboASA.SelectedItem.ToString().Trim());

            this.Close();
        }

        void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            cboASA.SelectedIndex = e.Row + 1;
        }

        void SaveErInfoNEW(string arg)
        {
            string strChartDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
            string strChartTime = ComQuery.CurrentDateTime(clsDB.DbCon, "T");

            double dblEmrNo = 0;

            arg = "미국마취과학회 신체등급(ASA)" + ComNum.VBLF + arg;

            EmrForm pForm = clsEmrChart.SerEmrFormUpdateNo(clsDB.DbCon, "963");

            if (clsOrdFunction.GstrGbJob == "OPD")
            {
                //EmrPatient pAcp = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, clsOrdFunction.Pat.PtNo, clsOrdFunction.Pat.GbIO, clsOrdFunction.Pat.INDATE.Replace("-", ""), clsOrdFunction.Pat.DeptCode);
                //2021-01-01 변경
                EmrPatient pAcp = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, clsOrdFunction.Pat.PtNo, "O", strChartDate.Replace("-", ""), clsOrdFunction.Pat.DeptCode);
                if (pAcp == null)
                {
                    ComFunc.MsgBoxEx(this, "접수내역을 찾을 수 없습니다.");
                    return;
                }

                if (clsEmrQuery.SaveNewProgressEx(clsDB.DbCon, this, pAcp, 0, arg, ref dblEmrNo, strChartDate, strChartTime) == false)
                {
                    ComFunc.MsgBoxEx(this, "경과기록지 저장도중 에러가 발생했습니다.");
                }
            }
            else
            {
                EmrPatient pAcp = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, clsOrdFunction.Pat.PtNo, clsOrdFunction.Pat.GbIO, clsOrdFunction.Pat.INDATE.Replace("-", ""), clsOrdFunction.Pat.DeptCode);
                if (pAcp == null)
                {
                    ComFunc.MsgBoxEx(this, "접수내역을 찾을 수 없습니다.");
                    return;
                }

                if (clsEmrQuery.SaveNewProgressEx(clsDB.DbCon, this, pAcp, 0, arg, ref dblEmrNo, strChartDate, strChartTime) == false)
                {
                    ComFunc.MsgBoxEx(this, "경과기록지 저장도중 에러가 발생했습니다.");
                }
            }
        }


        #region 이전 함수
        //void SaveErInfoXML(string arg)
        //{
        //    string strPtNO = "";
        //    string strSName = "";
        //    string strDeptCode = "";
        //    string strDrCode = "";
        //    string strBDATE = "";

        //    double dblEmrNo = 0;
        //    string strHead = "";
        //    string strChartX1 = "";
        //    string strChartX2 = "";
        //    string strXML = "";
        //    string strXMLCert = "";
        //    string strTagHead = "";
        //    string strTagTail = "";
        //    string strTagVal = "";

        //    char keyChar = '"';

        //    strXML = "";

        //    arg = "미국마취과학회 신체등급(ASA)" + ComNum.VBLF + arg;

        //    strHead = "<?xml version=" + VB.Chr(34) + "1.0" + VB.Chr(34) + " encoding=" + VB.Chr(34) + "UTF-8" + VB.Chr(34) + "?>";
        //    strChartX1 = "<chart>";
        //    strChartX2 = "</chart>";

        //    strXML = strHead + strChartX1;

        //    strTagHead = "<ta1 type=" + keyChar + "textArea" + keyChar + " label=" + keyChar + "Progress" + keyChar + "><![CDATA[";
        //    strTagVal = arg;
        //    strTagTail = "]]></ta1>";

        //    strXML += strTagHead + strTagVal + strTagTail;
        //    strXML += strChartX2;

        //    strXMLCert = strXML;

        //    SQL = "";
        //    SQL += ComNum.VBLF + "SELECT KOSMOS_EMR.GetEmrXmlNo() FunSeqNo FROM Dual";

        //    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

        //    if (SqlErr != "")
        //    {
        //        ComFunc.MsgBox("조회중 문제가 발생했습니다");
        //        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
        //        return;
        //    }

        //    if (dt.Rows.Count > 0)
        //    {
        //        dblEmrNo = VB.Val(dt.Rows[0]["FunSeqNo"].ToString().Trim());
        //    }

        //    dt.Dispose();
        //    dt = null;

        //    string strChartDate = "";
        //    string strChartTime = "";

        //    SQL = "";
        //    SQL += ComNum.VBLF + "SELECT ";
        //    SQL += ComNum.VBLF + "  TO_CHAR(SYSDATE,'YYYYMMDD') AS CURRENTDATE,";
        //    SQL += ComNum.VBLF + "  TO_CHAR(SYSDATE,'HH24MISS') AS CURRENTTIME";
        //    SQL += ComNum.VBLF + "FROM DUAL";

        //    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

        //    if (SqlErr != "")
        //    {
        //        ComFunc.MsgBox("조회중 문제가 발생했습니다");
        //        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
        //        return;
        //    }

        //    if (dt.Rows.Count > 0)
        //    {
        //        strChartDate = dt.Rows[0]["CURRENTDATE"].ToString().Trim();
        //        strChartTime = dt.Rows[0]["CURRENTTIME"].ToString().Trim();
        //    }

        //    dt.Dispose();
        //    dt = null;

        //    //면허번호로 의사코드 가져오기
        //    string strDrCd = "";

        //    SQL = "";
        //    SQL += ComNum.VBLF + "SELECT * ";
        //    SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR";
        //    SQL += ComNum.VBLF + "WHERE 1=1";
        //    if (mnJobSabun == 0)
        //    {
        //        SQL += ComNum.VBLF + "      AND DOCCODE = " + clsPublic.GnJobSabun;
        //    }
        //    else
        //    {
        //        SQL += ComNum.VBLF + "      AND DOCCODE = " + mnJobSabun;
        //    }

        //    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

        //    if (SqlErr != "")
        //    {
        //        ComFunc.MsgBox("조회중 문제가 발생했습니다");
        //        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
        //        return;
        //    }

        //    if(dt.Rows.Count > 0)
        //    {
        //        strDrCd = dt.Rows[0]["DRCODE"].ToString().Trim();
        //    }

        //    dt.Dispose();
        //    dt = null;

        //    //TODO : 데이터 생성부분...
        ////    Dim Conn As ADODB.Connection
        ////    Dim cmd As ADODB.Command

        ////    Set cmd = New ADODB.Command
        ////    With cmd
        ////     .ActiveConnection = adoConnect

        ////     .CommandText = "KOSMOS_EMR.XMLINSRT3"
        ////     .CommandType = adCmdStoredProc

        ////     .Parameters.Append.CreateParameter("p_EMRNO", adDouble, adParamInput, 0, dblEmrNo)
        ////     .Parameters.Append.CreateParameter("p_FORMNO", adDouble, adParamInput, 0, Val(963))
        ////     .Parameters.Append.CreateParameter("p_USEID", adVarChar, adParamInput, 8, GnJobSabun)
        ////     .Parameters.Append.CreateParameter("p_CHARTDATE", adVarChar, adParamInput, 8, strChartDate)
        ////     .Parameters.Append.CreateParameter("p_CHARTTIME", adVarChar, adParamInput, 6, strChartTime)
        ////     .Parameters.Append.CreateParameter("p_ACPNO", adDouble, adParamInput, 0, 0)
        ////     .Parameters.Append.CreateParameter("p_PTNO", adVarChar, adParamInput, 9, Pat.PtNo)
        ////        If UCase(App.EXEName) = "MTSIORDER" Then
        ////           .Parameters.Append.CreateParameter("p_INOUTCLS", adVarChar, adParamInput, 1, "I")
        ////        Else
        ////            .Parameters.Append.CreateParameter("p_INOUTCLS", adVarChar, adParamInput, 1, "O")
        ////        End If
        ////        .Parameters.Append.CreateParameter("p_MEDFRDATE", adVarChar, adParamInput, 8, IIf(Trim(Pat.INDATE) = "", Replace(GstrSysDate, "-", ""), Replace(Pat.INDATE, "-", "")))
        ////        .Parameters.Append.CreateParameter("p_MEDFRTIME", adVarChar, adParamInput, 6, "120000")
        ////        .Parameters.Append.CreateParameter("p_MEDENDDATE", adVarChar, adParamInput, 8, "")
        ////        .Parameters.Append.CreateParameter("p_MEDENDTIME", adVarChar, adParamInput, 6, "")
        ////        .Parameters.Append.CreateParameter("p_MEDDEPTCD", adVarChar, adParamInput, 4, Trim(Pat.DeptCode))
        ////        .Parameters.Append.CreateParameter("p_MEDDRCD", adVarChar, adParamInput, 6, Trim(Pat.DrCode))
        ////        .Parameters.Append.CreateParameter("p_MIBICHECK", adVarChar, adParamInput, 1, "0")
        ////        .Parameters.Append.CreateParameter("p_WRITEDATE", adVarChar, adParamInput, 8, strChartDate)
        ////        .Parameters.Append.CreateParameter("p_WRITETIME", adVarChar, adParamInput, 6, strChartTime)
        ////        .Parameters.Append.CreateParameter("p_UPDATENO", adSmallInt, adParamInput, 0, 1)
        ////        .Parameters.Append.CreateParameter("p_CHARTXML", adLongVarChar, adParamInput, Len(strXML), strXML)

        ////            .Execute
        ////     End With
        ////'
        ////    If Result <> 0 Then
        ////        MsgBox "경과기록지 생성 중 에러가 발생하였습니다.", vbInformation, "확인"
        ////    End If

        //}
        #endregion
    }
}
