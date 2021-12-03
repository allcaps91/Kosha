using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스
using System.Collections.Generic;

namespace ComLibB
{
    /// <summary>
    /// \nurse\nrQI\Frm지표결과_자동생성.frm
    /// </summary>
    public partial class frmJiPyo : Form
    {
        public frmJiPyo()
        {
            InitializeComponent();
        }

        private void frmJiPyo_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            DateTime dtp = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));

            cboYYMM.Items.Clear();
            for(int i = 0; i < 25; i++)
            {
                dtp = dtp.AddMonths(-1);
                cboYYMM.Items.Add(dtp.ToString("yyyyMM"));
            }

            cboYYMM.SelectedIndex = 0;

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            GetBuse();
            LIST_Display();
        }

        void GetBuse()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            cboBuse.Items.Clear();
            cboBuse.Items.Add("전체부서");

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "SELECT a.BuCode,b.Name,COUNT(*) CNT ";
                SQL += ComNum.VBLF + " FROM NUR_QI_MST a,BAS_BUSE b ";
                SQL += ComNum.VBLF + "WHERE a.DelDate IS NULL ";
                SQL += ComNum.VBLF + "  AND a.BuCode=b.BuCode(+) ";
                SQL += ComNum.VBLF + "GROUP BY a.BuCode,b.Name ";
                SQL += ComNum.VBLF + "ORDER BY a.BuCode,b.Name ";

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

                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    cboBuse.Items.Add(dt.Rows[i]["BuCode"].ToString().Trim() + "." + dt.Rows[i]["NAME"].ToString().Trim());
                }

                cboBuse.SelectedIndex = 0;

                //cboBuse.Enabled = false;

                //cboBuse.SelectedIndex = cboBuse.Items.IndexOf(VB.Pstr(clsPublic.GstrWardCodes, ".", 1).Trim());

                //cboBuse.Enabled = clsType.User.Sabun == "4349" || clsType.User.Sabun == "23758" || clsType.User.Sabun == "7306" || clsType.User.Sabun == "30814";

                switch (clsType.User.BuseCode)
                {
                    case "000101":
                    case "000103":
                    case "033101":
                    case "077101":
                    case "077501":
                    case "078100":
                    case "078101":
                        cboBuse.Enabled = true;
                        cboBuse.SelectedIndex = 0;
                        break;
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        void LIST_Display()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strBuCode = cboBuse.Text.Trim() == "전체부서" ? "" : VB.Pstr(cboBuse.Text, ".", 1);

            ss1_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                //'자동형성할 목록을 읽음
                SQL = "SELECT Code,Name FROM NUR_QI_MST ";
                SQL += ComNum.VBLF + "WHERE (DelDate IS NULL OR DelDate ='') ";
                SQL += ComNum.VBLF + "  AND GbAuto='Y' ";
                if (strBuCode.Length > 0)
                {
                    SQL += ComNum.VBLF + "  AND BuCode ='" + strBuCode + "' ";

                }
                SQL += ComNum.VBLF + "ORDER BY Code ";

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

                ss1_Sheet1.RowCount = dt.Rows.Count;
                ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    ss1_Sheet1.Cells[i, 0].Text = "";
                    ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Code"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Name"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            StartBuild();
            return;    
        }

        void StartBuild()
        {

            string FstrYYMM = cboYYMM.Text.Trim();
            string FstrSDate = VB.Left(FstrYYMM, 4) + "-" + VB.Right(FstrYYMM, 2) + "-01";
            DateTime dtp = Convert.ToDateTime(FstrSDate);
            string FstrEDate = dtp.ToString("yyyy-MM-") + DateTime.DaysInMonth(dtp.Year, dtp.Month); 

            string strCode = "";
            string strName = "";

            int nJobCnt = 0;
            int nTotCnt = 0;
            int i = 0;

            progressBar1.Value = 0;

            for(i = 0; i < ss1_Sheet1.RowCount; i++)
            {
                if(ss1_Sheet1.Cells[i, 0].Text == "True")
                {
                    nTotCnt += 1;
                }
            }

            if(nTotCnt == 0)
            {
                ComFunc.MsgBox("작업할 목록을 한건도 선택 안함");
                return;
            }

            progressBar1.Maximum = nTotCnt;

            for (i = 0; i < ss1_Sheet1.RowCount; i++)
            {
                if(ss1_Sheet1.Cells[i, 0].Text == "True")
                {
                    strCode = ss1_Sheet1.Cells[i, 1].Text.Trim();
                    strName = ss1_Sheet1.Cells[i, 2].Text.Trim();

                    ss1_Sheet1.Cells[i, 3, i, 4].Text = "";
                    ss1_Sheet1.Cells[i, 3].Text = ComQuery.CurrentDateTime(clsDB.DbCon, "T");

                    nJobCnt += 1;

                    progressBar1.Value = nJobCnt;

                    switch(strCode)
                    {
                        case "21011":
                            Build_21011_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;  //중환자 중증도 분류
                        case "27101":
                            Build_27101_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //내시경
                        case "27102":
                            Build_27102_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //심초음파
                        case "27103":
                            Build_27103_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //기능검사 신속성 - 방사선과
                        case "27201":
                            Build_27201_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //위내시경 생검
                        case "27202":
                            Build_27202_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //자궁세포진 검사
                        case "27203":
                            Build_27203_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //체액세포병리 검사
                        case "27204":
                            Build_27204_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //혈액배양 검사
                        case "27401":
                            Build_27401_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //혈액반납량
                        case "27402":
                            Build_27402_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //혈액폐기량
                        case "25601":
                            Build_25601_Process(FstrYYMM, FstrSDate, FstrEDate);
                            break;   //응급혈액검사
                        case "25602":
                            Build_25602_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //응급화학검사
                        case "25603":
                            Build_25603_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //응급동맥혈검사
                        case "25604":
                            Build_25604_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //응급요검사
                        case "25605":
                            Build_25605_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //응급지원서비스-단순방사선검사
                        case "91001":
                            Build_91001_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //응급환자귀가
                        case "91002":
                            Build_91002_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //응급환자입원
                        case "91003":
                            Build_91003_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //응급환자이송
                        case "91004":
                            Build_91004_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //응급환자수술
                        case "26101":
                            Build_26101_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //정규수술대기간
                        case "26201":
                            Build_26201_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //정규수술대기간 5분
                        case "26401":
                            Build_26401_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //정규수술취소
                        case "26402":
                            Build_26402_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //정규수술취소 - 과별
                        case "26403":
                            Build_26403_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //정규수술취소 - 사유
                        case "26404":
                            Build_26404_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //정규수술취소 - 과장
                        case "28301":
                            Build_28301_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //Brain MRI
                        case "28302":
                            Build_28302_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //ABDOMEN USG
                        case "28303":
                            Build_28303_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //Chest CT
                        case "28304":
                            Build_28304_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //UGIS
                        case "28305":
                            Build_28305_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //혈관조영
                        case "28306":
                            Build_28306_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //대장조영
                        case "28307":
                            Build_28307_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //abdomen CT
                        case "92011":
                            Build_92011_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //모성신생아(분만)
                        case "92012":
                            Build_92012_Process(FstrYYMM, FstrSDate, FstrEDate);
                            break;   //모성신생아(젖물림)
                        case "92021":
                            Build_92021_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //중환자실
                        case "92031":
                            Build_92031_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //폐렴
                        case "92041":
                            Build_92041_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //수술예방항생제
                        case "93011":
                            Build_93011_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //중증뇌질환-
                        case "93012":
                            Build_93012_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //중증외상질환-
                        case "93013":
                            Build_93013_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //중증뇌질환-
                        case "93014":
                            Build_93014_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //중증외상질환-
                        case "51001":
                            Build_51001_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //욕창 - 위험대상 생성
                        case "51002":
                            Build_51002_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //낙상 - 발생대상 생성
                        case "51003":
                            Build_51003_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //투약 - 발생대상 생성
                        case "52001":
                            Build_52001_Process(FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //중증도 분류 통계
                        //case "92051": Build_92051_Process(FstrYYMM, FstrSDate, FstrEDate) break;   //임상질지표-병동 CONSULT 현황(전체)

                        case "92052": //'임상질지표-병동 CONSULT 현황(의사)
                        case "92053": //'임상질지표-병동 CONSULT 현황(병동)
                        case "92054": //'임상질지표-병동 CONSULT 현황(과별)
                            //통합
                            Build_92052_Process(strCode, FstrYYMM, FstrSDate, FstrEDate);
                            Application.DoEvents();
                            break;   //'임상질지표-병동 CONSULT 현황(의사)
                    }

                    ss1_Sheet1.Cells[i, 4].Text = ComQuery.CurrentDateTime(clsDB.DbCon, "T");
                }
            }
        }

        /// <summary>
        /// 정규수술대기간
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns>
        bool Build_26101_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;
            DataTable dt2 = null;

            long nCnt1 = 0;
            long nCnt2 = 0;
            long nCnt3 = 0;
            long nOpCNT = 0;

            string strGDate = "";
            string strOpDate = "";
            string strInDate = "";
            long nTime = 0;
            long nIlsu = 0;

            string strDeptCode = "";
            long nIpdNo = 0;
            string strInDept = "";

            string strCODE = "26101";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                //'정규수술
                SQL = " SELECT PANO,SName,TO_CHAR(OPDATE,'MM') MONTH,TO_CHAR(OPDATE,'YYYY-MM-DD') OPDATE,OpTimeFrom, ";
                SQL += ComNum.VBLF + "  GbDelay,OpSTime,OpCancel,Sex,Age,DeptCode,WardCode,RoomCode, DrCode  ";
                SQL += ComNum.VBLF + "  FROM ADMIN.ORAN_MASTER ";
                SQL += ComNum.VBLF + "   WHERE OPDATE >=TO_DATE('" + argSDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND OPDATE <=TO_DATE('"  + argEDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND (GbAngio IS NULL OR GbAngio <> 'Y') ";
                SQL += ComNum.VBLF + "    AND IPDOPD ='I' "; //'입원수술
                SQL += ComNum.VBLF + "    AND (OPBUN = '1' OR OpBun IS NULL) "; //'정규수술

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    nCnt1 += 1; //전체인원


                    strGDate = Convert.ToDateTime(dt.Rows[i]["OPDATE"].ToString()).AddDays(1).ToShortDateString();
                    strOpDate = dt.Rows[i]["OPDATE"].ToString() + " " + VB.Val(dt.Rows[i]["OPDATE"].ToString()).ToString("00:00");
                    strDeptCode = dt.Rows[i]["DeptCode"].ToString().Trim();


                    //'입원일자
                    SQL = " SELECT TO_CHAR(INDATE,'YYYY-MM-DD HH24:MI') INDATE,IPDNO,DeptCode  ";
                    SQL += ComNum.VBLF + "   FROM ADMIN.IPD_NEW_MASTER ";
                    SQL += ComNum.VBLF + "  WHERE PANO = '" + dt.Rows[i]["PANO"].ToString() + "' ";
                    SQL += ComNum.VBLF + "    AND InDate<=TO_DATE('" + strGDate + " 23:59','YYYY-MM-DD HH24:MI') ";
                    SQL += ComNum.VBLF + "    AND (OUTDATE >=TO_DATE('" + dt.Rows[i]["OPDATE"].ToString() + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "         OR OUTDATE IS NULL) ";
                    SQL += ComNum.VBLF + "  ORDER BY InDate ";

                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if(dt2.Rows.Count > 0)
                    {
                        strInDate = dt2.Rows[0]["INDATE"].ToString();
                        nIpdNo = Convert.ToInt64(dt2.Rows[0]["IPDNO"].ToString());
                        strInDept = dt2.Rows[0]["DeptCode"].ToString().Trim();
                    }

                    dt2.Dispose();
                    dt2 = null;


                    //'입원 후 처음수술 여부를 읽음
                    nOpCNT = 0;
                    nIlsu = 0;
                    nTime = 0;
                    if(strInDate.Length > 0)
                    {

                        SQL = "SELECT COUNT(*) CNT FROM ADMIN.ORAN_MASTER ";
                        SQL += ComNum.VBLF + "WHERE Pano='" + dt.Rows[i]["Pano"].ToString() + "' ";
                        SQL += ComNum.VBLF + "  AND IpdOpd='I' ";
                        SQL += ComNum.VBLF + "  AND OpDate>=TO_DATE('" + VB.Left(strInDate, 10) + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "  AND OpDate< TO_DATE('"  + VB.Left(strOpDate, 10) + "','YYYY-MM-DD') ";

                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        if (dt2.Rows.Count > 0)
                        {
                            nOpCNT = Convert.ToInt32(dt2.Rows[0]["CNT"].ToString().Trim());
                        }

                        dt2.Dispose();
                        dt2 = null;

                        //'전실전과 내역을 읽어 최초 입원과를 찾음
                        SQL = "SELECT FrDept FROM ADMIN.IPD_TRANSFOR ";
                        SQL += ComNum.VBLF + "WHERE IPDNO=" + nIpdNo + " ";
                        SQL += ComNum.VBLF + "  AND TrsDate<=TO_DATE('" + VB.Left(strOpDate, 10) + " 23:59','YYYY-MM-DD HH24:MI') ";
                        SQL += ComNum.VBLF + "ORDER BY TrsDate ";

                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        if (dt2.Rows.Count > 0)
                        {
                            strInDept = dt2.Rows[0]["FrDept"].ToString().Trim();
                        }

                        dt2.Dispose();
                        dt2 = null;

                        //'입원 후 경과일수
                        nIlsu = DATE_ILSU(VB.Left(strOpDate, 10), VB.Left(strInDate, 10));
                        nTime = DATE_TIME(strInDate, strOpDate);
                    }

                    strOpDate = strOpDate.Length != 16 ? VB.Left(strOpDate, 10) : strOpDate;

                    if(nTime > 2880)
                    {
                        nCnt3 += 1;
                        //해당명단
                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DrCode,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',3,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"    ].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"     ].ToString().Trim() + "','" + strInDept + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"  ].ToString().Trim() + "', ";
                        SQL = SQL + "  TO_DATE('" + strInDate + "','YYYY-MM-DD HH24:MI'),";
                        SQL = SQL + "  TO_DATE('" + strOpDate + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                    else
                    {
                        nCnt2 += 1;
                        //해당명단
                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DrCode,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',2,";
                        SQL = SQL + " '" +  dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" +  dt.Rows[i]["SEX"].ToString().Trim() + "','" + strInDept + "', ";
                        SQL = SQL + " '" +  dt.Rows[i]["WARDCODE"].ToString().Trim() + "'," + dt.Rows[i]["ROOMCODE"].ToString().Trim() + ",";
                        SQL = SQL + " '" +  dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                        SQL = SQL + "  TO_DATE('" + strInDate + "','YYYY-MM-DD HH24:MI'),";
                        SQL = SQL + "  TO_DATE('" + strOpDate + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }

                //'총건수
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',3," + nCnt3 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 정규수술대기간 5분
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns>
        bool Build_26201_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;

            string strCODE = "26201";

            string strOpDate = "";
            string strInDate = "";
            long nTime = 0;
            string strInDept = "";
            string strSWardTime = "";
            string strEOrTime = "";

            long nCnt1 = 0;
            long nCnt2 = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                // '정규수술
                SQL = " SELECT PANO,SName,TO_CHAR(OPDATE,'MM') MONTH,TO_CHAR(OPDATE,'YYYY-MM-DD') OPDATE, ";
                SQL += ComNum.VBLF + "  GbDelay,SWardTime,EOrTime,OpCancel,Sex,Age,DeptCode,WardCode,RoomCode, DrCode  ";
                SQL += ComNum.VBLF + "  FROM ADMIN.ORAN_MASTER ";
                SQL += ComNum.VBLF + "   WHERE OPDATE >=TO_DATE('" + argSDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND OPDATE <=TO_DATE('" + argEDATE + "','YYYY-MM-DD') "; 
                SQL += ComNum.VBLF + "    AND (GbAngio IS NULL OR GbAngio='N') ";
                SQL += ComNum.VBLF + "    AND IPDOPD ='I' ";// '입원수술
                SQL += ComNum.VBLF + "    AND (OPBUN = '1' OR OpBun IS NULL) ";// '정규수술
                SQL += ComNum.VBLF + "    AND (OpRe IS NULL OR OpRe<>'1') ";// '재수술은 제외

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for(int i = 0; i < dt.Rows.Count; i++)
                {

                    nCnt1 += 1;

                    strSWardTime = dt.Rows[i]["SWardTime"].ToString().Trim();
                    strEOrTime = dt.Rows[i]["EOrTime"].ToString().Trim();

                    strInDate = dt.Rows[i]["OpDate"].ToString().Trim() + " " + strSWardTime;
                    strOpDate = dt.Rows[i]["OpDate"].ToString().Trim() + " " + strEOrTime;

                    if(strSWardTime.Length == 5 && strEOrTime.Length == 5)
                    {//소요시간을 계산

                        nTime = (long) ((VB.Val(VB.Mid(strEOrTime, 1, 2)) * 60) + VB.Val(VB.Right(strEOrTime, 2)) - (VB.Val(VB.Mid(strSWardTime, 1, 2)) * 60) + VB.Val(VB.Right(strSWardTime, 2)));

                        if(nTime > 5)
                        {
                            nCnt2 += 1;

                            //해당명단
                            SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DrCode,Time1,Time2) VALUES ('";
                            SQL += ArgYYMM + "','" + strCODE + "',2,";
                            SQL += ComNum.VBLF +  " '" + dt.Rows[i]["PANO"].ToString() + "','" + dt.Rows[i]["SNAME"].ToString() + "', ";
                            SQL += ComNum.VBLF +  " '" + dt.Rows[i]["SEX"].ToString() + "','" + strInDept + "', ";
                            SQL += ComNum.VBLF +  " '" + dt.Rows[i]["WARDCODE"].ToString() + "'," + dt.Rows[i]["ROOMCODE"].ToString() + ",";
                            SQL += ComNum.VBLF +  " '" + dt.Rows[i]["DRCODE"].ToString() + "', ";
                            SQL += ComNum.VBLF +  "  TO_DATE('" + strInDate + " ','YYYY-MM-DD HH24:MI'),";
                            SQL += ComNum.VBLF + "  TO_DATE('" + strOpDate + "','YYYY-MM-DD HH24:MI')  ) ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }

                    }
                }

                dt.Dispose();
                dt = null;

                //'총건수
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }
        /// <summary>
        /// 정규수술취소
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns>
        bool Build_26401_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;

            long nCnt1 = 0;
            long nCnt2 = 0;


            string strCODE = "26401";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'정규수술
                SQL = " SELECT PANO,SName,TO_CHAR(OPDATE,'MM') MONTH,TO_CHAR(OPDATE,'YYYY-MM-DD') OPDATE,OpTimeFrom, ";
                SQL += ComNum.VBLF + "  GbDelay,OpSTime,OpCancel,Sex,Age,DeptCode,WardCode,RoomCode, OpBun, DrCode  ";
                SQL += ComNum.VBLF + "  FROM ADMIN.ORAN_MASTER ";
                SQL += ComNum.VBLF + "   WHERE OPDATE >=TO_DATE('" + argSDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND OPDATE <=TO_DATE('" + argEDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND (GbAngio IS NULL OR GbAngio <>'Y') ";
                SQL += ComNum.VBLF + "    AND IPDOPD ='I' "; //'입원수술
                //'SQL = SQL & "    AND (OPBUN = '1' OR OpBun IS NULL)  " '정규수술
                SQL += ComNum.VBLF + "    AND (OPBUN = '1' OR OpBun IS NULL OR OpBun IN ('A','B','C','0')) ";// '정규수술

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    if(dt.Rows[i]["OpCancel"].ToString().Trim() == "")
                    {
                        nCnt1 += 1;
                    }


                    if(dt.Rows[i]["OpCancel"].ToString().Trim().Length > 0 || dt.Rows[i]["OpBun"].ToString().Trim() == "0" ||
                       dt.Rows[i]["OpBun"].ToString().Trim() == "A" || dt.Rows[i]["OpBun"].ToString().Trim() == "B")
                    {
                        nCnt2 += 1;

                        //'해당명단
                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DrCode,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',2,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DeptCode"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                        SQL = SQL + "  TO_DATE('" + "" + "','YYYY-MM-DD HH24:MI'),";
                        SQL = SQL + "  TO_DATE('" + "" + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                    }

                }


                //'총건수;
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }
        /// <summary>
        /// 정규수술취소 - 과별
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns>
        bool Build_26402_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;

            long nCnt1 = 0;
            long nCnt2 = 0;
            long nCnt3 = 0;
            long nCnt4 = 0;
            long nCnt5 = 0;
            long nCnt6 = 0;
            long nCnt7 = 0;
            long nCnt8 = 0;
            long nCnt9 = 0;

            string strVal = "";

            string strCODE = "26402";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }



                //'정규수술
                SQL = " SELECT PANO,SName,TO_CHAR(OPDATE,'MM') MONTH,TO_CHAR(OPDATE,'YYYY-MM-DD') OPDATE,OpTimeFrom, ";
                SQL += ComNum.VBLF + "  GbDelay,OpSTime,OpCancel,Sex,Age,DeptCode,WardCode,RoomCode, OpBun, DrCode  ";
                SQL += ComNum.VBLF + "  FROM ADMIN.ORAN_MASTER ";
                SQL += ComNum.VBLF + "   WHERE OPDATE >=TO_DATE('" + argSDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND OPDATE <=TO_DATE('" + argEDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND (GbAngio IS NULL OR GbAngio<>'Y') ";
                SQL += ComNum.VBLF + "    AND IPDOPD ='I' ";// '입원수술
                SQL += ComNum.VBLF + "    AND (OPBUN = '1' OR OpBun IS NULL OR OpBun IN ('A','B','C','0')) ";// '정규수술
                //'SQL = SQL & "    AND (OpRe IS NULL OR OpRe<>'1') " '재수술은 제외
                SQL += ComNum.VBLF + "    ORDER BY DeptCode ";
                SQL += ComNum.VBLF + "";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["OpCancel"].ToString().Trim() == "")
                    {
                        nCnt1 += 1;
                    }


                    if (dt.Rows[i]["OpCancel"].ToString().Trim().Length > 0 || dt.Rows[i]["OpBun"].ToString().Trim() == "0" ||
                       dt.Rows[i]["OpBun"].ToString().Trim() == "A" || dt.Rows[i]["OpBun"].ToString().Trim() == "B")
                    {
                        switch(dt.Rows[i]["DeptCode"].ToString().Trim())
                        {
                            case "GS":
                                strVal = "2";
                                nCnt2 += 1;
                                break;
                            case "OS":
                                strVal = "3";
                                nCnt3 += 1;
                                break;
                            case "NS":
                                strVal = "4";
                                nCnt4 += 1;
                                break;
                            case "UR":
                                strVal = "5";
                                nCnt5 += 1;
                                break;
                            case "EN":
                                strVal = "6";
                                nCnt6 += 1;
                                break;
                            case "OT":
                                strVal = "7";
                                nCnt7 += 1;
                                break;
                            case "OG":
                                strVal = "8";
                                nCnt8 += 1;
                                break;
                            default:
                                strVal = "9";
                                nCnt9 += 1;
                                break;
                        }


                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DrCode,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "'," + strVal +  ",";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DeptCode"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                        SQL = SQL + "  TO_DATE('" + "" + "','YYYY-MM-DD HH24:MI'),";
                        SQL = SQL + "  TO_DATE('" + "" + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }


                    }
                }


                //'총건수
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1, " + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',3," + nCnt3 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',4," + nCnt4 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',5," + nCnt5 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',6," + nCnt6 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',7," + nCnt7 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',8," + nCnt8 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',9," + nCnt9 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }
        /// <summary>
        /// 정규수술취소 - 사유
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns>
        bool Build_26403_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;

            long nCnt1 = 0;
            long nCnt2 = 0;
            long nCnt3 = 0;

            string strOK = "";

            string strInDate = "";
            string strOpDate = "";

            string strCODE = "26403";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;

                }


                //'정규수술
                SQL = " SELECT PANO,SName,TO_CHAR(OPDATE,'MM') MONTH,TO_CHAR(OPDATE,'YYYY-MM-DD') OPDATE,OpTimeFrom, ";
                //'SQL = SQL & "   SAYU01, SAYU02, SAYU03, SAYU04, SAYU05," & vbLf     '병원사유
                //'SQL = SQL & "  SAYU06, SAYU07, SAYU08, SAYU09, SAYU10, SAYU11, SAYU12, SAYU13, SAYU14, SAYU15," & vbLf '환자사유
                SQL += ComNum.VBLF + "  GbDelay,OpSTime,OpCancel,Sex,Age,DeptCode,WardCode,RoomCode,OpBun, DrCode  ";
                SQL += ComNum.VBLF + "  FROM ADMIN.ORAN_MASTER ";
                SQL += ComNum.VBLF + "   WHERE OPDATE >=TO_DATE('" + argSDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND OPDATE <=TO_DATE('" + argEDATE + "','YYYY-MM-DD') ";//
                SQL += ComNum.VBLF + "    AND (GbAngio IS NULL OR GbAngio<>'Y') ";
                SQL += ComNum.VBLF + "    AND IPDOPD ='I' "; //'입원수술
                SQL += ComNum.VBLF + "    AND (OPBUN = '1' OR OpBun IS NULL OR OpBun IN ('A','B','C','0')) ";// '정규수술
                //'SQL = SQL & "    AND (OpRe IS NULL OR OpRe<>'1') " '재수술은 제외

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["OpCancel"].ToString().Trim() == "")
                    {
                        nCnt1 += 1;
                    }


                    if (dt.Rows[i]["OpCancel"].ToString().Trim().Length > 0)
                    {
                        
                        if(dt.Rows[i]["OpBun"].ToString().Trim() == "A" || dt.Rows[i]["OpBun"].ToString().Trim() == "B" || dt.Rows[i]["OpBun"].ToString().Trim() == "C")
                        {
                            strOK = "1";
                        }


                        if (dt.Rows[i]["OpBun"].ToString().Trim() == "0" )
                        {
                            strOK = "2";
                        }

                    }

                    strInDate = dt.Rows[i]["OpDate"].ToString().Trim();
                    strOpDate = dt.Rows[i]["OpDate"].ToString().Trim();

                    if(strOK == "1")
                    {
                        nCnt2 += 1;

                        //'해당명단
                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DrCode,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',2,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DeptCode"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                        SQL = SQL + "  TO_DATE('" + strInDate + " ','YYYY-MM-DD HH24:MI'),";
                        SQL = SQL + "  TO_DATE('" + strOpDate + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                    else if(strOK == "2")
                    {
                        nCnt3 += 1;

                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DrCode,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',3,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DeptCode"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                        SQL = SQL + "  TO_DATE('" + strInDate + " ','YYYY-MM-DD HH24:MI'),";
                        SQL = SQL + "  TO_DATE('" + strOpDate + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }


                //'총건수
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                // '
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',3," + nCnt3 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }
        /// <summary>
        /// 정규수술취소 - 과장
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns>
        bool Build_26404_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;

            long nCnt1 = 0;
            long nCnt2 = 0; long nCnt3 = 0; long nCnt4 = 0; long nCnt5 = 0; long nCnt6 = 0; long nCnt7 = 0; long nCnt8 = 0; long nCnt9 = 0; long nCnt10 = 0; long nCnt11 = 0; long nCnt12 = 0;
            long nCnt13 = 0; long nCnt14 = 0; long nCnt15 = 0; long nCnt16 = 0; long nCnt17 = 0; long nCnt18 = 0; long nCnt19 = 0; long nCnt20 = 0; long nCnt21 = 0; long nCnt22 = 0; long nCnt23 = 0; long nCnt50 = 0;

            string strVal = "";

            string strCODE = "26404";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                //'정규수술
                SQL = " SELECT PANO,SName,TO_CHAR(OPDATE,'MM') MONTH,TO_CHAR(OPDATE,'YYYY-MM-DD') OPDATE,OpTimeFrom, ";
                SQL += ComNum.VBLF + "  GbDelay,OpSTime,OpCancel,Sex,Age,DeptCode,WardCode,RoomCode,DrCode  ";
                SQL += ComNum.VBLF + "  FROM ADMIN.ORAN_MASTER ";
                SQL += ComNum.VBLF + "   WHERE OPDATE >=TO_DATE('" + argSDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND OPDATE <=TO_DATE('" + argEDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND (GbAngio IS NULL OR GbAngio<>'Y') ";
                SQL += ComNum.VBLF + "    AND IPDOPD ='I' ";// '입원수술
                SQL += ComNum.VBLF + "    AND (OPBUN = '1' OR OpBun IS NULL OR OpBun ('A','B','0')) "; //'/정규수술
                //'SQL = SQL & "    AND (OpRe IS NULL OR OpRe<>'1') " '재수술은 제외
                SQL += ComNum.VBLF + "    ORDER BY DeptCode ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["OpCancel"].ToString().Trim() == "")
                    {
                        nCnt1 += 1;
                    }


                    if (dt.Rows[i]["OpCancel"].ToString().Trim().Length > 0)
                    {

                        switch(dt.Rows[i]["DrCode"].ToString().Trim())
                        {
                            case "1108":
                                strVal = "2";
                                nCnt2 += 1;
                                break;
                            case "2103":
                                strVal = "3";
                                nCnt3 += 1;
                                break;
                            case "2108":
                                strVal = "4";
                                nCnt4 += 1;
                                break;
                            case "2109":
                                strVal = "5";
                                nCnt5 += 1;
                                break;
                            case "2110":
                                strVal = "6";
                                nCnt6 += 1;
                                break;
                            case "2203":
                                strVal = "7";
                                nCnt7 += 1;
                                break;
                            case "2213":
                                strVal = "8";
                                nCnt8 += 1;
                                break;
                            case "2214":
                                strVal = "9";
                                nCnt9 += 1;
                                break;
                            case "2215":
                                strVal = "10";
                                nCnt10 += 1;
                                break;
                            case "2216":
                                strVal = "11";
                                nCnt11 += 1;
                                break;
                            case "2303":
                                strVal = "12";
                                nCnt12 += 1;
                                break;
                            case "2304":
                                strVal = "13";
                                nCnt13 += 1;
                                break;
                            case "2305":
                                strVal = "14";
                                nCnt14 += 1;
                                break;
                            case "2306":
                                strVal = "15";
                                nCnt15 += 1;
                                break;
                            case "3107":
                                strVal = "16";
                                nCnt16 += 1;
                                break;
                            case "3108":
                                strVal = "17";
                                nCnt17 += 1;
                                break;
                            case "3110":
                                strVal = "18";
                                nCnt18 += 1;
                                break;
                            case "4101":
                                strVal = "19";
                                nCnt19 += 1;
                                break;
                            case "4206":
                                strVal = "20";
                                nCnt20 += 1;
                                break;
                            case "4207":
                                strVal = "21";
                                nCnt21 += 1;
                                break;
                            case "5207":
                                strVal = "22";
                                nCnt22 += 1;
                                break;
                            case "5208":
                                strVal = "23";
                                nCnt23 += 1;
                                break;
                            default:
                                strVal = "50";
                                nCnt50 += 1;
                                break;

                        }

                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DrCode,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "'," + strVal + ",";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DeptCode"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                        SQL = SQL + "  TO_DATE('" + "" + " ','YYYY-MM-DD HH24:MI'),";
                        SQL = SQL + "  TO_DATE('" + "" + "','YYYY-MM-DD HH24:MI')  ) ";

                         SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                    }
                }

                //'총건수
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1, " + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',3," + nCnt3 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',4," + nCnt4 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',5," + nCnt5 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',6," + nCnt6 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',7," + nCnt7 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',8," + nCnt8 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',9," + nCnt9 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',10," + nCnt9 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',11," + nCnt9 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',12," + nCnt9 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',13," + nCnt9 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',14," + nCnt9 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',15," + nCnt9 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',16," + nCnt9 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',17," + nCnt9 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',18," + nCnt9 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',19," + nCnt9 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',20," + nCnt9 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',21," + nCnt9 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',22," + nCnt9 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',23," + nCnt9 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',50," + nCnt9 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 입원환자 위십이지장내시경 지표
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns>
        bool Build_27101_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;

            long nCnt1 = 0;
            long nCnt2 = 0;
            long nCnt3 = 0;
            long nTime = 0;

            string strCODE = "27101";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'당월 입원환자 내시경검사 내역을 읽음
                SQL = "SELECT  Ptno,SNAME,DEPTCODE,WARDCODE,ROOMCODE,SEX,DRCODE,GbJob,TO_CHAR(OrderDATE,'YYYY-MM-DD HH24:MI') OrderDate, ";
                SQL += ComNum.VBLF + "  TO_CHAR(ResultDate,'YYYY-MM-DD HH24:MI') ResultDate ";
                SQL += ComNum.VBLF + "  FROM ADMIN.ENDO_JUPMST  ";
                SQL += ComNum.VBLF + " WHERE ResultDate >= To_Date('" + argSDATE + "', 'YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND ResultDate <= To_Date('" + argEDATE + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL += ComNum.VBLF + "   AND GBIO = 'I' ";
                SQL += ComNum.VBLF + "   AND DeptCode NOT IN ( 'TO','HR') ";
                SQL += ComNum.VBLF + "   AND GbSunap = '1' ";
                SQL += ComNum.VBLF + " AND ORDERCODE IN ('00440110','00440120') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    nCnt1 += 1; //전체인원

                    nTime = DATE_TIME(dt.Rows[i]["OrderDate"].ToString().Trim(), dt.Rows[i]["ResultDate"].ToString().Trim());

                    if (nTime > 1440)
                    {
                        nCnt2 += 1;
                        //해당명단
                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DrCode,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',2,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                        SQL = SQL + "  TO_DATE('" + dt.Rows[i]["OrderDate"].ToString().Trim() + " ','YYYY-MM-DD HH24:MI'),";
                        SQL = SQL + "  TO_DATE('" + dt.Rows[i]["ResultDate"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                //'총건수
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',3," + nCnt3 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 입원환자 심장초음파 지표
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns>
        bool Build_27102_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;

            long nCnt1 = 0;
            long nCnt2 = 0;
            long nCnt3 = 0;
            long nTime = 0;

            string strCODE = "27102";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'당월 입원환자 내시경검사 내역을 읽음
                SQL = " SELECT  Ptno,SNAME,SEX,DEPTCODE,DRCODE,ROOMCODE, TO_CHAR(OrderDATE, 'YYYY-MM-DD HH24:MI') OrderDATE, " ;
                SQL += ComNum.VBLF + " TO_CHAR(RDATE,'YYYY-MM-DD') JDATE, "                                                             ;
                SQL += ComNum.VBLF + " TO_CHAR(RDate, 'YYYY-MM-DD HH24:MI') ResultDate "                                                ;
                SQL += ComNum.VBLF + " FROM ADMIN.ETC_JUPMST "                                                                     ;
                SQL += ComNum.VBLF + " WHERE RDate >= To_Date('" + argSDATE + "', 'YYYY-MM-DD') "                                       ;
                SQL += ComNum.VBLF + "   AND RDate <= To_Date('" + argEDATE + " 23:59', 'YYYY-MM-DD HH24:MI') "                         ;
                SQL += ComNum.VBLF + "   AND GbJob='3' ";  //'촬영자(검사자만)                                                      ;
                SQL += ComNum.VBLF + "   AND Gubun='3' "; //'심장초음파                                                                     ;
                SQL += ComNum.VBLF + "   AND GBIO = 'I' "                                                                               ;
                SQL += ComNum.VBLF + "   AND DeptCode<>'TO' "                                                                           ;
                SQL += ComNum.VBLF + "   AND DrCode NOT IN ('22545','19728') "                                                          ;
                SQL += ComNum.VBLF + " ORDER BY BDATE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    nCnt1 += 1; //전체인원

                    nTime = DATE_TIME(dt.Rows[i]["OrderDate"].ToString().Trim(), dt.Rows[i]["ResultDate"].ToString().Trim());

                    if (nTime > 2880)
                    {
                        nCnt2 += 1;
                        //해당명단
                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DrCode,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',2,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                        SQL = SQL + "  TO_DATE('" + dt.Rows[i]["OrderDate"].ToString().Trim() + " ','YYYY-MM-DD HH24:MI'),";
                        SQL = SQL + "  TO_DATE('" + dt.Rows[i]["ResultDate"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                //'총건수
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',3," + nCnt3 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 기능검사 신속성 - 방사선과
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns>
        bool Build_27103_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;
            DataTable dt2 = null;

            long nCnt1 = 0;
            long nCnt2 = 0;
            long nCnt3 = 0;
            long nTime = 0;

            string strCODE = "27103";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'기능검사신속
                SQL = "  SELECT  a.Pano, a.SName, a.IpdOpd, a.Sex, a.Age ,TO_CHAR(a.EnterDate, 'YYYY-MM-DD HH24:MI') EnterDate  , ";
                SQL += ComNum.VBLF + " a.DeptCode,  a.OrderCode, a.RoomCode,a.WardCode, TO_CHAR(a.SeekDate,'YYYY-MM-DD HH24:MI') ResultDate,";
                SQL += ComNum.VBLF + " TO_CHAR(a.BDATE,'YYYY-MM-DD HH24:MI') BDATE,";
                SQL += ComNum.VBLF + " a.DrCode , a.XCode, a.ROWID, a.Exinfo,TO_CHAR(a.SeekDate,'MM') MONTH ";
                SQL += ComNum.VBLF + " FROM ADMIN.XRAY_DETAIL a";
                SQL += ComNum.VBLF + "  WHERE a.SeekDate >= To_Date('" + argSDATE + "', 'YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND a.SeekDate < To_Date('" + Convert.ToDateTime(argEDATE).AddDays(1).ToShortDateString() + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND a.GBRESERVED IN ('7','8')    AND a.XJong = '1'    AND a.DeptCode <>'TO' ";
                SQL += ComNum.VBLF + "   AND a.DrCode NOT IN ('19728') ";
                SQL += ComNum.VBLF + " ORDER BY a.XJong, a.SeekDate, a.Pano ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    nCnt1 += 1; //전체인원

                    nTime = DATE_TIME(dt.Rows[i]["EnterDate"].ToString().Trim(), dt.Rows[i]["ResultDate"].ToString().Trim());

                    if (nTime > 2880)
                    {
                        nCnt2 += 1;
                        //해당명단
                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DrCode,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',2,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "',";
                        SQL = SQL + "  TO_DATE('" + dt.Rows[i]["Bdate"].ToString().Trim() + "','YYYY-MM-DD HH24:MI'),";
                        SQL = SQL + "  TO_DATE('" + dt.Rows[i]["ResultDate"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }


                    if(VB.Val(dt.Rows[i]["Exinfo"].ToString().Trim()) > 1000)
                    {
                        SQL = " SELECT TO_CHAR(ETime,'YYYY-MM-DD HH24:MI') ReadDate, ";
                        SQL += ComNum.VBLF + " TO_CHAR(ReadDate,'MM') Month ";
                        SQL += ComNum.VBLF + " FROM ADMIN.XRAY_RESULTNEW ";
                        SQL += ComNum.VBLF + " WHERE WRTNO = " + dt.Rows[i]["Exinfo"].ToString().Trim() + " ";
                        SQL += ComNum.VBLF + " ORDER BY SeekDate DESC ";

                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        if(dt2.Rows.Count> 0)
                        {
                            nTime = DATE_TIME(dt.Rows[0]["ResultDate"].ToString().Trim(), dt2.Rows[0]["ReadDate"].ToString().Trim());
                            if(nTime > 1440)
                            {
                                nCnt3 += 1;

                                //해당명단
                                SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DrCode,Time1,Time2) VALUES ('";
                                SQL = SQL + ArgYYMM + "','" + strCODE + "',3,";
                                SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                                SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                                SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                                SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "',";
                                SQL = SQL + "  TO_DATE('" + dt.Rows[i]["ResultDate"].ToString().Trim() + " ','YYYY-MM-DD HH24:MI'),";
                                SQL = SQL + "  TO_DATE('" + dt2.Rows[0]["ReadDate"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }
                            }
                        }


                        dt2.Dispose();
                        dt2 = null;
                    }
                }

                dt.Dispose();
                dt = null;

                //'총건수
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',3," + nCnt3 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 진단검사의학과 위내시경 생검
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns>
        bool Build_27201_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;

            long nCnt1 = 0;
            long nCnt2 = 0;
            long nTime = 0;

            string strCODE = "27201";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'위내시경 생검
                SQL = "SELECT b.Pano, b.SName, b.Age, b.Sex, b.Room RoomCode, b.Ward WardCode, b.DrCode, b.DeptCode, ";
                SQL += ComNum.VBLF + "  TO_CHAR(b.ResultDate,'YYYY-MM-DD HH24:MI') ResultDate, TO_CHAR(a.JDate,'YYYY-MM-DD HH24:MI') JDate "; 
                SQL += ComNum.VBLF + "  FROM ADMIN.EXAM_ANATMST a, ADMIN.EXAM_SPECMST b ";
                SQL += ComNum.VBLF + " WHERE b.RECEIVEDATE >= TO_DATE('" + argSDATE + "','YYYY-MM-DD') "; 
                SQL += ComNum.VBLF + "   AND b.RECEIVEDATE < TO_DATE('" + Convert.ToDateTime(argEDATE).AddDays(1).ToShortDateString() + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND a.GBJOB ='V' "; 
                SQL += ComNum.VBLF + "   AND a.SPECNO = b.SPECNO "; 
                SQL += ComNum.VBLF + "   AND a.MASTERCODE ='XR34' "; // '위내시경
                SQL += ComNum.VBLF + "   AND a.ANATNO LIKE 'S%' "; //   '조직

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    nCnt1 += 1; //전체인원

                    nTime = DATE_TIME(dt.Rows[i]["JDATE"].ToString().Trim(), dt.Rows[i]["ResultDate"].ToString().Trim());

                    if (nTime > 4320)
                    {
                        nCnt2 += 1;
                        //해당명단
                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DrCode,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',2,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["AGE"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "',";
                        SQL = SQL + "  TO_DATE('" + dt.Rows[i]["JDate"].ToString().Trim() + " ','YYYY-MM-DD HH24:MI'),";
                        SQL = SQL + "  TO_DATE('" + dt.Rows[i]["ResultDate"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                //'총건수
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 진단검사의학과 자궁세포진 검사
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns>
        bool Build_27202_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;

            long nCnt1 = 0;
            long nCnt2 = 0;
            long nTime = 0;

            string strCODE = "27202";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //    '자궁세포진 검사
                SQL = "SELECT b.Pano, b.SName, b.Age, b.Sex, b.Room RoomCode, b.Ward WardCode, b.DrCode, b.DeptCode, ";
                SQL += ComNum.VBLF + "  TO_CHAR(a.ResultDate,'YYYY-MM-DD HH24:MI') ResultDate,  TO_CHAR(a.JDate,'YYYY-MM-DD HH24:MI') JDate ";
                SQL += ComNum.VBLF + "  FROM ADMIN.EXAM_ANATMST a, ADMIN.EXAM_SPECMST b ";
                SQL += ComNum.VBLF + " WHERE b.RECEIVEDATE >= TO_DATE('" + argSDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND b.RECEIVEDATE < TO_DATE('" + Convert.ToDateTime(argEDATE).AddDays(1).ToShortDateString() + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND a.GBJOB ='V' ";
                SQL += ComNum.VBLF + "   AND a.SPECNO = b.SPECNO ";
                SQL += ComNum.VBLF + " AND A.MASTERCODE LIKE 'YY%' ";//          '자궁세포진
                SQL += ComNum.VBLF + " AND A.ANATNO LIKE 'C%' ";// '세포검사만

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    nCnt1 += 1; //전체인원

                    nTime = DATE_TIME(dt.Rows[i]["JDate"].ToString().Trim(), dt.Rows[i]["ResultDate"].ToString().Trim());

                    if (nTime > 2880)
                    {
                        nCnt2 += 1;
                        //해당명단
                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DrCode,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',2,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["AGE"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "',";
                        SQL = SQL + "  TO_DATE('" + dt.Rows[i]["JDate"].ToString().Trim() + " ','YYYY-MM-DD HH24:MI'),";
                        SQL = SQL + "  TO_DATE('" + dt.Rows[i]["ResultDate"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                //'총건수
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 진단검사의학과 체액세포병리 검사
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns>
        bool Build_27203_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;

            long nCnt1 = 0;
            long nCnt2 = 0;
            long nTime = 0;

            string strCODE = "27203";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'체액세포병리 검사
                SQL = "  SELECT b.Pano, b.SName, b.Age, b.Sex, b.Room RoomCode, b.Ward WardCode, b.DrCode, b.DeptCode, ";
                SQL += ComNum.VBLF + " TO_CHAR(a.ResultDate,'YYYY-MM-DD HH24:MI') ResultDate,  TO_CHAR(a.JDate,'YYYY-MM-DD HH24:MI') JDate ";
                SQL += ComNum.VBLF + "  FROM ADMIN.EXAM_ANATMST a, ADMIN.EXAM_SPECMST b ";
                SQL += ComNum.VBLF + " WHERE b.RECEIVEDATE >= TO_DATE('" + argSDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND b.RECEIVEDATE < TO_DATE('" + Convert.ToDateTime(argEDATE).AddDays(1).ToShortDateString() + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND a.GBJOB ='V' ";
                SQL += ComNum.VBLF + "   AND a.SPECNO = b.SPECNO ";
                SQL += ComNum.VBLF + " AND A.SPECCODE IN ('081','021','022','023','044','042','082','052','057')  ";
                SQL += ComNum.VBLF + " AND A.ANATNO LIKE 'C%' ";// '세포검사만

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    nCnt1 += 1; //전체인원

                    nTime = DATE_TIME(dt.Rows[i]["JDate"].ToString().Trim(), dt.Rows[i]["ResultDate"].ToString().Trim());

                    if (nTime > 2880)
                    {
                        nCnt2 += 1;
                        //해당명단
                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DrCode,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',2,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["AGE"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "',";
                        SQL = SQL + "  TO_DATE('" + dt.Rows[i]["JDate"].ToString().Trim() + " ','YYYY-MM-DD HH24:MI'),";
                        SQL = SQL + "  TO_DATE('" + dt.Rows[i]["ResultDate"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                //'총건수
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 진단검사의학과 혈액배양 검사
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns>
        bool Build_27204_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;

            long nCnt1 = 0;
            long nCnt2 = 0;
            long nTime = 0;

            string strCODE = "27204";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = " SELECT a.SpecNo,a.Pano,a.SName,a.IpdOpd,a.DeptCode, A.WARD, A.ROOM,";
                SQL += ComNum.VBLF + " TO_CHAR(a.BloodDate,'YYYY-MM-DD HH24:MI') BloodDate, ";
                SQL += ComNum.VBLF + " TO_CHAR(a.OrderDate,'YYYY-MM-DD HH24:MI') OrderDate, TO_CHAR(a.ReceiveDate,'YYYY-MM-DD HH24:MI') ReceiveDate, ";
                SQL += ComNum.VBLF + " TO_CHAR(a.ReceiveDate,'YYYY-MM-DD PM HH24:MI') ReceiveDate2, TO_CHAR(a.ResultDate,'YYYY-MM-DD HH24:MI') ResultDate,  ";
                SQL += ComNum.VBLF + " TO_CHAR(a.ResultDate,'YYYY-MM-DD PM HH24:MI') ResultDate2, DECODE(B.SUBCODE,'MI34','MI321','MI32','MI321', B.SUBCODE) ";
                SQL += ComNum.VBLF + " SUBCODE, COUNT(*) CNT  FROM ADMIN.EXAM_SPECMST a, ADMIN.EXAM_RESULTC b ";
                SQL += ComNum.VBLF + " WHERE a.ReceiveDate >= TO_DATE('" + argSDATE + "','YYYY-MM-DD')   ";
                SQL += ComNum.VBLF + " AND a.ReceiveDate <= TO_DATE('" + Convert.ToDateTime(argEDATE).AddDays(1).ToShortDateString() + "','YYYY-MM-DD')   ";
                SQL += ComNum.VBLF + " AND (a.Cancel IS NULL OR a.Cancel=' ')   AND a.Status='05'   AND a.SpecNo=b.SpecNo  ";
                SQL += ComNum.VBLF + " AND b.MASTERCode IN ('MI34','MI32')  AND B.SUBCODE IN ( 'MI34','MI32')  AND A.SPECCODE ='010' ";
                SQL += ComNum.VBLF + " AND UPPER(B.RESULT) LIKE '%ESCHERICHIA%'   ";
                SQL += ComNum.VBLF + " AND B.SPECNO NOT IN ( SELECT SPECNO FROM ADMIN.EXAM_RESULTC C ";
                SQL += ComNum.VBLF + " WHERE A.SPECNO = C.SPECNO AND C.MASTERCODE IN ('HR10') )  ";
                SQL += ComNum.VBLF + " GROUP BY a.SpecNo,a.Pano,a.SName,a.IpdOpd,a.DeptCode,        ";
                SQL += ComNum.VBLF + " a.BloodDate,a.orderdate,a.ReceiveDate,a.ResultDate, DECODE(B.SUBCODE,'MI34','MI321','MI32','MI321', B.SUBCODE), ";
                SQL += ComNum.VBLF + " A.WARD, A.ROOM  ORDER BY a.ReceiveDate,a.SpecNo";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    nCnt1 += 1; //전체인원

                    nTime = DATE_TIME(dt.Rows[i]["JDate"].ToString().Trim(), dt.Rows[i]["ResultDate"].ToString().Trim());

                    if (nTime > 5760)
                    {
                        nCnt2 += 1;
                        //해당명단
                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DrCode,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',2,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["AGE"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "',";
                        SQL = SQL + "  TO_DATE('" + dt.Rows[i]["ReceiveDate"].ToString().Trim() + " ','YYYY-MM-DD HH24:MI'),";
                        SQL = SQL + "  TO_DATE('" + dt.Rows[i]["ResultDate"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                //'총건수
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 혈액관리 효율성 반납량
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns>
        bool Build_27401_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;

            long nCnt1 = 0;
            long nCnt2 = 0;
            long nCnt3 = 0;

            string strCODE = "27401";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "SELECT TO_CHAR(a.BDATE,'YYYY-MM-DD') BDATE, a.GBJOB, ";
                SQL += ComNum.VBLF + "  a.Pano,b.SName,b.Sex ";
                SQL += ComNum.VBLF + "  FROM ADMIN.EXAM_BLOODTRANS a, ADMIN.BAS_PATIENT b ";
                SQL += ComNum.VBLF + " WHERE a.Pano=b.Pano ";
                SQL += ComNum.VBLF + "   AND a.BDATE >= TO_DATE('" + argSDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND a.BDATE < TO_DATE('" + Convert.ToDateTime(argSDATE).AddDays(1).ToShortDateString() + "','YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    nCnt1 += 1; //전체인원

                    if (dt.Rows[i]["GBJOB"].ToString().Trim() == "5")
                    {
                        nCnt2 += 1;
                        //해당명단
                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,Time1) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',2,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "', " + "  TO_DATE('" + dt.Rows[i]["BDate"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }

                    if (dt.Rows[i]["GBJOB"].ToString().Trim() == "7")
                    {
                        nCnt3 += 1;
                        //해당명단
                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,Time1) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',3,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "', " + "  TO_DATE('" + dt.Rows[i]["BDate"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                //'총건수
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',3," + nCnt3 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 혈액관리 효율성 폐기량
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns> 
        bool Build_27402_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;

            long nCnt1 = 0;
            long nCnt2 = 0;

            string strCODE = "27402";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "SELECT TO_CHAR(a.BDATE,'YYYY-MM-DD') BDATE, a.GBJOB, ";
                SQL += ComNum.VBLF + "  a.Pano,b.SName,b.Sex ";
                SQL += ComNum.VBLF + "  FROM ADMIN.EXAM_BLOODTRANS a, ADMIN.BAS_PATIENT b ";
                SQL += ComNum.VBLF + " WHERE a.Pano=b.Pano ";
                SQL += ComNum.VBLF + "   AND a.BDATE >= TO_DATE('" + argSDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND a.BDATE < TO_DATE('" + Convert.ToDateTime(argSDATE).AddDays(1).ToShortDateString() + "','YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    nCnt1 += 1; //전체인원

                    if (dt.Rows[i]["GBJOB"].ToString().Trim() == "7")
                    {
                        nCnt2 += 1;
                        //해당명단
                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,Time1) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',2,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + "  TO_DATE('" + dt.Rows[i]["BDate"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                //'총건수
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 응급지원서비스 응급혈액검사
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns> 
        bool Build_25601_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;

            string SQL_SELECT = "DECODE(B.SUBCODE,'HR01','HR011','HR02','HR011', B.SUBCODE) SUBCODE,";
            string SQL_AND = " AND b.SUBCode IN ('HR01','HR02') ";
            string SQL_GROUP = "DECODE(B.SUBCODE,'HR01','HR011','HR02','HR011', B.SUBCODE) ,";

            long nCnt1 = 0;
            long nCnt2 = 0;

            string strCODE = "25601";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'자료를 SELECT
                SQL = "SELECT a.SpecNo,a.Pano,a.SName,a.Sex,a.IpdOpd,a.DeptCode,a.DrCode, A.WARD WardCode, A.ROOM RoomCode,";
                SQL += ComNum.VBLF + " TO_CHAR(a.BloodDate,'YYYY-MM-DD HH24:MI') BloodDate,";
                SQL += ComNum.VBLF + " TO_CHAR(a.OrderDate,'YYYY-MM-DD HH24:MI') OrderDate,";
                SQL += ComNum.VBLF + " TO_CHAR(a.OrderDate,'YYYY-MM-DD PM HH24:MI') OrderDate2,";
                SQL += ComNum.VBLF + " TO_CHAR(a.ReceiveDate,'YYYY-MM-DD HH24:MI') ReceiveDate,";
                SQL += ComNum.VBLF + " TO_CHAR(a.ReceiveDate,'YYYY-MM-DD PM HH24:MI') ReceiveDate2,";
                SQL += ComNum.VBLF + " TO_CHAR(a.ResultDate,'YYYY-MM-DD HH24:MI') ResultDate, ";
                SQL += ComNum.VBLF + " TO_CHAR(a.ResultDate,'YYYY-MM-DD PM HH24:MI') ResultDate2, ";
                SQL += ComNum.VBLF + SQL_SELECT;///       '검사코드 셋1
                SQL += ComNum.VBLF + " COUNT(*) CNT ";
                SQL += ComNum.VBLF + " FROM ADMIN.EXAM_SPECMST a, ADMIN.EXAM_RESULTC b ";
                SQL += ComNum.VBLF + "WHERE a.ReceiveDate >= TO_DATE('" + argSDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  AND a.ReceiveDate <= TO_DATE('" + argEDATE + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL += ComNum.VBLF + "  AND (a.Cancel IS NULL OR a.Cancel=' ') ";
                SQL += ComNum.VBLF + "  AND a.Status='05' ";
                SQL += ComNum.VBLF + "  AND a.DeptCode='ER' ";
                SQL += ComNum.VBLF + "  AND a.SpecNo=b.SpecNo";
                SQL += ComNum.VBLF + SQL_AND;//       '검사코드 셋2
                SQL += ComNum.VBLF + " AND B.SPECNO NOT IN ( SELECT SPECNO FROM ADMIN.EXAM_RESULTC C WHERE A.SPECNO = C.SPECNO";// 'PB 검사는 제외
                SQL += ComNum.VBLF + " AND C.MASTERCODE IN ('HR10') )";
                SQL += ComNum.VBLF + "GROUP BY a.SpecNo,a.Pano,a.SName,a.Sex,a.IpdOpd,a.DeptCode,a.DrCode,";
                SQL += ComNum.VBLF + "         a.BloodDate,a.orderdate,a.ReceiveDate,a.ResultDate, ";
                SQL += ComNum.VBLF + SQL_GROUP;//       '검사코드 셋3
                SQL += ComNum.VBLF + " A.WARD, A.ROOM  ";
                SQL += ComNum.VBLF + "ORDER BY a.ReceiveDate,a.SpecNo ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    nCnt1 += 1; //전체인원

                    if (DATE_TIME(dt.Rows[i]["BloodDate"].ToString().Trim(), dt.Rows[i]["ResultDate"].ToString().Trim()) > 15)
                    {
                        nCnt2 += 1;
                        //해당명단
                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',2,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "',";
                        SQL = SQL + "TO_DATE('" + dt.Rows[i]["BloodDate"].ToString().Trim() + "', 'YYYY-MM-DD HH24:MI')" + ", ";
                        SQL = SQL + "TO_DATE('" + dt.Rows[i]["ResultDate"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                //'총건수
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 응급지원서비스 응급화학검사
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns> 
        bool Build_25602_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;

            string SQL_SELECT = " B.SUBCODE ,";
            string SQL_AND = " AND b.SUBCODE ='CR01'";
            string SQL_GROUP = " B.SUBCODE ,";

            long nCnt1 = 0;
            long nCnt2 = 0;

            string strCODE = "25602";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'자료를 SELECT
                SQL = "SELECT a.SpecNo,a.Pano,a.SName,a.Sex,a.IpdOpd,a.DeptCode,a.DrCode, A.WARD WardCode, A.ROOM RoomCode,";
                SQL += ComNum.VBLF + " TO_CHAR(a.BloodDate,'YYYY-MM-DD HH24:MI') BloodDate,";
                SQL += ComNum.VBLF + " TO_CHAR(a.OrderDate,'YYYY-MM-DD HH24:MI') OrderDate,";
                SQL += ComNum.VBLF + " TO_CHAR(a.OrderDate,'YYYY-MM-DD PM HH24:MI') OrderDate2,";
                SQL += ComNum.VBLF + " TO_CHAR(a.ReceiveDate,'YYYY-MM-DD HH24:MI') ReceiveDate,";
                SQL += ComNum.VBLF + " TO_CHAR(a.ReceiveDate,'YYYY-MM-DD PM HH24:MI') ReceiveDate2,";
                SQL += ComNum.VBLF + " TO_CHAR(a.ResultDate,'YYYY-MM-DD HH24:MI') ResultDate, ";
                SQL += ComNum.VBLF + " TO_CHAR(a.ResultDate,'YYYY-MM-DD PM HH24:MI') ResultDate2, ";
                SQL += ComNum.VBLF + SQL_SELECT;       //'검사코드 셋1
                SQL += ComNum.VBLF + " COUNT(*) CNT ";
                SQL += ComNum.VBLF + " FROM ADMIN.EXAM_SPECMST a, ADMIN.EXAM_RESULTC b ";
                SQL += ComNum.VBLF + "WHERE a.ReceiveDate >= TO_DATE('" + argSDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  AND a.ReceiveDate <= TO_DATE('" + argEDATE + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL += ComNum.VBLF + "  AND (a.Cancel IS NULL OR a.Cancel=' ') ";
                SQL += ComNum.VBLF + "  AND a.Status='05' ";
                SQL += ComNum.VBLF + "  AND a.DeptCode='ER' ";
                SQL += ComNum.VBLF + "  AND a.SpecNo=b.SpecNo";
                SQL += ComNum.VBLF + SQL_AND;  //    '검사코드 셋2;
                SQL += ComNum.VBLF + " AND B.SPECNO NOT IN ( SELECT SPECNO FROM ADMIN.EXAM_RESULTC C WHERE A.SPECNO = C.SPECNO";// 'PB 검사는 제외
                SQL += ComNum.VBLF + " AND C.MASTERCODE IN ('HR10') )";
                SQL += ComNum.VBLF + "GROUP BY a.SpecNo,a.Pano,a.SName,a.Sex,a.IpdOpd,a.DeptCode,a.DrCode,";
                SQL += ComNum.VBLF + "         a.BloodDate,a.orderdate,a.ReceiveDate,a.ResultDate, ";
                SQL += ComNum.VBLF + SQL_GROUP;//      '검사코드 셋3
                SQL += ComNum.VBLF + " A.WARD, A.ROOM  ";
                SQL += ComNum.VBLF + "ORDER BY a.ReceiveDate,a.SpecNo ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    nCnt1 += 1; //전체인원

                    if (DATE_TIME(dt.Rows[i]["BloodDate"].ToString().Trim(), dt.Rows[i]["ResultDate"].ToString().Trim()) > 35)
                    {
                        nCnt2 += 1;
                        //해당명단
                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',2,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                        SQL = SQL + "TO_DATE('" + dt.Rows[i]["BloodDate"].ToString().Trim() + "', 'YYYY-MM-DD HH24:MI')" + ",";
                        SQL = SQL + "TO_DATE('" + dt.Rows[i]["ResultDate"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                //'총건수
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 응급지원서비스 응급동멱핼겸사
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns> 
        bool Build_25603_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;

            string SQL_AND = " AND b.MASTERCode IN ('CR61','CR62') ";
            string SQL_SELECT = "DECODE(B.MASTERCODE,'CR61','CR23','CR62','CR23', B.MASTERCODE) SUBCODE,";
            string SQL_GROUP = "DECODE(B.MASTERCODE,'CR61','CR23','CR62','CR23', B.MASTERCODE), ";

            long nCnt1 = 0;
            long nCnt2 = 0;

            string strCODE = "25603";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'자료를 SELECT
                SQL = "SELECT a.SpecNo,a.Pano,a.SName,a.Sex,a.IpdOpd,a.DeptCode,a.DrCode, A.WARD WardCode, A.ROOM RoomCode,";
                SQL += ComNum.VBLF + " TO_CHAR(a.BloodDate,'YYYY-MM-DD HH24:MI') BloodDate,";
                SQL += ComNum.VBLF + " TO_CHAR(a.OrderDate,'YYYY-MM-DD HH24:MI') OrderDate,";
                SQL += ComNum.VBLF + " TO_CHAR(a.OrderDate,'YYYY-MM-DD PM HH24:MI') OrderDate2,";
                SQL += ComNum.VBLF + " TO_CHAR(a.ReceiveDate,'YYYY-MM-DD HH24:MI') ReceiveDate,";
                SQL += ComNum.VBLF + " TO_CHAR(a.ReceiveDate,'YYYY-MM-DD PM HH24:MI') ReceiveDate2,";
                SQL += ComNum.VBLF + " TO_CHAR(a.ResultDate,'YYYY-MM-DD HH24:MI') ResultDate, ";
                SQL += ComNum.VBLF + " TO_CHAR(a.ResultDate,'YYYY-MM-DD PM HH24:MI') ResultDate2, ";
                SQL += ComNum.VBLF + SQL_SELECT;//       '검사코드 셋1
                SQL += ComNum.VBLF + " COUNT(*) CNT ";
                SQL += ComNum.VBLF + " FROM ADMIN.EXAM_SPECMST a, ADMIN.EXAM_RESULTC b ";
                SQL += ComNum.VBLF + "WHERE a.ReceiveDate >= TO_DATE('" + argSDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  AND a.ReceiveDate <= TO_DATE('" + argEDATE + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL += ComNum.VBLF + "  AND (a.Cancel IS NULL OR a.Cancel=' ') ";
                SQL += ComNum.VBLF + "  AND a.Status='05' ";
                SQL += ComNum.VBLF + "  AND a.DeptCode='ER' ";
                SQL += ComNum.VBLF + "  AND a.SpecNo=b.SpecNo";
                SQL += ComNum.VBLF + SQL_AND;//       '검사코드 셋2
                SQL += ComNum.VBLF + " AND B.SPECNO NOT IN ( SELECT SPECNO FROM ADMIN.EXAM_RESULTC C WHERE A.SPECNO = C.SPECNO";// 'PB 검사는 제외
                SQL += ComNum.VBLF + " AND C.MASTERCODE IN ('HR10') )";
                SQL += ComNum.VBLF + "GROUP BY a.SpecNo,a.Pano,a.SName,a.Sex,a.IpdOpd,a.DeptCode,a.DrCode,";
                SQL += ComNum.VBLF + "         a.BloodDate,a.orderdate,a.ReceiveDate,a.ResultDate, ";
                SQL += ComNum.VBLF + SQL_GROUP;//       '검사코드 셋3
                SQL += ComNum.VBLF + " A.WARD, A.ROOM  ";
                SQL += ComNum.VBLF + "ORDER BY a.ReceiveDate,a.SpecNo ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    nCnt1 += 1; //전체인원

                    if (DATE_TIME(dt.Rows[i]["BloodDate"].ToString().Trim(), dt.Rows[i]["ResultDate"].ToString().Trim()) > 14)
                    {
                        nCnt2 += 1;
                        //해당명단
                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',2,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "',";
                        SQL = SQL + "TO_DATE('" + dt.Rows[i]["BloodDate"].ToString().Trim() + "', 'YYYY-MM-DD HH24:MI')" + ",";
                        SQL = SQL + "TO_DATE('" + dt.Rows[i]["ResultDate"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                //'총건수
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 응급지원서비스 응급요검사
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns> 
        bool Build_25604_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;

            string SQL_SELECT = "DECODE(B.SUBCODE,'UR02','UR001','UR03','UR001', B.SUBCODE) SUBCODE,";
            string SQL_AND = " AND b.SUBCode IN ('UR02','UR03') " + " AND B.SPECNO NOT IN ('0508200778','0508100970','0509221260','0509060537','0509160664')";
            string SQL_GROUP = "DECODE(B.SUBCODE,'UR02','UR001','UR03','UR001', B.SUBCODE) ,";

            long nCnt1 = 0;
            long nCnt2 = 0;

            string strCODE = "25604";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'자료를 SELECT
                SQL = "SELECT a.SpecNo,a.Pano,a.SName,a.Sex,a.IpdOpd,a.DeptCode,a.DrCode, A.WARD WardCode, A.ROOM RoomCode,";
                SQL += ComNum.VBLF +  " TO_CHAR(a.BloodDate,'YYYY-MM-DD HH24:MI') BloodDate,";
                SQL += ComNum.VBLF +  " TO_CHAR(a.OrderDate,'YYYY-MM-DD HH24:MI') OrderDate,";
                SQL += ComNum.VBLF +  " TO_CHAR(a.OrderDate,'YYYY-MM-DD PM HH24:MI') OrderDate2,";
                SQL += ComNum.VBLF +  " TO_CHAR(a.ReceiveDate,'YYYY-MM-DD HH24:MI') ReceiveDate,";
                SQL += ComNum.VBLF +  " TO_CHAR(a.ReceiveDate,'YYYY-MM-DD PM HH24:MI') ReceiveDate2,";
                SQL += ComNum.VBLF +  " TO_CHAR(a.ResultDate,'YYYY-MM-DD HH24:MI') ResultDate, ";
                SQL += ComNum.VBLF +  " TO_CHAR(a.ResultDate,'YYYY-MM-DD PM HH24:MI') ResultDate2, ";
                SQL += ComNum.VBLF +  SQL_SELECT;//       '검사코드 셋1
                SQL += ComNum.VBLF +  " COUNT(*) CNT ";
                SQL += ComNum.VBLF +  " FROM ADMIN.EXAM_SPECMST a, ADMIN.EXAM_RESULTC b ";
                SQL += ComNum.VBLF +  "WHERE a.ReceiveDate >= TO_DATE('" + argSDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF +  "  AND a.ReceiveDate <= TO_DATE('" + argEDATE + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL += ComNum.VBLF +  "  AND (a.Cancel IS NULL OR a.Cancel=' ') ";
                SQL += ComNum.VBLF +  "  AND a.Status='05' ";
                SQL += ComNum.VBLF +  "  AND a.DeptCode='ER' ";
                SQL += ComNum.VBLF +  "  AND a.SpecNo=b.SpecNo";
                SQL += ComNum.VBLF +  SQL_AND;//       '검사코드 셋2
                SQL += ComNum.VBLF +  " AND B.SPECNO NOT IN ( SELECT SPECNO FROM ADMIN.EXAM_RESULTC C WHERE A.SPECNO = C.SPECNO";// 'PB 검사는 제외
                SQL += ComNum.VBLF +  " AND C.MASTERCODE IN ('HR10') )";
                SQL += ComNum.VBLF +  "GROUP BY a.SpecNo,a.Pano,a.SName,a.Sex,a.IpdOpd,a.DeptCode,a.DrCode,";
                SQL += ComNum.VBLF +  "         a.BloodDate,a.orderdate,a.ReceiveDate,a.ResultDate, ";
                SQL += ComNum.VBLF +  SQL_GROUP;//       '검사코드 셋3
                SQL += ComNum.VBLF +  " A.WARD, A.ROOM  ";
                SQL += ComNum.VBLF + "ORDER BY a.ReceiveDate,a.SpecNo ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    nCnt1 += 1; //전체인원

                    if (DATE_TIME(dt.Rows[i]["BloodDate"].ToString().Trim(), dt.Rows[i]["ResultDate"].ToString().Trim()) > 9)
                    {
                        nCnt2 += 1;
                        //해당명단
                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',2,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "',";
                        SQL = SQL + "TO_DATE('" + dt.Rows[i]["BloodDate"].ToString().Trim() + "', 'YYYY-MM-DD HH24:MI')" + ",";
                        SQL = SQL + "TO_DATE('" + dt.Rows[i]["ResultDate"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                //'총건수
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 응급지원서비스 단순방사선검사
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns> 
        bool Build_25605_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;

            long nCnt1 = 0;
            long nCnt2 = 0;

            string strCODE = "25605";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "SELECT a.Pano,b.SName,TO_CHAR(a.SeekDate,'YYYY-MM-DD') ResultDate,a.DeptCode,";
                SQL += ComNum.VBLF + "  a.Sex,a.Age,a.WardCode,a.RoomCode,a.DrCode, ";
                SQL += ComNum.VBLF + " a.DrCode,c.DrName,d.XName,a.ExInfo,a.PacsNo,a.XJong,a.XCode,a.IpdOpd,a.OrderNo,";
                SQL += ComNum.VBLF + " TO_CHAR(a.EnterDate,'YYYY-MM-DD HH24:MI') Bday,";
                SQL += ComNum.VBLF + " TO_CHAR(a.SeekDate,'YYYY-MM-DD HH24:MI') JepsuTime,";
                SQL += ComNum.VBLF + " TO_CHAR(a.XSendDate,'YYYY-MM-DD HH24:MI') XSendDate,";
                SQL += ComNum.VBLF + " TO_CHAR(a.ORDERDATE,'YYYY-MM-DD HH24:MI') ORDERDATE,";
                SQL += ComNum.VBLF + " TO_CHAR(a.SENDDATE,'YYYY-MM-DD HH24:MI') SENDDATE,";
                SQL += ComNum.VBLF + " a.PacsStudyID,a.OrderName,a.Remark,a.ROWID ";
                SQL += ComNum.VBLF + " FROM ADMIN.XRAY_DETAIL a,ADMIN.BAS_PATIENT b,ADMIN.BAS_DOCTOR c,";
                SQL += ComNum.VBLF + "      ADMIN.XRAY_CODE d ";
                SQL += ComNum.VBLF + "WHERE a.SeekDate>=TO_DATE('" + argSDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  AND a.SeekDate<=TO_DATE('" + argEDATE + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL += ComNum.VBLF + "  AND a.GbReserved >='6' ";// '접수 또는 촬영완료
                SQL += ComNum.VBLF + "  AND a.XCode NOT IN ('XCDC') ";
                SQL += ComNum.VBLF + " AND a.DeptCode='ER' ";
                SQL += ComNum.VBLF + " AND a.XJong = '1' ";
                SQL += ComNum.VBLF + " AND a.XSubCode IN ('01','02') ";
                SQL += ComNum.VBLF + "  AND a.Pano=b.Pano(+) ";
                SQL += ComNum.VBLF + "  AND a.DrCode=c.DrCode(+) ";
                SQL += ComNum.VBLF + "  AND a.XCode=d.XCode(+) ";
                SQL += ComNum.VBLF + "ORDER BY a.Pano,a.SeekDate,a.XJong,a.XCode ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    nCnt1 += 1; //전체인원

                    if (DATE_TIME(dt.Rows[i]["ORDERDATE"].ToString().Trim(), dt.Rows[i]["SENDDATE"].ToString().Trim()) > 10)
                    {
                        nCnt2 += 1;
                        //해당명단
                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',2,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "',";
                        SQL = SQL + "TO_DATE('" + dt.Rows[i]["ORDERDATE"].ToString().Trim() + "', 'YYYY-MM-DD HH24:MI')" + ",";
                        SQL = SQL + "TO_DATE('" + dt.Rows[i]["SENDDATE"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                //'총건수
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// SPINE MRI
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns> 
        bool Build_28301_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;
            DataTable dt2 = null;

            long nCnt1 = 0;
            long nCnt2 = 0;
            long nCnt3 = 0;

            string strCODE = "28301";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'Brain MRI
                SQL = "SELECT a.Pano,b.SName,TO_CHAR(a.SeekDate,'YYYY-MM-DD') ResultDate,a.DeptCode,";
                SQL += ComNum.VBLF + "  a.Sex,a.Age,a.WardCode,a.RoomCode,a.DrCode, ";
                SQL += ComNum.VBLF + " a.DrCode,c.DrName,d.XName,a.ExInfo,a.PacsNo,a.XJong,a.XCode,a.IpdOpd,a.OrderNo,";
                SQL += ComNum.VBLF + " TO_CHAR(a.EnterDate,'YYYY-MM-DD HH24:MI') Bday,";
                SQL += ComNum.VBLF + " TO_CHAR(a.SeekDate,'YYYY-MM-DD HH24:MI') JepsuTime,";
                SQL += ComNum.VBLF + " TO_CHAR(a.XSendDate,'YYYY-MM-DD HH24:MI') XSendDate,";
                SQL += ComNum.VBLF + " a.PacsStudyID,a.OrderName,a.Remark,a.ROWID ";
                SQL += ComNum.VBLF + " FROM ADMIN.XRAY_DETAIL a,ADMIN.BAS_PATIENT b,ADMIN.BAS_DOCTOR c,";
                SQL += ComNum.VBLF + "      ADMIN.XRAY_CODE d ";
                SQL += ComNum.VBLF + " WHERE a.SeekDate>=TO_DATE('" + argSDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  AND a.SeekDate<=TO_DATE('"  + argEDATE + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL += ComNum.VBLF + "  AND a.GbReserved >='6' ";// '접수 또는 촬영완료
                SQL += ComNum.VBLF + "  AND a.XCode NOT IN ('XCDC') ";

                //'-------------------------------------------------
                SQL += ComNum.VBLF + " AND a.IpdOpd='I' ";
                SQL += ComNum.VBLF + " AND a.XJong = '5' ";
                SQL += ComNum.VBLF + " AND a.XSubCode = '01' ";// 'Brain MRI
                //'-------------------------------------------------

                SQL += ComNum.VBLF + "  AND a.Pano=b.Pano(+) ";
                SQL += ComNum.VBLF + "  AND a.DrCode=c.DrCode(+) ";
                SQL += ComNum.VBLF + "  AND a.XCode=d.XCode(+) ";
                SQL += ComNum.VBLF + "ORDER BY a.Pano,a.SeekDate,a.XJong,a.XCode ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    nCnt1 += 1; //전체인원

                    SQL = " SELECT TO_CHAR(ReadDate,'YYYY-MM-DD') ReadDate FROM ADMIN.XRAY_RESULTNEW ";
                    SQL += ComNum.VBLF + " WHERE WRTNO = " + dt.Rows[i]["Exinfo"].ToString().Trim() + " ";

                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (DATE_TIME(dt.Rows[i]["ResultDate"].ToString().Trim(), dt.Rows[i]["BDay"].ToString().Trim()) > 1)
                    {
                        nCnt2 += 1;
                        //해당명단
                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',2,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "',";
                        SQL = SQL + " " + "TO_DATE('" + dt.Rows[i]["BDay"].ToString().Trim() + "', 'YYYY-MM-DD HH24:MI')" + ",";
                        SQL = SQL + " " + "TO_DATE('" + dt.Rows[i]["ResultDate"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }

                    if(dt2.Rows.Count > 0)
                    {
                        if(DATE_TIME(dt2.Rows[0]["ReadDate"].ToString().Trim(), dt.Rows[i]["ResultDate"].ToString().Trim()) > 0)
                        {
                            nCnt3 += 1;
                            //해당명단
                            SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                            SQL = SQL + ArgYYMM + "','" + strCODE + "',3,";
                            SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "',";
                            SQL = SQL + " " + "TO_DATE('" + dt.Rows[i]["ResultDate"].ToString().Trim() + "', 'YYYY-MM-DD HH24:MI')" + ",";
                            SQL = SQL + " " + "TO_DATE('" + dt2.Rows[0]["ReadDate"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }
                    }

                    dt2.Dispose();
                    dt2 = null;
                }

                dt.Dispose();
                dt = null;

                //'총건수
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',3," + nCnt3 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// ABDOMEN USG
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns> 
        bool Build_28302_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;
            DataTable dt2 = null;

            long nCnt1 = 0;
            long nCnt2 = 0;
            long nCnt3 = 0;

            string strCODE = "28302";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'ABDOMEN USG
                SQL = "SELECT a.Pano,b.SName,TO_CHAR(a.SeekDate,'YYYY-MM-DD') ResultDate,a.DeptCode,";
                SQL += ComNum.VBLF + "  a.Sex,a.Age,a.WardCode,a.RoomCode,a.DrCode, ";
                SQL += ComNum.VBLF + " a.DrCode,c.DrName,d.XName,a.ExInfo,a.PacsNo,a.XJong,a.XCode,a.IpdOpd,a.OrderNo,";
                SQL += ComNum.VBLF + " TO_CHAR(a.EnterDate,'YYYY-MM-DD HH24:MI') Bday,";
                SQL += ComNum.VBLF + " TO_CHAR(a.SeekDate,'YYYY-MM-DD HH24:MI') JepsuTime,";
                SQL += ComNum.VBLF + " TO_CHAR(a.XSendDate,'YYYY-MM-DD HH24:MI') XSendDate,";
                SQL += ComNum.VBLF + " a.PacsStudyID,a.OrderName,a.Remark,a.ROWID ";
                SQL += ComNum.VBLF + " FROM ADMIN.XRAY_DETAIL a,ADMIN.BAS_PATIENT b,ADMIN.BAS_DOCTOR c,";
                SQL += ComNum.VBLF + "      ADMIN.XRAY_CODE d ";
                SQL += ComNum.VBLF + "WHERE a.SeekDate>=TO_DATE('" + argSDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  AND a.SeekDate<=TO_DATE('" + argEDATE + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL += ComNum.VBLF + "  AND a.GbReserved >='6' ";// '접수 또는 촬영완료
                SQL += ComNum.VBLF + "  AND a.XCode NOT IN ('XCDC') ";
                SQL += ComNum.VBLF + " AND a.IpdOpd='I' ";
                SQL += ComNum.VBLF + " AND a.XJong = '3' ";// '초음파
                SQL += ComNum.VBLF + " AND a.XSubCode IN ('01','02') ";// 'Abdomen
                SQL += ComNum.VBLF + "  AND a.Pano=b.Pano(+) ";
                SQL += ComNum.VBLF + "  AND a.DrCode=c.DrCode(+) ";
                SQL += ComNum.VBLF + "  AND a.XCode=d.XCode(+) ";
                SQL += ComNum.VBLF + "ORDER BY a.Pano,a.SeekDate,a.XJong,a.XCode ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    nCnt1 += 1; //전체인원

                    SQL = " SELECT TO_CHAR(ReadDate,'YYYY-MM-DD') ReadDate FROM ADMIN.XRAY_RESULTNEW ";
                    SQL += ComNum.VBLF + " WHERE WRTNO = " + dt.Rows[i]["Exinfo"].ToString().Trim() + " ";

                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (DATE_TIME(dt.Rows[i]["ResultDate"].ToString().Trim(), dt.Rows[i]["BDay"].ToString().Trim()) > 1)
                    {
                        nCnt2 += 1;
                        //해당명단
                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',2,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "',";
                        SQL = SQL + "TO_DATE('" + dt.Rows[i]["BDay"].ToString().Trim() + "', 'YYYY-MM-DD HH24:MI')" + ",";
                        SQL = SQL + "TO_DATE('" + dt.Rows[i]["ResultDate"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }

                    if (dt2.Rows.Count > 0)
                    {
                        if (DATE_TIME(dt2.Rows[0]["ReadDate"].ToString().Trim(), dt.Rows[i]["ResultDate"].ToString().Trim()) > 0)
                        {
                            nCnt3 += 1;
                            //해당명단
                            SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                            SQL = SQL + ArgYYMM + "','" + strCODE + "',3,";
                            SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "',";
                            SQL = SQL + "TO_DATE('" + dt.Rows[i]["ResultDate"].ToString().Trim() + "', 'YYYY-MM-DD HH24:MI')" + ",";
                            SQL = SQL + "TO_DATE('" + dt2.Rows[0]["ReadDate"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }
                    }

                    dt2.Dispose();
                    dt2 = null;
                }

                dt.Dispose();
                dt = null;

                //'총건수
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',3," + nCnt3 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// BRIAN CT
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns> 
        bool Build_28303_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;
            DataTable dt2 = null;

            long nCnt1 = 0;
            long nCnt2 = 0;
            long nCnt3 = 0;

            string strCODE = "28303";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                //'Chest CT
                SQL = "SELECT a.Pano,b.SName,TO_CHAR(a.SeekDate,'YYYY-MM-DD') ResultDate,a.DeptCode,";
                SQL += ComNum.VBLF + "  a.Sex,a.Age,a.WardCode,a.RoomCode,a.DrCode, ";
                SQL += ComNum.VBLF + " a.DrCode,c.DrName,d.XName,a.ExInfo,a.PacsNo,a.XJong,a.XCode,a.IpdOpd,a.OrderNo,";
                SQL += ComNum.VBLF + " TO_CHAR(a.EnterDate,'YYYY-MM-DD HH24:MI') Bday,";
                SQL += ComNum.VBLF + " TO_CHAR(a.SeekDate,'YYYY-MM-DD HH24:MI') JepsuTime,";
                SQL += ComNum.VBLF + " TO_CHAR(a.XSendDate,'YYYY-MM-DD HH24:MI') XSendDate,";
                SQL += ComNum.VBLF + " a.PacsStudyID,a.OrderName,a.Remark,a.ROWID ";
                SQL += ComNum.VBLF + " FROM ADMIN.XRAY_DETAIL a,ADMIN.BAS_PATIENT b,ADMIN.BAS_DOCTOR c,";
                SQL += ComNum.VBLF + "      ADMIN.XRAY_CODE d ";
                SQL += ComNum.VBLF + "WHERE a.SeekDate>=TO_DATE('" + argSDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  AND a.SeekDate<=TO_DATE('" + argEDATE + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL += ComNum.VBLF + "  AND a.GbReserved >='6' ";// '접수 또는 촬영완료
                SQL += ComNum.VBLF + "  AND a.XCode NOT IN ('XCDC') ";
                SQL += ComNum.VBLF + " AND a.IpdOpd='I' ";
                SQL += ComNum.VBLF + " AND a.XJong = '4' ";
                SQL += ComNum.VBLF + " AND a.XSubCode = '03' ";// 'Chest CT
                SQL += ComNum.VBLF + "  AND a.Pano=b.Pano(+) ";
                SQL += ComNum.VBLF + "  AND a.DrCode=c.DrCode(+) ";
                SQL += ComNum.VBLF + "  AND a.XCode=d.XCode(+) ";
                SQL += ComNum.VBLF + "ORDER BY a.Pano,a.SeekDate,a.XJong,a.XCode ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    nCnt1 += 1; //전체인원

                    SQL = " SELECT TO_CHAR(ReadDate,'YYYY-MM-DD') ReadDate FROM ADMIN.XRAY_RESULTNEW ";
                    SQL += ComNum.VBLF + " WHERE WRTNO = " + dt.Rows[i]["Exinfo"].ToString().Trim() + " ";

                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (DATE_TIME(dt.Rows[i]["ResultDate"].ToString().Trim(), dt.Rows[i]["BDay"].ToString().Trim()) > 1)
                    {
                        nCnt2 += 1;
                        //해당명단
                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',2,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "',";
                        SQL = SQL + " " + "TO_DATE('" + dt.Rows[i]["BDay"].ToString().Trim() + "', 'YYYY-MM-DD HH24:MI')" + ",";
                        SQL = SQL + " " + "TO_DATE('" + dt.Rows[i]["ResultDate"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }

                    if (dt2.Rows.Count > 0)
                    {
                        if (DATE_TIME(dt2.Rows[0]["ReadDate"].ToString().Trim(), dt.Rows[i]["ResultDate"].ToString().Trim()) > 0)
                        {
                            nCnt3 += 1;
                            //해당명단
                            SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                            SQL = SQL + ArgYYMM + "','" + strCODE + "',3,";
                            SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "',";
                            SQL = SQL + "TO_DATE('" + dt.Rows[i]["ResultDate"].ToString().Trim() + "', 'YYYY-MM-DD HH24:MI')" + ",";
                            SQL = SQL + "TO_DATE('" + dt2.Rows[0]["ReadDate"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }
                    }

                    dt2.Dispose();
                    dt2 = null;
                }

                dt.Dispose();
                dt = null;

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',3," + nCnt3 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// UGIS
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns> 
        bool Build_28304_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;
            DataTable dt2 = null;

            long nCnt1 = 0;
            long nCnt2 = 0;
            long nCnt3 = 0;

            string strCODE = "28304";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                //'UGIS
                SQL = "SELECT a.Pano,b.SName,TO_CHAR(a.SeekDate,'YYYY-MM-DD') ResultDate,a.DeptCode,";
                SQL += ComNum.VBLF + "  a.Sex,a.Age,a.WardCode,a.RoomCode,a.DrCode, ";
                SQL += ComNum.VBLF + " a.DrCode,c.DrName,d.XName,a.ExInfo,a.PacsNo,a.XJong,a.XCode,a.IpdOpd,a.OrderNo,";
                SQL += ComNum.VBLF + " TO_CHAR(a.EnterDate,'YYYY-MM-DD HH24:MI') Bday,";
                SQL += ComNum.VBLF + " TO_CHAR(a.SeekDate,'YYYY-MM-DD HH24:MI') JepsuTime,";
                SQL += ComNum.VBLF + " TO_CHAR(a.XSendDate,'YYYY-MM-DD HH24:MI') XSendDate,";
                SQL += ComNum.VBLF + " a.PacsStudyID,a.OrderName,a.Remark,a.ROWID ";
                SQL += ComNum.VBLF + " FROM ADMIN.XRAY_DETAIL a,ADMIN.BAS_PATIENT b,ADMIN.BAS_DOCTOR c,";
                SQL += ComNum.VBLF + "      ADMIN.XRAY_CODE d ";
                SQL += ComNum.VBLF + "WHERE a.SeekDate>=TO_DATE('" + argSDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  AND a.SeekDate<=TO_DATE('" + argEDATE + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL += ComNum.VBLF + "  AND a.GbReserved >='6' ";// '접수 또는 촬영완료
                SQL += ComNum.VBLF + "  AND a.XCode NOT IN ('XCDC') ";
                SQL += ComNum.VBLF + " AND a.IpdOpd='I' ";
                SQL += ComNum.VBLF + " AND a.XJong = '2' ";// '특수촬영
                SQL += ComNum.VBLF + " AND a.XSubCode = '02' ";// 'U.G.I
                SQL += ComNum.VBLF + "  AND a.Pano=b.Pano(+) ";
                SQL += ComNum.VBLF + "  AND a.DrCode=c.DrCode(+) ";
                SQL += ComNum.VBLF + "  AND a.XCode=d.XCode(+) ";
                SQL += ComNum.VBLF + "ORDER BY a.Pano,a.SeekDate,a.XJong,a.XCode ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    nCnt1 += 1; //전체인원

                    SQL = " SELECT TO_CHAR(ReadDate,'YYYY-MM-DD') ReadDate FROM ADMIN.XRAY_RESULTNEW ";
                    SQL += ComNum.VBLF + " WHERE WRTNO = " + dt.Rows[i]["Exinfo"].ToString().Trim() + " ";

                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (DATE_TIME(dt.Rows[i]["ResultDate"].ToString().Trim(), dt.Rows[i]["BDay"].ToString().Trim()) > 1)
                    {
                        nCnt2 += 1;
                        //해당명단
                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',2,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "',";
                        SQL = SQL + "TO_DATE('" + dt.Rows[i]["BDay"].ToString().Trim() + "', 'YYYY-MM-DD HH24:MI')" + ",";
                        SQL = SQL + "TO_DATE('" + dt.Rows[i]["ResultDate"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }

                    if (dt2.Rows.Count > 0)
                    {
                        if (DATE_TIME(dt2.Rows[0]["ReadDate"].ToString().Trim(), dt.Rows[i]["ResultDate"].ToString().Trim()) > 0)
                        {
                            nCnt3 += 1;
                            //해당명단
                            SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                            SQL = SQL + ArgYYMM + "','" + strCODE + "',3,";
                            SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "',";
                            SQL = SQL + " " + "TO_DATE('" + dt.Rows[i]["ResultDate"].ToString().Trim() + "', 'YYYY-MM-DD HH24:MI')" + ",";
                            SQL = SQL + " " + "TO_DATE('" + dt2.Rows[0]["ReadDate"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }
                    }

                    dt2.Dispose();
                    dt2 = null;
                }

                dt.Dispose();
                dt = null;

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',3," + nCnt3 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 혈관조영
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns> 
        bool Build_28305_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;
            DataTable dt2 = null;

            long nCnt1 = 0;
            long nCnt2 = 0;
            long nCnt3 = 0;

            string strCODE = "28305";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                //'혈관조영
                SQL = "SELECT a.Pano,b.SName,TO_CHAR(a.SeekDate,'YYYY-MM-DD') ResultDate,a.DeptCode,";
                SQL += ComNum.VBLF + "  a.Sex,a.Age,a.WardCode,a.RoomCode,a.DrCode, ";
                SQL += ComNum.VBLF + " a.DrCode,c.DrName,d.XName,a.ExInfo,a.PacsNo,a.XJong,a.XCode,a.IpdOpd,a.OrderNo,";
                SQL += ComNum.VBLF + " TO_CHAR(a.EnterDate,'YYYY-MM-DD HH24:MI') Bday,";
                SQL += ComNum.VBLF + " TO_CHAR(a.SeekDate,'YYYY-MM-DD HH24:MI') JepsuTime,";
                SQL += ComNum.VBLF + " TO_CHAR(a.XSendDate,'YYYY-MM-DD HH24:MI') XSendDate,";
                SQL += ComNum.VBLF + " a.PacsStudyID,a.OrderName,a.Remark,a.ROWID ";
                SQL += ComNum.VBLF + " FROM ADMIN.XRAY_DETAIL a,ADMIN.BAS_PATIENT b,ADMIN.BAS_DOCTOR c,";
                SQL += ComNum.VBLF + "      ADMIN.XRAY_CODE d ";
                SQL += ComNum.VBLF + "WHERE a.SeekDate>=TO_DATE('" + argSDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  AND a.SeekDate<=TO_DATE('" + argEDATE + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL += ComNum.VBLF + "  AND a.GbReserved >='6' ";// '접수 또는 촬영완료
                SQL += ComNum.VBLF + "  AND a.XCode NOT IN ('XCDC') ";
                SQL += ComNum.VBLF + " AND a.IpdOpd='I' ";
                SQL += ComNum.VBLF + " AND a.XJong = '2' ";// '특수촬영
                SQL += ComNum.VBLF + " AND a.XCode IN (SELECT XCode FROM ADMIN.XRAY_CODE ";
                SQL += ComNum.VBLF + "                  WHERE ClassCode='2' ";
                SQL += ComNum.VBLF + "                    AND SubCode IN ('36','64')) ";
                SQL += ComNum.VBLF + "  AND a.Pano=b.Pano(+) ";
                SQL += ComNum.VBLF + "  AND a.DrCode=c.DrCode(+) ";
                SQL += ComNum.VBLF + "  AND a.XCode=d.XCode(+) ";
                SQL += ComNum.VBLF + "ORDER BY a.Pano,a.SeekDate,a.XJong,a.XCode ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    nCnt1 += 1; //전체인원

                    SQL = " SELECT TO_CHAR(ReadDate,'YYYY-MM-DD') ReadDate FROM ADMIN.XRAY_RESULTNEW ";
                    SQL += ComNum.VBLF + " WHERE WRTNO = " + dt.Rows[i]["Exinfo"].ToString().Trim() + " ";

                    if (DATE_TIME(dt.Rows[i]["ResultDate"].ToString().Trim(), dt.Rows[i]["BDay"].ToString().Trim()) > 1)
                    {
                        nCnt2 += 1;
                        //해당명단
                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',2,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "','";
                        SQL = SQL + "TO_DATE('" + dt.Rows[i]["BDay"].ToString().Trim() + "', 'YYYY-MM-DD HH24:MI')" + ",";
                        SQL = SQL + "TO_DATE('" + dt.Rows[i]["ResultDate"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }

                    if (dt2.Rows.Count > 0)
                    {
                        if (DATE_TIME(dt2.Rows[0]["ReadDate"].ToString().Trim(), dt.Rows[i]["ResultDate"].ToString().Trim()) > 0)
                        {
                            nCnt3 += 1;
                            //해당명단
                            SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                            SQL = SQL + ArgYYMM + "','" + strCODE + "',3,";
                            SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "',";
                            SQL = SQL + "TO_DATE('" + dt.Rows[i]["ResultDate"].ToString().Trim() + "', 'YYYY-MM-DD HH24:MI')" + ",";
                            SQL = SQL + "TO_DATE('" + dt2.Rows[0]["ReadDate"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }
                    }

                    dt2.Dispose();
                    dt2 = null;
                }

                dt.Dispose();
                dt = null;

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',3," + nCnt3 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 대장조영
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns> 
        bool Build_28306_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;
            DataTable dt2 = null;

            long nCnt1 = 0;
            long nCnt2 = 0;
            long nCnt3 = 0;

            string strCODE = "28306";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                //'UGIS
                SQL = "SELECT a.Pano,b.SName,TO_CHAR(a.SeekDate,'YYYY-MM-DD') ResultDate,a.DeptCode,";
                SQL += ComNum.VBLF + "  a.Sex,a.Age,a.WardCode,a.RoomCode,a.DrCode, ";
                SQL += ComNum.VBLF + " a.DrCode,c.DrName,d.XName,a.ExInfo,a.PacsNo,a.XJong,a.XCode,a.IpdOpd,a.OrderNo,";
                SQL += ComNum.VBLF + " TO_CHAR(a.EnterDate,'YYYY-MM-DD HH24:MI') Bday,";
                SQL += ComNum.VBLF + " TO_CHAR(a.SeekDate,'YYYY-MM-DD HH24:MI') JepsuTime,";
                SQL += ComNum.VBLF + " TO_CHAR(a.XSendDate,'YYYY-MM-DD HH24:MI') XSendDate,";
                SQL += ComNum.VBLF + " a.PacsStudyID,a.OrderName,a.Remark,a.ROWID ";
                SQL += ComNum.VBLF + " FROM ADMIN.XRAY_DETAIL a,ADMIN.BAS_PATIENT b,ADMIN.BAS_DOCTOR c,";
                SQL += ComNum.VBLF + "      ADMIN.XRAY_CODE d ";
                SQL += ComNum.VBLF + "WHERE a.SeekDate>=TO_DATE('" + argSDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  AND a.SeekDate<=TO_DATE('" + argEDATE + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL += ComNum.VBLF + "  AND a.GbReserved >='6' ";// '접수 또는 촬영완료
                SQL += ComNum.VBLF + "  AND a.XCode NOT IN ('XCDC') ";
                SQL += ComNum.VBLF + " AND a.IpdOpd='I' ";
                SQL += ComNum.VBLF + " AND a.XJong = '2' ";// '특수촬영
                SQL += ComNum.VBLF + " AND a.XSubCode = '05' ";// '대장조영
                SQL += ComNum.VBLF + "  AND a.Pano=b.Pano(+) ";
                SQL += ComNum.VBLF + "  AND a.DrCode=c.DrCode(+) ";
                SQL += ComNum.VBLF + "  AND a.XCode=d.XCode(+) ";
                SQL += ComNum.VBLF + "ORDER BY a.Pano,a.SeekDate,a.XJong,a.XCode ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    nCnt1 += 1; //전체인원

                    SQL = " SELECT TO_CHAR(ReadDate,'YYYY-MM-DD') ReadDate FROM ADMIN.XRAY_RESULTNEW ";
                    SQL += ComNum.VBLF + " WHERE WRTNO = " + dt.Rows[i]["Exinfo"].ToString().Trim() + " ";

                    if (DATE_TIME(dt.Rows[i]["ResultDate"].ToString().Trim(), dt.Rows[i]["BDay"].ToString().Trim()) > 1)
                    {
                        nCnt2 += 1;
                        //해당명단
                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',2,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "',";
                        SQL = SQL + "TO_DATE('" + dt.Rows[i]["BDay"].ToString().Trim() + "', 'YYYY-MM-DD HH24:MI')" + ",";
                        SQL = SQL + "TO_DATE('" + dt.Rows[i]["ResultDate"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }

                    if (dt2.Rows.Count > 0)
                    {
                        if (DATE_TIME(dt2.Rows[0]["ReadDate"].ToString().Trim(), dt.Rows[i]["ResultDate"].ToString().Trim()) > 0)
                        {
                            nCnt3 += 1;
                            //해당명단
                            SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                            SQL = SQL + ArgYYMM + "','" + strCODE + "',3,";
                            SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "',";
                            SQL = SQL + "TO_DATE('" + dt.Rows[i]["ResultDate"].ToString().Trim() + "', 'YYYY-MM-DD HH24:MI')" + ",";
                            SQL = SQL + "TO_DATE('" + dt2.Rows[0]["ReadDate"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }
                    }

                    dt2.Dispose();
                    dt2 = null;
                }

                dt.Dispose();
                dt = null;

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',3," + nCnt3 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// abdomen CT
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns> 
        bool Build_28307_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;
            DataTable dt2 = null;

            long nCnt1 = 0;
            long nCnt2 = 0;
            long nCnt3 = 0;

            string strCODE = "28307";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                //'UGIS
                SQL = "SELECT a.Pano,b.SName,TO_CHAR(a.SeekDate,'YYYY-MM-DD') ResultDate,a.DeptCode,";
                SQL += ComNum.VBLF + "  a.Sex,a.Age,a.WardCode,a.RoomCode,a.DrCode, ";
                SQL += ComNum.VBLF + " a.DrCode,c.DrName,d.XName,a.ExInfo,a.PacsNo,a.XJong,a.XCode,a.IpdOpd,a.OrderNo,";
                SQL += ComNum.VBLF + " TO_CHAR(a.EnterDate,'YYYY-MM-DD HH24:MI') Bday,";
                SQL += ComNum.VBLF + " TO_CHAR(a.SeekDate,'YYYY-MM-DD HH24:MI') JepsuTime,";
                SQL += ComNum.VBLF + " TO_CHAR(a.XSendDate,'YYYY-MM-DD HH24:MI') XSendDate,";
                SQL += ComNum.VBLF + " a.PacsStudyID,a.OrderName,a.Remark,a.ROWID ";
                SQL += ComNum.VBLF + " FROM ADMIN.XRAY_DETAIL a,ADMIN.BAS_PATIENT b,ADMIN.BAS_DOCTOR c,";
                SQL += ComNum.VBLF + "      ADMIN.XRAY_CODE d ";
                SQL += ComNum.VBLF + "WHERE a.SeekDate>=TO_DATE('" + argSDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  AND a.SeekDate<=TO_DATE('" + argEDATE + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL += ComNum.VBLF + "  AND a.GbReserved >='6' ";// '접수 또는 촬영완료
                SQL += ComNum.VBLF + "  AND a.XCode NOT IN ('XCDC') ";
                SQL += ComNum.VBLF + " AND a.IpdOpd='I' ";
                SQL += ComNum.VBLF + " AND a.XJong = '4' ";// '특수촬영
                SQL += ComNum.VBLF + " AND a.XSubCode = '02' ";// 'U.G.I
                SQL += ComNum.VBLF + " AND SUBSTR(a.XCode,1,5) ='HA475' ";// 'U.G.I
                SQL += ComNum.VBLF + "  AND a.Pano=b.Pano(+) ";
                SQL += ComNum.VBLF + "  AND a.DrCode=c.DrCode(+) ";
                SQL += ComNum.VBLF + "  AND a.XCode=d.XCode(+) ";
                SQL += ComNum.VBLF + "ORDER BY a.Pano,a.SeekDate,a.XJong,a.XCode ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    nCnt1 += 1; //전체인원

                    SQL = " SELECT TO_CHAR(ReadDate,'YYYY-MM-DD') ReadDate FROM ADMIN.XRAY_RESULTNEW ";
                    SQL += ComNum.VBLF + " WHERE WRTNO = " + dt.Rows[i]["Exinfo"].ToString().Trim() + " ";

                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }


                    if (DATE_TIME(dt.Rows[i]["ResultDate"].ToString().Trim(), dt.Rows[i]["BDay"].ToString().Trim()) > 1)
                    {
                        nCnt2 += 1;
                        //해당명단
                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',2,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "',";
                        SQL = SQL + "TO_DATE('" + dt.Rows[i]["BDay"].ToString().Trim() + "', 'YYYY-MM-DD HH24:MI')" + ",";
                        SQL = SQL + "TO_DATE('" + dt.Rows[i]["ResultDate"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }

                    if (dt2.Rows.Count > 0)
                    {
                        if (DATE_TIME(dt2.Rows[0]["ReadDate"].ToString().Trim(), dt.Rows[i]["ResultDate"].ToString().Trim()) > 0)
                        {
                            nCnt3 += 1;
                            //해당명단
                            SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                            SQL = SQL + ArgYYMM + "','" + strCODE + "',3,";
                            SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "',";
                            SQL = SQL + "TO_DATE('" + dt.Rows[i]["ResultDate"].ToString().Trim() + "', 'YYYY-MM-DD HH24:MI')" + ",";
                            SQL = SQL + "TO_DATE('" + dt2.Rows[0]["ReadDate"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }
                    }

                    dt2.Dispose();
                    dt2 = null;
                }

                dt.Dispose();
                dt = null;

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',3," + nCnt3 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 중증뇌질환
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns> 
        bool Build_93011_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;
            DataTable dt2 = null;
            DataTable dt3 = null;

            long nCnt1 = 0;
            long nCnt2 = 0;
            long nCnt3 = 0;
            long nCnt4 = 0;
            long nCnt5 = 0;
            long nCnt6 = 0;
            long nCnt7 = 0;
            long nCnt8 = 0;
            long nCnt9 = 0;
            long nCnt10 = 0;
            long nCnt11 = 0;

            string strCODE = "93011";

            string strSPTMIINDT = ArgYYMM + "01";
            string strEPTMIINDT = clsVbfunc.LastDay(Convert.ToInt32(VB.Left(strSPTMIINDT, 4)), Convert.ToInt32(VB.Mid(strSPTMIINDT, 5, 2)));
            strEPTMIINDT = ArgYYMM + VB.Right(strEPTMIINDT, 2);

            string strTemp = "";

            string strPano = "";
            string strInDate = "";

            string strSDATE = "";
            string strEDATE = "";

            string strSDateT = "";
            string strEDateT = "";
            //string strTDateT = "";
            string strFDateT = "";

            string strDRCODE = "";

            string strTempDate1 = "";
            string strTempDate2 = "";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                SQL = " SELECT A.IDNO, A.INDT, A.INTM FROM ";
                SQL += ComNum.VBLF + " (SELECT A.DGOTIDNO IDNO, A.DGOTINDT INDT, A.DGOTINTM INTM                       ";
                SQL += ComNum.VBLF + "   FROM NUR_ER_EMIHDGOT A, (SELECT MAX(SEQNO) SEQNO, PTMIIDNO, PTMIINDT, PTMIINTM, PTMISTAT ";
                SQL += ComNum.VBLF + "      From NUR_ER_EMIHPTMI                                                       ";
                SQL += ComNum.VBLF + "      WHERE PTMIINDT >= '" + strSPTMIINDT + "'                                   ";
                SQL += ComNum.VBLF + "        AND PTMIINDT <= '" + strEPTMIINDT + "'                                   ";
                SQL += ComNum.VBLF + "      GROUP BY PTMIIDNO , PTMIINDT, PTMIINTM, PTMISTAT                           ";
                SQL += ComNum.VBLF + "      HAVING PTMISTAT <> 'D') B                                                  ";
                SQL += ComNum.VBLF + "      WHERE A.DGOTINDT >= '" + strSPTMIINDT + "'                                 ";
                SQL += ComNum.VBLF + "        AND A.DGOTINDT <= '" + strEPTMIINDT + "'                                 ";
                SQL += ComNum.VBLF + "    AND A.DGOTINDT = B.PTMIINDT                                                  ";
                SQL += ComNum.VBLF + "    AND A.DGOTINTM = B.PTMIINTM                                                  ";
                SQL += ComNum.VBLF + "    AND A.DGOTIDNO = B.PTMIIDNO                                                  ";
                SQL += ComNum.VBLF + "    AND A.DGOTDIAG >= 'I60'                                                      ";
                SQL += ComNum.VBLF + "    AND A.DGOTDIAG <= 'I640'                                                     ";
                SQL += ComNum.VBLF + "   GROUP BY A.DGOTIDNO, A.DGOTINDT, A.DGOTINTM                                   ";
                SQL += ComNum.VBLF +  " UNION ALL ";
                SQL += ComNum.VBLF + " SELECT A.DGDCIDNO IDNO, A.DGDCINDT INDT, A.DGDCINTM INTM                        ";
                SQL += ComNum.VBLF + "   FROM NUR_ER_EMIHDGDC A, (SELECT MAX(SEQNO) SEQNO, PTMIIDNO, PTMIINDT, PTMIINTM, PTMISTAT ";
                SQL += ComNum.VBLF + "      From NUR_ER_EMIHPTMI                                                       ";
                SQL += ComNum.VBLF + "      WHERE PTMIINDT >= '" + strSPTMIINDT + "'                                   ";
                SQL += ComNum.VBLF + "        AND PTMIINDT <= '" + strEPTMIINDT + "'                                   ";
                SQL += ComNum.VBLF + "      GROUP BY PTMIIDNO , PTMIINDT, PTMIINTM, PTMISTAT                           ";
                SQL += ComNum.VBLF + "      HAVING PTMISTAT <> 'D') B                                                  ";
                SQL += ComNum.VBLF + "      WHERE A.DGDCINDT >= '" + strSPTMIINDT + "'                                 ";
                SQL += ComNum.VBLF + "        AND A.DGDCINDT <= '" + strEPTMIINDT + "'                                 ";
                SQL += ComNum.VBLF + "    AND A.DGDCINDT = B.PTMIINDT                                                  ";
                SQL += ComNum.VBLF + "    AND A.DGDCINTM = B.PTMIINTM                                                  ";
                SQL += ComNum.VBLF + "    AND A.DGDCIDNO = B.PTMIIDNO                                                  ";
                SQL += ComNum.VBLF + "    AND A.DGDCDIAG >= 'I60'                                                      ";
                SQL += ComNum.VBLF + "    AND A.DGDCDIAG <= 'I640'                                                     ";
                SQL += ComNum.VBLF + "   GROUP BY A.DGDCIDNO, A.DGDCINDT, A.DGDCINTM) A                                ";
                SQL += ComNum.VBLF + "   GROUP BY A.IDNO, A.INDT, A.INTM";


                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    nCnt1 += 1; //전체인원
               
                    SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                    SQL = SQL + ArgYYMM + "','" + strCODE + "',1,";
                    SQL = SQL + " '" + dt.Rows[i]["IDNO"].ToString().Trim() + "','', ";
                    SQL = SQL + " '','ER', ";
                    SQL = SQL + " '',000,";
                    SQL = SQL + " '', ";
                    SQL = SQL + "  TO_DATE('" + strSPTMIINDT + "','YYYY-MM-DD HH24:MI'),";
                    SQL = SQL + "  TO_DATE('" + strEPTMIINDT + "','YYYY-MM-DD HH24:MI')  ) ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }


                    strPano = dt.Rows[i]["IDNO"].ToString().Trim();
                    strInDate = dt.Rows[i]["INDT"].ToString().Trim();


                    strSDATE = VB.Left(ArgYYMM, 4) +  "-" + VB.Right(ArgYYMM, 2) + "-01";
                    strEDATE = clsVbfunc.LastDay(Convert.ToInt32(VB.Left(strSPTMIINDT, 4)), Convert.ToInt32(VB.Mid(strSPTMIINDT, 5, 2)));

                    SQL = " SELECT A.PTMIIDNO, A.PTMINAME, A.PTMIBRTD, A.PTMISEXX, 'ER' DEPTCODE, '' WARDCODE, '' ROOMCODE,          ";
                    SQL += ComNum.VBLF + "  A.PTMIAKDT, A.PTMIAKTM, A.PTMIDCDT, A.PTMIDCTM, A.PTMIOTDT, A.PTMIOTTM, A.PTMIINDT, A.PTMIINTM,  ";
                    SQL += ComNum.VBLF + "  A.PTMIDRLC , A.PTMIEMRT, A.PTMISTAT, A.PTMIEMRT                                                  ";
                    SQL += ComNum.VBLF + "       FROM NUR_ER_EMIHPTMI A, (                                                                   ";
                    SQL += ComNum.VBLF + "  SELECT MAX(SEQNO) SEQNO, PTMIIDNO, PTMIINDT, PTMIINTM                                            ";
                    SQL += ComNum.VBLF + "  From NUR_ER_EMIHPTMI                                                                             ";
                    SQL += ComNum.VBLF + "  WHERE PTMIIDNO = '" + dt.Rows[i]["IDNO"].ToString().Trim() + "'                                           ";
                    SQL += ComNum.VBLF + "    AND PTMIINDT = '" + dt.Rows[i]["INDT"].ToString().Trim() + "'                                           ";
                    SQL += ComNum.VBLF + "    AND PTMIINTM = '" + dt.Rows[i]["INTM"].ToString().Trim() + "'                                           ";
                    SQL += ComNum.VBLF + "  GROUP BY PTMIIDNO, PTMIINDT, PTMIINTM) B                                                         ";
                    SQL += ComNum.VBLF + "         WHERE A.PTMIDGKD = '1'                                                                    ";
                    SQL += ComNum.VBLF + "         AND A.SEQNO = B.SEQNO                                                                     ";
                    SQL += ComNum.VBLF + "         AND A.PTMIIDNO = B.PTMIIDNO                                                               ";
                    SQL += ComNum.VBLF + "         AND A.PTMIINDT = B.PTMIINDT                                                               ";
                    SQL += ComNum.VBLF + "         AND A.PTMIINTM = B.PTMIINTM                                                               ";
                    SQL += ComNum.VBLF + "       ORDER BY A.PTMISTAT, A.PTMIINDT, A.PTMIINTM                                                 ";

                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if(dt2.Rows.Count > 0)
                    {
                        strDRCODE = READ_LicToDRCode(dt2.Rows[0]["PTMIDRLC"].ToString().Trim());

                        nCnt2 += 1;

                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',2,";
                        SQL = SQL + " '" + dt2.Rows[0]["PTMIIDNO"].ToString().Trim() + "','" + dt2.Rows[0]["PTMINAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt2.Rows[0]["PTMISEXX"].ToString().Trim() + "','" + dt2.Rows[0]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt2.Rows[0]["WARDCODE"].ToString().Trim() + "','" + dt2.Rows[0]["ROOMCODE"].ToString().Trim() + "',";
                        SQL = SQL + " '" + strDRCODE + "', ";
                        SQL = SQL + "  TO_DATE('" + strSDATE + "','YYYY-MM-DD HH24:MI'),";
                        SQL = SQL + "  TO_DATE('" + strEDATE + "','YYYY-MM-DD HH24:MI')  ) ";//  '응급환자 모니터링 P/G에서 조인용으로 사용함.(내원일시)

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }


                        //'발병일시
                        strTemp = dt2.Rows[0]["PTMIAKDT"].ToString().Trim();
                        strSDateT = VB.Left(strTemp, 4) + "-" + VB.Mid(strTemp, 5, 2) + "-" + VB.Right(strTemp, 2);
                        strTemp = dt2.Rows[0]["PTMIAKTM"].ToString().Trim();
                        strSDateT = strSDateT + " " + VB.Left(strTemp, 2) + ":" + VB.Right(strTemp, 2);

                        //'내원일시
                        strTemp = dt2.Rows[0]["PTMIINDT"].ToString().Trim();
                        strEDateT = VB.Left(strTemp, 4) + "-" + VB.Mid(strTemp, 5, 2) + "-" + VB.Right(strTemp, 2);
                        strTemp = dt2.Rows[0]["PTMIINTM"].ToString().Trim();
                        strEDateT = strEDateT + " " + VB.Left(strTemp, 2) + ":" + VB.Right(strTemp, 2);

                        strFDateT = "";


                        SQL = " SELECT HOJINDATE1 FROM NUR_ER_PATIENT ";
                        SQL += ComNum.VBLF + " WHERE PANO = '" + strPano + "' ";
                        SQL += ComNum.VBLF + "   AND INTIME = TO_DATE('" + strEDateT + "','YYYY-MM-DD HH24:MI') ";

                        SqlErr = clsDB.GetDataTable(ref dt3, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        if(dt3.Rows.Count > 0)
                        {
                            strFDateT = Convert.ToDateTime(dt3.Rows[0]["HOJINDATE1"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm");
                        }

                        dt3.Dispose();
                        dt3 = null;



                        if(DATE_TIME(strSDateT, strEDateT) <= 10080)
                        {
                            switch(dt2.Rows[0]["PTMIEMRT"].ToString().Trim())
                            {
                                case "31":
                                case "32":
                                case "33":
                                case "34":
                                case "38":
                                    nCnt3 += 1;

                                    SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                                    SQL = SQL + ArgYYMM + "','" + strCODE + "',3,";
                                    SQL = SQL + " '" + dt2.Rows[0]["PTMIIDNO"].ToString().Trim() + "','" + dt2.Rows[0]["PTMINAME"].ToString().Trim() + "', ";
                                    SQL = SQL + " '" + dt2.Rows[0]["PTMISEXX"].ToString().Trim() + "','" + dt2.Rows[0]["DEPTCODE"].ToString().Trim() + "', ";
                                    SQL = SQL + " '" + dt2.Rows[0]["WARDCODE"].ToString().Trim() + "','" + dt2.Rows[0]["ROOMCODE"].ToString().Trim() + "', ";
                                    SQL = SQL + " '" + strDRCODE + "', ";
                                    SQL = SQL + "TO_DATE('" + strSDateT + "','YYYY-MM-DD HH24:MI'),";
                                    SQL = SQL + "TO_DATE('" + strEDateT + "','YYYY-MM-DD HH24:MI')  ) ";

                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        ComFunc.MsgBox(SqlErr);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        Cursor.Current = Cursors.Default;
                                        return rtnVal;
                                    }
                                    break;
                            }
                        }


                        if (DATE_TIME(strSDateT, strEDateT) <= 180 && DATE_TIME(strSDateT, strEDateT) >= 0)
                        {
                            nCnt4 += 1;

                            SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                            SQL = SQL + ArgYYMM + "','" + strCODE + "',4,";
                            SQL = SQL + " '" + dt2.Rows[0]["PTMIIDNO"].ToString().Trim() + "','" + dt2.Rows[0]["PTMINAME"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt2.Rows[0]["PTMISEXX"].ToString().Trim() + "','" + dt2.Rows[0]["DEPTCODE"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt2.Rows[0]["WARDCODE"].ToString().Trim() + "'," + dt2.Rows[0]["ROOMCODE"].ToString().Trim() + ",";
                            SQL = SQL + " '" + strDRCODE + "', ";
                            SQL = SQL + "  TO_DATE('" + strSDateT + "','YYYY-MM-DD HH24:MI'),";
                            SQL = SQL + "  TO_DATE('" + strEDateT + "','YYYY-MM-DD HH24:MI')  ) ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }



                            //'혈전용해술(응급실 프로그램에서 가져온 자료임. 정도관리)
                            SQL = " SELECT REMARK1 ";
                            SQL = SQL + ComNum.VBLF + " FROM NUR_ER_JUNGDO ";
                            SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND JDATE = TO_DATE('" + strInDate + "', 'YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "   AND GUBUN = '3' ";
                            SQL = SQL + ComNum.VBLF + "   AND REMARK1 IS NOT NULL ";
                            SQL = SQL + ComNum.VBLF + " GROUP BY REMARK1 ";

                            SqlErr = clsDB.GetDataTable(ref dt3, SQL, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }

                            if (dt3.Rows.Count > 0)
                            {
                                if (DATE_TIME(strEDateT, dt3.Rows[0]["REMARK1"].ToString().Trim()) <= 30 && DATE_TIME(strEDateT, dt3.Rows[0]["REMARK1"].ToString().Trim()) >= 0)
                                {
                                    nCnt5 += 1;

                                    SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                                    SQL = SQL + ArgYYMM + "','" + strCODE + "',5,";
                                    SQL = SQL + " '" + dt2.Rows[0]["PTMIIDNO"].ToString().Trim() + "','" + dt2.Rows[0]["PTMINAME"].ToString().Trim() + "', ";
                                    SQL = SQL + " '" + dt2.Rows[0]["PTMISEXX"].ToString().Trim() + "','" + dt2.Rows[0]["DEPTCODE"].ToString().Trim() + "', ";
                                    SQL = SQL + " '" + dt2.Rows[0]["WARDCODE"].ToString().Trim() + "'," + dt2.Rows[0]["ROOMCODE"].ToString().Trim() + ",";
                                    SQL = SQL + " '" + strDRCODE + "', ";
                                    SQL = SQL + "  TO_DATE('" + strEDateT + "','YYYY-MM-DD HH24:MI'),";
                                    SQL = SQL + "  TO_DATE('" + dt3.Rows[0]["REMARK2"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        ComFunc.MsgBox(SqlErr);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        Cursor.Current = Cursors.Default;
                                        return rtnVal;
                                    }

                                }
                                strFDateT = Convert.ToDateTime(dt3.Rows[0]["HOJINDATE1"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm");
                            }

                            dt3.Dispose();
                            dt3 = null;


                            // '혈전용해술(EORDER 에서 가져온 자료임.)

                            if(DateTime.Compare(Convert.ToDateTime(strEDateT), Convert.ToDateTime("2009-08-04")) > 0)
                            {
                                SQL = " SELECT TO_DATE(BDATE, 'YYYY-MM-DD HH24:MI') BDATE, PTNO, ENTDATE, QTY, NAL FROM ADMIN.OCS_IORDER";
                            }
                            else
                            {
                                SQL = " SELECT TO_DATE(BDATE, 'YYYY-MM-DD HH24:MI') BDATE, PTNO, ENTDATE, QTY, NAL FROM ADMIN.OCS_EORDER";
                            }

                            SQL = SQL + ComNum.VBLF + " WHERE BDATE >= TO_DATE('" + Convert.ToDateTime(strEDateT).ToShortDateString() + "','YYYY-MM-DD')";
                            SQL = SQL + ComNum.VBLF + "   AND BDATE <= TO_DATE('" + Convert.ToDateTime(strEDateT).AddDays(2).ToShortDateString() + "','YYYY-MM-DD')";
                            if (DateTime.Compare(Convert.ToDateTime(strEDateT), Convert.ToDateTime("2009-08-04")) > 0)
                            {
                                SQL = SQL + ComNum.VBLF + " AND GBIOE IN ('E','EI') ";
                            }


                            SQL = SQL + ComNum.VBLF + "   AND SUCODE IN ('TPA2', 'TPA5', 'UROK2', 'UROK10', 'UROK50')";
                            SQL = SQL + ComNum.VBLF + "   AND PTNO = '" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + " ORDER BY ENTDATE ASC ";

                            SqlErr = clsDB.GetDataTable(ref dt3, SQL, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }

                            if (dt3.Rows.Count > 0)
                            {
                                nCnt6 += 1;

                                SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                                SQL = SQL + ArgYYMM + "','" + strCODE + "',6,";
                                SQL = SQL + " '" + dt2.Rows[0]["PTMIIDNO"].ToString().Trim() + "','" + dt2.Rows[0]["PTMINAME"].ToString().Trim() + "', ";
                                SQL = SQL + " '" + dt2.Rows[0]["PTMISEXX"].ToString().Trim() + "','" + dt2.Rows[0]["DEPTCODE"].ToString().Trim() + "', ";
                                SQL = SQL + " '" + dt2.Rows[0]["WARDCODE"].ToString().Trim() + "'," + dt2.Rows[0]["ROOMCODE"].ToString().Trim() + ",";
                                SQL = SQL + " '" + strDRCODE + "', ";
                                SQL = SQL + "  TO_DATE('" + strEDateT + "','YYYY-MM-DD HH24:MI'),";
                                SQL = SQL + "  TO_DATE('" + dt3.Rows[0]["ENTDATE"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }


                            }

                            dt3.Dispose();
                            dt3 = null;         
                        }


                        if (DATE_TIME(strSDateT, strEDateT) <= 1440 && DATE_TIME(strSDateT, strEDateT) >= 0)
                        {
                            nCnt7 += 1;

                            SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                            SQL = SQL + ArgYYMM + "','" + strCODE + "',7,";
                            SQL = SQL + " '" + dt2.Rows[0]["PTMIIDNO"].ToString().Trim() + "','" + dt2.Rows[0]["PTMINAME"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt2.Rows[0]["PTMISEXX"].ToString().Trim() + "','" + dt2.Rows[0]["DEPTCODE"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt2.Rows[0]["WARDCODE"].ToString().Trim() + "','" + dt2.Rows[0]["ROOMCODE"].ToString().Trim() + "',";
                            SQL = SQL + " '" + strDRCODE + "', ";
                            SQL = SQL + "  TO_DATE('" + strSDateT + "','YYYY-MM-DD HH24:MI'),";
                            SQL = SQL + "  TO_DATE('" + strEDateT + "','YYYY-MM-DD HH24:MI')  ) ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }


                            //'내원 1시간 이내 최종 치료 방침 결정 비율
                            if (DATE_TIME(strEDateT, strFDateT) <= 60 && strFDateT.Length > 0 && DATE_TIME(strEDateT, strFDateT) >= 0)
                            {
                                nCnt8 += 1;

                                SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                                SQL = SQL + ArgYYMM + "','" + strCODE + "',8,";
                                SQL = SQL + " '" + dt2.Rows[0]["PTMIIDNO"].ToString().Trim() + "','" + dt2.Rows[0]["PTMINAME"].ToString().Trim() + "', ";
                                SQL = SQL + " '" + dt2.Rows[0]["PTMISEXX"].ToString().Trim() + "','" + dt2.Rows[0]["DEPTCODE"].ToString().Trim() + "', ";
                                SQL = SQL + " '" + dt2.Rows[0]["WARDCODE"].ToString().Trim() + "'," + dt2.Rows[0]["ROOMCODE"].ToString().Trim() + ",";
                                SQL = SQL + " '" + strDRCODE + "', ";
                                SQL = SQL + "  TO_DATE('" + strEDateT + "','YYYY-MM-DD HH24:MI'),";
                                SQL = SQL + "  TO_DATE('" + strFDateT + "','YYYY-MM-DD HH24:MI')  ) ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }
                            }


                            //'24시간이내 발생 내원자 중 응급진료결과가 전원이나 입원인 경우
                            if (VB.Left(dt2.Rows[0]["PTMIEMRT"].ToString().Trim(), 1) == "2" || VB.Left(dt2.Rows[0]["PTMIEMRT"].ToString().Trim(), 1) == "3") 
                            {
                                nCnt11 += 1;

                                SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                                SQL = SQL + ArgYYMM + "','" + strCODE + "',11,";
                                SQL = SQL + " '" + dt2.Rows[0]["PTMIIDNO"].ToString().Trim() + "','" + dt2.Rows[0]["PTMINAME"].ToString().Trim() + "', ";
                                SQL = SQL + " '" + dt2.Rows[0]["PTMISEXX"].ToString().Trim() + "','" + dt2.Rows[0]["DEPTCODE"].ToString().Trim() + "', ";
                                SQL = SQL + " '" + dt2.Rows[0]["WARDCODE"].ToString().Trim() + "'," + dt2.Rows[0]["ROOMCODE"].ToString().Trim() + ",";
                                SQL = SQL + " '" + strDRCODE + "', ";
                                SQL = SQL + "  TO_DATE('" + strEDateT + "','YYYY-MM-DD HH24:MI'),";
                                SQL = SQL + "  TO_DATE('" + strFDateT + "','YYYY-MM-DD HH24:MI')  ) ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }
                            }
                        }


                        SQL = " SELECT A.PANO, TO_CHAR(A.OPDATE, 'YYYY-MM-DD HH24:MI') OPDATE, A.OPSTIME, A.DEPTCODE, B.JEPCODE, B.SUCODE, C.INDATE, C.WARDINDATE ";
                        SQL = SQL + ComNum.VBLF + " FROM ORAN_MASTER A, ORAN_SLIP B, IPD_NEW_MASTER C                                   ";
                        SQL = SQL + ComNum.VBLF + " WHERE A.WRTNO = B.WRTNO                                                             ";
                        SQL = SQL + ComNum.VBLF + "   AND A.PANO = '" + strPano + "'                                                    ";
                        SQL = SQL + ComNum.VBLF + "   AND A.PANO = C.PANO                                                               ";
                        SQL = SQL + ComNum.VBLF + "   AND TRUNC(C.INDATE) = TO_DATE('" + Convert.ToDateTime(strEDateT).ToShortDateString() + "','YYYY-MM-DD')                  ";
                        SQL = SQL + ComNum.VBLF + "   AND B.SUCODE IN ('S4641','S4642', 'M1661', 'M6631', 'M1662')                               ";
                        SQL = SQL + ComNum.VBLF + "   AND C.AMSET7 IN ('3','4','5')                                                     ";

                        SqlErr = clsDB.GetDataTable(ref dt3, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        if (dt3.Rows.Count > 0)
                        {
                            for(int k = 0; k < dt3.Rows.Count; k++)
                            {
                                strTempDate1 = dt3.Rows[k]["INDATE"].ToString().Trim();
                                if(strTempDate1 != strTempDate2)
                                {
                                    nCnt9 += 1;

                                    SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                                    SQL = SQL + ArgYYMM + "','" + strCODE + "',9,";
                                    SQL = SQL + " '" + dt2.Rows[0]["PTMIIDNO"].ToString().Trim() + "','" + dt2.Rows[0]["PTMINAME"].ToString().Trim() + "', ";
                                    SQL = SQL + " '" + (dt2.Rows[0]["PTMISEXX"].ToString().Trim().Equals("0") ? "F" : "M") + "','" + dt2.Rows[0]["DEPTCODE"].ToString().Trim() + "', ";
                                    SQL = SQL + " '" + dt2.Rows[0]["WARDCODE"].ToString().Trim() + "','" + dt2.Rows[0]["ROOMCODE"].ToString().Trim() + "',";
                                    SQL = SQL + " '" + strDRCODE + "', ";
                                    SQL = SQL + "  TO_DATE('" + strEDateT + "','YYYY-MM-DD HH24:MI'),";
                                    SQL = SQL + "  TO_DATE('" + dt3.Rows[0]["OPDATE"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        ComFunc.MsgBox(SqlErr);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        Cursor.Current = Cursors.Default;
                                        return rtnVal;
                                    }

                                    strTempDate2 = dt3.Rows[0]["INDATE"].ToString().Trim();
                                }
                            }
                        }

                        dt3.Dispose();
                        dt3 = null;


                        if (DateTime.Compare(Convert.ToDateTime(strEDateT), Convert.ToDateTime("2009-08-04")) > 0)
                        {
                            SQL = " SELECT PTNO, TO_CHAR(BDATE, 'YYYY-MM-DD HH24:MI') BDATE, SUM(QTY+NAL)  FROM ADMIN.OCS_IORDER";
                        }
                        else
                        {
                            SQL = " SELECT PTNO, TO_CHAR(BDATE, 'YYYY-MM-DD HH24:MI') BDATE, FROM ADMIN.OCS_EORDER";
                        }

                        SQL = SQL + ComNum.VBLF + "   WHERE SUCODE IN ('UROK50', 'UROK10', 'UROK2', 'TPA2', 'TPA5', 'METAL4')             ";
                        SQL = SQL + ComNum.VBLF + "     AND BDATE = TO_DATE('"  + Convert.ToDateTime(strEDateT).ToShortDateString() + "','YYYY-MM-DD')       ";
                        SQL = SQL + ComNum.VBLF + "     AND BDATE <= TO_DATE('" + Convert.ToDateTime(strEDateT).AddDays(2).ToShortDateString() + "','YYYY-MM-DD')";
                        if (DateTime.Compare(Convert.ToDateTime(strEDateT), Convert.ToDateTime("2009-08-04")) > 0)
                        {
                            SQL = SQL + ComNum.VBLF + " AND GBIOE IN ('E','EI') ";
                        }

                        SQL = SQL + ComNum.VBLF + "     AND PTNO = '" + strPano + "'                                                      " ;
                        SQL = SQL + ComNum.VBLF + "   GROUP BY   PTNO, BDATE                                                              ";
                        SQL = SQL + ComNum.VBLF + "   HAVING SUM(QTY+NAL) > 0 ";

                        SqlErr = clsDB.GetDataTable(ref dt3, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        if (dt3.Rows.Count > 0)
                        {
                            nCnt10 += 1;

                            SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                            SQL = SQL + ArgYYMM + "','" + strCODE + "',10,";
                            SQL = SQL + " '" + dt2.Rows[0]["PTMIIDNO"].ToString().Trim() + "','" + dt2.Rows[0]["PTMINAME"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt2.Rows[0]["PTMISEXX"].ToString().Trim() + "','" + dt2.Rows[0]["DEPTCODE"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt2.Rows[0]["WARDCODE"].ToString().Trim() + "','" + dt2.Rows[0]["ROOMCODE"].ToString().Trim() + "',";
                            SQL = SQL + " '" + strDRCODE + "', ";
                            SQL = SQL + "  TO_DATE('" + strEDateT + "','YYYY-MM-DD HH24:MI'),";
                            SQL = SQL + "  TO_DATE('" + dt3.Rows[0]["BDATE"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }

                        dt3.Dispose();
                        dt3 = null;
                    }

                    dt2.Dispose();
                    dt2 = null;
                }

                dt.Dispose();
                dt = null;

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',3," + nCnt3 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',4," + nCnt4 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',5," + nCnt5 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',6," + nCnt6 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',7," + nCnt7 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',8," + nCnt8 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',9," + nCnt9 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',10," + nCnt10 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',11," + nCnt11 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 중증외상질환
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns> 
        bool Build_93012_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;
            DataTable dt2 = null;
            DataTable dt3 = null;

            long nCnt1 = 0;
            long nCnt2 = 0;
            long nCnt3 = 0;
            long nCnt4 = 0;
            long nCnt5 = 0;
            long nCnt6 = 0;
            long nCnt7 = 0;

            string strCODE = "93012";

            string strIllCode = "'S020','S0200','S0201','S04','S049','S06','S061','S0610','S0611','S063','S0630','S0631'," +
                "'S065','S0650','S0651','S066','S067','S0670','S0671','S068','S0680','S0681','S12'," +
                "'S120','S121','S1210','S1211','S127','S1270','S1271','S129','S1290','S1291','S150','S157'," +
                "'S158','S179','S25','S250','S251','S252','S255','S257','S258','S259','S260','S2600','S2601'," +
                "'S268','S2680','S2681','S269','S2690','S2691','S27','S270','S2700','S2701','S271','S2710'," +
                "'S2711','S272','S2720','S2721','S273','S2730','S2731','S277','S2770','S2771','S278'," +
                "'S2780','S2781','S279','S2790','S2791','S280','S328','S3280','S3281','S345','S348','S35'," +
                "'S350','S351','S351','S352','S353','S354','S355','S357','S358','S359','S36','S360'," +
                "'S3600','S3601','S361','S3610','S3611','S362','S3620','S3621','S363','S3630','S3631'," +
                "'S364','S3640','S3641','S365','S3650','S3651','S367','S3670','S3671','S368','S3680'," +
                "'S3681','S369','S3690','S3691','S37','S377','S3770','S3771','S378','S3780','S3781'," +
                "'S38','S38','S396','S48','S75','S750','S751','S799','S842','S842','S85','S88','S880'," +
                "'S889','T016','T021','T025','T049','T136','T144','T148','T17','T190','T206','T213'," +
                "'T221','T247','T257','T27','T287','T290','T293','T314','T541','T542','T600','T602'," +
                "'T603','T608','T609','T68','T71','T751','T790','T791','T794','T900','T909','T934'";

            string strSPTMIINDT = ArgYYMM + "01";
            string strEPTMIINDT = clsVbfunc.LastDay(Convert.ToInt32(VB.Left(strSPTMIINDT, 4)), Convert.ToInt32(VB.Mid(strSPTMIINDT, 5, 2)));
            strEPTMIINDT = ArgYYMM + VB.Right(strEPTMIINDT, 2);

            string strTemp = "";

            string strPano      = "";
            string strInDate    = "";

            string strSDate = "";
            string strEDate = "";

            string strSDateT = "";
            string strEDateT = "";
            string strTDateT = "";
            string strFDateT = "";

            string strDRCODE = "";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


               SQL = " SELECT A.DGOTIDNO IDNO, A.DGOTINDT INDT, A.DGOTINTM INTM                              ";
               SQL += ComNum.VBLF + "   FROM NUR_ER_EMIHDGOT A, (SELECT Max(SEQNO) SEQNO, PTMIIDNO, PTMIINDT, PTMIINTM, PTMISTAT ";
               SQL += ComNum.VBLF + "      From NUR_ER_EMIHPTMI                                                       ";
               SQL += ComNum.VBLF + "      WHERE PTMIINDT >= '" + strSPTMIINDT + "'                                   ";
               SQL += ComNum.VBLF + "        AND PTMIINDT <= '" + strEPTMIINDT + "'                                   ";
               SQL += ComNum.VBLF + "        AND PTMIDGKD = '2'                                                       ";
               SQL += ComNum.VBLF + "      GROUP BY PTMIIDNO , PTMIINDT, PTMIINTM, PTMISTAT                           ";
               SQL += ComNum.VBLF + "      HAVING PTMISTAT <> 'D') B                                                  ";
               SQL += ComNum.VBLF + "      WHERE A.DGOTINDT >= '" + strSPTMIINDT + "'                                 ";
               SQL += ComNum.VBLF + "        AND A.DGOTINDT <= '" + strEPTMIINDT + "'                                 ";
               SQL += ComNum.VBLF + "    AND A.DGOTINDT = B.PTMIINDT                                                  ";
               SQL += ComNum.VBLF + "    AND A.DGOTINTM = B.PTMIINTM                                                  ";
               SQL += ComNum.VBLF + "    AND A.DGOTIDNO = B.PTMIIDNO                                                  ";
               SQL += ComNum.VBLF + "    AND A.DGOTDIAG IN  (" + strIllCode + ")                                      ";
               SQL += ComNum.VBLF + "   GROUP BY A.DGOTIDNO, A.DGOTINDT, A.DGOTINTM                                   ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    strPano = dt.Rows[i]["IDNO"].ToString().Trim();
                    if(strPano == "07605280")
                    {
                        strPano = strPano;
                    }

                    strInDate = dt.Rows[i]["INDT"].ToString().Trim();

                    strSDate = VB.Left(ArgYYMM, 4) + "-" + VB.Right(ArgYYMM, 2) + "-01";
                    strEDate = clsVbfunc.LastDay(Convert.ToInt32(VB.Left(ArgYYMM, 4)), Convert.ToInt32(VB.Right(ArgYYMM, 2)));

                    SQL = " SELECT A.PTMIIDNO, A.PTMINAME, A.PTMIBRTD, A.PTMISEXX, 'ER' DEPTCODE, '' WARDCODE, '' ROOMCODE,         ";
                    SQL += ComNum.VBLF + "  A.PTMIAKDT, A.PTMIAKTM, A.PTMIDCDT, A.PTMIDCTM, A.PTMIOTDT, A.PTMIOTTM, A.PTMIINDT, A.PTMIINTM,  ";
                    SQL += ComNum.VBLF + "  A.PTMIDRLC , A.PTMIEMRT, A.PTMISTAT                                                              ";
                    SQL += ComNum.VBLF + "       FROM NUR_ER_EMIHPTMI A, (                                                                   ";
                    SQL += ComNum.VBLF + "  SELECT MAX(SEQNO) SEQNO, PTMIIDNO, PTMIINDT, PTMIINTM                                            ";
                    SQL += ComNum.VBLF + "  From NUR_ER_EMIHPTMI                                                                             ";
                    SQL += ComNum.VBLF + "  WHERE PTMIIDNO = '" + dt.Rows[i]["IDNO"].ToString().Trim() + "'                                  ";
                    SQL += ComNum.VBLF + "    AND PTMIINDT = '" + dt.Rows[i]["INDT"].ToString().Trim() + "'                                  ";
                    SQL += ComNum.VBLF + "    AND PTMIINTM = '" + dt.Rows[i]["INTM"].ToString().Trim() + "'                                  ";
                    SQL += ComNum.VBLF + "  GROUP BY PTMIIDNO, PTMIINDT, PTMIINTM) B, NUR_ER_PATIENT C                                       ";
                    SQL += ComNum.VBLF + "       WHERE A.SEQNO = B.SEQNO                                                                     ";
                    SQL += ComNum.VBLF + "         AND A.PTMIIDNO = C.PANO                                                                   ";
                    SQL += ComNum.VBLF + "         AND A.PTMIINDT = TO_CHAR(C.INTIME, 'YYYYMMDD')                                            ";
                    SQL += ComNum.VBLF + "         AND A.PTMIINTM = TO_CHAR(C.INTIME, 'HH24MI')";
                    SQL += ComNum.VBLF + "         AND A.PTMIIDNO = B.PTMIIDNO                                                               ";//
                    SQL += ComNum.VBLF + "         AND A.PTMIINDT = B.PTMIINDT                                                               ";//
                    SQL += ComNum.VBLF + "         AND A.PTMIINTM = B.PTMIINTM                                                               ";//
                    SQL += ComNum.VBLF + "         AND A.PTMIDGKD = '2'                                                                      ";//  '내원사유가 질병외인 경우
                    SQL += ComNum.VBLF + "       ORDER BY A.PTMISTAT, A.PTMIINDT, A.PTMIINTM                                                 ";//

                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if(dt2.Rows.Count > 0)
                    {
                        nCnt1 += 1; //전체인원

                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',1,";
                        SQL = SQL + " '" + dt.Rows[i]["IDNO"].ToString().Trim() + "','', ";
                        SQL = SQL + " '','ER', ";
                        SQL = SQL + " '',000,";
                        SQL = SQL + " '', ";
                        SQL = SQL + "  TO_DATE('" + strSPTMIINDT + "','YYYY-MM-DD HH24:MI'),";
                        SQL = SQL + "  TO_DATE('" + strEPTMIINDT + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }



                        strDRCODE = READ_LicToDRCode(dt2.Rows[0]["PTMIDRLC"].ToString().Trim());
                        //'발병일시
                        strTemp = dt2.Rows[0]["PTMIAKDT"].ToString().Trim();
                        strSDateT = VB.Left(strTemp, 4) + "-" + VB.Mid(strTemp, 5, 2) + "-" + VB.Right(strTemp, 2);
                        strTemp = dt2.Rows[0]["PTMIAKTM"].ToString().Trim();
                        strSDateT = strSDateT + " " + VB.Left(strTemp, 2) + ":" + VB.Right(strTemp, 2);

                        //'내원일시
                        strTemp = dt2.Rows[0]["PTMIINDT"].ToString().Trim();
                        strEDateT = VB.Left(strTemp, 4) + "-" + VB.Mid(strTemp, 5, 2) + "-" + VB.Right(strTemp, 2);
                        strTemp = dt2.Rows[0]["PTMIINTM"].ToString().Trim();
                        strEDateT = strEDateT + " " + VB.Left(strTemp, 2) + ":" + VB.Right(strTemp, 2);

                        //'퇴실일시
                        strTemp = dt2.Rows[0]["PTMIOTDT"].ToString().Trim();
                        strTDateT = VB.Left(strTemp, 4) + "-" + VB.Mid(strTemp, 5, 2) + "-" + VB.Right(strTemp, 2);
                        strTemp = dt2.Rows[0]["PTMIOTTM"].ToString().Trim();
                        strTDateT = strEDateT + " " + VB.Left(strTemp, 2) + ":" + VB.Right(strTemp, 2);


                        if(DATE_TIME(strSDateT, strEDateT) <= 1440)
                        {
                            nCnt2 += 1;


                            SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                            SQL = SQL + ArgYYMM + "','" + strCODE + "',2,";
                            SQL = SQL + " '" + dt2.Rows[0]["PTMIIDNO"].ToString().Trim() + "','" + dt2.Rows[0]["PTMINAME"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt2.Rows[0]["PTMISEXX"].ToString().Trim() + "','" + dt2.Rows[0]["DEPTCODE"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt2.Rows[0]["WARDCODE"].ToString().Trim() + "','" + dt2.Rows[0]["ROOMCODE"].ToString().Trim() + "',";
                            SQL = SQL + " '" + strDRCODE + "', ";
                            SQL = SQL + "  TO_DATE('" + strEDateT + "','YYYY-MM-DD HH24:MI'),";
                            SQL = SQL + "  TO_DATE('" + strInDate + "','YYYY-MM-DD HH24:MI')  ) ";//  '응급환자 모니터링 P/G에서 조인용으로 사용함.(내원일시)

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }

                            strFDateT = "";


                            SQL = " SELECT HOJINDATE1 FROM NUR_ER_PATIENT ";
                            SQL += ComNum.VBLF + " WHERE PANO = '" + strPano + "' ";
                            SQL += ComNum.VBLF + "   AND INTIME = TO_DATE('" + strEDateT + "','YYYY-MM-DD HH24:MI') ";

                            SqlErr = clsDB.GetDataTable(ref dt3, SQL, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }

                            if(dt3.Rows.Count > 0)
                            {
                                strFDateT = dt3.Rows[0]["HOJINDATE1"].ToString();
                            }

                            dt3.Dispose();
                            dt3 = null;


                            if(DATE_TIME(strSDateT, strEDateT) <= 1440)
                            {
                                switch(dt2.Rows[0]["PTMIEMRT"].ToString().Trim())
                                {
                                    case "31":
                                    case "32":
                                    case "33":
                                    case "34":
                                    case "38":
                                        nCnt3 += 1;

                                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                                        SQL = SQL + ArgYYMM + "','" + strCODE + "',3,";
                                        SQL = SQL + " '" + dt2.Rows[0]["PTMIIDNO"].ToString().Trim() + "','" + dt2.Rows[0]["PTMINAME"].ToString().Trim() + "', ";
                                        SQL = SQL + " '" + dt2.Rows[0]["PTMISEXX"].ToString().Trim() + "','" + dt2.Rows[0]["DEPTCODE"].ToString().Trim() + "', ";
                                        SQL = SQL + " '" + dt2.Rows[0]["WARDCODE"].ToString().Trim() + "','" + dt2.Rows[0]["ROOMCODE"].ToString().Trim() + "',";
                                        SQL = SQL + " '" + strDRCODE + "', ";
                                        SQL = SQL + "  TO_DATE('" + strSDateT + "','YYYY-MM-DD HH24:MI'),";
                                        SQL = SQL + "  TO_DATE('" + strEDateT + "','YYYY-MM-DD HH24:MI')  ) ";//  '응급환자 모니터링 P/G에서 조인용으로 사용함.(내원일시)

                                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                        if (SqlErr != "")
                                        {
                                            clsDB.setRollbackTran(clsDB.DbCon);
                                            ComFunc.MsgBox(SqlErr);
                                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                            Cursor.Current = Cursors.Default;
                                            return rtnVal;
                                        }
                                        break;
                                }
                            }

                            if (DATE_TIME(strSDateT, strEDateT) <= 180)
                            {
                                nCnt4 += 1;

                                SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                                SQL = SQL + ArgYYMM + "','" + strCODE + "',4,";
                                SQL = SQL + " '" + dt2.Rows[0]["PTMIIDNO"].ToString().Trim() + "','" + dt2.Rows[0]["PTMINAME"].ToString().Trim() + "', ";
                                SQL = SQL + " '" + dt2.Rows[0]["PTMISEXX"].ToString().Trim() + "','" + dt2.Rows[0]["DEPTCODE"].ToString().Trim() + "', ";
                                SQL = SQL + " '" + dt2.Rows[0]["WARDCODE"].ToString().Trim() + "','" + dt2.Rows[0]["ROOMCODE"].ToString().Trim() + "',";
                                SQL = SQL + " '" + strDRCODE + "', ";
                                SQL = SQL + "  TO_DATE('" + strSDateT + "','YYYY-MM-DD HH24:MI'),";
                                SQL = SQL + "  TO_DATE('" + strEDateT + "','YYYY-MM-DD HH24:MI')  ) ";//  '응급환자 모니터링 P/G에서 조인용으로 사용함.(내원일시)

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }

                                if (DATE_TIME(strSDateT, strEDateT) >= 30 && strFDateT.Length > 0)
                                {
                                    nCnt5 += 1;

                                    SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                                    SQL = SQL + ArgYYMM + "','" + strCODE + "',5,";
                                    SQL = SQL + " '" + dt2.Rows[0]["PTMIIDNO"].ToString().Trim() + "','" + dt2.Rows[0]["PTMINAME"].ToString().Trim() + "', ";
                                    SQL = SQL + " '" + dt2.Rows[0]["PTMISEXX"].ToString().Trim() + "','" + dt2.Rows[0]["DEPTCODE"].ToString().Trim() + "', ";
                                    SQL = SQL + " '" + dt2.Rows[0]["WARDCODE"].ToString().Trim() + "','" + dt2.Rows[0]["ROOMCODE"].ToString().Trim() + "',";
                                    SQL = SQL + " '" + strDRCODE + "', ";
                                    SQL = SQL + "  TO_DATE('" + strSDateT + "','YYYY-MM-DD HH24:MI'),";
                                    SQL = SQL + "  TO_DATE('" + strEDateT + "','YYYY-MM-DD HH24:MI')  ) ";//  '응급환자 모니터링 P/G에서 조인용으로 사용함.(내원일시)

                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        ComFunc.MsgBox(SqlErr);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        Cursor.Current = Cursors.Default;
                                        return rtnVal;
                                    }
                                }
                            }


                            if (DATE_TIME(strSDateT, strEDateT) >= 180 && DATE_TIME(strSDateT, strEDateT) <= 1440)
                            {
                                nCnt6 += 1;

                                SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                                SQL = SQL + ArgYYMM + "','" + strCODE + "',6,";
                                SQL = SQL + " '" + dt2.Rows[0]["PTMIIDNO"].ToString().Trim() + "','" + dt2.Rows[0]["PTMINAME"].ToString().Trim() + "', ";
                                SQL = SQL + " '" + dt2.Rows[0]["PTMISEXX"].ToString().Trim() + "','" + dt2.Rows[0]["DEPTCODE"].ToString().Trim() + "', ";
                                SQL = SQL + " '" + dt2.Rows[0]["WARDCODE"].ToString().Trim() + "'," + dt2.Rows[0]["ROOMCODE"].ToString().Trim() + ",";
                                SQL = SQL + " '" + strDRCODE + "', ";
                                SQL = SQL + "  TO_DATE('" + strSDateT + "','YYYY-MM-DD HH24:MI'),";
                                SQL = SQL + "  TO_DATE('" + strEDateT + "','YYYY-MM-DD HH24:MI')  ) ";//  '응급환자 모니터링 P/G에서 조인용으로 사용함.(내원일시)

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }


                                if(DATE_TIME(strEDateT, strFDateT) >= 60 && strFDateT.Length > 0)
                                {
                                    nCnt7 += 1;

                                    SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                                    SQL = SQL + ArgYYMM + "','" + strCODE + "',7,";
                                    SQL = SQL + " '" + dt2.Rows[0]["PTMIIDNO"].ToString().Trim() + "','" + dt2.Rows[0]["PTMINAME"].ToString().Trim() + "', ";
                                    SQL = SQL + " '" + dt2.Rows[0]["PTMISEXX"].ToString().Trim() + "','" + dt2.Rows[0]["DEPTCODE"].ToString().Trim() + "', ";
                                    SQL = SQL + " '" + dt2.Rows[0]["WARDCODE"].ToString().Trim() + "'," + dt2.Rows[0]["ROOMCODE"].ToString().Trim() + ",";
                                    SQL = SQL + " '" + strDRCODE + "', ";
                                    SQL = SQL + "  TO_DATE('" + strSDateT + "','YYYY-MM-DD HH24:MI'),";
                                    SQL = SQL + "  TO_DATE('" + strEDateT + "','YYYY-MM-DD HH24:MI')  ) ";//  '응급환자 모니터링 P/G에서 조인용으로 사용함.(내원일시)

                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        ComFunc.MsgBox(SqlErr);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        Cursor.Current = Cursors.Default;
                                        return rtnVal;
                                    }
                                }
                            }
                        }

                    }

                    dt2.Dispose();
                    dt2 = null;
                }

                dt.Dispose();
                dt = null;

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',3," + nCnt3 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',4," + nCnt4 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',5," + nCnt5 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',6," + nCnt6 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',7," + nCnt7 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 응급 중증 뇌질환 - 마스터 기준
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns> 
        bool Build_93013_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;
            DataTable dt2 = null;

            long nCnt1 = 0;
            long nCnt2 = 0;
            long nCnt3 = 0;
            long nCnt4 = 0;
            long nCnt5 = 0;
            long nCnt6 = 0;
            long nCnt7 = 0;
            long nCnt8 = 0;

            string strCODE = "93013";
        
            string strSPTMIINDT = ArgYYMM + "01";
            string strEPTMIINDT = clsVbfunc.LastDay(Convert.ToInt32(VB.Left(strSPTMIINDT, 4)), Convert.ToInt32(VB.Mid(strSPTMIINDT, 5, 2)));
            strEPTMIINDT = ArgYYMM + VB.Right(strEPTMIINDT, 2);

            string strTemp = "";

            string strPano = "";
            string strInDate = "";

            string strSDateT = "";
            string strEDateT = "";
            string strFDateT = "";

            string strDRCODE = "";

            string strTempDate1 = "";
            string strTempDate2 = "";


            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                SQL = " SELECT A.PTMIIDNO, A.PTMINAME, A.PTMIBRTD, A.PTMISEXX, 'ER' DEPTCODE, '' WARDCODE, '' ROOMCODE,         ";
                SQL = SQL + ComNum.VBLF + "  A.PTMIAKDT, A.PTMIAKTM, A.PTMIDCDT, A.PTMIDCTM, A.PTMIOTDT, A.PTMIOTTM, A.PTMIINDT, A.PTMIINTM,  ";
                SQL = SQL + ComNum.VBLF + "  A.PTMIDRLC , A.PTMIEMRT, A.PTMISTAT                                                              ";
                SQL = SQL + ComNum.VBLF + "       FROM NUR_ER_EMIHPTMI A, (                                                                   ";
                SQL = SQL + ComNum.VBLF + "  SELECT MAX(SEQNO) SEQNO, PTMIIDNO, PTMIINDT, PTMIINTM                                            ";
                SQL = SQL + ComNum.VBLF + "  From NUR_ER_EMIHPTMI                                                                             ";
                SQL = SQL + ComNum.VBLF + "  WHERE PTMIINDT >= '" + strSPTMIINDT + "'                                                         ";
                SQL = SQL + ComNum.VBLF + "    AND PTMIINDT <= '" + strEPTMIINDT + "'                                                         ";
                SQL = SQL + ComNum.VBLF + "  GROUP BY PTMIIDNO, PTMIINDT, PTMIINTM) B                                                         ";
                SQL = SQL + ComNum.VBLF + "  WHERE A.ILLCODE >= 'I60'                                                                         ";
                SQL = SQL + ComNum.VBLF + "         AND A.ILLCODE <= 'I639'                                                                   ";
                SQL = SQL + ComNum.VBLF + "         AND A.PTMIDGKD = '1'                                                                      ";
                SQL = SQL + ComNum.VBLF + "         AND A.SEQNO = B.SEQNO                                                                     ";
                SQL = SQL + ComNum.VBLF + "         AND A.PTMIIDNO = B.PTMIIDNO                                                               ";
                SQL = SQL + ComNum.VBLF + "         AND A.PTMIINDT = B.PTMIINDT                                                               ";
                SQL = SQL + ComNum.VBLF + "         AND A.PTMIINTM = B.PTMIINTM                                                               ";
                SQL = SQL + ComNum.VBLF + "       ORDER BY A.PTMISTAT, A.PTMIINDT, A.PTMIINTM                                                 ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    strPano = dt.Rows[i]["PTMIIDNO"].ToString().Trim();
                    strDRCODE = READ_LicToDRCode(dt.Rows[i]["PTMIDRLC"].ToString().Trim());

                    nCnt1 += 1; //전체인원
                    SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                    SQL = SQL + ArgYYMM + "','" + strCODE + "',1,";
                    SQL = SQL + " '" + dt.Rows[i]["PTMIIDNO"].ToString().Trim() + "','" + dt.Rows[i]["PTMINAME"].ToString().Trim() + "', ";
                    SQL = SQL + " '" + dt.Rows[i]["PTMISEXX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                    SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                    SQL = SQL + " '" + strDRCODE + "', ";
                    SQL = SQL + "  TO_DATE('" + strSPTMIINDT + "','YYYY-MM-DD HH24:MI'),";
                    SQL = SQL + "  TO_DATE('" + strEPTMIINDT + "','YYYY-MM-DD HH24:MI')  ) ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    //'발병일시
                    strTemp = dt.Rows[i]["PTMIAKDT"].ToString().Trim();
                    strSDateT = VB.Left(strTemp, 4) + "-" + VB.Mid(strTemp, 5, 2) + "-" + VB.Right(strTemp, 2);
                    strTemp = dt.Rows[i]["PTMIAKTM"].ToString().Trim();
                    strSDateT = strSDateT + " " + VB.Left(strTemp, 2) + ":" + VB.Right(strTemp, 2);

                    //'내원일시
                    strTemp = dt.Rows[i]["PTMIINDT"].ToString().Trim();
                    strEDateT = VB.Left(strTemp, 4) + "-" + VB.Mid(strTemp, 5, 2) + "-" + VB.Right(strTemp, 2);
                    strTemp = dt.Rows[i]["PTMIINTM"].ToString().Trim();
                    strEDateT = strEDateT + " " + VB.Left(strTemp, 2) + ":" + VB.Right(strTemp, 2);

                    //주 증상 최종진단 시간
                    SQL = " SELECT HOJINDATE1 FROM NUR_ER_PATIENT ";
                    SQL = SQL + " WHERE PANO = '" + strPano + "' ";
                    SQL = SQL + "   AND INTIME = TO_DATE('" + strEDateT + "','YYYY-MM-DD HH24:MI') ";

                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if(dt2.Rows.Count > 0)
                    {
                        strFDateT = Convert.ToDateTime(dt2.Rows[0]["HOJINDATE1"].ToString()).ToString("yyyy-MM-dd HH:mm");
                    }

                    if (DATE_TIME(strSDateT, strEDateT) <= 10080)
                    {
                        switch (dt.Rows[0]["PTMIEMRT"].ToString().Trim())
                        {
                            case "31":
                            case "32":
                            case "33":
                            case "34":
                            case "38":
                                nCnt2 += 1;

                                SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                                SQL = SQL + ArgYYMM + "','" + strCODE + "',2,";
                                SQL = SQL + " '" + dt.Rows[i]["PTMIIDNO"].ToString().Trim() + "','" + dt.Rows[i]["PTMINAME"].ToString().Trim() + "', ";
                                SQL = SQL + " '" + dt.Rows[i]["PTMISEXX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                                SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                                SQL = SQL + " '" + strDRCODE + "', ";
                                SQL = SQL + "  TO_DATE('" + strSDateT + "','YYYY-MM-DD HH24:MI'),";
                                SQL = SQL + "  TO_DATE('" + dt.Rows[i]["PTMIDCDT"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }
                                break;
                        }
                    }

                    dt2.Dispose();
                    dt2 = null;

                    if (DATE_TIME(strSDateT, strEDateT) <= 180)
                    {
                        nCnt3 += 1;

                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',3,";
                        SQL = SQL + " '" + dt.Rows[i]["PTMIIDNO"].ToString().Trim() + "','" + dt.Rows[i]["PTMINAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["PTMISEXX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                        SQL = SQL + " '" + strDRCODE + "', ";
                        SQL = SQL + "  TO_DATE('" + strSDateT + "','YYYY-MM-DD HH24:MI'),";
                        SQL = SQL + "  TO_DATE('" + strEDateT + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }


                        SQL = " SELECT PANO, JDATE, REMARK2 ";
                        SQL = SQL + " FROM NUR_ER_JUNGDO ";
                        SQL = SQL + " WHERE PANO = '" + strPano + "' ";
                        SQL = SQL + "   AND JDATE = TO_DATE('" + strInDate + "', 'YYYY-MM-DD') ";
                        SQL = SQL + "   AND GUBUN = '3' ";
                        SQL = SQL + "   AND REMARK1 IS NOT NULL ";
                        SQL = SQL + " GROUP BY PANO, JDATE, REMARK2 ";

                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        if(dt2.Rows.Count > 0)
                        {
                            if (DATE_TIME(strEDateT, dt2.Rows[0]["REMARK2"].ToString().Trim()) <= 30)
                            {
                                nCnt4 += 1;

                                SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                                SQL = SQL + ArgYYMM + "','" + strCODE + "',4,";
                                SQL = SQL + " '" + dt.Rows[i]["PTMIIDNO"].ToString().Trim() + "','" + dt.Rows[i]["PTMINAME"].ToString().Trim() + "', ";
                                SQL = SQL + " '" + dt.Rows[i]["PTMISEXX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                                SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                                SQL = SQL + " '" + strDRCODE + "', ";
                                SQL = SQL + "  TO_DATE('" + strSDateT + "','YYYY-MM-DD HH24:MI'),";
                                SQL = SQL + "  TO_DATE('" + strEDateT + "','YYYY-MM-DD HH24:MI')  ) ";//  '응급환자 모니터링 P/G에서 조인용으로 사용함.(내원일시)

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }
                            }
                        }

                        dt2.Dispose();
                        dt2 = null;

                    }


                    if (DATE_TIME(strSDateT, strEDateT) <= 1440)
                    {
                        nCnt5 += 1;

                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',5,";
                        SQL = SQL + " '" + dt.Rows[i]["PTMIIDNO"].ToString().Trim() + "','" + dt.Rows[i]["PTMINAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["PTMISEXX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                        SQL = SQL + " '" + strDRCODE + "', ";
                        SQL = SQL + "  TO_DATE('" + strSDateT + "','YYYY-MM-DD HH24:MI'),";
                        SQL = SQL + "  TO_DATE('" + strEDateT + "','YYYY-MM-DD HH24:MI')  ) ";//  '응급환자 모니터링 P/G에서 조인용으로 사용함.(내원일시)

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        //'퇴실일시
                        strTemp = dt.Rows[i]["PTMIOTDT"].ToString().Trim();
                        strEDateT = VB.Left(strTemp, 4) + "-" + VB.Mid(strTemp, 5, 2) + "-" + VB.Right(strTemp, 2);
                        strTemp = dt.Rows[i]["PTMIOTTM"].ToString().Trim();
                        strEDateT = strEDateT + " " + VB.Left(strTemp, 2) + ":" + VB.Right(strTemp, 2);


                        if (DATE_TIME(strEDateT, strFDateT) <= 60 && strTemp.Length > 10)
                        {
                            nCnt6 += 1;

                            SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                            SQL = SQL + ArgYYMM + "','" + strCODE + "',6,";
                            SQL = SQL + " '" + dt.Rows[i]["PTMIIDNO"].ToString().Trim() + "','" + dt.Rows[i]["PTMINAME"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["PTMISEXX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                            SQL = SQL + " '" + strDRCODE + "', ";
                            SQL = SQL + "  TO_DATE('" + strSDateT + "','YYYY-MM-DD HH24:MI'),";
                            SQL = SQL + "  TO_DATE('" + strEDateT + "','YYYY-MM-DD HH24:MI')  ) ";//  '응급환자 모니터링 P/G에서 조인용으로 사용함.(내원일시)

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }
                    }



                    //'내원일시
                    strTemp = dt.Rows[i]["PTMIINDT"].ToString().Trim();
                    strEDateT = VB.Left(strTemp, 4) + "-" + VB.Mid(strTemp, 5, 2) + "-" + VB.Right(strTemp, 2);
                    strTemp = dt.Rows[i]["PTMIINTM"].ToString().Trim();
                    strEDateT = strEDateT + " " + VB.Left(strTemp, 2) + ":" + VB.Right(strTemp, 2);



                    SQL = " SELECT A.PANO, TO_CHAR(A.OPDATE, 'YYYY-MM-DD HH24:MI') OPDATE, A.OPSTIME, A.DEPTCODE, B.JEPCODE, B.SUCODE, C.INDATE, C.WARDINDATE ";
                    SQL = SQL + ComNum.VBLF + " FROM ORAN_MASTER A, ORAN_SLIP B, IPD_NEW_MASTER C                                   ";
                    SQL = SQL + ComNum.VBLF + " WHERE A.WRTNO = B.WRTNO                                                             ";
                    SQL = SQL + ComNum.VBLF + "   AND A.PANO = '" + strPano + "'                                                    ";
                    SQL = SQL + ComNum.VBLF + "   AND A.PANO = C.PANO                                                               ";
                    SQL = SQL + ComNum.VBLF + "   AND C.INDATE = TO_DATE('" + strEDateT + "','YYYY-MM-DD HH24:MI')                  ";
                    SQL = SQL + ComNum.VBLF + "   AND B.SUCODE IN ('UROK50', 'UROK10', 'UROK2', 'HPRB', 'HPRA', 'S4641','S4642',    ";
                    SQL = SQL + ComNum.VBLF + " 'M1661', 'M1662', 'M6631', 'M6601')                                                          ";
                    SQL = SQL + ComNum.VBLF + "   AND C.AMSET7 IN ('3','4','5')                                                     ";
                    SQL = SQL + ComNum.VBLF + "   AND A.OPBUN =  '2'                                                                ";

                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }


                    if(dt2.Rows.Count > 0)
                    {
                        for(int k = 0; k < dt2.Rows.Count; k++)
                        {
                            strTempDate1 = dt2.Rows[k]["INDATE"].ToString().Trim();
                            if(strTempDate1 != strTempDate2)
                            {
                                switch(dt2.Rows[k]["SUCODE"].ToString().Trim())
                                {
                                    case "UROK50":
                                    case "UROK10":
                                    case "UROK2":
                                    case "M6631":
                                    case "HPRB":
                                    case "HPRA":
                                        nCnt7 += 1;

                                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                                        SQL = SQL + ArgYYMM + "','" + strCODE + "',7,";
                                        SQL = SQL + " '" + dt.Rows[i]["PTMIIDNO"].ToString().Trim() + "','" + dt.Rows[i]["PTMINAME"].ToString().Trim() + "', ";
                                        SQL = SQL + " '" + dt.Rows[i]["PTMISEXX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                                        SQL = SQL + " '" + strDRCODE + "', ";
                                        SQL = SQL + "  TO_DATE('" + strEDateT + "','YYYY-MM-DD HH24:MI'),";
                                        SQL = SQL + "  TO_DATE('" + dt2.Rows[k]["OPDATE"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";//  '응급환자 모니터링 P/G에서 조인용으로 사용함.(내원일시)

                                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                        if (SqlErr != "")
                                        {
                                            clsDB.setRollbackTran(clsDB.DbCon);
                                            ComFunc.MsgBox(SqlErr);
                                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                            Cursor.Current = Cursors.Default;
                                            return rtnVal;
                                        }
                                        break;
                                    case "S4641":
                                    case "S4642":
                                    case "M1661":
                                    case "M6601":

                                        nCnt8 += 1;

                                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                                        SQL = SQL + ArgYYMM + "','" + strCODE + "',8,";
                                        SQL = SQL + " '" + dt.Rows[i]["PTMIIDNO"].ToString().Trim() + "','" + dt.Rows[i]["PTMINAME"].ToString().Trim() + "', ";
                                        SQL = SQL + " '" + dt.Rows[i]["PTMISEXX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                                        SQL = SQL + " '" + strDRCODE + "', ";
                                        SQL = SQL + "  TO_DATE('" + strEDateT + "','YYYY-MM-DD HH24:MI'),";
                                        SQL = SQL + "  TO_DATE('" + dt2.Rows[k]["OPDATE"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";//  '응급환자 모니터링 P/G에서 조인용으로 사용함.(내원일시)

                                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                        if (SqlErr != "")
                                        {
                                            clsDB.setRollbackTran(clsDB.DbCon);
                                            ComFunc.MsgBox(SqlErr);
                                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                            Cursor.Current = Cursors.Default;
                                            return rtnVal;
                                        }
                                        break;
                                }
                            }
                            strTempDate2 = dt2.Rows[k]["INDATE"].ToString().Trim();
                        }
                    }


                    dt2.Dispose();
                    dt2 = null;

                }

                dt.Dispose();
                dt = null;

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',3," + nCnt3 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',4," + nCnt4 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',5," + nCnt5 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',6," + nCnt6 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',7," + nCnt7 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',8," + nCnt8 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 응급 중증 외상질환 - 마스터 기준
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns> 
        bool Build_93014_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;
            DataTable dt2 = null;

            long nCnt1 = 0;
            long nCnt2 = 0;
            long nCnt3 = 0;
            long nCnt4 = 0;
            long nCnt5 = 0;
            long nCnt6 = 0;
            long nCnt7 = 0;

            string strIllCode = "'S020','S04','S049','S06','S061','S063','S065','S067','S068','S12','S121','S129','S150','S157','S158','S172','S25','" +
                 "S250','S252','S257','S258','S259','S26','S260','S268','S269','S27','S270','S271','S272','S273','S277','S278','S279','" +
                 "S280','S281','S328','S35','S350','S351','S352','S353','S355','S357','S358','S359','S36','S360','S361','S362','S363','" +
                 "S364','S365','S367','S368','S369','S37','S377','S378','S38','S396','S48','S75','S750','S751','S842','S85','S88','S880','"+
                "S889','T018','T025','T136','T790','T791','T794'";

            string strCODE = "93014";

            string strSPTMIINDT = ArgYYMM + "01";
            string strEPTMIINDT = clsVbfunc.LastDay(Convert.ToInt32(VB.Left(strSPTMIINDT, 4)), Convert.ToInt32(VB.Mid(strSPTMIINDT, 5, 2)));
            strEPTMIINDT = ArgYYMM + VB.Right(strEPTMIINDT, 2);

            string strTemp = "";

            string strPano = "";
            string strInDate = "";

            string strSDateT = "";
            string strEDateT = "";

            string strSDATE = "";
            string strEDATE = "";

            string strDRCODE = "";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                SQL = "     SELECT IDNO, INDT, INTM FROM ( SELECT DGOTIDNO IDNO, DGOTINDT INDT, DGOTINTM INTM ";
                SQL += ComNum.VBLF + "      From NUR_ER_EMIHDGOT                                                       ";
                SQL += ComNum.VBLF + "      WHERE DGOTINDT >= '" + strSPTMIINDT + "'                                   ";
                SQL += ComNum.VBLF + "        AND DGOTINDT <= '" + strEPTMIINDT + "'                                   ";
                SQL += ComNum.VBLF + "       AND DGOTDIAG IN (" + strIllCode + ")                                      ";
                SQL += ComNum.VBLF + "      GROUP BY DGOTIDNO, DGOTINDT, DGOTINTM                                      ";
                SQL += ComNum.VBLF + "       Union All                                                                 ";
                SQL += ComNum.VBLF + "      SELECT DGDCIDNO, DGDCINDT, DGDCINTM                                        ";
                SQL += ComNum.VBLF + "      From NUR_ER_EMIHDGDC                                                       ";
                SQL += ComNum.VBLF + "      WHERE DGDCINDT >= '" + strSPTMIINDT + "'                                   ";
                SQL += ComNum.VBLF + "        AND DGDCINDT <= '" + strEPTMIINDT + "'                                   ";
                SQL += ComNum.VBLF + "       AND DGDCDIAG IN (" + strIllCode + ")                                      ";
                SQL += ComNum.VBLF + "      GROUP BY DGDCIDNO, DGDCINDT, DGDCINTM) X                                   ";
                SQL += ComNum.VBLF + "      GROUP BY INDT, INTM, IDNO                                                  ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    nCnt1 += 1; //전체인원
                    SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                    SQL = SQL + ArgYYMM + "','" + strCODE + "',1,";
                    SQL = SQL + " '" + dt.Rows[i]["IDNO"].ToString().Trim() + "','', ";
                    SQL = SQL + " '','ER', ";
                    SQL = SQL + " '',000,";
                    SQL = SQL + " '', ";
                    SQL = SQL + "  TO_DATE('" + strSPTMIINDT + "','YYYY-MM-DD HH24:MI'),";
                    SQL = SQL + "  TO_DATE('" + strEPTMIINDT + "','YYYY-MM-DD HH24:MI')  ) ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }


                    strPano = dt.Rows[i]["IDNO"].ToString().Trim();
                    strInDate = dt.Rows[i]["INDT"].ToString().Trim();


                    strSDATE = VB.Left(ArgYYMM, 4) + "-" + VB.Right(ArgYYMM, 2) + "-01";
                    strEDATE = clsVbfunc.LastDay(Convert.ToInt32(VB.Left(ArgYYMM, 4)), Convert.ToInt32(VB.Mid(ArgYYMM, 5, 2)));


                    SQL = " SELECT PTMIIDNO, PTMINAME, PTMIBRTD, PTMISEXX, 'ER' DEPTCODE, '' WARDCODE, '' ROOMCODE,            ";
                    SQL = SQL + ComNum.VBLF + " PTMIAKDT, PTMIAKTM, PTMIDCDT, PTMIDCTM, PTMIOTDT, PTMIOTTM, PTMIINDT, PTMIINTM, PTMIDRLC,   ";
                    SQL = SQL + ComNum.VBLF + " PTMIEMRT                                                                                    ";
                    SQL = SQL + ComNum.VBLF + " FROM NUR_ER_EMIHPTMI                                                                        ";
                    SQL = SQL + ComNum.VBLF + " WHERE PTMIIDNO = '" + dt.Rows[i]["IDNO"].ToString().Trim() + "'                                      ";
                    SQL = SQL + ComNum.VBLF + "   AND PTMIINDT = '" + dt.Rows[i]["INDT"].ToString().Trim() + "'                                      ";
                    SQL = SQL + ComNum.VBLF + "   AND ILLCODE IN (" + strIllCode + ")                                                       ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY PTMIINDT, PTMIINTM                                                                 ";

                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }


                    if (dt2.Rows.Count > 0)
                    {
                        strDRCODE = READ_LicToDRCode(dt2.Rows[0]["PTMIDRLC"].ToString().Trim());
                        nCnt2 += 1;

                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',2,";
                        SQL = SQL + " '" + dt2.Rows[0]["PTMIIDNO"].ToString().Trim() + "','" + dt2.Rows[0]["PTMINAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt2.Rows[0]["PTMISEXX"].ToString().Trim() + "','" + dt2.Rows[0]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt2.Rows[0]["WARDCODE"].ToString().Trim() + "',"  + dt2.Rows[0]["ROOMCODE"].ToString().Trim() + ",";
                        SQL = SQL + " '" + strDRCODE + "', ";
                        SQL = SQL + "  TO_DATE('" + strSDATE + "','YYYY-MM-DD HH24:MI'),";
                        SQL = SQL + "  TO_DATE('" + strEDATE + "','YYYY-MM-DD HH24:MI')  ) ";//  '응급환자 모니터링 P/G에서 조인용으로 사용함.(내원일시)

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }


                        //'발병일시
                        strTemp = dt2.Rows[0]["PTMIAKDT"].ToString().Trim();
                        strSDateT = VB.Left(strTemp, 4) + "-" + VB.Mid(strTemp, 5, 2) + "-" + VB.Right(strTemp, 2);
                        strTemp = dt2.Rows[0]["PTMIAKTM"].ToString().Trim();
                        strSDateT = strSDateT + " " + VB.Left(strTemp, 2) + ":" + VB.Right(strTemp, 2);

                        //'내원일시
                        strTemp = dt2.Rows[0]["PTMIINDT"].ToString().Trim();
                        strEDateT = VB.Left(strTemp, 4) + "-" + VB.Mid(strTemp, 5, 2) + "-" + VB.Right(strTemp, 2);
                        strTemp = dt2.Rows[0]["PTMIINTM"].ToString().Trim();
                        strEDateT = strEDateT + " " + VB.Left(strTemp, 2) + ":" + VB.Right(strTemp, 2);


                        if(DATE_TIME(strSDATE, strEDateT) <= 1440)
                        {
                            switch(dt2.Rows[0][""].ToString().Trim())
                            {
                                case "31":
                                case "32":
                                case "33":
                                case "34":
                                case "38":
                                    nCnt3 += 1;

                                    SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                                    SQL = SQL + ArgYYMM + "','" + strCODE + "',3,";
                                    SQL = SQL + " '" + dt2.Rows[0]["PTMIIDNO"].ToString().Trim() + "','" + dt2.Rows[0]["PTMINAME"].ToString().Trim() + "', ";
                                    SQL = SQL + " '" + dt2.Rows[0]["PTMISEXX"].ToString().Trim() + "','" + dt2.Rows[0]["DEPTCODE"].ToString().Trim() + "', ";
                                    SQL = SQL + " '" + dt2.Rows[0]["WARDCODE"].ToString().Trim() + "'," + dt2.Rows[0]["ROOMCODE"].ToString().Trim() + ",";
                                    SQL = SQL + " '" + strDRCODE + "', ";
                                    SQL = SQL + "  TO_DATE('" + strSDateT + "','YYYY-MM-DD HH24:MI'),";
                                    SQL = SQL + "  TO_DATE('" + dt2.Rows[0]["PTMIDCDT"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";//  '응급환자 모니터링 P/G에서 조인용으로 사용함.(내원일시)

                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        ComFunc.MsgBox(SqlErr);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        Cursor.Current = Cursors.Default;
                                        return rtnVal;
                                    }
                                    break;
                            }
                        }


                        if(DATE_TIME(strSDateT, strEDateT) <= 180)
                        {

                            nCnt4 += 1;

                            SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                            SQL = SQL + ArgYYMM + "','" + strCODE + "',4,";
                            SQL = SQL + " '" + dt2.Rows[0]["PTMIIDNO"].ToString().Trim() + "','" + dt2.Rows[0]["PTMINAME"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt2.Rows[0]["PTMISEXX"].ToString().Trim() + "','" + dt2.Rows[0]["DEPTCODE"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt2.Rows[0]["WARDCODE"].ToString().Trim() + "'," + dt2.Rows[0]["ROOMCODE"].ToString().Trim() + ",";
                            SQL = SQL + " '" + strDRCODE + "', ";
                            SQL = SQL + "  TO_DATE('" + strSDateT + "','YYYY-MM-DD HH24:MI'),";
                            SQL = SQL + "  TO_DATE('" + strEDateT + "','YYYY-MM-DD HH24:MI')  ) ";//  '응급환자 모니터링 P/G에서 조인용으로 사용함.(내원일시)

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }

                            //'퇴실일시
                            strTemp = dt2.Rows[0]["PTMIOTDT"].ToString().Trim();
                            strEDateT = VB.Left(strTemp, 4) + "-" + VB.Mid(strTemp, 5, 2) + "-" + VB.Right(strTemp, 2);
                            strTemp = dt2.Rows[0]["PTMIOTTM"].ToString().Trim();
                            strEDateT = strEDateT + " " + VB.Left(strTemp, 2) + ":" + VB.Right(strTemp, 2);

                            if (DATE_TIME(strSDateT, strEDateT) >= 30)
                            {

                                nCnt5 += 1;

                                SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                                SQL = SQL + ArgYYMM + "','" + strCODE + "',5,";
                                SQL = SQL + " '" + dt2.Rows[0]["PTMIIDNO"].ToString().Trim() + "','" + dt2.Rows[0]["PTMINAME"].ToString().Trim() + "', ";
                                SQL = SQL + " '" + dt2.Rows[0]["PTMISEXX"].ToString().Trim() + "','" + dt2.Rows[0]["DEPTCODE"].ToString().Trim() + "', ";
                                SQL = SQL + " '" + dt2.Rows[0]["WARDCODE"].ToString().Trim() + "'," + dt2.Rows[0]["ROOMCODE"].ToString().Trim() + ",";
                                SQL = SQL + " '" + strDRCODE + "', ";
                                SQL = SQL + "  TO_DATE('" + strSDateT + "','YYYY-MM-DD HH24:MI'),";
                                SQL = SQL + "  TO_DATE('" + strEDateT + "','YYYY-MM-DD HH24:MI')  ) ";//  '응급환자 모니터링 P/G에서 조인용으로 사용함.(내원일시)

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }
                            }


                            if (DATE_TIME(strSDateT, strEDateT) >= 180 && DATE_TIME(strSDateT, strEDateT) <= 720)
                            {

                                nCnt6 += 1;

                                SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                                SQL = SQL + ArgYYMM + "','" + strCODE + "',6,";
                                SQL = SQL + " '" + dt2.Rows[0]["PTMIIDNO"].ToString().Trim() + "','" + dt2.Rows[0]["PTMINAME"].ToString().Trim() + "', ";
                                SQL = SQL + " '" + dt2.Rows[0]["PTMISEXX"].ToString().Trim() + "','" + dt2.Rows[0]["DEPTCODE"].ToString().Trim() + "', ";
                                SQL = SQL + " '" + dt2.Rows[0]["WARDCODE"].ToString().Trim() + "'," + dt2.Rows[0]["ROOMCODE"].ToString().Trim() + ",";
                                SQL = SQL + " '" + strDRCODE + "', ";
                                SQL = SQL + "  TO_DATE('" + strSDateT + "','YYYY-MM-DD HH24:MI'),";
                                SQL = SQL + "  TO_DATE('" + strEDateT + "','YYYY-MM-DD HH24:MI')  ) ";//  '응급환자 모니터링 P/G에서 조인용으로 사용함.(내원일시)

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }



                                //'퇴실일시
                                strTemp = dt2.Rows[0]["PTMIOTDT"].ToString().Trim();
                                strEDateT = VB.Left(strTemp, 4) + "-" + VB.Mid(strTemp, 5, 2) + "-" + VB.Right(strTemp, 2);
                                strTemp = dt2.Rows[0]["PTMIOTTM"].ToString().Trim();
                                strEDateT = strEDateT + " " + VB.Left(strTemp, 2) + ":" + VB.Right(strTemp, 2);

                                if (DATE_TIME(strSDateT, strEDateT) >= 60)
                                {
                                    nCnt7 += 1;

                                    SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                                    SQL = SQL + ArgYYMM + "','" + strCODE + "',7,";
                                    SQL = SQL + " '" + dt2.Rows[0]["PTMIIDNO"].ToString().Trim() + "','" + dt2.Rows[0]["PTMINAME"].ToString().Trim() + "', ";
                                    SQL = SQL + " '" + dt2.Rows[0]["PTMISEXX"].ToString().Trim() + "','" + dt2.Rows[0]["DEPTCODE"].ToString().Trim() + "', ";
                                    SQL = SQL + " '" + dt2.Rows[0]["WARDCODE"].ToString().Trim() + "'," + dt2.Rows[0]["ROOMCODE"].ToString().Trim() + ",";
                                    SQL = SQL + " '" + strDRCODE + "', ";
                                    SQL = SQL + "  TO_DATE('" + strSDateT + "','YYYY-MM-DD HH24:MI'),";
                                    SQL = SQL + "  TO_DATE('" + strEDateT + "','YYYY-MM-DD HH24:MI')  ) ";//  '응급환자 모니터링 P/G에서 조인용으로 사용함.(내원일시)

                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        ComFunc.MsgBox(SqlErr);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        Cursor.Current = Cursors.Default;
                                        return rtnVal;
                                    }
                                }
                            }
                        }
                    }

                    dt2.Dispose();
                    dt2 = null;

                }

                dt.Dispose();
                dt = null;

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',3," + nCnt3 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',4," + nCnt4 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',5," + nCnt5 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',6," + nCnt6 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',7," + nCnt7 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO: 값 가져오는게 이상함
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        string READ_LicToDRCode(string arg)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strVal = "0000";

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT A.KORNAME, B.SABUN ";
                SQL += ComNum.VBLF + " FROM ADMIN.INSA_MST A, ADMIN.INSA_MSTL B ";
                SQL += ComNum.VBLF + " WHERE B.BUNHO = '" + arg.Trim() + "' ";
                SQL += ComNum.VBLF + "   AND A.SABUN = B.SABUN ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return strVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return strVal;
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return strVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return strVal;
            }
        }

        /// <summary>
        /// 응급환자 이동서비스(응급환자 귀가)
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns> 
        bool Build_91001_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;

            long nCnt1 = 0;
            long nCnt2 = 0;
            long nCnt3 = 0;

            string strCODE = "91001";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'응급환자귀가
                SQL = "SELECT a.PANO,a.DeptCode,a.WardCode,a.Room RoomCode,a.Sex,a.Age,b.SName, ";
                SQL += ComNum.VBLF + "  TO_CHAR(a.HoJinDate1,'YYYY-MM-DD HH24:MI') HoJinDate1, ";
                SQL += ComNum.VBLF + "  TO_CHAR(a.InTime,'YYYY-MM-DD HH24:MI') InTime, ";
                SQL += ComNum.VBLF + "  TO_CHAR(a.OutTime,'YYYY-MM-DD HH24:MI') OutTime ";
                SQL += ComNum.VBLF + "  FROM ADMIN.NUR_ER_PATIENT a, ADMIN.BAS_PATIENT b   ";
                SQL += ComNum.VBLF + " WHERE a.Pano=b.Pano(+) ";
                SQL += ComNum.VBLF + "   AND a.JDATE >= TO_DATE('" + argSDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND a.JDATE < TO_DATE('" + Convert.ToDateTime(argEDATE).AddDays(1).ToShortDateString() + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND a.OUTGBN ='2' ";
                SQL += ComNum.VBLF + "   AND a.HoJinDate1 IS NOT NULL ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    nCnt1 += 1; //전체인원

                    if (DATE_TIME(dt.Rows[i]["InTime"].ToString().Trim(), dt.Rows[i]["HoJinDate1"].ToString().Trim()) > 130)
                    {
                        nCnt2 += 1;
                        //해당명단
                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',2,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "', ";
                        //SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "','";
                        SQL = SQL + "TO_DATE('" + dt.Rows[i]["InTime"].ToString().Trim() + "', 'YYYY-MM-DD HH24:MI')" + ",";
                        SQL = SQL + "TO_DATE('" + dt.Rows[i]["HoJinDate1"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }

                    if (DATE_TIME(dt.Rows[i]["InTime"].ToString().Trim(), dt.Rows[i]["OutTime"].ToString().Trim()) > 160)
                    {
                        nCnt3 += 1;
                        //해당명단
                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',3,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "', ";
                        //SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "','";
                        SQL = SQL + "TO_DATE('" + dt.Rows[i]["HoJinDate1"].ToString().Trim() + "', 'YYYY-MM-DD HH24:MI')" + ",";
                        SQL = SQL + "TO_DATE('" + dt.Rows[i]["OutTime"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                //'총건수
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',3," + nCnt3 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 응급환자 이동서비스 -응급환자입원
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns> 
        bool Build_91002_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;

            long nCnt1 = 0;
            long nCnt2 = 0;
            long nCnt3 = 0;

            string strCODE = "91002";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'응급환자귀가
                SQL = "SELECT a.PANO,a.DeptCode,a.WardCode,a.Room RoomCode,a.Sex,a.Age,b.SName, ";
                SQL += ComNum.VBLF + "  TO_CHAR(a.HoJinDate1,'YYYY-MM-DD HH24:MI') HoJinDate1, ";
                SQL += ComNum.VBLF + "  TO_CHAR(a.InTime,'YYYY-MM-DD HH24:MI') InTime, ";
                SQL += ComNum.VBLF + "  TO_CHAR(a.OutTime,'YYYY-MM-DD HH24:MI') OutTime ";
                SQL += ComNum.VBLF + "  FROM ADMIN.NUR_ER_PATIENT a, ADMIN.BAS_PATIENT b   ";
                SQL += ComNum.VBLF + " WHERE a.Pano=b.Pano(+) ";
                SQL += ComNum.VBLF + "   AND a.JDATE >= TO_DATE('" + argSDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND a.JDATE < TO_DATE('" + Convert.ToDateTime(argEDATE).AddDays(1).ToShortDateString() + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND a.OUTGBN ='1' ";//   '입원
                SQL += ComNum.VBLF + "   AND a.OutTime IS NOT NULL ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    nCnt1 += 1; //전체인원

                    if (DATE_TIME(dt.Rows[i]["InTime"].ToString().Trim(), dt.Rows[i]["HoJinDate1"].ToString().Trim()) > 120)
                    {
                        nCnt2 += 1;
                        //해당명단
                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',2,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "', ";
                        //SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "','";
                        SQL = SQL + "TO_DATE('" + dt.Rows[i]["InTime"].ToString().Trim() + "', 'YYYY-MM-DD HH24:MI')" + ",";
                        SQL = SQL + "TO_DATE('" + dt.Rows[i]["HoJinDate1"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }

                    if (DATE_TIME(dt.Rows[i]["InTime"].ToString().Trim(), dt.Rows[i]["OutTime"].ToString().Trim()) > 190)
                    {
                        nCnt3 += 1;
                        //해당명단
                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',3,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "', ";
                        //SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "','";
                        SQL = SQL + "TO_DATE('" + dt.Rows[i]["HoJinDate1"].ToString().Trim() + "', 'YYYY-MM-DD HH24:MI')" + ",";
                        SQL = SQL + "TO_DATE('" + dt.Rows[i]["OutTime"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                //'총건수
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',3," + nCnt3 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 응급환자 이동서비스 -응급환자이송
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns> 
        bool Build_91003_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;

            long nCnt1 = 0;
            long nCnt2 = 0;
            long nCnt3 = 0;

            string strCODE = "91003";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'응급환자이송
                SQL = "SELECT a.PANO,a.DeptCode,a.WardCode,a.Room RoomCode,a.Sex,a.Age,b.SName, ";
                SQL += ComNum.VBLF + "  TO_CHAR(a.HoJinDate1,'YYYY-MM-DD HH24:MI') HoJinDate1, ";
                SQL += ComNum.VBLF + "  TO_CHAR(a.InTime,'YYYY-MM-DD HH24:MI') InTime, ";
                SQL += ComNum.VBLF + "  TO_CHAR(a.OutTime,'YYYY-MM-DD HH24:MI') OutTime ";
                SQL += ComNum.VBLF + "  FROM ADMIN.NUR_ER_PATIENT a, ADMIN.BAS_PATIENT b   ";
                SQL += ComNum.VBLF + " WHERE a.Pano=b.Pano(+) ";
                SQL += ComNum.VBLF + "   AND a.JDATE >= TO_DATE('" + argSDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND a.JDATE < TO_DATE('" + Convert.ToDateTime(argEDATE).AddDays(1).ToShortDateString() + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND a.OUTGBN ='6' ";//   '이송
                SQL += ComNum.VBLF + "   AND a.HoJinDate1 IS NOT NULL ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    nCnt1 += 1; //전체인원

                    if (DATE_TIME(dt.Rows[i]["InTime"].ToString().Trim(), dt.Rows[i]["HoJinDate1"].ToString().Trim()) > 120)
                    {
                        nCnt2 += 1;
                        //해당명단
                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',2,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "', ";
                        //SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "','";
                        SQL = SQL + "TO_DATE('" + dt.Rows[i]["InTime"].ToString().Trim() + "', 'YYYY-MM-DD HH24:MI')" + ",";
                        SQL = SQL + "TO_DATE('" + dt.Rows[i]["HoJinDate1"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }

                    if (DATE_TIME(dt.Rows[i]["InTime"].ToString().Trim(), dt.Rows[i]["OutTime"].ToString().Trim()) > 150)
                    {
                        nCnt3 += 1;
                        //해당명단
                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',3,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "', ";
                        //SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "','";
                        SQL = SQL + "TO_DATE('" + dt.Rows[i]["HoJinDate1"].ToString().Trim() + "', 'YYYY-MM-DD HH24:MI')" + ",";
                        SQL = SQL + "TO_DATE('" + dt.Rows[i]["OutTime"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                //'총건수
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',3," + nCnt3 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 응급환자 이동서비스 - 응급환자수술
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns> 
        bool Build_91004_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;

            long nCnt1 = 0;
            long nCnt2 = 0;
            long nCnt3 = 0;

            string strCODE = "91004";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                SQL = "SELECT a.PANO,a.DeptCode,a.WardCode,a.Room RoomCode,a.Sex,a.Age,b.SName, ";
                SQL += ComNum.VBLF + "  TO_CHAR(a.HoJinDate1,'YYYY-MM-DD HH24:MI') HoJinDate1, ";
                SQL += ComNum.VBLF + "  TO_CHAR(a.InTime,'YYYY-MM-DD HH24:MI') InTime, ";
                SQL += ComNum.VBLF + "  TO_CHAR(a.OutTime,'YYYY-MM-DD HH24:MI') OutTime ";
                SQL += ComNum.VBLF + "  FROM ADMIN.NUR_ER_PATIENT a, ADMIN.BAS_PATIENT b   ";
                SQL += ComNum.VBLF + " WHERE a.Pano=b.Pano(+) ";
                SQL += ComNum.VBLF + "   AND a.JDATE >= TO_DATE('" + argSDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND a.JDATE < TO_DATE('" + Convert.ToDateTime(argEDATE).AddDays(1).ToShortDateString() + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND a.OUTGBN ='9' ";//   '수술
                SQL += ComNum.VBLF + "   AND a.HoJinDate1 IS NOT NULL ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    nCnt1 += 1; //전체인원

                    if (DATE_TIME(dt.Rows[i]["InTime"].ToString().Trim(), dt.Rows[i]["HoJinDate1"].ToString().Trim()) > 140)
                    {
                        nCnt2 += 1;
                        //해당명단
                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',2,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "', ";
                        //SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "','";
                        SQL = SQL  + "TO_DATE('" + dt.Rows[i]["InTime"].ToString().Trim() + "', 'YYYY-MM-DD HH24:MI')" + ", ";
                        SQL = SQL  + "TO_DATE('" + dt.Rows[i]["HoJinDate1"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }

                    if (DATE_TIME(dt.Rows[i]["InTime"].ToString().Trim(), dt.Rows[i]["OutTime"].ToString().Trim()) > 220)
                    {
                        nCnt3 += 1;
                        //해당명단
                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',3,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "', ";
                        //SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "','";
                        SQL = SQL + "TO_DATE('" + dt.Rows[i]["HoJinDate1"].ToString().Trim() + "', 'YYYY-MM-DD HH24:MI')" + ", ";
                        SQL = SQL + "TO_DATE('" + dt.Rows[i]["OutTime"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                //'총건수
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',3," + nCnt3 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 중환자 중증도 분류
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns>
        bool Build_21011_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;

            string strFDate = "";
            string strTDate = "";
            string strToDate = "";
            string strNextDate = "";
            string strNewData = "";
            string strOldData = "";
            string strSysDate = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).ToShortDateString();

            long[] nCnt1 = new long[7]; //233
            long[] nCnt2 = new long[7]; //234

            int intGrade = 0;
            int i = 0;
            int j = 0;

            string strCODE = "21011";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'조회할 날짜 범위를 설정
                strFDate = argSDATE;
                strTDate = argEDATE;
                for(int KKK = 1;  KKK < 3; KKK++)
                {
                    for (i = 0; i < VB.Val(VB.Right(strTDate, 2)); i++)
                    {
                        strToDate = VB.Left(strFDate, 8) + (i + 1).ToString("00");
                        strNextDate = Convert.ToDateTime(strToDate).AddDays(1).ToShortDateString();


                        SQL = "";
                        SQL = " SELECT Pano,Grade FROM NUR_SERIOUS ";
                        SQL += ComNum.VBLF + "  WHERE Pano IN (";
                        SQL += ComNum.VBLF + "                   SELECT  Pano ";

                        if(strToDate == strSysDate)
                        {
                            SQL += ComNum.VBLF + "                    FROM IPD_NEW_MASTER  " ;
                            SQL += ComNum.VBLF + "                       WHERE (OutDate IS NULL OR OutDate>=TO_DATE('" + strNextDate + "','YYYY-MM-DD')) " ;
                            SQL += ComNum.VBLF + "                       AND IpwonTime < TO_DATE('" + strNextDate + "','YYYY-MM-DD') " ;
                            SQL += ComNum.VBLF + "                       AND Amset4 <> '3' ";
                        }
                        else
                        {
                            SQL += ComNum.VBLF + "                    FROM IPD_BM  ";
                            SQL += ComNum.VBLF + "                       WHERE JobDate=TO_DATE('" + strToDate + "','YYYY-MM-DD') " ;
                        }

                        SQL += ComNum.VBLF + "                       AND Pano < '90000000' ";
                        SQL += ComNum.VBLF + "                       AND Pano <> '81000004' ";

                        if(KKK == 1)
                        {
                            SQL += ComNum.VBLF + " AND RoomCode = '233' ";
                        }
                        else if(KKK == 2)
                        {
                            SQL += ComNum.VBLF + " AND RoomCode = '234' ";
                        }

                        SQL += ComNum.VBLF + "                   )  ";
                        SQL += ComNum.VBLF + "   AND JobTime <TO_DATE('" + strNextDate + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "   AND GbICU='Y' ";
                        SQL += ComNum.VBLF + "   ORDER BY Pano,JobTime DESC ";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        for(j = 0; j < dt.Rows.Count; j++)
                        {
                            strNewData = dt.Rows[j]["PANO"].ToString().Trim();
                            intGrade = (int)VB.Val(dt.Rows[j]["Grade"].ToString().Trim());
                            if (strNewData != strOldData && intGrade > 0)
                            {

                                if(KKK == 1)
                                {
                                    nCnt1[0] += 1; // 233;
                                }
                                else if(KKK == 2)
                                {
                                    nCnt2[0] += 1; // 234;
                                }


                                switch(intGrade)
                                {
                                    case 1:
                                    case 2:
                                    case 3:
                                    case 4:
                                    case 5:
                                    case 6:
                                        if(KKK == 1)
                                        {
                                            nCnt1[intGrade] += 1; // 233
                                        }
                                        else if(KKK == 2)
                                        {
                                            nCnt2[intGrade] += 1; // 234
                                        }
                                        break;
                                }
                            }
                            strOldData = dt.Rows[j]["Pano"].ToString().Trim();
                        }
                        dt.Dispose();
                        dt = null;

                    }
                }


                j = 0;
                for(i = 11; i < 18; i++)
                {
                    //'233 총건수 ~ 6 군
                    SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                    SQL = SQL + ArgYYMM + "','" + strCODE + "'," + i + "," + nCnt1[j] + ") ";

                    j += 1;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }


                j = 0;
                for (i = 21; i < 27; i++)
                {
                    //'234 총건수 ~ 6군
                    SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                    SQL = SQL + ArgYYMM + "','" + strCODE + "'," + i + "," + nCnt2[j] + ") ";

                    j += 1;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }


                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 임상질지표 - 모성 신생아(분만)
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns>
        bool Build_92011_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;

            long nCnt1 = 0; long nCnt2 = 0; long nCnt3 = 0; long nCnt4 = 0;
            long nCnt5 = 0; long nCnt6 = 0; long nCnt7 = 0; long nCnt8 = 0;
            long nCnt9 = 0; long nCnt10 = 0; long nCnt11 = 0;
            long nCnt12 = 0; long nCnt13 = 0; long nCnt14 = 0;

            int intTemp = 0;
            int intVal = 0;

            int i = 0;

            string strCODE = "92011";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = " SELECT A.CHULSANDATE, A.SUYU_SIDO_TIME, A.GBBUNMAN, A.FOOD_11, A.FOOD_12, A.FOOD_22, A.FOOD_32, ";
                SQL += ComNum.VBLF + " A.PANO, B.SNAME, B.AGE, B.SEX, B.DEPTCODE, B.WARDCODE, B.ROOMCODE, B.DRCODE ";
                SQL += ComNum.VBLF + " FROM NUR_JIPYO_MOSUNG A, IPD_NEW_MASTER B ";
                SQL += ComNum.VBLF + " WHERE B.OUTDATE >= TO_DATE('" + argSDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND B.OUTDATE <= TO_DATE('" + argEDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND A.GBDANTEA = '1' "; //'단태아
                SQL += ComNum.VBLF + "   AND A.JAETAEWEEK >= 37 "; //'재태기간 37주 이상
                SQL += ComNum.VBLF + "   AND A.WEIGTH >= 2500 "; //'체중 2500g 이상
                SQL += ComNum.VBLF + "   AND APGARSCORE >= 8 ";  //'APGARSCORE 8점 이상
                SQL += ComNum.VBLF + "   AND A.PANO = B.PANO ";
                SQL += ComNum.VBLF + "   AND A.GBBUNMAN IN ('1', '2') "; //'제외대상은 통계에서 제외
                SQL += ComNum.VBLF + "   AND A.GBENTRY IN ('1','2') ";
                SQL += ComNum.VBLF + "   AND TRUNC(A.OUTDATE) = TRUNC(B.OUTDATE) ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                for (i = 0; i < dt.Rows.Count; i++)
                {

                    intTemp = dt.Rows[i]["SUYU_SIDO_TIME"].ToString().Trim().Length == 0 ? 0 : DATE_TIME(dt.Rows[i]["CHULSANDATE"].ToString().Trim(), dt.Rows[i]["SUYU_SIDO_TIME"].ToString().Trim());

                    //'분만 후 모유수유시도 비율

                    if (dt.Rows[i]["GBBUNMAN"].ToString().Trim() == "1") // 질식분만
                    {
                        nCnt1 += 1; //'총 분만건수 - 질식분만


                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',1,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                        SQL = SQL + "  TO_DATE('" + dt.Rows[i]["CHULSANDATE"].ToString().Trim() + "','YYYY-MM-DD HH24:MI'),";
                        SQL = SQL + "  TO_DATE('" + dt.Rows[i]["SUYU_SIDO_TIME"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }


                        if (intTemp <= 60 && intTemp > 0) // 1시간 이내
                        {
                            nCnt2 += 1;
                            intVal = 2;
                        }
                        else if (intTemp > 60 && intTemp <= 360) // 6시간 이내
                        {
                            nCnt3 += 1;
                            intVal = 3;
                        }
                        else if (intTemp > 360 && intTemp <= 720) // 12시간 이내
                        {
                            nCnt4 += 1;
                            intVal = 4;
                        }
                        else if (intTemp > 720 && intTemp <= 1440) // 24시간 이내
                        {
                            nCnt5 += 1;
                            intVal = 5;
                        }
                        else if (intTemp > 1440) // 24시간 이후
                        {
                            nCnt6 += 1;
                            intVal = 6;
                        }
                    }
                    else if (dt.Rows[i]["GBBUNMAN"].ToString().Trim() == "2")
                    {
                        nCnt7 += 1;
                        intVal = 7;

                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',7,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                        SQL = SQL + "  TO_DATE('" + dt.Rows[i]["CHULSANDATE"].ToString().Trim() + "','YYYY-MM-DD HH24:MI'),";
                        SQL = SQL + "  TO_DATE('" + dt.Rows[i]["SUYU_SIDO_TIME"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        if (intTemp <= 180 && intTemp > 0) // 3시간 이내
                        {
                            nCnt8 += 1;
                            intVal = 8;
                        }
                        else if (intTemp > 180 && intTemp <= 360) // 6시간 이내
                        {
                            nCnt9 += 1;
                            intVal = 9;
                        }
                        else if (intTemp > 360 && intTemp <= 720) // 12시간 이내
                        {
                            nCnt10 += 1;
                            intVal = 10;
                        }
                        else if (intTemp > 720 && intTemp <= 1440) // 24시간 이내
                        {
                            nCnt11 += 1;
                            intVal = 11;
                        }
                        else if (intTemp > 1441) // 24시간 이후
                        {
                            nCnt12 += 1;
                            intVal = 12;
                        }
                    }

                    SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                    SQL = SQL + ArgYYMM + "','" + strCODE + "'," + intVal + ",";
                    SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                    SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                    SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                    SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                    SQL = SQL + "  TO_DATE('" + dt.Rows[i]["CHULSANDATE"].ToString().Trim() + "','YYYY-MM-DD HH24:MI'),";
                    SQL = SQL + "  TO_DATE('" + dt.Rows[i]["SUYU_SIDO_TIME"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }


                    //'분만 후 공급된 음식물의 내용과 방법
                    if (dt.Rows[i]["FOOD_11"].ToString().Trim() == "1" || dt.Rows[i]["FOOD_12"].ToString().Trim() == "1" &&
                        dt.Rows[i]["FOOD_22"].ToString().Trim() == "2" && dt.Rows[i]["Food_32"].ToString().Trim() == "2") // 젖물림, 컵혹은 스푼
                    {
                        nCnt13 += 1;
                        intVal = 13;
                    }
                    else if (dt.Rows[i]["FOOD_11"].ToString().Trim() == "1" || dt.Rows[i]["FOOD_12"].ToString().Trim() == "1" ||
                             dt.Rows[i]["FOOD_22"].ToString().Trim() == "1" || dt.Rows[i]["Food_32"].ToString().Trim() == "1") //젖물림, 컵 혹은 스푼 + 물, 포도당(컵 혹은 스푼)
                    {
                        nCnt14 += 1;
                        intVal = 14;
                    }

                    SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                    SQL = SQL + ArgYYMM + "','" + strCODE + "'," + intVal + ",";
                    SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                    SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                    SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                    SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                    SQL = SQL + "  TO_DATE('" + dt.Rows[i]["CHULSANDATE"].ToString().Trim() + "','YYYY-MM-DD HH24:MI'),";
                    SQL = SQL + "  TO_DATE('" + dt.Rows[i]["SUYU_SIDO_TIME"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                }



                //총건수 - 질식분만
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //질식분만 - 1시간전
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //질식분만 - 6시간전
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',3," + nCnt3 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //질식분만 - 12시간전
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',4," + nCnt4 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //질식분만 - 24시간전
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',5," + nCnt5 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //질식분만 - 24시간초과
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',6," + nCnt6 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //총건수 - 제왕절개
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',7," + nCnt7 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //제왕절개 - 3시간전
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',8," + nCnt8 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //제왕절개 - 6시간전
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',9," + nCnt9 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //제왕절개 - 12시간전
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',10," + nCnt10 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //제왕절개 - 24시간전
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',11," + nCnt11 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //제왕절개 - 24시간 초과
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',12," + nCnt12 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //음식물 - 젖물림, 컵 혹은 스푼
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',13," + nCnt13 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                //음식물 - + 물, 포도당(컵 혹은 스푼)
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',14," + nCnt14 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 임상질지표 - 모성 신생아(젖물림)
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns>
        bool Build_92012_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;

            long nCnt1 = 0; long nCnt2 = 0; long nCnt3 = 0; long nCnt4 = 0;
            long nCnt5 = 0; long nCnt6 = 0; long nCnt7 = 0; long nCnt8 = 0;
            long nCnt9 = 0;

            int intTemp = 0;
            int intVal = 0;

            int i = 0;

            string strCODE = "92012";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                SQL = " SELECT A.SUYU_SAYU2, A.CHULSANDATE, A.SUYU_SIDO_TIME, A.GBBUNMAN, A.FOOD_11, A.FOOD_12, A.FOOD_22, A.FOOD_32, ";
                SQL += ComNum.VBLF + " A.PANO, B.SNAME, B.AGE, B.SEX, B.DEPTCODE, B.WARDCODE, B.ROOMCODE, B.DRCODE ";
                SQL += ComNum.VBLF + " FROM NUR_JIPYO_MOSUNG A, IPD_NEW_MASTER B ";
                SQL += ComNum.VBLF + " WHERE B.OUTDATE >= TO_DATE('" + argSDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND B.OUTDATE <=  TO_DATE('" + argEDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND A.GBDANTEA = '1' ";  //'단태아
                SQL += ComNum.VBLF + "   AND A.JAETAEWEEK >= 37 ";  //'재태기간 37주 이상
                SQL += ComNum.VBLF + "   AND A.WEIGTH >= 2500 ";   //'체중 2500g 이상
                SQL += ComNum.VBLF + "   AND APGARSCORE >= 8 ";    //'APGARSCORE 8점 이상
                SQL += ComNum.VBLF + "   AND A.PANO = B.PANO ";
                SQL += ComNum.VBLF + "   AND A.GBENTRY IN ('1','2') ";
                SQL += ComNum.VBLF + "   AND TRUNC(A.OUTDATE) = TRUNC(B.OUTDATE) ";


                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                for (i = 0; i < dt.Rows.Count; i++)
                {

                    intTemp = dt.Rows[i]["SUYU_SIDO_TIME"].ToString().Trim().Length == 0 ? 0 : DATE_TIME(dt.Rows[i]["CHULSANDATE"].ToString().Trim(), dt.Rows[i]["SUYU_SIDO_TIME"].ToString().Trim());

                    //'분만 후 모유수유시도 비율
                    nCnt1 += 1; //'총 분만건수 - 질식분만


                    SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                    SQL = SQL + ArgYYMM + "','" + strCODE + "',1,";
                    SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                    SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                    SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                    SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                    SQL = SQL + "  TO_DATE('" + dt.Rows[i]["CHULSANDATE"].ToString().Trim() + "','YYYY-MM-DD HH24:MI'),";
                    SQL = SQL + "  TO_DATE('" + dt.Rows[i]["SUYU_SIDO_TIME"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }


                    if (dt.Rows[i]["GBBUNMAN"].ToString().Trim() == "1" && dt.Rows[i]["SUYU_SAYU2"].ToString().Trim() != "1") // 질식분만
                    {
                        nCnt2 += 1;

                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',7,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                        SQL = SQL + "  TO_DATE('" + dt.Rows[i]["CHULSANDATE"].ToString().Trim() + "','YYYY-MM-DD HH24:MI'),";
                        SQL = SQL + "  TO_DATE('" + dt.Rows[i]["SUYU_SIDO_TIME"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        //분만 후 공급된 음식물의 내용과 방법
                        if (dt.Rows[i]["FOOD_11"].ToString().Trim() == "1" || dt.Rows[i]["FOOD_11"].ToString().Trim() == "1" &&
                            dt.Rows[i]["Food_22"].ToString().Trim() == "2" && dt.Rows[i]["Food_32"].ToString().Trim() == "2")
                        {
                            nCnt3 += 1;
                            intVal = 3;
                        }
                        else if (dt.Rows[i]["FOOD_11"].ToString().Trim() == "1" || dt.Rows[i]["FOOD_12"].ToString().Trim() == "1" ||
                                 dt.Rows[i]["Food_22"].ToString().Trim() == "1" || dt.Rows[i]["Food_32"].ToString().Trim() == "1")
                        {
                            nCnt4 += 1;
                            intVal = 4;
                        }
                    }
                    else if (dt.Rows[i]["GBBUNMAN"].ToString().Trim() == "2" && dt.Rows[i]["SUYU_SAYU2"].ToString().Trim() != "1") // 질식분만
                    {
                        nCnt6 += 1;

                        if (dt.Rows[i]["FOOD_11"].ToString().Trim() == "1" || dt.Rows[i]["FOOD_12"].ToString().Trim() == "1" &&
                            dt.Rows[i]["Food_22"].ToString().Trim() == "2" && dt.Rows[i]["Food_32"].ToString().Trim() == "2")
                        {
                            nCnt7 += 1;
                            intVal = 7;
                        }
                        else if (dt.Rows[i]["FOOD_11"].ToString().Trim() == "1" || dt.Rows[i]["FOOD_12"].ToString().Trim() == "1" ||
                                 dt.Rows[i]["Food_22"].ToString().Trim() == "1" || dt.Rows[i]["Food_32"].ToString().Trim() == "1")
                        {
                            nCnt8 += 1;
                            intVal = 8;
                        }
                    }


                    SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                    SQL = SQL + ArgYYMM + "','" + strCODE + "'," + intVal + ",";
                    SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                    SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                    SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                    SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                    SQL = SQL + "  TO_DATE('" + dt.Rows[i]["CHULSANDATE"].ToString().Trim() + "','YYYY-MM-DD HH24:MI'),";
                    SQL = SQL + "  TO_DATE('" + dt.Rows[i]["SUYU_SIDO_TIME"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }


                    //중도거부자
                    if (dt.Rows[i]["SUYU_SAYU2"].ToString().Trim() == "2")
                    {
                        nCnt5 += 1;
                        intVal = 5;

                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "'," + intVal + ",";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                        SQL = SQL + "  TO_DATE('" + dt.Rows[i]["CHULSANDATE"].ToString().Trim() + "','YYYY-MM-DD HH24:MI'),";
                        SQL = SQL + "  TO_DATE('" + dt.Rows[i]["SUYU_SIDO_TIME"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }

                    //'입원시 모유수유거부율
                    if (dt.Rows[i]["SUYU_SAYU2"].ToString().Trim() == "1")
                    {
                        nCnt9 += 1;
                        intVal = 9;

                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "'," + intVal + ",";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                        SQL = SQL + "  TO_DATE('" + dt.Rows[i]["CHULSANDATE"].ToString().Trim() + "','YYYY-MM-DD HH24:MI'),";
                        SQL = SQL + "  TO_DATE('" + dt.Rows[i]["SUYU_SIDO_TIME"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }



                //총건수 - 질식분만
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //질식분만 - 1시간전
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //질식분만 - 6시간전
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',3," + nCnt3 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //질식분만 - 12시간전
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',4," + nCnt4 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //질식분만 - 24시간전
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',5," + nCnt5 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //질식분만 - 24시간초과
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',6," + nCnt6 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //총건수 - 제왕절개
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',7," + nCnt7 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //제왕절개 - 3시간전
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',8," + nCnt8 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //제왕절개 - 6시간전
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',9," + nCnt9 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 임상질지표 - 중환자실 - 기계호흡환자
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns>
        bool Build_92021_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;
            DataTable dt2 = null;

            long nCnt1 = 0; long nCnt2 = 0; long nCnt3 = 0; long nCnt4 = 0;
            long nCnt5 = 0; long nCnt6 = 0; long nCnt7 = 0; long nCnt8 = 0;
            long nCnt9 = 0;
            long nCnt10 = 0;


            string strPano = ""; string  strSName= ""; string  strAge= "";string  strSex= "";string  strDeptCode= "";string  strWARDCODE= ""; string strRoomCode = ""; string strDRCODE= "";

            int i = 0;

            string strCODE = "92021";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                SQL = "  SELECT PANO, JDATE, GH_YN, SG_GG_YN, SG_YN, GY_TUYAK, DVT_YN, ";
                SQL += ComNum.VBLF + "         DVT_GG_YN , DVT_TUYAK, TG_GG_YN, TG_YN, JG_YN ";
                SQL += ComNum.VBLF + "  FROM NUR_JIPYO_ICU_DAILY ";
                SQL += ComNum.VBLF + " WHERE JDATE >= '" + argSDATE + "'";
                SQL += ComNum.VBLF + "   AND JDATE <= '" + argEDATE + "'";
                SQL += ComNum.VBLF + "   AND Pano<>'81000004'";
                //'진정상태는 무조건 100%라고 ICU에서 이야기 함.
                //'일수에 포함되는 건의 기준은 ICU입실시간이 24시간 FULL인경우임.
                //'진정상태가 'N'인 건은 ICU 입실일과 퇴실일임(입실시간이 24시간이 안되기 때문.)
                SQL += ComNum.VBLF + "   AND JG_YN = 'Y' ";
                 //'SQL = SQL & "  and pano in( '06979240', '06979286') "
                SQL += ComNum.VBLF + " ORDER BY JDATE, PANO";


                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                for (i = 0; i < dt.Rows.Count; i++)
                {
                    SQL = " SELECT PANO, SNAME, Age, Sex, DEPTCODE, WardCode, ROOMCODE, DRCODE ";
                    SQL += ComNum.VBLF + " FROM IPD_NEW_MASTER ";
                    SQL += ComNum.VBLF + " WHERE PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "'";
                    SQL += ComNum.VBLF + "   AND TRUNC(INDATE) <= TO_DATE('" +   dt.Rows[i]["JDATE"].ToString().Trim() + "', 'YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "   AND (TRUNC(ACTDATE) >= TO_DATE('" + dt.Rows[i]["JDATE"].ToString().Trim() + "', 'YYYY-MM-DD') OR ACTDATE IS NULL) ";
                    SQL += ComNum.VBLF + "   AND GBSTS <> '9' ";

                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if(dt2.Rows.Count > 0)
                    {
                        strPano = dt2.Rows[0]["PANO"].ToString().Trim();
                        strSName = dt2.Rows[0]["SNAME"].ToString().Trim();
                        strAge = dt2.Rows[0]["AGE"].ToString().Trim();
                        strSex = dt2.Rows[0]["SEX"].ToString().Trim();
                        strDeptCode = dt2.Rows[0]["DEPTCODE"].ToString().Trim();
                        strWARDCODE = dt2.Rows[0]["WARDCODE"].ToString().Trim();
                        strRoomCode = dt2.Rows[0]["ROOMCODE"].ToString().Trim();
                        strDRCODE = dt2.Rows[0]["DRCODE"].ToString().Trim();
                    }

                    dt2.Dispose();
                    dt2 = null;


                    if(dt.Rows[i]["GH_YN"].ToString().Trim() == "Y" && dt.Rows[i]["SG_GG_YN"].ToString().Trim().Length == 0)
                    {
                        nCnt1 += 1;

                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',1,";
                        SQL += ComNum.VBLF +  " '" + strPano + "','" + strSName + "', ";
                        SQL += ComNum.VBLF +  " '" + strSex + "','" + strDeptCode + "', ";
                        SQL += ComNum.VBLF +  " '" + strWARDCODE + "'," + strRoomCode + ",";
                        SQL += ComNum.VBLF +  " '" + strDRCODE + "', ";
                        SQL += ComNum.VBLF + "  TO_DATE('" + dt.Rows[i]["JDATE"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }


                    if (dt.Rows[i]["GH_YN"].ToString().Trim() == "Y" && dt.Rows[i]["SG_GG_YN"].ToString().Trim().Length == 0 && dt.Rows[i]["SG_YN"].ToString().Trim() == "Y")
                    {
                        nCnt2 += 1;

                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',2,";
                        SQL += ComNum.VBLF + " '" + strPano + "','" + strSName + "', ";
                        SQL += ComNum.VBLF + " '" + strSex + "','" + strDeptCode + "', ";
                        SQL += ComNum.VBLF + " '" + strWARDCODE + "'," + strRoomCode + ",";
                        SQL += ComNum.VBLF + " '" + strDRCODE + "', ";
                        SQL += ComNum.VBLF + "  TO_DATE('" + dt.Rows[i]["JDATE"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }



                    if (dt.Rows[i]["GH_YN"].ToString().Trim() == "Y")
                    {
                        nCnt3 += 1;

                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',3'";
                        SQL += ComNum.VBLF + " '" + strPano + "','" + strSName + "', ";
                        SQL += ComNum.VBLF + " '" + strSex + "','" + strDeptCode + "', ";
                        SQL += ComNum.VBLF + " '" + strWARDCODE + "'," + strRoomCode + ",";
                        SQL += ComNum.VBLF + " '" + strDRCODE + "', ";
                        SQL += ComNum.VBLF + "  TO_DATE('" + dt.Rows[i]["JDATE"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }



                    if (dt.Rows[i]["GH_YN"].ToString().Trim() == "Y" && dt.Rows[i]["GY_TUYAK"].ToString().Trim() == "Y")
                    {
                        nCnt4 += 1;

                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',4'";
                        SQL += ComNum.VBLF + " '" + strPano + "','" + strSName + "', ";
                        SQL += ComNum.VBLF + " '" + strSex + "','" + strDeptCode + "', ";
                        SQL += ComNum.VBLF + " '" + strWARDCODE + "'," + strRoomCode + ",";
                        SQL += ComNum.VBLF + " '" + strDRCODE + "', ";
                        SQL += ComNum.VBLF + "  TO_DATE('" + dt.Rows[i]["JDATE"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }



                    if (dt.Rows[i]["GH_YN"].ToString().Trim() == "Y" && dt.Rows[i]["DVT_YN"].ToString().Trim() == "Y" && dt.Rows[i]["DVT_GG_YN"].ToString().Trim().Length == 0)
                    {
                        nCnt5 += 1;

                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',5'";
                        SQL += ComNum.VBLF + " '" + strPano + "','" + strSName + "', ";
                        SQL += ComNum.VBLF + " '" + strSex + "','" + strDeptCode + "', ";
                        SQL += ComNum.VBLF + " '" + strWARDCODE + "'," + strRoomCode + ",";
                        SQL += ComNum.VBLF + " '" + strDRCODE + "', ";
                        SQL += ComNum.VBLF + "  TO_DATE('" + dt.Rows[i]["JDATE"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }




                    if (dt.Rows[i]["GH_YN"].ToString().Trim() == "Y" && dt.Rows[i]["DVT_YN"].ToString().Trim() == "Y" && dt.Rows[i]["DVT_GG_YN"].ToString().Trim().Length == 0 &&
                        dt.Rows[i]["DVT_TUYAK"].ToString().Trim() == "Y")
                    {
                        nCnt6 += 1;

                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',6'";
                        SQL += ComNum.VBLF + " '" + strPano + "','" + strSName + "', ";
                        SQL += ComNum.VBLF + " '" + strSex + "','" + strDeptCode + "', ";
                        SQL += ComNum.VBLF + " '" + strWARDCODE + "'," + strRoomCode + ",";
                        SQL += ComNum.VBLF + " '" + strDRCODE + "', ";
                        SQL += ComNum.VBLF + "  TO_DATE('" + dt.Rows[i]["JDATE"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }

                    if (dt.Rows[i]["TG_GG_YN"].ToString().Trim() == "Y")
                    {
                        nCnt7 += 1;

                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',7'";
                        SQL += ComNum.VBLF + " '" + strPano + "','" + strSName + "', ";
                        SQL += ComNum.VBLF + " '" + strSex + "','" + strDeptCode + "', ";
                        SQL += ComNum.VBLF + " '" + strWARDCODE + "'," + strRoomCode + ",";
                        SQL += ComNum.VBLF + " '" + strDRCODE + "', ";
                        SQL += ComNum.VBLF + "  TO_DATE('" + dt.Rows[i]["JDATE"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }



                    if (dt.Rows[i]["TG_GG_YN"].ToString().Trim() == "Y" && dt.Rows[i]["TG_YN"].ToString().Trim() == "Y")
                    {
                        nCnt8 += 1;

                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',8'";
                        SQL += ComNum.VBLF + " '" + strPano + "','" + strSName + "', ";
                        SQL += ComNum.VBLF + " '" + strSex + "','" + strDeptCode + "', ";
                        SQL += ComNum.VBLF + " '" + strWARDCODE + "'," + strRoomCode + ",";
                        SQL += ComNum.VBLF + " '" + strDRCODE + "', ";
                        SQL += ComNum.VBLF + "  TO_DATE('" + dt.Rows[i]["JDATE"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
       
                    nCnt9 += 1;
                    SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1) VALUES ('";
                    SQL = SQL + ArgYYMM + "','" + strCODE + "',9'";
                    SQL += ComNum.VBLF + " '" + strPano + "','" + strSName + "', ";
                    SQL += ComNum.VBLF + " '" + strSex + "','" + strDeptCode + "', ";
                    SQL += ComNum.VBLF + " '" + strWARDCODE + "'," + strRoomCode + ",";
                    SQL += ComNum.VBLF + " '" + strDRCODE + "', ";
                    SQL += ComNum.VBLF + "  TO_DATE('" + dt.Rows[i]["JDATE"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }



                    if (dt.Rows[i]["JG_YN"].ToString().Trim() == "Y" )
                    {
                        nCnt10 += 1;

                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',10'";
                        SQL += ComNum.VBLF + " '" + strPano + "','" + strSName + "', ";
                        SQL += ComNum.VBLF + " '" + strSex + "','" + strDeptCode + "', ";
                        SQL += ComNum.VBLF + " '" + strWARDCODE + "'," + strRoomCode + ",";
                        SQL += ComNum.VBLF + " '" + strDRCODE + "', ";
                        SQL += ComNum.VBLF + "  TO_DATE('" + dt.Rows[i]["JDATE"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }

                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',3," + nCnt3 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',4," + nCnt4 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',5," + nCnt5 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',6," + nCnt6 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',7," + nCnt7 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',8," + nCnt8 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',9," + nCnt9 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',10," + nCnt9 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 임상질지표 - 폐렴
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns>
        bool Build_92031_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;

            long nCnt1 = 0; long nCnt2 = 0; long nCnt3 = 0; long nCnt4 = 0;
            long nCnt5 = 0; long nCnt6 = 0; long nCnt7 = 0; long nCnt8 = 0;
            long nCnt9 = 0;
            long nCnt10 = 0;
            
            string strPano = "";
            string strSName = "";
            string strAge = "";
            string strSex = "";
            string strDeptCode = "";
            string strWARDCODE = "";
            string strRoomCode = "";
            string strDRCODE = "";

            int intTemp = 0;
            string strTempA = "";      

            int i = 0;

            string strCODE = "92031";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                SQL = " SELECT A.DMH_TIME, A.MBSS_TIME, A.IN_TIME, A.HB_EXAM_YN, A.HSJ_STIME_01, A.HB_EXAM_TIME, A.GYSD_YN, ";
                SQL += ComNum.VBLF + " A.GYSD_DOCTOR, A.GYSD_NURSE, A.SMOOK_GBN, A.GYSD_NOT,  ";
                SQL += ComNum.VBLF + " A.PANO, B.SNAME, B.AGE, B.SEX, B.DEPTCODE, B.WARDCODE, B.ROOMCODE, B.DRCODE ";
                SQL += ComNum.VBLF + "   FROM NUR_JIPYO_PNEUMONIA A, IPD_NEW_MASTER B";
                //' 2009-06-17 전혜경쌤 요청으로 퇴원일자로
                //'    SQL = SQL & " WHERE A.IN_TIME >= '" & ArgSDate & "'"
                //'    SQL = SQL & "   AND A.IN_TIME <= '" & ArgEDate & "'"
                SQL += ComNum.VBLF + " WHERE B.OUTDATE >= TO_DATE('" + argSDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND B.OUTDATE <= TO_DATE('" + argEDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND A.PANO = B.PANO ";
                SQL += ComNum.VBLF + "   AND A.IPDNO = B.IPDNO ";
                SQL += ComNum.VBLF + "   AND A.GBENTRY = '1' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                for (i = 0; i < dt.Rows.Count; i++)
                {
              
                    strPano = dt.Rows[i]["PANO"].ToString().Trim();
                    strSName = dt.Rows[i]["SNAME"].ToString().Trim();
                    strAge = dt.Rows[i]["AGE"].ToString().Trim();
                    strSex = dt.Rows[i]["SEX"].ToString().Trim();
                    strDeptCode = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    strWARDCODE = dt.Rows[i]["WARDCODE"].ToString().Trim();
                    strRoomCode = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                    strDRCODE = dt.Rows[i]["DRCODE"].ToString().Trim();
                 
                    nCnt1 += 1;

                    SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1) VALUES ('";
                    SQL = SQL + ArgYYMM + "','" + strCODE + "',1,";
                    SQL += ComNum.VBLF + " '" + strPano + "','" + strSName + "', ";
                    SQL += ComNum.VBLF + " '" + strSex + "','" + strDeptCode + "', ";
                    SQL += ComNum.VBLF + " '" + strWARDCODE + "'," + strRoomCode + ",";
                    SQL += ComNum.VBLF + " '" + strDRCODE + "', ";
                    SQL += ComNum.VBLF + "  TO_DATE('" + dt.Rows[i]["JDATE"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }


                    strTempA = dt.Rows[i]["DMH_TIME"].ToString().Trim().Length == 0 ? dt.Rows[i]["MBss_TIME"].ToString().Trim() : dt.Rows[i]["DMH_TIME"].ToString().Trim();
                    intTemp = DATE_TIME(dt.Rows[i]["IN_TIME"].ToString().Trim(), strTempA);


                    if(intTemp >= 0 && intTemp <= 1440)
                    {
                        nCnt2 += 1;

                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',2,";
                        SQL = SQL + " '" + strPano + "','" + strSName + "', ";
                        SQL = SQL + " '" + strSex + "','" + strDeptCode + "', ";
                        SQL = SQL + " '" + strWARDCODE + "'," + strRoomCode + ",";
                        SQL = SQL + " '" + strDRCODE + "', ";
                        SQL = SQL + "  TO_DATE('" + strTempA + "','YYYY-MM-DD HH24:MI'),";
                        SQL = SQL + "  TO_DATE('" + dt.Rows[i]["IN_TIME"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }

                    if(dt.Rows[i]["HB_EXAM_YN"].ToString().Trim() == "1")
                    {
                        nCnt3 += 1;

                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',3,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                        SQL = SQL + "  TO_DATE('" + argSDATE + "','YYYY-MM-DD HH24:MI'),";
                        SQL = SQL + "  TO_DATE('" + argEDATE + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }



                        intTemp = DATE_TIME(dt.Rows[i]["HB_EXAM_TIME"].ToString().Trim(), dt.Rows[i]["HSJ_STIME_01"].ToString().Trim());
                        if (intTemp > 0)
                        {
                            nCnt4 += 1;


                            SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                            SQL = SQL + ArgYYMM + "','" + strCODE + "',4,";
                            SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                            SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                            SQL = SQL + "  TO_DATE('" + dt.Rows[i]["HSJ_STIME_01"].ToString().Trim() + "','YYYY-MM-DD HH24:MI'),";
                            SQL = SQL + "  TO_DATE('" + dt.Rows[i]["HB_EXAM_TIME"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }

                    }

                    nCnt5 += 1;
                    SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                    SQL = SQL + ArgYYMM + "','" + strCODE + "',5,";
                    SQL = SQL + " '" + dt.Rows[i][ "PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                    SQL = SQL + " '" + dt.Rows[i][ "SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                    SQL = SQL + " '" + dt.Rows[i][ "WARDCODE"].ToString().Trim() + "'," + dt.Rows[i]["ROOMCODE"].ToString().Trim() + ",";
                    SQL = SQL + " '" + dt.Rows[i][ "DRCODE"].ToString().Trim() + "', ";
                    SQL = SQL + "  TO_DATE('" + argSDATE + "','YYYY-MM-DD HH24:MI'),";
                    SQL = SQL + "  TO_DATE('" + argEDATE + "','YYYY-MM-DD HH24:MI')  ) ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }


                    //병원 도착 후 8시간 이내 첫 항생제를 투여 받은 폐렴 환자 비율
                    intTemp = DATE_TIME(dt.Rows[i]["IN_TIME"].ToString().Trim(), dt.Rows[i]["HSJ_STIME_01"].ToString().Trim());

                    if(intTemp > 0 && intTemp <= 480)
                    {
                        nCnt6 += 1;

                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',6,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                        SQL = SQL + "  TO_DATE('" + dt.Rows[i]["IN_TIME"].ToString().Trim() + "','YYYY-MM-DD HH24:MI'),";
                        SQL = SQL + "  TO_DATE('" + dt.Rows[i]["HSJ_STIME_01"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }

                    //'금연 상담
                    //'흡연력이 없는 사람 제외, 상담불가능자 제외


                    if(dt.Rows[i]["SMOOK_GBN"].ToString().Trim() != "2" && dt.Rows[i]["GYSD_NOT"].ToString().Trim() != "1")
                    {
                        nCnt7 += 1;

                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',7,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                        SQL = SQL + "  TO_DATE('" + argSDATE + "','YYYY-MM-DD HH24:MI'),";
                        SQL = SQL + "  TO_DATE('" + argEDATE + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }


                        if (dt.Rows[i]["SMOOK_GBN"].ToString().Trim() == "1")
                        {
                            nCnt8 += 1;

                            SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                            SQL = SQL + ArgYYMM + "','" + strCODE + "',8,";
                            SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" +    dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" +     dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                            SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                            SQL = SQL + "  TO_DATE('" + argSDATE + "','YYYY-MM-DD HH24:MI'),";
                            SQL = SQL + "  TO_DATE('" + argEDATE + "','YYYY-MM-DD HH24:MI')  ) ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }



                        if (dt.Rows[i]["GYSD_DOCTOR"].ToString().Trim() == "1")
                        {
                            nCnt9 += 1;

                            SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                            SQL = SQL + ArgYYMM + "','" + strCODE + "',9,";
                            SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                            SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                            SQL = SQL + "  TO_DATE('" + argSDATE + "','YYYY-MM-DD HH24:MI'),";
                            SQL = SQL + "  TO_DATE('" + argEDATE + "','YYYY-MM-DD HH24:MI')  ) ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }


                        if (dt.Rows[i]["GYSD_NURSE"].ToString().Trim() == "1")
                        {
                            nCnt10 += 1;

                            SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                            SQL = SQL + ArgYYMM + "','" + strCODE + "',10,";
                            SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                            SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                            SQL = SQL + "  TO_DATE('" + argSDATE + "','YYYY-MM-DD HH24:MI'),";
                            SQL = SQL + "  TO_DATE('" + argEDATE + "','YYYY-MM-DD HH24:MI')  ) ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }
                    }
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',3," + nCnt3 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',4," + nCnt4 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',5," + nCnt5 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',6," + nCnt6 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',7," + nCnt7 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',8," + nCnt8 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',9," + nCnt9 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',10," + nCnt9 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 임상질지표 - 수술 감염 예방적 항생제
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns>
        bool Build_92041_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;
            DataTable dt2 = null;

            long nCnt1 = 0; long nCnt2 = 0; long nCnt3 = 0; long nCnt4 = 0;
            long nCnt5 = 0; long nCnt6 = 0; long nCnt7 = 0; long nCnt8 = 0;
            long nCnt9 = 0;
            long nCnt10 = 0;
            long nCnt11 = 0;
            long nCnt12 = 0;
            long nCnt13 = 0;
            long nCnt14 = 0;
            long nCnt15 = 0;
            long nCnt16 = 0;
            long nCnt17 = 0;
            long nCnt18 = 0;

            string strWRTNO = "";

            int i = 0;
            int k = 0;

            string strTemp = "";


            string strCODE = "92041";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                SQL = " SELECT A.PANO, A.WRTNO, A.OPDATE, A.INFECTION, A.INFECTIONDATE, A.OPSTIME, A.OPETIME, ";
                SQL += ComNum.VBLF + " B.SNAME , B.Age, B.Sex, B.DEPTCODE, B.WardCode, B.ROOMCODE, B.DRCODE, A.AVOID, A.TUYAKILSU, A.SUSUNGB  ";
                SQL += ComNum.VBLF + " FROM NUR_JIPYO_ORAN A, ORAN_MASTER B, IPD_NEW_MASTER C ";
                SQL += ComNum.VBLF + " Where A.WRTNO = B.WRTNO ";
                SQL += ComNum.VBLF + " AND A.PANO = C.PANO ";
                SQL += ComNum.VBLF + " AND C.OP_JIPYO = 'Y' ";
                SQL += ComNum.VBLF + " AND A.OPDATE >= TO_DATE('" + argSDATE + "', 'YYYY-MM-DD') ";
                SQL += ComNum.VBLF + " AND A.OPDATE <= TO_DATE('" + argEDATE + "', 'YYYY-MM-DD') ";
                SQL += ComNum.VBLF + " AND A.TUYAKILSU > 0 ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                for (i = 0; i < dt.Rows.Count; i++)
                {

                  if(dt.Rows[i]["SUSUNGB"].ToString().Trim() == "1") // 수술예방적 항생제 투여 대상자
                    {
                        strWRTNO = dt.Rows[i]["WRTNO"].ToString().Trim();
                        nCnt1 += 1;

                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',1,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                        SQL = SQL + "  TO_DATE('" + dt.Rows[i]["CHULSANDATE"].ToString().Trim() + "','YYYY-MM-DD HH24:MI'),";
                        SQL = SQL + "  TO_DATE('" + dt.Rows[i]["SUYU_SIDO_TIME"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        SQL = " SELECT PANO, OPDATE, SUNEXT, SUNAMEG, TUYAKSDATE, TUYAKEDATE, NODRUG, TUYAKROUTE ";
                        SQL = SQL + ComNum.VBLF + " From NUR_JIPYO_ORAN_SUB ";
                        SQL = SQL + ComNum.VBLF + " WHERE WRTNO = '" + strWRTNO + "'";
                        SQL = SQL + ComNum.VBLF + " ORDER By TUYAKEDATE ASC ";

                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        if(dt2.Rows.Count > 0)
                        {
                            strTemp = dt.Rows[i]["OPSTIME"].ToString().Trim();
                            k = 0;

                            for (int j = 0; j < dt2.Rows.Count; j++)
                            {
                               if(DATE_TIME(strTemp, dt2.Rows[j]["TUYAKSDATE"].ToString().Trim()) <= 60 &&
                                  DATE_TIME(strTemp, dt2.Rows[j]["TUYAKSDATE"].ToString().Trim()) >= 0)
                                {
                                    k += 1;
                                }
                            }



                            if(k > 0 )
                            {
                                nCnt2 += 1;

                                SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                                SQL = SQL + ArgYYMM + "','" + strCODE + "',2,";
                                SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                                SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                                SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                                SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                                SQL = SQL + "  TO_DATE('" + argSDATE + "','YYYY-MM-DD HH24:MI'),";
                                SQL = SQL + "  TO_DATE('" + argEDATE + "','YYYY-MM-DD HH24:MI')  ) ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }

                            }
                        }

                        dt2.Dispose();
                        dt2 = null;


                        if(VB.Val(dt.Rows[i]["AVOID"].ToString().Trim()) > 0) //피해야 할 수술 감염 예방적 항생제 사용 건수 1건 이상일때
                        {
                            nCnt3 += 1;

                            SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                            SQL = SQL + ArgYYMM + "','" + strCODE + "',9,";
                            SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                            SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                            SQL = SQL + "  TO_DATE('" + argSDATE + "','YYYY-MM-DD HH24:MI'),";
                            SQL = SQL + "  TO_DATE('" + argEDATE + "','YYYY-MM-DD HH24:MI')  ) ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }

                    }
                    else if(dt.Rows[i]["SUSUNGB"].ToString().Trim() == "2") //수술예방적 항생제 투여 대상자
                    {
                        strWRTNO = dt.Rows[i]["WRTNO"].ToString().Trim();
                        nCnt4 += 1;


                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',10,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                        SQL = SQL + "  TO_DATE('" + argSDATE + "','YYYY-MM-DD HH24:MI'),";
                        SQL = SQL + "  TO_DATE('" + argEDATE + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }



                        SQL = " SELECT PANO, OPDATE, SUNEXT, SUNAMEG, TUYAKSDATE, TUYAKEDATE, NODRUG, TUYAKROUTE ";
                        SQL += ComNum.VBLF + " From NUR_JIPYO_ORAN_SUB ";
                        SQL += ComNum.VBLF + " WHERE WRTNO = '" + strWRTNO + "'";
                        SQL += ComNum.VBLF + " ORDER By TUYAKEDATE ASC ";

                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        if (dt2.Rows.Count > 0)
                        {
                            strTemp = dt.Rows[i]["OPSTIME"].ToString().Trim(); //수술시작시간
                            k = 0;

                            for (int j = 0; j < dt2.Rows.Count; j++)
                            {
                                //'수술 1시간 이내 예방적 항생제 투여한 건수
                                if (DATE_TIME(strTemp, dt2.Rows[j]["TUYAKSDATE"].ToString().Trim()) <= 60 &&
                                   DATE_TIME(strTemp, dt2.Rows[j]["TUYAKSDATE"].ToString().Trim()) >= 0)
                                {
                                    k += 1;
                                }
                            }


                            //'수술중 예방적 항생제 투여한 건수 1건 이상일때
                            if (k > 0)
                            {
                                nCnt5 += 1;

                                SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                                SQL = SQL + ArgYYMM + "','" + strCODE + "',11,";
                                SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                                SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                                SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                                SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                                SQL = SQL + "  TO_DATE('" + argSDATE + "','YYYY-MM-DD HH24:MI'),";
                                SQL = SQL + "  TO_DATE('" + argEDATE + "','YYYY-MM-DD HH24:MI')  ) ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }

                            }
                        }

                        dt2.Dispose();
                        dt2 = null;


                        //'피해야 할 수술 감염 예방적 항생제 사용 건수 1건 이상일때
                        if (VB.Val(dt.Rows[i]["AVOID"].ToString().Trim()) > 0)
                        {
                            nCnt6 += 1;

                            SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                            SQL = SQL + ArgYYMM + "','" + strCODE + "',18,";
                            SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                            SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                            SQL = SQL + "  TO_DATE('" + argSDATE + "','YYYY-MM-DD HH24:MI'),";
                            SQL = SQL + "  TO_DATE('" + argEDATE + "','YYYY-MM-DD HH24:MI')  ) ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }
                    }

                    else if (dt.Rows[i]["SUSUNGB"].ToString().Trim() == "3") //'수술예방적 항생제 투여 대상자
                    {
                        strWRTNO = dt.Rows[i]["WRTNO"].ToString().Trim();
                        nCnt7 += 1;


                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',19,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                        SQL = SQL + "  TO_DATE('" + argSDATE + "','YYYY-MM-DD HH24:MI'),";
                        SQL = SQL + "  TO_DATE('" + argEDATE + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }



                        SQL = " SELECT PANO, OPDATE, SUNEXT, SUNAMEG, TUYAKSDATE, TUYAKEDATE, NODRUG, TUYAKROUTE ";
                        SQL += ComNum.VBLF + " From NUR_JIPYO_ORAN_SUB ";
                        SQL += ComNum.VBLF + " WHERE WRTNO = '" + strWRTNO + "'";
                        SQL += ComNum.VBLF + " ORDER By TUYAKEDATE ASC ";

                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        if (dt2.Rows.Count > 0)
                        {
                            strTemp = dt.Rows[i]["OPSTIME"].ToString().Trim(); //'수술시작시간
                            k = 0;

                            for (int j = 0; j < dt2.Rows.Count; j++)
                            {
                                //'수술 1시간 이내 예방적 항생제 투여한 건수
                                if (DATE_TIME(strTemp, dt2.Rows[j]["TUYAKSDATE"].ToString().Trim()) <= 60 &&
                                   DATE_TIME(strTemp, dt2.Rows[j]["TUYAKSDATE"].ToString().Trim()) >= 0) 
                                {
                                    k += 1;
                                }
                            }


                            //'수술중 예방적 항생제 투여한 건수 1건 이상일때
                            if (k > 0)
                            {
                                nCnt8 += 1;

                                SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                                SQL = SQL + ArgYYMM + "','" + strCODE + "',20,";
                                SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                                SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                                SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                                SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                                SQL = SQL + "  TO_DATE('" + argSDATE + "','YYYY-MM-DD HH24:MI'),";
                                SQL = SQL + "  TO_DATE('" + argEDATE + "','YYYY-MM-DD HH24:MI')  ) ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }

                            }
                        }

                        dt2.Dispose();
                        dt2 = null;

                        //'피해야 할 수술 감염 예방적 항생제 사용 건수 1건 이상일때
                        if (VB.Val(dt.Rows[i]["AVOID"].ToString().Trim()) > 0)
                        {
                            nCnt9 += 1;

                            SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                            SQL = SQL + ArgYYMM + "','" + strCODE + "',27,";
                            SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                            SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                            SQL = SQL + "  TO_DATE('" + argSDATE + "','YYYY-MM-DD HH24:MI'),";
                            SQL = SQL + "  TO_DATE('" + argEDATE + "','YYYY-MM-DD HH24:MI')  ) ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }


                    }


                    else if (dt.Rows[i]["SUSUNGB"].ToString().Trim() == "4") //'수술예방적 항생제 투여 대상자
                    {
                        strWRTNO = dt.Rows[i]["WRTNO"].ToString().Trim();
                        nCnt10 += 1;


                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',28,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                        SQL = SQL + "  TO_DATE('" + argSDATE + "','YYYY-MM-DD HH24:MI'),";
                        SQL = SQL + "  TO_DATE('" + argEDATE + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }



                        SQL = " SELECT PANO, OPDATE, SUNEXT, SUNAMEG, TUYAKSDATE, TUYAKEDATE, NODRUG, TUYAKROUTE ";
                        SQL += ComNum.VBLF + " From NUR_JIPYO_ORAN_SUB ";
                        SQL += ComNum.VBLF + " WHERE WRTNO = '" + strWRTNO + "'";
                        SQL += ComNum.VBLF + " ORDER By TUYAKEDATE ASC ";

                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        if (dt2.Rows.Count > 0) //'수술시작시간
                        {
                            strTemp = dt.Rows[i]["OPSTIME"].ToString().Trim();
                            k = 0;

                            for (int j = 0; j < dt2.Rows.Count; j++)
                            {
                                //'수술 1시간 이내 예방적 항생제 투여한 건수
                                if (DATE_TIME(strTemp, dt2.Rows[j]["TUYAKSDATE"].ToString().Trim()) <= 60 &&
                                   DATE_TIME(strTemp, dt2.Rows[j]["TUYAKSDATE"].ToString().Trim()) >= 0)
                                {
                                    k += 1;
                                }
                            }


                            //'수술중 예방적 항생제 투여한 건수 1건 이상일때
                            if (k > 0)
                            {
                                nCnt11 += 1;

                                SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                                SQL = SQL + ArgYYMM + "','" + strCODE + "',29,";
                                SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                                SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                                SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                                SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                                SQL = SQL + "  TO_DATE('" + argSDATE + "','YYYY-MM-DD HH24:MI'),";
                                SQL = SQL + "  TO_DATE('" + argEDATE + "','YYYY-MM-DD HH24:MI')  ) ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }

                            }
                        }

                        dt2.Dispose();
                        dt2 = null;

                       // '피해야 할 수술 감염 예방적 항생제 사용 건수 1건 이상일때
                        if (VB.Val(dt.Rows[i]["AVOID"].ToString().Trim()) > 0)
                        {
                            nCnt12 += 1;

                            SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                            SQL = SQL + ArgYYMM + "','" + strCODE + "',36,";
                            SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                            SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                            SQL = SQL + "  TO_DATE('" + argSDATE + "','YYYY-MM-DD HH24:MI'),";
                            SQL = SQL + "  TO_DATE('" + argEDATE + "','YYYY-MM-DD HH24:MI')  ) ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }

                    }

                    else if (dt.Rows[i]["SUSUNGB"].ToString().Trim() == "5")
                    {
                        strWRTNO = dt.Rows[i]["WRTNO"].ToString().Trim();
                        nCnt13 += 1; //'수술예방적 항생제 투여 대상자


                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',37,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                        SQL = SQL + "  TO_DATE('" + argSDATE + "','YYYY-MM-DD HH24:MI'),";
                        SQL = SQL + "  TO_DATE('" + argEDATE + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }



                        SQL = " SELECT PANO, OPDATE, SUNEXT, SUNAMEG, TUYAKSDATE, TUYAKEDATE, NODRUG, TUYAKROUTE ";
                        SQL += ComNum.VBLF + " From NUR_JIPYO_ORAN_SUB ";
                        SQL += ComNum.VBLF + " WHERE WRTNO = '" + strWRTNO + "'";
                        SQL += ComNum.VBLF + " ORDER By TUYAKEDATE ASC ";

                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        if (dt2.Rows.Count > 0)
                        {
                            strTemp = dt.Rows[i]["OPSTIME"].ToString().Trim();
                            k = 0;

                            for (int j = 0; j < dt2.Rows.Count; j++)
                            {
                                if (DATE_TIME(strTemp, dt2.Rows[j]["TUYAKSDATE"].ToString().Trim()) <= 60 &&
                                   DATE_TIME(strTemp, dt2.Rows[j]["TUYAKSDATE"].ToString().Trim()) >= 0)
                                {
                                    k += 1;
                                }
                            }



                            if (k > 0)
                            {
                                nCnt14 += 1;

                                SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                                SQL = SQL + ArgYYMM + "','" + strCODE + "',38,";
                                SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                                SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                                SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                                SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                                SQL = SQL + "  TO_DATE('" + argSDATE + "','YYYY-MM-DD HH24:MI'),";
                                SQL = SQL + "  TO_DATE('" + argEDATE + "','YYYY-MM-DD HH24:MI')  ) ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }

                            }
                        }

                        dt2.Dispose();
                        dt2 = null;


                        if (VB.Val(dt.Rows[i]["AVOID"].ToString().Trim()) > 0)
                        {
                            nCnt15 += 1;

                            SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                            SQL = SQL + ArgYYMM + "','" + strCODE + "',45,";
                            SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                            SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                            SQL = SQL + "  TO_DATE('" + argSDATE + "','YYYY-MM-DD HH24:MI'),";
                            SQL = SQL + "  TO_DATE('" + argEDATE + "','YYYY-MM-DD HH24:MI')  ) ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }


                    }

                    else if (dt.Rows[i]["SUSUNGB"].ToString().Trim() == "6")
                    {
                        strWRTNO = dt.Rows[i]["WRTNO"].ToString().Trim();
                        nCnt16 += 1;


                        SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                        SQL = SQL + ArgYYMM + "','" + strCODE + "',46,";
                        SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                        SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                        SQL = SQL + "  TO_DATE('" + argSDATE + "','YYYY-MM-DD HH24:MI'),";
                        SQL = SQL + "  TO_DATE('" + argEDATE + "','YYYY-MM-DD HH24:MI')  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }



                        SQL = " SELECT PANO, OPDATE, SUNEXT, SUNAMEG, TUYAKSDATE, TUYAKEDATE, NODRUG, TUYAKROUTE ";
                        SQL += ComNum.VBLF + " From NUR_JIPYO_ORAN_SUB ";
                        SQL += ComNum.VBLF + " WHERE WRTNO = '" + strWRTNO + "'";
                        SQL += ComNum.VBLF + " ORDER By TUYAKEDATE ASC ";

                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        if (dt2.Rows.Count > 0)
                        {
                            strTemp = dt.Rows[i]["OPSTIME"].ToString().Trim();
                            k = 0;

                            for (int j = 0; j < dt2.Rows.Count; j++)
                            {
                                if (DATE_TIME(strTemp, dt2.Rows[j]["TUYAKSDATE"].ToString().Trim()) <= 60 &&
                                   DATE_TIME(strTemp, dt2.Rows[j]["TUYAKSDATE"].ToString().Trim()) >= 0)
                                {
                                    k += 1;
                                }
                            }



                            if (k > 0)
                            {
                                nCnt17 += 1;

                                SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                                SQL = SQL + ArgYYMM + "','" + strCODE + "',47,";
                                SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                                SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                                SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                                SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                                SQL = SQL + "  TO_DATE('" + argSDATE + "','YYYY-MM-DD HH24:MI'),";
                                SQL = SQL + "  TO_DATE('" + argEDATE + "','YYYY-MM-DD HH24:MI')  ) ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }

                            }
                        }

                        dt2.Dispose();
                        dt2 = null;


                        if (VB.Val(dt.Rows[i]["AVOID"].ToString().Trim()) > 0)
                        {
                            nCnt18 += 1;

                            SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                            SQL = SQL + ArgYYMM + "','" + strCODE + "',54,";
                            SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                            SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                            SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                            SQL = SQL + "  TO_DATE('" + argSDATE + "','YYYY-MM-DD HH24:MI'),";
                            SQL = SQL + "  TO_DATE('" + argEDATE + "','YYYY-MM-DD HH24:MI')  ) ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }
                    }
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',3," + nCnt3 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',4," + nCnt4 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',5," + nCnt5 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',6," + nCnt6 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',7," + nCnt7 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',8," + nCnt8 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',9," + nCnt9 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',10," + nCnt9 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',11," + nCnt9 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',12," + nCnt9 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',13," + nCnt9 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',14," + nCnt9 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',15," + nCnt9 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',16," + nCnt9 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',17," + nCnt9 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',18," + nCnt9 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// '임상질지표-병동 CONSULT 현황(의사)
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns>
        bool Build_92052_Process(string strCode, string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;

            int nTemp = 0;
            int nTempA = 0;

            string strRDate = "";
            string strCdate = "";


            string strCODE = strCode;
            string strGubun = "";

            string strTemp = "";
            List<string> strItem = new List<string>();
            int[] nCnt = new int[0];

            switch (strCode)
            {
                case "92052":
                    strGubun = "TDRCODE";
                    break;
                case "92053":
                    strGubun = "WARD";
                    break;
                case "92054":
                    strGubun = "TDEPT";
                    break;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_ITEM ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'아이템 생성 - 의사별
                SQL = " SELECT " + strGubun;
                SQL += ComNum.VBLF + " From NUR_CONSULT ";
                SQL += ComNum.VBLF + " WHERE BDATE >= TO_DATE('" + argSDATE + "', 'YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND BDATE <= TO_DATE('" + argEDATE + "', 'YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND RDATE IS NOT NULL "; //'결과 도착일 없는것 제외
                SQL += ComNum.VBLF + "   AND CDATE IS NOT NULL "; //'의뢰일 없는것 제외
                SQL += ComNum.VBLF + " GROUP BY " + strGubun;
                SQL += ComNum.VBLF + " ORDER BY " + strGubun;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    nTemp += 1;
                    strTemp = dt.Rows[i][strGubun].ToString().Trim();
                    strItem.Add(strTemp);

                    SQL = "INSERT INTO NUR_QI_ITEM (Code,Item,Title,Rank,GbDisplay,Remark,";
                    SQL = SQL + "\r" + " Bunmo,Bunja, YYMM) ";
                    SQL = SQL + "\r" + " VALUES ('" + strCODE + "'," + nTemp + ",'" + clsVbfunc.GetBASDoctorName(clsDB.DbCon, strTemp) + "',";
                    SQL = SQL + "\r" + nTemp + ",'C','','N','N', '" + ArgYYMM + "') ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    nTemp += 1;
                    SQL = "INSERT INTO NUR_QI_ITEM (Code,Item,Title,Rank,GbDisplay,Remark,";
                    SQL = SQL + " Bunmo,Bunja, YYMM) ";
                    SQL = SQL + " VALUES ('" + strCODE + "'," + nTemp + ",'" + "└> 당일이내(중환자실)" + "',";
                    SQL = SQL + nTemp + ",'C','','N','N', '" + ArgYYMM + "') ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    nTemp += 1;
                    SQL = "INSERT INTO NUR_QI_ITEM (Code,Item,Title,Rank,GbDisplay,Remark,";
                    SQL = SQL + " Bunmo,Bunja, YYMM) ";
                    SQL = SQL + " VALUES ('" + strCODE + "'," + nTemp + ",'" + "└> 당일이후(중환자실)" + "',";
                    SQL = SQL + nTemp + ",'C','','N','N', '" + ArgYYMM + "') ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    nTemp += 1;
                    SQL = "INSERT INTO NUR_QI_ITEM (Code,Item,Title,Rank,GbDisplay,Remark,";
                    SQL = SQL + " Bunmo,Bunja, YYMM) ";
                    SQL = SQL + " VALUES ('" + strCODE + "'," + nTemp + ",'" + clsVbfunc.GetBASDoctorName(clsDB.DbCon, strTemp) + "(일반)',";
                    SQL = SQL + nTemp + ",'C','','N','N', '" + ArgYYMM + "') ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }


                    nTemp += 1;
                    SQL = "INSERT INTO NUR_QI_ITEM (Code,Item,Title,Rank,GbDisplay,Remark,";
                    SQL = SQL + " Bunmo,Bunja, YYMM) ";
                    SQL = SQL + " VALUES ('" + strCODE + "'," + nTemp + ",'" + "└> 24시간 이내" + "',";
                    SQL = SQL + nTemp + ",'C','','N','N', '" + ArgYYMM + "') ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    nTemp += 1;
                    SQL = "INSERT INTO NUR_QI_ITEM (Code,Item,Title,Rank,GbDisplay,Remark,";
                    SQL = SQL + " Bunmo,Bunja, YYMM) ";
                    SQL = SQL + " VALUES ('" + strCODE + "'," + nTemp + ",'" + "└> 48시간 이내" + "',";
                    SQL = SQL + nTemp + ",'C','','N','N', '" + ArgYYMM + "') ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    nTemp += 1;
                    SQL = "INSERT INTO NUR_QI_ITEM (Code,Item,Title,Rank,GbDisplay,Remark,";
                    SQL = SQL + " Bunmo,Bunja, YYMM) ";
                    SQL = SQL + " VALUES ('" + strCODE + "'," + nTemp + ",'" + "└> 72시간 이내" + "',";
                    SQL = SQL + nTemp + ",'C','','N','N', '" + ArgYYMM + "') ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    nTemp += 1;
                    SQL = "INSERT INTO NUR_QI_ITEM (Code,Item,Title,Rank,GbDisplay,Remark,";
                    SQL = SQL + " Bunmo,Bunja, YYMM) ";
                    SQL = SQL + " VALUES ('" + strCODE + "'," + nTemp + ",'" + "└> 72시간 이후" + "',";
                    SQL = SQL + nTemp + ",'C','','N','N', '" + ArgYYMM + "') ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }


                Array.Resize(ref nCnt, nTemp);

                for (int i = 0; i < strItem.Count; i++)
                {

                    SQL = " SELECT B.PANO, B.SNAME, B.SEX, A.TDEPT DEPTCODE,  A.WARD WARDCODE, B.ROOMCODE, A.TDRCODE DRCODE, ";
                    SQL += ComNum.VBLF + " TO_CHAR(A.RDATE, 'YYYY-MM-DD HH24:MI') RDATE, TO_CHAR(A.CDATE, 'YYYY-MM-DD HH24:MI') CDATE ";
                    SQL += ComNum.VBLF + "  FROM NUR_CONSULT A, IPD_NEW_MASTER B ";
                    SQL += ComNum.VBLF + " WHERE A.BDATE >= TO_DATE('" + argSDATE + "', 'YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "   AND A.BDATE <= TO_DATE('" + argEDATE + "', 'YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "   AND A.TDRCODE = '" + strItem[i] + "' ";
                    SQL += ComNum.VBLF + "   AND B.GBSTS <> '9' ";
                    SQL += ComNum.VBLF + "   AND TRUNC(B.INDATE) <= A.BDATE ";
                    SQL += ComNum.VBLF + "   AND (B.OUTDATE IS NULL OR TRUNC(B.OUTDATE) >= A.BDATE) ";
                    SQL += ComNum.VBLF + "   AND A.PANO = B.PANO ";
                    SQL += ComNum.VBLF + "   AND A.RDATE IS NOT NULL ";// '결과 도착일 없는것 제외
                    SQL += ComNum.VBLF + "   AND A.CDATE IS NOT NULL ";// '의뢰일 없는것 제외
                    SQL += ComNum.VBLF + "   ORDER BY A.PANO ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        strRDate = dt.Rows[j]["RDATE"].ToString().Trim();
                        strCdate = dt.Rows[j]["CDATE"].ToString().Trim();

                        switch (dt.Rows[j]["WARDCODE"].ToString())
                        {
                            case "SICU":
                            case "MICU":

                                nCnt[nTempA] += 1;


                                SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                                SQL = SQL + ArgYYMM + "','" + strCODE + "'," + nTempA + 1 + ",";
                                SQL = SQL + " '" + dt.Rows[j]["PANO"].ToString().Trim() + "','" + dt.Rows[j]["SNAME"].ToString().Trim() + "', ";
                                SQL = SQL + " '" + dt.Rows[j]["SEX"].ToString().Trim() + "','" + dt.Rows[j]["DEPTCODE"].ToString().Trim() + "', ";
                                SQL = SQL + " '" + dt.Rows[j]["WARDCODE"].ToString().Trim() + "'," + dt.Rows[j]["ROOMCODE"].ToString().Trim() + ",";
                                SQL = SQL + " '" + dt.Rows[j]["DRCODE"].ToString().Trim() + "', ";
                                SQL = SQL + "  TO_DATE('" + strRDate + "','YYYY-MM-DD HH24:MI'),";
                                SQL = SQL + "  TO_DATE('" + strCdate + "','YYYY-MM-DD HH24:MI')  ) ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }

                                if (VB.Left(strCdate, 10) == VB.Left(strRDate, 10))
                                {

                                    nCnt[nTempA + 1] += 1;
                                    SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                                    SQL = SQL + ArgYYMM + "','" + strCODE + "'," + nTempA + 2 + ",";
                                    SQL = SQL + " '" + dt.Rows[j]["PANO"].ToString().Trim() + "','" + dt.Rows[j]["SNAME"].ToString().Trim() + "', ";
                                    SQL = SQL + " '" + dt.Rows[j]["SEX"].ToString().Trim() + "','" + dt.Rows[j]["DEPTCODE"].ToString().Trim() + "', ";
                                    SQL = SQL + " '" + dt.Rows[j]["WARDCODE"].ToString().Trim() + "'," + dt.Rows[j]["ROOMCODE"].ToString().Trim() + ",";
                                    SQL = SQL + " '" + dt.Rows[j]["DRCODE"].ToString().Trim() + "', ";
                                    SQL = SQL + "  TO_DATE('" + strRDate + "','YYYY-MM-DD HH24:MI'),";
                                    SQL = SQL + "  TO_DATE('" + strCdate + "','YYYY-MM-DD HH24:MI')  ) ";

                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        ComFunc.MsgBox(SqlErr);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        Cursor.Current = Cursors.Default;
                                        return rtnVal;
                                    }
                                }
                                else
                                {
                                    nCnt[nTempA + 2] += 1;
                                    SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                                    SQL = SQL + ArgYYMM + "','" + strCODE + "'," + nTempA + 3 + ",";
                                    SQL = SQL + " '" + dt.Rows[j]["PANO"].ToString().Trim() + "','" + dt.Rows[j]["SNAME"].ToString().Trim() + "', ";
                                    SQL = SQL + " '" + dt.Rows[j]["SEX"].ToString().Trim() + "','" + dt.Rows[j]["DEPTCODE"].ToString().Trim() + "', ";
                                    SQL = SQL + " '" + dt.Rows[j]["WARDCODE"].ToString().Trim() + "'," + dt.Rows[j]["ROOMCODE"].ToString().Trim() + ",";
                                    SQL = SQL + " '" + dt.Rows[j]["DRCODE"].ToString().Trim() + "', ";
                                    SQL = SQL + "  TO_DATE('" + strRDate + "','YYYY-MM-DD HH24:MI'),";
                                    SQL = SQL + "  TO_DATE('" + strCdate + "','YYYY-MM-DD HH24:MI')  ) ";

                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        ComFunc.MsgBox(SqlErr);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        Cursor.Current = Cursors.Default;
                                        return rtnVal;
                                    }
                                }
                                break;
                            default:

                                nCnt[nTempA + 3] += 1;

                                SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                                SQL = SQL + ArgYYMM + "','" + strCODE + "'," + nTempA + 4 + ",";
                                SQL = SQL + " '" + dt.Rows[j]["PANO"].ToString().Trim() + "','" + dt.Rows[j]["SNAME"].ToString().Trim() + "', ";
                                SQL = SQL + " '" + dt.Rows[j]["SEX"].ToString().Trim() + "','" + dt.Rows[j]["DEPTCODE"].ToString().Trim() + "', ";
                                SQL = SQL + " '" + dt.Rows[j]["WARDCODE"].ToString().Trim() + "'," + dt.Rows[j]["ROOMCODE"].ToString().Trim() + ",";
                                SQL = SQL + " '" + dt.Rows[j]["DRCODE"].ToString().Trim() + "', ";
                                SQL = SQL + "  TO_DATE('" + strRDate + "','YYYY-MM-DD HH24:MI'),";
                                SQL = SQL + "  TO_DATE('" + strCdate + "','YYYY-MM-DD HH24:MI')  ) ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }


                                nTemp = DATE_TIME(strCdate, strRDate);
                                if (nTemp < 1440)
                                {
                                    nCnt[nTempA + 4] += 1;
                                    SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                                    SQL = SQL + ArgYYMM + "','" + strCODE + "'," + nTempA + 5 + ",";
                                    SQL = SQL + " '" + dt.Rows[j]["PANO"].ToString().Trim() + "','" + dt.Rows[j]["SNAME"].ToString().Trim() + "', ";
                                    SQL = SQL + " '" + dt.Rows[j]["SEX"].ToString().Trim() + "','" + dt.Rows[j]["DEPTCODE"].ToString().Trim() + "', ";
                                    SQL = SQL + " '" + dt.Rows[j]["WARDCODE"].ToString().Trim() + "'," + dt.Rows[j]["ROOMCODE"].ToString().Trim() + ",";
                                    SQL = SQL + " '" + dt.Rows[j]["DRCODE"].ToString().Trim() + "', ";
                                    SQL = SQL + "  TO_DATE('" + strRDate + "','YYYY-MM-DD HH24:MI'),";
                                    SQL = SQL + "  TO_DATE('" + strCdate + "','YYYY-MM-DD HH24:MI')  ) ";

                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        ComFunc.MsgBox(SqlErr);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        Cursor.Current = Cursors.Default;
                                        return rtnVal;
                                    }
                                }
                                else if (nTemp < 2880)
                                {
                                    nCnt[nTempA + 5] += 1;
                                    SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                                    SQL = SQL + ArgYYMM + "','" + strCODE + "'," + nTempA + 6 + ",";
                                    SQL = SQL + " '" + dt.Rows[j]["PANO"].ToString().Trim() + "','" + dt.Rows[j]["SNAME"].ToString().Trim() + "', ";
                                    SQL = SQL + " '" + dt.Rows[j]["SEX"].ToString().Trim() + "','" + dt.Rows[j]["DEPTCODE"].ToString().Trim() + "', ";
                                    SQL = SQL + " '" + dt.Rows[j]["WARDCODE"].ToString().Trim() + "'," + dt.Rows[j]["ROOMCODE"].ToString().Trim() + ",";
                                    SQL = SQL + " '" + dt.Rows[j]["DRCODE"].ToString().Trim() + "', ";
                                    SQL = SQL + "  TO_DATE('" + strRDate + "','YYYY-MM-DD HH24:MI'),";
                                    SQL = SQL + "  TO_DATE('" + strCdate + "','YYYY-MM-DD HH24:MI')  ) ";

                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        ComFunc.MsgBox(SqlErr);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        Cursor.Current = Cursors.Default;
                                        return rtnVal;
                                    }
                                }

                                else if (nTemp < 4320)
                                {
                                    nCnt[nTempA + 6] += 1;
                                    SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                                    SQL = SQL + ArgYYMM + "','" + strCODE + "'," + nTempA + 7 + ",";
                                    SQL = SQL + " '" + dt.Rows[j]["PANO"].ToString().Trim() + "','" + dt.Rows[j]["SNAME"].ToString().Trim() + "', ";
                                    SQL = SQL + " '" + dt.Rows[j]["SEX"].ToString().Trim() + "','" + dt.Rows[j]["DEPTCODE"].ToString().Trim() + "', ";
                                    SQL = SQL + " '" + dt.Rows[j]["WARDCODE"].ToString().Trim() + "'," + dt.Rows[j]["ROOMCODE"].ToString().Trim() + ",";
                                    SQL = SQL + " '" + dt.Rows[j]["DRCODE"].ToString().Trim() + "', ";
                                    SQL = SQL + "  TO_DATE('" + strRDate + "','YYYY-MM-DD HH24:MI'),";
                                    SQL = SQL + "  TO_DATE('" + strCdate + "','YYYY-MM-DD HH24:MI')  ) ";

                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        ComFunc.MsgBox(SqlErr);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        Cursor.Current = Cursors.Default;
                                        return rtnVal;
                                    }
                                }
                                else if (nTemp >= 4320)
                                {
                                    nCnt[nTempA + 7] += 1;
                                    SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                                    SQL = SQL + ArgYYMM + "','" + strCODE + "'," + nTempA + 8 + ",";
                                    SQL = SQL + " '" + dt.Rows[j]["PANO"].ToString().Trim() + "','" + dt.Rows[j]["SNAME"].ToString().Trim() + "', ";
                                    SQL = SQL + " '" + dt.Rows[j]["SEX"].ToString().Trim() + "','" + dt.Rows[j]["DEPTCODE"].ToString().Trim() + "', ";
                                    SQL = SQL + " '" + dt.Rows[j]["WARDCODE"].ToString().Trim() + "'," + dt.Rows[j]["ROOMCODE"].ToString().Trim() + ",";
                                    SQL = SQL + " '" + dt.Rows[j]["DRCODE"].ToString().Trim() + "', ";
                                    SQL = SQL + "  TO_DATE('" + strRDate + "','YYYY-MM-DD HH24:MI'),";
                                    SQL = SQL + "  TO_DATE('" + strCdate + "','YYYY-MM-DD HH24:MI')  ) ";

                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        ComFunc.MsgBox(SqlErr);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        Cursor.Current = Cursors.Default;
                                        return rtnVal;
                                    }
                                }

                                break;
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    nTempA += 8;
                }

                for (int i = 0; i < nTempA; i++)
                {
                    SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                    SQL = SQL + ArgYYMM + "','" + strCODE + "'," + i + 1 + "," + nCnt[i] + ") ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }


                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 욕창 위험대상
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns>
        bool Build_51001_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;

            int i = 0;
            string strCODE = "51001";
            long nCnt1 = 0;
            long nCnt2 = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                #region  이전 쿼리
                //'욕창위험 대상군
                SQL = " SELECT A.PANO, A.SNAME, A.SEX, A.DEPTCODE, B.WARDCODE, A.ROOMCODE, B.DRCODE, TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE, TO_CHAR(A.ACTDATE, 'YYYY-MM-DD') ENTDATE, A.AGE, A.TOTAL, B.IPDNO ";
                SQL += ComNum.VBLF + " FROM ADMIN.NUR_BRADEN_SCALE A, ADMIN.IPD_NEW_MASTER B, ADMIN.NUR_BRADEN_SCALE_DETAIL C";
                SQL += ComNum.VBLF + " WHERE A.ACTDATE >= TO_DATE('" + argSDATE + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "   AND A.ACTDATE <= TO_DATE('" + argEDATE + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "   AND A.IPDNO = B.IPDNO";
                SQL += ComNum.VBLF + "   AND A.PANO = B.PANO";
                SQL += ComNum.VBLF + "   AND A.IPDNO = C.IPDNO(+)";
                SQL += ComNum.VBLF + "   AND ((A.AGE >= 60 AND TOTAL <= 18)";
                SQL += ComNum.VBLF + "    OR (A.AGE < 60 AND TOTAL <16)";
                SQL += ComNum.VBLF + "    OR C.BUN1 = '1' ";
                SQL += ComNum.VBLF + "    OR C.BUN2 = '1' ";
                SQL += ComNum.VBLF + "    OR C.BUN3 = '1' ";
                SQL += ComNum.VBLF + "    OR C.BUN4 = '1' ";
                SQL += ComNum.VBLF + "    OR C.BUN5 = '1' ";
                SQL += ComNum.VBLF + "    OR C.BUN6 = '1' ";
                SQL += ComNum.VBLF + "    OR C.BUN7 = '1'";
                SQL += ComNum.VBLF + "    OR C.YOKGBN = '1') ";
                SQL += ComNum.VBLF + " Union All";
                SQL += ComNum.VBLF + "  SELECT A.PANO, A.SNAME, A.SEX, A.DEPTCODE, B.WARDCODE, A.ROOMCODE, B.DRCODE, TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE, TO_CHAR(A.ACTDATE, 'YYYY-MM-DD') ENTDATE, A.AGE, A.TOTAL, B.IPDNO";
                SQL += ComNum.VBLF + "  FROM ADMIN.NUR_BRADEN_SCALE_CHILD A, ADMIN.IPD_NEW_MASTER B";
                SQL += ComNum.VBLF + " WHERE A.ACTDATE >= TO_DATE('" + argSDATE + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "   AND A.ACTDATE <= TO_DATE('" + argEDATE + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "    AND A.IPDNO = B.IPDNO";
                SQL += ComNum.VBLF + "    AND A.PANO = B.PANO";
                SQL += ComNum.VBLF + "    AND A.TOTAL <= 16 ";
                SQL += ComNum.VBLF + " Union All";
                SQL += ComNum.VBLF + "  SELECT A.PANO, A.SNAME, A.SEX, A.DEPTCODE, B.WARDCODE, A.ROOMCODE, B.DRCODE, TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE, TO_CHAR(A.ACTDATE, 'YYYY-MM-DD') ENTDATE, A.AGE, A.TOTAL, B.IPDNO";
                SQL += ComNum.VBLF + "  FROM ADMIN.NUR_BRADEN_SCALE_BABY A, ADMIN.IPD_NEW_MASTER B";
                SQL += ComNum.VBLF + " WHERE A.ACTDATE >= TO_DATE('" + argSDATE + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "   AND A.ACTDATE <= TO_DATE('" + argEDATE + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "    AND A.IPDNO = B.IPDNO";
                SQL += ComNum.VBLF + "    AND A.PANO = B.PANO";
                SQL += ComNum.VBLF + "    AND A.TOTAL <= 20";
                #endregion

                //'욕창위험 2019-09-03
                SQL = " SELECT A.PANO, A.SNAME, A.SEX, A.DEPTCODE, B.WARDCODE, A.ROOMCODE, B.DRCODE, TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE, TO_CHAR(A.ACTDATE, 'YYYY-MM-DD') ENTDATE, A.AGE, A.TOTAL, B.IPDNO ";
                SQL += ComNum.VBLF + " FROM ADMIN.NUR_BRADEN_SCALE A, ADMIN.IPD_NEW_MASTER B ";
                SQL += ComNum.VBLF + " WHERE A.ACTDATE >= TO_DATE('" + argSDATE + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "   AND A.ACTDATE <= TO_DATE('" + argEDATE + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "   AND A.IPDNO = B.IPDNO";
                SQL += ComNum.VBLF + "   AND A.PANO = B.PANO";
                SQL += ComNum.VBLF + "   AND ADMIN.FC_READ_WARNING_BRADEN(A.IPDNO) > 0 ";
                SQL += ComNum.VBLF + " Union All";
                SQL += ComNum.VBLF + "  SELECT A.PANO, A.SNAME, A.SEX, A.DEPTCODE, B.WARDCODE, A.ROOMCODE, B.DRCODE, TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE, TO_CHAR(A.ACTDATE, 'YYYY-MM-DD') ENTDATE, A.AGE, A.TOTAL, B.IPDNO";
                SQL += ComNum.VBLF + "  FROM ADMIN.NUR_BRADEN_SCALE_CHILD A, ADMIN.IPD_NEW_MASTER B";
                SQL += ComNum.VBLF + " WHERE A.ACTDATE >= TO_DATE('" + argSDATE + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "   AND A.ACTDATE <= TO_DATE('" + argEDATE + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "    AND A.IPDNO = B.IPDNO";
                SQL += ComNum.VBLF + "    AND A.PANO = B.PANO";
                SQL += ComNum.VBLF + "    AND ADMIN.FC_READ_WARNING_BRADEN(A.IPDNO) > 0 ";
                SQL += ComNum.VBLF + " Union All";
                SQL += ComNum.VBLF + "  SELECT A.PANO, A.SNAME, A.SEX, A.DEPTCODE, B.WARDCODE, A.ROOMCODE, B.DRCODE, TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE, TO_CHAR(A.ACTDATE, 'YYYY-MM-DD') ENTDATE, A.AGE, A.TOTAL, B.IPDNO";
                SQL += ComNum.VBLF + "  FROM ADMIN.NUR_BRADEN_SCALE_BABY A, ADMIN.IPD_NEW_MASTER B";
                SQL += ComNum.VBLF + " WHERE A.ACTDATE >= TO_DATE('" + argSDATE + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "   AND A.ACTDATE <= TO_DATE('" + argEDATE + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "    AND A.IPDNO = B.IPDNO";
                SQL += ComNum.VBLF + "    AND A.PANO = B.PANO";
                SQL += ComNum.VBLF + "    AND ADMIN.FC_READ_WARNING_BRADEN(A.IPDNO) > 0 ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for(i = 0; i < dt.Rows.Count; i++)
                {
                    nCnt1 = nCnt1 + 1;
                    SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                    SQL = SQL + ArgYYMM + "','" + strCODE + "',1,";
                    SQL = SQL + "\r" + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                    SQL = SQL + "\r" + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                    SQL = SQL + "\r" + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "'," + dt.Rows[i]["ROOMCODE"].ToString().Trim() + ",";
                    SQL = SQL + "\r" + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                    SQL = SQL + "\r" + "  TO_DATE('" + dt.Rows[i]["ACTDATE"].ToString().Trim() + "','YYYY-MM-DD'),";
                    SQL = SQL + "\r" + "  TO_DATE('" + dt.Rows[i]["ENTDATE"].ToString().Trim() + "','YYYY-MM-DD')  ) ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }

                dt.Dispose();
                dt = null;


                // '욕창보고서 작성 건수
                SQL = " SELECT PANO, SNAME, SEX, DEPTCODE, WARDCODE, ROOMCODE, DRCODE, TIME1, TIME2 FROM ( ";
                SQL += ComNum.VBLF + " SELECT A.PANO, A.SNAME, A.SEX, A.DEPTCODE, A.WARDCODE, A.ROOMCODE, B.DRCODE, TO_CHAR(B.INDATE,'YYYY-MM-DD') TIME1, TO_CHAR(A.ACTDATE,'YYYY-MM-DD') TIME2"    ;
                SQL += ComNum.VBLF + " FROM ADMIN.NUR_PRESSURE_SORE A, ADMIN.IPD_NEW_MASTER B"                                                                             ;
                SQL += ComNum.VBLF + " WHERE A.ACTDATE >= TO_DATE('" + argSDATE + "','YYYY-MM-DD')"                                                                                    ;
                SQL += ComNum.VBLF + "   AND A.ACTDATE <= TO_DATE('" + argEDATE + "','YYYY-MM-DD')"                                                                                    ;
                SQL += ComNum.VBLF + " AND A.IPDNO = B.IPDNO"                                                                                                                          ;
                SQL += ComNum.VBLF + " AND A.PANO = B.PANO"                                                                                                                            ;
                SQL += ComNum.VBLF + " AND A.DELDATE IS NULL"                                                                                                                          ;
                SQL += ComNum.VBLF + " GROUP BY A.PANO, A.SNAME, A.SEX, A.DEPTCODE, A.WARDCODE, A.ROOMCODE, B.DRCODE, TO_CHAR(B.INDATE,'YYYY-MM-DD'), TO_CHAR(A.ACTDATE,'YYYY-MM-DD')";
                SQL += ComNum.VBLF + " UNION ALL ";
                SQL += ComNum.VBLF + " SELECT A.PANO, B.SNAME, B.SEX, A.DEPTCODE, 'OPD' WARDCODE, A.ROOMCODE, B.DRCODE, TO_CHAR(B.BDATE, 'YYYY-MM-DD') TIME1, TO_CHAR(A.ACTDATE, 'YYYY-MM-DD') TIME2 ";
                SQL += ComNum.VBLF + "   FROM ADMIN.NUR_PRESSURE_SORE A, ADMIN.OPD_MASTER B ";
                SQL += ComNum.VBLF + "  WHERE A.ACTDATE >= TO_DATE('" + argSDATE + " 00:00', 'YYYY-MM-DD HH24:MI') ";
                SQL += ComNum.VBLF + "    AND A.ACTDATE <= TO_DATE('" + argEDATE + " 23:59', 'YYYY-MM-DD HH24:MI') ";
                SQL += ComNum.VBLF + "    AND A.PANO = B.PANO ";
                SQL += ComNum.VBLF + "    AND A.DEPTCODE = B.DEPTCODE ";
                SQL += ComNum.VBLF + "    AND TRUNC(A.ACTDATE) = B.BDATE ";
                SQL += ComNum.VBLF + "    AND A.IPDNO = 0 ";
                SQL += ComNum.VBLF + "  GROUP BY A.PANO, B.SNAME, B.SEX, A.DEPTCODE, 'OPD', A.ROOMCODE, B.DRCODE, TO_CHAR(B.BDATE, 'YYYY-MM-DD'), TO_CHAR(A.ACTDATE, 'YYYY-MM-DD') ";
                SQL += ComNum.VBLF + " ) GROUP BY PANO, SNAME, SEX, DEPTCODE, WARDCODE, ROOMCODE, DRCODE, TIME1, TIME2 ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nCnt2 = nCnt2 + 1;
                    SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                    SQL = SQL + ArgYYMM + "','" + strCODE + "',2,";
                    SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                    SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                    SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                    SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                    SQL = SQL + "  TO_DATE('" + dt.Rows[i]["TIME1"].ToString().Trim() + "','YYYY-MM-DD'),";
                    SQL = SQL + "  TO_DATE('" + dt.Rows[i]["TIME2"].ToString().Trim() + "','YYYY-MM-DD')  ) ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }

                //'욕창대상자
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'욕창보고서
                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 낙상 위험 대상
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns>
        bool Build_51002_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;

            int i = 0;
            string strCODE = "51002";
            long nCnt1 = 0;
            long nCnt2 = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                #region 이전 쿼리
                SQL = " SELECT PANO, SNAME, SEX, DEPTCODE, WARDCODE, ROOMCODE, DRCODE, ACTDATE, ENTDATE, AGE"                                                                     ;
                SQL += ComNum.VBLF + " FROM ("                                                                                                                                      ;
                SQL += ComNum.VBLF + " SELECT A.PANO, A.SNAME, A.SEX, A.DEPTCODE, B.WARDCODE, A.ROOMCODE, B.DRCODE, TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE, TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ENTDATE, A.AGE";
                SQL += ComNum.VBLF + " FROM ADMIN.NUR_FALLMORSE_SCALE A, ADMIN.IPD_NEW_MASTER B, ADMIN.NUR_FALLMORSE_CAUSE C"                                     ;
                SQL += ComNum.VBLF + " WHERE A.ACTDATE >= TO_DATE('" + argSDATE + "','YYYY-MM-DD')"                                                                                 ;
                SQL += ComNum.VBLF + "   AND A.ACTDATE <= TO_DATE('" + argEDATE + "','YYYY-MM-DD')"                                                                                 ;
                SQL += ComNum.VBLF + "   AND A.IPDNO = B.IPDNO"                                                                                                                     ;
                SQL += ComNum.VBLF + "   AND A.PANO = B.PANO"                                                                                                                       ;
                SQL += ComNum.VBLF + "   AND A.IPDNO = C.IPDNO(+)"                                                                                                                  ;
                SQL += ComNum.VBLF + "   AND (TOTAL >= 51"                                                                                                                          ;
                SQL += ComNum.VBLF + "    OR C.CAUSE1 = '1' "                                                                                                                       ;
                SQL += ComNum.VBLF + "    OR C.CAUSE2 = '1' "                                                                                                                       ;
                SQL += ComNum.VBLF + "    OR C.CAUSE3 = '1' "                                                                                                                       ;
                SQL += ComNum.VBLF + "    OR C.CAUSE4 = '1' "                                                                                                                       ;
                SQL += ComNum.VBLF + "    OR C.CAUSE5 = '1' "                                                                                                                       ;
                SQL += ComNum.VBLF + "    OR C.CAUSE6 = '1' "                                                                                                                       ;
                SQL += ComNum.VBLF + "    OR C.CAUSE7 = '1' "                                                                                                                       ;
                SQL += ComNum.VBLF + "    OR C.CAUSE8 = '1' "                                                                                                                       ;
                SQL += ComNum.VBLF + "    OR C.CAUSE9 = '1' "                                                                                                                       ;
                SQL += ComNum.VBLF + "    OR C.CAUSE10 = '1' "                                                                                                                      ;
                SQL += ComNum.VBLF + "    OR C.CAUSE11 = '1' "                                                                                                                      ;
                SQL += ComNum.VBLF + "    OR C.EXAM1 = '1' "                                                                                                                        ;
                SQL += ComNum.VBLF + "    OR C.EXAM2 = '1' "                                                                                                                        ;
                SQL += ComNum.VBLF + "    OR C.EXAM3 = '1' "                                                                                                                        ;
                SQL += ComNum.VBLF + "    OR C.EXAM4 = '1' "                                                                                                                        ;
                SQL += ComNum.VBLF + "    OR C.CAUSEETC <> '') "                                                                                                                    ;
                SQL += ComNum.VBLF + " GROUP BY A.PANO, A.SNAME, A.SEX, A.DEPTCODE, B.WARDCODE, A.ROOMCODE, B.DRCODE, TO_CHAR(A.ACTDATE,'YYYY-MM-DD'), TO_CHAR(A.ACTDATE,'YYYY-MM-DD'), A.AGE";
                SQL += ComNum.VBLF + " UNION ALL "                                                                                                                                  ;
                SQL += ComNum.VBLF + " SELECT A.PANO, A.SNAME, A.SEX, A.DEPTCODE, B.WARDCODE, A.ROOMCODE, B.DRCODE, TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE, TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ENTDATE, A.AGE";
                SQL += ComNum.VBLF + " FROM ADMIN.NUR_FALLHUMPDUMP_SCALE A, ADMIN.IPD_NEW_MASTER B, ADMIN.NUR_FALLHUMPDUMP_CAUSE C"                               ;
                SQL += ComNum.VBLF + " WHERE A.ACTDATE >= TO_DATE('" + argSDATE + "','YYYY-MM-DD')"                                                                                 ;
                SQL += ComNum.VBLF + "   AND A.ACTDATE <= TO_DATE('" + argEDATE + "','YYYY-MM-DD')"                                                                                 ;
                SQL += ComNum.VBLF + "   AND A.IPDNO = B.IPDNO"                                                                                                                     ;
                SQL += ComNum.VBLF + "   AND A.PANO = B.PANO"                                                                                                                       ;
                SQL += ComNum.VBLF + "   AND A.IPDNO = C.IPDNO(+)"                                                                                                                  ;
                SQL += ComNum.VBLF + "   AND (TOTAL >= 12"                                                                                                                          ;
                SQL += ComNum.VBLF + "    OR C.EXAM1 = '1' "                                                                                                                        ;
                SQL += ComNum.VBLF + "    OR C.EXAM2 = '1' "                                                                                                                        ;
                SQL += ComNum.VBLF + "    OR C.EXAM3 = '1' "                                                                                                                        ;
                SQL += ComNum.VBLF + "    OR C.EXAM4 = '1') "                                                                                                                       ;
                SQL += ComNum.VBLF + " GROUP BY A.PANO, A.SNAME, A.SEX, A.DEPTCODE, B.WARDCODE, A.ROOMCODE, B.DRCODE, TO_CHAR(A.ACTDATE,'YYYY-MM-DD'), TO_CHAR(A.ACTDATE,'YYYY-MM-DD'), A.AGE";
                SQL += ComNum.VBLF + ") "                                                                                                                                           ;
                SQL += ComNum.VBLF + " GROUP BY PANO, SNAME, SEX, DEPTCODE, WARDCODE, ROOMCODE, DRCODE, ACTDATE, ENTDATE, AGE";
                #endregion

                //2019-09-03
                SQL = " SELECT PANO, SNAME, SEX, DEPTCODE, WARDCODE, ROOMCODE, DRCODE, ACTDATE, ENTDATE, AGE";
                SQL += ComNum.VBLF + " FROM (";
                SQL += ComNum.VBLF + " SELECT A.PANO, A.SNAME, A.SEX, A.DEPTCODE, B.WARDCODE, A.ROOMCODE, B.DRCODE, TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE, TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ENTDATE, A.AGE";
                SQL += ComNum.VBLF + " FROM ADMIN.NUR_FALLMORSE_SCALE A, ADMIN.IPD_NEW_MASTER B ";
                SQL += ComNum.VBLF + " WHERE A.ACTDATE >= TO_DATE('" + argSDATE + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "   AND A.ACTDATE <= TO_DATE('" + argEDATE + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "   AND A.IPDNO = B.IPDNO";
                SQL += ComNum.VBLF + "   AND A.PANO = B.PANO";
                SQL += ComNum.VBLF + "   AND A.IPDNO > 0 ";
                SQL += ComNum.VBLF + "   AND ADMIN.FC_NUR_FALL_REPORT_CHK(A.PANO, A.ACTDATE) = 'Y' ";
                SQL += ComNum.VBLF + " GROUP BY A.PANO, A.SNAME, A.SEX, A.DEPTCODE, B.WARDCODE, A.ROOMCODE, B.DRCODE, TO_CHAR(A.ACTDATE,'YYYY-MM-DD'), TO_CHAR(A.ACTDATE,'YYYY-MM-DD'), A.AGE";
                SQL += ComNum.VBLF + " UNION ALL ";
                SQL += ComNum.VBLF + " SELECT A.PANO, A.SNAME, A.SEX, A.DEPTCODE, B.WARDCODE, A.ROOMCODE, B.DRCODE, TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE, TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ENTDATE, A.AGE";
                SQL += ComNum.VBLF + " FROM ADMIN.NUR_FALLHUMPDUMP_SCALE A, ADMIN.IPD_NEW_MASTER B ";
                SQL += ComNum.VBLF + " WHERE A.ACTDATE >= TO_DATE('" + argSDATE + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "   AND A.ACTDATE <= TO_DATE('" + argEDATE + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "   AND A.IPDNO = B.IPDNO";
                SQL += ComNum.VBLF + "   AND A.PANO = B.PANO";
                SQL += ComNum.VBLF + "   AND A.IPDNO > 0 ";
                SQL += ComNum.VBLF + "   AND ADMIN.FC_NUR_FALL_REPORT_CHK(A.PANO, A.ACTDATE) = 'Y' ";
                SQL += ComNum.VBLF + " GROUP BY A.PANO, A.SNAME, A.SEX, A.DEPTCODE, B.WARDCODE, A.ROOMCODE, B.DRCODE, TO_CHAR(A.ACTDATE,'YYYY-MM-DD'), TO_CHAR(A.ACTDATE,'YYYY-MM-DD'), A.AGE";
                SQL += ComNum.VBLF + " UNION ALL ";
                SQL += ComNum.VBLF + " SELECT A.PANO, B.SNAME, B.SEX, 'OPD' DEPTCODE, 'OPD' WARDCODE, 0 ROOMCODE, B.DRCODE, TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE, TO_CHAR(A.ENTDATE,'YYYY-MM-DD') ENTDATE, B.AGE ";
                SQL += ComNum.VBLF + " FROM ADMIN.NUR_FALL_SCALE_OPD A, ADMIN.OPD_MASTER B ";
                SQL += ComNum.VBLF + " WHERE A.ACTDATE >= TO_DATE('" + argSDATE + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "   AND A.ACTDATE <= TO_DATE('" + argEDATE + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "   AND A.PANO = B.PANO ";
                SQL += ComNum.VBLF + "   AND A.ACTDATE = B.BDATE ";
                SQL += ComNum.VBLF + "   GROUP BY A.PANO, B.SNAME, B.SEX, 'OPD', 'OPD', '', B.DRCODE, TO_CHAR(A.ACTDATE, 'YYYY-MM-DD'), TO_CHAR(A.ENTDATE, 'YYYY-MM-DD'), B.AGE ";
                SQL += ComNum.VBLF + ") ";
                SQL += ComNum.VBLF + " GROUP BY PANO, SNAME, SEX, DEPTCODE, WARDCODE, ROOMCODE, DRCODE, ACTDATE, ENTDATE, AGE";


                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for(i = 0; i < dt.Rows.Count; i++)
                {
                    nCnt1 += 1;


                    SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                    SQL = SQL + ArgYYMM + "','" + strCODE + "',1,";
                    SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                    SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                    SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                    SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                    SQL = SQL + "  TO_DATE('" + dt.Rows[i]["ACTDATE"].ToString().Trim() + "','YYYY-MM-DD'),";
                    SQL = SQL + "  TO_DATE('" + dt.Rows[i]["ENTDATE"].ToString().Trim() + "','YYYY-MM-DD')  ) ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }

                dt.Dispose();
                dt = null;

                // '낙상보고서 작성 건수  //2019-09-04 외래 작성 건수 추가
                SQL = " SELECT PANO, SNAME, SEX, DEPTCODE, WARDCODE, ROOMCODE, DRCODE, TIME1, TIME2 FROM ( ";
                SQL += ComNum.VBLF + " SELECT A.PANO, A.SNAME, A.SEX, A.DEPTCODE, A.WARDCODE, A.ROOMCODE, B.DRCODE, TO_CHAR(B.INDATE,'YYYY-MM-DD') TIME1, TO_CHAR(A.ACTDATE,'YYYY-MM-DD') TIME2";
                SQL += ComNum.VBLF + " FROM ADMIN.NUR_FALL_REPORT A, ADMIN.IPD_NEW_MASTER B";
                SQL += ComNum.VBLF + " WHERE A.ACTDATE >= TO_DATE('" + argSDATE + " 00:00','YYYY-MM-DD HH24:MI')";
                SQL += ComNum.VBLF + "   AND A.ACTDATE <= TO_DATE('" + argEDATE + " 23:59','YYYY-MM-DD HH24:MI')";
                SQL += ComNum.VBLF + "   AND A.IPDNO = B.IPDNO";
                SQL += ComNum.VBLF + "   AND A.PANO = B.PANO";
                SQL += ComNum.VBLF + "   AND A.IPDNO > 0 ";
                SQL += ComNum.VBLF + " GROUP BY A.PANO, A.SNAME, A.SEX, A.DEPTCODE, A.WARDCODE, A.ROOMCODE, B.DRCODE, B.INDATE, A.ACTDATE";
                SQL += ComNum.VBLF + " UNION ALL ";
                SQL += ComNum.VBLF + " SELECT A.PANO, B.SNAME, B.SEX, A.DEPTCODE, 'OPD' WARDCODE, A.ROOMCODE, B.DRCODE, TO_CHAR(B.BDATE, 'YYYY-MM-DD') TIME1, TO_CHAR(A.ACTDATE, 'YYYY-MM-DD') TIME2 ";
                SQL += ComNum.VBLF + "   FROM ADMIN.NUR_FALL_REPORT A, ADMIN.OPD_MASTER B ";
                SQL += ComNum.VBLF + "  WHERE A.ACTDATE >= TO_DATE('" + argSDATE + " 00:00', 'YYYY-MM-DD HH24:MI') ";
                SQL += ComNum.VBLF + "    AND A.ACTDATE <= TO_DATE('" + argEDATE + " 23:59', 'YYYY-MM-DD HH24:MI') ";
                SQL += ComNum.VBLF + "    AND A.PANO = B.PANO ";
                SQL += ComNum.VBLF + "    AND A.DEPTCODE = B.DEPTCODE ";
                SQL += ComNum.VBLF + "    AND TRUNC(A.ACTDATE) = B.BDATE ";
                SQL += ComNum.VBLF + "    AND A.IPDNO = 0 ";
                SQL += ComNum.VBLF + "  GROUP BY A.PANO, B.SNAME, B.SEX, A.DEPTCODE, 'OPD', A.ROOMCODE, B.DRCODE, TO_CHAR(B.BDATE, 'YYYY-MM-DD'), TO_CHAR(A.ACTDATE, 'YYYY-MM-DD') ";
                SQL += ComNum.VBLF + " ) GROUP BY PANO, SNAME, SEX, DEPTCODE, WARDCODE, ROOMCODE, DRCODE, TIME1, TIME2 ";


                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nCnt2 += 1;

                    SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                    SQL = SQL + ArgYYMM + "','" + strCODE + "',2,";
                    SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                    SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                    SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                    SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                    SQL = SQL + "  TO_DATE('" + dt.Rows[i]["TIME1"].ToString().Trim() + "','YYYY-MM-DD HH24:MI'),";
                    SQL = SQL + "  TO_DATE('" + dt.Rows[i]["TIME2"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }


                dt.Dispose();
                dt = null;


                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 투약 위험 대상
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns>
        bool Build_51003_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;

            int i = 0;
            string strCODE = "51003";
            long nCnt1 = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                SQL = " SELECT PANO, SNAME, SEX, DEPTCODE, WARDCODE, ROOMCODE, DRCODE, TIME1, TIME2 FROM ( ";
                SQL = SQL + ComNum.VBLF + " SELECT A.PANO, A.SNAME, A.SEX, A.DEPTCODE, A.WARDCODE, A.ROOMCODE, B.DRCODE, TO_CHAR(B.INDATE, 'YYYY-MM-DD') TIME1, TO_CHAR(A.BDATE,'YYYY-MM-DD') TIME2";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.NUR_STD_DRUG A, ADMIN.IPD_NEW_MASTER B";
                SQL = SQL + ComNum.VBLF + " WHERE A.BDATE >= TO_DATE('" + argSDATE + " 00:00','YYYY-MM-DD HH24:MI')";
                SQL = SQL + ComNum.VBLF + "   AND A.BDATE <= TO_DATE('" + argEDATE + " 23:59','YYYY-MM-DD HH24:MI')";
                SQL = SQL + ComNum.VBLF + "   AND A.IPDNO = B.IPDNO";
                SQL = SQL + ComNum.VBLF + "   AND A.PANO = B.PANO";
                SQL = SQL + ComNum.VBLF + "  UNION ALL ";
                SQL = SQL + ComNum.VBLF + " SELECT A.PANO, B.SNAME, B.SEX, A.DEPTCODE, 'OPD' WARDCODE, A.ROOMCODE, B.DRCODE, TO_CHAR(B.BDATE, 'YYYY-MM-DD') TIME1, TO_CHAR(A.BDATE, 'YYYY-MM-DD') TIME2 "; 
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.NUR_STD_DRUG A, ADMIN.OPD_MASTER B";
                SQL = SQL + ComNum.VBLF + " WHERE A.BDATE >= TO_DATE('" + argSDATE + " 00:00','YYYY-MM-DD HH24:MI')";
                SQL = SQL + ComNum.VBLF + "   AND A.BDATE <= TO_DATE('" + argEDATE + " 23:59','YYYY-MM-DD HH24:MI')";
                SQL = SQL + ComNum.VBLF + "   AND DECODE(A.DEPTCODE, 'EM','ER',A.DEPTCODE) = B.DEPTCODE ";
                SQL = SQL + ComNum.VBLF + "   AND TRUNC(A.BDATE) = B.BDATE ";
                SQL = SQL + ComNum.VBLF + "   AND A.PANO = B.PANO ";
                SQL = SQL + ComNum.VBLF + "   AND A.IPDNO = 0 ";
                SQL = SQL + ComNum.VBLF + " ) GROUP BY PANO, SNAME, SEX, DEPTCODE, WARDCODE, ROOMCODE, DRCODE, TIME1, TIME2 ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nCnt1 += 1;


                    SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                    SQL = SQL + ArgYYMM + "','" + strCODE + "',1,";
                    SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                    SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                    SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                    SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                    SQL = SQL + "  TO_DATE('" + dt.Rows[i]["TIME1"].ToString().Trim() + "','YYYY-MM-DD HH24:MI'),";
                    SQL = SQL + "  TO_DATE('" + dt.Rows[i]["TIME2"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 중등도 분류표
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        /// <returns></returns>
        bool Build_52001_Process(string ArgYYMM, string argSDATE, string argEDATE)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;

            int i = 0;
            string strCODE = "52001";
            long nCnt1 = 0;
            long nCnt2 = 0;
            long nCnt3 = 0;
            long nCnt4 = 0;
            long nCnt5 = 0;

            int intVal = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //'종전 결과를 삭제함
                SQL = "DELETE FROM NUR_QI_DATA ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "DELETE FROM NUR_QI_DATA_LIST ";
                SQL += ComNum.VBLF + "WHERE YYMM='" + ArgYYMM + "' ";
                SQL += ComNum.VBLF + "  AND Code='" + strCODE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //'투약오류 보고서
                SQL = " SELECT A.PANO, B.SNAME, B.SEX, A.DEPTCODE, A.WARDCODE, A.ROOMCODE, B.DRCODE,";
                SQL += ComNum.VBLF + " TO_CHAR(B.INDATE, 'YYYY-MM-DD') TIME1, TO_CHAR(A.JOBTIME, 'YYYY-MM-DD') TIME2, A.GRADE";
                SQL += ComNum.VBLF + " FROM ADMIN.NUR_SERIOUSK A, ADMIN.IPD_NEW_MASTER B";
                SQL += ComNum.VBLF + " WHERE A.JOBTIME >= TO_DATE('" + argSDATE + " 00:00','YYYY-MM-DD HH24:MI')";
                SQL += ComNum.VBLF + "   AND A.JOBTIME <= TO_DATE('" + argEDATE + " 23:59','YYYY-MM-DD HH24:MI')";
                SQL += ComNum.VBLF + "   AND A.IPDNO = B.IPDNO";
                SQL += ComNum.VBLF + "   AND A.PANO = B.PANO";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    switch(dt.Rows[i]["GRADE"].ToString().Trim())
                    {
                        case "1":
                            nCnt1 += 1;
                            intVal = 1;
                            break;
                        case "2":
                            nCnt2 += 1;
                            intVal = 2;
                            break;
                        case "3":
                            nCnt3 += 1;
                            intVal = 3;
                            break;
                        case "4":
                            nCnt4 += 1;
                            intVal = 4;
                            break;
                        default:
                            nCnt5 += 1;
                            intVal = 5;
                            break;
                    }

                    SQL = "INSERT INTO NUR_QI_DATA_LIST (YYMM,Code,Item,PANO,SNAME,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE,Time1,Time2) VALUES ('";
                    SQL = SQL + ArgYYMM + "','" + strCODE + "'," + intVal + ",";
                    SQL = SQL + " '" + dt.Rows[i]["PANO"].ToString().Trim() + "','" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                    SQL = SQL + " '" + dt.Rows[i]["SEX"].ToString().Trim() + "','" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                    SQL = SQL + " '" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "','" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "',";
                    SQL = SQL + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                    SQL = SQL + "  TO_DATE('" + dt.Rows[i]["TIME1"].ToString().Trim() + "','YYYY-MM-DD HH24:MI'),";
                    SQL = SQL + "  TO_DATE('" + dt.Rows[i]["TIME2"].ToString().Trim() + "','YYYY-MM-DD HH24:MI')  ) ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',1," + nCnt1 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',2," + nCnt2 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',3," + nCnt3 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',4," + nCnt4 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "INSERT INTO NUR_QI_DATA (YYMM,Code,Item,Result) VALUES ('";
                SQL = SQL + ArgYYMM + "','" + strCODE + "',52," + nCnt5 + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        int DATE_ILSU(string strSDate, string strEDate)
        {
            int rtnVal = 0;
            try
            {
                return (int) Math.Round((Convert.ToDateTime(strSDate) - Convert.ToDateTime(strEDate)).TotalDays, 2);
            }
            catch
            {
                return rtnVal;
            }
        }

        int DATE_TIME(string strSDate, string strEDate)
        {
            int rtnVal = 0;
            try
            {
                TimeSpan tSpan = Convert.ToDateTime(strSDate) - Convert.ToDateTime(strEDate);
                return (int)Math.Round(tSpan.TotalMinutes, 2);
            }
            catch
            {
                return rtnVal;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ss1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ss1_Sheet1.RowCount == 0) return;

            if (e.ColumnHeader && e.Column == 0)
            {
                ss1_Sheet1.Cells[0, 0, ss1_Sheet1.RowCount - 1, 0].Value = !Convert.ToBoolean(ss1_Sheet1.Cells[0, 0].Value);
            }
        }
    }
}
