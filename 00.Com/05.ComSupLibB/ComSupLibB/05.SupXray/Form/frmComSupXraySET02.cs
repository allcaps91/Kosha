using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray
    /// File Name       : frmComSupXraySET02.cs
    /// Description     : 영상의학과 판독프로그램에 사용되는 세팅 폼
    /// Author          : 윤조연
    /// Create Date     : 2017-07-03
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\xray\xuread\xupacs\FrmLinkSet.frm(FrmLinkSet) >> frmComSupXraySET02.cs 폼이름 재정의" />
    public partial class frmComSupXraySET02 : Form
    {
        #region 클래스 선언 및 etc....

        string path = @"d:\interface\ini\xulink_setup.txt";

        clsComSupXrayRead.XrayReadSet XRS = new clsComSupXrayRead.XrayReadSet();

        #endregion

        public frmComSupXraySET02()
        {
            InitializeComponent();

            setEvent();
        }

        //기본값 세팅
        void setCtrlData()
        {
            //txtPano.Text = gstrPano;
            //lblSName.Text = gstrSName;

            Read_PacsLink_set();
            Read_set_disp();



        }

        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);


            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnSave.Click += new EventHandler(eBtnEvent);



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

                screen_clear();

                setCtrlData();
            }

        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }

            else if (sender == this.btnSave)
            {
                //저장
                eSave();
            }


        }

        void eSave()
        {
            string strTemp = "";

            if (chkYN.Checked == true)
            {
                strTemp += "Y";
                XRS.Link_YN =true;
            }
            else
            {
                strTemp += "N";
                XRS.Link_YN = false;
            }

            if (chkSet1.Checked == true)
            {
                strTemp += "Y";
                XRS.Link_Set1 = true;
            }
            else
            {
                strTemp += "N";
                XRS.Link_Set1 = false;
            }

            if (chkSet2.Checked == true)
            {
                strTemp += "Y";
                XRS.Link_Set2 = true;
            }
            else
            {
                strTemp += "N";
                XRS.Link_Set2 = false;
            }
            
            if (chkSet3.Checked == true)
            {
                strTemp += "Y";
                XRS.Link_Set3 = true;
            }
            else
            {
                strTemp += "N";
                XRS.Link_Set3 = false;
            }

            clsVbfunc.WriteFile(path, strTemp);
        }

        void screen_clear()
        {
            //



        }

        void Read_PacsLink_set()
        {
            XRS = new clsComSupXrayRead.XrayReadSet();

            if (!System.IO.File.Exists(path)) return;

            string textLine = System.IO.File.ReadAllText(path, Encoding.Default);

            //설정파일을 읽음
            if (textLine.Length > 0)
            {
                XRS.Link_YN = VB.Mid(textLine, 1, 1) == "Y" ? true : false;
                XRS.Link_Set1 = VB.Mid(textLine, 2, 1) == "Y" ? true : false;
                XRS.Link_Set2 = VB.Mid(textLine, 3, 1) == "Y" ? true : false;
                XRS.Link_Set3 = VB.Mid(textLine, 4, 1) == "Y" ? true : false;

            }
        }

        void Read_set_disp()
        {

            if (XRS.Link_YN == true) chkYN.Checked = true;

            if (XRS.Link_Set1 == true) chkSet1.Checked = true;
            if (XRS.Link_Set2 == true) chkSet2.Checked = true;
            if (XRS.Link_Set3 == true) chkSet3.Checked = true;

        }

    }
}
