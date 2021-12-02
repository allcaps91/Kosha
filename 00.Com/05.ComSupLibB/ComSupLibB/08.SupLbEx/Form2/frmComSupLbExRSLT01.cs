using ComBase; //기본 클래스
using ComSupLibB.Com;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ComSupLibB.SupLbEx
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupInfc
    /// File Name       : frmComSupInfcCODE01.cs
    /// Description     : Foot Note
    /// Author          : 김홍록
    /// Create Date     : 2017-06-22
    /// Update History  : 
    /// </summary>
    /// <history>       
    ///                 2017.06.26.김홍록 : 이름을 변경하는 루틴을 필요 없음.
    /// </history>
    /// <seealso cref= "d:\psmh\exam\excode\EXCODE13.frm" />
    public partial class frmComSupLbExRSLT01 : Form
    {

        clsComSupLbExSQL lbExSql = new clsComSupLbExSQL();
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread spread = new clsSpread();

        List<string> gStrFootNote;

        public delegate void ePSMH_RETURN(object sender, List<string> strFootNote);
        /// <summary>버튼을 클릭할경우 조회된 내용이 반영</summary>
        public event ePSMH_RETURN ePSMH_FootNote;

        /// <summary>생성자</summary>
        public frmComSupLbExRSLT01(List<string> strFootNote)
        {
            InitializeComponent();

            this.gStrFootNote = strFootNote;
            setEvent();
        }

        void setEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnFootNote.Click += new EventHandler(eBtnClick);
            this.btnOk.Click += new EventHandler(eBtnClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
            }
            else
            {
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
                setCtrl();
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
            else if (sender == this.btnOk)
            {
                string[] arrSplite = new string[] { "\r\n" };
                string s = this.txtFootNote.Text;
                string[] strFootNote = s.Split(arrSplite, StringSplitOptions.None);

                List<string> lstFootNote = new List<string>();

                for (int i = 0; i < strFootNote.Length; i++)
                {
                    if (string.IsNullOrEmpty(strFootNote[i].Replace("\r\n", "").Trim()) == false)
                    {
                        lstFootNote.Add(strFootNote[i]);
                    }
                }

                ePSMH_FootNote(this, lstFootNote);
            }
            else if (sender == this.btnFootNote)
            {
                frmComSupLbExHELP01 f = new frmComSupLbExHELP01(clsComSupLbExSQL.enmEXAM_SPECODE_GUBUN.FOOT, this.btnFootNote);
                f.ePsmhReturnValue += new frmComSupLbExHELP01.PSMH_RETURN_VALUE(ePSMH_RETURN_VALUE);
                f.ShowDialog();
            }

        }

        void ePSMH_RETURN_VALUE(object sender, string code, string name, string Yname)
        {
            this.txtFootNote.Text += "\r\n" + name;
        }

        void setCtrl()
        {
            for (int i = 0; i < this.gStrFootNote.Count; i++)
            {
                this.txtFootNote.Text += this.gStrFootNote[i] + "\r\n";
            }
        }

    }
}
