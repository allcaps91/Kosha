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

namespace ComSupLibB.Com
{
    /// <summary>
    /// Class Name      : ComSupLibB.Com
    /// File Name       : frmComSupVIEW01.cs
    /// Description     : 평가관련 퇴원자 리스트 통계 폼
    /// Author          : 윤조연
    /// Create Date     : 2017-06-20
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 기존 Frm퇴원환자명부1.frm(Frm퇴원환자명부1) 폼 frmComSupVIEW01.cs 으로 변경함
    /// 기준 테이블 - MID_SUMMARY, ETC_MID_PANOLIST
    /// </history>
    /// <seealso cref= "tong\tlservice\Frm퇴원환자명부1.frm >> frmComSupVIEW01.cs 폼이름 재정의" />
    public partial class frmComSupVIEW01 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수        
        clsComSupSpd csupspd = new clsComSupSpd();

        string SQL = "";    //Query문
        string SqlErr = ""; //에러문 받는 변수        
        int intRowAffected = 0; //변경된 Row 받는 변수

        bool bSave = false;

        string[] argsql = null;//쿼리변수사용
        enum enmsql { Gubun, SDate,TDate,OptDept1,OptDept2, OptJ1,OptJ2 };
        enum enmspd { chk,Pano,Sname,IpDate,OutDate,iDept,tDept,tDrCode,Jilsu,Sex,Age,ROWID };

        #endregion

        public frmComSupVIEW01(bool argsave =false)
        {
            InitializeComponent();

            bSave = argsave;
            //
            setEvent();
        }

       //기본값 세팅
        void setCtrlData()
        {
            //txtPano.Text = gstrPano;
            //lblSName.Text = gstrSName;

            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

            int year = Convert.ToInt16( VB.Left(cpublic.strSysDate, 4));

            for (int i = 0; i < 10; i++)
            {
                ssList2.ActiveSheet.Cells[i, 0].Text = year.ToString();
                year--;
            }

        }        

        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            //this.btnSearch2.Click += BtnSearch1_Click;
            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnSearch.Click += new EventHandler(eBtnEvent);
            this.btnSearch2.Click += new EventHandler(eBtnEvent);

            this.btnPrint.Click += new EventHandler(eBtnEvent);
            this.btnPrint2.Click += new EventHandler(eBtnEvent);

            this.ssList.ButtonClicked += ssList_ButtonClicked;

        }

        void ssList_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (bSave == false) return; //저장권한 없음

            
            string strChk = "" ;
            string strPano = "" ;
            string strSName = "" ;
            string strInDate = "" ;
            string strOutDate = "" ;
            string strDeptCode = "" ;
            string strROWID = "" ;


            // clsTrans DT = new clsTrans();
            clsDB.setBeginTran(clsDB.DbCon);

            if (e.Column < 0 || e.Row < 0) return;
            if (e.Column != (int)enmspd.chk) return;

            strChk = ssList.ActiveSheet.Cells[e.Row, (int)enmspd.chk].Text.ToString();
            strPano = ssList.ActiveSheet.Cells[e.Row, (int)enmspd.Pano].Text.ToString();
            strSName = ssList.ActiveSheet.Cells[e.Row, (int)enmspd.Sname].Text.ToString();
            strInDate = ssList.ActiveSheet.Cells[e.Row, (int)enmspd.IpDate].Text.ToString();
            strOutDate = ssList.ActiveSheet.Cells[e.Row, (int)enmspd.OutDate].Text.ToString();
            strDeptCode = ssList.ActiveSheet.Cells[e.Row, (int)enmspd.tDept].Text.ToString();
            strROWID = ssList.ActiveSheet.Cells[e.Row, (int)enmspd.ROWID].Text.ToString();

            string strYear = VB.Left(dtpFDate.Text,4);
            string strGbn = "";
            if (strDeptCode=="MD" || strDeptCode == "PD" || strDeptCode == "NP" || strDeptCode == "RM" || strDeptCode == "MD")
            {
                strGbn = "1";
            }
            else
            {
                strGbn = "2";
            }
            


            try
            {

                SQL = " DELETE " + ComNum.DB_PMPA + "ETC_MID_PANOLIST WHERE M_ROWID='" + strROWID + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return ;
                }

                if (strChk == "True")
                {
                    ssList.ActiveSheet.Rows.Get(e.Row).BackColor = System.Drawing.Color.LightBlue; //컬럼전체

                    SQL = "";
                    SQL += " INSERT INTO " + ComNum.DB_PMPA + "ETC_MID_PANOLIST  \r\n";
                    SQL += " (Pano,SName,Year,GbBun,DeptCode,InDate,OutDate,M_ROWID) \r\n";
                    SQL += "VALUES ( \r\n";

                    SQL += " '" + strPano + "',  \r\n"; //
                    SQL += " '" + strSName + "',  \r\n"; //
                    SQL += " '" + strYear + "',  \r\n"; //
                    SQL += " '" + strGbn + "',  \r\n"; //
                    SQL += " '" + strDeptCode + "',  \r\n"; //
                    SQL += " TO_DATE('" + strInDate + "','YYYY-MM-DD'),  \r\n"; //
                    SQL += " TO_DATE('" + strOutDate + "','YYYY-MM-DD'),  \r\n"; //
                    SQL += " '" + strROWID + "'  \r\n"; //
                    
                    SQL += ") \r\n";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return ;
                    }

                }
                else
                {
                    ssList.ActiveSheet.Rows.Get(e.Row).BackColor = System.Drawing.Color.White; //컬럼전체

                }
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return ;
            }

            clsDB.setCommitTran(clsDB.DbCon);

            sel_Count();
            chk_select_data();


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

                setCtrlData();

                screen_clear();

                //
                //clsSupFnExSpd.AUTO_SPREAD_SET_SupFnExMain(ssList_Sheet1, 10, 0);

                data_delete();
            }

        }

        void data_delete()
        {
            // clsTrans DT = new clsTrans();
            clsDB.setBeginTran(clsDB.DbCon);

            SQL = " DELETE " + ComNum.DB_PMPA + "ETC_MID_PANOLIST  ";
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
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
            else if (sender == this.btnSearch)
            {
                //
                GetData(ssList,"기본명단");
            }
            else if (sender == this.btnSearch2)
            {
                //
                GetData(ssList, "선택명단");
            }
            else if (sender == this.btnPrint)
            {
                //
                ePrint("기본명단");
            }
            else if (sender == this.btnPrint2)
            {
                //
                ePrint("선택명단");
            }


        }

        void ePrint(string str)
        {
            string[] strhead = new string[2];
            string[] strfont = new string[2];

            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

            strfont[0] = "/fn\"바탕체\" /fz\"14\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strfont[1] = "/fn\"바탕체\" /fz\"12\" /fb0 /fi0 /fu0 /fk0 /fs2";
            if (str== "기본명단")
            {
                strhead[0] = "/c/f1/n" + "퇴 원 환 자   명 부" + "/f1/n";
            }
            else if (str == "선택명단")
            {
                strhead[0] = "/c/f1/n" + "퇴 원 환 자   명 부" + "/f1/n";
            }

            strhead[1] = "/n/l/f2" + "작업기간 : 선택한 환자"+ " /l/f2" + "  출력시간 : " + cpublic.strSysDate + " " + cpublic.strSysTime + " /n";
                        

            csupspd.SPREAD_PRINT(ssList_Sheet1, ssList, strhead, strfont, 10, 10, 1, true);
        }

        void screen_clear()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

            //txtSearch.Text = "";   
            //dtpFDate.Text =cpublic.strSysDate;  

        }

        // 쿼리부분
        DataTable sel_MidSummary(string[] arg)
        {
            DataTable dt = null;
            
            SQL = "";
            SQL += " SELECT                                                                                         \r\n";
            SQL += "  a.Pano,TO_CHAR(a.OUTDATE,'YYYY-MM-DD') OutDate,                                               \r\n";
            SQL += "      a.Sname,a.Idept,a.Tdept,a.TDoctor,TO_CHAR(a.InDate,'YYYY-MM-DD') InDate,                  \r\n";
            SQL += "      a.Jilsu,a.Jumin2,a.Age,a.Bi,a.CResult,a.TModel,a.GbDie,a.Kukso,a.Kukso1,                  \r\n";
            SQL += "      a.Kukso2,a.Kukso3,a.Sgun1,a.SGun1_B,Sancd,a.NbGb,a.BabyType,a.Canc,a.HULAK,               \r\n";
            SQL += "      b.DrName,c.Sex,a.ROWID                                                                    \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "MID_SUMMARY a ," + ComNum.DB_PMPA + "BAS_DOCTOR b ,               \r\n";
            SQL += "   " + ComNum.DB_PMPA + "BAS_PATIENT c                                                          \r\n";
            SQL += "  WHERE 1 = 1                                                                                   \r\n";
            SQL += "   AND A.OutDate >=TO_DATE('" + arg[(int)enmsql.SDate] + "','YYYY-MM-DD')                       \r\n";
            SQL += "   AND A.OutDate <=TO_DATE('" + arg[(int)enmsql.TDate] + "','YYYY-MM-DD')                       \r\n";
            if (arg[(int)enmsql.OptJ2] == "True")
            {
                SQL += "   AND a.JIlsu >= 7                                                                         \r\n";
                SQL += "   AND a.JIlsu <= 21                                                                        \r\n";
            }
           
            if (arg[(int)enmsql.OptDept1] == "True")
            {
                SQL += "  AND a.TDept IN ('MD','PD','DM','NP','RM')                                                 \r\n";
            }
            else if (arg[(int)enmsql.OptDept2] == "True")
            {
                SQL += "  AND a.TDept NOT IN ('MD','PD','DM','NP','RM')                                             \r\n";
            }

            SQL += "   AND a.Pano NOT IN (SELECT Pano FROM MID_NOT_PANO)                                            \r\n";

            if (arg[(int)enmsql.Gubun]=="기본명단")
            {
                
            }
            else if (arg[(int)enmsql.Gubun] == "선택명단")
            {
                SQL += "   AND a.ROWID IN (SELECT M_ROWID FROM KOSMOS_PMPA.ETC_MID_PANOLIST)                         \r\n";
            }


            SQL += "   AND a.TDoctor=b.DrCode(+)                                                                    \r\n";
            SQL += "   AND a.Pano=c.Pano(+)                                                                         \r\n";
            SQL += "  ORDER BY a.TDept,a.SName,a.Pano,a.OutDate                                                     \r\n";
            
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return null;
            }

            return dt;


        }

        DataTable sel_EtcMidPanoCnt()
        {
            DataTable dt = null;
           


            SQL = "";
            SQL += " SELECT                                                                                         \r\n";
            SQL += "  Year,SUM(DECODE(GbBun,'1',1,0)) Cnt1,SUM(DECODE(GbBun,'1',0,1)) Cnt2                           \r\n";            
            SQL += "   FROM " + ComNum.DB_PMPA + "ETC_MID_PANOLIST                                                  \r\n";            
            SQL += "  WHERE 1 = 1                                                                                   \r\n";
            //SQL += "   AND Year='" + VB.Left( dtpFDate.Text,4) + "'                                                 \r\n";                      
            SQL += "   GROUP BY Year                                                                                 \r\n";
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return null;
            }

            return dt;


        }

        void chk_select_data()
        {

            DataTable dt = null;
            string strPano = "", strPano2 = "";
            string strInDate = "", strInDate2 = "";
            int i = 0, j = 0;

            dt = sel_EtcMidPanoList();

            if (dt == null) return;
            if (dt.Rows.Count > 0)
            {
                for ( i = 0; i < dt.Rows.Count; i++)
                {
                    strPano = dt.Rows[i]["Pano"].ToString().Trim();
                    strInDate = dt.Rows[i]["InDate"].ToString().Trim();

                    for ( j = 0; j < ssList.ActiveSheet.RowCount; j++)
                    {
                        strPano2 =ssList.ActiveSheet.Cells[j, (int)enmspd.Pano].Text;
                        strInDate2 = ssList.ActiveSheet.Cells[j, (int)enmspd.IpDate].Text;

                        if (strPano == strPano2  && strInDate == strInDate2 ) 
                        {
                            ssList.ActiveSheet.Cells[j, (int)enmspd.chk].Value = true;
                            ssList.ActiveSheet.Rows.Get(j).BackColor = System.Drawing.Color.HotPink; //컬럼전체
                        }


                    }
                }
            }
            

        }

        DataTable sel_EtcMidPanoList()
        {
            DataTable dt = null;
            
            SQL = "";
            SQL += " SELECT                                                                                         \r\n";
            SQL += "  Pano,TO_CHAR(InDate,'YYYY-MM-DD') InDate,ROWID                                                \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "ETC_MID_PANOLIST                                                  \r\n";
            SQL += "  WHERE 1 = 1                                                                                   \r\n";
            SQL += "   AND Year='" + VB.Left( dtpFDate.Text,4) + "'                                                 \r\n";                      
                                                                                            
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return null;
            }

            return dt;


        }

        void GetData(FarPoint.Win.Spread.FpSpread spd,string argGubun)
        {
            int i = 0;
            
            DataTable dt = null;
            int nRow = -1;
            string strOK = "";
                 
                 

            #region //쿼리 배열 변수 세팅

            argsql = new string[Enum.GetValues(typeof(enmsql)).Length];
            argsql[(int)enmsql.Gubun] = argGubun;
            argsql[(int)enmsql.SDate] = dtpFDate.Text.Trim();
            argsql[(int)enmsql.TDate] = dtpTDate.Text.Trim();
            argsql[(int)enmsql.OptDept1] = optGubun1.Checked.ToString();
            argsql[(int)enmsql.OptDept2] = optGubun2.Checked.ToString();

            argsql[(int)enmsql.OptJ1] = optJewon1.Checked.ToString();

            if (VB.Left(argsql[(int)enmsql.SDate], 4) != VB.Left(argsql[(int)enmsql.TDate], 4))
            {
                MessageBox.Show("자료조회시 같은 연도로 조회하십시오!!");
                return;
            }
            

            #endregion
            dt = sel_MidSummary(argsql);

            #region //데이터셋 읽어 자료 표시

            spd.ActiveSheet.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;
            spd.ActiveSheet.RowCount = dt.Rows.Count;
            spd.ActiveSheet.SetRowHeight(-1, 24);

            for (i = 0; i < dt.Rows.Count; i++)
            {

                strOK = "OK";
                
                if (strOK == "OK")
                {
                    nRow++;

                    spd.ActiveSheet.Cells[nRow, (int)enmspd.chk].Value = "";
                    spd.ActiveSheet.Cells[nRow, (int)enmspd.Pano].Value = dt.Rows[i]["Pano"].ToString().Trim();
                    spd.ActiveSheet.Cells[nRow, (int)enmspd.Sname].Value = dt.Rows[i]["Sname"].ToString().Trim();
                    spd.ActiveSheet.Cells[nRow, (int)enmspd.IpDate].Value = dt.Rows[i]["InDate"].ToString().Trim();
                    spd.ActiveSheet.Cells[nRow, (int)enmspd.OutDate].Value = dt.Rows[i]["OutDate"].ToString().Trim();
                    spd.ActiveSheet.Cells[nRow, (int)enmspd.iDept].Value = dt.Rows[i]["Idept"].ToString().Trim();
                    spd.ActiveSheet.Cells[nRow, (int)enmspd.tDept].Value = dt.Rows[i]["Tdept"].ToString().Trim();
                    spd.ActiveSheet.Cells[nRow, (int)enmspd.tDrCode].Value = dt.Rows[i]["DrName"].ToString().Trim();
                    spd.ActiveSheet.Cells[nRow, (int)enmspd.Jilsu].Value = dt.Rows[i]["Jilsu"].ToString().Trim();
                    spd.ActiveSheet.Cells[nRow, (int)enmspd.Sex].Value = dt.Rows[i]["Sex"].ToString().Trim();
                    spd.ActiveSheet.Cells[nRow, (int)enmspd.Age].Value = dt.Rows[i]["Age"].ToString().Trim();
                    spd.ActiveSheet.Cells[nRow, (int)enmspd.ROWID].Value = dt.Rows[i]["ROWID"].ToString().Trim();


                }

            }

            dt.Dispose();
            dt = null;

            Cursor.Current = Cursors.Default;

            #endregion

            //
            sel_Count();

            //
            chk_select_data();

        }

        void sel_Count()
        {
            int i = 0;
            int j = 0;
            string strYear = "";
            long nTot1 = 0, nTot2 = 0;


            DataTable dt = null;

            #region //쿼리 및 자료 표시
            dt = sel_EtcMidPanoCnt();

            //자료표시
            if (dt == null) return;
            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strYear = dt.Rows[i]["Year"].ToString().Trim();
                    for (j = 0; j < 10; j++)
                    {
                        if (ssList2.ActiveSheet.Cells[j, 0].Text == strYear)
                        {
                            ssList2.ActiveSheet.Cells[j, 1].Text = dt.Rows[i]["Cnt1"].ToString().Trim();
                            ssList2.ActiveSheet.Cells[j, 2].Text = dt.Rows[i]["Cnt2"].ToString().Trim();

                            nTot1 += Convert.ToInt32(dt.Rows[i]["Cnt1"].ToString().Trim());
                            nTot2 += Convert.ToInt32(dt.Rows[i]["Cnt2"].ToString().Trim());
                        }
                    }

                }
            }

            //합계
            ssList2.ActiveSheet.Cells[10, 1].Text = nTot1.ToString();
            ssList2.ActiveSheet.Cells[10, 2].Text = nTot2.ToString();

            #endregion
        }
    }
}
