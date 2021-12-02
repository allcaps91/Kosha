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
    /// File Name       : frmPmpaViewBloodABO.cs
    /// Description     : 특수혈액형(Rh-)명단조회
    /// Author          : 안정수
    /// Create Date     : 2017-07-06
    /// Update History  : 2017-11-30
    /// <history>         
    /// d:\psmh\Etc\csinfo\csinfo54.frm(FrmBloodAboView) => frmPmpaViewBloodABO.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\Etc\csinfo\csinfo54.frm(FrmBloodAboView)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewBloodABO : Form, MainFormMessage
    {
        ComFunc CF = new ComFunc();

        #region MainFormMessage InterFace

        public MainFormMessage mCallForm = null;

        public void MsgActivedForm(Form frm)
        {

        }

        public void MsgUnloadForm(Form frm)
        {

        }

        public void MsgFormClear()
        {

        }

        public void MsgSendPara(string strPara)
        {

        }

        #endregion

        public frmPmpaViewBloodABO(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmPmpaViewBloodABO()
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

            this.optJob0.CheckedChanged += new EventHandler(eBtnEvent);
            this.optJob1.CheckedChanged += new EventHandler(eBtnEvent);
            this.optJob2.CheckedChanged += new EventHandler(eBtnEvent);

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

            optJob0.Checked = true;

            Set_Combo();

            /// 본소스에서도 구현이 안되어있으므로 false 처리
            btnPrint.Visible = false;
        }

        void Set_Combo()
        {
            dtpFdate.Text = Convert.ToDateTime(dtpTdate.Text).AddDays(-180).ToShortDateString();
            dtpTdate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            cboAbo.Items.Clear();
            cboAbo.Items.Add("전체");
            cboAbo.Items.Add("A-");
            cboAbo.Items.Add("B-");
            cboAbo.Items.Add("O-");
            cboAbo.Items.Add("AB-");

            cboAbo.SelectedIndex = 0;
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

            else if (sender == this.btnPrint)
            {
                //                
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                ePrint();
            }

            else if (sender == this.optJob0)
            {
                ssList_Sheet1.ColumnHeader.Cells[0, 1].Text = "등록번호";
            }

            else if (sender == this.optJob1)
            {
                ssList_Sheet1.ColumnHeader.Cells[0, 1].Text = "종검번호";
            }

            else if (sender == this.optJob2)
            {
                ssList_Sheet1.ColumnHeader.Cells[0, 1].Text = "검진번호";
            }
        }

        void ePrint()
        {
            /// VB 본소스에서도 프린트 기능 출력 안 되어있으므로 Visible = false 처리
        }

        void eGetData()
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strAbo = cboAbo.SelectedItem.ToString();
            ssList_Sheet1.Rows.Count = 0;

            // 특수혈액형을 SELECT
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                        ";
            SQL += ComNum.VBLF + "  a.Pano,a.SName,a.Age,a.Sex,a.Ward,a.Room,a.Abo,                                             ";
            SQL += ComNum.VBLF + "  TO_CHAR(a.ModifyDate,'YYYY-MM-DD HH24:MI') MDate,                                           ";
            SQL += ComNum.VBLF + "  a.Sabun,a.Bi";

            //환자마스타
            if (optJob0.Checked == true)
            {
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "EXAM_BLOOD_MASTER a, " + ComNum.DB_PMPA + "BAS_PATIENT b        ";
                SQL += ComNum.VBLF + "  WHERE 1=1                                                                               ";
                SQL += ComNum.VBLF + "      AND a.ModifyDate>=TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD')                     ";
                SQL += ComNum.VBLF + "      AND a.ModifyDate<=TO_DATE('" + dtpTdate.Text + "23:59','YYYY-MM-DD HH24:MI')        ";

                if (strAbo == "전체")
                {
                    SQL += ComNum.VBLF + "      AND a.Abo IN ('A-','B-','AB-','O-')                                             ";
                }
                else
                {
                    SQL += ComNum.VBLF + "      AND a.Abo = '" + strAbo + "'                                                    ";
                }
                SQL += ComNum.VBLF + "      AND a.Pano=b.Pano(+)                                                                ";
                SQL += ComNum.VBLF + "      AND a.SName=b.SName                                                                 ";
            }

            //종합검진
            else if (optJob1.Checked == true)
            {
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "EXAM_BLOOD_MASTER a, " + ComNum.DB_PMPA + "MED_ID b             ";
                SQL += ComNum.VBLF + "  WHERE 1=1                                                                               ";
                SQL += ComNum.VBLF + "      AND a.ModifyDate>=TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD')                     ";
                SQL += ComNum.VBLF + "      AND a.ModifyDate<=TO_DATE('" + dtpTdate.Text + "23:59','YYYY-MM-DD HH24:MI')        ";

                if (strAbo == "전체")
                {
                    SQL += ComNum.VBLF + "      AND a.Abo IN ('A-','B-','AB-','O-')                                             ";
                }
                else
                {
                    SQL += ComNum.VBLF + "      AND a.Abo = '" + strAbo + "'                                                    ";
                }
                SQL += ComNum.VBLF + "      AND TO_NUMBER(a.Pano)=b.WRTNO(+)                                                    ";
                SQL += ComNum.VBLF + "      AND a.SName=RTrim(b.Name)                                                           ";
            }

            //일반검진
            else if (optJob2.Checked == true)
            {
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "EXAM_BLOOD_MASTER a, " + ComNum.DB_PMPA + "HIC_PATIENT b        ";
                SQL += ComNum.VBLF + "  WHERE 1=1                                                                               ";
                SQL += ComNum.VBLF + "      AND a.ModifyDate>=TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD')                     ";
                SQL += ComNum.VBLF + "      AND a.ModifyDate<=TO_DATE('" + dtpTdate.Text + "23:59','YYYY-MM-DD HH24:MI')        ";

                if (strAbo == "전체")
                {
                    SQL += ComNum.VBLF + "      AND a.Abo IN ('A-','B-','AB-','O-')                                             ";
                }
                else
                {
                    SQL += ComNum.VBLF + "      AND a.Abo = '" + strAbo + "'                                                    ";
                }
                SQL += ComNum.VBLF + "      AND TO_NUMBER(a.Pano)=b.Pano(+)                                                     ";
                SQL += ComNum.VBLF + "      AND a.SName=RTRIM(b.SName)                                                          ";
            }

            SQL += ComNum.VBLF + "ORDER BY a.ModifyDate DESC,a.Pano                                                             ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
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
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["MDate"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Age"].ToString().Trim() + "/" + dt.Rows[i]["Sex"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Ward"].ToString().Trim();
                        if (dt.Rows[i]["Room"].ToString().Trim() == "0")
                        {
                            ssList_Sheet1.Cells[i, 5].Text = "";
                        }
                        else
                        {
                            ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Room"].ToString().Trim();
                        }
                        ssList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Abo"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["Bi"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 8].Text = CF.READ_PassName(clsDB.DbCon, dt.Rows[i]["Sabun"].ToString().Trim());
                    }

                }

                btnPrint.Enabled = true;
            }

            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt.Dispose();
            dt = null;
        }

    }
}
