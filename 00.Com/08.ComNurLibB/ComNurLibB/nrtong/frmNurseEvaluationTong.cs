using System;
using System.Data;
using System.Windows.Forms;
using ComBase;
using System.IO;
using FarPoint.Win.Spread.CellType;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmNurseEvaluationTong.cs
    /// Description     : 간호 포괄 서비스 병동 일일평가서 통계
    /// Author          : 안정수
    /// Create Date     : 2018-02-05
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 기존 Frm간호간병통합평가표통계.frm(Frm간호간병통합평가표통계) 폼 frmNrstdMain.cs 으로 변경함
    /// </history>
    /// <seealso cref= "\nurse\nrtong\Frm간호간병통합평가표통계.frm(Frm간호간병통합평가표통계) >> frmNurseEvaluationTong.cs 폼이름 재정의" />
    public partial class frmNurseEvaluationTong : Form, MainFormMessage
    {
        #region 클래스 선언 및 etc....

        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsNurse CN = new clsNurse();

        string SQL = "";
        string SqlErr = "";
        DataTable dt = null;

        string FstrWard = "";
        int[,] FnTot = new int[21, 21];

        #endregion


        #region MainFormMessage InterFace

        public MainFormMessage mCallForm = null;

        public void MsgActivedForm(Form frm)
        {

        }

        public void MsgUnloadForm(Form frm)
        {

        }

        public void MsgFormClear()
        {

        }

        public void MsgSendPara(string strPara)
        {

        }

        #endregion

        public frmNurseEvaluationTong(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmNurseEvaluationTong()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnView.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnCheck.Click += new EventHandler(eBtnClick);
            this.btnGraph.Click += new EventHandler(eBtnClick);

            //this.btnSearch.Click += new EventHandler(eBtnSearch);

            this.btnPrint.Click += new EventHandler(eBtnPrint);

            //this.eControl.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            //this.eControl.LostFocus += new EventHandler(eControl_LostFocus);
            //this.eControl.GotFocus += new EventHandler(eControl_GotFocus);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            else
            {
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
                ComFunc.ReadSysDate(clsDB.DbCon);

                Set_Init();
            }
        }

        void eFormActivated(object sender, EventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgActivedForm(this);
            }
        }

        void eFormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgUnloadForm(this);
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }

            else if (sender == this.btnView)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eGetData();
            }
            else if (sender == this.btnSave)
            {
                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eBtnSave();
                eGetData();
            }

            else if (sender == this.btnCheck)
            {
                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eBtnCheck();
                eGetData();
            }

            else if (sender == this.btnGraph)
            {

                int i = 0;
                int j = 0;

                double[] nVal1 = new double[14];
                double[] nVal2 = new double[14];
                double[] nVal3 = new double[14];

                string strTitle = "";

                for (i = 0; i < ssList_Sheet1.RowCount; i++)
                {
                    for (j = 9; j < 23; j++)
                    {
                        switch (ssList_Sheet1.Cells[i, j].Text.Trim())
                        {
                            case "0":
                                nVal1[j - 9]++;
                                break;
                            case "1":
                                nVal2[j - 9]++;
                                break;
                            case "2":
                                nVal3[j - 9]++;
                                break;
                        }

                    }

                }

                strTitle = dtpFDate.Text + "~" + dtpTDate.Text;
                frmNurseEvaluationGraph frmNurseEvaluationGraphX = new frmNurseEvaluationGraph(nVal1, nVal2, nVal3, strTitle);
                frmNurseEvaluationGraphX.StartPosition = FormStartPosition.CenterParent;
                frmNurseEvaluationGraphX.ShowDialog();

                nVal1 = null;
                nVal2 = null;
                nVal3 = null;

                clsNurse.setClearMemory(frmNurseEvaluationGraphX);
            }

        }

        void eBtnPrint(object sender, EventArgs e)
        {
            if (sender == this.btnPrint)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                ePrint();
            }
        }

        void Set_Init()
        {
            string strDate = "";

            //2019-01-21 박창욱 : 40병동 수간호사 권한 추가
            if (clsType.User.Sabun != "48265" && clsType.User.JobGroup != "JOB013053" && clsType.User.Sabun != "22231")
            {
                ComFunc.MsgBox("권한이 없습니다. 전산정보팀으로 연락주세요.");
                this.Close();
                return;
            }

            strDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            dtpTDate.Value = Convert.ToDateTime(VB.Left(strDate, 7) + "-15");
            dtpFDate.Value = dtpTDate.Value.AddMonths(-1).AddDays(1);

            cboBi.Items.Clear();
            cboBi.Items.Add("전체");
            cboBi.Items.Add("건강보험");
            cboBi.Items.Add("자보");
            cboBi.Items.Add("산재");
            cboBi.Items.Add("의료급여");
            cboBi.Items.Add("일반");
            cboBi.SelectedIndex = 0;

            ComboWard_SET();

            FstrWard = clsPublic.GstrHelpCode;
            cboWard.Text = "40";

            clsSpread spread = new clsSpread();

            for (int i = 0; i < ssList_Sheet1.ColumnCount; i++)
            {
                spread.setSpdFilter(ssList, i, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            }

            spread = null;
        }

        void ComboWard_SET()
        {
            int i = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  WardCode, WardName";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_WARD";
            SQL += ComNum.VBLF + "WHERE WARDCODE NOT IN ('IU','NP','2W','IQ','ER')";
            SQL += ComNum.VBLF + "  AND USED = 'Y'";
            SQL += ComNum.VBLF + "ORDER BY WardCode ";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if(dt.Rows.Count > 0)
            {
                cboWard.Items.Clear();
                cboWard.Items.Add("전체");

                for(i = 0; i < dt.Rows.Count; i++)
                {
                    cboWard.Items.Add(dt.Rows[i]["WardCode"].ToString().Trim());
                }

                //cboWard.Items.Add("SICU");
                //cboWard.Items.Add("MICU");
            }

            dt.Dispose();
            dt = null;

            cboWard.SelectedIndex = 0;

            //foreach(string s in cboWard.Items)
            //{
            //    if(s == clsNurse.gsWard)
            //    {
            //        cboWard.Text = s;
            //        cboWard.Enabled = false;
            //        break;
            //    }
            //}
        }

        void ePrint()
        {
            // 스프레드의 내용을 엑셀파일로 저장
            //SaveFileDialog mDlg = new SaveFileDialog();
            //mDlg.InitialDirectory = Application.StartupPath;
            //mDlg.Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*";
            //mDlg.FilterIndex = 1;

            string strDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D").Replace("-", "") + "_" + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T").Replace(":", "");

            string dirPath = "C:\\간호간병통합서비스";
            //if (mDlg.ShowDialog() == DialogResult.OK)
            //{
            //ssList.SaveExcel(mDlg.FileName, FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            
            //2019-01-18 박창욱 : xls에서 xlsx로 확장자 변경
            ssList.SaveExcel(Path.Combine(dirPath,  strDate + ".xlsx"), FarPoint.Excel.ExcelSaveFlags.UseOOXMLFormat| FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders);
            ComFunc.MsgBox("저장이 완료 되었습니다.");
            //}
        }

        void SS_Clear()
        {
            ssList.ActiveSheet.Cells[1, 0, ssList.ActiveSheet.Rows.Count - 1, ssList.ActiveSheet.Columns.Count - 1].Text = "";
        }

        void eBtnCheck()
        {
            //1) 중복 자료로 최종 데이터 놔두고 백업 후 삭제

            //int i = 0;
            //string strROWID = "";
            int intRowAffected = 0;


            SQL = " INSERT INTO KOSMOS_PMPA.NUR_TOTALCARE_NOTE_DEL ";
            SQL += ComNum.VBLF + " SELECT A.* ";
            SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_TOTALCARE_NOTE A, (SELECT MAX(ENTDATE) ENTDATE, PANO, JOBTIME ";
            SQL += ComNum.VBLF + "                                           FROM KOSMOS_PMPA.NUR_TOTALCARE_NOTE ";
            SQL += ComNum.VBLF + "                                          GROUP BY PANO, JOBTIME) B ";
            SQL += ComNum.VBLF + " WHERE A.PANO = B.PANO ";
            SQL += ComNum.VBLF + "   AND A.JOBTIME = B.JOBTIME ";
            SQL += ComNum.VBLF + "   AND A.ENTDATE<> B.ENTDATE ";
            SQL += ComNum.VBLF + "   AND A.JOBTIME  >=   TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "   AND A.JOBTIME  <=   TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD')";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("저장 중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            SQL = " DELETE KOSMOS_PMPA.NUR_TOTALCARE_NOTE ";
            SQL += ComNum.VBLF + " WHERE ROWID IN (";
            SQL += ComNum.VBLF + " SELECT A.ROWID ";
            SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_TOTALCARE_NOTE A, (SELECT MAX(ENTDATE) ENTDATE, PANO, JOBTIME ";
            SQL += ComNum.VBLF + "                                           FROM KOSMOS_PMPA.NUR_TOTALCARE_NOTE ";
            SQL += ComNum.VBLF + "                                          GROUP BY PANO, JOBTIME) B ";
            SQL += ComNum.VBLF + " WHERE A.PANO = B.PANO ";
            SQL += ComNum.VBLF + "   AND A.JOBTIME = B.JOBTIME ";
            SQL += ComNum.VBLF + "   AND A.ENTDATE<> B.ENTDATE ";
            SQL += ComNum.VBLF + "   AND A.JOBTIME  >=   TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "   AND A.JOBTIME  <=   TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD'))";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("저장 중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            return;

        }

        void GetCount()
        {

            //int i = 0;
            //string strROWID = "";
            //int intRowAffected = 0;

            lbCnt.Text = "";

            int nCnt = 0;
            int nCnt2 = 0;

            SQL = "SELECT COUNT(*) CNT FROM( ";
            SQL += ComNum.VBLF + " SELECT B.JOBTIME, B.PANO ";
            SQL += ComNum.VBLF + "   FROM NUR_MASTER A, KOSMOS_PMPA.NUR_TOTALCARE_NOTE B, BAS_PATIENT C ,IPD_NEW_MASTER D, TONG_PATIENT E";
            //SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.NUR_TOTALCARE_NOTE ";
            SQL += ComNum.VBLF + "  WHERE B.JOBTIME  >=   TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "    AND B.JOBTIME  <=   TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "    AND A.IPDNO = B.IPDNO ";
            SQL += ComNum.VBLF + "    AND A.PANO = E.PANO ";
            SQL += ComNum.VBLF + "    AND B.JOBTIME =E.JOBDATE";
            SQL += ComNum.VBLF + "    AND A.IPDNO = D.IPDNO";
            SQL += ComNum.VBLF + "    AND B.wardcode  =   '" + cboWard.Text.Trim() + "'";
            SQL += ComNum.VBLF + "    AND A.PANO = C.PANO";
            SQL += ComNum.VBLF + "    AND a.Pano < '90000000'";
            SQL += ComNum.VBLF + "    AND D.GBSTS <> '9'";
            SQL += ComNum.VBLF + "    AND E.T_CARE ='Y'";
            SQL += ComNum.VBLF + "    AND E.PACLASS <>'1'";

            if (cboBi.SelectedItem.ToString().Trim() == "건강보험")
            {
                SQL += ComNum.VBLF + "  AND D.BI IN ('11', '12', '13', '32', '41', '42', '43', '44')";
            }

            else if (cboBi.SelectedItem.ToString().Trim() == "자보")
            {
                SQL += ComNum.VBLF + "  AND D.BI IN ('52', '55')";
            }

            else if (cboBi.SelectedItem.ToString().Trim() == "산재")
            {
                SQL += ComNum.VBLF + "  AND D.BI IN ('31', '33')";
            }

            else if (cboBi.SelectedItem.ToString().Trim() == "일반")
            {
                SQL += ComNum.VBLF + "  AND D.BI IN ('51', '54')";
            }

            else if (cboBi.SelectedItem.ToString().Trim() == "의료급여")
            {
                SQL += ComNum.VBLF + "  AND D.BI IN ('21', '22', '23', '24')";
            }

            SQL += ComNum.VBLF + "      AND a.Pano <> '81000004'";
            SQL += ComNum.VBLF + "  GROUP BY B.JOBTIME, B.PANO) ";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["CNT"].ToString().Trim(), out nCnt);
            }
            dt.Dispose();
            dt = null;

            SQL = " SELECT COUNT(*) CNT FROM (";
            SQL += ComNum.VBLF + " SELECT B.JOBTIME, B.PANO, B.ENTDATE ";
            SQL += ComNum.VBLF + "   FROM NUR_MASTER A, KOSMOS_PMPA.NUR_TOTALCARE_NOTE B, BAS_PATIENT C ,IPD_NEW_MASTER D, TONG_PATIENT E";
            //SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.NUR_TOTALCARE_NOTE ";
            SQL += ComNum.VBLF + "  WHERE B.JOBTIME  >=   TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "    AND B.JOBTIME  <=   TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "    AND A.IPDNO = B.IPDNO ";
            SQL += ComNum.VBLF + "    AND A.PANO = E.PANO ";
            SQL += ComNum.VBLF + "    AND B.JOBTIME =E.JOBDATE";
            SQL += ComNum.VBLF + "    AND A.IPDNO = D.IPDNO";
            SQL += ComNum.VBLF + "    AND B.wardcode  =   '" + cboWard.Text.Trim() + "'";
            SQL += ComNum.VBLF + "    AND A.PANO = C.PANO";
            SQL += ComNum.VBLF + "    AND a.Pano < '90000000'";
            SQL += ComNum.VBLF + "    AND D.GBSTS <> '9'";
            SQL += ComNum.VBLF + "    AND E.T_CARE ='Y'";
            SQL += ComNum.VBLF + "    AND E.PACLASS <>'1'";

            if (cboBi.SelectedItem.ToString().Trim() == "건강보험")
            {
                SQL += ComNum.VBLF + "  AND D.BI IN ('11', '12', '13', '32', '41', '42', '43', '44')";
            }

            else if (cboBi.SelectedItem.ToString().Trim() == "자보")
            {
                SQL += ComNum.VBLF + "  AND D.BI IN ('52', '55')";
            }

            else if (cboBi.SelectedItem.ToString().Trim() == "산재")
            {
                SQL += ComNum.VBLF + "  AND D.BI IN ('31', '33')";
            }

            else if (cboBi.SelectedItem.ToString().Trim() == "일반")
            {
                SQL += ComNum.VBLF + "  AND D.BI IN ('51', '54')";
            }

            else if (cboBi.SelectedItem.ToString().Trim() == "의료급여")
            {
                SQL += ComNum.VBLF + "  AND D.BI IN ('21', '22', '23', '24')";
            }

            SQL += ComNum.VBLF + "      AND a.Pano <> '81000004'";
            SQL += ComNum.VBLF + "      GROUP BY B.JOBTIME, B.PANO, B.ENTDATE)";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["CNT"].ToString().Trim(), out nCnt2);
            }
            dt.Dispose();
            dt = null;

            if (nCnt != nCnt2)
            {
                lbCnt.BackColor = System.Drawing.Color.Black;
                lbCnt.ForeColor = System.Drawing.Color.Yellow;
            }
            else
            {
                lbCnt.BackColor = System.Drawing.Color.White;
                lbCnt.ForeColor = System.Drawing.Color.Black;
            }

            lbCnt.Text = "중증도 인원수 : " + nCnt + "명 / 작성수 : " + nCnt2 + "명";

            return;
        }
        
        void eGetData()
        {
            int i = 0;
            int nRow = 0;
            int nREAD = 0;

            string strIPDNO = "";

            SS_Clear();

            GetCount();

            strIPDNO = VB.Pstr(clsPublic.GstrHelpCode, "@", 3);

            SQL = "";
            //SQL += ComNum.VBLF + "SELECT ";
            //SQL += ComNum.VBLF + "  '포항성모병원'  Hspname,'37100068' Hspcode,'2' HspLv,b.wardcode,b.wardcode||'병동' wardname, ";
            //SQL += ComNum.VBLF + "  B.Pano, a.SName, decode(a.Sex,'M','1','2') Sex,A.AGE, JUMIN1 ,JUMIN2 ,B.DIAGNOSIS,TO_CHAR(B.JOBTIME,'YYYYMMDD') JOBTIME,";
            //SQL += ComNum.VBLF + "  nvl(B.BUN01,0) BUN01,nvl(B.BUN02,0) BUN02,nvl(B.BUN03,0) BUN03,nvl(B.BUN04,0) BUN04,nvl(B.BUN05,0) BUN05,nvl(B.BUN06,0) BUN06,";
            //SQL += ComNum.VBLF + "  nvl(B.BUN07,0) BUN07,nvl(B.BUN08,0) BUN08,nvl(B.BUN09,0) BUN09,nvl(B.BUN10,0) BUN10,";
            //SQL += ComNum.VBLF + "  B.BUN11,B.BUN12,B.BUN13,B.BUN14,";
            //SQL += ComNum.VBLF + "  VALUE_OP, VALUE_ROOMLV ,";
            //SQL += ComNum.VBLF + "  NVL(VALUE_FALL,'0') VALUE_FALL ,NVL(VALUE_BEDSORE,'0') VALUE_BEDSORE , ";
            //SQL += ComNum.VBLF + "  B.ENDSABUN,TO_CHAR(B.ENTDATE,'YYYY-MM-DD HH24:MM') ENTDATE,A.IPDNO ";
            //SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_MASTER A, " + ComNum.DB_PMPA + "NUR_TOTALCARE_NOTE B, " + ComNum.DB_PMPA + "BAS_PATIENT C, ";
            //SQL += ComNum.VBLF +           ComNum.DB_PMPA + "IPD_NEW_MASTER D, " + ComNum.DB_PMPA + "TONG_PATIENT E";

            SQL = "";
            SQL += " SELECT  DISTINCT '포항성모병원'  Hspname,'37100068' Hspcode,'2' HspLv,";
            SQL += ComNum.VBLF + "decode(b.wardcode,'40','C002','60','C001','C001') wardcode,b.wardcode||'병동' wardname, ";
            SQL += ComNum.VBLF + " B.Pano, a.SName, decode(a.Sex,'M','1','2') Sex,A.AGE, ";
            SQL += ComNum.VBLF + "JUMIN1 ,JUMIN2 ,B.DIAGNOSIS,TO_CHAR(B.JOBTIME,'YYYYMMDD') JOBTIME, ";
            SQL += ComNum.VBLF + " nvl(B.BUN01,0) BUN01, nvl(B.BUN02,0) BUN02, nvl(B.BUN03,0) BUN03, nvl(B.BUN04,0) BUN04, ";
            SQL += ComNum.VBLF + " nvl(B.BUN05,0) BUN05, nvl(B.BUN06,0) BUN06, nvl(B.BUN07,0) BUN07, nvl(B.BUN08,0) BUN08, ";
            SQL += ComNum.VBLF + " nvl(B.BUN09,0) BUN09, nvl(B.BUN10,0) BUN10, NVL(B.BUN11,0) BUN11, NVL(B.BUN12,0) BUN12, ";
            SQL += ComNum.VBLF + " NVL(B.BUN13,0) BUN13, NVL(B.BUN14,0) BUN14, ";
            SQL += ComNum.VBLF + " VALUE_OP, VALUE_ROOMLV , NVL(VALUE_FALL,'0') VALUE_FALL , NVL(VALUE_BEDSORE,'0') VALUE_BEDSORE ,";
            SQL += ComNum.VBLF + " B.JOBTIME, B.ENDSABUN,TO_CHAR(B.ENTDATE,'YYYY-MM-DD HH24:MM') ENTDATE,A.IPDNO,";
            SQL += ComNum.VBLF + "  decode(substr(e.BI,1,1),'1','1','2','2',decode(e.BI,'31','3','52','4','5')) bi ,";
            SQL += ComNum.VBLF + " FC_TONG_PATIENT_HS(D.PANO,B.wardcode,to_char(E.JOBDATE,'yyyy-mm-dd')) gubun, B.ROWID ROWID2 ";
            SQL += ComNum.VBLF + "  FROM NUR_MASTER A, KOSMOS_PMPA.NUR_TOTALCARE_NOTE B, BAS_PATIENT C ,IPD_NEW_MASTER D , TONG_PATIENT E";

            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND A.IPDNO = B.IPDNO ";
            SQL += ComNum.VBLF + "      AND A.PANO = E.PANO ";
            SQL += ComNum.VBLF + "      AND B.JOBTIME =E.JOBDATE";
            SQL += ComNum.VBLF + "      AND A.IPDNO = D.IPDNO";
            SQL += ComNum.VBLF + "      AND B.JOBTIME  >=   TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "      AND B.JOBTIME  <=   TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "      AND B.wardcode  =   '" + cboWard.Text.Trim() + "'";
            SQL += ComNum.VBLF + "      AND A.PANO = C.PANO";
            SQL += ComNum.VBLF + "      AND a.Pano < '90000000'";
            SQL += ComNum.VBLF + "      AND D.GBSTS <> '9'";
            SQL += ComNum.VBLF + "      AND E.T_CARE ='Y'";
            SQL += ComNum.VBLF + "      AND E.PACLASS <>'1'";

            if(cboBi.SelectedItem.ToString().Trim() == "건강보험")
            {
                SQL += ComNum.VBLF + "  AND D.BI IN ('11', '12', '13', '32', '41', '42', '43', '44')";
            }

            else if (cboBi.SelectedItem.ToString().Trim() == "자보")
            {
                SQL += ComNum.VBLF + "  AND D.BI IN ('52', '55')";
            }

            else if (cboBi.SelectedItem.ToString().Trim() == "산재")
            {
                SQL += ComNum.VBLF + "  AND D.BI IN ('31', '33')";
            }

            else if (cboBi.SelectedItem.ToString().Trim() == "일반")
            {
                SQL += ComNum.VBLF + "  AND D.BI IN ('51', '54')";
            }

            else if (cboBi.SelectedItem.ToString().Trim() == "의료급여")
            {
                SQL += ComNum.VBLF + "  AND D.BI IN ('21', '22', '23', '24')";
            }

            SQL += ComNum.VBLF + "      AND a.Pano <> '81000004'";         
            SQL += ComNum.VBLF + "ORDER BY  B.JOBTIME,B.Pano";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if(dt.Rows.Count > 0)
            {
                nREAD = dt.Rows.Count;
                nRow = 0;

                for(i = 0; i < nREAD; i++)
                {
                    nRow = nRow + 1;
                    ssList.ActiveSheet.Rows.Count = nRow;

                    ssList.ActiveSheet.Cells[nRow - 1, 0].Text = dt.Rows[i]["Hspcode"].ToString().Trim();
                    ssList.ActiveSheet.Cells[nRow - 1, 1].Text = dt.Rows[i]["HspLv"].ToString().Trim();
                    //ssList.ActiveSheet.Cells[nRow - 1, 2].Text = "C001";
                    ssList.ActiveSheet.Cells[nRow - 1, 2].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                    ssList.ActiveSheet.Cells[nRow - 1, 3].Text = dt.Rows[i]["JOBTIME"].ToString().Trim();
                    ssList.ActiveSheet.Cells[nRow - 1, 4].Value = nRow;
                    ssList.ActiveSheet.Cells[nRow - 1, 5].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    
                    if(VB.Mid(dt.Rows[i]["JUMIN2"].ToString().Trim(), 1, 1) == "3" || VB.Mid(dt.Rows[i]["JUMIN2"].ToString().Trim(), 1, 1) == "4")
                    {
                        ssList.ActiveSheet.Cells[nRow - 1, 6].Text = "20" + dt.Rows[i]["JUMIN1"].ToString().Trim();
                    }

                    else
                    {
                        ssList.ActiveSheet.Cells[nRow - 1, 6].Text = "19" + dt.Rows[i]["JUMIN1"].ToString().Trim();
                    }

                    ssList.ActiveSheet.Cells[nRow - 1, 7].Text = dt.Rows[i]["SEX"].ToString().Trim();
                    ssList.ActiveSheet.Cells[nRow - 1, 8].Text = dt.Rows[i]["DIAGNOSIS"].ToString().Trim();

                    ssList.ActiveSheet.Cells[nRow - 1, 9].Text = dt.Rows[i]["BUN01"].ToString().Trim();
                    ssList.ActiveSheet.Cells[nRow - 1, 10].Text = dt.Rows[i]["BUN02"].ToString().Trim();
                    ssList.ActiveSheet.Cells[nRow - 1, 11].Text = dt.Rows[i]["BUN03"].ToString().Trim();
                    ssList.ActiveSheet.Cells[nRow - 1, 12].Text = dt.Rows[i]["BUN04"].ToString().Trim();
                    ssList.ActiveSheet.Cells[nRow - 1, 13].Text = dt.Rows[i]["BUN05"].ToString().Trim();
                    ssList.ActiveSheet.Cells[nRow - 1, 14].Text = dt.Rows[i]["BUN06"].ToString().Trim();
                    ssList.ActiveSheet.Cells[nRow - 1, 15].Text = dt.Rows[i]["BUN07"].ToString().Trim();
                    ssList.ActiveSheet.Cells[nRow - 1, 16].Text = dt.Rows[i]["BUN08"].ToString().Trim();
                    ssList.ActiveSheet.Cells[nRow - 1, 17].Text = dt.Rows[i]["BUN09"].ToString().Trim();
                    ssList.ActiveSheet.Cells[nRow - 1, 18].Text = dt.Rows[i]["BUN10"].ToString().Trim();
                    ssList.ActiveSheet.Cells[nRow - 1, 19].Text = dt.Rows[i]["BUN11"].ToString().Trim();
                    ssList.ActiveSheet.Cells[nRow - 1, 20].Text = dt.Rows[i]["BUN12"].ToString().Trim();
                    ssList.ActiveSheet.Cells[nRow - 1, 21].Text = dt.Rows[i]["BUN13"].ToString().Trim();
                    ssList.ActiveSheet.Cells[nRow - 1, 22].Text = dt.Rows[i]["BUN14"].ToString().Trim();

                    ssList.ActiveSheet.Cells[nRow - 1, 23].Text = dt.Rows[i]["VALUE_OP"].ToString().Trim();
                    ssList.ActiveSheet.Cells[nRow - 1, 24].Text = dt.Rows[i]["VALUE_ROOMLV"].ToString().Trim();
                    ssList.ActiveSheet.Cells[nRow - 1, 25].Text = dt.Rows[i]["VALUE_FALL"].ToString().Trim();
                    ssList.ActiveSheet.Cells[nRow - 1, 26].Text = dt.Rows[i]["VALUE_BEDSORE"].ToString().Trim();
                    ssList.ActiveSheet.Cells[nRow - 1, 27].Text = dt.Rows[i]["bi"].ToString().Trim();
                    ssList.ActiveSheet.Cells[nRow - 1, 28].Text = dt.Rows[i]["gubun"].ToString().Trim();
                    ssList.ActiveSheet.Cells[nRow - 1, 29].Text = dt.Rows[i]["ROWID2"].ToString().Trim();
                }
            }

            TextCellType pgTextCellType = new TextCellType();
            pgTextCellType.CharacterCasing = CharacterCasing.Upper;
            ssList_Sheet1.Cells[0, 8, ssList_Sheet1.RowCount - 1, 8].CellType = pgTextCellType;

            dt.Dispose();
            dt = null;
        }

        private void ssList_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {

            if (e.Row < 1)
            { return; }

            if (e.Column != 8)
            { return; }

            ssList_Sheet1.Cells[e.Row, 30].Text = "Y";
            ssList_Sheet1.Cells[e.Row, 0, e.Row, ssList_Sheet1.ColumnCount-1].ForeColor = System.Drawing.Color.Blue;

        }

        private void eBtnSave()
        {
            int i = 0;
            string strROWID = "";
            string strCHANGE = "";
            string strILL = "";
            int intRowAffected = 0;

            for (i = 0; i <= ssList_Sheet1.RowCount - 1; i++)
            {
                strROWID = ssList.ActiveSheet.Cells[i, 29].Text.Trim(); 
                strCHANGE = ssList.ActiveSheet.Cells[i, 30].Text.Trim();
                strILL = ssList.ActiveSheet.Cells[i, 8].Text.Trim();

                if (strROWID != "" && strCHANGE == "Y")
                {
                    SQL = " UPDATE KOSMOS_PMPA.NUR_TOTALCARE_NOTE SET DIAGNOSIS = '" + strILL + "' ";
                    SQL += ComNum.VBLF + " WHERE ROWID = '" + strROWID + "'";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("저장 중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                }


            }

        }
    }
}
