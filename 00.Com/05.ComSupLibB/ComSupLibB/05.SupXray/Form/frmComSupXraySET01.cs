using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray
    /// File Name       : frmComSupXraySET01.cs
    /// Description     : 영상의학과 응급실 촬영기사 스케쥴 설정
    /// Author          : 윤조연
    /// Create Date     : 2017-07-03
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\xray\xuwork\Frm응급촬영기사설정.frm(Frm응급촬영기사설정) >> frmComSupXraySET01.cs 폼이름 재정의" />
    public partial class frmComSupXraySET01 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수
        conPatInfo pinfo = new conPatInfo(); //공통정보
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread methodSpd = new clsSpread();
        clsQuery cQuery = new clsQuery();
        clsComSup sup = new clsComSup();
        clsComSupSpd ccomspd = new clsComSupSpd();
        clsComSupXraySQL xraySql = new clsComSupXraySQL();

        string gstrSDate = "";
        string gstrTDate = "";
        
        //시트정보
        enum enmXCode { Gubun,Code,STime,ETime,Remark };
        string[] sSpdXCode = { "구분","코드","시작","종료","참고사항" };
        int[] nSpdXCode = { 30,50,40,40,190 };

        enum enmSch { Sabun, SName,Change };
        string[] sSpdSch = { "사번","성명","수정" };
        int[] nSpdSch = { 50,50,30 };

        #endregion

        public frmComSupXraySET01()
        {
            InitializeComponent();

            setEvent();
        }

        //기본값 세팅
        void setCtrlData(PsmhDb pDbCon)
        {
            //txtPano.Text = gstrPano;
            //lblSName.Text = gstrSName;
            
            setCombo(pDbCon);                      

        }

        void setCombo(PsmhDb pDbCon)
        {
            clsVbfunc.SetCboDate(pDbCon, cboYYYYMM, 10, "" , "2", 1);
            cboYYYYMM.SelectedIndex = 1;
        }
        
        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            
            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnSearch1.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);
            this.btnSave.Click += new EventHandler(eBtnEvent);


            this.ssList1.EditChange += new EditorNotifyEventHandler(eSpreadEditChange);
            this.ssList1.ClipboardPasted += new ClipboardPastedEventHandler(eSpreadClipboardPaste);

            this.exSpliter1.Click += new EventHandler(eSpliterClick);
            

        }

        void eSpliterClick(object sender, EventArgs e)
        {
            DevComponents.DotNetBar.ExpandableSplitter ex = (DevComponents.DotNetBar.ExpandableSplitter)sender;


            if (ex.Expanded == true)
            {             
                panLeft.Size = new System.Drawing.Size(60, 454);
            }
            else
            {                
                panLeft.Size = new System.Drawing.Size(330, 454);
            }
        }

        void eSpreadClipboardPaste(object sender, FarPoint.Win.Spread.ClipboardPastedEventArgs e)
        {  
            if (e.CellRange.Row < 0 || e.CellRange.Column < 0) return;
            FpSpread o = (FpSpread)sender;

            o.ActiveSheet.Cells[e.CellRange.Row, (int)enmSch.Change].Text = "Y";

        }

        void eSpreadEditChange(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Row < 0 || e.Column < 0) return;

            FpSpread o = (FpSpread)sender;

            o.ActiveSheet.Cells[e.Row, (int)enmSch.Change].Text = "Y";
        }               

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
            }
            else
            {
                //
                setSpd(ssList1, sSpdSch, nSpdSch, 1, 0);

                setSpd_Code(ssList2, sSpdXCode, nSpdXCode, 1, 0);
                ssList1.ActiveSheet.ColumnHeader.Rows[0].Height = 40;
                ssList1.ActiveSheet.Columns[(int)enmSch.Change].Visible = false;

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                screen_clear();

                setCtrlData(clsDB.DbCon);

                //
                screen_display();

                GetData_Code(clsDB.DbCon, ssList2);
            }
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
            else if (sender == this.btnSearch1)
            {
                //조회
                GetData(clsDB.DbCon, ssList1, cboYYYYMM.Text.Trim());
            }
            else if (sender == this.btnSave)
            {
                //저장
                eSave(clsDB.DbCon, cboYYYYMM.Text.Trim());
            }
            else if (sender == this.btnPrint)
            {
                //출력
                ePrint();
            }
            
        }

        void eSave(PsmhDb pDbCon, string argDate)
        {
            readDate2SDate(argDate); 

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strSabun = "";
            string strChange = "";
            string strDayGbn = "";
            string strDate = "";
            string strPartGbn = ""; 

            try
            {
                for (int i = 0; i <= ssList1.ActiveSheet.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
                {
                    strChange = ssList1.ActiveSheet.Cells[i, (int)enmSch.Change].Text.Trim();
                    strSabun = ssList1.ActiveSheet.Cells[i, (int)enmSch.Sabun].Text.Trim();
                    if (strChange == "Y")
                    {
                        for (int j = 0; j < Convert.ToInt16(VB.Right(gstrTDate, 2)); j++)
                        {

                            #region //데이타 체크
                            strDayGbn = ssList1.ActiveSheet.Cells[i, j + 3].Text.Trim().ToUpper();
                            strDate = VB.Left(gstrSDate, 8) + ComFunc.SetAutoZero((j + 1).ToString(), 2);

                            if (strDayGbn != "")
                            {
                                dt = cQuery.Get_BasBcode(pDbCon, "XRAY_응급촬영번표", strDayGbn);
                                if (dt != null && dt.Rows.Count == 0)
                                {
                                    MessageBox.Show("사번:" + strSabun + "  일자:" + strDate + " 스케쥴코드 : " + strDayGbn + " 오류입니다..");
                                    clsDB.setRollbackTran(pDbCon);
                                    return;
                                }
                            }
                            #endregion

                            #region 2018-11-07 안정수, CT기사

                            if(VB.Right(strDayGbn, 1) == "T")
                            {
                                strPartGbn = "02";
                            }
                            else
                            {
                                strPartGbn = "01";
                            }

                            #endregion

                            clsDB.setBeginTran(pDbCon);

                            #region //백업 스케쥴 저장
                            SQL = " INSERT INTO " + ComNum.DB_PMPA + "XRAY_GISA_HIS                         \r\n";
                            SQL += "   (BDATE,SABUN,PART,SCH,ENTDATE )                                      \r\n";
                            SQL += "  SELECT BDATE,SABUN,PART,SCH,ENTDATE                                   \r\n";
                            SQL += "    FROM " + ComNum.DB_PMPA + "XRAY_GISA                                \r\n";
                            SQL += "     WHERE 1=1                                                          \r\n";
                            SQL += "      AND SABUN='" + strSabun + "'                                      \r\n";
                            SQL += "      AND BDate =TO_DATE('" + strDate + "','YYYY-MM-DD')                \r\n";
                            #endregion

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                                return;
                            }
                            else
                            {
                                #region //기존 스케쥴 삭제
                                SQL = " DELETE " + ComNum.DB_PMPA + "XRAY_GISA                                  \r\n";
                                SQL += "     WHERE 1=1                                                          \r\n";
                                SQL += "      AND SABUN='" + strSabun + "'                                      \r\n";
                                SQL += "      AND BDate =TO_DATE('" + strDate + "','YYYY-MM-DD')                \r\n";
                                #endregion

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(pDbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                                    return;
                                }
                                else
                                {
                                    #region //스케쥴 저장
                                    SQL = " INSERT INTO " + ComNum.DB_PMPA + "XRAY_GISA                             \r\n";
                                    SQL += "     (SABUN,BDATE,PART,SCH,ENTDATE )    VALUES                          \r\n";
                                    SQL += "   (                                                                    \r\n";
                                    SQL += "  '" + strSabun + "',                                                   \r\n";
                                    SQL += "  TO_DATE('" + strDate + "','YYYY-MM-DD'),                              \r\n";
                                    //2018-11-07 안정수, 기존 01로 고정값으로 들어가는 부분을 CT와 분류하기 위하여 strPartGbn으로 변경함
                                    //SQL += "  '01', '" + strDayGbn + "', SYSDATE                                  \r\n";
                                    SQL += "  '" + strPartGbn +"', '" + strDayGbn + "', SYSDATE                     \r\n";
                                    SQL += "   )                                                                    \r\n";
                                    #endregion

                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(pDbCon);
                                        ComFunc.MsgBox(SqlErr);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                                        return;
                                    }
                                }
                            }

                            if (SqlErr == "")
                            {
                                clsDB.setCommitTran(pDbCon);
                            }

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                return;
            }

            
            //
            screen_display();

        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            strTitle = "응급촬영 기사 스케쥴 " + "(" + cboYYYYMM.Text.Trim() + ")";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, true);

            SPR.setSpdPrint(ssList1, PrePrint, setMargin, setOption, strHeader, strFooter);

        }

        void screen_clear()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
                                 
            
        }

        void screen_display()
        {
            GetData(clsDB.DbCon, ssList1, cboYYYYMM.Text.Trim());
        }

        void screen_display2()
        {
            GetData(clsDB.DbCon, ssList1, cboYYYYMM.Text.Trim());
        }

        void readDate2SDate(string argDate)
        {
            string strTemp = (argDate.Trim().Replace("년", "").Replace("월", "")).Replace(" ", "").Trim();
            gstrSDate = VB.Left(strTemp, 4) + "-" + VB.Mid(strTemp, 5, 2) + "-01";
            gstrTDate = Convert.ToString(Convert.ToDateTime(gstrSDate).AddMonths(1).AddDays(-1).ToShortDateString());
        }                      

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argDate)
        {
            int i = 0;
            int icol = 0;
            DataTable dt = null;
            DataTable dt2 = null;

            ssList1.Enabled = false;

            readDate2SDate(argDate); 

            Spd.ActiveSheet.RowCount = 0;
             
            //쿼리실행                  
            //dt = cQuery.Get_BasBcode(pDbCon, "XRAY_응급촬영기사", "", " Code,Name ","", " Code "); 
            dt = cQuery.Get_BasBcode(pDbCon, "C#_XRAY_촬영기사", "", " Code,Name ", " AND Part ='E' ", " Code ");

            setSpd(pDbCon, ssList1, gstrSDate, gstrTDate,dt.Rows.Count);
                        
            #region //데이터셋 읽어 자료 표시

            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                                        
                    Spd.ActiveSheet.Cells[i, (int)enmSch.Sabun].Text = dt.Rows[i]["Code"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)enmSch.SName].Text = dt.Rows[i]["Name"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)enmSch.Change].Text = "";

                    dt2 = xraySql.sel_XrayGisa(pDbCon, dt.Rows[i]["Code"].ToString().Trim(), gstrSDate, gstrTDate);
                    if (dt2 != null && dt2.Rows.Count > 0)
                    {
                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            icol = Convert.ToInt16(dt2.Rows[j]["ILJA"].ToString().Trim());
                            Spd.ActiveSheet.Cells[i, icol+2].Text = dt2.Rows[j]["SCH"].ToString().Trim();
                        }
                    }
                                                          

                }
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;

            #endregion


            ssList1.Enabled = true;
            
        }

        void GetData_Code(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd)
        {

            int i = 0;
            DataTable dt = null;
            string strTemp = "";
            string strNew = "", strOld = "";


            Spd.ActiveSheet.RowCount = 0;

            //쿼리실행                  
            dt = cQuery.Get_BasBcode(pDbCon, "XRAY_응급촬영번표", "", " Code,Name,Gubun2 ", "", " Sort,Code ");

            #region //데이터셋 읽어 자료 표시

            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strTemp = dt.Rows[i]["Name"].ToString().Trim();
                    strNew = clsComSup.setP(strTemp, " ", 3);

                    Spd.ActiveSheet.Cells[i, (int)enmXCode.Gubun].Text = dt.Rows[i]["Gubun2"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)enmXCode.Code].Text = dt.Rows[i]["Code"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)enmXCode.STime].Text = clsComSup.setP(strTemp, "^^", 1);
                    Spd.ActiveSheet.Cells[i, (int)enmXCode.ETime].Text = clsComSup.setP(strTemp, "^^", 2);
                    Spd.ActiveSheet.Cells[i, (int)enmXCode.Remark].Text = clsComSup.setP(strTemp, "^^", 3);

                    if (dt.Rows[i]["Gubun2"].ToString().Trim() == "토요일")
                    {
                        //ssList2.ActiveSheet.Rows.Get(i).BackColor = Color.FromArgb(255, 215, 255);
                        ssList2.ActiveSheet.Rows.Get(i).BackColor = Color.FromArgb(215, 255, 215);
                    }
                    else if (dt.Rows[i]["Gubun2"].ToString().Trim() == "휴일")
                    {                        
                        ssList2.ActiveSheet.Rows.Get(i).BackColor = Color.FromArgb(255, 172, 214);
                    }
                    else
                    {
                        ssList2.ActiveSheet.Rows.Get(i).BackColor = Color.FromArgb(255, 255, 255);
                    }

                    strOld = strNew;

                }
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;

            #endregion


        }

        void setSpd(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmSch)).Length;

            spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 30;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            spd.ActiveSheet.FrozenColumnCount = (int)enmSch.SName + 1;//컬럼고정

            //헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
                        

            //정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            //methodSpd.setColAlign(spd, (int)enmSch.Remark, clsSpread.HAlign_L, clsSpread.VAlign_C);

            //히든
            //methodSpd.setColStyle(spd, -1, (int)enmSch.Change, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmSch.Gubun, clsSpread.enmSpdType.Hide);

            // sort, filter            
            //methodSpd.setSpdSort(spd, (int)enmXCode.GbIO, true);
            //methodSpd.setSpdFilter(spd, (int)enmXCode.BDate, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);


        }

        void setSpd(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread spd,string sDate, string tDate, int RowCnt)
        {
            string[] colName = null;
            int[] size = null;
            string yoil = "";

            FarPoint.Win.Spread.CellType.TextCellType spdObj = new FarPoint.Win.Spread.CellType.TextCellType();
            spdObj.Multiline = false;

            //휴일체크
            string[] strDay = null;
            strDay = sup.read_huil(pDbCon, sDate, tDate);

            int nDay = Convert.ToInt16(VB.Right(tDate, 2)) + 3;

            colName = new string[nDay];
            size = new int[nDay];
                        

            spd.ActiveSheet.ColumnCount =nDay;
            spd.ActiveSheet.RowCount = RowCnt;
                        

            for (int i = 0; i < nDay; i++)
            {
                if (i < 3)
                {
                    colName[i] = sSpdSch[i];
                    size[i] = nSpdSch[i];
                }
                else
                {
                    yoil =  VB.Left( clsVbfunc.GetYoIl( VB.Left(sDate,8) + ComFunc.SetAutoZero( (i-2).ToString(),2)) ,1);
                    colName[i] = (i - 2) + "\r\n" + "("+ yoil + ")";
                    size[i] = 45;

                    if (VB.Left(strDay[i - 3], 1) == "일" || VB.Left(strDay[i - 3], 1) == "*")
                    {
                        spd.ActiveSheet.Columns.Get(i).BackColor = Color.FromArgb(255, 172, 214);
                    }
                    else if (VB.Left(strDay[i - 3], 1) == "토")
                    {
                        spd.ActiveSheet.Columns.Get(i).BackColor = Color.FromArgb(215, 255, 215);
                    }
                    else
                    {
                        spd.ActiveSheet.Columns.Get(i).BackColor = Color.FromArgb(255, 255, 255);
                    }

                    methodSpd.setColStyle(spd, -1, i, clsSpread.enmSpdType.Text);
                    methodSpd.setColAlign(spd, i, clsSpread.HAlign_C, clsSpread.VAlign_C);
                    
                    spd.ActiveSheet.Columns.Get(i).CellType = spdObj;
                                        
                }
            }
                        

            methodSpd.setHeader(spd, colName, size);
        }

        void setSpd_Code(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmXCode)).Length;

            spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 30;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            //methodSpd.setColStyle(spd, -1, (int)enmOrderview.Pano, clsSpread.enmSpdType.Text);

            //컬럼 머지
            methodSpd.setColMerge(spd, (int)enmXCode.Gubun);

            //정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmXCode.Remark, clsSpread.HAlign_L, clsSpread.VAlign_C);

            //히든
            //methodSpd.setColStyle(spd, -1, (int)enmXCode.Bun, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmXCode.Gubun, clsSpread.enmSpdType.Hide);

            // sort, filter            
            //methodSpd.setSpdSort(spd, (int)enmXCode.GbIO, true);
            //methodSpd.setSpdFilter(spd, (int)enmXCode.BDate, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            ////6.특정문구 색상
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;
            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "토요일", false);
            //unary.BackColor = Color.FromArgb(255, 215, 255); 
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmXCode.Gubun, unary); //구분

            
        }
    }
}
