using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewGamekList.cs
    /// Description     : 감액내역조회
    /// Author          : 박창욱
    /// Create Date     : 2017-10-20
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\IPD\iument\Frm감액내역조회.frm(Frm감액내역조회.frm) >> frmPmpaViewGamekList.cs 폼이름 재정의" />	
    public partial class frmPmpaViewGamekList : Form
    {
        public frmPmpaViewGamekList()
        {
            InitializeComponent();
        }

        void Screen_Display()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nRead = 0;
            int nRow = 0;
            double nAmt = 0;
            double nBonin = 0;
            double nHalin = 0;
            double nRATE = 0;
            double nTTot1 = 0;
            double nBTot1 = 0;
            double nBTot2 = 0;  //본인부담합계
            double nHTot1 = 0;
            double nHTot2 = 0;  //할인액 합계
            string strBun = "";
            string strNu = "";
            string strSugbF = "";
            string strSugbK = "";
            string strDtlBun = "";
            string strDeptCode = "";

            try
            {
                //치과를 제외한 다른 진료과를 표시함
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT a.Bun, a.Nu, a.SuCode,";
                SQL = SQL + ComNum.VBLF + "        a.SuNext, c.SuNameK, b.SugbK,";
                SQL = SQL + ComNum.VBLF + "        b.SugbF, c.DtlBun, SUM(a.Amt1+a.Amt2) Amt ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a, " + ComNum.DB_PMPA + "BAS_SUT b, " + ComNum.DB_PMPA + "BAS_SUN c";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND a.TRSNO = " + clsPmpaType.TIT.Trsno;
                SQL = SQL + ComNum.VBLF + "    AND a.DeptCode <> 'DT'";
                SQL = SQL + ComNum.VBLF + "    AND a.Sucode = b.Sucode(+)";
                SQL = SQL + ComNum.VBLF + "    AND a.SuNext = c.SuNext(+)";
                SQL = SQL + ComNum.VBLF + "  GROUP BY a.Bun,a.Nu,a.SuCode,a.SuNext,c.SuNameK,b.SugbK,b.SugbF,c.DtlBun";
                SQL = SQL + ComNum.VBLF + "  ORDER BY a.Bun,a.Nu,a.SuCode,a.SuNext";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                nRead = dt.Rows.Count;

                for (i = 0; i < nRead; i++)
                {
                    strDeptCode = "**";
                    strBun = dt.Rows[i]["Bun"].ToString().Trim();
                    strNu = dt.Rows[i]["Nu"].ToString().Trim();
                    strSugbF = dt.Rows[i]["SugbF"].ToString().Trim();
                    strSugbK = dt.Rows[i]["SugbK"].ToString().Trim();
                    strDtlBun = dt.Rows[i]["DtlBun"].ToString().Trim();
                    nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());

                    #region DtlView_SUB

                    nRow += 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }

                    ssView_Sheet1.Cells[nRow - 1, 0].Text = strDeptCode;
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["SuNameK"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = strSugbK;
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = strSugbF;
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["Bun"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = strNu;
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = nAmt.ToString("#,##0");
                    ssView_Sheet1.Cells[nRow - 1, 11].Text = strDtlBun;

                    nBonin = 0;
                    nRATE = 0;
                    nHalin = 0;

                    //보험환자 CT, MRI
                    if ((clsPmpaType.TIT.Bi == "11" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "13") && (strBun == "72" || strBun == "73"))
                    {
                        if (string.Compare(strNu, "20") > 0)
                        {
                            nBonin = nAmt;
                        }
                        else
                        {
                            nBonin = nAmt * clsPmpaPb.OBON[(int)VB.Val(clsPmpaType.TIT.Bi)] / 100;
                        }
                    }
                    else
                    {
                        if (string.Compare(strNu, "20") > 0)
                        {
                            nBonin = nAmt;
                        }
                        else
                        {
                            nBonin = nAmt * clsPmpaType.TIT.BonRate / 100;
                        }
                    }

                    if (strNu == "34")  //비급여 식대
                    {
                        nRATE = clsPmpaType.GAM.FOOD_Rate;
                    }
                    else if (strNu == "35")  //병실차액
                    {
                        nRATE = clsPmpaType.GAM.ROOM_Rate;
                    }
                    else if (strNu == "36") //초음파
                    {
                        nRATE = clsPmpaType.GAM.SONO_Rate;
                    }
                    else if (strNu == "38") //MRI
                    {
                        nRATE = clsPmpaType.GAM.MRI_Rate;
                    }
                    //일반과(치과제외) 할인 대상금액을 누적
                    else if (strDeptCode != "DT")
                    {
                        if (strDtlBun == "0101")    //진찰료
                        {
                            nRATE = clsPmpaType.GAM.Jin_Rate;
                        }
                        else if (strDtlBun == "0103")   //응급관리료
                        {
                            nRATE = clsPmpaType.GAM.ER_Rate;
                        }
                        //재단성직자 감액(본인부담전액 감액)
                        else if (clsPmpaType.GAM.GbGameK == "11")
                        {
                            nRATE = clsPmpaType.GAM.Gam_Rate;
                        }
                        else if (strSugbK != "1")
                        {
                            nRATE = clsPmpaType.GAM.Gam_Rate;
                        }
                    }
                    //치과 할인대상 금액 누적
                    else
                    {
                        if (strDtlBun == "0101")    //진찰료
                        {
                            nRATE = clsPmpaType.GAM.DTJin_Rate;
                        }
                        else if (strSugbF == "1")   //치과 비급여
                        {
                            if (strDtlBun == "4002") //보철
                            {
                                nRATE = clsPmpaType.GAM.DT2_Rate;
                            }
                            else if (strDtlBun == "4003")   //임플란트
                            {
                                nRATE = clsPmpaType.GAM.DT3_Rate;
                            }
                            else
                            {
                                nRATE = clsPmpaType.GAM.DT1_Rate;
                            }
                        }
                        else    //치과 급여
                        {
                            if (strSugbK != "1")    //수가코드에 할인대상인 수가만
                            {
                                nRATE = clsPmpaType.GAM.DTGam_Rate;
                            }
                        }
                    }

                    if (nRATE == 0 || nBonin == 0)
                    {
                        nHalin = 0;
                    }
                    else
                    {
                        nHalin = nBonin * nRATE / 100;
                    }

                    ssView_Sheet1.Cells[nRow - 1, 8].Text = nBonin.ToString("#,##0");
                    ssView_Sheet1.Cells[nRow - 1, 9].Text = nRATE.ToString();
                    ssView_Sheet1.Cells[nRow - 1, 10].Text = nHalin.ToString("#,##0");

                    nTTot1 += nAmt;
                    if (strDeptCode == "**")
                    {
                        nBTot1 += nBonin;
                        nHTot1 += nHalin;
                    }
                    else
                    {
                        nBTot2 += nBonin;
                        nHTot2 += nHalin;
                    }

                    #endregion
                }
                dt.Dispose();
                dt = null;

                if (nRead > 0)
                {
                    nRow += 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = "일반과 소계";
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = nBTot1.ToString("#,##0");
                    ssView_Sheet1.Cells[nRow - 1, 10].Text = nHTot1.ToString("#,##0");
                }


                //치과 표시함
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT a.Bun, a.Nu, a.SuCode, a.SuNext,";
                SQL = SQL + ComNum.VBLF + "        c.SuNameK, b.SugbK, b.SugbF,";
                SQL = SQL + ComNum.VBLF + "        c.DtlBun, SUM(a.Amt1+a.Amt2) Amt";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a, " + ComNum.DB_PMPA + "BAS_SUT b, " + ComNum.DB_PMPA + "BAS_SUN c ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND a.TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                SQL = SQL + ComNum.VBLF + "    AND a.DeptCode = 'DT' ";
                SQL = SQL + ComNum.VBLF + "    AND a.Sucode = b.Sucode(+) ";
                SQL = SQL + ComNum.VBLF + "    AND a.SuNext = c.SuNext(+) ";
                SQL = SQL + ComNum.VBLF + "  GROUP BY a.Nu,a.SuCode,a.SuNext,c.SuNameK,b.SugbK,b.SugbF,c.DtlBun, a.Bun ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY a.Nu,a.SuCode,a.SuNext  ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                nRead = dt.Rows.Count;

                for (i = 0; i < nRead; i++)
                {
                    strDeptCode = "DT";
                    strBun = dt.Rows[i]["Bun"].ToString().Trim();
                    strNu = dt.Rows[i]["Nu"].ToString().Trim();
                    strSugbF = dt.Rows[i]["SugbF"].ToString().Trim();
                    strSugbF = dt.Rows[i]["SugbK"].ToString().Trim();
                    strDtlBun = dt.Rows[i]["DtlBun"].ToString().Trim();
                    nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());

                    #region DtlView_SUB

                    nRow += 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }

                    ssView_Sheet1.Cells[nRow - 1, 0].Text = strDeptCode;
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["SuNameK"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = strSugbK;
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = strSugbF;
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["Bun"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = strNu;
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = nAmt.ToString("#,##0");
                    ssView_Sheet1.Cells[nRow - 1, 11].Text = strDtlBun;

                    nBonin = 0;
                    nRATE = 0;
                    nHalin = 0;

                    //보험환자 CT, MRI
                    if ((clsPmpaType.TIT.Bi == "11" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "13") && (strBun == "72" || strBun == "73"))
                    {
                        if (string.Compare(strNu, "20") > 0)
                        {
                            nBonin = nAmt;
                        }
                        else
                        {
                            nBonin = nAmt * clsPmpaPb.OBON[(int)VB.Val(clsPmpaType.TIT.Bi)] / 100;
                        }
                    }
                    else
                    {
                        if (string.Compare(strNu, "20") > 0)
                        {
                            nBonin = nAmt;
                        }
                        else
                        {
                            nBonin = nAmt * clsPmpaType.TIT.BonRate / 100;
                        }
                    }

                    if (strNu == "34")  //비급여 식대
                    {
                        nRATE = clsPmpaType.GAM.FOOD_Rate;
                    }
                    else if (strNu == "35")  //병실차액
                    {
                        nRATE = clsPmpaType.GAM.ROOM_Rate;
                    }
                    else if (strNu == "36") //초음파
                    {
                        nRATE = clsPmpaType.GAM.SONO_Rate;
                    }
                    else if (strNu == "38") //MRI
                    {
                        nRATE = clsPmpaType.GAM.MRI_Rate;
                    }
                    //일반과(치과제외) 할인 대상금액을 누적
                    else if (strDeptCode != "DT")
                    {
                        if (strDtlBun == "0101")    //진찰료
                        {
                            nRATE = clsPmpaType.GAM.DTJin_Rate;
                        }
                        else if (strDtlBun == "0103")   //응급관리료
                        {
                            nRATE = clsPmpaType.GAM.ER_Rate;
                        }
                        //재단성직자 감액(본인부담전액 감액)
                        else if (clsPmpaType.GAM.GbGameK == "11")
                        {
                            nRATE = clsPmpaType.GAM.Gam_Rate;
                        }
                        else if (strSugbK != "1")
                        {
                            nRATE = clsPmpaType.GAM.Gam_Rate;
                        }
                    }
                    //치과 할인대상 금액 누적
                    else
                    {
                        if (strDtlBun == "0101")    //진찰료
                        {
                            nRATE = clsPmpaType.GAM.DTJin_Rate;
                        }
                        else if (strSugbF == "1")   //치과 비급여
                        {
                            if (strDtlBun == "4002") //보철
                            {
                                nRATE = clsPmpaType.GAM.DT2_Rate;
                            }
                            else if (strDtlBun == "4003")   //임플란트
                            {
                                nRATE = clsPmpaType.GAM.DT3_Rate;
                            }
                            else
                            {
                                nRATE = clsPmpaType.GAM.DT1_Rate;
                            }
                        }
                        else    //치과 급여
                        {
                            if (strSugbK != "1")    //수가코드에 할인대상인 수가만
                            {
                                nRATE = clsPmpaType.GAM.DTGam_Rate;
                            }
                        }
                    }

                    if (nRATE == 0 || nBonin == 0)
                    {
                        nHalin = 0;
                    }
                    else
                    {
                        nHalin = nBonin * nRATE / 100;
                    }

                    ssView_Sheet1.Cells[nRow - 1, 8].Text = nBonin.ToString("#,##0");
                    ssView_Sheet1.Cells[nRow - 1, 9].Text = nRATE.ToString();
                    ssView_Sheet1.Cells[nRow - 1, 10].Text = nHalin.ToString("#,##0");

                    nTTot1 += nAmt;
                    if (strDeptCode == "**")
                    {
                        nBTot1 += nBonin;
                        nHTot1 += nHalin;
                    }
                    else
                    {
                        nBTot2 += nBonin;
                        nHTot2 += nHalin;
                    }

                    #endregion
                }
                dt.Dispose();
                dt = null;

                if (nRead > 0)
                {
                    nRow += 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = "치과 소계";
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = nBTot2.ToString("#,##0");
                    ssView_Sheet1.Cells[nRow - 1, 10].Text = nHTot2.ToString("#,##0");
                }


                nRow += 1;
                ssView_Sheet1.RowCount = nRow;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                ssView_Sheet1.Cells[nRow - 1, 2].Text = "** 합계 **";
                ssView_Sheet1.Cells[nRow - 1, 7].Text = nTTot1.ToString("#,##0");
                ssView_Sheet1.Cells[nRow - 1, 8].Text = (nBTot1 + nBTot2).ToString("#,##0");
                ssView_Sheet1.Cells[nRow - 1, 10].Text = (nHTot1 + nHTot2).ToString("#,##0");
                
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nRead = 0;
            string strList = "";

            txtPano.Text = txtPano.Text.Trim();
            if (rdoJob2.Checked == false && txtPano.Text == "")
            {
                ComFunc.MsgBox("퇴원환자는 반드시 등록번호 또는 성명을 입력하셔야 합니다.");
                return;
            }

            if (rdoJob0.Checked == true)
            {
                if (txtPano.Text != "")
                {
                    txtPano.Text = VB.Val(txtPano.Text).ToString("00000000");
                }
            }

            try
            {
                //환자명단을 SELECT
                if (rdoJob2.Checked == true) //퇴원일자별
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT a.IPDNO,a.TRSNO,a.Pano,b.SName,b.DeptCode,b.RoomCode,b.GbSTS,a.Bi,";
                    SQL = SQL + ComNum.VBLF + " TO_CHAR(a.InDate,'YYYY-MM-DD') InDate, ";
                    SQL = SQL + ComNum.VBLF + " TO_CHAR(a.OutDate,'YYYY-MM-DD') OutDate ";
                    SQL = SQL + ComNum.VBLF + " FROM IPD_TRANS a,IPD_NEW_MASTER b ";
                    SQL = SQL + ComNum.VBLF + "WHERE a.ActDate=TO_DATE('" + dtpOutDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND a.GbIPD <> 'D' ";      //삭제는 제외
                    SQL = SQL + ComNum.VBLF + "  AND a.Amt54 <> 0 ";        //감액이 0원이 아닌것
                    SQL = SQL + ComNum.VBLF + "  AND a.IPDNO=b.IPDNO(+) ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY b.SName,a.Pano,b.InDate ";
                }
                else
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT a.IPDNO,b.TRSNO,a.Pano,a.SName,b.DeptCode,a.RoomCode,a.GbSTS,b.Bi,";
                    SQL = SQL + ComNum.VBLF + " TO_CHAR(b.InDate,'YYYY-MM-DD') InDate, ";
                    SQL = SQL + ComNum.VBLF + " TO_CHAR(b.OutDate,'YYYY-MM-DD') OutDate ";
                    SQL = SQL + ComNum.VBLF + " FROM IPD_NEW_MASTER a,IPD_TRANS b ";
                    if (rdoJob0.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE a.Pano='" + txtPano.Text + "' ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE a.SName='" + txtPano.Text + "' ";
                    }
                    SQL = SQL + ComNum.VBLF + "  AND a.IPDNO=b.IPDNO(+) ";
                    SQL = SQL + ComNum.VBLF + "  AND b.Amt54 <> 0 ";        //감액이 0원이 아닌것
                    SQL = SQL + ComNum.VBLF + "  AND b.GBIPD <> 'D' ";      //삭제는 제외
                    SQL = SQL + ComNum.VBLF + "ORDER BY a.SName,a.Pano,b.InDate ";
                }
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRead = dt.Rows.Count;

                ssList_Sheet1.RowCount = 0;

                for (i = 0; i < nRead; i++)
                {
                    ssList_Sheet1.RowCount += 1;

                    strList = dt.Rows[i]["Pano"].ToString().Trim() + " ";
                    strList += VB.Left(dt.Rows[i]["SName"].ToString().Trim() + VB.Space(11), 11);
                    strList += dt.Rows[i]["Bi"].ToString().Trim() + " ";
                    strList += dt.Rows[i]["InDate"].ToString().Trim() + " ";
                    strList += dt.Rows[i]["OutDate"].ToString().Trim() + VB.Space(10) + "{}";
                    strList += dt.Rows[i]["IPDNO"].ToString().Trim() + "{}";
                    strList += dt.Rows[i]["TRSNO"].ToString().Trim();

                    ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 0].Text = strList;
                }

                ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                dt.Dispose();
                dt = null;

                ssList.Focus();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void frmPmpaViewGamekList_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;
            ssList_Sheet1.RowCount = 0;
            ssList_Sheet1.RowCount = 1;
            ssInfo_Sheet1.Cells[0, 0, 0, ssInfo_Sheet1.ColumnCount - 1].Text = "";
            txtPano.Text = "";
            dtpOutDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
        }

        void Display_IPD_Master(double argIpdNo)
        {
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;
            ssInfo_Sheet1.Cells[0, 0, 0, ssInfo_Sheet1.ColumnCount - 1].Text = "";

            //재원마스타를 읽음
            clsIument ci = new clsIument();

            ci.Read_Ipd_Master(clsDB.DbCon, "", (long)argIpdNo);

            ssInfo_Sheet1.Cells[0, 0].Text = clsPmpaType.IMST.Pano;
            ssInfo_Sheet1.Cells[0, 1].Text = clsPmpaType.IMST.Sname;
            ssInfo_Sheet1.Cells[0, 2].Text = clsPmpaType.IMST.Age + "/" + clsPmpaType.IMST.Sex;
            ssInfo_Sheet1.Cells[0, 3].Text = clsPmpaType.IMST.WardCode;
            ssInfo_Sheet1.Cells[0, 4].Text = clsPmpaType.IMST.RoomCode.ToString();
            ssInfo_Sheet1.Cells[0, 5].Text = clsPmpaType.IMST.InDate;
            ssInfo_Sheet1.Cells[0, 6].Text = clsPmpaType.IMST.Ilsu.ToString();
            ssInfo_Sheet1.Cells[0, 7].Text = clsPmpaType.IMST.Bi;
            ssInfo_Sheet1.Cells[0, 8].Text = clsPmpaType.IMST.DeptCode;
            ssInfo_Sheet1.Cells[0, 9].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, clsPmpaType.IMST.DrCode);
            ssInfo_Sheet1.Cells[0, 10].Text = clsPmpaType.IMST.GbGameK;
            ssInfo_Sheet1.Cells[0, 11].Text = clsPmpaType.IMST.IPDNO.ToString();
            ssInfo_Sheet1.Cells[0, 12].Text = "";

        }

        private void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssList_Sheet1.RowCount == 0)
            {
                return;
            }

            ssList_Sheet1.Cells[0, 0, ssList_Sheet1.RowCount - 1, ssList_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssList_Sheet1.Cells[e.Row, 0, e.Row, ssList_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            double nTRSNO = 0;
            double nIPDNO = 0;
            string strMsg = "";
            clsIument ci = new clsIument();
            clsIpdAcct cia = new clsIpdAcct();
            clsPmpaFunc cpf = new clsPmpaFunc();

            nIPDNO = VB.Val(VB.Pstr(ssList_Sheet1.Cells[e.Row, 0].Text, "{}", 2));
            nTRSNO = VB.Val(VB.Pstr(ssList_Sheet1.Cells[e.Row, 0].Text, "{}", 3));

            Display_IPD_Master(nIPDNO);
            ci.Read_Ipd_Mst_Trans(clsDB.DbCon, ssInfo_Sheet1.Cells[0, 0].Text.Trim(), (long)nTRSNO, "");

            cpf.READ_GAMEK_RATE(clsDB.DbCon, clsPmpaType.TIT.GbGameK, clsPmpaType.TIT.Bi, clsPmpaType.TIT.OutDate, clsPmpaType.TIT.GelCode, "I");

            strMsg = "감액계정: (" + clsPmpaType.IMST.GbGameK + ") ";
            strMsg += clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_감액코드명", clsPmpaType.IMST.GbGameK) + " ";
            strMsg += "▶할인액: " + clsPmpaType.TIT.Amt[53].ToString("#,##0");
            lblGamek.Text = strMsg;

            Screen_Display();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "감 액  상 세  내 역";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("등록번호: " + clsPmpaType.TIT.Pano + " 성명: " + clsPmpaType.TIT.Sname, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String(" 종류: " + clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_환자종류", clsPmpaType.IMST.Bi), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String(" 재원기간: " + clsPmpaType.TIT.InDate + "~" + clsPmpaType.TIT.OutDate, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("할인계정: (" + clsPmpaType.TIT.GbGameK + ") " + clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_감액코드명", clsPmpaType.IMST.GbGameK), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String(" ▶할인액: " + clsPmpaType.TIT.Amt[53].ToString("#,##0"), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void rdoJob_CheckedChanged(object sender, EventArgs e)
        {
            txtPano.Text = "";
            if (rdoJob0.Checked == true)
            {
                lblPano.Text = "등록번호";
                txtPano.Text = "";
                txtPano.Visible = true;
                dtpOutDate.Visible = false;
                txtPano.Focus();
            }
            else if (rdoJob1.Checked == true)
            {
                lblPano.Text = "환자성명";
                txtPano.Text = "";
                txtPano.Visible = true;
                dtpOutDate.Visible = false;
                txtPano.Focus();
            }
            else
            {
                lblPano.Text = "퇴원일자";
                txtPano.Text = "";
                txtPano.Visible = false;
                dtpOutDate.Visible = true;
                dtpOutDate.Focus();
            }


        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.Focus();
            }
        }

        private void ssView_CellClick(object sender, CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssView_Sheet1.Cells[e.Row, 0, e.Row, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;
        }
    }
}
