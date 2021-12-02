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
    /// File Name       : frmPmpaViewSname2.cs
    /// Description     : 수진자 List 조회
    /// Author          : 안정수
    /// Create Date     : 2017-07-06
    /// Update History  : 
    /// <history>       
    /// d:\psmh\OPD\jengsan\FrmSnameView2.frm(FrmSnameView2) => frmPmpaViewSname2.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\jengsan\FrmSnameView2.frm(FrmSnameView2)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewSname2 : Form
    {
        ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread();

        string mstrHelp = "";
        string mstrName = "";
        string mstrPANO = "";

        public frmPmpaViewSname2()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewSname2(string pstrHelp)
        {
            InitializeComponent();
            mstrHelp = pstrHelp;
            setEvent();
        }

        public frmPmpaViewSname2(string pstrName, string pstrPANO)
        {
            InitializeComponent();

            mstrName = pstrName;
            mstrPANO = pstrPANO;

            setEvent();
        }

        public frmPmpaViewSname2(string pstrHelp, string pstrName, string pstrPANO)
        {
            InitializeComponent();

            mstrHelp = pstrHelp;
            mstrName = pstrName;
            mstrPANO = pstrPANO;

            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);

            this.optBun0.CheckedChanged += new EventHandler(eControl_Checked);
            this.optBun1.CheckedChanged += new EventHandler(eControl_Checked);
            this.optBun2.CheckedChanged += new EventHandler(eControl_Checked);
            this.optBun3.CheckedChanged += new EventHandler(eControl_Checked);
            this.optBun4.CheckedChanged += new EventHandler(eControl_Checked);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등   
            if (mstrHelp != "" && (mstrName != "" || mstrPANO != ""))
            {
                eGetData();
            }
            optBun0.Checked = true;
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                clsPmpaPb.GstrName = "";
                clsPmpaPb.GstrYear = "";
                clsPmpaPb.GstrSname = "";
                clsPmpaPb.GstrJumin1 = "";
                clsPmpaPb.GstrJumin2 = "";
                clsPmpaPb.GstrStartDate = "";
                clsPmpaPb.GstrLastDate = "";
                clsPmpaPb.GstrZipCode = "";
                clsPmpaPb.GstrJiname = "";
                clsPmpaPb.GstrJuso = "";
                clsPmpaPb.GstrJuso1 = "";
                clsPmpaPb.GstrSex = "";
                clsPmpaPb.GstrFal = "";
                clsPmpaPb.GstrJumin = "";

                clsPmpaPb.GstrHelp = "";


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

        void eControl_Checked(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            ssList_Sheet1.Rows.Count = 0;

            if (sender == this.optBun0)
            {
                ssList_Sheet1.Rows.Count = 0;

                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                            ";
                SQL += ComNum.VBLF + "  P.Pano,P.Sname,P.Sex,P.Jumin1,P.Jumin2,                                         ";
                SQL += ComNum.VBLF + "  TO_CHAR(P.StartDate,'YYYY-MM-DD') StartDate,                                    ";
                SQL += ComNum.VBLF + "  TO_CHAR(P.LastDate,'YYYY-MM-DD') LastDate,JiName,P.ZipCode1,                    ";
                SQL += ComNum.VBLF + "  P.ZipCode2,ZipName1,ZipName2,ZipName3,Juso,Tel                                  ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT P, " + ComNum.DB_PMPA + "BAS_AREA A,       ";
                SQL += ComNum.VBLF + ComNum.DB_PMPA + "BAS_ZIPSNEW Z, " + ComNum.DB_PMPA + "OPD_MASTER C      ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                         ";
                SQL += ComNum.VBLF + "      AND P.JiCode = A.JiCode(+)                                                  ";
                SQL += ComNum.VBLF + "      AND P.ZipCode1 = Z.ZipCode1(+)                                              ";
                SQL += ComNum.VBLF + "      AND P.ZipCode2 = Z.ZipCode2(+)                                              ";
                SQL += ComNum.VBLF + "      AND C.ACTDATE >= TO_DATE('" + dtpFDate.Text + "', 'YYYY-MM-DD')             ";
                SQL += ComNum.VBLF + "      AND C.ACTDATE <= TO_DATE('" + dtpTDate.Text + "', 'YYYY-MM-DD')             ";
                SQL += ComNum.VBLF + "      AND C.PANO = P.PANO                                                         ";
                SQL += ComNum.VBLF + "GROUP BY P.Pano, P.Sname, P.Sex, P.Jumin1,P.Jumin2, P.startdate, P.Lastdate,      ";
                SQL += ComNum.VBLF + "         JiName, P.ZipCode1, P.ZipCode2, ZipName1, ZipName2, ZipName3, Juso, Tel  ";
                SQL += ComNum.VBLF + "ORDER BY  P.JUMIN1, P.Sname, P.Pano                                               ";
            }

            else if (sender == this.optBun1)
            {
                ssList_Sheet1.Rows.Count = 0;
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                            ";
                SQL += ComNum.VBLF + "  P.Pano,P.Sname,P.Sex,P.Jumin1,P.Jumin2,                                         ";
                SQL += ComNum.VBLF + "  TO_CHAR(P.StartDate,'YYYY-MM-DD') StartDate,                                    ";
                SQL += ComNum.VBLF + "  TO_CHAR(P.LastDate,'YYYY-MM-DD') LastDate,JiName,P.ZipCode1,                    ";
                SQL += ComNum.VBLF + "  P.ZipCode2,ZipName1,ZipName2,ZipName3,Juso,Tel                                  ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT P, " + ComNum.DB_PMPA + "BAS_AREA A,       ";
                SQL += ComNum.VBLF + ComNum.DB_PMPA + "BAS_ZIPSNEW Z, " + ComNum.DB_PMPA + "IPD_NEW_MASTER C  ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                         ";
                SQL += ComNum.VBLF + "      AND P.JiCode = A.JiCode(+)                                                  ";
                SQL += ComNum.VBLF + "      AND P.ZipCode1 = Z.ZipCode1(+)                                              ";
                SQL += ComNum.VBLF + "      AND P.ZipCode2 = Z.ZipCode2(+)                                              ";
                SQL += ComNum.VBLF + "      AND C.OUTDATE >= TO_DATE('" + dtpFDate.Text + "', 'YYYY-MM-DD')             ";
                SQL += ComNum.VBLF + "      AND C.OUTDATE <= TO_DATE('" + dtpTDate.Text + "', 'YYYY-MM-DD')             ";
                SQL += ComNum.VBLF + "      AND C.PANO = P.PANO                                                         ";
                SQL += ComNum.VBLF + "GROUP BY P.Pano, P.Sname, P.Sex, P.Jumin1,P.Jumin2, P.startdate, P.Lastdate,      ";
                SQL += ComNum.VBLF + "         JiName, P.ZipCode1, P.ZipCode2, ZipName1, ZipName2, ZipName3, Juso, Tel  ";
                SQL += ComNum.VBLF + "ORDER BY  P.JUMIN1, P.Sname, P.Pano                                               ";
            }

            else if (sender == this.optBun2)
            {
                ssList_Sheet1.Rows.Count = 0;
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                            ";
                SQL += ComNum.VBLF + "  PANO, SNAME, SUBSTR(JUMIN,1,6) JUMIN1, SUBSTR(JUMIN,7,7) JUMIN2,                ";
                SQL += ComNum.VBLF + "  '' LASTDATE, '' JINAME, '' TEL, '' JUSO,'' SEX,                                 ";
                SQL += ComNum.VBLF + "  '' STARTDATE, '' ZIPCODE1, '' ZIPCODE2, '' ZIPNAME1, '' ZIPNAME2, '' ZIPNAME3   ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_JUNSLIP                                            ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                         ";
                SQL += ComNum.VBLF + "      AND ACTDATE >= TO_DATE('" + dtpFDate.Text + "', 'YYYY-MM-DD')               ";
                SQL += ComNum.VBLF + "      AND ACTDATE <= TO_DATE('" + dtpTDate.Text + "', 'YYYY-MM-DD')               ";
                SQL += ComNum.VBLF + "      AND DEPTCODE  = 'HR'                                                        ";
                SQL += ComNum.VBLF + "GROUP BY PANO, SNAME, JUMIN                                                      ";
            }

            else if (sender == this.optBun3)
            {
                ssList_Sheet1.Rows.Count = 0;
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                            ";
                SQL += ComNum.VBLF + "  PANO, SNAME, SUBSTR(JUMIN,1,6) JUMIN1, SUBSTR(JUMIN,7,7) JUMIN2,                ";
                SQL += ComNum.VBLF + "  '' LASTDATE, '' JINAME, '' TEL, '' JUSO,'' SEX,                                 ";
                SQL += ComNum.VBLF + "  '' STARTDATE, '' ZIPCODE1, '' ZIPCODE2, '' ZIPNAME1, '' ZIPNAME2, '' ZIPNAME3   ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_JUNSLIP                                            ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                         ";
                SQL += ComNum.VBLF + "      AND ACTDATE >= TO_DATE('" + dtpFDate.Text + "', 'YYYY-MM-DD')               ";
                SQL += ComNum.VBLF + "      AND ACTDATE <= TO_DATE('" + dtpTDate.Text + "', 'YYYY-MM-DD')               ";
                SQL += ComNum.VBLF + "      AND DEPTCODE  = 'TO'                                                        ";
                SQL += ComNum.VBLF + "GROUP BY PANO, SNAME, JUMIN                                                      ";
            }

            else if (sender == this.optBun4)
            {
                ssList_Sheet1.Rows.Count = 0;
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                            ";
                SQL += ComNum.VBLF + "  PANO, SNAME, SUBSTR(JUMIN,1,6) JUMIN1, SUBSTR(JUMIN,7,7) JUMIN2,                ";
                SQL += ComNum.VBLF + "  '' LASTDATE, '' JINAME, '' TEL, '' JUSO,'' SEX,                                 ";
                SQL += ComNum.VBLF + "  '' STARTDATE, '' ZIPCODE1, '' ZIPCODE2, '' ZIPNAME1, '' ZIPNAME2, '' ZIPNAME3   ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_JUNSLIP                                            ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                         ";
                SQL += ComNum.VBLF + "      AND ACTDATE >= TO_DATE('" + dtpFDate.Text + "', 'YYYY-MM-DD')               ";
                SQL += ComNum.VBLF + "      AND ACTDATE <= TO_DATE('" + dtpTDate.Text + "', 'YYYY-MM-DD')               ";
                SQL += ComNum.VBLF + "      AND DEPTCODE  = 'H7'                                                        ";
                SQL += ComNum.VBLF + "GROUP BY PANO, SNAME, JUMIN                                                      ";
            }

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                //if (dt.Rows.Count == 0)
                //{
                //    dt.Dispose();
                //    dt = null;
                //    ComFunc.MsgBox("해당 DATA가 없습니다.");
                //    return;
                //}

                if (dt.Rows.Count > 0)
                {
                    ssList_Sheet1.Rows.Count = dt.Rows.Count;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Sname"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Sex"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Jumin1"].ToString().Trim() + "-" + dt.Rows[i]["Jumin2"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["LastDate"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["JiName"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Tel"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["Juso"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 8].Text = dt.Rows[i]["StartDate"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 9].Text = dt.Rows[i]["ZipCode1"].ToString().Trim() + "-" + dt.Rows[i]["ZipCode2"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 10].Text = dt.Rows[i]["ZipName1"].ToString().Trim() + " " + dt.Rows[i]["ZipName2"].ToString().Trim() + " " + dt.Rows[i]["ZipName3"].ToString().Trim();
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

        void eGetData()
        {
            int i = 0;
            int nREAD = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (mstrHelp == "")
            {
                for (i = 7; i <= 10; i++)
                {
                    ssList_Sheet1.Columns[i].Visible = false;
                }

                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                        ";
                SQL += ComNum.VBLF + "  Pano,Sname,Sex,Jumin1,Jumin2,                                               ";
                SQL += ComNum.VBLF + "  TO_CHAR(StartDate,'YYYY-MM-DD') StartDate,                                  ";
                SQL += ComNum.VBLF + "  TO_CHAR(LastDate,'YYYY-MM-DD') LastDate,JiName,P.ZipCode1,                  ";
                SQL += ComNum.VBLF + "  P.ZipCode2,ZipName1,ZipName2,ZipName3,Juso,Tel                              ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT P, " + ComNum.DB_PMPA + "BAS_AREA A,   ";
                SQL += ComNum.VBLF + ComNum.DB_PMPA + "BAS_ZIPSNEW Z                                      ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
                SQL += ComNum.VBLF + "      AND P.JiCode = A.JiCode(+)                                              ";
                SQL += ComNum.VBLF + "      AND P.ZipCode1 = Z.ZipCode1(+)                                          ";
                SQL += ComNum.VBLF + "      AND P.ZipCode2 = Z.ZipCode2(+)                                          ";
                if (clsPmpaPb.GstrFal == "1")
                {
                    SQL += ComNum.VBLF + "  AND Sname LIKE '" + clsPmpaPb.GstrName + "%'                            ";
                }
                else
                {
                    SQL += ComNum.VBLF + "  AND Pano = '" + clsPmpaPb.GstrPANO + "'                                 ";
                }
                SQL += ComNum.VBLF + "GROUP BY Pano, Sname, Sex, Jumin1, Jumin2, startdate, Lastdate,               ";
                SQL += ComNum.VBLF + "JiName, P.ZipCode1, P.ZipCode2, ZipName1, ZipName2, ZipName3, Juso, Tel       ";
                SQL += ComNum.VBLF + "ORDER BY  P.JUMIN1, Sname, Pano                                               ";

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

                        ssList_Sheet1.Rows.Count = nREAD;

                        for (i = 0; i < nREAD; i++)
                        {
                            ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                            ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Sname"].ToString().Trim();
                            ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Sex"].ToString().Trim();
                            ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Jumin1"].ToString().Trim() + "-" + dt.Rows[i]["Jumin2"].ToString().Trim();
                            ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["LastDate"].ToString().Trim();
                            ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["JiName"].ToString().Trim();
                            ssList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Tel"].ToString().Trim();
                            ssList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["Juso"].ToString().Trim();
                            ssList_Sheet1.Cells[i, 8].Text = dt.Rows[i]["StartDate"].ToString().Trim();
                            ssList_Sheet1.Cells[i, 9].Text = dt.Rows[i]["ZipCode1"].ToString().Trim() + "-" + dt.Rows[i]["ZipCode2"].ToString().Trim();
                            ssList_Sheet1.Cells[i, 10].Text = dt.Rows[i]["ZipName1"].ToString().Trim() + " " + dt.Rows[i]["ZipName2"].ToString().Trim() + " " + dt.Rows[i]["ZipName3"].ToString().Trim();
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

                ComFunc.Form_Center(this);
            }


        }

        void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            clsPmpaPb.GstrPANO = ssList_Sheet1.Cells[e.Row, 0].Text;                   //등록번호
            clsPmpaPb.GstrSname = ssList_Sheet1.Cells[e.Row, 1].Text;                  //수신자명
            clsPmpaPb.GstrSex = ssList_Sheet1.Cells[e.Row, 2].Text;                    //성별
            clsPmpaPb.GstrJumin1 = VB.Left(ssList_Sheet1.Cells[e.Row, 3].Text, 6);     //주민등록번호
            clsPmpaPb.GstrJumin2 = VB.Right(ssList_Sheet1.Cells[e.Row, 3].Text, 7);    //주민등록번호

            clsPmpaPb.GstrLastDate = ssList_Sheet1.Cells[e.Row, 4].Text;               //최종내원일
            clsPmpaPb.GstrJiname = ssList_Sheet1.Cells[e.Row, 5].Text;                 //지역
            clsPmpaPb.GstrJuso = ssList_Sheet1.Cells[e.Row, 7].Text;                   //주소
            clsPmpaPb.GstrStartDate = ssList_Sheet1.Cells[e.Row, 8].Text;              //최초내원일
            clsPmpaPb.GstrZipCode = ssList_Sheet1.Cells[e.Row, 9].Text;                //우편번호
            clsPmpaPb.GstrJuso1 = ssList_Sheet1.Cells[e.Row, 10].Text;                 //주소1

            this.Close();
        }
    }
}
