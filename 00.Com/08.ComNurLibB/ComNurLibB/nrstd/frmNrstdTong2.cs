using System;
using System.Data;
using System.Windows.Forms;
using ComBase;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmNrstdTong2.cs
    /// Description     : 통계2 - 각종통계
    /// Author          : 안정수
    /// Create Date     : 2018-01-26    
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 기존 FrmTong2_1.frm(FrmTong2_1) 폼 frmNrstdTong2.cs 으로 변경함
    /// </history>
    /// <seealso cref= "\nurse\nrstd\FrmTong2_1.frm(FrmTong2_1) >> frmNrstdTong2.cs 폼이름 재정의" />
    public partial class frmNrstdTong2 : Form, MainFormMessage
    {
        #region 클래스 선언 및 etc....

        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();


        string SQL = "";
        string SqlErr = "";
        DataTable dt = null;

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

        public frmNrstdTong2(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmNrstdTong2()
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

            this.optGbn21.CheckedChanged += new EventHandler(eControl_Changed);
            this.optGbn23.CheckedChanged += new EventHandler(eControl_Changed);
            this.optGbn24.CheckedChanged += new EventHandler(eControl_Changed);
            this.optGbn25.CheckedChanged += new EventHandler(eControl_Changed);

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

                optGbn11.Checked = true;
                optGbn21.Checked = true;
                optGbn22.Visible = false;

                Set_Year();
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
        }

        void eControl_Changed(object sender, EventArgs e)
        {
            if(sender == this.optGbn21)
            {
                txtTitle.Text = "";
            }

            else if (sender == this.optGbn23)
            {
                txtTitle.Text = "'암환자'통계는 퇴원일자 기준이며 의무기록실 퇴원상병 중 C코드를 바탕으로 조회됩니다.";
            }

            else if (sender == this.optGbn24)
            {
                txtTitle.Text = "";
            }

            else if (sender == this.optGbn25)
            {
                txtTitle.Text = "'수술'통계는 수술 후 입원 건수 및 Angio 시술 건수는 제외되어 있습니다.";
            }
        }

        void Set_Year()
        {
            int i = 0;

            COMBO_WARD_SET();

            cboYear.Items.Clear();

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  TO_CHAR(ADD_MONTHS(SYSDATE, -12 * (LEVEL - 1)),'YYYY') YYYY";
            SQL += ComNum.VBLF + "FROM DUAL";
            SQL += ComNum.VBLF + "CONNECT BY LEVEL < 12";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if(dt.Rows.Count > 0)
            {
                for(i = 0; i < dt.Rows.Count; i++)
                {
                    cboYear.Items.Add(dt.Rows[i]["YYYY"].ToString().Trim());
                }

                cboYear.SelectedIndex = 0;
            }

            dt.Dispose();
            dt = null;
        }

        void COMBO_WARD_SET()
        {
            int i = 0;
            //int j = 0;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT WardCode, WardName";
            SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.BAS_WARD";
            SQL = SQL + ComNum.VBLF + "WHERE WARDCODE NOT IN('33', '35', 'IU', 'NP', '2W', 'IQ', 'ER')";
            SQL = SQL + ComNum.VBLF + "AND USED = 'Y'";
            SQL = SQL + ComNum.VBLF + "ORDER BY WardCode";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                cboWard.Items.Clear();
                cboWard.Items.Add("전체");

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboWard.Items.Add(dt.Rows[i]["WardCode"].ToString().Trim());
                }

                cboWard.Items.Add("ER");
                cboWard.Items.Add("SICU");
                cboWard.Items.Add("MICU");
            }

            dt.Dispose();
            dt = null;

            cboWard.SelectedIndex = -1;

            foreach (string s in cboWard.Items)
            {
                if (s == clsNurse.gsWard)
                {
                    cboWard.SelectedText = clsNurse.gsWard;
                    cboWard.Enabled = false;
                    break;
                }
            }

            if (clsType.User.IdNumber != "")
            {
                if (NURSE_Manager_Check(Convert.ToInt64(clsType.User.IdNumber)) == true)
                //if (clsIpdNr.NURSE_Manager_Check(Convert.ToInt64(clsType.User.Sabun)) == true)
                {
                    cboWard.Enabled = true;
                    cboWard.SelectedIndex = 0;
                }
            }
        }

        void eGetData()
        {
            string strGubun = "";
            string strYYYY = "";
            string strWARD = "";

            int nSum = 0;

            int i = 0;
            int j = 0;

            string strOLD = "";

            if(optGbn11.Checked == true)
            {
                strGubun = "1";
            }

            else
            {
                strGubun = "2";
            }

            strYYYY = cboYear.SelectedItem.ToString().Trim();
            strWARD = cboWard.SelectedItem.ToString().Trim();

            ssList.ActiveSheet.Rows.Count = 0;

            if(optGbn21.Checked == true)
            {
                SQL = READ_GUBUN1(strGubun, strYYYY, strWARD);
            }

            else if(optGbn22.Checked == true)
            {
                SQL = READ_GUBUN2(strGubun, strYYYY, strWARD);
                return;
            }

            else if (optGbn23.Checked == true)
            {
                SQL = READ_GUBUN3(strGubun, strYYYY, strWARD);
            }

            else if (optGbn24.Checked == true)
            {
                SQL = READ_GUBUN4(strGubun, strYYYY, strWARD);
            }

            else if (optGbn25.Checked == true)
            {
                SQL = READ_GUBUN5(strGubun, strYYYY, strWARD);
            }

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if(dt.Rows.Count > 0)
            {
                for(i = 0; i < dt.Rows.Count; i++)
                {
                    if(strOLD != dt.Rows[i]["A"].ToString().Trim() || strOLD == "")
                    {
                        ssList.ActiveSheet.Rows.Count += 1;

                        strOLD = dt.Rows[i]["A"].ToString().Trim();
                        ssList.ActiveSheet.Cells[ssList.ActiveSheet.Rows.Count - 1, 0].Text = strOLD;
                    }

                    ssList.ActiveSheet.Cells[ssList.ActiveSheet.Rows.Count - 1, (1 + Convert.ToInt32(VB.Val(dt.Rows[i]["MM"].ToString().Trim())))].Text = VB.Val(dt.Rows[i]["CNT"].ToString().Trim()).ToString();
                }
            }

            dt.Dispose();
            dt = null;

            ssList.ActiveSheet.Rows.Count += 1;

            ssList.ActiveSheet.Cells[ssList.ActiveSheet.Rows.Count - 1, 0].Text = "합     계";

            for(i = 2; i < ssList.ActiveSheet.ColumnCount - 1; i++)
            {
                nSum = 0;
                for(j = 0; j < ssList.ActiveSheet.Rows.Count; j++)
                {
                    nSum = nSum + Convert.ToInt32(VB.Val(ssList.ActiveSheet.Cells[j, i].Text));
                }

                ssList.ActiveSheet.Cells[ssList.ActiveSheet.Rows.Count - 1, i].Text = nSum.ToString();
            }

            for(i = 0; i < ssList.ActiveSheet.Rows.Count; i++)
            {
                nSum = 0;
                for(j = 2; j < ssList.ActiveSheet.ColumnCount - 1; j++)
                {
                    nSum = nSum + Convert.ToInt32(VB.Val(ssList.ActiveSheet.Cells[i, j].Text));
                }

                ssList.ActiveSheet.Cells[i, ssList.ActiveSheet.ColumnCount - 1].Text = nSum.ToString();
            }

            ssList.ActiveSheet.Rows[0, ssList.ActiveSheet.Rows.Count - 1].Height = 20;
        }

        //입원자통계
        public string READ_GUBUN1(string arg, string argYYYY, string argWard)
        {
            string rtnVal = "";

            if(arg == "1")
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  INDEPT B, B.DEPTNAMEK A, TO_CHAR(A.INDATE, 'MM') MM, B.PRINTRANKING, SUM(1) CNT";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_MASTER A, " + ComNum.DB_PMPA + "BAS_CLINICDEPT B, " + ComNum.DB_PMPA + "IPD_NEW_MASTER C";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND A.INDATE >= TO_DATE('" + argYYYY + "-01-01','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "      AND A.INDATE <= TO_DATE('" + argYYYY + "-12-31','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "      AND A.INDEPT = B.DEPTCODE";
                SQL += ComNum.VBLF + "      AND A.IPDNO = C.IPDNO";

                if(argWard != "전체")
                {
                    SQL += ComNum.VBLF + "  AND A.INWARD IN (" + ReadInWard(argWard) + ")";
                }

                SQL += ComNum.VBLF + "      AND C.GBSTS NOT IN ('9')";
                SQL += ComNum.VBLF + "GROUP BY INDEPT, DEPTNAMEK, B.PRINTRANKING, TO_CHAR(A.INDATE, 'MM')";
                SQL += ComNum.VBLF + "ORDER BY B.PRINTRANKING";
            }

            else if(arg == "2")
            {
                //:1~10세/11~20세/21~ 30세/31~ 40세/41 ~ 50세/51~ 60세/61~ 70세/71세 이상

                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  AGE A,  MM, SUM(1) CNT ";
                SQL += ComNum.VBLF + "FROM (";
                SQL += ComNum.VBLF + "      SELECT";
                SQL += ComNum.VBLF + "        CASE ";
                SQL += ComNum.VBLF + "            WHEN A.AGE >= 0 AND A.AGE <= 10 THEN '1 ~ 10세'";
                SQL += ComNum.VBLF + "            WHEN A.AGE >= 11 AND A.AGE <= 20 THEN '11 ~ 20세'";
                SQL += ComNum.VBLF + "            WHEN A.AGE >= 21 AND A.AGE <= 30 THEN '21 ~ 30세'";
                SQL += ComNum.VBLF + "            WHEN A.AGE >= 31 AND A.AGE <= 40 THEN '31 ~ 40세'";
                SQL += ComNum.VBLF + "            WHEN A.AGE >= 41 AND A.AGE <= 50 THEN '41 ~ 50세'";
                SQL += ComNum.VBLF + "            WHEN A.AGE >= 51 AND A.AGE <= 60 THEN '51 ~ 60세'";
                SQL += ComNum.VBLF + "            WHEN A.AGE >= 61 AND A.AGE <= 70 THEN '61 ~ 70세'";
                SQL += ComNum.VBLF + "            WHEN A.AGE > 70 THEN '71세 이상'";
                SQL += ComNum.VBLF + "        END AGE, TO_CHAR(A.INDATE,'MM') MM";
                SQL += ComNum.VBLF + "      FROM " + ComNum.DB_PMPA + "NUR_MASTER A, " + ComNum.DB_PMPA + "IPD_NEW_MASTER C";
                SQL += ComNum.VBLF + "      WHERE 1=1";
                SQL += ComNum.VBLF + "        AND A.INDATE >= TO_DATE('" + argYYYY + "-01-01','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "        AND A.INDATE <= TO_DATE('" + argYYYY + "-12-31','YYYY-MM-DD')";

                if(argWard != "전체")
                {
                    SQL += ComNum.VBLF + "    AND A.INWARD IN (" + ReadInWard(argWard) + ")";
                }

                SQL += ComNum.VBLF + "        AND A.IPDNO = C.IPDNO";
                SQL += ComNum.VBLF + "        AND C.GBSTS NOT IN ('9'))";
                SQL += ComNum.VBLF + "GROUP BY AGE, MM";
                SQL += ComNum.VBLF + "ORDER BY AGE, MM";         
            }

            rtnVal = SQL;

            return rtnVal;
        }

        //재원자통계
        public string READ_GUBUN2(string arg, string argYYYY, string argWard)
        {            
            //본 소스에 구현되있지 않음
            return "";
        }

        //암환자
        public string READ_GUBUN3(string arg, string argYYMM, string argWard)
        {
            string rtnVal = "";

            if (arg == "1")
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  DEPTNAMEK A, TO_CHAR(OUTDATE, 'MM') MM, B.PRINTRANKING, SUM(1) CNT ";
                SQL += ComNum.VBLF + "FROM (";
                SQL += ComNum.VBLF + "      SELECT";
                SQL += ComNum.VBLF + "        B.DEPTCODE, A.PANO, A.OUTDATE, AGE ";                
                SQL += ComNum.VBLF + "      FROM " + ComNum.DB_PMPA + "MID_DIAGNOSIS A, " + ComNum.DB_PMPA + "IPD_NEW_MASTER B";
                SQL += ComNum.VBLF + "      WHERE 1=1";
                SQL += ComNum.VBLF + "        AND DIAGNOSIS1 LIKE 'C%'";
                SQL += ComNum.VBLF + "        AND A.PANO = B.PANO";
                SQL += ComNum.VBLF + "        AND A.OUTDATE = B.OUTDATE";
                SQL += ComNum.VBLF + "        AND B.GBSTS NOT IN ('9')";
                SQL += ComNum.VBLF + "        AND B.OUTDATE >= TO_DATE('" + argYYMM + "-01-01','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "        AND B.OUTDATE <= TO_DATE('" + argYYMM + "-12-31','YYYY-MM-DD')";

                if (argWard != "전체")
                {
                    SQL += ComNum.VBLF + "    AND B.WARDCODE IN (" + ReadInWard(argWard) + ")";
                }

                SQL += ComNum.VBLF + "      GROUP BY DEPTCODE, A.PANO, A.OUTDATE, AGE) A, " + ComNum.DB_PMPA + "BAS_CLINICDEPT B";
                SQL += ComNum.VBLF + "WHERE a.DeptCode = b.DeptCode";
                SQL += ComNum.VBLF + "GROUP BY DEPTNAMEK, TO_CHAR(OUTDATE, 'MM'), B.PRINTRANKING";
                SQL += ComNum.VBLF + "ORDER BY B.PRINTRANKING ";
            }

            else if (arg == "2")
            {
                //:1~10세/11~20세/21~ 30세/31~ 40세/41 ~ 50세/51~ 60세/61~ 70세/71세 이상

                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  AGE A,  MM, SUM(1) CNT ";
                SQL += ComNum.VBLF + "FROM (";
                SQL += ComNum.VBLF + "      SELECT";
                SQL += ComNum.VBLF + "        CASE ";
                SQL += ComNum.VBLF + "            WHEN AGE >= 0 AND AGE <= 10 THEN '1 ~ 10세'";
                SQL += ComNum.VBLF + "            WHEN AGE >= 11 AND AGE <= 20 THEN '11 ~ 20세'";
                SQL += ComNum.VBLF + "            WHEN AGE >= 21 AND AGE <= 30 THEN '21 ~ 30세'";
                SQL += ComNum.VBLF + "            WHEN AGE >= 31 AND AGE <= 40 THEN '31 ~ 40세'";
                SQL += ComNum.VBLF + "            WHEN AGE >= 41 AND AGE <= 50 THEN '41 ~ 50세'";
                SQL += ComNum.VBLF + "            WHEN AGE >= 51 AND AGE <= 60 THEN '51 ~ 60세'";
                SQL += ComNum.VBLF + "            WHEN AGE >= 61 AND AGE <= 70 THEN '61 ~ 70세'";
                SQL += ComNum.VBLF + "            WHEN AGE > 70 THEN '71세 이상'";
                SQL += ComNum.VBLF + "        END AGE, TO_CHAR(OUTDATE,'MM') MM";
                SQL += ComNum.VBLF + "      FROM ( ";
                SQL += ComNum.VBLF + "              SELECT";
                SQL += ComNum.VBLF + "                  A.PANO, B.OUTDATE, B.AGE";
                SQL += ComNum.VBLF + "              FROM " + ComNum.DB_PMPA + "MID_DIAGNOSIS A, " + ComNum.DB_PMPA + "IPD_NEW_MASTER B";
                SQL += ComNum.VBLF + "              WHERE 1=1";
                SQL += ComNum.VBLF + "                AND DIAGNOSIS1 LIKE 'C%'";
                SQL += ComNum.VBLF + "                AND B.OUTDATE >= TO_DATE('" + argYYMM + "-01-01','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "                AND B.OUTDATE <= TO_DATE('" + argYYMM + "-12-31','YYYY-MM-DD')";

                if (argWard != "전체")
                {
                    SQL += ComNum.VBLF + "            AND B.WARDCODE IN (" + ReadInWard(argWard) + ")";
                }

                SQL += ComNum.VBLF + "                AND A.PANO = B.PANO";
                SQL += ComNum.VBLF + "                AND B.GBSTS NOT IN ('9')";
                SQL += ComNum.VBLF + "                AND A.OUTDATE = B.OUTDATE";
                SQL += ComNum.VBLF + "              GROUP BY A.PANO, B.OUTDATE, B.AGE))";
                SQL += ComNum.VBLF + "GROUP BY AGE, MM";
                SQL += ComNum.VBLF + "ORDER BY AGE, MM";
            }

            rtnVal = SQL;

            return rtnVal;
        }

        //사망자
        public string READ_GUBUN4(string arg, string argYYMM, string argWard)
        {
            string rtnVal = "";

            if (arg == "1")
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  DEPTNAMEK A, TO_CHAR(OUTDATE, 'MM') MM, B.PRINTRANKING, SUM(1) CNT ";
                SQL += ComNum.VBLF + "FROM (";
                SQL += ComNum.VBLF + "      SELECT";
                SQL += ComNum.VBLF + "        PANO, DDATE OUTDATE, DEPTCODE ";
                SQL += ComNum.VBLF + "      FROM " + ComNum.DB_PMPA + "NUR_STD_DEATH";
                SQL += ComNum.VBLF + "      WHERE 1=1";                
                SQL += ComNum.VBLF + "        AND DDATE >= TO_DATE('" + argYYMM + "-01-01','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "        AND DDATE <= TO_DATE('" + argYYMM + "-12-31','YYYY-MM-DD')";

                if (argWard != "전체")
                {
                    SQL += ComNum.VBLF + "    AND WARDCODE IN (" + ReadInWard(argWard) + ")";
                }

                SQL += ComNum.VBLF + "  ) A, " + ComNum.DB_PMPA + "BAS_CLINICDEPT B";
                SQL += ComNum.VBLF + "WHERE A.DeptCode = B.DeptCode";
                SQL += ComNum.VBLF + "GROUP BY DEPTNAMEK, TO_CHAR(OUTDATE, 'MM'), B.PRINTRANKING";
                SQL += ComNum.VBLF + "ORDER BY B.PRINTRANKING ";
            }

            else if (arg == "2")
            {
                //:1~10세/11~20세/21~ 30세/31~ 40세/41 ~ 50세/51~ 60세/61~ 70세/71세 이상

                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  AGE A,  MM, SUM(1) CNT ";
                SQL += ComNum.VBLF + "FROM (";
                SQL += ComNum.VBLF + "      SELECT";
                SQL += ComNum.VBLF + "        CASE ";
                SQL += ComNum.VBLF + "            WHEN AGE >= 0 AND AGE <= 10 THEN '1 ~ 10세'";
                SQL += ComNum.VBLF + "            WHEN AGE >= 11 AND AGE <= 20 THEN '11 ~ 20세'";
                SQL += ComNum.VBLF + "            WHEN AGE >= 21 AND AGE <= 30 THEN '21 ~ 30세'";
                SQL += ComNum.VBLF + "            WHEN AGE >= 31 AND AGE <= 40 THEN '31 ~ 40세'";
                SQL += ComNum.VBLF + "            WHEN AGE >= 41 AND AGE <= 50 THEN '41 ~ 50세'";
                SQL += ComNum.VBLF + "            WHEN AGE >= 51 AND AGE <= 60 THEN '51 ~ 60세'";
                SQL += ComNum.VBLF + "            WHEN AGE >= 61 AND AGE <= 70 THEN '61 ~ 70세'";
                SQL += ComNum.VBLF + "            WHEN AGE > 70 THEN '71세 이상'";
                SQL += ComNum.VBLF + "        END AGE, TO_CHAR(OUTDATE,'MM') MM";
                SQL += ComNum.VBLF + "      FROM ( ";
                SQL += ComNum.VBLF + "              SELECT";
                SQL += ComNum.VBLF + "                  PANO, DDATE OUTDATE, AGE";
                SQL += ComNum.VBLF + "              FROM " + ComNum.DB_PMPA + "NUR_STD_DEATH";
                SQL += ComNum.VBLF + "              WHERE 1=1";                
                SQL += ComNum.VBLF + "                AND DDATE >= TO_DATE('" + argYYMM + "-01-01','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "                AND DDATE <= TO_DATE('" + argYYMM + "-12-31','YYYY-MM-DD')";

                if (argWard != "전체")
                {
                    SQL += ComNum.VBLF + "            AND WARDCODE IN (" + ReadInWard(argWard) + ")";
                }

                SQL += ComNum.VBLF + "            )) ";                
                SQL += ComNum.VBLF + "GROUP BY AGE, MM";
                SQL += ComNum.VBLF + "ORDER BY AGE, MM";
            }

            rtnVal = SQL;

            return rtnVal;
        }

        //수술
        public string READ_GUBUN5(string arg, string argYYMM, string argWard)
        {
            string rtnVal = "";

            if (arg == "1")
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  DEPTNAMEK A, TO_CHAR(OUTDATE, 'MM') MM, B.PRINTRANKING, SUM(1) CNT ";
                SQL += ComNum.VBLF + "FROM (";
                SQL += ComNum.VBLF + "      SELECT";
                SQL += ComNum.VBLF + "        PANO, OPDATE OUTDATE, DEPTCODE ";
                SQL += ComNum.VBLF + "      FROM " + ComNum.DB_PMPA + "ORAN_MASTER";
                SQL += ComNum.VBLF + "      WHERE 1=1";
                SQL += ComNum.VBLF + "        AND OpDate >= TO_DATE('" + argYYMM + "-01-01','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "        AND OpDate <= TO_DATE('" + argYYMM + "-12-31','YYYY-MM-DD')";

                if (argWard != "전체")
                {
                    SQL += ComNum.VBLF + "    AND WARDCODE IN (" + ReadInWard(argWard) + ")";
                }

                SQL += ComNum.VBLF + "        AND OpCancel IS NULL";
                SQL += ComNum.VBLF + "        AND IPDOPD = 'I'";
                SQL += ComNum.VBLF + "        AND (OPBUN IN ('1','2','3','4') OR OPBUN IS NULL)";
                SQL += ComNum.VBLF + "        AND (GbAngio IS NULL OR GbAngio <> 'Y')";                
                SQL += ComNum.VBLF + "      ) A, " + ComNum.DB_PMPA + "BAS_CLINICDEPT B";
                SQL += ComNum.VBLF + "WHERE a.DeptCode = b.DeptCode";
                SQL += ComNum.VBLF + "GROUP BY DEPTNAMEK, TO_CHAR(OUTDATE, 'MM'), B.PRINTRANKING";
                SQL += ComNum.VBLF + "ORDER BY B.PRINTRANKING ";
            }

            else if (arg == "2")
            {
                //:1~10세/11~20세/21~ 30세/31~ 40세/41 ~ 50세/51~ 60세/61~ 70세/71세 이상

                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  AGE A,  MM, SUM(1) CNT ";
                SQL += ComNum.VBLF + "FROM (";
                SQL += ComNum.VBLF + "      SELECT";
                SQL += ComNum.VBLF + "        CASE ";
                SQL += ComNum.VBLF + "            WHEN AGE >= 0 AND AGE <= 10 THEN '1 ~ 10세'";
                SQL += ComNum.VBLF + "            WHEN AGE >= 11 AND AGE <= 20 THEN '11 ~ 20세'";
                SQL += ComNum.VBLF + "            WHEN AGE >= 21 AND AGE <= 30 THEN '21 ~ 30세'";
                SQL += ComNum.VBLF + "            WHEN AGE >= 31 AND AGE <= 40 THEN '31 ~ 40세'";
                SQL += ComNum.VBLF + "            WHEN AGE >= 41 AND AGE <= 50 THEN '41 ~ 50세'";
                SQL += ComNum.VBLF + "            WHEN AGE >= 51 AND AGE <= 60 THEN '51 ~ 60세'";
                SQL += ComNum.VBLF + "            WHEN AGE >= 61 AND AGE <= 70 THEN '61 ~ 70세'";
                SQL += ComNum.VBLF + "            WHEN AGE > 70 THEN '71세 이상'";
                SQL += ComNum.VBLF + "        END AGE, TO_CHAR(OUTDATE,'MM') MM";
                SQL += ComNum.VBLF + "      FROM ( ";
                SQL += ComNum.VBLF + "              SELECT";
                SQL += ComNum.VBLF + "                  PANO, OPDATE OUTDATE, AGE";
                SQL += ComNum.VBLF + "              FROM " + ComNum.DB_PMPA + "ORAN_MASTER";
                SQL += ComNum.VBLF + "              WHERE 1=1";
                SQL += ComNum.VBLF + "                AND OpDate >= TO_DATE('" + argYYMM + "-01-01','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "                AND OpDate <= TO_DATE('" + argYYMM + "-12-31','YYYY-MM-DD')";

                if (argWard != "전체")
                {
                    SQL += ComNum.VBLF + "            AND WARDCODE IN (" + ReadInWard(argWard) + ")";
                }

                SQL += ComNum.VBLF + "                AND OpCancel IS NULL";
                SQL += ComNum.VBLF + "                AND IPDOPD = 'I'";
                SQL += ComNum.VBLF + "                AND (OPBUN IN ('1','2','3','4') OR OPBUN IS NULL)";
                SQL += ComNum.VBLF + "                AND (GbAngio IS NULL OR GbAngio <> 'Y')";
                SQL += ComNum.VBLF + "            )) ";
                SQL += ComNum.VBLF + "GROUP BY AGE, MM";
                SQL += ComNum.VBLF + "ORDER BY AGE, MM";
            }

            rtnVal = SQL;

            return rtnVal;
        }

        /// <summary>
        /// <seealso cref="Nrinfo.bas : NURSE_Manager_Check"/>
        /// </summary>
        /// <param name="ArgSabun"></param>
        /// <returns></returns>
        public bool NURSE_Manager_Check(long ArgSabun)
        {
            bool rtnVal = false;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  CODE";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND Gubun='NUR_간호부관리자사번'";
            SQL += ComNum.VBLF + "      AND Code=" + ArgSabun + "";
            SQL += ComNum.VBLF + "      AND DELDATE IS NULL";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }

            if (dt.Rows.Count > 0)
            {
                if (VB.Val(dt.Rows[0]["Code"].ToString().Trim()) > 0)
                {
                    rtnVal = true;
                }

                else
                {
                    rtnVal = false;
                }
            }

            else
            {
                rtnVal = false;
            }

            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        /// <summary>
        /// <seealso cref="Nrinfo.bas : ReadInWard"/>
        /// </summary>
        /// <param name="argWard"></param>
        /// <returns></returns>
        public string ReadInWard(string argWard)
        {
            //과거 병동 데이터 조회 되도록 프로그램
            //쿼리 사용시 IN으로 조회해야함.

            string rtnVal = "";
            int i = 0;
            DataTable dt1 = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  CODE ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND GUBUN = 'NUR_과거병동조회'";
            SQL += ComNum.VBLF + "      AND NAME = '" + argWard + "'";
            SQL += ComNum.VBLF + "      AND DELDATE IS NULL";

            SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }

            if(dt1.Rows.Count > 0)
            {
                for(i = 0; i < dt1.Rows.Count; i++)
                {
                    rtnVal = rtnVal + dt1.Rows[i]["CODE"].ToString().Trim() + "','";
                }

                rtnVal = "'" + rtnVal;
                rtnVal = VB.Mid(rtnVal, 1, (rtnVal.Length) - 2);
            }

            else
            {
                rtnVal = "'" + argWard + "'";
            }

            dt1.Dispose();
            dt1 = null;
            return rtnVal;
        }

    }
}

