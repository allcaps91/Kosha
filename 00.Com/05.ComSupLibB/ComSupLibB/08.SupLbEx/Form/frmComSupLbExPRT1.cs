using ComBase; //기본 클래스
using FarPoint.Win.Spread;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using ComDbB;
using System;

namespace ComSupLibB.SupLbEx
{

    /// <summary>
    /// Class Name : ComSupLibB.SupLbEx
    /// File Name : frmComSupLbExPrt01.cs
    /// Title or Description : 진단검사의학과 출력용
    /// Author : 김홍록
    /// Create Date : 2017-06-07
    /// Update History : 
    /// </summary>
    public partial class frmComSupLbExPRT1 : Form
    {
        string strRowId;

        clsComSupLbEx LbEx = new clsComSupLbEx();
        clsComSupLbExSQL lbExSQL = new clsComSupLbExSQL();
        clsSpread spread = new clsSpread();
        ComFunc CF = new ComFunc();
        public enum enmType { VRFC, PB, BLOOD, COVID };

        public frmComSupLbExPRT1()
        {
            InitializeComponent();
        }

        /// <summary>종합검증 생성자</summary>
        public frmComSupLbExPRT1(enmType prtType, string pRowId = "", string pano = "", string bDate = "", string specNo = "", string gbIO = "", string gbDeptCode = "", string gbWard = "", bool prePrint = false)
        {
            InitializeComponent();
            this.strRowId = pRowId;

            if (prtType == enmType.VRFC)
            {
                if (setVRFC(pRowId))
                {
                    printSS(this.ss_Vrfc, prePrint);
                }
            }
            else if (prtType == enmType.PB)
            {
                if (gbIO == "")
                {
                    if (setPB(pano, bDate))
                    {
                        printSS(this.ss_PB, prePrint);
                    }
                }
                else
                {
                    if (gbWard == "")
                    {
                        if (setPB(pano, bDate, gbIO, gbDeptCode))
                        {
                            printSS(this.ss_PB, prePrint);
                        }
                    }
                    else
                    {
                        if (setPB(pano, bDate, gbIO, gbDeptCode, gbWard))
                        {
                            printSS(this.ss_PB, prePrint);
                        }
                    }
                }
            }
            else if (prtType == enmType.BLOOD)
            {
                if (setBLOOD(specNo))
                {
                    printSS(this.ss_Blood, prePrint);
                }
            }
            else if (prtType == enmType.COVID)
            {
                if (setCOVID(pano,bDate))
                {
                    printSS(this.ss_Covid, prePrint);
                }
            }
        }

        bool setVRFC(string pRowId)
        {

            bool b = true;
            DataTable dt = lbExSQL.sel_EXAM_VERIFY_Print(clsDB.DbCon, pRowId);

            if (dt != null && dt.Rows.Count > 0)
            {
                #region MVC -View

                this.ss_Vrfc.Sheets[0].Cells[2, 3].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.PANO.ToString()].ToString().Trim();
                this.ss_Vrfc.Sheets[0].Cells[2, 5].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.WARD.ToString()].ToString().Trim();

                this.ss_Vrfc.Sheets[0].Cells[3, 3].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.SNAME.ToString()].ToString().Trim();
                this.ss_Vrfc.Sheets[0].Cells[3, 5].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.DEPT_NAME.ToString()].ToString().Trim();

                this.ss_Vrfc.Sheets[0].Cells[4, 3].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.AGE_SEX.ToString()].ToString().Trim();
                this.ss_Vrfc.Sheets[0].Cells[4, 5].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.DR_NAME.ToString()].ToString().Trim();

                this.ss_Vrfc.Sheets[0].Cells[5, 3].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.DISEASE.ToString()].ToString().Trim();

                this.ss_Vrfc.Sheets[0].Cells[8, 3].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.DISDATE.ToString()].ToString().Trim();

                this.ss_Vrfc.Sheets[0].Cells[9, 2].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.ITEMS1.ToString()].ToString().Trim();

                this.ss_Vrfc.Sheets[0].Cells[19, 2].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.ITEMS2.ToString()].ToString().Trim();

                this.ss_Vrfc.Sheets[0].Cells[30, 2].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.VERIFY1.ToString()].ToString().Trim();
                this.ss_Vrfc.Sheets[0].Cells[30, 4].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.VERIFY4.ToString()].ToString().Trim();

                this.ss_Vrfc.Sheets[0].Cells[31, 2].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.VERIFY2.ToString()].ToString().Trim();
                this.ss_Vrfc.Sheets[0].Cells[31, 4].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.VERIFY5.ToString()].ToString().Trim();

                this.ss_Vrfc.Sheets[0].Cells[32, 2].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.VERIFY3.ToString()].ToString().Trim();
                this.ss_Vrfc.Sheets[0].Cells[32, 4].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.VERIFY6.ToString()].ToString().Trim();

                this.ss_Vrfc.Sheets[0].Cells[36, 2].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.COMMENTS.ToString()].ToString().Trim();

                this.ss_Vrfc.Sheets[0].Cells[42, 2].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.RECOMMENDATION.ToString()].ToString().Trim();

                this.ss_Vrfc.Sheets[0].Cells[49, 5].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.JDATE.ToString()].ToString().Trim();

                this.ss_Vrfc.Sheets[0].Cells[50, 5].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.RDR_NAME.ToString()].ToString().Trim();

                this.ss_Vrfc.Sheets[0].Cells[51, 5].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.RDR_BUNHO.ToString()].ToString().Trim();

                this.ss_Vrfc.Sheets[0].Cells[53, 3].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.JYYYY.ToString()].ToString().Trim();

                #endregion  
            }
            else
            {
                ComFunc.MsgBox("조회 조건에 해당하는 데이터가 존재 하지 않습니다.");
                b = false;
            }

            return b;
        }

        bool setPB(string pano, string bDate, string gbIO = "", string gbDept = "", string gbWard = "")
        {
            bool b = true;

            DataTable dt = lbExSQL.sel_EXAM_ORDER_PB(clsDB.DbCon, pano, bDate, gbIO, gbDept, gbWard);

            if (dt != null && dt.Rows.Count > 0)
            {
                ss_PB.ActiveSheet.Cells[1, 1].Value = dt.Rows[0][(int)clsComSupLbExSQL.enmSelExamOrderPb.PANO].ToString();
                ss_PB.ActiveSheet.Cells[1, 3].Value = dt.Rows[0][(int)clsComSupLbExSQL.enmSelExamOrderPb.WARDCODE].ToString();

                ss_PB.ActiveSheet.Cells[2, 1].Value = dt.Rows[0][(int)clsComSupLbExSQL.enmSelExamOrderPb.SNAME].ToString();
                ss_PB.ActiveSheet.Cells[2, 3].Value = dt.Rows[0][(int)clsComSupLbExSQL.enmSelExamOrderPb.ROOMCODE].ToString();

                ss_PB.ActiveSheet.Cells[3, 1].Value = dt.Rows[0][(int)clsComSupLbExSQL.enmSelExamOrderPb.JUMIN].ToString();
                ss_PB.ActiveSheet.Cells[3, 3].Value = dt.Rows[0][(int)clsComSupLbExSQL.enmSelExamOrderPb.DEPTCODE].ToString();

                ss_PB.ActiveSheet.Cells[4, 1].Value = dt.Rows[0][(int)clsComSupLbExSQL.enmSelExamOrderPb.BDATE].ToString();
                ss_PB.ActiveSheet.Cells[4, 3].Value = dt.Rows[0][(int)clsComSupLbExSQL.enmSelExamOrderPb.DRNAME].ToString();

                ss_PB.ActiveSheet.Cells[9, 0].Value = dt.Rows[0][(int)clsComSupLbExSQL.enmSelExamOrderPb.REQUEST1].ToString();
                ss_PB.ActiveSheet.Cells[11, 0].Value = dt.Rows[0][(int)clsComSupLbExSQL.enmSelExamOrderPb.REQUEST2].ToString();
            
            }
            else
            {
                //ComFunc.MsgBox("조회 조건에 해당하는 데이터가 존재 하지 않습니다.");
                b = false;
            }
            return b;
        }


        bool setCOVID(string pano, string bDate)
        {
            bool b = true;

            DataTable dt = lbExSQL.sel_EXAM_ORDER_COVID(clsDB.DbCon, pano, bDate);

            if (dt != null && dt.Rows.Count > 0)
            {

                ss_Covid.ActiveSheet.Cells[6, 2].Text = clsOrdFunction.Pat.sName;
                ss_Covid.ActiveSheet.Cells[6, 9].Text = clsOrdFunction.Pat.Birth;
                ss_Covid.ActiveSheet.Cells[6, 12].Text = clsOrdFunction.Pat.Sex;

                //2020-12-22 추가 
                ss_Covid.ActiveSheet.Cells[7, 2].Text = clsOrdFunction.Pat.PtNo;

                ss_Covid.ActiveSheet.Cells[8, 2].Text = dt.Rows[0]["BALDATE"].ToString().Trim();
                ss_Covid.ActiveSheet.Cells[8, 9].Text = dt.Rows[0]["BLOODDATE"].ToString().Trim();

                if (dt.Rows[0]["GBJONG"].ToString().Trim() == "1")
                {
                    ss_Covid.ActiveSheet.Cells[9, 3].Text = "True";
                }
                else if (dt.Rows[0]["GBJONG"].ToString().Trim() == "2")
                {
                    ss_Covid.ActiveSheet.Cells[10, 3].Text = "True";
                }
                else
                {
                    ss_Covid.ActiveSheet.Cells[11, 3].Text = "True";
                }

                if (dt.Rows[0]["GBBLOOD"].ToString().Trim() == "1")
                {
                    ss_Covid.ActiveSheet.Cells[13, 3].Text = "True";
                }
                else
                {
                    ss_Covid.ActiveSheet.Cells[13, 7].Text = "True";
                }

                ss_Covid.ActiveSheet.Cells[16, 0].Text = dt.Rows[0]["DRCOMMENT"].ToString().Trim();

                ss_Covid.ActiveSheet.Cells[18, 0].Text = VB.Left(dt.Rows[0]["WRITEDATE"].ToString().Trim(), 4) + " 년" + VB.Space(8) + VB.Mid(dt.Rows[0]["WRITEDATE"].ToString().Trim(), 6, 2) + " 월" + VB.Space(8) + VB.Mid(dt.Rows[0]["WRITEDATE"].ToString().Trim(), 9, 2) + " 일";


                ss_Covid.ActiveSheet.Cells[17, 8].Text = CF.Read_SabunName(clsDB.DbCon, dt.Rows[0]["WRITESABUN"].ToString().Trim());
                SetDrSign(ss_Covid, 17, 11, ComFunc.SetAutoZero(dt.Rows[0]["WRITESABUN"].ToString().Trim(), 5));
            }
            else
            {
                //ComFunc.MsgBox("조회 조건에 해당하는 데이터가 존재 하지 않습니다.");
                b = false;
            }
            return b;
        }
        bool setBLOOD(string specNo)
        {
            bool b = true;

            DataTable dt = lbExSQL.sel_EXAM_RESULTC_Blood(clsDB.DbCon, specNo);

            if (dt != null && dt.Rows.Count > 0)
            {

                this.ss_Blood.Sheets[0].Cells[1, 1].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamResultcBlood.PANO.ToString()].ToString().Trim();
                this.ss_Blood.Sheets[0].Cells[1, 3].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamResultcBlood.SNAME.ToString()].ToString().Trim();
                this.ss_Blood.Sheets[0].Cells[1, 6].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamResultcBlood.AGE_SEX.ToString()].ToString().Trim();

                this.ss_Blood.Sheets[0].Cells[2, 1].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamResultcBlood.ROOM.ToString()].ToString().Trim();
                this.ss_Blood.Sheets[0].Cells[2, 3].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamResultcBlood.DEPT_NAME.ToString()].ToString().Trim();
                this.ss_Blood.Sheets[0].Cells[2, 6].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamResultcBlood.DR_NAME.ToString()].ToString().Trim();

                this.ss_Blood.Sheets[0].Cells[3, 1].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamResultcBlood.DIAGNOSIS.ToString()].ToString().Trim();

                this.ss_Blood.Sheets[0].Cells[4, 1].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamResultcBlood.BLOOD_TYPE.ToString()].ToString().Trim();
                this.ss_Blood.Sheets[0].Cells[4, 5].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamResultcBlood.BLOOD_HIS.ToString()].ToString().Trim();

                //TODO : 2017.05.29.김홍록 : 다른 혈액이 1개 이상 난 경우를 반드시 확인 요망. 

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    this.ss_Blood.Sheets[0].Cells[8 + i, 0].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamResultcBlood.BLOOD_NAME.ToString()].ToString().Trim();
                    this.ss_Blood.Sheets[0].Cells[8 + i, 4].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamResultcBlood.CNT.ToString()].ToString().Trim();
                }

                this.ss_Blood.Sheets[0].Cells[20, 1].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamResultcBlood.BDATE.ToString()].ToString().Trim();
                this.ss_Blood.Sheets[0].Cells[20, 6].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamResultcBlood.ACT_DATE.ToString()].ToString().Trim();
            }
            else
            {
                //ComFunc.MsgBox("조회 조건에 해당하는 데이터가 존재 하지 않습니다.");
                b = false;
            }

            return b;
        }

        void printSS(FpSpread o, bool prePrint)
        {

            string header = string.Empty;
            string foot = string.Empty;

            clsSpread.SpdPrint_Margin margin = new clsSpread.SpdPrint_Margin(30, 10, 10, 10, 10, 10);
            clsSpread.SpdPrint_Option option = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait
                                            , PrintType.All, 0, 0, false, false, false, false, false, false, false);

            try
            {
                spread.setSpdPrint(o, prePrint, margin, option, header, foot, Centering.Horizontal);
            }
            catch (System.Exception EX)
            {
                ComFunc.MsgBox(EX.ToString());                
            }            
        }

        private void SetDrSign(FarPoint.Win.Spread.FpSpread spd, int row, int Col, string sabun)
        {
            Image ImageX = GetDrSign(clsDB.DbCon, sabun, "");
            FarPoint.Win.Spread.CellType.TextCellType cellType = new FarPoint.Win.Spread.CellType.TextCellType();
            cellType.BackgroundImage = new FarPoint.Win.Picture(ImageX, FarPoint.Win.RenderStyle.Stretch);
            spd.ActiveSheet.Cells[row, Col].CellType = cellType;

            ImageX = null;
            cellType = null;
        }

        private Image GetDrSign(PsmhDb pDbCon, string strSabun, string strgubun)
        {
            Image rtnVAL = null;

            if (string.IsNullOrEmpty(strSabun)) return rtnVAL;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "SIGNATURE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR ";
                if (strgubun == "1")
                {
                    SQL = SQL + ComNum.VBLF + "WHERE TRIM(drcode) = '" + strSabun + "'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "WHERE TRIM(SABUN) = '" + strSabun + "'";
                }


                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return rtnVAL;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return rtnVAL;
                }

                if (dt.Rows[0]["SIGNATURE"] == DBNull.Value)
                {
                    ComFunc.MsgBox("현재 의사는 서명이 없습니다 확인해주세요.");
                    return rtnVAL;
                }

                using (MemoryStream memStream = new MemoryStream((byte[])dt.Rows[0]["SIGNATURE"]))
                {
                    rtnVAL = Image.FromStream(memStream);
                }

                return rtnVAL;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVAL;
            }
        }
    }
}
