using ComBase;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComMirLibB.MirEnt; //청구기본 클래스us
using FarPoint.Win.Spread;

namespace ComMirLibB.Com
{
    /// Class Name      : ComMirLibB.dll
    /// File Name       : frmComMirOutDrugEntry.cs
    /// Description     : 원외처방전 등록
    /// Author          : 박성완
    /// Create Date     : 2018-01-09
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <vbp>
    /// default : VB\PSMHH\mir\FrmOutDrug.FRM
    public partial class frmComMirOutDrugEntry : Form
    {
        clsComMir.cls_Table_Mir_Insid TID = new clsComMir.cls_Table_Mir_Insid();
        clsComMirEntSpd ComMirEntSpd = new clsComMirEntSpd();

        public frmComMirOutDrugEntry(clsComMir.cls_Table_Mir_Insid cls_Table_Mir_Insid)
        {
            TID = cls_Table_Mir_Insid;

            InitializeComponent();

            SetEvent();
        }

        private void SetEvent()
        {
            this.Load += FrmComMirOutDrugEntry_Load;
            this.btnSearch.Click += BtnSearch_Click;
            this.btnSave.Click += BtnSave_Click;
            this.btnExit.Click += BtnExit_Click;

            this.ss1.ButtonClicked += Ss1_ButtonClicked;
            this.ss2.CellDoubleClick += Ss2_CellDoubleClick;

        }

        private void Ss2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            string strRowid = "";
            string strDel = "";
            string strGbSelf = "";

            int intRowAffected = 0; //변경된 Row 받는 변수
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            strRowid = ss2.ActiveSheet.Cells[e.Row, 9].Text.Trim();
            strDel = ss2.ActiveSheet.Cells[e.Row, 10].Text.Trim();
            strGbSelf = ss2.ActiveSheet.Cells[e.Row, 11].Text.Trim();

            Cursor.Current = Cursors.WaitCursor;


            try
            {
                if (e.Column == 11)
                {
                    clsDB.setBeginTran(clsDB.DbCon);

                    if (strGbSelf == "1")
                    {
                        strGbSelf = "0";
                    }
                    else if (strGbSelf == "2")
                    {
                        strGbSelf = "1";
                    }
                    else
                    {
                        strGbSelf = "2";
                    }

                    if (TID.Bi == "52")
                    {
                        SQL = "UPDATE KOSMOS_PMPA.MIR_TAOUTDRUG SET GBSELF= '" + strGbSelf + "' " + ComNum.VBLF;
                    }
                    else
                    {
                        SQL = "UPDATE KOSMOS_PMPA.MIR_OUTDRUG SET GBSELF= '" + strGbSelf + "' " + ComNum.VBLF;
                    }
                    SQL += " WHERE ROWID ='" + strRowid + "'" + ComNum.VBLF;
                    clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                    }
                    else
                    {
                        clsDB.setCommitTran(clsDB.DbCon);
                    }

                    ss2.ActiveSheet.Cells[e.Row, 11].Text = strGbSelf;
                    return;
                }

                if (e.RowHeader == false)
                {
                    return;
                }

                clsDB.setBeginTran(clsDB.DbCon);
                SQL = "";
                if (strDel == "D")
                {
                    if (TID.Bi == "52")
                    {
                        SQL += "UPDATE KOSMOS_PMPA.MIR_TAOUTDRUG SET FLAG='Y'";
                    }
                    else
                    {
                        SQL += "UPDATE KOSMOS_PMPA.MIR_OUTDRUG SET FLAG='Y'";
                    }
                }
                else
                {
                    if (TID.Bi == "52")
                    {
                        SQL += "UPDATE KOSMOS_PMPA.MIR_TAOUTDRUG SET FLAG='D'";
                    }
                    else
                    {
                        SQL += "UPDATE KOSMOS_PMPA.MIR_OUTDRUG SET FLAG='D'";
                    }
                }
                //SQL += "UPDATE KOSMOS_PMPA.MIR_OUTDRUG SET FLAG='D'";
                SQL += " WHERE ROWID ='" + strRowid + "'" + ComNum.VBLF;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                }
                else
                {
                    clsDB.setCommitTran(clsDB.DbCon);
                    if (strDel == "D")
                    {
                        ss2.ActiveSheet.Rows[e.Row].BackColor = Color.FromArgb(255, 255, 255);
                        ss2.ActiveSheet.Cells[e.Row, 10].Text = "";
                    }
                    else
                    {
                        ss2.ActiveSheet.Rows[e.Row].BackColor = Color.FromArgb(255, 0, 0);
                        ss2.ActiveSheet.Cells[e.Row, 10].Text = "D";
                    }
                }

                Cursor.Current = Cursors.Default;
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

        private void Ss1_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (ss1.ActiveSheet.Cells[e.Row, 0].Text == "True")
            {
                ss1.ActiveSheet.Rows[e.Row].BackColor = Color.Yellow;
            }
            else
            {
                ss1.ActiveSheet.Rows[e.Row].BackColor = Color.White;
            }        
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            //TODO:청구 메인 완성후 구현 필요 
            //BaseMir 에서 메인폼 컨트롤 제어
            //Call READ_MIR_OUTDRUG
            this.Close();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void UpdateData()
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            long nWrtno = 0;
            string strChk = "";
            string strRowid = "";
            string strFlag = "";
            string strSlip = "";
            string strSlipNo = "";
            double nMaxNal = 0;
                
            int intRowAffected = 0; //변경된 Row 받는 변수
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (int i = 0; i < ss1.ActiveSheet.NonEmptyRowCount; i++)
                {
                    strChk = ss1.ActiveSheet.Cells[i, 0].Text;
                    strSlip = VB.Left(ss1.ActiveSheet.Cells[i, 1].Text, 8);
                    strSlipNo = VB.Mid(ss1.ActiveSheet.Cells[i, 1].Text, 10, 5);
                    strRowid = ss1.ActiveSheet.Cells[i, 10].Text;
                    nMaxNal = VB.Val(ss1.ActiveSheet.Cells[i, 12].Text);
                    nWrtno = 0;
                    strFlag = "0";

                    if (strChk == "True")
                    {
                        nWrtno = TID.WRTNO;
                        strFlag = "P";
                    }

                    if (TID.Bi == "52")
                    {
                        SQL = "";
                        SQL += "UPDATE KOSMOS_PMPA.MIR_TAOUTDRUGMST SET GBMIR ='Y', MAXNAL = " + nMaxNal + ", WRTNO=" + nWrtno + " , FLAG ='" + strFlag + "'" + ComNum.VBLF;
                        SQL += " WHERE ROWID='" + strRowid + "' " + ComNum.VBLF;
                    }
                    else
                    {
                        SQL = "";
                        SQL += "UPDATE KOSMOS_PMPA.MIR_OUTDRUGMST SET GBMIR ='Y', MAXNAL = " + nMaxNal + ", WRTNO=" + nWrtno + " , FLAG ='" + strFlag + "'" + ComNum.VBLF;
                        SQL += " WHERE ROWID='" + strRowid + "' " + ComNum.VBLF;

                    }
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        MessageBox.Show("UPDATE MIR_OUTDRUGMST ERROR!!!");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }                    
                }

                clsDB.setCommitTran(clsDB.DbCon);

                //TODO:청구 메인 완성후 구현 필요 
                //BaseMir 에서 메인폼 컨트롤 제어
                //Call READ_MIR_OUTDRUG

                Cursor.Current = Cursors.Default;
                DialogResult = System.Windows.Forms.DialogResult.OK;
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



        private void BtnSearch_Click(object sender, EventArgs e)
        {

            if (TID.Bi == "52")
            {
                READ_OUTDRUGMST_TA("ALL");
            }
            else
            {
                READ_OUTDRUGMST("ALL");
            }

            
        }

        private void FrmComMirOutDrugEntry_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) this.Close(); //폼 권한 조회                           
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ComMirEntSpd.setColStyle_number(ss1, -1, 12, 0, 999, -999, CellHorizontalAlignment.Right, CellVerticalAlignment.Center);

            if (TID.Bi == "52")
            {
                READ_OUTDRUGMST_TA();
            }
            else
            {
                READ_OUTDRUGMST();
            }
        }

        private void READ_OUTDRUGMST_TA(string argGb = "")
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인	

            clsComMirMethod method = new clsComMirMethod();

            string strSlipDate = "";
            int? nSlipNo = 0;
            string strFlag = "";
            string strSDate = "";
            string strEDate = "";
            int nRowss2 = 0;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            DataTable dt1 = null;

            if (TID.Bi == "31")
            {
                strSDate = TID.FrDate;
            }
            else
            {
                if (TID.JinDate1.Length != 8) { return; }
                if (TID.JinDate1.Trim() == "") { return; }
                strSDate = VB.Left(TID.JinDate1, 4) + "-" + VB.Mid(TID.JinDate1, 5, 2) + "-" + VB.Right(TID.JinDate1, 2);
            }

            DateTime firstDay = new DateTime(Convert.ToInt16(VB.Left(TID.JinDate1, 4)), Convert.ToInt16(VB.Mid(TID.JinDate1, 5, 2)), 01);
            strEDate = firstDay.AddMonths(1).AddDays(-1).ToShortDateString();

            lblPano.Text = TID.Pano;
            lblSName.Text = TID.SName.Trim();
            lblDtName.Text = method.getDtName_Dtno(TID.DTno);
            lblWrtno.Text = TID.WRTNO.ToString().Trim();

            Cursor.Current = Cursors.WaitCursor;

            ss1.ActiveSheet.Columns[10].Visible = false;

            try
            {
                //원외처방전 자료 READ
                SQL = "" + ComNum.VBLF;
                SQL += "SELECT TO_CHAR(a.SlipDate,'YYYYMMDD') SlipDate,a.SlipNo," + ComNum.VBLF;
                SQL += "       TO_CHAR(a.BDate,'YYYY-MM-DD') BDate,a.Bi,a.DeptCode," + ComNum.VBLF;
                SQL += "       TO_CHAR(a.ActDate,'YYYY-MM-DD') ActDate, a.WRTNO, A.Flag, a.MAXNAL," + ComNum.VBLF;
                SQL += "       MAX(DECODE(b.Bun,'11',b.Nal,0)) Bun11," + ComNum.VBLF;
                SQL += "       MAX(DECODE(b.Bun,'12',b.Nal,0)) Bun12," + ComNum.VBLF;
                SQL += "       MAX(DECODE(b.Bun,'20',b.Nal,0)) Bun20, A.ROWID AROWID" + ComNum.VBLF;
                SQL += "  FROM KOSMOS_PMPA.MIR_TAOUTDRUGMST a, KOSMOS_PMPA.MIR_TAOUTDRUG b " + ComNum.VBLF;
                SQL += " WHERE a.Pano = '" + TID.Pano + "' " + ComNum.VBLF;
                SQL += "   AND a.BDate >= TO_DATE('" + strSDate + "','YYYY-MM-DD') " + ComNum.VBLF;
                SQL += "   AND a.BDate <= TO_DATE('" + strEDate + "','YYYY-MM-DD') " + ComNum.VBLF;
                
                if (argGb == "ALL")
                {
                    SQL += "   AND (a.WRTNO = 0 OR a.WRTNO IS NULL OR a.WRTNO=" + TID.WRTNO + " ) " + ComNum.VBLF;
                }
                else
                {
                    SQL += "   AND a.Flag = 'P' " + ComNum.VBLF;  //인쇄한 원외처방전
                    SQL += "   AND (a.WRTNO = 0 OR a.WRTNO IS NULL OR a.WRTNO=" + TID.WRTNO + ") " + ComNum.VBLF;
                }

                if (TID.Bi == "31")
                {
                    SQL += "  AND BI ='31'" + ComNum.VBLF;
                }
                else
                {
                    SQL += "  AND BI NOT IN ('31') " + ComNum.VBLF;
                }
                SQL += "   AND a.SlipDate=b.SlipDate(+) " + ComNum.VBLF;
                SQL += "   AND a.SlipNo  =b.SlipNo(+)   " + ComNum.VBLF;
                SQL += " GROUP BY a.SlipDate,a.SlipNo,a.BDate,a.Bi,a.DeptCode,a.ActDate,a.WRTNO,a.ROWID, A.FLAG, A.MAXNAL " + ComNum.VBLF;
                SQL += " ORDER BY a.SlipDate,a.SlipNo,a.BDate,a.Bi,a.DeptCode " + ComNum.VBLF;


                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ss1.ActiveSheet.Rows.Count = 0;
                ss2.ActiveSheet.Rows.Count = 0;
                ss1.ActiveSheet.Rows.Count = dt.Rows.Count;
                ss1.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);
                ss2.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ss1.ActiveSheet.Cells[i, 1].Text = $"{dt.Rows[i]["SLIPDATE"].ToString()}-{dt.Rows[i]["SLipNo"].ToString().PadLeft(5, '0')}";
                        ss1.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["BDate"].ToString();
                        ss1.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["Bi"].ToString();
                        ss1.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["DeptCode"].ToString();
                        ss1.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["ActDate"].ToString();
                        ss1.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["Bun11"].ToString();
                        ss1.ActiveSheet.Cells[i, 7].Text = dt.Rows[i]["Bun12"].ToString();
                        ss1.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["Bun20"].ToString();
                        ss1.ActiveSheet.Cells[i, 9].Text = dt.Rows[i]["WRTNO"].ToString();
                        ss1.ActiveSheet.Cells[i, 10].Text = dt.Rows[i]["AROWID"].ToString();

                        ss1.ActiveSheet.Cells[i, 11].Text = dt.Rows[i]["FLAG"].ToString() == "P" ? "인쇄" : "미인쇄";
                        ss1.ActiveSheet.Cells[i, 12].Text = dt.Rows[i]["MAXNAL"].ToString();

                        if (Convert.ToInt64(dt.Rows[i]["WRTNO"]) == TID.WRTNO)
                        {
                            ss1.ActiveSheet.Cells[i, 0].Text = "True";
                            ss1.ActiveSheet.Rows[i].BackColor = Color.Yellow;
                        }

                        strSlipDate = dt.Rows[i]["SlipDate"].ToString();
                        nSlipNo = Convert.ToInt32(dt?.Rows?[i]["SlipNo"]);

                        SQL = "" + ComNum.VBLF;
                        SQL += "SELECT a.Bun,a.SuCode,a.DivQty,a.Div,a.Nal,a.EdiCode,a.EdiQty, a.Flag , a.GBSELF, " + ComNum.VBLF;
                        SQL += "      b.SuNameK,c.Pname,c.Danwi1,c.Danwi2,c.Spec,a.ROWID " + ComNum.VBLF;
                        SQL += " FROM KOSMOS_PMPA.MIR_TAOUTDRUG a,BAS_SUN b,EDI_SUGA c " + ComNum.VBLF;
                        SQL += " WHERE a.Pano = '" + TID.Pano + "' " + ComNum.VBLF;
                        SQL += "  AND  a.SlipDate = TO_DATE('" + strSlipDate + "', 'YYYYMMDD') " + ComNum.VBLF;
                        SQL += "  AND a.SlipNo = " + nSlipNo + " " + ComNum.VBLF;
                        SQL += "  AND a.SuCode = b.SuNext " + ComNum.VBLF;
                        SQL += "  AND a.EdiCode = c.Code(+) " + ComNum.VBLF;
                        SQL += "   AND a.WRTNO=" + TID.WRTNO + " " + ComNum.VBLF;
                        SQL += "ORDER BY a.Bun,a.SuCode " + ComNum.VBLF;
                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);




                        for (int j = 0; j < dt1.Rows.Count; j++)
                        {
                            ss2.ActiveSheet.Rows.Count++;

                            if (strFlag != VB.Right(strSlipDate, 4) + "-" + nSlipNo)
                            {
                                ss2.ActiveSheet.Cells[nRowss2, 0].Text = VB.Right(strSlipDate, 4) + "-" + nSlipNo;
                                strFlag = VB.Right(strSlipDate, 4) + "-" + nSlipNo;
                            }

                            ss2.ActiveSheet.Cells[nRowss2, 1].Text = dt1.Rows[j]["SuCode"].ToString();
                            ss2.ActiveSheet.Cells[nRowss2, 2].Text = string.Format("{0:#####0.00}", dt1.Rows[j]["DivQty"]);
                            ss2.ActiveSheet.Cells[nRowss2, 3].Text = dt1.Rows[j]["Div"].ToString().Trim();
                            ss2.ActiveSheet.Cells[nRowss2, 4].Text = dt1.Rows[j]["Nal"].ToString().Trim();
                            ss2.ActiveSheet.Cells[nRowss2, 5].Text = dt1.Rows[j]["EdiCode"].ToString().Trim();
                            ss2.ActiveSheet.Cells[nRowss2, 6].Text = string.Format("{0:#####0.00}", dt1.Rows[j]["EdiQty"]);
                            ss2.ActiveSheet.Cells[nRowss2, 7].Text = dt1.Rows[j]["SuNameK"].ToString();
                            ss2.ActiveSheet.Cells[nRowss2, 8].Text = dt1.Rows[j]["Pname"].ToString();
                            ss2.ActiveSheet.Cells[nRowss2, 9].Text = dt1.Rows[j]["ROWID"].ToString();
                            ss2.ActiveSheet.Cells[nRowss2, 10].Text = dt1.Rows[j]["Flag"].ToString();
                            ss2.ActiveSheet.Cells[nRowss2, 11].Text = dt1.Rows[j]["GBSELF"].ToString();

                            if (dt1.Rows[j]["Flag"].ToString() == "D")
                            {
                                ss2.ActiveSheet.Rows[nRowss2].BackColor = Color.FromArgb(255, 200, 200);
                            }

                            nRowss2++;
                        }

                        dt1.Dispose();
                        dt1 = null;

                    }
                    dt.Dispose();
                    dt = null;
                }

                ss2.ActiveSheet.Columns[9].Visible = false;
                ss2.ActiveSheet.Columns[10].Visible = false;

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


        private void READ_OUTDRUGMST(string argGb = "")
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인	

            clsComMirMethod method = new clsComMirMethod();

            string strSlipDate = "";
            int? nSlipNo = 0;
            string strFlag = "";
            string strSDate = "";
            string strEDate = "";
            int nRowss2 = 0;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            DataTable dt1 = null;

            if (TID.Bi == "31")
            {
                strSDate = TID.FrDate;
            }
            else
            {
                if (TID.JinDate1.Length != 8) { return; }
                if (TID.JinDate1.Trim() == "") { return; }
                strSDate = VB.Left(TID.JinDate1, 4) + "-" + VB.Mid(TID.JinDate1, 5, 2) + "-" + VB.Right(TID.JinDate1, 2);
            }

            DateTime firstDay = new DateTime(Convert.ToInt16(VB.Left(TID.JinDate1, 4)), Convert.ToInt16(VB.Mid(TID.JinDate1, 5, 2)), 01);
            strEDate = firstDay.AddMonths(1).AddDays(-1).ToShortDateString();

            lblPano.Text = TID.Pano;
            lblSName.Text = TID.SName.Trim();
            lblDtName.Text = method.getDtName_Dtno(TID.DTno);
            lblWrtno.Text = TID.WRTNO.ToString().Trim();

            Cursor.Current = Cursors.WaitCursor;

            ss1.ActiveSheet.Columns[10].Visible = false;

            try
            {
                //원외처방전 자료 READ
                SQL = "" + ComNum.VBLF;
                SQL += "SELECT TO_CHAR(a.SlipDate,'YYYYMMDD') SlipDate,a.SlipNo," + ComNum.VBLF;
                SQL += "       TO_CHAR(a.BDate,'YYYY-MM-DD') BDate,a.Bi,a.DeptCode," + ComNum.VBLF;
                SQL += "       TO_CHAR(a.ActDate,'YYYY-MM-DD') ActDate, a.WRTNO, A.Flag, a.MAXNAL," + ComNum.VBLF;
                SQL += "       MAX(DECODE(b.Bun,'11',b.Nal,0)) Bun11," + ComNum.VBLF;
                SQL += "       MAX(DECODE(b.Bun,'12',b.Nal,0)) Bun12," + ComNum.VBLF;
                SQL += "       MAX(DECODE(b.Bun,'20',b.Nal,0)) Bun20, A.ROWID AROWID, A.DIEASE1 DIEASE1" + ComNum.VBLF;//2019-05-22 DIEASE1 처방전발행 상병 KHS
                SQL += "  FROM KOSMOS_PMPA.MIR_OUTDRUGMST a, KOSMOS_PMPA.MIR_OUTDRUG b " + ComNum.VBLF;
                SQL += " WHERE a.Pano = '" + TID.Pano + "' " + ComNum.VBLF;
                SQL += "   AND a.BDate >= TO_DATE('" + strSDate + "','YYYY-MM-DD') " + ComNum.VBLF;
                SQL += "   AND a.BDate <= TO_DATE('" + strEDate + "','YYYY-MM-DD') " + ComNum.VBLF;

                if (argGb == "ALL")
                {
                    SQL += "   AND (a.WRTNO = 0 OR a.WRTNO IS NULL OR a.WRTNO=" + TID.WRTNO + " ) " + ComNum.VBLF;
                }
                else
                {
                    SQL += "   AND a.Flag = 'P' " + ComNum.VBLF;  //인쇄한 원외처방전
                    SQL += "   AND (a.WRTNO = 0 OR a.WRTNO IS NULL OR a.WRTNO=" + TID.WRTNO + ") " + ComNum.VBLF;
                }

                if (TID.Bi == "31")
                {
                    SQL += "  AND BI ='31'" + ComNum.VBLF;
                }
                else
                {
                    SQL += "  AND BI NOT IN ('31') " + ComNum.VBLF;
                }
                SQL += "   AND a.SlipDate=b.SlipDate(+) " + ComNum.VBLF;
                SQL += "   AND a.SlipNo  =b.SlipNo(+)   " + ComNum.VBLF;
                SQL += " GROUP BY a.SlipDate,a.SlipNo,a.BDate,a.Bi,a.DeptCode,a.ActDate,a.WRTNO,a.ROWID, A.FLAG, A.MAXNAL, A.DIEASE1 " + ComNum.VBLF;//2019-05-22 DIEASE1 처방전발행 상병 KHS
                SQL += " ORDER BY a.SlipDate,a.SlipNo,a.BDate,a.Bi,a.DeptCode " + ComNum.VBLF;


                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ss1.ActiveSheet.Rows.Count = 0;
                ss2.ActiveSheet.Rows.Count = 0;
                ss1.ActiveSheet.Rows.Count = dt.Rows.Count;
                ss1.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);
                ss2.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ss1.ActiveSheet.Cells[i, 1].Text = $"{dt.Rows[i]["SLIPDATE"].ToString()}-{dt.Rows[i]["SLipNo"].ToString().PadLeft(5, '0')}";
                        ss1.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["BDate"].ToString();
                        ss1.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["Bi"].ToString();
                        ss1.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["DeptCode"].ToString();
                        ss1.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["ActDate"].ToString();
                        ss1.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["Bun11"].ToString();
                        ss1.ActiveSheet.Cells[i, 7].Text = dt.Rows[i]["Bun12"].ToString();
                        ss1.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["Bun20"].ToString();
                        ss1.ActiveSheet.Cells[i, 9].Text = dt.Rows[i]["WRTNO"].ToString();
                        ss1.ActiveSheet.Cells[i, 10].Text = dt.Rows[i]["AROWID"].ToString();

                        ss1.ActiveSheet.Cells[i, 11].Text = dt.Rows[i]["FLAG"].ToString() == "P" ? "인쇄" : "미인쇄";
                        ss1.ActiveSheet.Cells[i, 12].Text = dt.Rows[i]["MAXNAL"].ToString();
                        ss1.ActiveSheet.Cells[i, 13].Text = dt.Rows[i]["DIEASE1"].ToString();//2019-05-22 DIEASE1 처방전발행 상병 KHS

                        if (Convert.ToInt64(dt.Rows[i]["WRTNO"]) == TID.WRTNO)
                        {
                            ss1.ActiveSheet.Cells[i, 0].Text = "True";
                            ss1.ActiveSheet.Rows[i].BackColor = Color.Yellow;
                        }

                        strSlipDate = dt.Rows[i]["SlipDate"].ToString();
                        nSlipNo = Convert.ToInt32(dt?.Rows?[i]["SlipNo"]);

                        SQL = "" + ComNum.VBLF;
                        SQL += "SELECT a.Bun,a.SuCode,a.DivQty,a.Div,a.Nal,a.EdiCode,a.EdiQty, a.Flag , a.GBSELF, " + ComNum.VBLF;
                        SQL += "      b.SuNameK,c.Pname,c.Danwi1,c.Danwi2,c.Spec,a.ROWID " + ComNum.VBLF;
                        SQL += " FROM KOSMOS_PMPA.MIR_OUTDRUG a,BAS_SUN b,EDI_SUGA c " + ComNum.VBLF;
                        SQL += " WHERE a.Pano = '" + TID.Pano + "' " + ComNum.VBLF;
                        SQL += "  AND  a.SlipDate = TO_DATE('" + strSlipDate + "', 'YYYYMMDD') " + ComNum.VBLF;
                        SQL += "  AND a.SlipNo = " + nSlipNo + " " + ComNum.VBLF;
                        SQL += "  AND a.SuCode = b.SuNext " + ComNum.VBLF;
                        SQL += "  AND a.EdiCode = c.Code(+) " + ComNum.VBLF;
                        SQL += "   AND a.WRTNO=" + TID.WRTNO + " " + ComNum.VBLF;
                        SQL += "ORDER BY a.Bun,a.SuCode " + ComNum.VBLF;
                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);




                        for (int j = 0; j < dt1.Rows.Count; j++)
                        {
                            ss2.ActiveSheet.Rows.Count++;

                            if (strFlag != VB.Right(strSlipDate, 4) + "-" + nSlipNo)
                            {
                                ss2.ActiveSheet.Cells[nRowss2, 0].Text = VB.Right(strSlipDate, 4) + "-" + nSlipNo;
                                strFlag = VB.Right(strSlipDate, 4) + "-" + nSlipNo;
                            }

                            ss2.ActiveSheet.Cells[nRowss2, 1].Text = dt1.Rows[j]["SuCode"].ToString();
                            ss2.ActiveSheet.Cells[nRowss2, 2].Text = string.Format("{0:#####0.00}", dt1.Rows[j]["DivQty"]);
                            ss2.ActiveSheet.Cells[nRowss2, 3].Text = dt1.Rows[j]["Div"].ToString().Trim();
                            ss2.ActiveSheet.Cells[nRowss2, 4].Text = dt1.Rows[j]["Nal"].ToString().Trim();
                            ss2.ActiveSheet.Cells[nRowss2, 5].Text = dt1.Rows[j]["EdiCode"].ToString().Trim();
                            ss2.ActiveSheet.Cells[nRowss2, 6].Text = string.Format("{0:#####0.00}", dt1.Rows[j]["EdiQty"]);
                            ss2.ActiveSheet.Cells[nRowss2, 7].Text = dt1.Rows[j]["SuNameK"].ToString();
                            ss2.ActiveSheet.Cells[nRowss2, 8].Text = dt1.Rows[j]["Pname"].ToString();
                            ss2.ActiveSheet.Cells[nRowss2, 9].Text = dt1.Rows[j]["ROWID"].ToString();
                            ss2.ActiveSheet.Cells[nRowss2, 10].Text = dt1.Rows[j]["Flag"].ToString();
                            ss2.ActiveSheet.Cells[nRowss2, 11].Text = dt1.Rows[j]["GBSELF"].ToString();

                            if (dt1.Rows[j]["Flag"].ToString() == "D")
                            {
                                ss2.ActiveSheet.Rows[nRowss2].BackColor = Color.FromArgb(255, 200, 200);
                            }

                            nRowss2++;
                        }

                        dt1.Dispose();
                        dt1 = null;

                    }
                    dt.Dispose();
                    dt = null;
                }

                ss2.ActiveSheet.Columns[9].Visible = false;
                ss2.ActiveSheet.Columns[10].Visible = false;

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
