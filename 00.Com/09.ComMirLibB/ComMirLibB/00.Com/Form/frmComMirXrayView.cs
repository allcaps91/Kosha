using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComMirLibB.Com
{
    /// Class Name      : ComMirLibB.dll
    /// File Name       : frmComMirXrayView.cs
    /// Description     : 방사선 판독 조회
    /// Author          : 박성완
    /// Create Date     : 2017-12-22
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <vbp>
    /// default : VB\PSMHH\mir\XRAYVIEW.FRM
    public partial class frmComMirXrayView : Form
    {
        ComFunc CF = new ComFunc();
        private string FstrPano = "";

        private string FstrPano_B = "";
        private string FstrJinDate_B = "";
        private string FstrOutDate_B = "";
        private string FstrSName_B = "";
        private string FstrBi_B = "";

        string FstrReadROWID = "";

        public frmComMirXrayView()
        {
            InitializeComponent();

            SetEvent();
        }
        public frmComMirXrayView(string GstrPano)
        {
            FstrPano = GstrPano;

            InitializeComponent();

            SetEvent();
        }
        public frmComMirXrayView(string GstrPano_B, string GstrJinDate_B, string GstrOutDate_B, string GstrSName_B, string GstrBi_B)
        {
            FstrPano_B = GstrPano_B;
            FstrJinDate_B = GstrJinDate_B;
            FstrOutDate_B = GstrOutDate_B;
            FstrSName_B = GstrSName_B;
            FstrBi_B = GstrBi_B;

            InitializeComponent();

            SetEvent();
        }

        private void SetEvent()
        {
            this.Load += FrmComMirXrayView_Load;

            this.btnSearch.Click += BtnSearch_Click;
            this.btnExit.Click += BtnExit_Click;
            this.btnPrint.Click += BtnPrint_Click;

            this.ssMain.CellDoubleClick += SsMain_CellDoubleClick;
            this.txtPano.KeyPress += TxtPano_KeyPress;

        }

        private void TxtPano_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 13)
            {
                return;
            }
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            txtPano.Text = txtPano.Text.PadLeft(8, '0');

            SQL = "";
            SQL += "SELECT SNAME, BI FROM KOSMOS_PMPA.BAS_PATIENT ";
            SQL += " WHERE PANO = '" + txtPano.Text + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (dt.Rows.Count > 0)
            {
                lblName.Text = "성명: " + dt.Rows[0]["SNAME"].ToString();
                lblBi.Text = "종류: " + dt.Rows[0]["BI"].ToString();
            }
            else
            {
                MessageBox.Show("해당하는 환자가 없습니다.");
                lblName.Text = "성명: ";
                lblBi.Text = "종류: ";
            }
            dt.Dispose();
            dt = null;
        }


        private void SsMain_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true)
            {
                return;
            }
            else
            { 
                string SQL = "";    //Query문
                string SqlErr = ""; //에러문 받는 변수
                DataTable dt = null;

                string strWrtno = "";

                txtXCode.Text = "";
                txtXName.Text = "";
                txtSName.Text = "";
                txtResult.Text = "";

                strWrtno = ssMain.ActiveSheet.Cells[e.Row, 9].Text.Trim();

                //해당 판독번호로 판독결과를 READ
                SQL = "" + ComNum.VBLF;
                SQL += "SELECT Pano,SName,TO_CHAR(ReadDate,'YYYY-MM-DD') ReadDate,TO_CHAR(SeekDate,'YYYY-MM-DD') SeekDate," + ComNum.VBLF;
                SQL += "       XJong,SName,Sex,Age,IpdOpd,DeptCode,DrCode,WardCode,RoomCode,XDrCode1,XDrCode2," + ComNum.VBLF;
                SQL += "       XDrCode3,IllCode1,IllCode2,IllCode3,XCode,XName,Result,Result1,Approve,WRTNO, ROWID " + ComNum.VBLF;
                SQL += "  FROM XRAY_RESULTNEW " + ComNum.VBLF;
                SQL += " WHERE WRTNO=" + strWrtno + " " + ComNum.VBLF;
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    MessageBox.Show("판독결과가 없습니다.");
                    return;
                }

                txtXCode.Text = dt.Rows[0]["XCODE"].ToString();
                txtXName.Text = dt.Rows[0]["XNAME"].ToString();
                txtSName.Text = dt.Rows[0]["SNAME"].ToString();
                FstrReadROWID = dt.Rows[0]["ROWID"].ToString();//2019-09-16 추가 작업 

                if (dt.Rows[0]["Approve"].ToString() == "N")
                {
                    txtResult.Text = ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                    txtResult.Text += "      ▶ 판독중입니다. 잠시후 다시 확인을 하십시오. ◀";
                }
                else
                {
                    txtResult.Text = dt.Rows[0]["RESULT"].ToString() + dt.Rows[0]["RESULT1"].ToString();
                }
                dt.Dispose();
                dt = null;
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            SearchData();
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            ePrint(FstrReadROWID);
        }

        private void SearchData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인	

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "   SELECT a.Pano,A.SName,TO_CHAR(a.SeekDate,'YYYYMMDD HH24:MI') SeekDate,a.DeptCode," + ComNum.VBLF;
                SQL += "       a.DrCode,d.XName, NVL(a.ExInfo, 0) ExInfo, a.IpdOpd, A.XJONG , TO_CHAR(a.rDate,'YYYY-MM-DD HH24:MI') rdate,   " + ComNum.VBLF;
                SQL += "       KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(a.Drcode) DrName  " + ComNum.VBLF;
                SQL += "  FROM KOSMOS_PMPA.XRAY_DETAIL a,  KOSMOS_PMPA.XRAY_CODE d " + ComNum.VBLF;
                SQL += " WHERE a.ENTERDate>=TO_DATE('" + dtpSDate.Text + "','YYYY-MM-DD') " + ComNum.VBLF;
                SQL += "   AND a.ENTERDate< TO_DATE('" + dtpEDate.Value.AddDays(1).ToShortDateString() + "','YYYY-MM-DD') " + ComNum.VBLF;
                SQL += "   AND PANO = '" + txtPano.Text + "' " + ComNum.VBLF;
                SQL += "   AND a.XCode NOT IN ('F12','F08','XCDC','F71C','F74C','FR71C','FR74C') " + ComNum.VBLF;
                SQL += "   AND a.XCode=d.XCode(+) " + ComNum.VBLF;
                SQL += "   AND ( a.GbReserved >='6'  or a.GbReserved ='1') " + ComNum.VBLF; //접수 또는 촬영완료
                SQL += "  ORDER BY A.SEEKDATE DESC" + ComNum.VBLF;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }
                ssMain_Sheet1.Rows.Count = dt.Rows.Count;
                ssMain_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssMain.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString();
                    ssMain.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["Sname"].ToString().Trim();

                    if (Convert.ToInt32(dt.Rows[i]["ExInfo"].ToString()) > 1000)
                    {
                        ssMain.ActiveSheet.Cells[i, 2].Text = "◎";
                    }

                    ssMain.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["SEEKDate"].ToString().Trim();
                    ssMain.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["IPDOPD"].ToString().Trim();
                    ssMain.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    ssMain.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["DRNAME"].ToString().Trim();

                    switch (dt.Rows[i]["XJONG"].ToString().Trim())
                    {
                        case "1": ssMain.ActiveSheet.Cells[i, 7].Text = "일반"; break;
                        case "2": ssMain.ActiveSheet.Cells[i, 7].Text = "특수"; break;
                        case "3": ssMain.ActiveSheet.Cells[i, 7].Text = "SONO"; break;
                        case "4": ssMain.ActiveSheet.Cells[i, 7].Text = "CT"; break;
                        case "5": ssMain.ActiveSheet.Cells[i, 7].Text = "MRI"; break;
                        case "6": ssMain.ActiveSheet.Cells[i, 7].Text = "RI"; break;
                        case "7": ssMain.ActiveSheet.Cells[i, 7].Text = "BMD"; break;
                        case "E": ssMain.ActiveSheet.Cells[i, 7].Text = "EMG"; break;
                        case "A": ssMain.ActiveSheet.Cells[i, 7].Text = "SONO"; break;
                    }


                    ssMain.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["XName"].ToString().Trim();
                    ssMain.ActiveSheet.Cells[i, 9].Text = dt.Rows[i]["EXINFO"].ToString().Trim();
                    ssMain.ActiveSheet.Cells[i, 10].Text = dt.Rows[i]["RDATE"].ToString();
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void FrmComMirXrayView_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
            }
            else
            {
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                lblName.Text = "";
                lblBi.Text = "";
                txtPano.Text = "";

                if (FstrPano_B == "")
                {
                    txtPano.Text = FstrPano;
                    dtpSDate.Text = "2018-01-01";
                    dtpEDate.Text = DateTime.Now.ToShortDateString();
                }
                else
                {
                    txtPano.Text = FstrPano_B;
                    dtpSDate.Text = FstrJinDate_B;
                    dtpEDate.Text = FstrOutDate_B;
                    lblName.Text = "성명:  " + FstrSName_B;
                    lblBi.Text = "종류:  " + FstrBi_B;
                }

                ssMain.ActiveSheet.Columns[9].Visible = false;
                txtXCode.Text = "";
                txtXName.Text = "";
                txtSName.Text = "";
                txtResult.Text = "";

                if (txtPano.Text != "")
                {
                    btnSearch.PerformClick();
                }
                
            }
        }

        void ePrint(string argROWID)
        {
            if(argROWID != "")
            {
                //int i = 0;
                //int j = 0;

                string strBI = "";
                //string strBiName = "";
                string strJumin = "";
                //string strXJong = "";
                string strDeptName = "";
                string strDrName = "";
                string strResultDrName = "";
                //string strIpdOpd = "";
                //string strXCode = "";
                //string strXName = "";

                string strResult = "";
                //string strChar = "";
                //string strPRT = "";

                //int nPriLine = 0;
                //int nPrtPage = 0;
                //int nPrtCNT = 0;

                //long nReportLine = 0;
                //int nLine2Char = 0;
                //int nPage2Line = 0;
                //int nReportTotalPage = 0;

                //long nPrtX = 0;

                string WRTNO = "";
                string PANO = "";
                string SNAME = "";
                string AGE = "";
                string SEX = "";
                string DEPTCODE = "";
                string DRCODE = "";
                string IPDOPD = "";
                string WARDCODE = "";
                string ROOMCODE = "";
                string SEEKDATE = "";
                string READDATE = "";
                string READTIME = "";
                string XJONG = "";
                string XCODE = "";
                string XNAME = "";
                string XDRSABUN = "";

                string SQL = "";    //Query문
                string SqlErr = ""; //에러문 받는 변수
                DataTable dt = null;
                DataTable dt1 = null;
                //DataTable dt2 = null;

                Cursor.Current = Cursors.WaitCursor;

                SQL = "SELECT Pano,SName,TO_CHAR(ReadDate,'YYYY-MM-DD') ReadDate,TO_CHAR(ReadTime,'YYYY-MM-DD HH24:MI') ReadTime,TO_CHAR(SeekDate,'YYYY-MM-DD') SeekDate,";
                SQL += "XJong,SName,Sex,Age,IpdOpd,DeptCode,DrCode,WardCode,RoomCode,XDrCode1,XDrCode2,ADDENDUM1,ADDENDUM2," + ComNum.VBLF;
                SQL += "XDrCode3,IllCode1,IllCode2,IllCode3,XCode,XName,Result,Result1,PrtCNT,ViewCNT,WRTNO" + ComNum.VBLF;
                SQL += "FROM XRAY_RESULTNEW" + ComNum.VBLF;
                SQL += "WHERE ROWID='" + argROWID + "'" + ComNum.VBLF;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                WRTNO = dt.Rows[0]["WRTNO"].ToString().Trim();
                PANO = dt.Rows[0]["PANO"].ToString().Trim();
                SNAME = dt.Rows[0]["SNAME"].ToString().Trim();
                AGE = dt.Rows[0]["AGE"].ToString().Trim();
                SEX = dt.Rows[0]["SEX"].ToString().Trim();
                DEPTCODE = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                DRCODE = dt.Rows[0]["DRCODE"].ToString().Trim();
                IPDOPD = dt.Rows[0]["IPDOPD"].ToString().Trim();
                WARDCODE = dt.Rows[0]["WARDCODE"].ToString().Trim();
                ROOMCODE = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                SEEKDATE = dt.Rows[0]["SEEKDATE"].ToString().Trim();
                READDATE = dt.Rows[0]["READDATE"].ToString().Trim();
                READTIME = dt.Rows[0]["READTIME"].ToString().Trim();
                XJONG = dt.Rows[0]["XJONG"].ToString().Trim();
                XCODE = dt.Rows[0]["XCODE"].ToString().Trim();
                XNAME = dt.Rows[0]["XNAME"].ToString().Trim();
                XDRSABUN = dt.Rows[0]["XDRCODE1"].ToString().Trim();

                strResult = dt.Rows[0]["RESULT"].ToString() + dt.Rows[0]["RESULT1"].ToString();

                if(dt.Rows[0]["ADDENDUM1"].ToString() != "" && dt.Rows[0]["ADDENDUM2"].ToString() != "")
                {
                    strResult = strResult + ComNum.VBLF + "Addendum : " + ComNum.VBLF + dt.Rows[0]["ADDENDUM1"].ToString().Trim() + dt.Rows[0]["ADDENDUM2"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                //자료를 SELECT  
                SQL = "";
                SQL += "SELECT Exid, TO_CHAR(ENTERDATE, 'YYYY-MM-DD') ENTERDATE FROM KOSMOS_PMPA.XRAY_DETAIL " + ComNum.VBLF;
                SQL += "WHERE Pano='" + PANO + "' " + ComNum.VBLF;
                SQL += "AND ExInfo = '" + WRTNO + "'" + ComNum.VBLF;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                string GISA = "";
                string ENTERDATE = "";

                if(dt.Rows.Count >0)
                {
                    #region READ_XRAY_GISA
                    SQL = "";
                    SQL += "SELECT Name FROM BAS_PASS " + ComNum.VBLF;
                    SQL += "WHERE IDnumber = '"+dt.Rows[0]["Exid"].ToString().Trim()+"'" + ComNum.VBLF;
                    SQL += "AND ProgramID = ' '" + ComNum.VBLF;

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if(dt1.Rows.Count == 1)
                    {
                        GISA = dt1.Rows[0]["Name"].ToString().Trim();
                    }else
                    {
                        GISA = "";
                    }

                    dt1.Dispose();
                    dt1 = null;
                    #endregion

                    ENTERDATE = dt.Rows[0]["EnterDate"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                //환자 마스타의 자료를 READ 

                SQL = "";
                SQL += "SELECT Bi,Jumin1,Jumin2" + ComNum.VBLF;
                SQL += "FROM BAS_Patient" + ComNum.VBLF;
                SQL += "WHERE PAno = '" + PANO + "'" + ComNum.VBLF;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                strBI = "";
                strJumin = "";

                if(dt.Rows.Count == 1)
                {
                    strBI = dt.Rows[0]["BI"].ToString().Trim();

                    strJumin = clsAES.Read_Jumin_AES(clsDB.DbCon, PANO);
                }

                //switch (strBI)
                //{
                //    case "11":
                //    case "12":
                //    case "13":
                //        strBiName = "건강보험";
                //        break;
                //    case "21":
                //        strBiName = "의료급여1종";
                //        break;
                //    case "22":
                //        strBiName = "의료급여2종";
                //        break;
                //    case "23":
                //        strBiName = "의료급여3종";
                //        break;
                //    case "24":
                //        strBiName = "행려환자";
                //        break;
                //    case "31":
                //        strBiName = "산재";
                //        break;
                //    case "32":
                //        strBiName = "공상";
                //        break;
                //    case "33":
                //        strBiName = "산재공상";
                //        break;
                //    case "41":
                //        strBiName = "공단100%";
                //        break;
                //    case "42":
                //        strBiName = "직장100%";
                //        break;
                //    case "43":
                //        strBiName = "지역100%";
                //        break;
                //    case "44":
                //        strBiName = "가족계획";
                //        break;
                //    case "51":
                //        strBiName = "일반";
                //        break;
                //    case "52":
                //        strBiName = "TA보험";
                //        break;
                //    case "53":
                //        strBiName = "계약처";
                //        break;
                //    case "54":
                //        strBiName = "미확인";
                //        break;
                //    case "55":
                //        strBiName = "TA일반";
                //        break;
                //    default:
                //        strBiName = "";
                //        break;
                //}

                strResultDrName = CF.READ_PassName(clsDB.DbCon, XDRSABUN);
                strResult = strResult.Trim();

                //switch (XJONG)
                //{
                //    case "1": strXJong = "일반촬영"; break;
                //    case "2": strXJong = "특수촬영"; break;
                //    case "3": strXJong = "SONO촬영"; break;
                //    case "4": strXJong = "CT촬영"; break;
                //    case "5": strXJong = "MRI촬영"; break;
                //    case "6": strXJong = "RI촬영"; break;
                //    case "7": strXJong = "BMD촬영"; break;
                //    default: strXJong = ""; break;
                //}

                strDeptName = CF.READ_DEPTNAMEK(clsDB.DbCon, DEPTCODE);
                strDrName = CF.READ_DrName(clsDB.DbCon, DRCODE);
                //strIpdOpd = "외래";

                //if (IPDOPD == "I")
                //{
                //    strIpdOpd = "입원";
                //}

                clsSpread SPR = new clsSpread();
                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                string strTitle = "";
                string strSubTitle1 = "";
                string strSubTitle2 = "";
                string strSubTitle3 = "";
                string strSubTitle4 = "";
                string strSubTitle5 = "";
                string strHeader = "";
                string strFooter = "";
                string strSubFooter1 = "";
                string strSubFooter2 = "";
                string strSubFooter3 = "";
                bool PrePrint = false;

                switch (XJONG)
                {
                    case "6":
                        strTitle = "R I Study Report";
                        break;
                    case "E":
                        strTitle = "전기진단검사 결과지";
                        break;
                    default:
                        strTitle = "방사선 촬영 결과지";
                        break;
                }

                strSubTitle1 = "==================================================================================================";
                strSubTitle2 = "    등록번호 : " + VB.Left(PANO + VB.Space(20), 24) + "성 명 : " + VB.Left(SNAME + VB.Space(20), 20) + "성 별 : " + AGE + "/" + SEX + "     ";
                if (IPDOPD == "I")
                {
                    strSubTitle2 += WARDCODE + "/" + ROOMCODE;
                }
                else
                {
                    strSubTitle2 += "외래";
                }
                strSubTitle3 = "    의 뢰 과 : " + VB.Left(strDeptName + VB.Space(20), 20) + "의 사 : " + VB.Left(strDrName + VB.Space(20), 20) + "검사 요청일 : " + ENTERDATE;
                strSubTitle4 = "==================================================================================================";
                strSubTitle5 = "검사명 : " + XNAME;

                strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += SPR.setSpdPrint_String("\n", new Font("굴림체", 5, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += SPR.setSpdPrint_String(strSubTitle1, new Font("굴림체", 10, FontStyle.Bold), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += SPR.setSpdPrint_String("\n", new Font("굴림체", 5, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += SPR.setSpdPrint_String(strSubTitle2, new Font("굴림체", 10, FontStyle.Bold), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += SPR.setSpdPrint_String("\n", new Font("굴림체", 5, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += SPR.setSpdPrint_String(strSubTitle3, new Font("굴림체", 10, FontStyle.Bold), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += SPR.setSpdPrint_String("\n", new Font("굴림체", 5, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += SPR.setSpdPrint_String(strSubTitle4, new Font("굴림체", 10, FontStyle.Bold), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += SPR.setSpdPrint_String("\n", new Font("굴림체", 5, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += SPR.setSpdPrint_String("\n", new Font("굴림체", 5, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += SPR.setSpdPrint_String(strSubTitle5, new Font("굴림체", 10, FontStyle.Bold), clsSpread.enmSpdHAlign.Left, false, true);

                strSubFooter1 = "촬영일자 : " + VB.Left(SEEKDATE+VB.Space(20),15) + "판독일자 : " +VB.Left(READDATE+VB.Space(20),15);
                switch (XJONG)
                {
                    case "6":
                        strSubFooter1 += "판독의사 : " + "ChanWoo Lee. M.D. (30842)";
                        break;
                    case "E":
                        strSubFooter1 += "판독의사 : 김 종 민 (56384)";
                        break;
                    default:
                        if(int.Parse(XDRSABUN) > 99000 && int.Parse(XDRSABUN) < 99100)
                        {
                            strSubFooter1 += "판독의사 : " + strResultDrName + " (" + CF.READ_OutPanDoctorDRNO(clsDB.DbCon, XDRSABUN) + ")" ;
                        }
                        else
                        {
                            strSubFooter1 += "판독의사 : " + strResultDrName + " (" + clsVbfunc.GetOCSDoctorDRBUNHO(clsDB.DbCon, XDRSABUN) + ")";
                        }
                        break;
                }
                strSubFooter2 = "==================================================================================================";
                strSubFooter3 = "※ 포항성모병원 영상의학과 ※     ☎ 전화 : 054-260-8163      FAX : 054-260-8006   ";

                strFooter = SPR.setSpdPrint_String(strSubFooter1, new Font("굴림체", 10, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strFooter += SPR.setSpdPrint_String("\n", new Font("굴림체", 5, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strFooter += SPR.setSpdPrint_String(strSubFooter2, new Font("굴림체", 10, FontStyle.Bold), clsSpread.enmSpdHAlign.Left, false, true);
                strFooter += SPR.setSpdPrint_String("\n", new Font("굴림체", 5, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strFooter += SPR.setSpdPrint_String(strSubFooter3, new Font("굴림체", 10, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 30, 50, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, false, false, false, false, false);

                ssResult.ActiveSheet.Cells[0, 0].Text = txtResult.Text;
                SPR.setSpdPrint(ssResult, PrePrint, setMargin, setOption, strHeader, strFooter);

                Cursor.Current = Cursors.Default;
            }
        }

    }
}
