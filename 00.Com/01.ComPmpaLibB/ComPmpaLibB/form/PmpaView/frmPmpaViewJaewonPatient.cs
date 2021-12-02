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
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewJaewonPatient.cs
    /// Description     : 재원 보호 환자 리스트
    /// Author          : 안정수
    /// Create Date     : 2017-09-05
    /// Update History  : 2017-11-03
    /// <history>       
    /// d:\psmh\OPD\oiguide\Frm재원보호환자.frm(Frm재원보호환자) => frmPmpaViewJaewonPatient.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oiguide\Frm재원보호환자.frm(Frm재원보호환자)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewJaewonPatient : Form
    {
        public frmPmpaViewJaewonPatient()
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

        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }

            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            optBun0.Checked = true;
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
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
        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strBi = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;


            if (optBun0.Checked == true)
            {
                strBi = "전체";
            }
            else if (optBun1.Checked == true)
            {
                strBi = "1종";
            }
            else
            {
                strBi = "2종";
            }

            //Print Head 지정
            strTitle = "보호환자 재원 리스트(" + strBi + ")";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + VB.Space(3) + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 55, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Auto, PrintType.All, 0, 0, true, false, false, true, false, true, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        void eGetData()
        {
            int i = 0;
            string strBi = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssList_Sheet1.Rows.Count = 0;

            if (optBun1.Checked == true)
            {
                strBi = "'21'";
            }
            else if (optBun2.Checked == true)
            {
                strBi = "'22'";
            }
            else
            {
                strBi = "'21','22'";
            }

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                                ";
            SQL += ComNum.VBLF + "  A.PANO, A.SNAME, A.BI, A.WARDCODE, A.ROOMCODE, B.JUMIN1||'-'||B.JUMIN2 JUMIN,                                       ";
            SQL += ComNum.VBLF + "  A.AGE, A.SEX, A.DEPTCODE, C.DRNAME, TO_CHAR(A.INDATE,'YYYY-MM-DD') INDATE                                           ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A, " + ComNum.DB_PMPA + "BAS_PATIENT B, " + ComNum.DB_PMPA + "BAS_DOCTOR C  ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                             ";
            SQL += ComNum.VBLF + "      AND A.PANO = B.PANO(+)                                                                                          ";
            SQL += ComNum.VBLF + "      AND A.DRCODE = C.DRCODE(+)                                                                                      ";
            SQL += ComNum.VBLF + "      AND A.GBSTS  = '0'                                                                                              ";
            SQL += ComNum.VBLF + "      AND A.OUTDATE IS NULL                                                                                           ";
            //2012-11-27 사생활보호요청
            SQL += ComNum.VBLF + "      AND (A.SECRET IS NULL OR A.SECRET ='')                                                                          ";
            if (strBi != "")
            {
                SQL += ComNum.VBLF + "  AND A.BI IN (" + strBi + ")                                                                                     ";
            }
            SQL += ComNum.VBLF + "ORDER BY WARDCODE, ROOMCODE                                                                                           ";

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
                    ssList_Sheet1.Rows.Count = dt.Rows.Count;
                    ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["BI"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["JUMIN"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["AGE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["SEX"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 9].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 10].Text = dt.Rows[i]["INDATE"].ToString().Trim();
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

    }
}
