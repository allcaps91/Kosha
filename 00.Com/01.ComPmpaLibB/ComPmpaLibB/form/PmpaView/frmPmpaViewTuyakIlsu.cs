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
    /// File Name       : frmPmpaViewTuyakIlsu.cs
    /// Description     : 외래 처방(약) 일수 조회
    /// Author          : 안정수
    /// Create Date     : 2017-09-28
    /// Update History  : 
    /// <history>       
    /// d:\psmh\OPD\oviewa\OVIEWA16.FRM(FrmTuyakIlsu) => frmPmpaViewTuyakIlsu.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oviewa\OVIEWA16.FRM(FrmTuyakIlsu)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewTuyakIlsu : Form
    {
        ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread();

        public frmPmpaViewTuyakIlsu()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);
            this.btnCancel.Click += new EventHandler(eBtnEvent);
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

            CS.Spread_All_Clear(ssList);

            txtFIlsu.Text = "0";
            txtTIlsu.Text = "0";
        }

        void eBtnEvent(object sender, EventArgs e)
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

            else if (sender == this.btnPrint)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                ePrint();
            }

            else if (sender == this.btnCancel)
            {
                ComFunc.SetAllControlClear(groupBox1);
                CS.Spread_All_Clear(ssList);
                progressBar1.Value = 0;
            }
        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string SubTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            btnPrint.Enabled = false;

            #region //시트 히든

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;
            ssList.ActiveSheet.Cells[0, 11].Text = "zzz";
            ssList.ActiveSheet.Columns[11].Visible = false;


            #endregion

            strTitle = dtpFDate.Text + "부터 " + dtpTDate.Text + "까지 외래 ";
            SubTitle = txtFIlsu.Text + "일분-" + txtTIlsu.Text + "일분 약처방 명단";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(SubTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 100, 30);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, true, true, true, false, false, false, (float)0.7);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            #region //시트 히든 복원

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;
            #endregion

            btnPrint.Enabled = true;
        }

        void eGetData()
        {
            int i = 0;
            int nRow = 0;
            int nREAD = 0;
            int nIlsu = 0;

            string strNewData = "";
            string strOldData = "";

            CS.Spread_All_Clear(ssList);
            Cursor.Current = Cursors.WaitCursor;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt1 = null;

            btnView.Enabled = false;
            btnPrint.Enabled = false;

            progressBar1.Value = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                            ";
            SQL += ComNum.VBLF + "  DeptCode, DrCode, BIlja, Pano, Bi, Nal, SNAME, DRNAME,                          ";
            SQL += ComNum.VBLF + "  ZipCode1, ZipCode2, Juso, ZIPCODE, MAILJUSO                                     ";
            SQL += ComNum.VBLF + "FROM  (SELECT S.DeptCode, S.DrCode, TO_CHAR(S.Bdate,'YYYY-MM-DD') BIlja,          ";
            SQL += ComNum.VBLF + "              S.Pano, S.Bi, MAX(S.Nal) Nal,                                       ";
            SQL += ComNum.VBLF + "              MAX(SNAME) SNAME, MAX(DRNAME) DRNAME,                               ";
            SQL += ComNum.VBLF + "              P.ZipCode1, P.ZipCode2, P.Juso, P.ZipCode1||P.ZipCode2 ZIPCODE      ";
            SQL += ComNum.VBLF + "      FROM OPD_SLIP        S,                                                     ";
            SQL += ComNum.VBLF + "           BAS_Patient     P,                                                     ";
            SQL += ComNum.VBLF + "           BAS_DOCTOR      D                                                      ";
            SQL += ComNum.VBLF + "      WHERE S.ActDate >= TO_DATE('" + dtpFDate.Text + "','yyyy-mm-dd')            ";
            SQL += ComNum.VBLF + "              AND S.Bdate BETWEEN TO_DATE('" + dtpFDate.Text + "','yyyy-mm-dd')   ";
            SQL += ComNum.VBLF + "                              AND TO_DATE('" + dtpTDate.Text + "','yyyy-mm-dd')   ";
            SQL += ComNum.VBLF + "              AND S.PANO   = P.PANO                                               ";
            SQL += ComNum.VBLF + "              AND S.Bun >= '11'                                                   ";
            SQL += ComNum.VBLF + "              AND S.Bun <= '21'                                                   ";
            SQL += ComNum.VBLF + "              AND S.NAL BETWEEN " + VB.Val(txtFIlsu.Text);
            SQL += ComNum.VBLF + "                            AND " + VB.Val(txtTIlsu.Text);
            SQL += ComNum.VBLF + "              AND S.GbGisul = '0'                                                 ";
            SQL += ComNum.VBLF + "              AND S.DRCODE  = D.DRCODE                                            ";
            SQL += ComNum.VBLF + "      GROUP BY S.DeptCode, S.DrCode, S.Bdate, S.Pano, S.Bi,                       ";
            SQL += ComNum.VBLF + "            P.ZipCode1 , P.ZipCode2, P.Juso, P.ZipCode1||P.ZipCode2               ";
            SQL += ComNum.VBLF + "         )             A,                                                         ";
            SQL += ComNum.VBLF + "         BAS_MAIL      B                                                          ";
            SQL += ComNum.VBLF + "WHERE  A.ZIPCODE = B.MAILCODE(+)                                                  ";
            SQL += ComNum.VBLF + "ORDER BY DeptCode, DrCode, BIlja, Pano, Bi                                        ";

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

                nRow = 0;
                strOldData = "";

                if (dt.Rows.Count > 0)
                {
                    nREAD = dt.Rows.Count;

                    for (i = 0; i < nREAD; i++)
                    {
                        nIlsu = Convert.ToInt32(dt.Rows[i]["Nal"].ToString().Trim());

                        nRow += 1;

                        if (nRow > ssList_Sheet1.Rows.Count)
                        {
                            ssList_Sheet1.Rows.Count = nRow;
                        }

                        strNewData = dt.Rows[i]["DeptCode"].ToString().Trim() + dt.Rows[i]["DrCode"].ToString().Trim();

                        if (strNewData != strOldData)
                        {
                            ssList_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                            ssList_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["DrName"].ToString().Trim();
                            strOldData = strNewData;
                        }

                        ssList_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["BIlja"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["Sname"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 5].Text = nIlsu.ToString();
                        ssList_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["Bi"].ToString().Trim();

                        //상병코드 READ
                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT                                                                        ";
                        SQL += ComNum.VBLF + "  a.IllCode,b.IllNameK                                                        ";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_ILLS a, " + ComNum.DB_PMPA + "BAS_ILLS b       ";
                        SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
                        SQL += ComNum.VBLF + "      AND a.YYMM = '" + VB.Mid(dt.Rows[i]["BIlja"].ToString().Trim(), 3, 2)
                                                                + VB.Mid(dt.Rows[i]["BIlja"].ToString().Trim(), 6, 2) + "'  ";
                        SQL += ComNum.VBLF + "      AND a.Pano = '" + dt.Rows[i]["Pano"].ToString().Trim() + "'             ";
                        SQL += ComNum.VBLF + "      AND a.DeptCode = '" + dt.Rows[i]["DeptCode"].ToString().Trim() + "'     ";
                        SQL += ComNum.VBLF + "      AND a.IllCode = b.IllCode                                               ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            ssList_Sheet1.Cells[nRow - 1, 7].Text = dt1.Rows[0]["IllNameK"].ToString().Trim();
                        }

                        else if (dt1.Rows.Count > 1)
                        {
                            ssList_Sheet1.Cells[nRow - 1, 8].Text = dt1.Rows[1]["IllNameK"].ToString().Trim();
                        }

                        dt1.Dispose();
                        dt1 = null;

                        ssList_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["MailJuso"].ToString().Trim() + " " + dt.Rows[i]["Juso"].ToString().Trim();

                        progressBar1.Value = VB.Fix((i + 1) / nREAD * 100);

                    }

                    progressBar1.Value = 100;
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

            ssList_Sheet1.Rows.Count = nRow;

            btnView.Enabled = true;
            btnPrint.Enabled = true;
            Cursor.Current = Cursors.Default;

        }
    }
}
