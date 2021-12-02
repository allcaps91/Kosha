using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComMirLibB.Com
{
    /// Class Name      : ComMirLibB.dll
    /// File Name       : frmComMirJinHistory.cs
    /// Description     : 진료상병조회(History)
    /// Author          : 박성완
    /// Create Date     : 2017-12-28
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 부모폼 완성후 나머지 부분 구현 필요
    /// </history>
    /// <vbp>
    /// default : VB\PSMHH\mir\JINHIS.FRM
    public partial class frmComMirJinHistory : Form
    {
        private string FstrPaNo = "";
        private string FstrBi;

        public frmComMirJinHistory(string strPaNo, string strBi)
        {
            FstrPaNo = strPaNo;
            FstrBi = strBi;

            InitializeComponent();

            SetEvent();
        }

        private void SetEvent()
        {
            this.Load += FrmComMirJinHistory_Load;
            this.ss2List.CellDoubleClick += Ss2List_CellDoubleClick;
            this.ss2Main.CellDoubleClick += Ss2Main_CellDoubleClick;
            this.btnExit.Click += new EventHandler(eBtnClick);
        }
        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;

                //this.Hide();
                this.Close();
                return;
            }
        }
        private void Ss2Main_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            //TODO: 부모폼 완성시 구현한다.
            if (e.ColumnHeader == true || e.RowHeader == true)
            {
                return;
            }

            string strIll = "";

            strIll = ss2Main.ActiveSheet.Cells[e.Row, 2].Text;

        }

        private void Ss2List_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true || e.RowHeader == true)
                return;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strString = "";
            string strInDate = "";
            long nIPDNO = 0;
            long nTRSNo = 0;
            string strGbIPD = "";
            int nRow = 0;

            strGbIPD = ss2List.ActiveSheet.Cells[e.Row, 1].Text == "지병" ? "9" : "";
            strInDate = ss2List.ActiveSheet.Cells[e.Row, 4].Text;
            strString += "  입원일: " + strInDate;
            nIPDNO = Convert.ToInt64(ss2List.ActiveSheet.Cells[e.Row, 8].Text);
            nTRSNo = Convert.ToInt64(ss2List.ActiveSheet.Cells[e.Row, 9].Text);

            SQL = "";
            SQL += " SELECT RowId,IllCode,IllName,RANK,REMARK, GBILL  FROM MIR_ILLS                   " + ComNum.VBLF;
            SQL += "  WHERE Pano = '" + FstrPaNo + "'                              " + ComNum.VBLF;
            SQL += "    AND Bi = '" + FstrBi + "'                                 " + ComNum.VBLF;
            SQL += "    AND IpdOpd = 'I'                                          " + ComNum.VBLF;
            SQL += "    AND InDate  = TO_DATE('" + strInDate + "','YYYY-MM-DD')  " + ComNum.VBLF;
            SQL += "    AND (IPDNO IS NULL OR IPDNO = " + nIPDNO + ")" + ComNum.VBLF;
            SQL += "    AND TRSNO =" + nTRSNo + "  " + ComNum.VBLF; //2009-09-14 윤조연 추가함
            if (strGbIPD == "9") { SQL += "  AND GBIPD = '9' " + ComNum.VBLF; }
            SQL += "  ORDER BY Rank,IllCode                                       " + ComNum.VBLF;
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            ss2Main.ActiveSheet.Rows.Count = 0;
            ss2Main.ActiveSheet.Rows.Count = dt.Rows.Count;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["RANK"].ToString() == "0")
                {
                    txtRemark1.Text = dt.Rows[i]["IllName"].ToString();
                    txtRemark2.Text = dt.Rows[i]["REMARK"].ToString();
                }
                else
                {
                    ss2Main.ActiveSheet.Cells[nRow, 1].Text = dt.Rows[i]["RANK"].ToString();
                    ss2Main.ActiveSheet.Cells[nRow, 2].Text = dt.Rows[i]["IllCode"].ToString();
                    ss2Main.ActiveSheet.Cells[nRow, 3].Text = dt.Rows[i]["GBILL"].ToString();
                    ss2Main.ActiveSheet.Cells[nRow, 4].Text = dt.Rows[i]["IllName"].ToString();
                    nRow++;
                }
            }

            dt.Dispose();
            dt = null;
        }

        private void FrmComMirJinHistory_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) this.Close(); //폼 권한 조회                           
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인	

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "";
                SQL += "SELECT IpdOpd,TO_CHAR(SDate,'yyyy-mm-dd') Sdate," + ComNum.VBLF;
                SQL += "       TO_CHAR(EDate,'yyyy-mm-dd') Edate,Ilsu,DeptCode," + ComNum.VBLF;
                SQL += "       DrName,Bi,Pname,Kiho,Gkiho,Tamt,Jamt,Gamt," + ComNum.VBLF;
                SQL += "       Mamt,Yamt,IllCode1,IllCode2,IllCode3" + ComNum.VBLF;
                SQL += "  FROM KOSMOS_PMPA.BAS_HISTORY H, KOSMOS_PMPA.BAS_DOCTOR D" + ComNum.VBLF;
                SQL += " WHERE H.Pano = '" + FstrPaNo + "'" + ComNum.VBLF;
                SQL += "   AND H.DrCode = D.DrCode(+)" + ComNum.VBLF;
                SQL += "    AND H.SDATE >=TRUNC(SYSDATE -180) " + ComNum.VBLF;
                SQL += " ORDER BY Sdate DESC " + ComNum.VBLF;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);


                ss1Main_Sheet1.Rows.Count = dt.Rows.Count;
                ss1Main_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ss1Main.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["IpdOpd"].ToString();
                    ss1Main.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["Sdate"].ToString();
                    ss1Main.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["Edate"].ToString();
                    ss1Main.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["Ilsu"].ToString();
                    ss1Main.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["DeptCode"].ToString();
                    ss1Main.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["IllCode1"].ToString();
                    ss1Main.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["IllCode2"].ToString();
                    ss1Main.ActiveSheet.Cells[i, 7].Text = dt.Rows[i]["IllCode3"].ToString();
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL += " SELECT TRSNO, IPDNO, GBSTS, PANO, BI, DeptCode, GBIPD, ILSU, " + ComNum.VBLF;
                SQL += " TO_CHAR(InDate,'yyyy-mm-dd') INDATE,   " + ComNum.VBLF;
                SQL += " TO_CHAR(OUTDATE,'yyyy-mm-dd') OUTDATE   " + ComNum.VBLF;
                SQL += "   FROM KOSMOS_PMPA.IPD_TRANS                                             " + ComNum.VBLF;
                SQL += "  WHERE Pano = '" + FstrPaNo + "'                          " + ComNum.VBLF;
                SQL += "    AND GBIPD NOT IN ('D')" + ComNum.VBLF;
                SQL += "  ORDER BY INDATE DESC " + ComNum.VBLF;
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                ss2List.ActiveSheet.Rows.Count = 0;
                ss2List.ActiveSheet.Rows.Count = dt.Rows.Count;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ss2List.ActiveSheet.Cells[i, 0].Text = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "IPD_입원상태", dt.Rows[i]["GBSTS"].ToString());
                    ss2List.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["GBIPD"].ToString() == "9" ? "지병" : "";
                    ss2List.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["PANO"].ToString();
                    ss2List.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["BI"].ToString();
                    ss2List.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["INDATE"].ToString();
                    ss2List.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["OUTDATE"].ToString();
                    ss2List.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["DEPTCODE"].ToString();
                    ss2List.ActiveSheet.Cells[i, 7].Text = dt.Rows[i]["ILSU"].ToString();
                    ss2List.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["IPDNO"].ToString();
                    ss2List.ActiveSheet.Cells[i, 9].Text = dt.Rows[i]["TRSNO"].ToString();
                }

                dt.Dispose();
                dt = null;

                if(FstrBi != "52")
                {
                    SQL = "";
                    SQL += "SELECT YYMM, IpdOpd, JINDATE1, BI, DEPTCODE1, IllCode1,IllCode2,IllCode3" + ComNum.VBLF;
                    SQL += "  FROM MIR_INSID H" + ComNum.VBLF;
                    SQL += " WHERE H.Pano = '" + FstrPaNo + "'" + ComNum.VBLF;
                    SQL += "   AND H.EDIMIRNO > 0 " + ComNum.VBLF;
                    SQL += " ORDER BY YYMM DESC " + ComNum.VBLF;
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                }
                else
                {
                    SQL = "";
                    SQL += "SELECT YYMM, IpdOpd, TO_CHAR(OUTDATE,'YYYY-MM-DD') OUTDATE, '" + FstrBi + "' BI, DEPTCODE1, IllCode1,IllCode2,IllCode3" + ComNum.VBLF;
                    SQL += "  FROM MIR_TAID H" + ComNum.VBLF;
                    SQL += " WHERE H.Pano = '" + FstrPaNo + "'" + ComNum.VBLF;
                    SQL += "   AND H.EDIMIRNO > 0 " + ComNum.VBLF;
                    SQL += " ORDER BY YYMM DESC " + ComNum.VBLF;
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                }

                ss3Main.ActiveSheet.Rows.Count = 0;
                ss3Main.ActiveSheet.Rows.Count = dt.Rows.Count;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ss3Main.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["IPDOPD"].ToString();
                    ss3Main.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["YYMM"].ToString();
                    if(FstrBi != "52")
                    {
                        ss3Main.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["JINDATE1"].ToString();
                    }
                    else
                    {
                        ss3Main.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["OUTDATE"].ToString();
                    }
                    
                    ss3Main.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["BI"].ToString();
                    ss3Main.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["DeptCode1"].ToString();
                    ss3Main.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["IllCode1"].ToString();
                    ss3Main.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["IllCode2"].ToString();
                    ss3Main.ActiveSheet.Cells[i, 7].Text = dt.Rows[i]["IllCode3"].ToString();
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = " SELECT  'O' GBIO , A.BDATE, A.ILLCODE, A.DEPTCODE,  SEQNO,   B.ILLNAMEK" + ComNum.VBLF;
                SQL += " FROM KOSMOS_OCS.OCS_OILLS A, KOSMOS_PMPA.BAS_ILLS B" + ComNum.VBLF;
                SQL += " WHERE A.ILLCODE = B.ILLCODE(+)" + ComNum.VBLF;
                SQL += " AND PTNO ='" + FstrPaNo + "' " + ComNum.VBLF;
                SQL += " AND BDATE >= TRUNC(SYSDATE -180) " + ComNum.VBLF;
                SQL += " UNION ALL" + ComNum.VBLF;
                SQL += " SELECT 'I' GBIO,  A.BDATE, A.ILLCODE, A.DEPTCODE,  SEQNO,   B.ILLNAMEK" + ComNum.VBLF;
                SQL += " FROM KOSMOS_OCS.OCS_IILLS A, KOSMOS_PMPA.BAS_ILLS B" + ComNum.VBLF;
                SQL += " WHERE A.ILLCODE = B.ILLCODE(+)" + ComNum.VBLF;
                SQL += " AND PTNO ='" + FstrPaNo + "' " + ComNum.VBLF;
                SQL += " AND BDATE >= TRUNC(SYSDATE -180) " + ComNum.VBLF;
                SQL += " ORDER BY BDATE DESC , SEQNO" + ComNum.VBLF;
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                ss4Main.ActiveSheet.Rows.Count = 0;
                ss4Main.ActiveSheet.Rows.Count = dt.Rows.Count;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ss4Main.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["GBIO"].ToString();
                    ss4Main.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["BDATE"].ToString();
                    ss4Main.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["DEPTCODE"].ToString();
                    ss4Main.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["ILLCODE"].ToString();
                    ss4Main.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["ILLNAMEK"].ToString();
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }      
    }
}
