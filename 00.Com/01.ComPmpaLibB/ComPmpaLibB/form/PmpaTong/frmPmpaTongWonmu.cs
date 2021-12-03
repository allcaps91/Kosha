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
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaTongWonmu.cs
    /// Description     : 의사별 내시경건수 통계
    /// Author          : 안정수
    /// Create Date     : 2017-08-23
    /// Update History  : 2017-11-02
    /// <history>  
    /// 빌드형성 부분 실제 테스트 필요
    /// d:\psmh\OPD\oumsad\Frm원무과통계자료.frm(Frm원무과통계작업) => frmPmpaTongWonmu.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oumsad\Frm원무과통계자료.frm(Frm원무과통계작업)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaTongWonmu : Form
    {
        ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread(); 

        public frmPmpaTongWonmu()
        {
            InitializeComponent();
            setEvent();
        }      

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
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

            optGbn0.Checked = true;
            string CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");

            CS.Spread_All_Clear(ssList2);
            CS.Spread_All_Clear(ssList3);
            CS.Spread_All_Clear(ssList4);

            ssList2_Sheet1.Rows.Count = 0;
            ssList3_Sheet1.Rows.Count = 0;
            ssList4_Sheet1.Rows.Count = 0;

            txtWard.Text = "";
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }

            else if (sender == this.btnView)
            {
                //                
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }

                if(tabItem1.IsSelected == true)
                {
                    eGetData1();
                }
                else if(tabItem2.IsSelected == true)
                {
                    eGetData2();
                }
                else if(tabItem3.IsSelected == true)
                {
                    eGetData3();
                }
             
            }

            else if (sender == this.btnBuild)
            {
                // 
                btnBuild_Click();


            }
        }

        void eGetData1()
        {
            int i = 0;
            int j = 0;
            int nCNT = 0;
            int nErCnt = 0;
            int nRow = 0;

            string strBDate = "";
            string strBDate_Old = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            nRow = 2;

            ssList1_Sheet1.Rows.Count = nRow;

            //외래통계 자료구함
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                ";
            SQL += ComNum.VBLF + "  BDate,DeptCode, COUNT(Pano) CNT                                     ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER                                 ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                             ";
            SQL += ComNum.VBLF + "      AND BDate >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')      ";
            SQL += ComNum.VBLF + "      AND BDate <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD')      ";
            SQL += ComNum.VBLF + "      AND Jin NOT IN ('D')                                        ";
            SQL += ComNum.VBLF + "Group By BDate,DeptCode                                               ";
            SQL += ComNum.VBLF + "Order By BDate                                                        ";

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
                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        strBDate = dt.Rows[i]["BDate"].ToString().Trim();

                        if(strBDate != strBDate_Old)
                        {
                            nRow += 1;
                            if(ssList1_Sheet1.Rows.Count < nRow)
                            {
                                ssList1_Sheet1.Rows.Count = nRow;
                            }
                            strBDate_Old = strBDate;
                            nCNT = 0;
                            nErCnt = 0;
                        }

                        ssList1_Sheet1.Cells[nRow - 1, 0].Text = VB.Left(dt.Rows[i]["BDate"].ToString().Trim(), 10);

                        if(dt.Rows[i]["DeptCode"].ToString().Trim() == "ER")
                        {
                            nErCnt += Convert.ToInt32(VB.Val(dt.Rows[i]["CNT"].ToString().Trim()));
                            ssList1_Sheet1.Cells[nRow - 1, 5].Text = String.Format("{0:###,###,###}", nErCnt);                            
                        }
                        else
                        {
                            nCNT += Convert.ToInt32(VB.Val(dt.Rows[i]["CNT"].ToString().Trim()));
                            ssList1_Sheet1.Cells[nRow - 1, 1].Text = String.Format("{0:###,###,###}", nCNT);
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

            //입원통계자료 구함
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                ";
            SQL += ComNum.VBLF + "  to_char(JobDate,'yyyy-mm-dd') JobDate ,GbBackUp,Count(Pano) CNT                                    ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_BM                                     ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                             ";
            SQL += ComNum.VBLF + "      AND JobDate >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')    ";  //입원
            SQL += ComNum.VBLF + "      AND JobDate <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD')    ";  //입원
            SQL += ComNum.VBLF + "      AND SubStr(Pano,1,3) <> '810'                                   ";
            SQL += ComNum.VBLF + "Group By JobDate,GbBackUp                                             ";
            SQL += ComNum.VBLF + "Order By JobDate                                                      ";

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
                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        for(j = 2; j < ssList1_Sheet1.Rows.Count; j++)
                        {
                            if(ssList1_Sheet1.Cells[j, 0].Text == dt.Rows[i]["JobDate"].ToString().Trim())
                            {
                                switch (dt.Rows[i]["GbBackUp"].ToString().Trim())
                                {
                                    case "I":
                                        ssList1_Sheet1.Cells[j, 2].Text = String.Format("{0:###,###,###}", dt.Rows[i]["CNT"].ToString().Trim());
                                        break;
                                    case "T":
                                        ssList1_Sheet1.Cells[j, 3].Text = String.Format("{0:###,###,###}", dt.Rows[i]["CNT"].ToString().Trim());
                                        break;
                                    case "J":
                                        ssList1_Sheet1.Cells[j, 4].Text = String.Format("{0:###,###,###}", dt.Rows[i]["CNT"].ToString().Trim());
                                        break;
                                }
                            }
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

        }

        void eGetData2()
        {
            int i = 0;
            int j = 0;
            int nREAD = 0;
            int nRow = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt1 = null;

            nRow = 0;
            ssList2_Sheet1.Rows.Count = 0;

            //외래통계 자료 구함
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                         ";
            SQL += ComNum.VBLF + "  PANO,SNAME,SEX || '/' || AGE AS SAGE,DEPTCODE,DRCODE,BI,WARDCODE,ROOMCODE,   ";
            SQL += ComNum.VBLF + "  TO_CHAR(INDATE,'YYYY-MM-DD') INDATE, TO_CHAR(OUTDATE,'YYYY-MM-DD') OUTDATE,  ";
            SQL += ComNum.VBLF + "  TO_CHAR(JOBDATE,'YYYY-MM-DD') JOBDATE                                        ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "TONG_PATIENT                                        ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                      ";
            SQL += ComNum.VBLF + "      AND JobDate >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')             ";
            SQL += ComNum.VBLF + "      AND JobDate <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD')             ";
            if(txtWard.Text != "")
            {
                SQL += ComNum.VBLF + "      AND WardCode = '" + txtWard.Text + "'                                "; 
            }
            else if(optGbn0.Checked == true)
            {
                SQL += ComNum.VBLF + "      AND PACLASS = '3'                                                    "; //재원자만
            }
            else if (optGbn1.Checked == true)
            {
                SQL += ComNum.VBLF + "      AND PACLASS = '2'                                                    "; //입원자만
            }
            else if (optGbn2.Checked == true)
            {
                SQL += ComNum.VBLF + "      AND PACLASS = '4'                                                    "; //퇴원자만
            }
            else if (optGbn3.Checked == true)
            {
                SQL += ComNum.VBLF + "      AND PACLASS = '5'                                                    "; //전출자만
            }
            else if (optGbn4.Checked == true)
            {
                SQL += ComNum.VBLF + "      AND PACLASS = '6'                                                    "; //전입자만
            }
            SQL += ComNum.VBLF + "Order By JobDate,SNAME                                                         ";
            

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
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            if (dt.Rows.Count > 0)
            {
                nREAD = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nRow += 1;

                    if (ssList2_Sheet1.Rows.Count < nRow)
                    {
                        ssList2_Sheet1.Rows.Count = nRow;
                    }

                    ssList2_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssList2_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssList2_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["SAGE"].ToString().Trim();
                    ssList2_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    ssList2_Sheet1.Cells[nRow - 1, 4].Text = CF.READ_DrName(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString().Trim());
                    ssList2_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["BI"].ToString().Trim();
                    ssList2_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                    ssList2_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                    ssList2_Sheet1.Cells[nRow - 1, 8].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                    ssList2_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["OUTDATE"].ToString().Trim();
                    ssList2_Sheet1.Cells[nRow - 1, 10].Text = dt.Rows[i]["JOBDATE"].ToString().Trim();

                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT                                                                                    ";
                    SQL += ComNum.VBLF + "  ROWID                                                                                   ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_TOTAL_CARE                                                 ";
                    SQL += ComNum.VBLF + "WHERE 1=1                                                                                 ";
                    SQL += ComNum.VBLF + "      AND Pano='" + dt.Rows[i]["PANO"].ToString().Trim() + "'                             ";
                    SQL += ComNum.VBLF + "      AND INDATE=TO_DATE('" + dt.Rows[i]["INDATE"].ToString().Trim() + "','YYYY-MM-DD')   ";

                    try
                    {

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        //if (dt1.Rows.Count == 0)
                        //{
                        //    dt1.Dispose();
                        //    dt1 = null;
                        //    ComFunc.MsgBox("해당 DATA가 없습니다.");
                        //    return;
                        //}

                        if (dt1.Rows.Count > 0)
                        {
                            if(dt1.Rows[0]["ROWID"].ToString().Trim() != "")
                            {
                                ssList2_Sheet1.Rows[nRow - 1].BackColor = Color.LightGreen;
                            }
                            else
                            {
                                ssList2_Sheet1.Rows[nRow - 1].BackColor = Color.LightPink;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ComFunc.MsgBox(ex.Message);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    }
                    dt1.Dispose();
                    dt1 = null;

                }
            }
            dt.Dispose();
            dt = null;

        }

        void eGetData3()
        {
            int i = 0;
            int j = 0;
            int y = 0;
            int nREAD = 0;
            int nRow = 0;

            string strJobDate = "";
            string strJobDate_Old = "";
            string strJobDate_Temp = "";
            string strPaClass = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            nRow = 0;
            ssList3_Sheet1.Rows.Count = 0;
            CS.Spread_All_Clear(ssList3);

            ssList4_Sheet1.Rows.Count = 0;
            CS.Spread_All_Clear(ssList4);

            //외래통계 자료구함
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                            ";
            SQL += ComNum.VBLF + "  JOBDATE   GUBUN,                                                                                                ";
            SQL += ComNum.VBLF + "  SUM(DECODE(PACLASS,'3',CPANO) ) JCNT,                                                                           ";
            SQL += ComNum.VBLF + "  SUM(DECODE(PACLASS,'2',CPANO) ) ICNT,                                                                           ";
            SQL += ComNum.VBLF + "  SUM(DECODE(PACLASS,'4',CPANO) )  -  nvl(SUM(DECODE(PACLASS,'5',CPANO) ),0 ) TCNT,                               ";
            SQL += ComNum.VBLF + "  SUM(DECODE(PACLASS,'5',CPANO) ) DTCNT                                                                           ";
            SQL += ComNum.VBLF + "FROM (                                                                                                            ";
            SQL += ComNum.VBLF + "          SELECT to_char(JOBDATE,'yyyy-mm-dd')  JOBDATE , PACLASS, COUNT(a.PANO) CPANO                            ";
            SQL += ComNum.VBLF + "          From ADMIN.TONG_PATIENT_TCARE a, ipd_new_master b                                                 ";
            SQL += ComNum.VBLF + "          WHERE JobDate >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')                                          ";
            SQL += ComNum.VBLF + "              AND JobDate <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD')                                        ";
            SQL += ComNum.VBLF + "              and a.pano=b.pano                                                                                   ";
            SQL += ComNum.VBLF + "              and a.indate=trunc(b.indate)                                                                        ";
            SQL += ComNum.VBLF + "              GROUP By to_char(JOBDATE,'yyyy-mm-dd') , PACLASS   union all                                        ";
            SQL += ComNum.VBLF + "          SELECT to_char(JOBDATE,'yyyy-mm-dd')  JOBDATE, '5' PACLASS, COUNT(a.PANO) CPANO                         ";
            SQL += ComNum.VBLF + "          From ADMIN.TONG_PATIENT_TCARE a, ipd_new_master b                                                 ";
            SQL += ComNum.VBLF + "          WHERE JobDate >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')                                          ";
            SQL += ComNum.VBLF + "              AND JobDate <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD')                                        ";
            SQL += ComNum.VBLF + "              and a.pano=b.pano                                                                                   ";
            SQL += ComNum.VBLF + "              and a.indate=trunc(b.indate)                                                                        ";
            SQL += ComNum.VBLF + "              and a.indate=b.outdate                                                                              ";
            SQL += ComNum.VBLF + "              and PACLASS ='4'                                                                                    ";
            SQL += ComNum.VBLF + "              GROUP By to_char(JOBDATE,'yyyy-mm-dd') , PACLASS   )  group by ROLLUP(jobdate)    ORDER By JOBDATE  ";            

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
                    nREAD = dt.Rows.Count;
                                        
                    for(i = 0; i < nREAD; i++)
                    {
                        nRow += 1;
                        if (ssList3_Sheet1.Rows.Count < nRow)
                        {
                            ssList3_Sheet1.Rows.Count = nRow;
                        }

                        ssList3_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["GUBUN"].ToString().Trim();
                        ssList3_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["JCNT"].ToString().Trim();
                        ssList3_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["ICNT"].ToString().Trim();
                        ssList3_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["TCNT"].ToString().Trim();
                        ssList3_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["DTCNT"].ToString().Trim();
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
        }

        void btnBuild_Click()
        {
            string strFDate = dtpFDate.Text;
            string strTDate = dtpTDate.Text;

            if (MessageBox.Show("통계기간▶" + strFDate + " ~ " + strTDate + "\r\n" + "\r\n" + "해당기간 통계자료를 형성하시겠습니까?", "통합간호간병 통계자료 형성!!", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

             
            clsDB.setBeginTran(clsDB.DbCon);

            //이전 Data Check
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                        ";
            SQL += ComNum.VBLF + "  Count(Pano) CPANO                                           ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "TONG_PATIENT_TCARE                 ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                     ";
            SQL += ComNum.VBLF + "      AND JobDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "      AND JobDate <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
            
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
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            if (dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(VB.Val(dt.Rows[0]["CPANO"].ToString().Trim())) > 0)
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "DELETE " + ComNum.DB_PMPA + "TONG_PATIENT_TCARE               ";
                    SQL += ComNum.VBLF + "WHERE 1=1                                                     ";
                    SQL += ComNum.VBLF + "      AND JobDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "      AND JobDate <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    try
                    {
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox("통계빌드시 오류발생!!(전산실 연락요망)");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(ex.Message);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
            }

            dt.Dispose();
            dt = null;

            //재원자 집계 구분 3
            SQL = "";
            SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "TONG_PATIENT_TCARE                          ";
            SQL += ComNum.VBLF + "(                                                                             ";
            SQL += ComNum.VBLF + "JOBDATE,PACLASS,PANO,SNAME,SEX,AGE,DEPTCODE,DRCODE,BI,WARDCODE,ROOMCODE,      ";
            SQL += ComNum.VBLF + "INDATE , OutDate, T_CARE                                                      ";
            SQL += ComNum.VBLF + ")                                                                             ";
            SQL += ComNum.VBLF + "SELECT                                                                        ";
            SQL += ComNum.VBLF + "  JOBDATE,PACLASS,PANO,SNAME,SEX,AGE,DEPTCODE,DRCODE,BI,WARDCODE,ROOMCODE,    ";
            SQL += ComNum.VBLF + "  INDATE , OutDate, T_CARE                                                    ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "TONG_PATIENT                                       ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
            SQL += ComNum.VBLF + "      AND JobDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD')                 ";
            SQL += ComNum.VBLF + "      AND JobDate <= TO_DATE('" + strTDate + "','YYYY-MM-DD')                 ";
            SQL += ComNum.VBLF + "      AND WardCode = '60'                                                     ";
            SQL += ComNum.VBLF + "      AND PACLASS = '3'                                                       ";  //재원자만
            SQL += ComNum.VBLF + "      AND T_CARE ='Y'                                                         ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            try
            {
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("재원자 통계빌드시 오류발생!!(전산실 연락요망)");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            //입원자 집계 구분 2
            SQL = "";
            SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "TONG_PATIENT_TCARE                          ";
            SQL += ComNum.VBLF + "(                                                                             ";
            SQL += ComNum.VBLF + "JOBDATE,PACLASS,PANO,SNAME,SEX,AGE,DEPTCODE,DRCODE,BI,WARDCODE,ROOMCODE,      ";
            SQL += ComNum.VBLF + "INDATE , OutDate, T_CARE                                                      ";
            SQL += ComNum.VBLF + ")                                                                             ";
            SQL += ComNum.VBLF + "SELECT                                                                        ";
            SQL += ComNum.VBLF + "  JOBDATE,'2',PANO,SNAME,SEX,AGE,DEPTCODE,DRCODE,BI,WARDCODE,ROOMCODE,        ";
            SQL += ComNum.VBLF + "  INDATE , OutDate, T_CARE                                                    ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "TONG_PATIENT                                       ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
            SQL += ComNum.VBLF + "      AND JobDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD')                 ";
            SQL += ComNum.VBLF + "      AND JobDate <= TO_DATE('" + strTDate + "','YYYY-MM-DD')                 ";
            SQL += ComNum.VBLF + "      AND WardCode = '60'                                                     ";
            SQL += ComNum.VBLF + "      AND PACLASS IN ( '2', '6' )                                             ";  //재원자만

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            try
            {
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("입원자 통계빌드시 오류발생!!(전산실 연락요망)");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            //퇴원자 집계 구분 4
            SQL = "";
            SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "TONG_PATIENT_TCARE                          ";
            SQL += ComNum.VBLF + "(                                                                             ";
            SQL += ComNum.VBLF + "JOBDATE,PACLASS,PANO,SNAME,SEX,AGE,DEPTCODE,DRCODE,BI,WARDCODE,ROOMCODE,      ";
            SQL += ComNum.VBLF + "INDATE , OutDate, T_CARE                                                      ";
            SQL += ComNum.VBLF + ")                                                                             ";
            SQL += ComNum.VBLF + "SELECT                                                                        ";
            SQL += ComNum.VBLF + "  JOBDATE,'4',PANO,SNAME,SEX,AGE,DEPTCODE,DRCODE,BI,WARDCODE,ROOMCODE,        ";
            SQL += ComNum.VBLF + "  INDATE , OutDate, T_CARE                                                    ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "TONG_PATIENT                                       ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
            SQL += ComNum.VBLF + "      AND JobDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD')                 ";
            SQL += ComNum.VBLF + "      AND JobDate <= TO_DATE('" + strTDate + "','YYYY-MM-DD')                 ";
            SQL += ComNum.VBLF + "      AND WardCode = '60'                                                     ";
            SQL += ComNum.VBLF + "      AND PACLASS IN ( '4', '5' )                                             ";  //재원자만

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            try
            {
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("퇴원자 통계빌드시 오류발생!!(전산실 연락요망)");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            clsDB.setCommitTran(clsDB.DbCon);
            ComFunc.MsgBox("빌드완료!!");
            Cursor.Current = Cursors.Default;



        }

    }
}
