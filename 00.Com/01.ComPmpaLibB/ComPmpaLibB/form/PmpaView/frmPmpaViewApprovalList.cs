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
    /// File Name       : frmPmpaViewApprovalList.cs
    /// Description     : 승인내역조회 폼
    /// Author          : 안정수
    /// Create Date     : 2017-08-23
    /// Update History  : 2017-11-02
    /// <history>       
    /// d:\psmh\OPD\ONhic\Frm승인내역조회.frm(Frm승인내역조회) => frmPmpaViewApprovalList.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\ONhic\Frm승인내역조회.frm(Frm승인내역조회)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewApprovalList : Form
    {
        string mstrHelpCode = "";
        clsSpread CS = new clsSpread();

        public frmPmpaViewApprovalList()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewApprovalList(string GstrHelpCode)
        {
            InitializeComponent();
            setEvent();
            mstrHelpCode = GstrHelpCode;
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnCancel.Click += new EventHandler(eBtnEvent);
            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnSave.Click += new EventHandler(eBtnEvent);
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

            txtRequestNo.Text = "";
            btnSave.Enabled = false;

            if (mstrHelpCode != "")
            {
                txtRequestNo.Text = mstrHelpCode;
                eGetData();
                mstrHelpCode = "";
            }

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
                eGetData();
            }

            else if (sender == this.btnSave)
            {
                //                
                if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eSaveData();
            }

            else if (sender == this.btnCancel)
            {
                txtRequestNo.Text = "";                
                CS.Spread_All_Clear(ssList);
            }
        }

        void eGetData()
        {
            int i = 0;
            int j = 0;
            int nREAD = 0;
            int nRow = 0;
            int nCnt = 0;

            string strJob = "";
            string strTemp = "";
            string strCOLNM = "";
            string strData = "";

            string str기본항목 = "";
            string strM1 = "";
            string strM3 = "";
            string strM5 = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            if(txtRequestNo.Text == "")
            {
                ComFunc.MsgBox("요청번호를 입력하세요!!");
                return;
            }

            str기본항목 = "요청번호^WRTNO{}회계일자^ActDate{}등록번호^Pano{}";
            str기본항목 += "진료과^DeptCode{}수진자명^SName{}";
            str기본항목 += "요청시각^ReqTime{}처리시각^SendTime{}";
            str기본항목 += "요청구분^ReqType{}주민등록번호^Jumin{}";
            str기본항목 += "작업요청자 사번^REQ_Sabun{}작업상태^JOB_STS{}";
            strM1 = "";
            strM3 = "";
            strM5 = "";

            ssList_Sheet1.Rows.Count = 200;

            //DB에서 자료를 읽음
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                    ";
            SQL += ComNum.VBLF + "  WRTNO,TO_CHAR(ACTDATE,'YYYY-MM-DD') ActDate,PANO,DEPTCODE,SNAME,                        ";
            SQL += ComNum.VBLF + "  TO_CHAR(REQTIME,'YYYY-MM-DD HH24:MI') ReqTime,                                          ";
            SQL += ComNum.VBLF + "  TO_CHAR(SENDTIME,'YYYY-MM-DD HH24:MI') SENDTIME,                                        ";
            SQL += ComNum.VBLF + "  REQTYPE,JUMIN,REQ_SABUN,JOB_STS,                                                        ";
            SQL += ComNum.VBLF + "  M2_JAGEK,M2_CDATE,M2_SUJIN_NAME,M2_SEDAE_NAME,M2_KIHO,                                  ";
            SQL += ComNum.VBLF + "  M2_GKIHO,M2_SANGSIL,M2_BONIN,M2_GJAN_AMT,M2_CHULGUK,M2_JANG_DATE,M2_SHOSPITAL1,         ";
            SQL += ComNum.VBLF + "  M2_SHOSPITAL2,M2_SHOSPITAL3,M2_SHOSPITAL4,M2_SHOSPITAL_NAME1,M2_SHOSPITAL_NAME2,        ";
            SQL += ComNum.VBLF + "  M2_SHOSPITAL_NAME3,M2_SHOSPITAL_NAME4,M3_JINTYPE,M3_ILSU,M3_TUYAK,M3_BONIN_AMT,         ";
            SQL += ComNum.VBLF + "  M3_GC_AMT,M3_JOHAP_AMT,M3_MSYM,M3_JIN_DATE,M3_ODRUG,M3_BONIN_GBN,M3_TA_HOSPITAL,        ";
            SQL += ComNum.VBLF + "  M3_JANG_NO,M4_APPROVE,M4_APPROVE_NO,M4_BONIN_AMT,M4_GC_AMT,M4_GJ_AMT,M5_APPROVE_NO,     ";
            SQL += ComNum.VBLF + "  M5_JIN_DATE , M6_CANCEL_CODE, M6_APPROVE_NO, M6_GJ_AMT                                  ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_NHIC                                                       ";
            SQL += ComNum.VBLF + "WHERE WRTNO = " + txtRequestNo.Text.Replace(",", "") + "                                  ";

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

                    strJob = dt.Rows[0]["ReqType"].ToString().Trim();

                    //------------------------------------
                    //  기본사항을 Dispaly
                    //------------------------------------

                    nCnt = VB.I(str기본항목, "{}") - 1;
                    nRow = 0;

                    for (i = 1; i <= nCnt; i++)
                    {
                        strTemp = VB.Pstr(str기본항목, "{}", i);
                        if (strTemp != "")
                        {
                            strCOLNM = VB.Pstr(strTemp, "^", 2);
                            nRow += 1;
                        }

                        if (nRow > ssList_Sheet1.Rows.Count)
                        {
                            ssList_Sheet1.Rows.Count = nRow;
                        }

                        ssList_Sheet1.Cells[nRow - 1, 0].Text = " " + VB.Pstr(strTemp, "^", 1);
                        ssList_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[0][strCOLNM].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 2].Text = "";
                        ssList_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[0][strCOLNM].ToString().Trim();
                    }

                    if (strJob == "M1")
                    {
                        strM1 = "자격여부^M2_Jagek{}자격취득일^M2_CDate{}수진자성명^M2_Sujin_Name{}";
                        strM1 += "세대주성명^M2_SEDAE_Name{}보장기관기호^M2_Kiho{}";
                        strM1 += "시설기호(증번호)^M2_GKiho{}급여제한일자^M2_SANGSIL{}";
                        strM1 += "본인부담여부^M2_Bonin{}건강생활유지비잔액^M2_GJAN_Amt{}";
                        strM1 += "출국자여부^M2_CHULGUK{}장애인등록일자^M2_Jang_Date{}";
                        strM1 += "선택기관기호1^M2_SHOSPITAL1{}선택기관기호2^M2_SHOSPITAL2{}";
                        strM1 += "선택기관기호3^M2_SHOSPITAL3{}선택기관기호4^M2_SHOSPITAL4{}";
                        strM1 += "선택기관이름1^M2_SHOSPITAL_Name1{}선택기관이름2^M2_SHOSPITAL_Name2{}";
                        strM1 += "선택기관이름3^M2_SHOSPITAL_Name3{}선택기관이름4^M2_SHOSPITAL_Name4{}";

                        nCnt = VB.I(strM1, "{}") - 1;

                        for (i = 0; i < nREAD; i++)
                        {
                            for (j = 1; j <= nCnt; j++)
                            {
                                strTemp = VB.Pstr(strM1, "{}", j);
                                if (strTemp != "")
                                {
                                    strCOLNM = VB.Pstr(strTemp, "^", 2);
                                    nRow += 1;
                                }

                                if (nRow > ssList_Sheet1.Rows.Count)
                                {
                                    ssList_Sheet1.Rows.Count = nRow;
                                }

                                ssList_Sheet1.Cells[nRow - 1, 0].Text = " " + VB.Pstr(strTemp, "^", 1);
                                ssList_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[0][strCOLNM].ToString().Trim();
                                if (VB.Left(strCOLNM, 3) != "M2")
                                {
                                    //SS1.CellType = SS_CELL_TYPE_STATIC_TEXT
                                    //SS1.TypeTextOrient = SS_CELL_TEXTORIENT_HORIZONTAL
                                    //SS1.TypeEllipses = False
                                    //SS1.TypeHAlign = SS_CELL_H_ALIGN_LEFT
                                    //SS1.TypeVAlign = SS_CELL_V_ALIGN_TOP
                                    //SS1.TypeTextShadow = False
                                    //SS1.TypeTextShadowIn = False
                                    //SS1.TypeTextWordWrap = False
                                    //SS1.TypeTextPrefix = False
                                    ssList_Sheet1.Cells[nRow - 1, 2].Text = "";
                                }
                                else
                                {
                                    ssList_Sheet1.Cells[nRow - 1, 2].Text = strCOLNM;
                                }

                                ssList_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i][strCOLNM].ToString().Trim();
                            }
                        }
                    }

                    else if (strJob == "M3")
                    {
                        strM3 = "진료형태^M3_JinType{}입(내)원일수^M3_ILSU{}투약일수^M3_TUYAK{}";
                        strM3 += "본인일부부담금^M3_BONIN_AMT{}건강생활유지비청구액^M3_GC_AMT{}";
                        strM3 += "기관부담금^M3_JOHAP_Amt{}주상병분류기호^M3_MSYM{}";
                        strM3 += "진료일자^M3_JIN_DATE{}처방전교부번호^M3_ODRUG{}";
                        strM3 += "본인부담여부^M3_BONIN_GBN{}타기관의뢰^M3_TA_HOSPITAL{}";
                        strM3 += "장애시 진료확인번호^M3_JANGNO{}";
                        strM3 += "승인여부^M4_Approve{}진료확인번호^M4_APPROVE_NO{}";
                        strM3 += "본인일부부담금^M4_BONIN_AMT{}";
                        strM3 += "건강생활유지비청구액^M4_GC_AMT{}";
                        strM3 += "건강생활유지비잔액^M4_GJ_AMT{}";

                        nCnt = VB.I(strM3, "{}") - 1;

                        for (i = 0; i <= nCnt; i++)
                        {
                            strTemp = VB.Pstr(strM3, "{}", i);
                            if (strTemp != "")
                            {
                                strCOLNM = VB.Pstr(strTemp, "^", 2);
                                nRow += 1;
                            }

                            if (nRow > ssList_Sheet1.Rows.Count)
                            {
                                ssList_Sheet1.Rows.Count = nRow;
                            }

                            ssList_Sheet1.Cells[nRow - 1, 0].Text = " " + VB.Pstr(strTemp, "^", 1);
                            ssList_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[0][strCOLNM].ToString().Trim();
                            if (VB.Left(strCOLNM, 3) != "M4")
                            {
                                //SS1.CellType = SS_CELL_TYPE_STATIC_TEXT
                                //SS1.TypeTextOrient = SS_CELL_TEXTORIENT_HORIZONTAL
                                //SS1.TypeEllipses = False
                                //SS1.TypeHAlign = SS_CELL_H_ALIGN_LEFT
                                //SS1.TypeVAlign = SS_CELL_V_ALIGN_TOP
                                //SS1.TypeTextShadow = False
                                //SS1.TypeTextShadowIn = False
                                //SS1.TypeTextWordWrap = False
                                //SS1.TypeTextPrefix = False
                                ssList_Sheet1.Cells[nRow - 1, 2].Text = "";
                            }
                            else
                            {
                                ssList_Sheet1.Cells[nRow - 1, 2].Text = strCOLNM;
                            }

                            ssList_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[0][strCOLNM].ToString().Trim();
                        }
                    }

                    else if (strJob == "M5")
                    {
                        strM5 = "취소 진료확인번호^M5_APPROVE_NO{}취소 진료일자^M5_JIN_DATE{}";
                        strM5 += "취소여부^M6_CANCEL_Code{}진료확인번호^M6_APPROVE_NO{}";
                        strM5 += "취소후유지비잔액^M6_GJ_AMT{}";

                        nCnt = VB.I(strM5, "{}") - 1;

                        for (i = 0; i <= nCnt; i++)
                        {
                            strTemp = VB.Pstr(strM5, "{}", i);
                            if (strTemp != "")
                            {
                                strCOLNM = VB.Pstr(strTemp, "^", 2);
                                nRow += 1;
                            }

                            if (nRow > ssList_Sheet1.Rows.Count)
                            {
                                ssList_Sheet1.Rows.Count = nRow;
                            }

                            ssList_Sheet1.Cells[nRow - 1, 0].Text = " " + VB.Pstr(strTemp, "^", 1);
                            ssList_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[0][strCOLNM].ToString().Trim();
                            if (VB.Left(strCOLNM, 3) != "M6")
                            {
                                //SS1.CellType = SS_CELL_TYPE_STATIC_TEXT
                                //SS1.TypeTextOrient = SS_CELL_TEXTORIENT_HORIZONTAL
                                //SS1.TypeEllipses = False
                                //SS1.TypeHAlign = SS_CELL_H_ALIGN_LEFT
                                //SS1.TypeVAlign = SS_CELL_V_ALIGN_TOP
                                //SS1.TypeTextShadow = False
                                //SS1.TypeTextShadowIn = False
                                //SS1.TypeTextWordWrap = False
                                //SS1.TypeTextPrefix = False
                                ssList_Sheet1.Cells[nRow - 1, 2].Text = "";
                            }
                            else
                            {
                                ssList_Sheet1.Cells[nRow - 1, 2].Text = strCOLNM;
                            }

                            ssList_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[0][strCOLNM].ToString().Trim();
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

            ssList_Sheet1.Rows.Count = nRow;
            btnView.Enabled = true;
        }

        void eSaveData()
        {
            ComFunc.MsgBox("!! 아직 저장기능은 지원하지 않습니다. !!");
        }

        void txtRequestNo_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == 13)
            {
                btnExit.Focus();
                //SendKeys.Send("{TAB}");
            }
        }
    }
}
