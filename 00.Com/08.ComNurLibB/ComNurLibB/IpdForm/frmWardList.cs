using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmWardList.cs
    /// Description     : 병동현황
    /// Author          : 박창욱
    /// Create Date     : 2018-02-28
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// TODO : 전역변수 - GstrHelpCode
    /// </history>
    /// <seealso cref= "\frm병동현황.frm(Frm병동현황.frm) >> frmWardList.cs 폼이름 재정의" />	
    public partial class frmWardList : Form
    {
        int nTotTBed = 0;
        int nTotGBed = 0;
        int nTotHBed = 0;
        int nTotOBed = 0; //퇴원수속중
        string FstrPanoList = "";
        string strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        public frmWardList()
        {
            InitializeComponent();
        }
        
        string mstrWard = "";

        public frmWardList(string strWard)
        {
            InitializeComponent();

            mstrWard = strWard;
        }

        void Pano_List_Set()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strOldData = "";
            string strNewData = "";
            string strGbSTS = "";
            string strDAT = "";
            string strOK = "";


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT WardCode,RoomCode,Pano,SName,Age,DeptCode,GbSTS,";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(InDate,'YYYY-MM-DD')  InDate,";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(OutDate,'YYYY-MM-DD') OutDate";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER";
                SQL = SQL + ComNum.VBLF + "  WHERE ActDate IS NULL";
                SQL = SQL + ComNum.VBLF + "    AND PANO NOT IN ('81000004')";
                switch (cboWard.Text.Trim())
                {
                    case "SICU":
                        SQL = SQL + ComNum.VBLF + "    AND RoomCode = '233'";
                        break;
                    case "MICU":
                        SQL = SQL + ComNum.VBLF + "    AND RoomCode = '234'";
                        break;
                    case "ND":
                        SQL = SQL + ComNum.VBLF + "    AND WardCode IN ('ND','IQ')";
                        break;
                    case "전체":
                        SQL = SQL + ComNum.VBLF + "    AND WardCode > ' '";
                        break;
                    case "":
                        SQL = SQL + ComNum.VBLF + "    AND WardCode > ' '";
                        break;
                    default:
                        SQL = SQL + ComNum.VBLF + "    AND WardCode = '" + cboWard.Text.Trim() + "'";
                        break;
                }
                SQL = SQL + ComNum.VBLF + "  ORDER BY WardCode,RoomCode,SName";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    return;
                }

                FstrPanoList = "";
                strOldData = "";

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strGbSTS = dt.Rows[i]["GbSTS"].ToString().Trim();
                    strOK = "OK";
                    if (strGbSTS == "!" || strGbSTS == "7" || strGbSTS == "9")
                    {
                        strOK = "NO";
                    }
                    if (dt.Rows[i]["OutDate"].ToString().Trim() != "" && Convert.ToDateTime(dt.Rows[i]["OutDate"].ToString().Trim()) < Convert.ToDateTime(strSysDate))
                    {
                        strOK = "NO";
                    }
                    if (dt.Rows[i]["Pano"].ToString().Trim() == "81000004")
                    {
                        strOK = "NO";
                    }

                    if (strOK == "OK")
                    {
                        strNewData = dt.Rows[i]["WardCode"].ToString().Trim() + "-";
                        strNewData += dt.Rows[i]["RoomCode"].ToString().Trim();
                        if (strOldData != strNewData)
                        {
                            FstrPanoList += "{{@}}" + strNewData + "{@}";
                            strOldData = strNewData;
                        }

                        if (dt.Rows[i]["InDate"].ToString().Trim() == strSysDate)
                        {
                            strDAT = "1@";
                            strDAT += dt.Rows[i]["DeptCode"].ToString().Trim() + ",";
                            strDAT += dt.Rows[i]["SName"].ToString().Trim() + "*,";
                            strDAT += dt.Rows[i]["Age"].ToString().Trim();
                        }
                        else if (dt.Rows[i]["OutDate"].ToString().Trim() == strSysDate)
                        {
                            strDAT = "2@";
                            strDAT += dt.Rows[i]["DeptCode"].ToString().Trim() + ",";
                            strDAT += dt.Rows[i]["SName"].ToString().Trim() + "*,";
                            strDAT += dt.Rows[i]["Age"].ToString().Trim();
                        }
                        else
                        {
                            strDAT = "0@";
                            strDAT += dt.Rows[i]["DeptCode"].ToString().Trim() + ",";
                            strDAT += dt.Rows[i]["SName"].ToString().Trim() + "*,";
                            strDAT += dt.Rows[i]["Age"].ToString().Trim();
                        }
                        FstrPanoList += strDAT + "{}";
                    }
                }

                FstrPanoList += "{{@}}";

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void SSBuildMain()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nRowCnt = 0;
            int nCount = 0;
            int nTBed = 0;
            int nHBed = 0;
            int nGBed = 0;
            int nTTBed = 0;
            int nTHBed = 0;
            int nTGBed = 0;

            string strWard = "";
            string SaveWard = "";
            string strRoom = "";
            string strClass = "";
            string strSex = "";
            
            FarPoint.Win.ComplexBorder complexBorder1 = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),      //LEFT
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),      //TOP
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),      //RIGHT
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),      //BOTTOM
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            FarPoint.Win.ComplexBorder complexBorder2 = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),      //LEFT
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),      //TOP
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),      //RIGHT
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),      //BOTTOM
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            FarPoint.Win.ComplexBorder complexBorder3 = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),      //LEFT
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),      //TOP
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),      //RIGHT
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),      //BOTTOM
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            FarPoint.Win.ComplexBorder complexBorder4 = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),      //LEFT
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),      //TOP
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),      //RIGHT
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),      //BOTTOM
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            ssView_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT WardCode, RoomCode, RoomClass, Sex, TBed, HBed, (TBed - Hbed ) nGBed";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ROOM";
                SQL = SQL + ComNum.VBLF + "  WHERE TBed > 0";
                switch (cboWard.Text.Trim())
                {
                    case "SICU":
                        SQL = SQL + ComNum.VBLF + "    AND RoomCode = '233'";
                        break;
                    case "MICU":
                        SQL = SQL + ComNum.VBLF + "    AND RoomCode = '234'";
                        break;
                    case "ND":
                        SQL = SQL + ComNum.VBLF + "    AND WardCode IN ('ND','IQ')";
                        break;
                    case "전체":
                        SQL = SQL + ComNum.VBLF + "    AND WardCode > ' '";
                        break;
                    case "":
                        SQL = SQL + ComNum.VBLF + "    AND WardCode > ' '";
                        break;
                    default:
                        SQL = SQL + ComNum.VBLF + "    AND WardCode = '" + cboWard.Text.Trim() + "'";
                        break;
                }
                SQL = SQL + ComNum.VBLF + "  ORDER BY WardCode, RoomCode, RoomClass";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRowCnt = dt.Rows.Count;

                nTotTBed = 0;
                nTotHBed = 0;
                nTotGBed = 0;
                nTotOBed = 0;
                SaveWard = "";

                for (i = 0; i < nRowCnt; i++)
                {
                    nCount += 1;
                    ssView_Sheet1.RowCount = nCount;

                    strWard = dt.Rows[i]["WardCode"].ToString().Trim();
                    strRoom = dt.Rows[i]["RoomCode"].ToString().Trim();
                    if (strRoom == "640" && strWard == "IQ")
                    {
                        strWard = "IQ";
                    }
                    strClass = dt.Rows[i]["RoomClass"].ToString().Trim();
                    strSex = dt.Rows[i]["Sex"].ToString().Trim();
                    nTBed = (int)VB.Val(dt.Rows[i]["TBed"].ToString().Trim());

                    if (nCount == 1)
                    {
                        SaveWard = strWard;
                        ssView_Sheet1.Cells[nCount - 1, 0].Text = strWard;
                        ssView_Sheet1.Cells[nCount - 1, 1].Text = strRoom;
                        ssView_Sheet1.Cells[nCount - 1, 2].Text = strClass;
                        ssView_Sheet1.Cells[nCount - 1, 3].Text = strSex;

                        IPD_SName_Check(strWard, strRoom, nHBed, nGBed, nTBed, ref nCount, ref nTTBed, ref nTHBed, ref nTGBed);
                    }
                    else if (SaveWard != strWard)
                    {
                        #region Change_Ward

                        ssView_Sheet1.Cells[nCount - 1, 0].Text = "SUB";
                        ssView_Sheet1.Cells[nCount - 1, 1].Text = "TOTAL";
                        ssView_Sheet1.Cells[nCount - 1, 4].Text = nTTBed.ToString();
                        ssView_Sheet1.Cells[nCount - 1, 5].Text = nTHBed.ToString();
                        ssView_Sheet1.Cells[nCount - 1, 6].Text = nTGBed.ToString();

                        nCount += 1;
                        ssView_Sheet1.RowCount = nCount;

                        nTTBed = 0;
                        nTHBed = 0;
                        nTGBed = 0;

                        #endregion

                        SaveWard = strWard;

                        ssView_Sheet1.Cells[nCount - 1, 0].Text = strWard;
                        ssView_Sheet1.Cells[nCount - 1, 1].Text = strRoom;
                        ssView_Sheet1.Cells[nCount - 1, 2].Text = strClass;
                        ssView_Sheet1.Cells[nCount - 1, 3].Text = strSex;

                        IPD_SName_Check(strWard, strRoom, nHBed, nGBed, nTBed, ref nCount, ref nTTBed, ref nTHBed, ref nTGBed);
                    }
                    else if (SaveWard == strWard)
                    {
                        ssView_Sheet1.Cells[nCount - 1, 0].Text = strWard;
                        ssView_Sheet1.Cells[nCount - 1, 1].Text = strRoom;
                        ssView_Sheet1.Cells[nCount - 1, 2].Text = strClass;
                        ssView_Sheet1.Cells[nCount - 1, 3].Text = strSex;

                        IPD_SName_Check(strWard, strRoom, nHBed, nGBed, nTBed, ref nCount, ref nTTBed, ref nTHBed, ref nTGBed);
                    }
                }
                dt.Dispose();
                dt = null;

                nCount += 1;
                ssView_Sheet1.RowCount = nCount;

                #region Change_Ward

                ssView_Sheet1.Cells[nCount - 1, 0].Text = "SUB";
                ssView_Sheet1.Cells[nCount - 1, 1].Text = "TOTAL";
                ssView_Sheet1.Cells[nCount - 1, 4].Text = nTTBed.ToString();
                ssView_Sheet1.Cells[nCount - 1, 5].Text = nTHBed.ToString();
                ssView_Sheet1.Cells[nCount - 1, 6].Text = nTGBed.ToString();

                nCount += 1;
                ssView_Sheet1.RowCount = nCount;

                nTTBed = 0;
                nTHBed = 0;
                nTGBed = 0;

                #endregion

                ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, 13].Border = complexBorder1;
                ssView_Sheet1.Cells[0, 14, ssView_Sheet1.RowCount - 1, 14].Border = complexBorder2;

                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    if (ssView_Sheet1.Cells[i, 0].Text.Trim() == "SUB")
                    {
                        ssView_Sheet1.Cells[i - 1, 0, i - 1, 13].Border = complexBorder3;
                        ssView_Sheet1.Cells[i, 0, i, 13].Border = complexBorder3;
                        ssView_Sheet1.Cells[i - 1, 14].Border = complexBorder4;
                        ssView_Sheet1.Cells[i, 14].Border = complexBorder4;
                    }
                }

                #region TotalMessage

                ssView_Sheet1.Cells[nCount - 1, 0].Text = "총";
                ssView_Sheet1.Cells[nCount - 1, 1].Text = "TOTAL";
                ssView_Sheet1.Cells[nCount - 1, 4].Text = nTotTBed.ToString();
                ssView_Sheet1.Cells[nCount - 1, 5].Text = nTotHBed.ToString();
                ssView_Sheet1.Cells[nCount - 1, 6].Text = nTotGBed.ToString();
                ssView_Sheet1.Cells[nCount - 1, 7].Text = "퇴원중:" + nTotOBed;

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, 0, ssView_Sheet1.RowCount - 2, 13].Border = complexBorder3;
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0, ssView_Sheet1.RowCount - 1, 13].Border = complexBorder3;
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, 14].Border = complexBorder4;
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 14].Border = complexBorder4;

                #endregion

                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void IPD_SName_Check(string strWard, string strRoom, int nHBed, int nGBed, int nTBed, ref int nCount, ref int nTTBed, ref int nTHBed, ref int nTGBed)
        {
            int nCNT = 0;
            string strGbnChar = "";
            string strRoomData = "";
            string strGbSTS = "";

            strGbnChar = "{{@}}" + strWard + "-" + strRoom + "{@}";
            strRoomData = VB.Pstr(VB.Pstr(FstrPanoList, strGbnChar, 2), "{{@}}", 1);
            nHBed = (int)VB.L(strRoomData, "{}") - 1;
            nGBed = nTBed - nHBed;

            ssView_Sheet1.Cells[nCount - 1, 4].Text = nTBed.ToString();
            ssView_Sheet1.Cells[nCount - 1, 5].Text = nHBed.ToString();
            ssView_Sheet1.Cells[nCount - 1, 6].Text = nGBed.ToString();

            if (nHBed > 0)
            {
                nCNT = 0;
                for (int i = 1; i <= nHBed; i++)
                {
                    nCNT++;
                    if (nCNT > 8)
                    {
                        nCNT = 1;
                        nCount++;
                        ssView_Sheet1.RowCount = nCount;
                    }
                    
                    ssView_Sheet1.Cells[nCount - 1, 8 + (nCNT - 1) - 1].Text = VB.Pstr(VB.Pstr(strRoomData, "{}", i), "@", 2);
                    strGbSTS = VB.Pstr(VB.Pstr(strRoomData, "{}", i), "@", 1);

                    if (strGbSTS == "2")    //당일퇴원
                    {
                        ssView_Sheet1.Cells[nCount - 1, 8 + (nCNT - 1) - 1].BackColor = Color.FromArgb(192, 192, 255);
                    }
                    else if (strGbSTS == "1")   //당일입원
                    {
                        ssView_Sheet1.Cells[nCount - 1, 8 + (nCNT - 1) - 1].BackColor = Color.FromArgb(255, 128, 255);
                    }
                }
            }

            nTTBed += nTBed;
            nTHBed += nHBed;
            nTGBed += nGBed;
            nTotTBed += nTBed;
            nTotHBed += nHBed;
            nTotGBed += nGBed;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "병동별 병상 가동현황";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("출력시간 : " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A"), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Search_Data();
        }

        void Search_Data()
        {
            int i = 0;
            int nGaCnt = 0;
            int nJaCnt = 0;
            int nRoom = 0;
            string strSex = "";
            string strWard = "";

            Pano_List_Set();
            SSBuildMain();

            for (i = 1; i <= ssView_Sheet1.RowCount; i++)
            {
                strWard = ssView_Sheet1.Cells[i - 1, 0].Text.Trim();
                strSex = ssView_Sheet1.Cells[i - 1, 3].Text.Trim();
                nRoom = (int)VB.Val(ssView_Sheet1.Cells[i - 1, 4].Text);
                nGaCnt = (int)VB.Val(ssView_Sheet1.Cells[i - 1, 5].Text);
                nJaCnt = (int)VB.Val(ssView_Sheet1.Cells[i - 1, 6].Text);

                if (i != ssView_Sheet1.RowCount)
                {
                    if (strSex == "M")
                    {
                        if (nJaCnt > 0 && strWard != "SUB")
                        {
                            ssView_Sheet1.Cells[i - 1, 8 + nGaCnt - 1, i - 1,  7 + nRoom - 1].BackColor = Color.FromArgb(0, 0, 255);
                        }
                    }
                    else if (strSex == "F" && strWard != "SUB")
                    {
                        if (nJaCnt > 0)
                        {
                            ssView_Sheet1.Cells[i - 1, 8 + nGaCnt - 1, i - 1,  7 + nRoom - 1].BackColor = Color.FromArgb(255, 0, 255);
                        }
                    }
                    else
                    {
                        if (nJaCnt > 0 && strWard != "SUB")
                        {
                            ssView_Sheet1.Cells[i - 1, 8 + nGaCnt - 1, i - 1,  7 + nRoom - 1].BackColor = Color.FromArgb(255, 255, 0);
                        }
                    }
                }
            }
        }

        private void frmWardList_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            cboWard.Items.Add("전체");
            clsVbfunc.SetWardCodeCombo(clsDB.DbCon, cboWard, "1", false, 2);
            cboWard.SelectedIndex = 0;

            if (clsPublic.GstrHelpCode != "")
            {
                cboWard.SelectedIndex = cboWard.Items.IndexOf(mstrWard);
                cboWard.Enabled = false;
                Search_Data();
            }

            ssView_Sheet1.Columns[2].Visible = false;

            Search_Data();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
