using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

/// <summary>
/// Description : 고객정보조회
/// Author : 박병규
/// Create Date : 2017.06.13
/// </summary>
/// <history>
/// </history>
/// <seealso cref="frmCsinfoView.frm"/> 

namespace ComLibB
{
    public partial class frmViewCsinfo : Form
    {
        frmSearchPatient frmSearchPatientEvent = null;

        clsSpread CS = null;
        ComQuery CQ = null;
        ComFunc CF = null;
        clsOrdFunction OF = null;

        DataTable Dt = new DataTable();
        string SQL = string.Empty;
        string SqlErr = string.Empty;
        int intRowCnt = 0;

        string strPtno = string.Empty;
        string strJumin = string.Empty;
        string strDeathDate = string.Empty;

        clsPublic cpublic = new clsPublic();
        
        public frmViewCsinfo()
        {
            InitializeComponent();
            setParam();
        }

        public frmViewCsinfo(string strPtNo)
        {
            InitializeComponent();
            setParam();

            clsPublic.GstrPtno = strPtNo;
        }

        private void setParam()
        {
            this.Load += new EventHandler(eForm_Load);

            this.txtPtno.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.txtPtno.KeyDown  += new KeyEventHandler(eCtl_KeyDown);
        }

        private void eCtl_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender == this.txtPtno && e.Control == true && e.KeyCode == Keys.V)
            {
                string strPano = Clipboard.GetData(DataFormats.Text).ToString();
                if (string.IsNullOrEmpty(strPano) == false)
                    txtPtno.Text = strPano;
            }
        }

        private void eCtl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.txtPtno && e.KeyChar == (Char)13) { Screen_Display(); }
        }

        private void eForm_Load(object sender, EventArgs e)
        {
            CS = new clsSpread();
            CQ = new ComQuery();
            CF = new ComFunc();
            OF = new clsOrdFunction();

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); }       //Form 권한조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");                       //Form 기본값 셋팅

            txtPtno.Text = "";
            txtSname.Text = "";

            eForm_Clear();

            read_sysdate();

            if (clsPublic.GstrPtno != "")
            {
                txtPtno.Text = clsPublic.GstrPtno;
                clsPublic.GstrPtno = "";

                Screen_Display();
            }

            stcMain.SelectedTabIndex = 0;
        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        private void Screen_Display()
        {
            DataTable DtSub = new DataTable();
            string strPostCode = string.Empty;
            string strBirth = string.Empty;
            string strData = string.Empty;

            int nAge = 0;
            int nInfoCnt = 0;

            eForm_Clear();

            txtPtno.Text = ComFunc.LPAD(txtPtno.Text, 8, "0");

            strPtno = txtPtno.Text.Trim();

            Cursor.Current = Cursors.WaitCursor;

            #region //환자마스터 정보를 가져오기
            //환자마스터 정보
            SQL = "";
            SQL += ComNum.VBLF + " SELECT SName, Sex, Jumin1, ";
            SQL += ComNum.VBLF + "        Jumin2, Jumin3, ZipCode1, ";
            SQL += ComNum.VBLF + "        ZipCode2, Juso, Tel, ";
            SQL += ComNum.VBLF + "        TO_CHAR(StartDate,'YYYY-MM-DD') StartDate, ";
            SQL += ComNum.VBLF + "        TO_CHAR(LastDate,'YYYY-MM-DD')  LastDate, ";
            SQL += ComNum.VBLF + "        TO_CHAR(Birth,'YYYY-MM-DD')  Birth, ";
            SQL += ComNum.VBLF + "        GbBirth, DeptCode, HPhone, ";
            SQL += ComNum.VBLF + "        EMail, Jikup, GbJuger, ";
            SQL += ComNum.VBLF + "        Religion, GbInfor, GbSMS, ";
            SQL += ComNum.VBLF + "        Gb_VIP, Gb_VIP_Remark, ZipCode3, ";
            SQL += ComNum.VBLF + "        BuildNo, RoadDetail ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND Pano = '" + strPtno + "' ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count >= 1)
            {
                txtSname.Text = Dt.Rows[0]["SName"].ToString().Trim();

                strJumin = Dt.Rows[0]["Jumin1"].ToString().Trim();
                if (Dt.Rows[0]["Jumin3"].ToString().Trim() != "")
                    strJumin += clsAES.DeAES(Dt.Rows[0]["Jumin3"].ToString().Trim());
                else
                    strJumin += Dt.Rows[0]["Jumin2"].ToString().Trim();

                nAge = ComFunc.AgeCalc(clsDB.DbCon, strJumin);
                strPostCode = Dt.Rows[0]["ZipCode1"].ToString().Trim() + Dt.Rows[0]["ZipCode2"].ToString().Trim();

                ssList_Sheet1.Cells[0, 1].Text = VB.Left(strJumin,6) + "-" + VB.Right(strJumin, 7); //주민등록번호
                ssList_Sheet1.Cells[0, 3].Text = Dt.Rows[0]["Sex"].ToString().Trim() + "/" + nAge;  //성별/나이
                switch (Dt.Rows[0]["Religion"].ToString().Trim())   //종교
                {
                    case "1": ssList_Sheet1.Cells[0, 5].Text = "가톨릭"; break;
                    case "2": ssList_Sheet1.Cells[0, 5].Text = "기독교"; break;
                    case "3": ssList_Sheet1.Cells[0, 5].Text = "불교"; break;
                    case "4": ssList_Sheet1.Cells[0, 5].Text = "천도교"; break;
                    case "5": ssList_Sheet1.Cells[0, 5].Text = "유교"; break;
                    case "9": ssList_Sheet1.Cells[0, 5].Text = "기타"; break;

                    default:  ssList_Sheet1.Cells[0, 5].Text = ""; break;
                }

                ssList_Sheet1.Cells[1, 1].Text = Dt.Rows[0]["StartDate"].ToString().Trim();         //최초내원일
                ssList_Sheet1.Cells[1, 3].Text = Dt.Rows[0]["LastDate"].ToString().Trim();          //최종내원일
                ssList_Sheet1.Cells[1, 5].Text = Dt.Rows[0]["DeptCode"].ToString().Trim();          //최종진료과목

                ssList_Sheet1.Cells[2, 1].Text = Dt.Rows[0]["Tel"].ToString().Trim();               //전화번호
                ssList_Sheet1.Cells[2, 3].Text = Dt.Rows[0]["HPhone"].ToString().Trim();            //휴대폰번호
                ssList_Sheet1.Cells[2, 5].Text = Dt.Rows[0]["EMail"].ToString().Trim();             //전자우편

                if (Dt.Rows[0]["BuildNo"].ToString().Trim() != "")
                {
                    ssList_Sheet1.Cells[3, 1].Text = Dt.Rows[0]["ZipCode3"].ToString().Trim();       //우편번호
                    ssList_Sheet1.Cells[3, 3].Text = CQ.Read_RoadJuso(clsDB.DbCon, Dt.Rows[0]["BuildNo"].ToString().Trim()) + " " + Dt.Rows[0]["RoadDetail"].ToString().Trim();
                }
                else
                {
                    ssList_Sheet1.Cells[3, 1].Text = VB.Left(strPostCode, 3) + "-" + VB.Right(strPostCode, 3); //우편번호
                    ssList_Sheet1.Cells[3, 3].Text = CQ.Read_Juso(clsDB.DbCon, strPostCode) + " " + Dt.Rows[0]["Juso"].ToString().Trim();
                }

                strBirth = Dt.Rows[0]["Birth"].ToString().Trim();   //생년월일
                if (Dt.Rows[0]["Birth"].ToString().Trim() == "-")
                    ssList_Sheet1.Cells[4, 1].Text = strBirth + " (음력)";   
                else
                    ssList_Sheet1.Cells[4, 1].Text = strBirth + " (양력)";   

                if (strBirth != "" && Dt.Rows[0]["GbBirth"].ToString().Trim() == "-") //당해년도 생일
                {
                    strBirth = VB.Left(cpublic.strSysDate, 4) + VB.Right(strBirth, 6);
                    strBirth = ComFunc.ToSolar(strBirth);
                    
                    if (int.Parse(VB.Left(strBirth,4)) > int.Parse(VB.Left(cpublic.strSysDate, 4)))
                    {
                        strBirth = Dt.Rows[0]["Birth"].ToString().Trim();
                        strBirth = VB.Left(VB.DateAdd("yyyy", -1, cpublic.strSysDate).ToString(),4) + VB.Right(strBirth,6);
                        strBirth = ComFunc.ToSolar(strBirth);
                    }
                }
                else
                {
                    strBirth = VB.Left(cpublic.strSysDate, 4) + VB.Right(strBirth, 6);
                }

                ssList_Sheet1.Cells[4, 3].Text = strBirth;    //금년생일

                ssList_Sheet1.Cells[6, 5].Text = CF.Read_Csinfo_Name(clsDB.DbCon, "4", Dt.Rows[0]["Jikup"].ToString().Trim());   //직업명

                if (Dt.Rows[0]["GbSMS"].ToString().Trim() == "Y")
                    chkSMS.Checked = true;

                txtPmpaMsg.Text = Dt.Rows[0]["GbInfor"].ToString().Trim();  //원무팀 전달사항

                txtGubun.Text = Dt.Rows[0]["Gb_VIP"].ToString().Trim() + CF.Read_Bcode_Name(clsDB.DbCon, "BAS_VIP_구분코드", Dt.Rows[0]["Gb_VIP"].ToString().Trim());
                txtVRemark.Text = Dt.Rows[0]["Gb_VIP_Remark"].ToString().Trim();    //VIP정보

            }
            Dt.Dispose();
            Dt = null;

            //감액마스터 정보
            SQL = "";
            SQL += ComNum.VBLF + " SELECT a.GamMessage, b.SName ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_GAMF A, ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_BUSE B ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND A.GamJumin3   = '" + clsAES.AES(strJumin) + "' ";
            SQL += ComNum.VBLF + "    AND A.GamOut IS NULL ";
            SQL += ComNum.VBLF + "    AND A.GamEnd IS NULL ";
            SQL += ComNum.VBLF + "    AND A.GamSosok    = B.BuCode(+) ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count >= 1)
                ssList_Sheet1.Cells[6, 1].Text = Dt.Rows[0]["SName"].ToString().Trim() + " " + Dt.Rows[0]["GamMessage"].ToString().Trim();       

            Dt.Dispose();
            Dt = null;

            //특이사항 정보(블랙리스트 메모)
            SQL = "";
            SQL += ComNum.VBLF + " SELECT MEMO ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_OCSMEMO_MID ";
            SQL += ComNum.VBLF + " WHERE 1    = 1 ";
            SQL += ComNum.VBLF + "   AND Pano = '" + txtPtno.Text + "' ";
            SQL += ComNum.VBLF + "   AND (DDATE IS NULL OR DDATE ='' ) ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count >= 1)
                txtInfo.Rtf = VB.Replace(Dt.Rows[0]["MEMO"].ToString().Trim(), "`", "'");

            Dt.Dispose();
            Dt = null;

            //참고사항 정보
            SQL = "";
            SQL += ComNum.VBLF + " SELECT LtdName, Remark ";
            SQL += ComNum.VBLF + "   FROM  " + ComNum.DB_PMPA + "ETC_CSINFO_MST ";
            SQL += ComNum.VBLF + "  WHERE 1    = 1 ";
            SQL += ComNum.VBLF + "    AND Pano = '" + txtPtno.Text + "' ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count >= 1)
            {
                txtRemark.Text = VB.Replace(Dt.Rows[0]["Remark"].ToString().Trim(), "`", "'");  //기타사항
                ssList_Sheet1.Cells[6, 3].Text = Dt.Rows[0]["LtdName"].ToString().Trim();   //근무회사
            }
            
            Dt.Dispose();
            Dt = null;

            //혈액형 정보
            SQL = "";
            SQL += ComNum.VBLF + " SELECT Abo ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "EXAM_BLOOD_MASTER ";
            SQL += ComNum.VBLF + "  WHERE 1    = 1 ";
            SQL += ComNum.VBLF + "    AND Pano = '" + txtPtno.Text + "' ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count >= 1)
                ssList_Sheet1.Cells[4, 5].Text = VB.Replace(Dt.Rows[0]["Abo"].ToString().Trim(), "`", "'");

            Dt.Dispose();
            Dt = null;

            //입원상태 정보
            SQL = "";
            SQL += ComNum.VBLF + " SELECT WardCode, RoomCode, DeptCode, ";
            SQL += ComNum.VBLF + "        DrCode, ";
            SQL += ComNum.VBLF + "        KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(DrCode) Drname, ";
            SQL += ComNum.VBLF + "        Ilsu, SECRET ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
            SQL += ComNum.VBLF + "  WHERE 1    = 1 ";
            SQL += ComNum.VBLF + "    AND Pano = '" + txtPtno.Text + "' ";
            SQL += ComNum.VBLF + "    AND GBSTS IN ('0','2') ";
            SQL += ComNum.VBLF + "    AND OUTDATE IS NULL ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count >= 1)
            {
                strData = Dt.Rows[0]["DeptCode"].ToString().Trim() + "(";
                strData += Dt.Rows[0]["Drname"].ToString().Trim() + ")";
                strData += Dt.Rows[0]["WardCode"].ToString().Trim() + "-";
                strData += Dt.Rows[0]["RoomCode"].ToString().Trim() + " ";
                strData += "재원(" + Dt.Rows[0]["Ilsu"].ToString().Trim() + ")일";

                ssList_Sheet1.Cells[5, 5].Text = strData;

                if (Dt.Rows[0]["SECRET"].ToString().Trim() == "1")
                    ComFunc.MsgBox("사생활 보호 대상요청자입니다. 안내시 주의하세요.", "안내주의");
            }

            Dt.Dispose();
            Dt = null;

            #endregion

            #region //고객정보 가져오기
            //고객정보
            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(BDate,'YYYY-MM-DD') BDate, ";
            SQL += ComNum.VBLF + "        DeptCode, Gubun, Code, ";
            SQL += ComNum.VBLF + "        Remark, BuseName ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ETC_CSINFO_DATA ";
            SQL += ComNum.VBLF + "  WHERE 1    = 1 ";
            SQL += ComNum.VBLF + "    AND Pano = '" + txtPtno.Text + "' ";
            SQL += ComNum.VBLF + "    AND DelDate IS NULL ";
            SQL += ComNum.VBLF + "  ORDER BY BDate DESC ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            ssItem1_Sheet1.RowCount = Dt.Rows.Count;
            ssItem1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                ssItem1_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["BDate"].ToString().Trim();
                ssItem1_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["BuseName"].ToString().Trim();
                ssItem1_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["DeptCode"].ToString().Trim();
                ssItem1_Sheet1.Cells[i, 3].Text = CF.Read_Csinfo_Name(clsDB.DbCon, "1", Dt.Rows[i]["Gubun"].ToString().Trim());
                ssItem1_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["Remark"].ToString().Trim();
            }

            Dt.Dispose();
            Dt = null;

            //사망일자 정보
            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(DeathDate,'YYYY-MM-DD') DeathDate ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ETC_CSINFO_DEATH ";
            SQL += ComNum.VBLF + "  WHERE 1    = 1 ";
            SQL += ComNum.VBLF + "    AND Pano = '" + txtPtno.Text + "' ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count >= 1)
            {
                txtDeathDate.Text = Dt.Rows[0]["DeathDate"].ToString().Trim();
                strDeathDate = Dt.Rows[0]["DeathDate"].ToString().Trim();
            }

            Dt.Dispose();
            Dt = null;

            //암등록 정보
            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(TrDate,'YYYY-MM-DD') TrDate, T1Code1 ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MID_CANCER ";
            SQL += ComNum.VBLF + "  WHERE 1    = 1 ";
            SQL += ComNum.VBLF + "    AND Pano = '" + txtPtno.Text + "' ";
            SQL += ComNum.VBLF + "ORDER BY TrDate DESC ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count >= 1)
            {
                strData = Dt.Rows[0]["TrDate"].ToString().Trim() + " ";
                strData += Dt.Rows[0]["T1Code1"].ToString().Trim() + " ";
                strData += CF.Read_IllsName(clsDB.DbCon, Dt.Rows[0]["T1Code1"].ToString().Trim(), "1");

                txtCancer.Text = strData;
            }

            Dt.Dispose();
            Dt = null;

            #endregion

            #region //개인미수금액 가져오기
            SQL = "";
            SQL += ComNum.VBLF + " SELECT JAmt ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GAINMST ";
            SQL += ComNum.VBLF + "  WHERE 1    = 1 ";
            SQL += ComNum.VBLF + "    AND Pano = '" + txtPtno.Text + "' ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count >= 1)
            {
                txtCancer.Text = string.Format("{0:#,##0}", Dt.Rows[0]["JAmt"].ToString().Trim());
            }

            Dt.Dispose();
            Dt = null;

            #endregion

            #region //진료빈도(과별횟수) 가져오기
            //입원진료 횟수
            SQL = "";
            SQL += ComNum.VBLF + " SELECT B.PrintRanking, A.TDept, COUNT(*) CNT ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MID_SUMMARY A, ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_CLINICDEPT B ";
            SQL += ComNum.VBLF + "  WHERE 1             = 1 ";
            SQL += ComNum.VBLF + "    AND A.Pano        = '" + txtPtno.Text + "' ";
            SQL += ComNum.VBLF + "    AND A.TDept       = B.DeptCode(+) ";
            SQL += ComNum.VBLF + "  GROUP BY B.PrintRanking, A.TDept ";
            SQL += ComNum.VBLF + "  ORDER BY B.PrintRanking, A.TDept ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count >= 1)
            {
                strData = "";

                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    strData += Dt.Rows[i]["TDept"].ToString().Trim() + "(";
                    strData += Dt.Rows[i]["CNT"].ToString().Trim() + "),";
                }

                if (strData != "") { strData = VB.Left(strData, VB.Len(strData) - 1); }
                ssList_Sheet1.Cells[5, 1].Text = strData;
            }

            Dt.Dispose();
            Dt = null;

            //외래진료 횟수
            SQL = "";
            SQL += ComNum.VBLF + " SELECT B.PrintRanking, A.DeptCode, COUNT(*) CNT ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_HISTORY A, ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_CLINICDEPT B ";
            SQL += ComNum.VBLF + "  WHERE 1             = 1 ";
            SQL += ComNum.VBLF + "    AND A.Pano        = '" + txtPtno.Text + "' ";
            SQL += ComNum.VBLF + "    AND A.IpdOpd      = 'O' ";
            SQL += ComNum.VBLF + "    AND A.DeptCode    = B.DeptCode(+) ";
            SQL += ComNum.VBLF + "  GROUP BY B.PrintRanking, A.DeptCode ";
            SQL += ComNum.VBLF + "  ORDER BY B.PrintRanking, A.DeptCode ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count >= 1)
            {
                strData = "";

                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    strData += Dt.Rows[i]["DeptCode"].ToString().Trim() + "(";
                    strData += Dt.Rows[i]["CNT"].ToString().Trim() + "),";
                }

                if (strData != "") { strData = VB.Left(strData, VB.Len(strData) - 1); }
                ssList_Sheet1.Cells[5, 3].Text = strData;
            }

            Dt.Dispose();
            Dt = null;
            #endregion

            #region //개인별 예약사항 가져오기
            strData = "";

            //외래예약
            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(Date3,'YYYY-MM/DD HH24:MI') RDate, DeptCode ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_RESERVED ";
            SQL += ComNum.VBLF + "  WHERE 1    = 1 ";
            SQL += ComNum.VBLF + "    AND Pano = '" + txtPtno.Text + "' ";
            SQL += ComNum.VBLF + "  ORDER BY Date3, DeptCode ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count >= 1)
            {
                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    strData += Dt.Rows[i]["RDate"].ToString().Trim() + "(";
                    strData += Dt.Rows[i]["DeptCode"].ToString().Trim() + "),";
                }
            }

            Dt.Dispose();
            Dt = null;

            //전화접수예약
            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(RDate,'YYYY-MM/DD') RDate, RTime, DeptCode ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_TELRESV ";
            SQL += ComNum.VBLF + "  WHERE 1      = 1 ";
            SQL += ComNum.VBLF + "    AND Pano   = '" + txtPtno.Text + "' ";
            SQL += ComNum.VBLF + "    AND RDate  >= TRUNC(SYSDATE) ";
            SQL += ComNum.VBLF + "  ORDER BY RDate, RTime, DeptCode ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count >= 1)
            {
                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    strData += "☎";
                    strData += VB.Right(Dt.Rows[i]["RDate"].ToString().Trim(),5) + " ";
                    strData += Dt.Rows[i]["RTime"].ToString().Trim() + "(";
                    strData += Dt.Rows[i]["DeptCode"].ToString().Trim() + "),";
                }
            }

            Dt.Dispose();
            Dt = null;

            if (strData != "") { strData = VB.Left(strData, VB.Len(strData) - 1); }
            ssList_Sheet1.Cells[7, 1].Text = strData;

            #endregion

            #region //의무기록정보 가져오기
            SQL = "";
            SQL += ComNum.VBLF + " SELECT Pano, TO_CHAR(OUTDATE,'YYYY-MM-DD') OutDate, ";
            SQL += ComNum.VBLF + "        Sname, Idept, Tdept, ";
            SQL += ComNum.VBLF + "        TDoctor, ";
            SQL += ComNum.VBLF + "        KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(TDoctor) Drname, ";
            SQL += ComNum.VBLF + "        TO_CHAR(InDate,'YYYY-MM-DD') InDate,";
            SQL += ComNum.VBLF + "        Jilsu, Jumin1, Jumin2, Age, ";
            SQL += ComNum.VBLF + "        Bi, CResult, TModel, ";
            SQL += ComNum.VBLF + "        GbDie, Kukso, Kukso1,";
            SQL += ComNum.VBLF + "        Kukso2, Kukso3, Sgun1, ";
            SQL += ComNum.VBLF + "        SGun1_B, Sancd, NbGb, ";
            SQL += ComNum.VBLF + "        BabyType, Canc ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MID_SUMMARY ";
            SQL += ComNum.VBLF + "  WHERE 1    = 1 ";
            SQL += ComNum.VBLF + "    AND Pano = '" + txtPtno.Text + "' ";
            SQL += ComNum.VBLF + "   ORDER BY OutDate DESC ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            txtEmrCnt.Text = Dt.Rows.Count.ToString();  //의무기록 건수

            ssItem2_Sheet1.RowCount = Dt.Rows.Count;
            ssItem2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                ssItem2_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["OutDate"].ToString().Trim();
                ssItem2_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["Idept"].ToString().Trim();
                ssItem2_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["Tdept"].ToString().Trim();
                ssItem2_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["Drname"].ToString().Trim();
                ssItem2_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["InDate"].ToString().Trim();
                ssItem2_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["Jilsu"].ToString().Trim();
                ssItem2_Sheet1.Cells[i, 6].Text = ComFunc.SexCheck(Dt.Rows[i]["Jumin1"].ToString().Trim() + Dt.Rows[i]["Jumin2"].ToString().Trim(), "1");

                ssItem2_Sheet1.Cells[i, 7].Text = Dt.Rows[i]["Age"].ToString().Trim();
                ssItem2_Sheet1.Cells[i, 8].Text = CF.Read_Bi_Name(clsDB.DbCon, Dt.Rows[i]["Bi"].ToString().Trim(), "2");
                ssItem2_Sheet1.Cells[i, 9].Text = Dt.Rows[i]["TModel"].ToString().Trim();
                ssItem2_Sheet1.Cells[i, 10].Text = Dt.Rows[i]["CResult"].ToString().Trim();

                SQL = "";
                SQL += ComNum.VBLF + " SELECT Diagnosis1 ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MID_DIAGNOSIS ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND Pano      = '" + Dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                SQL += ComNum.VBLF + "    AND OUTDATE   = TO_DATE('" + Dt.Rows[i]["OUTDATE"].ToString().Trim() + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND ROWNUM    < 5 ";
                SQL += ComNum.VBLF + "  ORDER BY SeqNo, Diagnosis1 ";
                SqlErr = clsDB.GetDataTable(ref DtSub, SQL, clsDB.DbCon);

                for (int j = 0; j < DtSub.Rows.Count; j++)
                {
                    ssItem2_Sheet1.Cells[i, j + 11].Text = DtSub.Rows[j]["Diagnosis1"].ToString().Trim();
                }

                DtSub.Dispose();
                DtSub = null;

                SQL = "";
                SQL += ComNum.VBLF + " SELECT Operation ";
                SQL += ComNum.VBLF + "   FROM  " + ComNum.DB_PMPA + "MID_OP ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND Pano      = '" + Dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                SQL += ComNum.VBLF + "    AND OUTDATE   = TO_DATE('" + Dt.Rows[i]["OUTDATE"].ToString().Trim() + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND ROWNUM    < 4 ";
                SQL += ComNum.VBLF + "  ORDER BY SeqNo,Operation ";

                SqlErr = clsDB.GetDataTable(ref DtSub, SQL, clsDB.DbCon);

                for (int j = 0; j < DtSub.Rows.Count; j++)
                {
                    ssItem2_Sheet1.Cells[i, j + 15].Text = DtSub.Rows[j]["Operation"].ToString().Trim();
                }

                DtSub.Dispose();
                DtSub = null;
                
                ssItem2_Sheet1.Cells[i, 18].Text = Dt.Rows[i]["GbDie"].ToString().Trim();
                ssItem2_Sheet1.Cells[i, 19].Text = Dt.Rows[i]["Kukso"].ToString().Trim();
                ssItem2_Sheet1.Cells[i, 20].Text = Dt.Rows[i]["Kukso1"].ToString().Trim();
                ssItem2_Sheet1.Cells[i, 21].Text = Dt.Rows[i]["Kukso2"].ToString().Trim();
                ssItem2_Sheet1.Cells[i, 22].Text = Dt.Rows[i]["NbGb"].ToString().Trim() + "/" + Dt.Rows[i]["BabyType"].ToString().Trim();
                ssItem2_Sheet1.Cells[i, 23].Text = Dt.Rows[i]["SGun1"].ToString().Trim();
                ssItem2_Sheet1.Cells[i, 24].Text = Dt.Rows[i]["SGun1_B"].ToString().Trim();
                ssItem2_Sheet1.Cells[i, 25].Text = Dt.Rows[i]["Sancd"].ToString().Trim();

                if (strDeathDate == "")
                {
                    if (Dt.Rows[i]["TModel"].ToString().Trim() == "5")  //사망퇴원
                        strDeathDate = Dt.Rows[i]["OutDate"].ToString().Trim();

                    if (VB.Val(Dt.Rows[i]["CResult"].ToString().Trim()) >= 7)  //48시간이전,이후 사망
                        strDeathDate = Dt.Rows[i]["OutDate"].ToString().Trim();
                }
            }

            Dt.Dispose();
            Dt = null;
            #endregion

            #region //진료정보 가져오기
            SQL = "";
            SQL += ComNum.VBLF + " SELECT IpdOpd, TO_CHAR(SDate,'yyyy-mm-dd') Sdate, ";
            SQL += ComNum.VBLF + "        TO_CHAR(EDate,'yyyy-mm-dd') Edate, ";
            SQL += ComNum.VBLF + "        Ilsu, DeptCode, DRCODE, ";
            SQL += ComNum.VBLF + "        KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(DRCODE) DrName, ";
            SQL += ComNum.VBLF + "        Bi, Pname, Kiho, ";
            SQL += ComNum.VBLF + "        Gkiho, Tamt, Jamt, ";
            SQL += ComNum.VBLF + "        Gamt, Mamt, Yamt ";
            SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_HISTORY ";
            SQL += ComNum.VBLF + "  WHERE 1       = 1 ";
            SQL += ComNum.VBLF + "    AND Pano    = '" + txtPtno.Text + "' ";
            SQL += ComNum.VBLF + "    AND TAmt    > 0 ";
            SQL += ComNum.VBLF + "  ORDER BY Sdate DESC ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            txtJinCnt.Text = Dt.Rows.Count.ToString();  //진료정보 건수

            ssItem3_Sheet1.RowCount = Dt.Rows.Count;
            ssItem3_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                if (Dt.Rows[i]["IpdOpd"].ToString().Trim() == "I")
                {
                    ssItem3_Sheet1.Cells[i, 0].Text = "입원";
                    ssItem3_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["Sdate"].ToString().Trim();
                    ssItem3_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["Edate"].ToString().Trim();
                }
                else
                {
                    ssItem3_Sheet1.Cells[i, 0].Text = "외래";
                    ssItem3_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["Sdate"].ToString().Trim();
                }

                ssItem3_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["Ilsu"].ToString().Trim();
                ssItem3_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["DeptCode"].ToString().Trim();
                ssItem3_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["DrName"].ToString().Trim();
                ssItem3_Sheet1.Cells[i, 7].Text = CF.Read_Bi_Name(clsDB.DbCon, Dt.Rows[i]["bi"].ToString().Trim(),"2");
                ssItem3_Sheet1.Cells[i, 8].Text = Dt.Rows[i]["Pname"].ToString().Trim();
                ssItem3_Sheet1.Cells[i, 9].Text = Dt.Rows[i]["Kiho"].ToString().Trim();
                ssItem3_Sheet1.Cells[i, 10].Text = Dt.Rows[i]["Gkiho"].ToString().Trim();
            }

            Dt.Dispose();
            Dt = null;

            #endregion

            #region //건강증진정보 가져오기
            long nHeaNo = 0;
            long nHicNo = 0;

            //종합검진
            SQL = "";
            SQL += ComNum.VBLF + " SELECT Pano ";
            SQL += ComNum.VBLF + "   FROM  " + ComNum.DB_PMPA + "HEA_PATIENT ";
            SQL += ComNum.VBLF + "  WHERE 1             = 1 ";
            SQL += ComNum.VBLF + "    AND Ptno        = '" + txtPtno.Text + "' ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count >= 1)
                nHeaNo = long.Parse(Dt.Rows[0]["Pano"].ToString().Trim());
            
            Dt.Dispose();
            Dt = null;

            if (nHeaNo == 0) { return; }

            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(SDate,'YYYY-MM-DD') SDate, ";
            SQL += ComNum.VBLF + "        TO_CHAR(IDate,'YYYY-MM-DD') IDate, ";
            SQL += ComNum.VBLF + "        GjJong, B.NAME, GBSTS ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "HEA_JEPSU A,";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "HEA_EXJONG B";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND A.Pano    = " + nHeaNo + " ";
            SQL += ComNum.VBLF + "    AND A.GJJONG  =  B.CODE ";
            SQL += ComNum.VBLF + "    AND A.DelDate IS NULL ";
            SQL += ComNum.VBLF + " ORDER BY SDate DESC ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            txtMedCnt.Text = Dt.Rows.Count.ToString();  //종합검진 건수

            ssItem4_1_Sheet1.RowCount = Dt.Rows.Count;
            ssItem4_1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                ssItem4_1_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["SDate"].ToString().Trim();
                ssItem4_1_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["IDate"].ToString().Trim();
                ssItem4_1_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["NAME"].ToString().Trim();

                switch (Dt.Rows[i]["GBSTS"].ToString().Trim())
                {
                    case "0":
                        ssItem4_1_Sheet1.Cells[i, 3].Text = "예약접수"; break;
                    case "1":
                        ssItem4_1_Sheet1.Cells[i, 3].Text = "수진등록"; break;
                    case "2":
                        ssItem4_1_Sheet1.Cells[i, 3].Text = "일부입력"; break;
                    case "3":
                        ssItem4_1_Sheet1.Cells[i, 3].Text = "입력완료"; break;
                    case "9":
                        ssItem4_1_Sheet1.Cells[i, 3].Text = "판정완료"; break;
                    case "D":
                        ssItem4_1_Sheet1.Cells[i, 3].Text = "삭제"; break;
                }
            }

            Dt.Dispose();
            Dt = null;

            //건강검진
            SQL = "";
            SQL += ComNum.VBLF + " SELECT Pano ";
            SQL += ComNum.VBLF + "   FROM  " + ComNum.DB_PMPA + "HIC_PATIENT ";
            SQL += ComNum.VBLF + "  WHERE 1             = 1 ";
            SQL += ComNum.VBLF + "    AND Ptno        = '" + txtPtno.Text + "' ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count >= 1)
                nHicNo = long.Parse(Dt.Rows[0]["Pano"].ToString().Trim());

            Dt.Dispose();
            Dt = null;

            if (nHeaNo == 0) { return; }

            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(A.JepDate,'YYYY-MM-DD') JepDate, ";
            SQL += ComNum.VBLF + "        A.GjJong, B.NAME, A.Pano, ";
            SQL += ComNum.VBLF + "        A.SName, A.WRTNO, A.LtdCode, ";
            SQL += ComNum.VBLF + "        A.BuseName, A.SExams, A.UCodes, ";
            SQL += ComNum.VBLF + "        A.Jisa, A.Kiho ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "HIC_JEPSU A,";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "HIC_EXJONG B";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND A.Pano    = " + nHicNo + " ";
            SQL += ComNum.VBLF + "    AND A.GJJONG  =  B.CODE ";
            SQL += ComNum.VBLF + "    AND A.DelDate IS NULL ";
            SQL += ComNum.VBLF + "  ORDER BY A.JepDate DESC ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            txtHicCnt.Text = Dt.Rows.Count.ToString();  //건강검진 건수

            ssItem4_2_Sheet1.RowCount = Dt.Rows.Count;
            ssItem4_2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                ssItem4_2_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["JepDate"].ToString().Trim();
                ssItem4_2_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["NAME"].ToString().Trim();
                ssItem4_2_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["Pano"].ToString().Trim();

                ssItem4_2_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["WRTNO"].ToString().Trim();
                ssItem4_2_Sheet1.Cells[i, 4].Text = CF.Read_Ltd_Name(clsDB.DbCon, Dt.Rows[i]["LtdCode"].ToString().Trim());
                ssItem4_2_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["BuseName"].ToString().Trim();
                ssItem4_2_Sheet1.Cells[i, 6].Text = SExam_Names_Disp(Dt.Rows[i]["SExams"].ToString().Trim());
                ssItem4_2_Sheet1.Cells[i, 7].Text = UCode_Names_Disp(Dt.Rows[i]["UCodes"].ToString().Trim());

            }

            Dt.Dispose();
            Dt = null;
            
            #endregion


            if (txtPmpaMsg.Text != "") { nInfoCnt += 1; }
            txtInfoCnt.Text = nInfoCnt.ToString();

            Cursor.Current = Cursors.Default;
        }

        private string SExam_Names_Disp(string ArgCode)
        {
            DataTable DtDisp = new DataTable();
            string rtnVal = string.Empty;
            string strSql = string.Empty;

            if (ArgCode == "") { return rtnVal; }

            for (int i = 1; i <= VB.I(ArgCode, ","); i++)
            {
                if (VB.Pstr(ArgCode, ",", i).Trim() != "")
                    strSql += "'" + VB.Pstr(ArgCode, ",", i).Trim() + "',";
            }

            if (strSql == "") { return rtnVal; }

            strSql = VB.Left(strSql, strSql.Length - 1);

            SQL = "";
            SQL += ComNum.VBLF + " SELECT Code, Name, YName ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "HIC_GROUPCODE ";
            SQL += ComNum.VBLF + "  WHERE Code IN (" + strSql + ") ";
            SQL += ComNum.VBLF + "ORDER BY Code ";
            SqlErr = clsDB.GetDataTable(ref DtDisp, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            for (int i = 0; i < DtDisp.Rows.Count; i++)
            {
                if (DtDisp.Rows[i]["YName"].ToString().Trim() != "")
                    rtnVal += DtDisp.Rows[i]["YName"].ToString().Trim() + ",";
                else
                    rtnVal += DtDisp.Rows[i]["Name"].ToString().Trim() + ",";
            }

            DtDisp.Dispose();
            DtDisp = null;

            return rtnVal;
        }

        private string UCode_Names_Disp(string ArgCode)
        {
            DataTable DtDisp = new DataTable();
            string rtnVal = string.Empty;
            string strSql = string.Empty;

            if (ArgCode == "") { return rtnVal;}

            for (int i = 1; i <= VB.I(ArgCode,","); i++)
            {
                if (VB.Pstr(ArgCode,",",i).Trim() != "")
                    strSql += "'" + VB.Pstr(ArgCode,",",i).Trim() + "',";
            }

            if (strSql == "") { return rtnVal; }

            strSql = VB.Left(strSql, strSql.Length - 1);
            
            SQL = "";
            SQL += ComNum.VBLF + " SELECT Code, Name, YName ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "HIC_MCODE ";
            SQL += ComNum.VBLF + "  WHERE Code IN (" + strSql + ") ";
            SQL += ComNum.VBLF + "ORDER BY Code ";
            SqlErr = clsDB.GetDataTable(ref DtDisp, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            for (int i = 0; i < DtDisp.Rows.Count; i++)
            {
                if (DtDisp.Rows[i]["YName"].ToString().Trim() != "")
                    rtnVal += DtDisp.Rows[i]["YName"].ToString().Trim() + ",";
                else
                    rtnVal += DtDisp.Rows[i]["Name"].ToString().Trim() + ",";
            }

            DtDisp.Dispose();
            DtDisp = null;

            return rtnVal;
        }

        private void eForm_Clear()
        {
            strPtno = "";
            strJumin = "";
            strDeathDate = "";

            //고객정보현황
            ComFunc.SetAllControlClear(grbCnt);
            //고객기본정보
            CS.Spread_Clear_Range(ssList, 0, 1, ssList.Sheets[0].RowCount, 1);
            CS.Spread_Clear_Range(ssList, 0, 3, ssList.Sheets[0].RowCount, 1);
            CS.Spread_Clear_Range(ssList, 0, 5, ssList.Sheets[0].RowCount, 1);
            //고객정보
            ComFunc.SetAllControlClear(pnlItem1);
            //의무기록(퇴원)
            ComFunc.SetAllControlClear(pnlItem2);
            //진료정보
            ComFunc.SetAllControlClear(pnlItem3);
            //건강증진
            ComFunc.SetAllControlClear(pnlItem4);
            //기타사항
            ComFunc.SetAllControlClear(pnlItem5);
            //특이사항
            ComFunc.SetAllControlClear(pnlItem6);
            txtInfo.Rtf = "";
            //vip정보
            ComFunc.SetAllControlClear(pnlItem7);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtPtno.Text == "") { return; }
            txtPtno.Text = string.Format("{0:00000000}", txtPtno.Text);
            Screen_Display();
        }

        private void btnSaveVip_Click(object sender, EventArgs e)
        {
            frmPmpaMasterVIP frm = new frmPmpaMasterVIP(txtPtno.Text.Trim());
            frm.ShowDialog();
            OF.fn_ClearMemory(frm);

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            eForm_Clear();

            txtPtno.Text = "";
            txtSname.Text = "";
            txtPtno.Focus();
        }

        private void btnSaveCsinfo_Click(object sender, EventArgs e)
        {
            frmMasterCsinfo frm = new frmMasterCsinfo(txtPtno.Text.Trim());
            frm.ShowDialog();
            OF.fn_ClearMemory(frm);

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ssItem2_LeaveCell(object sender, FarPoint.Win.Spread.LeaveCellEventArgs e)
        {
            string strCode = string.Empty;

            switch (e.NewColumn)
            {
                case 9:
                    sBarMsg1.Text = "퇴원형태: 1.퇴원지시후 2.자의퇴원 3.전송 4.탈원 5.사망";
                    return;

                case 10:
                    sBarMsg1.Text = "퇴원결과: 1.완쾌 2.호전 3.호전않됨 4.치료못함 5.진단뿐 6.가망없는 퇴원 7.48이전사망, 8.48이후사망";
                    return;

                case 18:
                    sBarMsg1.Text = "사망구분: 1.수술중 2.마취사망 3.수술후 10일이내 4.신생아 사망";
                    return;

                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                    strCode = ssItem2_Sheet1.Cells[e.NewRow, e.NewColumn].Text.Trim();
                    sBarMsg1.Text = CF.Read_IllsName(clsDB.DbCon, strCode, "1");
                    return;

                default:
                    sBarMsg1.Text = "";
                    return;
            }
        }

        private void btnPtnoSearch_Click(object sender, EventArgs e)
        {
            frmSearchPatientEvent = new frmSearchPatient();
            frmSearchPatientEvent.rSendPano += FrmSearchPatientEvent_rSendPano;
            frmSearchPatientEvent.rEventExit += FrmSearchPatientEvent_rEventExit;
            frmSearchPatientEvent.ShowDialog();
            OF.fn_ClearMemory(frmSearchPatientEvent);

        }

        private void FrmSearchPatientEvent_rEventExit()
        {
            frmSearchPatientEvent.Dispose();
            frmSearchPatientEvent = null;
        }

        private void FrmSearchPatientEvent_rSendPano(string strPano)
        {
            txtPtno.Text = strPano;
            Screen_Display();

            frmSearchPatientEvent.Dispose();
            frmSearchPatientEvent = null;
        }

        private void txtPtno_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
