using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComLibB;
using FarPoint.Win.Spread;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaTongERCarMonth.cs
    /// Description     : 응급차량 월별 통계
    /// Author          : 안정수
    /// Create Date     : 2017-09-25
    /// Update History  : 2017-10-31
    /// TODO : 실제 서버에서 테스트 필요함
    /// <history>       
    /// d:\psmh\OPD\olrepa\Frm응급차량월별통계.frm(Frm응급차량월별통계) => frmPmpaTongERCarMonth.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\olrepa\Frm응급차량월별통계.frm(Frm응급차량월별통계)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaTongERCarMonth : Form
    {
        public delegate void EventExit();
        public event EventExit rEventExit;

        string FstrYYMM = "";
        string FstrFDate = "";
        string FstrTDate = "";
        string FstrCOMMIT = "";
        string[] FstrCar = new string[201];
        int[,] FnCnt = new int[3, 201];

        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();

        public frmPmpaTongERCarMonth()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnBuild.Click += new EventHandler(eBtnEvent);

        }

        void eFormLoad(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등            

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            Set_Init();
            eGetData();
        }

        void Set_Init()
        {
            int i = 0;
            int nYY = 0;
            int nMM = 0;

            string CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");

            nYY = Convert.ToInt32(VB.Left(CurrentDate, 4));
            nMM = Convert.ToInt32(VB.Mid(CurrentDate, 6, 2));

            cboBYYMM.Items.Clear();
            cboYYMM.Items.Clear();

            for(i = 1; i <= 12; i++)
            {
                cboBYYMM.Items.Add(ComFunc.SetAutoZero(nYY.ToString(), 4) + "-" + ComFunc.SetAutoZero(nMM.ToString(), 2));
                cboYYMM.Items.Add(ComFunc.SetAutoZero(nYY.ToString(), 4) + "-" + ComFunc.SetAutoZero(nMM.ToString(), 2));
                nMM -= 1;
                if(nMM == 0)
                {
                    nYY -= 1;
                    nMM = 12;
                }
            }

            cboBYYMM.SelectedIndex = 1;
            cboYYMM.SelectedIndex = 1;       
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                rEventExit();
                return;
            }

            else if (sender == this.btnPrint)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                ePrint();
            }

            else if (sender == this.btnView)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }                
                eGetData();
            }
            
            else if (sender == this.btnBuild)
            {
                eBuildData();
            }
        }

        //월말통계 형성여부를 Check
        public int Check_Month_Tong_Build()
        {
            int rtnVal = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            //월통계 형성여부를 Check
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                    ";
            SQL += ComNum.VBLF + "  COUNT(*) CNT                            ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "TONG_ER_CAR    ";
            SQL += ComNum.VBLF + "WHERE 1=1                                 ";
            SQL += ComNum.VBLF + "      AND YYMM='" + FstrYYMM + "'         ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return 0;
            }

            rtnVal = 0;

            if(dt.Rows.Count > 0)
            {
                rtnVal = Convert.ToInt32(dt.Rows[0]["CNT"].ToString().Trim());
            }

            return rtnVal;
        }

        void Month_ER_Car_Build()
        {
            int i = 0;
            int j = 0;
            int n = 0;
            int nRead = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

                         

            for (i = 0; i < FstrCar.Length; i++)
            {
                FstrCar[i] = "";
                for(j = 0; j < 3; j++)
                {
                    FnCnt[j, i] = 0;
                }
            }

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                            ";
            SQL += ComNum.VBLF + "  ErCar,COUNT(ERCAR) CNT,OutGbn                                                   ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_ER_PATIENT                                         ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                         ";
            SQL += ComNum.VBLF + "      AND InTime>=TO_DATE('" + FstrFDate + " 00:00" + "','YYYY-MM-DD HH24:MI')    ";
            SQL += ComNum.VBLF + "      AND InTime<=TO_DATE('" + FstrTDate + " 23:59" + "','YYYY-MM-DD HH24:MI')    ";
            SQL += ComNum.VBLF + "      AND ERCAR IS NOT NULL                                                       ";
            SQL += ComNum.VBLF + "GROUP By ErCar,OutGbn                                                             ";
            SQL += ComNum.VBLF + "ORDER By ErCar                                                                    ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    nRead = dt.Rows.Count;

                    for(i = 0; i < nRead; i++)
                    {
                        FstrCar[i] = dt.Rows[i]["ErCar"].ToString().Trim();
                        switch (dt.Rows[i]["OutGbn"].ToString().Trim())
                        {
                            //입원
                            case "1":
                            case "9":
                                n = 0;
                                break;

                            //외래
                            case "2":
                            case "3":
                            case "4":
                            case "8":
                                n = 1;
                                break;

                            //취소
                            case "5":
                            case "6":
                                n = 2;
                                break;

                            //나머지는 외래로...
                            default:
                                n = 1;
                                break;
                        }
                        FnCnt[n, i] = Convert.ToInt32(VB.Val(dt.Rows[i]["CNT"].ToString().Trim()));
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

            clsDB.setBeginTran(clsDB.DbCon);

            for(i = 0; i <= 200; i++)                
            {
                if(FstrCar[i] != "")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "TONG_ER_CAR ";
                    SQL += ComNum.VBLF + "(                                             ";
                    SQL += ComNum.VBLF + "YYMM, CODE, OPDCNT, IPDCNT, CANCNT            ";
                    SQL += ComNum.VBLF + ")                                             ";
                    SQL += ComNum.VBLF + "VALUES(                                       ";
                    SQL += ComNum.VBLF + "  '" + FstrYYMM + "',                         ";
                    SQL += ComNum.VBLF + "  '" + FstrCar[i] + "',                       ";
                    SQL += ComNum.VBLF + "  " + FnCnt[1, i] + ",                        ";
                    SQL += ComNum.VBLF + "  " + FnCnt[0, i] + ",                        ";
                    SQL += ComNum.VBLF + "  " + FnCnt[2, i] + "                         ";
                    SQL += ComNum.VBLF + ")                                             ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
            }

            clsDB.setCommitTran(clsDB.DbCon);
        }

        void ePrint()
        {            
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";

            string strYear = "";
            string strMonth = "";

            bool PrePrint = true;


            #region //시트 히든

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;
            //ssList.ActiveSheet.Cells[0, 9].Text = "zzz";
            //ssList.ActiveSheet.Columns[9].Visible = false;


            #endregion

            strYear = VB.Left(cboYYMM.SelectedItem.ToString().Trim(), 4);
            strMonth = VB.Right(cboYYMM.SelectedItem.ToString().Trim(), 2);

            strTitle = strYear + "년 " + strMonth + "월 관내 119 구급차 본원 이송 현황";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 80, 10, 50, 20);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, false, false, false, false, false);            

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            #region //시트 히든 복원

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;
            #endregion
        }

        void Screen_Clear()
        {
            int i = 0;
            int j = 0;

            for(i = 2; i < ssList_Sheet1.Rows.Count; i++)
            {
                for(j = 0; j < ssList_Sheet1.Columns.Count; j++)
                {
                    ssList_Sheet1.Cells[i, j].Text = "";
                }
            }
        }

        void eBuildData()
        {
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

                         

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            FstrYYMM = VB.Left(cboBYYMM.SelectedItem.ToString(), 4) + ComFunc.MidH(cboBYYMM.SelectedItem.ToString(), 6, 2);
            FstrFDate = VB.Left(FstrYYMM, 4) + "-" + VB.Right(FstrYYMM, 2) + "-01";
            FstrTDate = CF.READ_LASTDAY(clsDB.DbCon, FstrFDate);

            FstrCOMMIT = "OK";

            //월통계가 형성되었는지 점검
            if(Check_Month_Tong_Build() > 0)
            {
                if (MessageBox.Show("이미 월통계가 형성되었습니다." + "\r\n" + "월통계를 삭제하고 다시 작업하시겠습니까?", "작업선택", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    clsDB.setBeginTran(clsDB.DbCon);

                    //기존의 자료를 삭제함
                    SQL = "";
                    SQL += ComNum.VBLF + "DELETE " + ComNum.DB_PMPA + "TONG_ER_CAR  ";
                    SQL += ComNum.VBLF + "WHERE YYMM='" + FstrYYMM + "'             ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);

                    Month_ER_Car_Build();
                }
                else
                {
                    FstrCOMMIT = "NO";
                }
            }
            else
            {
                Month_ER_Car_Build();
            }

            if(FstrCOMMIT == "OK")
            {
                ComFunc.MsgBox("통계형성 완료");
            }
        }

        void eGetData()
        {
            int i = 0;
            int j = 0;
            int nRead = 0;
            int nRow = 0;
            int nCol = 0;
            int nCnt = 0;
            int nAvr = 0;

            int nTotal = 0;

            int[] nMonTot = new int[5];
            int[] nTot = new int[100];

            string strFYYMM = "";
            string strTYYMM = "";

            string strNewCode = "";
            string strOldCode = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Screen_Clear();

            for( i = 0; i < nMonTot.Length; i++)
            {
                nMonTot[i] = 0;
            }

            for( i = 0; i < nTot.Length; i++)
            {
                nTot[i] = 0;
            }

            strFYYMM = VB.Left(cboYYMM.SelectedItem.ToString(), 4) + "01";
            strTYYMM = VB.Left(cboYYMM.SelectedItem.ToString(), 4) + "12";

            nAvr = 0;
            nRow = 2;

            strNewCode = "";
            strOldCode = "";

            Screen_Clear();
            ssList_Sheet1.RowCount = 60;
            ssList_Sheet1.Cells[0, 15].Text = VB.Right(cboYYMM.SelectedItem.ToString(), 2) + "월";

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                        ";
            SQL += ComNum.VBLF + "  YYMM,b.nCode Code,SUM(NVL(OPDCNT,0)+NVL(IPDCNT,0)+NVL(CANCNT,0)) CNT,               ";
            SQL += ComNum.VBLF + "  SUM(NVL(OPDCNT,0)) OCNT,SUM(NVL(IPDCNT,0)) ICNT,SUM(NVL(CANCNT,0)) CCNT     ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "TONG_ER_CAR a ,ETC_ER_CAR b                                       ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
            SQL += ComNum.VBLF + "and a.code =  b.code                                                                     ";
            SQL += ComNum.VBLF + "      AND YYMM>='" + strFYYMM + "'                                            ";
            SQL += ComNum.VBLF + "      AND YYMM<='" + strTYYMM + "'                                            ";
            SQL += ComNum.VBLF + "GROUP By b.nCode,YYMM                                                            ";
            SQL += ComNum.VBLF + "ORDER By b.nCode,YYMM                                                            ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    nRead = dt.Rows.Count;
                    ssList_Sheet1.SetRowHeight(-1, 18)  ;
                    for (i = 0; i < nRead; i++)
                    {
                        nAvr += 1;

                        strNewCode = dt.Rows[i]["CODE"].ToString().Trim();

                        //if (strNewCode == "034")
                        //{
                        //    i = i;
                        //}

                        if(strNewCode != strOldCode)
                        {
                            if(strOldCode != "")
                            {

                                nRow += 1;
                                //ssList_Sheet1.RowCount += 1;
                            }
                            strOldCode = strNewCode;
                            nAvr = 1;
                        }

                        ssList_Sheet1.Cells[nRow, 0].Text = CF.READ_ERCAR_NAME(clsDB.DbCon, dt.Rows[i]["CODE"].ToString().Trim());

                        //해당월에 건수 표시
                        nCol = Convert.ToInt32(VB.Val(VB.Right(dt.Rows[i]["YYMM"].ToString().Trim(), 2)));
                        ssList_Sheet1.Cells[nRow, nCol].Text = VB.Val(dt.Rows[i]["CNT"].ToString().Trim()).ToString();

                        //월별합계 집계
                        nTot[nRow] += Convert.ToInt32(VB.Val(dt.Rows[i]["CNT"].ToString().Trim()));
                        ssList_Sheet1.Cells[nRow, 13].Text = nTot[nRow].ToString();

                        //월별평균
                        if(nAvr != 0)
                        {
                            int Div = Convert.ToInt32((nTot[nRow] / nAvr) + 0.5);
                            ssList_Sheet1.Cells[nRow, 14].Text = Div.ToString();
                        }
                        else
                        {
                            ssList_Sheet1.Cells[nRow, 14].Text = 0.ToString();
                        }

                        if (dt.Rows[i]["YYMM"].ToString().Trim() == cboYYMM.SelectedItem.ToString().Replace("-", ""))
                        {
                            ssList_Sheet1.Cells[nRow, 15].Text = VB.Val(dt.Rows[i]["CNT"].ToString().Trim()).ToString();
                            ssList_Sheet1.Cells[nRow, 16].Text = VB.Val(dt.Rows[i]["ICNT"].ToString().Trim()).ToString();
                            ssList_Sheet1.Cells[nRow, 17].Text = VB.Val(dt.Rows[i]["OCNT"].ToString().Trim()).ToString();
                            ssList_Sheet1.Cells[nRow, 18].Text = VB.Val(dt.Rows[i]["CCNT"].ToString().Trim()).ToString();
                            nAvr = 0;
                        }
                    }
                }
               
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;
            ssList_Sheet1.RowCount = nRow+2;
            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 0].Text = "합     계";
            ssList_Sheet1.Rows[ssList_Sheet1.Rows.Count - 1].BackColor = Color.LightGray;

            nCnt = 0;

            //마지막줄 합계 표시
            for (i = 1; i < ssList_Sheet1.Columns.Count; i++)
            {                
                for( j = 2; j < ssList_Sheet1.Rows.Count; j++)
                {
                    if(i != 14)
                    {                        
                        nCnt += Convert.ToInt32(VB.Val(ssList_Sheet1.Cells[j, i].Text));
                    }
                }

                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, i].Text = nCnt.ToString();
                nCnt = 0;

                if (i == 13 || i == 14)
                {
                    ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 13].Text = "";
                    ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 14].Text = "";
                }
            }

            



        }

    }
}
