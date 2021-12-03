using ComBase;
using ComDbB;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : 
    /// File Name       : frm.cs
    /// Description     : 
    /// Author          : 김민철
    /// Create Date     : 
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 기존 xxx 폼 xxx.cs 으로 변경함
    /// </history>
    /// <seealso cref= " " />

    public partial class frmPmpaSpecInq : Form
    {
        
        ComFunc CF = new ComFunc();
        clsSpread spread = new clsSpread();
        Thread thread;

        string FstrPANO = string.Empty;
        string FstrFDATE = string.Empty;
        string FstrTDATE = string.Empty;

        const int nCol_SCHK = 16;
        const int nCol_PANO = 60 + 15;
        const int nCol_DATE = 60 + 15;
        const int nCol_TIME = 110 + 15;
        const int nCol_SNAME = 60 + 15;
        const int nCol_CHEK = 50 + 15;
        const int nCol_AGE = 30 + 15;
        const int nCol_SEX = 30 + 15;
        const int nCol_DPNM = 60 + 15;
        const int nCol_DPCD = 30 + 15;
        const int nCol_WARD = 40 + 15;
        const int nCol_SPNO = 60 + 15;
        const int nCol_IOPD = 30 + 15;
        const int nCol_NAME = 150 + 15;
        const int nCol_JUSO = 300 + 15;
        const int nCol_STAT = 60 + 15;
        const int nCol_EXCD = 60 + 15;
        const int nCol_JUMN1 = 70 + 15;
        const int nCol_TEL = 100 + 15;
        const int nCol_LNAME = 460;
        const int nCol_ORDERNAME = 180;

        enum enmSel_EXAM_SPECMST_RCP03       {       CHK,      SPECNO,    RECEIVEDATE,      SNAME,  IPDOPD_NM,  DEPTCODE,     WARD,     ROOM,     DRCODE,  WORKSTS, EXAM_NAME, SPECCODE_NM, STATUS_NM,      BLOODDATE,          BDATE,     RESULTDATE,      PANO,   STATUS,   CANCEL_NM,  IPDOPD,  PB, CANCEL };
        string[] sSel_EXAM_SPECMST_RCP03 =   {    "선택",  "검체번호",     "접수일시", "환자성명", "환자구분",      "과",   "병동",   "병실", "의사번호",     "WS",  "검사명",      "검체",    "상태",     "채혈일시",     "처방일자",     "결과일시", "환자번호","STATUS", "CANCEL_NM", "IPDOPD", "PB", "CANCEL" };
        int[] nSel_EXAM_SPECMST_RCP03 =      { nCol_SCHK,   nCol_PANO, nCol_TIME - 20,  nCol_PANO,  nCol_IOPD, nCol_SCHK, nCol_AGE, nCol_AGE,   nCol_AGE, nCol_AGE, nCol_NAME,    nCol_AGE,  nCol_AGE+30, nCol_TIME - 20, nCol_TIME - 20, nCol_TIME - 20, nCol_PANO,5, 5,5,5,5 };        

        DateTime sysdate;

        public frmPmpaSpecInq()
        {
            InitializeComponent();
            setEvent();
            setCombo();
            setCtrlDate();

            setSpdStyle(this.ss_EXAM_SPECMST, null, sSel_EXAM_SPECMST_RCP03, nSel_EXAM_SPECMST_RCP03);


        }

        public frmPmpaSpecInq(string strPANO, string strFDATE, string strTDATE)
        {
            InitializeComponent();
            
            setEvent();
            setCombo();
            setCtrlDate();
            
            FstrPANO = strPANO;
            FstrFDATE = strFDATE;
            FstrTDATE = strTDATE;
            
            //setSpdStyle(this.ss_EXAM_SPECMST, null, sSel_EXAM_SPECMST_RCP03, nSel_EXAM_SPECMST_RCP03);
        }

        void setEvent()
        {
            this.Load               += new EventHandler(eFormLoad);
            
            //버튼 클릭 이벤트
            this.btnExit.Click      += new EventHandler(eBtnClick);
            this.btnSearch.Click    += new EventHandler(eBtnSearch);
            this.btnClear.Click     += new EventHandler(eBtnClick);

            
            //DataTimePicker 이벤트
            this.dtp_FDATE.ValueChanged += new EventHandler(CF.eDtpFormatSet);
            this.dtp_TDATE.ValueChanged += new EventHandler(CF.eDtpFormatSet);
        }

        void eBtnSearch(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            if (sender == this.btnSearch)
            {
                Display_SPREAD(clsDB.DbCon, this.ss_EXAM_SPECMST);
            }

        }

        /// <summary> 콤버박스에서 All을 삽입여부</summary>
        public enum enmComParamComboType
        {
            /// <summary> ****.전체 형태로 삽입</summary>
            ALL,
            /// <summary> Null.형태로 삽입</summary>
            NULL,
            /// <summary> 데이터만 삽입</summary>
            None
        }


        string getGubunText(string s, string gubun) // 12345.생화학 -> 12345를 반환
        {
            string strReturn = "";
            strReturn = s;

            if (strReturn != null && strReturn.Length > 0 && strReturn.IndexOf(gubun) > 0)
            {
                strReturn = strReturn.Substring(0, strReturn.IndexOf(gubun));
                if (strReturn.ToUpper() == "NULL" || strReturn.IndexOf('*') == 0)
                {
                    strReturn = "*";
                }
            }

            return strReturn;
        }

        void setCombo_View(ComboBox cbo, DataTable dt, enmComParamComboType e)
        {
            if (ComFunc.isDataTableNull(dt) == false)
            {
                cbo.Text = null;
                cbo.Items.Clear();

                string s = getGubunText(dt.Rows[0][0].ToString(), ".");
                string s2 = string.Empty;

                if (e == enmComParamComboType.ALL)
                {
                    for (int i = 0; i < s.Length; i++)
                    {
                        s2 += "*";
                    }
                    cbo.Items.Add(s2 + ".전체");

                }
                else if (e == enmComParamComboType.NULL)
                {
                    cbo.Items.Add("");
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cbo.Items.Add(dt.Rows[i][0].ToString().Trim());
                }

                if (cbo.Items.Count > 0) cbo.SelectedIndex = 0;

            }
            else
            {
                cbo.Text = null;
                cbo.Items.Clear();
                cbo.Items.Add("");
            }
        }

        DataTable sel_BAS_BCODE_COMBO(PsmhDb pDbCon, string gubun, bool isDelName = false)
        {
            DataTable dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            SQL = "";

            try
            {
                SQL = "";

                if (isDelName == true)
                {
                    SQL += " SELECT CODE AS CODE         \r\n";
                }
                else
                {
                    SQL += " SELECT CODE || '.' || NAME AS CODE         \r\n";
                }

                SQL += "   FROM " + ComNum.DB_PMPA + "BAS_BCODE B   \r\n";
                SQL += "  WHERE 1 = 1                               \r\n";
                SQL += "    AND GUBUN = " + ComFunc.covSqlstr(gubun, false);
                SQL += "  ORDER BY SORT,CODE                             \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }


            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }



            return dt;

        }

        void setCombo()
        {
            DataTable dt = sel_BAS_BCODE_COMBO(clsDB.DbCon, "EXAM_WS");
            if (ComFunc.isDataTableNull(dt) == false)
            {
                setCombo_View(this.cboWS, dt, enmComParamComboType.ALL);
            }
            else
            {
                ComFunc.MsgBox("조회된 값이 존재하지 않습니다.");
            }
        }

        void setCtrlDate()
        {
            sysdate = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));
            this.dtp_FDATE.Value = sysdate.AddDays(-3);
            this.dtp_TDATE.Value = sysdate;
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
                                
                Screen_Clear();

                this.txt_PANO.Text = FstrPANO;

                this.dtp_FDATE.Text = FstrFDATE;
                this.dtp_TDATE.Text = FstrTDATE;

                if (FstrPANO != "")
                {
                    Display_SPREAD(clsDB.DbCon, this.ss_EXAM_SPECMST);
                } 
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }
        }

        void Screen_Clear()
        {
            CF.dtpClear(dtp_FDATE);
            CF.dtpClear(dtp_TDATE);
        }
        
        void Display_SPREAD(PsmhDb pDbCon, FpSpread Spd)
        {
            Spd.ActiveSheet.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;

            string strFDATE = this.dtp_FDATE.Value.ToString("yyyy-MM-dd");
            string strTDATE = this.dtp_TDATE.Value.ToString("yyyy-MM-dd");
            string strPANO = this.txt_PANO.Text.Trim();
            string strSNAME = this.txt_SNAME.Text.Trim();

            bool isEXCP = this.rdo_EXP.Checked;

            string strWS    = getGubunText(this.cboWS.Text.Trim(), ".").Trim();
            string strWARD = "";
            //string strWARD  = getGubunText(this.cboWard.Text.Trim(), ".").Trim();

            //bool isPB       =  this.chk_PB.Checked;
            //bool isIPD      = this.chk_IPD.Checked;
            //bool isOPD      = this.chk_OPD.Checked;
            //bool is61       = this.chk_61.Checked;
            //bool is62       = this.chk_62.Checked;

            bool isIPD = true;
            bool isOPD = true;
            
            if (rdoIO1.Checked == false)
            {
                isOPD = rdoIO2.Checked;
                isIPD = rdoIO3.Checked;
            }

            bool isPB = true;


            //스레드 시작
            thread = new Thread(() => tProcess(strFDATE, strTDATE, strPANO, strSNAME, isEXCP, strWS, strWARD, isPB, isIPD, isOPD));
            thread.Start();

            Cursor.Current = Cursors.Default;
        }

        DataSet sel_EXAM_SPECMST_RCP03(PsmhDb pDbCon, string strFDATE, string strTDATE, string strPANO, string strSNAME, bool isEXCP, string strWS, string strWARD, bool isPB, bool isIPD, bool isOPD)
        {
            DataSet ds = null;
            string SQL = "";

            SQL = "";
            SQL += "  SELECT                                                                                                        \r\n";
            SQL += "  		'' 																		    AS CHK			-- 01       \r\n";
            SQL += "  		, A.SPECNO                                    							    AS SPECNO		-- 02       \r\n";
            SQL += "  		, TO_CHAR(A.RECEIVEDATE,'YYYY-MM-DD HH24:MI') 							    AS RECEIVEDATE  -- 03       \r\n";
            SQL += "  		, A.SNAME                                     							    AS SNAME		-- 04       \r\n";
            SQL += "  		, CASE WHEN A.IPDOPD ='I' THEN '입원'                                                                   \r\n";
            SQL += "  		       WHEN A.IPDOPD !='I' AND A.BI ='61' THEN '종검'                                                   \r\n";
            SQL += "  		       WHEN A.IPDOPD !='I' AND A.BI ='71' THEN '건진'                                                   \r\n";
            SQL += "  		       WHEN A.IPDOPD !='I' AND A.BI ='81' THEN '수탁'                                                   \r\n";
            SQL += "  		       ELSE '외래' END													    AS IPDOPD_NM	-- 05       \r\n";
            SQL += "  		, A.DEPTCODE                                  							    AS DEPTCODE		-- 06       \r\n";
            SQL += "  		, A.WARD                                      							    AS WARD			-- 07       \r\n";
            SQL += "  		, A.ROOM                                      							    AS ROOM			-- 07       \r\n";
            SQL += "  		, A.DRCODE                                    							    AS DRCODE		-- 08       \r\n";
            SQL += "  		, A.WORKSTS                                   							    AS WORKSTS		-- 09       \r\n";
            SQL += "  		, ADMIN.FC_EXAM_RESULTC_EXAMNAME(A.SPECNO) 							AS EXAM_NAME	-- 10       \r\n";
            SQL += "  		, ADMIN.FC_EXAM_SPECMST_NM('14', A.SPECCODE,'Y') 					    AS SPECCODE_NM  -- 11       \r\n";
            SQL += "  		, ADMIN.FC_BAS_BCODE_NAME('EXAM_STATUS', A.STATUS) 	                AS STATUS_NM    -- 12       \r\n";
            SQL += "  		, TO_CHAR(A.BLOODDATE,'YYYY-MM-DD HH24:MI') 							    AS BLOODDATE	-- 13       \r\n";
            SQL += "  		, TO_CHAR(A.BDATE,'YYYY-MM-DD') 										    AS BDATE        -- 14       \r\n";
            SQL += "  		, TO_CHAR(A.RESULTDATE,'YYYY-MM-DD HH24:MI')							    AS RESULTDATE   -- 15       \r\n";
            SQL += "  		, A.PANO        														    AS PANO			-- 16       \r\n";
            SQL += "  		, A.STATUS        														    AS STATUS       -- 17       \r\n";
            SQL += "  		, ADMIN.FC_EXAM_SPECMST_NM('21', A.CANCEL,'N')   				        AS CANCEL_NM    -- 18       \r\n";
            SQL += "  		, A.IPDOPD                                              			        AS IPDOPD       -- 19       \r\n";

            if (isPB == true)
            {
                SQL += "    , (                                                 \r\n";
                SQL += "          SELECT CASE WHEN COUNT(*) > 0 THEN 'Y'        \r\n";
                SQL += "                  ELSE 'N'                              \r\n";
                SQL += "                  END                                   \r\n";
                SQL += "           FROM ADMIN.EXAM_RESULTC B                          \r\n";
                SQL += "          WHERE B.SPECNO = A.SPECNO                     \r\n";
                SQL += "            AND B.SUBCODE = 'HR10'                      \r\n";
                SQL += "        )                                                                      AS PB   --20                 \r\n";

            }
            else
            {
                SQL += "     , ''                                                                      AS PB  --20                  \r\n";
            }

            SQL += "  		, A.CANCEL                                             			           AS CANCEL       -- 21        \r\n";
            SQL += "    FROM ADMIN.EXAM_SPECMST A                                                                              \r\n";
            SQL += "   WHERE 1=1                                                                                                    \r\n";

            if (string.IsNullOrEmpty(strPANO.Trim()) == false)
            {
                SQL += "     AND A.PANO = " + ComFunc.covSqlstr(strPANO, false);
            }

            // 미완료
            if (isEXCP == true)
            {
                SQL += "     AND A.RECEIVEDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
                SQL += "   				           AND " + ComFunc.covSqlDate(strTDATE, false) + "+1 \r\n                           ";
                SQL += "     AND A.STATUS NOT IN ('00','05','06') 				-- 미완료만                                         \r\n";
            }
            else
            {
                SQL += "     AND A.BDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
                SQL += "   			 	     AND " + ComFunc.covSqlDate(strTDATE, false);
            }

            if (string.IsNullOrEmpty(strSNAME.Trim()) == false)
            {
                SQL += "     AND A.SNAME LIKE '" + strSNAME.Trim() + "%'";
            }

            SQL += "     AND (A.GB_GWAEXAM IS NULL OR A.GB_GWAEXAM <> 'Y')    -- 응급실검사(과)                                     \r\n";

            if (string.IsNullOrEmpty(strWS.Trim()) == false && strWS.Equals("*") == false)
            {
                SQL += "     AND A.WORKSTS LIKE '%" + strWS.Trim() + "%'";
            }

            // bool isIPD, bool isOPD, bool is61, bool is62
            if (isIPD == true || isOPD == true )
            {
                SQL += "     AND (   \r\n";

                if (isIPD == true)
                {
                    SQL += "        A.IPDOPD ='I' OR ";
                }

                if (isOPD == true)
                {
                    SQL += "       (A.IPDOPD='O' AND A.BI < '61') OR ";
                }

                SQL = SQL.Substring(0, SQL.Length - 3) + "\r\n";

                SQL += "           )  \r\n";
            }

            if (isEXCP == true)
            {
                SQL += "   ORDER BY A.RECEIVEDATE DESC,A.SPECNO                                                      \r\n";
            }
            else
            {
                SQL += "   ORDER BY A.SPECNO                                                                         \r\n";
            }


            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return ds;
        }

        void tProcess(string strFDATE, string strTDATE, string strPANO, string strSNAME, bool isEXCP, string strWS, string strWARD, bool isPB, bool isIPD, bool isOPD)
        {

            try
            {
                this.Invoke(new threadProcessDelegate(trunCircular), new object[] { true });
                this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = false));
                this.ss_EXAM_SPECMST.BeginInvoke(new System.Action(() => this.ss_EXAM_SPECMST.Enabled = false));

                DataSet ds = null;
                ds = sel_EXAM_SPECMST_RCP03(clsDB.DbCon, strFDATE, strTDATE, strPANO, strSNAME, isEXCP, strWS, strWARD, isPB, isIPD, isOPD);

                
                if (ComFunc.isDataSetNull(ds) == false)
                {
                    this.Invoke(new delegateSetSpdStyle(setSpdStyle), new object[] { this.ss_EXAM_SPECMST, ds, sSel_EXAM_SPECMST_RCP03, nSel_EXAM_SPECMST_RCP03 });
                }
                else
                {
                    this.Invoke(new delegateSetSpdStyle(setSpdStyle), new object[] { this.ss_EXAM_SPECMST, null, sSel_EXAM_SPECMST_RCP03, nSel_EXAM_SPECMST_RCP03 });
                }

                this.Invoke(new threadProcessDelegate(trunCircular), new object[] { false });
                this.ss_EXAM_SPECMST.BeginInvoke(new System.Action(() => this.ss_EXAM_SPECMST.Enabled = true));
                this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = true));

            }
            catch (Exception ex)
            {
                this.Invoke(new threadProcessDelegate(trunCircular), new object[] { false });
                this.ss_EXAM_SPECMST.BeginInvoke(new System.Action(() => this.ss_EXAM_SPECMST.Enabled = true));
                this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = true));

                ComFunc.MsgBox(ex.ToString());


            }
            string strSort = string.Empty;


        }
       
        delegate void threadProcessDelegate(bool b);
        void trunCircular(bool b)
        {
            this.barProgress.Visible = b;
            this.barProgress.IsRunning = b;
        }

        delegate void delegateSetSpdStyle(FpSpread spd, DataSet ds, string[] colName, int[] size);
        void setSpdStyle(FpSpread spd, DataSet ds, string[] colName, int[] size)
        {
            spd.ActiveSheet.ColumnHeader.Rows.Get(0).Height = 30;
            // 화면상의 정렬표시 Clear
            spd.ActiveSheet.ColumnHeader.Cells[0, 0, 0, spd.ActiveSheet.ColumnCount - 1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;

            spd.DataSource = ds;
            spd.ActiveSheet.ColumnCount = colName.Length;

            spd.TextTipDelay = 500;
            spd.TextTipPolicy = TextTipPolicy.Fixed;

            //spread.setSpdSort(spd, -1, true);

            //1. 스프레드 사이즈 설정
            if (ds == null || ds.Tables[0].Rows.Count == 0)
            {
                spd.ActiveSheet.RowCount = 0;
            }

            //2. 헤더 사이즈
            spread.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            spread.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);

            if (spd == this.ss_EXAM_SPECMST)
            {
                spread.setColStyle(spd, -1, (int)enmSel_EXAM_SPECMST_RCP03.CHK          , clsSpread.enmSpdType.CheckBox);
                spread.setColStyle(spd, -1, (int)enmSel_EXAM_SPECMST_RCP03.STATUS       , clsSpread.enmSpdType.Hide);
                spread.setColStyle(spd, -1, (int)enmSel_EXAM_SPECMST_RCP03.CANCEL       , clsSpread.enmSpdType.Hide);
                spread.setColStyle(spd, -1, (int)enmSel_EXAM_SPECMST_RCP03.CANCEL_NM    , clsSpread.enmSpdType.Hide);
                spread.setColStyle(spd, -1, (int)enmSel_EXAM_SPECMST_RCP03.PB           , clsSpread.enmSpdType.Hide);
                spread.setColStyle(spd, -1, (int)enmSel_EXAM_SPECMST_RCP03.IPDOPD       , clsSpread.enmSpdType.Hide);

                spd.ActiveSheet.Columns[-1].VerticalAlignment = CellVerticalAlignment.Center;
                spd.ActiveSheet.Columns[-1].HorizontalAlignment = CellHorizontalAlignment.Center;

                spd.ActiveSheet.Columns[(int)enmSel_EXAM_SPECMST_RCP03.WORKSTS].HorizontalAlignment = CellHorizontalAlignment.Left;
                spd.ActiveSheet.Columns[(int)enmSel_EXAM_SPECMST_RCP03.EXAM_NAME].HorizontalAlignment = CellHorizontalAlignment.Left;

                spread.setSpdFilter(spd, (int)enmSel_EXAM_SPECMST_RCP03.STATUS_NM, AutoFilterMode.EnhancedContextMenu, true);
                spread.setSpdFilter(spd, (int)enmSel_EXAM_SPECMST_RCP03.PANO, AutoFilterMode.EnhancedContextMenu, true);
                spread.setSpdFilter(spd, (int)enmSel_EXAM_SPECMST_RCP03.SNAME, AutoFilterMode.EnhancedContextMenu, true);
                spread.setSpdFilter(spd, (int)enmSel_EXAM_SPECMST_RCP03.WARD, AutoFilterMode.EnhancedContextMenu, true);
                spread.setSpdFilter(spd, (int)enmSel_EXAM_SPECMST_RCP03.ROOM, AutoFilterMode.EnhancedContextMenu, true);

                spread.setSpdFilter(spd, (int)enmSel_EXAM_SPECMST_RCP03.SPECNO, AutoFilterMode.EnhancedContextMenu, true);
                spread.setSpdFilter(spd, (int)enmSel_EXAM_SPECMST_RCP03.EXAM_NAME, AutoFilterMode.EnhancedContextMenu, true);

                UnaryComparisonConditionalFormattingRule unary;

                unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "취소", false);
                unary.BackColor = Color.FromArgb(255, 192, 192);
                spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSel_EXAM_SPECMST_RCP03.STATUS_NM, unary);

                unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "검사중", false);
                unary.BackColor = Color.FromArgb(192, 192, 255);
                spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSel_EXAM_SPECMST_RCP03.STATUS_NM, unary);

                unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "부분완료", false);
                unary.BackColor = Color.FromArgb(255, 255, 192);
                spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSel_EXAM_SPECMST_RCP03.STATUS_NM, unary);

                unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "미접수", false);
                unary.BackColor = Color.LightGray;
                spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSel_EXAM_SPECMST_RCP03.STATUS_NM, unary);
            }
        }

    }
}
