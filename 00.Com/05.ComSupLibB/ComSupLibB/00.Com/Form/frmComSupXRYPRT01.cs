using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using System.Drawing.Printing;
using System.Drawing;
using FarPoint.Win.Spread;

namespace ComSupLibB.Com
{
    /// <summary>
    /// Class Name      : ComSupLibB.Com
    /// File Name       : frmComSupXRYPRT01.cs
    /// Description     : 처방의 판독지 대상자 출력
    /// Author          : 윤조연
    /// Create Date     : 2017-06-01
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 기존 FrmXrayPrint.frm(FrmXrayPrint) 폼 frmComSupXRYPRT01.cs 으로 변경함
    /// </history>
    /// <seealso cref= "\FrmXrayPrint.frm >> frmComSupXRYPRT01.cs 폼이름 재정의" />
    public partial class frmComSupXRYPRT01 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수
        clsComSupSpd csupspd = new clsComSupSpd();
        clsComSup sup = new clsComSup();

        clsComSupSpd.cXrayPrt cXrayPrt = null;


        string[] argArry = null; //쿼리에 사용될 변수 배열값
        public enum enumXrayResultDr2 { Pano,DeptCode,DrCode,chkDept,opt1,opt2,opt3 }

        string gstrPano = string.Empty;
        string gstrSName = string.Empty;
        string gstrSexAge = string.Empty;
        string gstrDept = string.Empty;
        string gstrDrCode = string.Empty;
        string gstrDrName = string.Empty;

        #endregion

        public frmComSupXRYPRT01()
        {
            InitializeComponent();
            setEvent();
        }
        
        /// <summary>
        /// 방사선 처방의 판독지 인쇄 폼 로드시 기본값 로드
        /// </summary>
        /// <param name="strPano"></param>
        /// <param name="strSName"></param>
        /// <param name="strSexAge"></param>
        /// <param name="strDept"></param>
        /// <param name="strDrCode"></param>
        /// <param name="strDrName"></param>
        public frmComSupXRYPRT01(string strPano, string strSName,string strSexAge, string strDept, string strDrCode, string strDrName)
        {
            InitializeComponent();

            gstrPano = strPano;
            gstrSName = strSName;
            gstrSexAge = strSexAge;
            gstrDept = strDept;
            gstrDrCode = strDrCode;
            gstrDrName = strDrName;

            setEvent();

        }                           

        void setCtrlData()
        {
            cXrayPrt = new clsComSupSpd.cXrayPrt();
            cXrayPrt.Pano = gstrPano;
            cXrayPrt.strSName = gstrSName;
            cXrayPrt.strSexAge = gstrSexAge;
            cXrayPrt.DeptCode = gstrDept;
            cXrayPrt.DrCode = gstrDrCode;
            cXrayPrt.DrName = gstrDrName;

            txtPano.Text = gstrPano;
            lblSName.Text = gstrSName;
            lblSex.Text = gstrSexAge;
            txtDeptCode.Text = gstrDept;
            lblDrName.Text = gstrDrName;

        }

        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new EventHandler(eFormLoad);

            this.btnSearch.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);
            this.btnExit.Click += new EventHandler(eBtnEvent);

            this.ssList.ButtonClicked += ssList_ButtonClicked;
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
                //            
                csupspd.sSpd_XrayReadDr(ssList, csupspd.sSpdXrayReadDr, csupspd.nSpdXrayReadDr, 5, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등           

                screen_clear();

                setCtrlData();

                //tSearch.Text = gSearch;

            }
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
            else if (sender == this.btnSearch)
            {
                //조회
                GetData(clsDB.DbCon, ssList);
            }
            else if (sender == this.btnPrint)
            {
                //출력
                ePrint();
            }
            
        }

        void ssList_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column != 0) return;

            if (e.Column == (int)clsComSupSpd.enmXrayReadDr.chk)
            {
                if (ssList.ActiveSheet.Cells[e.Row, (int)clsComSupSpd.enmXrayReadDr.chk].Text == "True")
                {
                    ssList.ActiveSheet.Rows.Get(e.Row).ForeColor = System.Drawing.Color.FromArgb(255,0,0);                    
                }
                else
                {
                    ssList.ActiveSheet.Rows.Get(e.Row).ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                }
            }

        }

        void screen_clear()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

            txtPano.Text = "";
            txtDeptCode.Text = "";
                        
            lblSName.Text = "";
            lblSex.Text = "";
            lblDrName.Text = "";

        }

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread spd)
        {

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            spd.ActiveSheet.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            
            try
            {
                #region  //배열 초기화 및 값 세팅
                argArry = new string[Enum.GetValues(typeof(enumXrayResultDr2)).Length];
                argArry[(int)enumXrayResultDr2.Pano] = txtPano.Text.Trim();

                argArry[(int)enumXrayResultDr2.DeptCode] = txtDeptCode.Text.Trim();
                argArry[(int)enumXrayResultDr2.DrCode] = gstrDrCode;
                argArry[(int)enumXrayResultDr2.opt1] = opt1.Checked.ToString();
                argArry[(int)enumXrayResultDr2.opt2] = opt2.Checked.ToString();
                argArry[(int)enumXrayResultDr2.chkDept] = ChkDept.Checked.ToString();

                dt = sel_XrayReadDr(pDbCon, argArry);

                #endregion

                #region //데이터셋 읽어 자료 표시
                if (dt == null)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    spd.ActiveSheet.RowCount = dt.Rows.Count;
                    spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);
   
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                       // If OptGB(0).Value = True Then
                       //   SS1.Col = 1: SS1.Text = "1"
                       //   SS1.Col = -1: SS1.ForeColor = RGB(255, 0, 0)


                       //Else
                       //   SS1.Col = 1: SS1.Text = ""
                       //End If
                        spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmXrayReadDr.chk].Text = ""; //TODO : 윤조연 체크
                        spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmXrayReadDr.InDate].Text = dt.Rows[i]["ENTERDATE"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmXrayReadDr.SeekDate].Text = dt.Rows[i]["SEEKDATE"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmXrayReadDr.ExCode].Text = dt.Rows[i]["XCode"].ToString().Trim();
                        if (dt.Rows[i]["ORDERNAME"].ToString().Trim()=="")
                        {
                            spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmXrayReadDr.ExName].Text = dt.Rows[i]["SUNAMEE"].ToString().Trim() + " " + dt.Rows[i]["REMARK"].ToString().Trim();
                        }
                        else
                        {
                            spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmXrayReadDr.ExName].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim() + " " + dt.Rows[i]["REMARK"].ToString().Trim();
                        }
                        spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmXrayReadDr.ReadDate].Text = dt.Rows[i]["DRDATE"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmXrayReadDr.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                        spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmXrayReadDr.chk].Locked =false;//TODO : 윤조연 체크
                        if (Convert.ToInt32( dt.Rows[i]["EXINFO"].ToString()) > 1 || Convert.ToInt32(dt.Rows[i]["DRWRTNO"].ToString()) > 1)
                        {
                            spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmXrayReadDr.chk].Locked = true;//TODO : 윤조연 체크
                            spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmXrayReadDr.Bigo].Text = "방사선과에서 판독완료";
                        }
                        if (dt.Rows[i]["XJONG"].ToString()== "3" || dt.Rows[i]["XJONG"].ToString() == "7" )
                        {
                            spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmXrayReadDr.chk].Locked = true;//TODO : 윤조연 체크
                            if (dt.Rows[i]["XJONG"].ToString() == "3") spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmXrayReadDr.Bigo].Text = "SONO는 제외";
                            if (dt.Rows[i]["XJONG"].ToString() == "7") spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmXrayReadDr.Bigo].Text = "BMD는 제외";
                        }
                                                

                    }
                }


                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                #endregion

            }
            catch (Exception ex)
            {
                //
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장                
            }
            
        }

        // 쿼리부분
        DataTable sel_XrayReadDr(PsmhDb pDbCon, string[] arg)
        {
            DataTable dt = null;            
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            SQL = "";
            SQL += " SELECT                                                                                                         \r\n";
            SQL += "  A.EXINFO, A.ENTERDATE, TO_CHAR(A.SEEKDATE,'YYYY/MM/DD') SEEKDATE, A.XCODE, A.DRWRTNO,                         \r\n";
            SQL += "      a.ORDERNAME, B.SUNAMEK XNAME, B.SUNAMEE,  A.REMARK, A.DRDATE, A.DRCODE, A.ROWID, A.XJONG                  \r\n";            
            SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_DETAIL a ,"  + ComNum.DB_PMPA + "BAS_SUN b                                   \r\n";
            SQL += "  WHERE 1 = 1                                                                                                   \r\n";
            SQL += "   AND A.PANO ='" + arg[(int)enumXrayResultDr2.Pano] + "'                                                       \r\n";
            SQL += "   AND (A.PACSSTUDYID IS NOT NULL OR XJONG ='A'                                                                 \r\n";
            SQL += "     OR (A.GBRESERVED ='7' AND XCode in ('G2702','G2702B'))  )                                                  \r\n";
            SQL += "   AND A.ENTERDATE >=TO_DATE('2007-08-01','YYYY-MM-DD')                                                         \r\n";
            if (arg[(int)enumXrayResultDr2.chkDept]=="True")
            {
                SQL += "  AND A.DEPTCODE = '" + arg[(int)enumXrayResultDr2.DeptCode] + "'                                           \r\n";
            }
            else
            {
                SQL += "  AND A.DrCode = '" + arg[(int)enumXrayResultDr2.DrCode] + "'                                             \r\n";
            }
            if (arg[(int)enumXrayResultDr2.opt1] == "True")
            {
                SQL += "  AND A.DRDATE IS NULL                                                                                      \r\n";
            }
            else if (arg[(int)enumXrayResultDr2.opt2] == "True")
            {
                SQL += "  AND A.DRDATE IS NOT NULL                                                                                  \r\n";
            }
            SQL += "   AND A.XCODE = B.SUNEXT                                                                                       \r\n";            
            SQL += " ORDER BY 2,1,3                                                                                                 \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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

            return dt;


        }
                
        #region 출력관련

        void ePrint()
        {
            int nCnt = 0;
            //선택체크
            for (int i = 0; i < ssList.ActiveSheet.RowCount; i++)
            {
                if (ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmXrayReadDr.chk].Text == "True")
                {
                    nCnt ++;                    
                }                
            }

            if (nCnt ==0 )
            {
                ComFunc.MsgBox("선택후 출력하세요");
                return;
            }
            else if (nCnt > 10)
            {
                ComFunc.MsgBox("10개 까지만 선택후 출력하세요");
                return;
            }

            DataTable dt = sel_EmrTreatt(clsDB.DbCon, cXrayPrt.Pano);
            if (ComFunc.isDataTableNull(dt) == false)
            {
                cXrayPrt.Gubun = "1";
            }

            cXrayPrt.strBDate = cpublic.strSysDate.Replace("-","/");
            
            setXray_DrRead_clear(ssPrt);
            if (setXray_DrRead(ssPrt, cXrayPrt) == true)
            {
                sup.SpreadPrint(this.ssPrt, "처방의판독지", false, 20, 20, "기본프린트");
            }

        }

        void setXray_DrRead_clear(FpSpread Spd)
        {
            #region 처방의 판독지

            Spd.ActiveSheet.Cells[1, 1].Text = "";

            Spd.ActiveSheet.Cells[4, 2].Text = "";
            Spd.ActiveSheet.Cells[4, 7].Text = "";
            Spd.ActiveSheet.Cells[4, 12].Text = "";

            Spd.ActiveSheet.Cells[6, 2].Text = "";
            Spd.ActiveSheet.Cells[6, 7].Text = "";
            Spd.ActiveSheet.Cells[6, 12].Text = "";

            for (int i = 9; i <= 20; i++)
            {
                Spd.ActiveSheet.Cells[i, 1].Text = "";
                Spd.ActiveSheet.Cells[i, 3].Text = "";
            }
            
            Spd.ActiveSheet.Cells[21, 1].Text = "";
            
            #endregion

        }

        public bool setXray_DrRead(FpSpread Spd, clsComSupSpd.cXrayPrt argCls)
        {
            bool b = true;
            int nCnt = 0;        
            string s = string.Empty;
            string sDate = "";
            string strROWID = "";

            #region 처방의 판독지

            //OCR 체크
            if (cXrayPrt.Gubun =="1")
            {
                Spd.ActiveSheet.Rows[1].Visible = true;
            }
            else
            {
                Spd.ActiveSheet.Rows[1].Visible =false;
            }
            

            for (int i = 0; i < argCls.DeptCode.Length; i++)
            {
                s += VB.Asc(VB.Mid(argCls.DeptCode,i+1,1)) ;
            }            

            Spd.ActiveSheet.Cells[1, 1].Text = ConvertString(argCls.Pano)  + "  " + s + "  " + ConvertString(cpublic.strSysDate.Replace("-","")) + "        " + "1";

            Spd.ActiveSheet.Cells[4, 2].Text = argCls.Pano;
            Spd.ActiveSheet.Cells[6, 2].Text = argCls.DeptCode;

            Spd.ActiveSheet.Cells[4, 7].Text = argCls.strSName;
            Spd.ActiveSheet.Cells[6, 7].Text = argCls.DrName;

            Spd.ActiveSheet.Cells[4, 12].Text = argCls.strSexAge;
            Spd.ActiveSheet.Cells[6, 12].Text = argCls.strBDate;


            for (int i = 0; i < ssList.ActiveSheet.RowCount; i++)
            {
                if (ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmXrayReadDr.chk].Text == "True")
                {
                    nCnt++;

                    strROWID = ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmXrayReadDr.ROWID].Text.Trim();

                    if(up_Xray_Detail(strROWID)!="")
                    {
                        ComFunc.MsgBox("상태갱신 오류!! - xray_detail drDate");
                        return false;
                    }

                    if (sDate =="")
                    {
                        sDate = ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmXrayReadDr.SeekDate].Text.Trim();
                    }
                    else
                    {
                        sDate = VB.Space(10);

                        if (ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmXrayReadDr.SeekDate].Text.Trim() !=sDate)
                        {
                            sDate = ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmXrayReadDr.SeekDate].Text.Trim();
                        }
                        
                    }                    

                    Spd.ActiveSheet.Cells[nCnt + 8, 1].Text = sDate;
                    Spd.ActiveSheet.Cells[nCnt + 8, 3].Text = ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmXrayReadDr.ExName].Text.Trim(); 

                }

                

            }

            Spd.ActiveSheet.Cells[21, 1].Text = "결과 보고일 : " + cpublic.strSysDate.Replace("-", "/") + VB.Space(10) + " 판독의사: " + argCls.DrName + VB.Space(20) + "포항성모병원";
            

            #endregion

            return b;
        }

        #endregion

        #region 사용안함
        void sPrint()
        {
            //DataTable dt = null;

            //dt = sel_EmrTreatt(txtPano.Text.Trim());

            //if (dt == null)
            //{                                
            //    return;
            //}
            
            //TODO 윤조연 출력관련 코딩

            PrintDocument Print = new PrintDocument();
            PrintController printController = new StandardPrintController();
            Print.PrintController = printController;  //기본인쇄창 없애기

            PageSettings ps = new PageSettings();
            ps.PrinterSettings.PrinterName = "PrimoPDF";
            ps.Margins = new Margins(10, 10, 10, 10);
            ps.Landscape = true;
            Print.DefaultPageSettings = ps;
            Print.PrinterSettings.PrinterName = "PrimoPDF";


            string s = ConvertString("---");
            string s2 = "";
            for (int i = 0; i < txtDeptCode.TextLength; i++)
            {
                s2 += VB.Asc(VB.Mid(txtDeptCode.Text, i + 1, 1));
            }


            //if (dt.Rows.Count > 0)
            //{
            //    Print.PrintPage += new PrintPageEventHandler(Xray_Print_OCR);
            //}
            //else
            //{
                Print.PrintPage += new PrintPageEventHandler(Xray_Print);
            //}
            
            Print.Print();

        }

        void Xray_Print(object sender, PrintPageEventArgs e)
        {            
            int intY = 0;
            int intX = 0;

            e.Graphics.DrawString("테스트", new Font("굴림체", 11, FontStyle.Bold), Brushes.Black, intX + 100, intY + 100);
            e.Graphics.DrawString("홍길동", new Font("굴림체", 11, FontStyle.Bold), Brushes.Black, intX + 100, intY + 300);
            e.Graphics.DrawString("내과", new Font("굴림체", 11, FontStyle.Bold), Brushes.Black, intX + 100, intY + 500);
            
        }

        void Xray_Print_OCR(object sender, PrintPageEventArgs e)
        {
            //TODO 윤조연 출력관련 코딩
        }

        #endregion

        string ConvertString(string str)
        {
            string s = string.Empty;

            for (int i = 0; i < str.Length; i++)
            {
                s += VB.Mid(str, i + 1, 1) + " ";
            }

            return s;
        }

        string up_Xray_Detail(string argROWID)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //상태 갱신
                SqlErr = sup.up_Xray_Detail(clsDB.DbCon, argROWID, ref intRowAffected);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장                            
                    return SqlErr;
                }
                else
                {
                    clsDB.setCommitTran(clsDB.DbCon);
                    return SqlErr;
                }
                
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return SqlErr;
            }            

        }

        DataTable sel_EmrTreatt(PsmhDb pDbCon, string argPano)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            SQL = "";
            SQL += " SELECT                                                                                                         \r\n";
            SQL += "  ROWID                                                                                                         \r\n";            
            SQL += "   FROM " + ComNum.DB_EMR + "EMR_TREATT                                                                         \r\n";
            SQL += "  WHERE 1 = 1                                                                                                   \r\n";
            SQL += "   AND PATID ='" + argPano + "'                                                                                 \r\n";            
            SQL += "   AND CHECKED ='1'                                                                                             \r\n";
            
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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

            return dt;
        }

    }
}
