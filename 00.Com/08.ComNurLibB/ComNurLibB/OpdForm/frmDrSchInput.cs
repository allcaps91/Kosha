using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB;
using ComBase;
using ComLibB;


namespace ComNurLibB
{
    public partial class frmDrSchInput : Form
    {
        FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();

        ComFunc CF = new ComFunc();
        clsSpdNr cspd = new clsSpdNr();
        string strFDate = "", strTDate = "";

        clsNurArrayHuil[] cNur = null; // 일자정보 배열
        clsNurArraySch[] cNurSch = null; // sch 배열

        enum DrSchCol { Dept,DrName,DrCode,Change,GbSPC };
      
        public const int TCOL = 5;

        public frmDrSchInput()
        {
            InitializeComponent();
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        void frmDoctorSch_Load(object sender, EventArgs e)
        {

            //현재시간 날짜      
            ComFunc.ReadSysDate(clsDB.DbCon);

            //관리자체크
            SetMst();

            //
            SetEvent();

            //
            SetCombo();

            //
            SetClear();

            strFDate = cboYYMM.SelectedItem.ToString().Trim() + "-01";
            strTDate = clsNurse.READ_LASTDAY(strFDate);

            //스프레드
            SetSpread(ssList_Sheet1);

        }
        
        void SetEvent()
        {            
            
        }
 
        void SetCombo()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            
            int nYY, nMM;
            
            nYY = Convert.ToInt32(VB.Left(clsPublic.GstrSysDate, 4));
            nMM = Convert.ToInt32(VB.Mid(clsPublic.GstrSysDate, 6,2));

            cboYYMM.Items.Clear();

            for (i=1;i<=12;i++)
            {
                cboYYMM.Items.Add(ComFunc.SetAutoZero(nYY.ToString(), 4) + "-" + ComFunc.SetAutoZero(nMM.ToString(), 2));
                nMM++;
                if (nMM ==13)
                {
                    nYY++;
                    nMM = 1;
                }
            }

            cboYYMM.SelectedIndex = 0;

            // 과 세팅            
            cboDept.Items.Clear();
            cboDept1.Items.Clear();
            cboDept2.Items.Clear();
            cboDept3.Items.Clear();

            try
            {
                SQL = "";
                SQL = SQL + "SELECT  ";
                SQL = SQL + ComNum.VBLF + "  DEPTCODE, DEPTNAMEK  ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT";
                SQL = SQL + ComNum.VBLF + "   WHERE 1=1 ";
                SQL = SQL + ComNum.VBLF + "    AND DeptCode NOT IN ('II', 'RD' ) ";

                if(clsNurse.GstrHelpCode !="")
                {
                    if(clsNurse.GstrHelpCode.Length <=2)
                    {
                        SQL = SQL + ComNum.VBLF + "    AND DEPTCODE IN (SELECT DrDept1 FROM " + ComNum.DB_PMPA + "BAS_DOCTOR WHERE DrCode IN (" + clsNurse.GstrHelpCode + ") ) ";
                    }
                }

                SQL = SQL + ComNum.VBLF + "  ORDER BY PRINTRANKING ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                cboDept.Items.Add("**.전체");

                cboDept1.Items.Add("**");
                cboDept2.Items.Add("**");
                cboDept3.Items.Add("**");

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboDept.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim() + "." + dt.Rows[i]["DEPTNAMEK"].ToString().Trim());

                        if (dt.Rows[i]["DEPTCODE"].ToString().Trim() != "MD")
                        {
                            cboDept1.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim());
                            cboDept2.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim());
                            cboDept3.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim());
                        }
                    }

                }

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            cboDept.SelectedIndex = 0;
            cboDept1.SelectedIndex = 0;
            cboDept2.SelectedIndex = 0;
            cboDept3.SelectedIndex = 0;

        }

        //관리자 체크
        void SetMst()
        {
            btnRegist.Visible = false;
            lblmst.Text = "진료과별 의사 진료 스케쥴";

            btnRegist.Visible = true;
            lblmst.Text = "진료과별 의사 진료 스케쥴(등록관리자)";

            groupBox1.Height = 80;

        }
               
        bool SetArray()
        {            
            int nDay = 0;
            bool bchk = false;

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(a.JobDate,'DD') ILJA,a.HolyDay,a.TempHolyDay  ";            
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_JOB a";
                SQL = SQL + ComNum.VBLF + "  WHERE 1=1 ";            
                SQL = SQL + ComNum.VBLF + "   AND a.JobDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND a.JobDate<=TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                //strSql = strSql + ComNum.VBLF + "   AND HolyDay = '*' ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY a.JOBDATE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                cNur = new clsNurArrayHuil[0];

                string yoil = clsNurse.READ_YOIL(strFDate); // 처음 요일 체크

                if (dt.Rows.Count > 0)
                {
                    bchk = true; //휴일설정 데이타 존재해야 스케쥴 체크함

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        Array.Resize<clsNurArrayHuil>(ref cNur, cNur.Length + 1);

                        nDay = Convert.ToInt16(dt.Rows[i]["ILJA"].ToString());

                        cNur[i] = new clsNurArrayHuil(); //배열초기화

                        cNur[i].strDay = dt.Rows[i]["ILJA"].ToString().Trim();
                        cNur[i].nDay = Convert.ToInt16(dt.Rows[i]["ILJA"].ToString().Trim());
                        cNur[i].strHuil = dt.Rows[i]["HolyDay"].ToString().Trim();
                        cNur[i].strTHuil = dt.Rows[i]["TempHolyDay"].ToString().Trim();

                        cNur[i].strYoil = yoil;

                        yoil = clsNurse.READ_Next_Yoil(yoil);

                    }

                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
	            if (dt != null)
	            {
		            dt.Dispose();
		            dt = null;
	            }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
	            ComFunc.MsgBox(ex.Message);
            }
            return bchk;
        }

        bool SetArray2(string argDrCode)
        {
            int j = 0;            
            int nDay = 0;
            bool SchChk = false;
            string strYoil = "";
            int nLastDay = 0;
            string strSun = "";

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            nLastDay = Convert.ToInt16(VB.Right(strTDate, 2));
            cNurSch = new clsNurArraySch[0];


            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(SchDate,'DD') ILJA,GbJin, GbJin2, GbJin3 ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_SCHEDULE a";
                SQL = SQL + ComNum.VBLF + "  WHERE 1=1 ";
                SQL = SQL + ComNum.VBLF + "   AND a.DrCode ='" + argDrCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND a.SchDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND a.SchDate<=TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY a.SchDate ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (dt.Rows.Count > 0)                               
                {
                    SchChk = true;
                }
     
                dt.Dispose();
                dt = null;


                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(a.SchDate,'DD') ILJA,a.GbJin, a.GbJin2, a.GbJin3,b.GbChoice AS GBSPC ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_SCHEDULE a, " + ComNum.DB_PMPA + "BAS_DOCTOR b " ;
                SQL = SQL + ComNum.VBLF + "  WHERE 1=1 ";
                SQL = SQL + ComNum.VBLF + "   AND a.DrCode = b.DrCode(+) ";
                SQL = SQL + ComNum.VBLF + "   AND a.DrCode ='" + argDrCode + "' ";
                if (SchChk == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND a.SchDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND a.SchDate<=TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND a.SchDate>=TO_DATE('1990-01-01','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND a.SchDate<=TO_DATE('1990-01-06','YYYY-MM-DD') ";
                }
                SQL = SQL + ComNum.VBLF + "  ORDER BY a.SchDate ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (dt.Rows.Count > 0)
                {
                    //기존스케쥴 있을경우
                    if (SchChk == true)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            Array.Resize<clsNurArraySch>(ref cNurSch, cNurSch.Length + 1);

                            nDay = Convert.ToInt16(dt.Rows[i]["ILJA"].ToString());

                            cNurSch[i] = new clsNurArraySch(); //배열초기화

                            cNurSch[i].strGbn1 = dt.Rows[i]["GbJin"].ToString().Trim();
                            cNurSch[i].strGbn2 = dt.Rows[i]["GbJin2"].ToString().Trim();
                            cNurSch[i].strGbn3 = dt.Rows[i]["GbJin3"].ToString().Trim();
                            cNurSch[i].strSPC = dt.Rows[i]["GbSPC"].ToString().Trim();

                        }
                    }
                    else
                    {
                        //신규스케쥴일 경우

                        strYoil = clsNurse.READ_YOIL(strFDate); //첫날요일체크
                        strSun = "";
                        if (strYoil == "일요일")
                        {
                            strYoil = clsNurse.READ_Next_Yoil(strYoil);
                            strSun = "OK";
                        }

                        for (i =0; i < nLastDay ; i++)
                        {
                            if (strSun == "OK")
                            {
                                Array.Resize<clsNurArraySch>(ref cNurSch, cNurSch.Length + 1);
                                cNurSch[i] = new clsNurArraySch(); //배열초기화
                                strSun = "";
                            }
                            else
                            {
                                Array.Resize<clsNurArraySch>(ref cNurSch, cNurSch.Length + 1);
                                cNurSch[i] = new clsNurArraySch(); //배열초기화

                                if (strYoil != "일요일")
                                {
                                    cNurSch[i].strGbn1 = dt.Rows[j]["GbJin"].ToString().Trim();
                                    cNurSch[i].strGbn2 = dt.Rows[j]["GbJin2"].ToString().Trim();
                                    cNurSch[i].strGbn3 = dt.Rows[j]["GbJin3"].ToString().Trim();
                                    cNurSch[i].strSPC = dt.Rows[j]["GbSPC"].ToString().Trim();

                                    j++;
                                    if (j == 6) j = 0;

                                }
                                                        

                                strYoil = clsNurse.READ_Next_Yoil(strYoil);

                            }

                        }


                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return SchChk;            

        }

        void SetSpread(FarPoint.Win.Spread.SheetView Spd)
        {
            string yoil =  clsNurse.READ_YOIL(strFDate);
            int nCnt = 0;

            nCnt = Convert.ToInt16( VB.Right(strTDate ,2)) ;

            Spd.RowCount = 0;
            Spd.RowCount = 1;

            Spd.ColumnCount = 0;
            Spd.ColumnCount = (nCnt * 3) + TCOL;

            Spd.ColumnHeader.RowCount = 3;

            Spd.Columns[3,Spd.ColumnCount-1].Width = 25;

            //컬럼 고정 및 색상
            Spd.FrozenColumnCount = 2;
            Spd.Columns[0, (Spd.RowCount - 1)].BackColor = Color.Empty;
            int col2 = 2;
            if ((col2 > 0))
            {
                Spd.Columns[0, (col2 - 1)].BackColor = Color.LightGray;
            }


            Spd.ColumnHeader.Cells.Get(0, 0, 2, Spd.ColumnCount - 1).BackColor = Color.LightGray;

            Spd.AddColumnHeaderSpanCell(0, (int)DrSchCol.Dept, 3, 1);            
            Spd.ColumnHeader.Cells[0, (int)DrSchCol.Dept].Value = "과";
            Spd.Columns[0].Width = 30;

            Spd.AddColumnHeaderSpanCell(0, (int)DrSchCol.DrName, 3, 1);
            Spd.ColumnHeader.Cells[0, (int)DrSchCol.DrName].Value = "의사";

            Spd.AddColumnHeaderSpanCell(0, (int)DrSchCol.DrCode, 3, 1);
            Spd.ColumnHeader.Cells[0, (int)DrSchCol.DrCode].Value = "의사코드";
            Spd.Columns[2].Visible = false;

            Spd.AddColumnHeaderSpanCell(0, (int)DrSchCol.Change, 3, 1);
            Spd.ColumnHeader.Cells[0, (int)DrSchCol.Change].Value = "수정";
            Spd.Columns[3].Visible =false;

            Spd.AddColumnHeaderSpanCell(0, (int)DrSchCol.GbSPC, 4, 1);
            Spd.ColumnHeader.Cells[0, (int)DrSchCol.GbSPC].Value = "선택진료";
            Spd.Columns[4].Visible = true;
            Spd.Columns[4].Width = 35;


            for (int i =0; i < nCnt; i++)
            {
                
                Spd.AddColumnHeaderSpanCell(0, (i * 2) + TCOL + i, 1, 3);
                Spd.ColumnHeader.Cells[0, (i * 2) + TCOL + i].Value = i+1;

                Spd.AddColumnHeaderSpanCell(1, (i * 2) + TCOL + i, 1, 3);
                Spd.ColumnHeader.Cells[1, (i * 2) + TCOL + i].Value = yoil;
                Spd.ColumnHeader.Cells[2, (i * 2) + TCOL + i].Value = "AM";
                Spd.ColumnHeader.Cells[2, (i * 2) + TCOL + 1 + i].Value = "PM";

                Spd.ColumnHeader.Cells[2, (i * 2) + TCOL + 2 + i].Value = "야";
                Spd.Columns[(i * 2) + TCOL + 2 + i].Visible = false;

                if (yoil == "일요일")
                {
                    Spd.ColumnHeader.Cells[0, (i * 2) + TCOL + i].BackColor = Color.DarkGray;
                    Spd.ColumnHeader.Cells[1, (i * 2) + TCOL + i].BackColor = Color.DarkGray;
                    Spd.ColumnHeader.Cells[2, (i * 2) + TCOL + i].BackColor = Color.DarkGray;
                    Spd.ColumnHeader.Cells[2, (i * 2) + TCOL + 1 + i].BackColor = Color.DarkGray;
                    Spd.ColumnHeader.Cells[2, (i * 2) + TCOL + 2 + i].BackColor = Color.DarkGray;
                }

                yoil = clsNurse.READ_Next_Yoil(yoil);

            }

        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            string[] str = new string[3];
                        
            strFDate = cboYYMM.SelectedItem.ToString().Trim() + "-01";
            strTDate = clsNurse.READ_LASTDAY(strFDate);

            //배열 설정
            if(SetArray() ==false )
            {
                MessageBox.Show("BAS_JOB 일자별 데이타 체크!! 전산정보팀 연락바람!!");
                return;
            }
            

            //시트 설정
            SetSpread(ssList_Sheet1);

            //
            str[0] = cboDept1.SelectedItem.ToString().Trim() + VB.Left(cboDr1.SelectedItem.ToString().Trim(), 4);
            str[1] = cboDept2.SelectedItem.ToString().Trim() + VB.Left(cboDr2.SelectedItem.ToString().Trim(), 4);
            str[2] = cboDept3.SelectedItem.ToString().Trim() + VB.Left(cboDr3.SelectedItem.ToString().Trim(), 4);

            //자료표시
            GetData(ssList_Sheet1,strFDate,strTDate,txtSearch.Text.Trim(),str[0], str[1], str[2]);

        }

        void GetData(FarPoint.Win.Spread.SheetView Spd,string ArgFDate , string ArgTDate, string ArgSearch ="",string ArgDr1="", string ArgDr2 = "", string ArgDr3 = "")
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            
            int j = 0;            
            string strDrCode = "";
            string strsql1 = "";
            int nLastDay = 0;

            btnRegist.Enabled = true;
            btnPrint.Enabled = true;
            cboDept.Enabled =false;
            cboYYMM.Enabled = false;
            
            nLastDay = Convert.ToInt16(VB.Right(ArgTDate, 2)) ;

            try
            {

                //스케쥴 있는지 체크
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT ";
                SQL = SQL + ComNum.VBLF + "  COUNT(*) CNT  ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_SCHEDULE ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1=1 ";
                SQL = SQL + ComNum.VBLF + "   AND SchDate>=TO_DATE('" + ArgFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND SchDate<=TO_DATE('" + ArgTDate + "','YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if ( Convert.ToInt16(dt.Rows[0]["CNT"].ToString()) == 0)
                {
                    if(MessageBox.Show("해당월의 스케쥴을 신규로 만드시겠습니까?","작업선택",MessageBoxButtons.YesNo) == DialogResult.No)
                    {

                        dt.Dispose();
                        dt = null;
                        return;
                    }
                }

                dt.Dispose();
                dt = null;
            
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT ";
                SQL = SQL + ComNum.VBLF + "  A.DrDept1, A.DrCode, A.DrName  ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_DOCTOR a, " +  ComNum.DB_PMPA + "BAS_CLINICDEPT b ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1=1 ";
                SQL = SQL + ComNum.VBLF + "   AND A.Tour <> 'Y' ";
                SQL = SQL + ComNum.VBLF + "   AND A.SCHEDULE IS  NULL ";
                if (VB.Left(cboDept.SelectedItem.ToString().Trim(), 2) != "**")
                {
                    SQL = SQL + ComNum.VBLF + " AND A.DRDEPT1 = '" + VB.Left(cboDept.SelectedItem.ToString().Trim(), 2) + "' ";
                }
                SQL = SQL + ComNum.VBLF + " AND A.DrDept1 NOT IN ('HD','HR','PT','TO','R6') ";
                SQL = SQL + ComNum.VBLF + " AND A.DRDEPT1 = B.DEPTCODE(+) ";

                //조건검색
                if(ArgSearch!="")
                {       
                    SQL = SQL + ComNum.VBLF + " AND (a.DrName Like '%" + ArgSearch + "%'  ";
                    SQL = SQL + ComNum.VBLF + "     OR UPPER(a.DrDept1) Like '%" + ArgSearch.ToUpper() + "%' ";
                    SQL = SQL + ComNum.VBLF + "     OR b.DEPTNAMEK Like '%" + ArgSearch + "%' ";
                    SQL = SQL + ComNum.VBLF + "     OR UPPER(b.DEPTNAMEE) Like '%" + ArgSearch.ToUpper() + "%' ";
                    SQL = SQL + ComNum.VBLF + "      ) ";
                }
            
                strsql1 = sqlMade(ArgDr1, ArgDr2, ArgDr3);

                SQL = SQL + ComNum.VBLF + strsql1;

                SQL = SQL + ComNum.VBLF + "  ORDER BY B.PrintRanking, A.DrDept1, decode(a.drcode,'1102',1,'1104',2,'1114',3,'1113', 4, '1108',5 ,'1111', 6,'1120',7 ,'1119',8, '1107', 9, A.PrintRanking )";
                SQL = SQL + ComNum.VBLF + " ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                Spd.RowCount = dt.Rows.Count;

                //시트 세팅
                Spd.Columns[0, Spd.ColumnCount - 1].CellType = textCellType1;
                Spd.Columns[0, Spd.ColumnCount - 1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                Spd.Columns[0, Spd.ColumnCount - 1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

                if (dt.Rows.Count > 0)
                {
                    Spd.RowCount = dt.Rows.Count;
                    Spd.SetRowHeight(-1, ComNum.SPDROWHT);
                 
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strDrCode= ComFunc.SetAutoZero(dt.Rows[i]["DrCode"].ToString().Trim(),4);

                        Spd.Cells[i, (int)DrSchCol.Dept].Text = dt.Rows[i]["DrDept1"].ToString().Trim();
                        Spd.Cells[i, (int)DrSchCol.DrName].Text = dt.Rows[i]["DrName"].ToString().Trim();
                        Spd.Cells[i, (int)DrSchCol.DrCode].Text = strDrCode;
                        if (VB.Right(strDrCode, 2) == "99") Spd.Rows[i].Visible = false; // ROW 숨김

                        //의사별 스케쥴 및 신규체크 
                        bool SchChk = SetArray2(strDrCode);


                        for (j = 0; j < cNurSch.Length; j++)
                        {
                            //신규는 등록 Y
                            Spd.Cells[i, (int)DrSchCol.Change].Value= "";

                            //선택의사표시
                            if (j == 0 && cNurSch[j].strSPC == "Y")
                            {
                                Spd.Cells[i, (int)DrSchCol.GbSPC].Value = ssColor_Sheet1.Cells[1, 5].Text;
                                Spd.Cells[i, (int)DrSchCol.GbSPC].BackColor = ssColor_Sheet1.Cells[1, 5].BackColor;
                            }

                            if (SchChk==false)
                            {
                                Spd.Cells[i, (int)DrSchCol.Change].Value = "Y"; //신규                            
                            }
                       
                            //오전
                            Spd.Cells[i, (j * 2) + TCOL + j].Value = cNurSch[j].strGbn1;
                            if (Spd.Cells[i, (j * 2) + TCOL + j].Text != "" && cNur[j].strYoil != "일요일")
                            {
                                Spd.Cells[i, (j * 2) + TCOL + j].BackColor = Read_Sch_Gbn("AM", cNurSch[j].strGbn1);
                            }

                            //오후
                            Spd.Cells[i, (j * 2) + TCOL + 1 + j].Value = cNurSch[j].strGbn2;
                            if (Spd.Cells[i, (j * 2) + TCOL + 1 + j].Text != "" && cNur[j].strYoil != "일요일")
                            {
                                Spd.Cells[i, (j * 2) + TCOL + 1 + j].BackColor = Read_Sch_Gbn("PM", cNurSch[j].strGbn2);
                            }

                            //야간
                            Spd.Cells[i, (j * 2) + TCOL + 2 + j].Value = cNurSch[j].strGbn3;
                            if (Spd.Cells[i, (j * 2) + TCOL + 2 + j].Text != "" && cNur[j].strYoil != "일요일")
                            {
                                Spd.Cells[i, (j * 2) + TCOL + 2 + j].BackColor = Read_Sch_Gbn("야간", cNurSch[j].strGbn3);
                            }
                      
                                                
                            //수정조건1 + 외래간호과장 권한 추가요망 //TODO : 윤조연 스케쥴권한 추가 + 영상의학과 판독관련 권한
                            if (( VB.Left(ArgFDate,8) +  ComFunc.SetAutoZero( Spd.ColumnHeader.Cells[0, (j * 2) + TCOL + j].Text,2)).CompareTo(clsPublic.GstrSysDate) < 0)
                            {
                                Spd.Cells[i, (j * 2) + TCOL + j].Locked = true;
                                Spd.Cells[i, (j * 2) + TCOL + 1 + j].Locked = true;
                                Spd.Cells[i, (j * 2) + TCOL + 2 + j].Locked = true;
                            }
                        }                    
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }            
        }

        Color Read_Sch_Gbn(string argGbn1, string argGbn2)
        {
     
            //오전
            if (argGbn1 == "AM")
            {
               
                if (argGbn2 == "1")
                {
                    return ssColor_Sheet1.Cells[0, 0].BackColor;
                }
                else if (argGbn2 == "2")
                {
                    return ssColor_Sheet1.Cells[0, 1].BackColor;
                }
                else if (argGbn2 == "3")
                {
                    return ssColor_Sheet1.Cells[0, 2].BackColor;
                }
                else if (argGbn2 == "9")
                {
                    return ssColor_Sheet1.Cells[1, 3].BackColor;
                }
                else if (argGbn2 == "A")
                {
                    return ssColor_Sheet1.Cells[0, 5].BackColor;
                }
                else
                {
                    return ssColor_Sheet1.Cells[0, 3].BackColor;
                }

            }

            //오후  
            else if (argGbn1 == "PM")
            { 
                if (argGbn2 == "1")
                {
                    return ssColor_Sheet1.Cells[0, 0].BackColor;
                }
                else if (argGbn2 == "2")
                {
                    return ssColor_Sheet1.Cells[0, 1].BackColor;
                }
                else if (argGbn2 == "3")
                {
                    return ssColor_Sheet1.Cells[0, 2].BackColor;
                }
                else if (argGbn2 == "9")
                {
                    return ssColor_Sheet1.Cells[1, 3].BackColor;
                }
                else if (argGbn2 == "A")
                {
                    return ssColor_Sheet1.Cells[0, 5].BackColor;
                }
                else
                {
                    return ssColor_Sheet1.Cells[0, 3].BackColor;
                }
            }          

            //야간  
            else if (argGbn1 == "야간")
            { 
                if (argGbn2 == "1")
                {
                    return ssColor_Sheet1.Cells[0, 0].BackColor;
                }
                else
                {
                    return  ssColor_Sheet1.Cells[0, 3].BackColor;
                }
            }

            else
            {
                return ssColor_Sheet1.Cells[0, 3].BackColor;
            }
            
            
        }

        string sqlMade(string str1,string str2,string str3)
        {
            string strTemp="";
            string strchk = "";

            strTemp = " AND ( " ;

            if (str1 != "******" && str1 != "")
            {
                if (VB.Mid(str1, 1, 2) != "**" && VB.Mid(str1, 3, 4) != "****")
                {
                    strTemp = strTemp + ComNum.VBLF + "  A.DrCode = '" + VB.Mid(str1, 3, 4) + "' OR";
                    strchk = "OK";
                }
                else if (VB.Mid(str1, 1, 2) != "**" && VB.Mid(str1, 3, 4) == "****")
                {
                    strTemp = strTemp + ComNum.VBLF + "  A.DrDept1 = '" + VB.Mid(str1, 1, 2) + "' OR";
                    strchk = "OK";
                }
            }

            if (str2 != "******" && str2 != "")
            {
                if (VB.Mid(str2, 1, 2) != "**" && VB.Mid(str2, 3, 4) != "****")
                {
                    strTemp = strTemp + ComNum.VBLF + "  A.DrCode = '" + VB.Mid(str2, 3, 4) + "'  OR";
                    strchk = "OK";
                }
                else if (VB.Mid(str2, 1, 2) != "**" && VB.Mid(str2, 3, 4) == "****")
                {
                    strTemp = strTemp + ComNum.VBLF + "  A.DrDept1 = '" + VB.Mid(str2, 1, 2) + "'  OR";
                    strchk = "OK";
                }
            }

            if (str3 != "******" && str3 != "")
            {
                if (VB.Mid(str3, 1, 2) != "**" && VB.Mid(str3, 3, 4) != "****")
                {
                    strTemp = strTemp + ComNum.VBLF + "  A.DrCode = '" + VB.Mid(str3, 3, 4) + "'  OR";
                    strchk = "OK";
                }
                else if (VB.Mid(str3, 1, 2) != "**" && VB.Mid(str3, 3, 4) == "****")
                {
                    strTemp = strTemp + ComNum.VBLF + "  A.DrDept1 = '" + VB.Mid(str3, 1, 2) + "'  OR";
                    strchk = "OK";
                }
            }

            if (strTemp != "") strTemp = VB.Mid(strTemp, 1, VB.Len(strTemp) - 3);

            strTemp = strTemp + "    ) ";

            if (strchk != "OK") strTemp = "";

            return strTemp;
        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            string[] strhead = new string[2];
            string[] strfont = new string[2];

            strfont[0] = "/fn\"바탕체\" /fz\"14\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strfont[1] = "/fn\"바탕체\" /fz\"12\" /fb0 /fi0 /fu0 /fk0 /fs2";

            strhead[0] = "/c/f1/n" + "의사별 진료 스케쥴" + "/f1/n";            
            strhead[1] = "/n/l/f2" + "진료년월 : " + cboYYMM.SelectedItem.ToString().Trim() + " /l/f2" + "  출력시간 : " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + " /n";

            cspd.SPREAD_PRINT(ssList_Sheet1, ssList, strhead, strfont, 10, 10,2,true);
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            SetClear();
        }

        void SetClear()
        {

            ssList_Sheet1.RowCount = 0;

            btnRegist.Enabled = false;
            btnPrint.Enabled = false;

            cboDept.Enabled = true;
            cboYYMM.Enabled = true;

            cboDept.SelectedIndex = 0;

            cboDept1.SelectedIndex = 0;
            cboDept2.SelectedIndex = 0;
            cboDept3.SelectedIndex = 0;

        }

        void SelCombo(ComboBox argcbo1, ComboBox argcbo2)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            argcbo2.Items.Clear();
            argcbo2.Items.Add("****.전체");
            argcbo2.SelectedIndex = 0;

            try
            {
                SQL = "";
                SQL = SQL + "SELECT  DrName,DrCode FROM " + ComNum.DB_PMPA + "BAS_DOCTOR  ";
                SQL = SQL + ComNum.VBLF + "WHERE 1=1 ";
                SQL = SQL + ComNum.VBLF + " AND TOUR <> 'Y' ";
                SQL = SQL + ComNum.VBLF + " AND SUBSTR(DrCode,3,2) <> '99' ";
                SQL = SQL + ComNum.VBLF + " AND DrDept1='" + argcbo1.SelectedItem.ToString().Trim() + "'  ";
                
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                
                if (dt.Rows.Count > 0)
                {
                    argcbo2.Items.Clear();
                    argcbo2.Items.Add("****.전체");
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        argcbo2.Items.Add(dt.Rows[i]["DrCode"].ToString().Trim() + "." + dt.Rows[i]["DrName"].ToString().Trim());
                    }
                    argcbo2.SelectedIndex = 0;
                }

                dt.Dispose();
                dt = null;

                //argcbo2.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }
        void cboDept1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelCombo(cboDept1, cboDr1);
        }

        void cboDept2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelCombo(cboDept2, cboDr2);
        }

        void cboDept3_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelCombo(cboDept3, cboDr3);
        }

        void btnSel_Click(object sender, EventArgs e)
        {
            SetClear();

            if (groupBox1.Height == 80 * 2)
            {
                groupBox1.Height = 80 ;
                btnSel.Text = "조건검색보기";
            }
            else
            {
                groupBox1.Height = 80 * 2;
                btnSel.Text = "조건검색닫기";
            }
        }          

        void btnRegist_Click(object sender, EventArgs e)
        {
            //스케쥴
            SaveData(ssList_Sheet1,strFDate,strTDate);

            //PC과 스케쥴
            Auto_Save_Sch("PC", strFDate, strTDate);

            //
            SetClear();

            //
            MessageBox.Show("스케쥴 저장이 완료되었습니다");

        }
        
        //스케쥴 등록
        bool SaveData(FarPoint.Win.Spread.SheetView Spd, string argFDate, string argTDate)
        {
            bool rtVal = false;
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            
            int j = 0;            
            string strChange = "";
            string strDrCode = "";
            int nLastDay = 0;
            string strDate = "";
            string strGbn1 = "", strGbn2 = "", strGbn3 = "";
            string strDayGbn = "";
            string strROWID = "";

            nLastDay = Convert.ToInt32( VB.Right(argTDate, 2));
            
            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return rtVal; ; //권한 확인
                
                for (i = 0; i < ssList_Sheet1.RowCount; i++)
                {
                    //신규는 등록 Y
                    strChange = "";strDrCode = "";
                    strChange = Spd.Cells[i, (int)DrSchCol.Change].Text.Trim();
                    strDrCode = Spd.Cells[i, (int)DrSchCol.DrCode].Text.Trim();

                    if (strChange == "Y")
                    {

                        for (j = 0; j < nLastDay; j++)
                        {

                            strDate = VB.Left( argFDate,8) +  ComFunc.SetAutoZero( (j+1).ToString(),2);

                            strDayGbn = "1";

                            if (cNur[j].strYoil== "토요일") strDayGbn = "2";
                            if (cNur[j].strHuil == "*") strDayGbn = "3";
                                                                        
                            strGbn1 = Spd.Cells[i, (j * 2) + TCOL + j].Text.Trim();//오전                        
                            strGbn2 = Spd.Cells[i, (j * 2) + TCOL + 1 + j].Text.Trim();//오후                        
                            strGbn3 = Spd.Cells[i, (j * 2) + TCOL + 2 + j].Text.Trim();//야간
                            
                            strROWID = Read_Sch_One_Data(strDate, strDrCode);

                            if (strROWID != "")
                            {
                                SQL = "";
                                SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "BAS_SCHEDULE  SET ";
                                SQL = SQL + ComNum.VBLF + "   GBJINEND = '', ";
                                SQL = SQL + ComNum.VBLF + "   GBDAY = '" + strDayGbn + "', ";
                                SQL = SQL + ComNum.VBLF + "   GBJIN = '" + strGbn1 + "', ";
                                SQL = SQL + ComNum.VBLF + "   GBJIN2 = '" + strGbn2 + "', ";
                                SQL = SQL + ComNum.VBLF + "   GBJIN3 = '" + strGbn3 + "' ";
                                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                    Cursor.Current = Cursors.Default;
                                    return rtVal;
                                }
                            }
                            else
                            {
                                SQL = "";
                                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "BAS_SCHEDULE ";
                                SQL = SQL + ComNum.VBLF + " ( DRCODE, SCHDATE, GBDAY,GBJINEND, GBJIN, GBJIN2, GBJIN3 ) ";
                                SQL = SQL + ComNum.VBLF + "VALUES ( ";
                                SQL = SQL + ComNum.VBLF + " '" + strDrCode + "',";
                                SQL = SQL + ComNum.VBLF + " TO_DATE('" + strDate + "','YYYY-MM-DD'), ";
                                SQL = SQL + ComNum.VBLF + " '" + strDayGbn + "',' ', ";
                                SQL = SQL + ComNum.VBLF + " '" + strGbn1 + "', '" + strGbn2 + "', '" + strGbn3 + "' ";
                                SQL = SQL + ComNum.VBLF + " ) ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                    Cursor.Current = Cursors.Default;
                                    return rtVal;
                                }

                            }                            
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }

        private void ssList_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {           
            if (e.Row < 0 || e.Column < 0) return;

            string strGubun = "";

            strGubun = ssList_Sheet1.Cells[e.Row, e.Column].Text.Trim();

            if (VB.Len(strGubun) > 1)
            {
                MessageBox.Show("입력값을 확인후 작업하세요!!");
                return;
            }

            if (strGubun == "1")
            {
                ssList_Sheet1.Cells[e.Row, e.Column].BackColor = ssColor_Sheet1.Cells[0, 0].BackColor;
            }
            else if (strGubun == "2")
            {
                ssList_Sheet1.Cells[e.Row, e.Column].BackColor = ssColor_Sheet1.Cells[0, 1].BackColor;
            }
            else if (strGubun == "3")
            {
                ssList_Sheet1.Cells[e.Row, e.Column].BackColor = ssColor_Sheet1.Cells[0, 2].BackColor;
            }
            else if (strGubun == "9")
            {
                ssList_Sheet1.Cells[e.Row, e.Column].BackColor = ssColor_Sheet1.Cells[1, 3].BackColor;
            }
            else if (strGubun == "A")
            {
                ssList_Sheet1.Cells[e.Row, e.Column].BackColor = ssColor_Sheet1.Cells[0, 5].BackColor;
            }
            else
            {
                ssList_Sheet1.Cells[e.Row, e.Column].BackColor = ssColor_Sheet1.Cells[0, 3].BackColor;
            }

            ssList_Sheet1.Cells[e.Row, (int)DrSchCol.Change].Text = "Y";
        }

        void cboYYMM_SelectedIndexChanged(object sender, EventArgs e)
        {            
         
        }

        string Read_Sch_One_Data(string argDate,string argDrCode)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strTemp = "";

            try
            {
                SQL = "";
                SQL = SQL + "SELECT  ";
                SQL = SQL + ComNum.VBLF + "  ROWID  ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_SCHEDULE " ;
                SQL = SQL + ComNum.VBLF + "  WHERE 1=1";
                SQL = SQL + ComNum.VBLF + "   AND DrCode = '" + argDrCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND SchDate = TO_DATE( '" + argDate + "','YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (dt.Rows.Count > 0)
                {
                    strTemp = dt.Rows[0]["ROWID"].ToString().Trim();

                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
            
            return strTemp;
        }

        //PC과 스케쥴 자동입력
        bool Auto_Save_Sch(string argDept, string argFDate, string argTDate)
        {
            bool rtVal = false;
            int i = 0;
            int j = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            string strTemp = "";
            string strDrCode = "";
            string strDate = "";
            int nLastDay = 0;
            
            nLastDay = Convert.ToInt16(VB.Right(argTDate, 2));
            strTemp = "";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return rtVal; ; //권한 확인

                SQL = "";
                SQL = SQL + "SELECT  ";
                SQL = SQL + ComNum.VBLF + "  ROWID  ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_SCHEDULE ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1=1";
                SQL = SQL + ComNum.VBLF + "   AND DrCode IN ( SELECT DRCODE FROM " + ComNum.DB_MED + "OCS_DOCTOR WHERE DEPTCODE ='" + argDept + "' AND GBOUT ='N' ) ";
                SQL = SQL + ComNum.VBLF + "   AND SchDate >= TO_DATE( '" + argFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND SchDate <= TO_DATE( '" + argTDate + "','YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (dt.Rows.Count > 0)
                {
                    strTemp = "OK";

                }

                dt.Dispose();
                dt = null;

                if (strTemp == "OK") return rtVal;

                SQL = "";
                SQL = SQL + "SELECT  DRCODE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR  ";
                SQL = SQL + ComNum.VBLF + " WHERE 1=1 ";
                SQL = SQL + ComNum.VBLF + "  AND DEPTCODE ='" + argDept + "' AND GBOUT ='N'  ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (dt.Rows.Count > 0)
                {
                    for (i=0;i<dt.Rows.Count;i++)
                    {
                        strDrCode = dt.Rows[i]["DrCode"].ToString().Trim();

                        for( j=0 ;j<nLastDay ; j++)
                        {

                            strDate = VB.Left(argFDate, 8) + ComFunc.SetAutoZero((j + 1).ToString(), 2);

                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "BAS_SCHEDULE ";
                            SQL = SQL + ComNum.VBLF + " ( DRCODE, SCHDATE, GBDAY, GBJIN, GBJIN2 ) ";
                            SQL = SQL + ComNum.VBLF + "VALUES ( ";

                            //
                            if (cNur[j].strHuil != "*" && cNur[j].strYoil == "토요일")
                            {
                                SQL = SQL + ComNum.VBLF + " '" + strDrCode + "',  TO_DATE('" + strDate + "','YYYY-MM-DD'),'2','1','4'    ";
                            }
                            else if (cNur[j].strHuil != "*" && cNur[j].strYoil != "토요일")
                            {
                                SQL = SQL + ComNum.VBLF + " '" + strDrCode + "',  TO_DATE('" + strDate + "','YYYY-MM-DD'),'1','1','1'    ";
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + " '" + strDrCode + "',  TO_DATE('" + strDate + "','YYYY-MM-DD'),'3','4','4'    ";
                            }

                            SQL = SQL + ComNum.VBLF + " ) ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                Cursor.Current = Cursors.Default;
                                return rtVal;
                            }                            
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;                
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }

        }


    }
}

