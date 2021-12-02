using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.Com;
using ComSupLibB.SupXray;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComSupLibB.SupEnds
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupEnds
    /// File Name       : frmComSupEndsSCH02.cs
    /// Description     : 대장내시경 의사별 인원 스케쥴 설정
    /// Author          : 윤조연
    /// Create Date     : 2017-08-28
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\\\.frm() >> frmComSupEndsSCH02.cs 폼이름 재정의" />

    public partial class frmComSupEndsSCH02 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수        
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread methodSpd = new clsSpread();
        clsQuery Query = new clsQuery();
        clsComSup sup = new clsComSup();
        clsComSupXraySQL cSQL = new clsComSupXraySQL();
        clsSpread cspd = new clsSpread();
        clsComSupSpd ccomspd = new clsComSupSpd();
        clsComSupEndsSQL endsSql = new clsComSupEndsSQL();

        clsComSupEndsSQL.cENDO_DSCHEDULE cENDO_DSCHEDULE = null;

        clsSupSCHArray[] cSCH = null; // 일자정보 배열

        const int TCOL = 4;
        enum DrSchCol { Dept, DrName, DrCode, Change };

        string[] strDay = null;
        int nDrCNT = 6;
        
        #endregion

        public frmComSupEndsSCH02()
        {
            InitializeComponent();

            setEvent();
        }

        //기본값 세팅
        void setCtrlData()
        {       
            SetCombo();    
        }

        void setCtrlInit(PsmhDb pDbCon)
        {
            nDrCNT = 5;
                        
            clsCompuInfo.SetComputerInfo();
            DataTable dt = Query.Get_BasBcode(pDbCon, "C#_ENDO_대장스케쥴의사건수", "01", " Code || '.' || Name CodeName, Code ,Name   ");

            if (ComFunc.isDataTableNull(dt) == false)
            {
                if (dt.Rows.Count > 0 && dt.Rows[0]["Name"].ToString() !="")
                {
                    nDrCNT =  Convert.ToInt16(dt.Rows[0]["Name"].ToString());
                }                                               
                
            }
            
        }

        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnSearch1.Click += new EventHandler(eBtnEvent);
            this.btnCancel.Click += new EventHandler(eBtnEvent);
            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnSave.Click += new EventHandler(eBtnEvent);

            //this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.ssList.EditChange += new EditorNotifyEventHandler(eSpreadEditChange);

            this.cboYYMM.SelectedIndexChanged += new EventHandler(eCboEvent);

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

                screen_clear();

                setCtrlData();

                setCtrlInit(clsDB.DbCon);

                //SetSpread(ssList, date2cbo(cboYYMM.SelectedItem.ToString()));

                //기본 조회
                screen_display();
            }

        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == this.btnSave)
            {
                eSave( clsDB.DbCon, date2cbo(cboYYMM.SelectedItem.ToString()));
            }
            else if (sender == this.btnSearch1)
            {
                screen_display();
            }
            else if (sender == this.btnCancel)
            {
                screen_clear();
                ssList.ActiveSheet.RowCount = 0;
            }

        }
        
        void eCboEvent(object sender,EventArgs e)
        {
            if (sender == cboYYMM)
            {
                screen_display();
            }
        }

        void setSpread(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd,string argDate)
        {
            int i = 0;
            string yoil ="";
            int nCnt = 0;
            string strTDate = Convert.ToDateTime(VB.Left(Convert.ToDateTime(argDate).AddMonths(1).ToShortDateString(), 8) + "01").AddDays(-1).ToShortDateString();

            //휴일체크
            strDay = sup.read_huil(pDbCon, argDate, strTDate);

            nCnt =  Convert.ToInt16(VB.Right(strTDate, 2));

            Spd.ActiveSheet.RowCount = 0;
            Spd.ActiveSheet.RowCount = nDrCNT;

            Spd.ActiveSheet.ColumnCount = 0;
            Spd.ActiveSheet.ColumnCount = (nCnt * 3) + TCOL;

            methodSpd.setColAlign(Spd ,-1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColStyle(Spd, -1, -1, clsSpread.enmSpdType.Label);

            Spd.VerticalScrollBarWidth = 10;
            Spd.HorizontalScrollBarHeight = 10;

            Spd.ActiveSheet.ColumnHeader.RowCount = 3;

            Spd.ActiveSheet.Columns[3, Spd.ActiveSheet.ColumnCount - 1].Width = 22;

            //컬럼 고정 및 색상
            Spd.ActiveSheet.FrozenColumnCount = 2;
            Spd.ActiveSheet.Columns[0, (Spd.ActiveSheet.RowCount - 1)].BackColor = Color.Empty;
            int col2 = 2;
            if ((col2 > 0))
            {
                Spd.ActiveSheet.Columns[0, (col2 - 1)].BackColor = Color.LightGray;
            }


            Spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 2, Spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;

            Spd.ActiveSheet.AddColumnHeaderSpanCell(0, (int)DrSchCol.Dept, 3, 1);
            Spd.ActiveSheet.ColumnHeader.Cells[0, (int)DrSchCol.Dept].Value = "과";
            Spd.ActiveSheet.Columns[0].Width = 25;

            Spd.ActiveSheet.AddColumnHeaderSpanCell(0, (int)DrSchCol.DrName, 3, 1);
            Spd.ActiveSheet.ColumnHeader.Cells[0, (int)DrSchCol.DrName].Value = "의사";
            Spd.ActiveSheet.Columns[1].Width = 50;

            Spd.ActiveSheet.AddColumnHeaderSpanCell(0, (int)DrSchCol.DrCode, 3, 1);
            Spd.ActiveSheet.ColumnHeader.Cells[0, (int)DrSchCol.DrCode].Value = "의사코드";
            Spd.ActiveSheet.Columns[2].Visible = false;

            Spd.ActiveSheet.AddColumnHeaderSpanCell(0, (int)DrSchCol.Change, 3, 1);
            Spd.ActiveSheet.ColumnHeader.Cells[0, (int)DrSchCol.Change].Value = "수정";
            Spd.ActiveSheet.Columns[3].Visible = false;


            for (i = 0; i < nCnt; i++) 
            {

                if (cpublic.strSysDate.CompareTo( VB.Left(argDate,8) + ComFunc.SetAutoZero((i+1).ToString(),2)) <=0)
                {
                    methodSpd.setColStyle(Spd, -1, (i * 2) + TCOL + i, clsSpread.enmSpdType.Text);
                    methodSpd.setColStyle(Spd, -1, (i * 2) + TCOL + i+1, clsSpread.enmSpdType.Text);                    
                }

                yoil = clsVbfunc.GetYoIl(date2cbo(argDate,i+1));

                Spd.ActiveSheet.AddColumnHeaderSpanCell(0, (i * 2) + TCOL + i, 1, 3);
                Spd.ActiveSheet.ColumnHeader.Cells[0, (i * 2) + TCOL + i].Value = i + 1;

                Spd.ActiveSheet.AddColumnHeaderSpanCell(1, (i * 2) + TCOL + i, 1, 3);
                Spd.ActiveSheet.ColumnHeader.Cells[1, (i * 2) + TCOL + i].Value = VB.Left(yoil,1);
                Spd.ActiveSheet.ColumnHeader.Cells[2, (i * 2) + TCOL + i].Value = "AM";
                Spd.ActiveSheet.ColumnHeader.Cells[2, (i * 2) + TCOL + 1 + i].Value = "PM";

                Spd.ActiveSheet.ColumnHeader.Cells[2, (i * 2) + TCOL + 2 + i].Value = "야";
                Spd.ActiveSheet.Columns[(i * 2) + TCOL + 2 + i].Visible = false;

                if (yoil == "일요일" || strDay[i] == "*")
                {
                    Spd.ActiveSheet.ColumnHeader.Cells[0, (i * 2) + TCOL + i].BackColor = sup.SCH_Huil;
                    Spd.ActiveSheet.ColumnHeader.Cells[1, (i * 2) + TCOL + i].BackColor = sup.SCH_Huil;
                    Spd.ActiveSheet.ColumnHeader.Cells[2, (i * 2) + TCOL + i].BackColor = sup.SCH_Huil;
                    Spd.ActiveSheet.ColumnHeader.Cells[2, (i * 2) + TCOL + 1 + i].BackColor = sup.SCH_Huil;
                    Spd.ActiveSheet.ColumnHeader.Cells[2, (i * 2) + TCOL + 2 + i].BackColor = sup.SCH_Huil;
                }
                               
                
            }

        }

        void eSpreadEditChange(object sender, EditorNotifyEventArgs e)
        {
            FpSpread s = (FpSpread)sender;

            s.ActiveSheet.Cells[e.Row, (int)DrSchCol.Change].Text = "Y";

        }

        void eSave(PsmhDb pDbCon, string argDate)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strChange = "";
            string strDrName = "";
            int nLastDay =Convert.ToInt16(VB.Right(Convert.ToDateTime(VB.Left(Convert.ToDateTime(argDate).AddMonths(1).ToShortDateString(), 8) + "01").AddDays(-1).ToShortDateString(),2)) ;

            string strDate = "";

            // clsTrans DT = new clsTrans();


            clsDB.setBeginTran(pDbCon);

            try
            {
                for (int i = 0; i <= ssList.ActiveSheet.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
                {
                    strDrName = "";
                    strDrName = ssList.ActiveSheet.Cells[i, (int)DrSchCol.DrName].Text.Trim();
                    strChange = "";                    
                    strChange = ssList.ActiveSheet.Cells[i, (int)DrSchCol.Change].Text.Trim();

                    if (strChange == "Y" && strDrName !="")
                    {
                        for (int j = 0; j < nLastDay; j++)
                        {
                            strDate = VB.Left(argDate, 8) + ComFunc.SetAutoZero((j + 1).ToString(), 2);

                            cENDO_DSCHEDULE = new clsComSupEndsSQL.cENDO_DSCHEDULE();
                            cENDO_DSCHEDULE.SchDate = strDate;
                            cENDO_DSCHEDULE.DrCode = ComFunc.SetAutoZero(ssList.ActiveSheet.Cells[i, (int)DrSchCol.DrCode].Text.Trim(), 4);
                            cENDO_DSCHEDULE.Jin1 = ssList.ActiveSheet.Cells[i, (j * 2) + TCOL + j].Text.Trim();//오전                        
                            cENDO_DSCHEDULE.Jin2 = ssList.ActiveSheet.Cells[i, (j * 2) + TCOL + j + 1].Text.Trim();//오후                        

                            dt = sup.sel_BasSch(pDbCon, "01", cENDO_DSCHEDULE.DrCode, strDate, strDate);

                            if (ComFunc.isDataTableNull(dt) == false)
                            {
                                cENDO_DSCHEDULE.ROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                                SqlErr = endsSql.up_ENDO_DSCHEDULE(pDbCon, cENDO_DSCHEDULE, ref intRowAffected);
                            }
                            else
                            {
                                SqlErr = endsSql.ins_ENDO_DSCHEDULE(pDbCon, cENDO_DSCHEDULE, ref intRowAffected);
                            }

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                                return;
                            }
                        }

                    }

                }

                if (SqlErr == "")
                {
                    clsDB.setCommitTran(pDbCon);
                    ComFunc.MsgBox("자료가 저장되었습니다");
                }
                    

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                return;
            }
                   

            screen_clear();
            screen_display();

        }

        void SetCombo()
        {
            int i = 0;
            int nYY, nMM;

           
            nYY = Convert.ToInt16(VB.Left(cpublic.strSysDate, 4));
            nMM = Convert.ToInt16(VB.Mid(cpublic.strSysDate, 6, 2));
           

            cboYYMM.Items.Clear();

            for (i = 1; i <= 12; i++)
            {
                cboYYMM.Items.Add(ComFunc.SetAutoZero(nYY.ToString(), 4) + "-" + ComFunc.SetAutoZero(nMM.ToString(), 2));
                nMM++;
                if (nMM == 13)
                {
                    nYY++;
                    nMM = 1;
                }
            }

            cboYYMM.SelectedIndex = 0;                        

        }

        void screen_clear()
        {

            read_sysdate();

            //txtSearch.Text = "";   
            //dtpFDate.Text =cpublic.strSysDate;  

            //clear
            ssList.ActiveSheet.ClearRange(0, 0, (int)ssList.ActiveSheet.RowCount, (int)ssList.ActiveSheet.ColumnCount, true);

        }

        void screen_display()
        {
            string strDate = "";
            strDate = date2cbo(cboYYMM.SelectedItem.ToString());
            setSpread(clsDB.DbCon,ssList, strDate);
            GetData(clsDB.DbCon,ssList, strDate);

        }

        string date2cbo(string  argDate,int argDay =1)
        {
            return VB.Left(argDate,7) + "-" +  ComFunc.SetAutoZero(argDay.ToString(),2) ;
        }

        bool SetArray(PsmhDb pDbCon, string argDrCode,string argDate1,string argDate2)
        {
            bool bOK = true;

            DataTable dt = null;

            cSCH = new clsSupSCHArray[0];
            
            dt = sup.sel_BasSch(pDbCon, "00", argDrCode, argDate1, argDate2); //초기세팅은 의사스케쥴 참조

            if (ComFunc.isDataTableNull(dt) == false)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Array.Resize<clsSupSCHArray>(ref cSCH, cSCH.Length + 1);
                    cSCH[i] = new clsSupSCHArray();
                    cSCH[i].Day = dt.Rows[i]["ILJA"].ToString().Trim();
                    cSCH[i].Jin1 = dt.Rows[i]["GbJin"].ToString().Trim();
                    cSCH[i].Jin2 = dt.Rows[i]["GbJin2"].ToString().Trim();

                }
            }
            else
            {
                bOK = false;
            }

            dt = sup.sel_BasSch(pDbCon, "01", argDrCode, argDate1, argDate2);
           
            if (ComFunc.isDataTableNull(dt) == false)
            {             
                for (int i = 0; i < dt.Rows.Count; i++)
                {       
                    
                    cSCH[i].DJin1 = dt.Rows[i]["GbJin"].ToString().Trim();
                    cSCH[i].DJin2 = dt.Rows[i]["GbJin2"].ToString().Trim();                    

                }
            }
            

            return bOK;

        }

        void GetData(PsmhDb pDbCon,FarPoint.Win.Spread.FpSpread Spd,string argDate )
        {
            int i = 0;
            int j = 0;
            

            DataTable dt = null;
            DataTable dt2 = null;
            int nLastDay = 0;
            bool bAutoSet = false;
            bool bBase = false;
            bool bOK = true;
            string strDrCode = "";            
            string strTDate = "";
            strTDate = Convert.ToDateTime(VB.Left(Convert.ToDateTime(argDate).AddMonths(1).ToShortDateString(), 8) + "01").AddDays(-1).ToShortDateString();            
            
            nLastDay = Convert.ToInt16(VB.Right(strTDate, 2));
            if (chkBase.Checked == false)
            {
                dt2 = sup.sel_BasSch(pDbCon, "01", "", argDate, strTDate);
                if (ComFunc.isDataTableNull(dt2) == true)
                {
                    if (ComFunc.MsgBoxQ("해당월 대장내시경 스케쥴이 없습니다.." + "\r\n" +"특검일경우만 자동세팅 됩니다..?? ", "초기세팅", MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        return;
                    }
                    bAutoSet = true;
                }
            }
            else
            {
                bBase = true;
            }            

            string strOrdby = " a.endo_seq  ,B.PrintRanking, A.DrDept1  ,decode(a.drcode,'1102',1,'1104',2,'1114',3,'1113', 4, '1108',5 ,'1111', 6,'1120',7 ,'1119',8, '1107', 9, A.PrintRanking )  ";

            dt = endsSql.sel_BAS_DOCTOR(pDbCon, "","", strOrdby);

            if (ComFunc.isDataTableNull(dt) == false)
            {
                //2020-02-28 안정수 추가 
                if(dt.Rows.Count > nDrCNT)
                {
                    nDrCNT = dt.Rows.Count;
                    Spd.ActiveSheet.Rows.Count = nDrCNT;
                }

                for ( i = 0; i < dt.Rows.Count; i++)
                {
                    strDrCode = dt.Rows[i]["DrCode"].ToString().Trim();

                    bOK = SetArray(pDbCon, strDrCode, argDate, strTDate);
                    
                    Spd.ActiveSheet.Cells[i, TCOL + j].Text = "";
                    Spd.ActiveSheet.Cells[i, (int)DrSchCol.Dept].Text = dt.Rows[i]["DrDept1"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)DrSchCol.DrName].Text = dt.Rows[i]["DrName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)DrSchCol.DrCode].Text = strDrCode;

                    if (cSCH != null && cSCH.Length > 0)
                    {                                                
                        for (j = 0; j < nLastDay; j++)
                        {
                            //신규는 등록 Y
                            Spd.ActiveSheet.Cells[i, (int)DrSchCol.Change].Value = "";
                            if (chkBase.Checked == true || bAutoSet == true)
                            {
                                Spd.ActiveSheet.Cells[i, (int)DrSchCol.Change].Value = "Y";
                            }

                            if (bAutoSet==true)
                            {
                                if (cSCH[j].Jin1=="3")
                                {
                                    Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j].Value = "3";
                                }    
                                else
                                {
                                    Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j].Value = "0";
                                }
                                                                
                            }
                            else
                            {
                                if (bBase == true)
                                {
                                    if (cSCH[j].DJin1 =="" && strDay[j] != "*")
                                    {
                                        Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j].Value = "5";
                                    }
                                    else
                                    {
                                        Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j].Value = cSCH[j].DJin1;
                                    }
                                    
                                }
                                else
                                {
                                    Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j].Value = cSCH[j].DJin1;
                                }
                                
                            }
                            
                            if (optGubun1.Checked == true && strDay[j] == "*")
                            {
                                Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j].BackColor = sup.SCH_Huil;
                            }
                            else if (optGubun2.Checked == true && strDay[j] != "*")
                            {
                                if (cSCH[j].Jin1 == "1")
                                {
                                    Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j].BackColor = sup.SCH_1;
                                }
                                else if (cSCH[j].Jin1 == "2")
                                {
                                    Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j].BackColor = sup.SCH_2;
                                }
                                else if (cSCH[j].Jin1 == "3")
                                {
                                    Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j].BackColor = sup.SCH_3; 
                                }
                                else if (cSCH[j].Jin1 == "9" || cSCH[j].Jin1 == "6")
                                {
                                    //Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j].BackColor = sup.SCH_9;
                                    Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j].BackColor = Color.Pink;
                                }
                                else
                                {
                                    Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j].BackColor = sup.SCH_White;
                                }

                            }

                            if (bAutoSet == true)
                            {
                                if (cSCH[j].Jin2=="3")
                                {
                                    Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j + 1].Value = "3";
                                }
                                else
                                {
                                    Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j + 1].Value = "0";
                                }
                            }
                            else
                            {
                                if (bBase == true)
                                {
                                    if (cSCH[j].DJin2 =="" && strDay[j] != "*")
                                    {
                                        Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j + 1].Value = "5";
                                    }
                                    else
                                    {
                                        Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j + 1].Value = cSCH[j].DJin2;
                                    }
                                    
                                }
                                else
                                {
                                    Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j + 1].Value = cSCH[j].DJin2;
                                }
                                    
                                
                            }
                           
                                
                            if (optGubun1.Checked == true && strDay[j] == "*")
                            {
                                Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j + 1].BackColor = sup.SCH_Huil;
                            }
                            else if (optGubun2.Checked == true && strDay[j] != "*")
                            {
                                if (cSCH[j].Jin2 == "1")
                                {
                                    Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j + 1].BackColor = sup.SCH_1;
                                }
                                else if (cSCH[j].Jin2 == "2")
                                {
                                    Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j + 1].BackColor = sup.SCH_2;
                                }
                                else if (cSCH[j].Jin2 == "3")
                                {
                                    Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j + 1].BackColor = sup.SCH_3;
                                }
                                else if (cSCH[j].Jin2 == "9" || cSCH[j].Jin2 == "6")
                                {
                                    //Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j + 1].BackColor = sup.SCH_9;
                                    Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j + 1].BackColor = Color.Pink;
                                }
                                else
                                {
                                    Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j + 1].BackColor = sup.SCH_White;
                                }
                            }
                            
                        }
                    }

                }

            }

        }
        
        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

    }
}
