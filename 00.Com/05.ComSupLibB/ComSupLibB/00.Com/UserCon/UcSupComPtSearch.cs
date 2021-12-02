using ComLibB;
using ComSupLibB.Com;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using ComDbB; //DB연결
using ComBase; //기본 클래스
using Oracle.DataAccess.Client;  //이 참조는 필요없음

namespace ComSupLibB
{

    /// <summary>
    /// Class Name : ComSupLibB
    /// File Name : SupComPtInfo.cs
    /// Title or Description : 진료지원환자 찾기
    /// Author : 김홍록
    /// Create Date : 2017-06-09
    /// Update History : 
    /// </summary>
    public partial class UcSupComPtSearch : UserControl
    {

        /// <summary>이벤트</summary>
        /// <param name="sender">받은폼</param>
        /// <param name="pano">환자번호</param>
        /// <param name="sname">환자명</param>
        public delegate void PSMH_RETURN_VALUE(object sender, string pano, string sname); 
        /// <summary>이벤트헨들러</summary>
        public event PSMH_RETURN_VALUE ePSMH_UcSupComPtSearch_VALUE;

        clsMethod psmhMethod = new clsMethod();
        clsComSQL comSql = new clsComSQL();

        /// <summary>정보설정</summary>
        public enum enmType {
            /// <summary>환자정보검색</summary>
            PTINFO
            /// <summary>인사정보검색</summary>
            , ERPINFO
            /// <summary>BLOOD마스터</summary>
            , BLOODINFO
            /// <summary>검체정보</summary>
            , SPECINFO
        };

        Point pPSMH_Point;
        enmType ePSMH_TYPE;
        bool isPSMHTitleVisible = true;

        bool isUNITKEY = false;

        /// <summary>환자정보, 인사정보구분</summary>
        public enmType PSMH_TYPE
        {
            get { return ePSMH_TYPE; }
            set { ePSMH_TYPE = value; }
        }

        public bool PSMH_TITLE_VISIBLE
        {
            get { return isPSMHTitleVisible; }
            set { isPSMHTitleVisible = value; }

        }

        public bool PSMH_UNITKEY
        {
            get { return isUNITKEY; }
            set { isUNITKEY = value; }

        }

        /// <summary>정보검색 위치값</summary>
        public Point pPSMH_LPoint
        {
            get { return pPSMH_Point; }
            set { pPSMH_Point = value; }
        }

        /// <summary>기본생성자</summary>
        public UcSupComPtSearch()
        {
            InitializeComponent();
            setEvent();
        }        

        void setEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.txtSearch_PtInfo.KeyPress += new KeyPressEventHandler(txtPtNo_KeyPress);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            this.txtSearch_PtInfo.Text = "";
            this.txtSearch_SName.Text = "";

            if (isPSMHTitleVisible == true)
            {
                if (this.ePSMH_TYPE == enmType.PTINFO)
                {
                    this.lblPtNo.Text = "환자정보";
                }
                else if (this.ePSMH_TYPE == enmType.ERPINFO)
                {
                    this.lblPtNo.Text = "직원정보";
                }
                else if (this.ePSMH_TYPE == enmType.BLOODINFO)
                {
                    this.lblPtNo.Text = "수혈환자";
                }
                else if (this.ePSMH_TYPE == enmType.SPECINFO)
                {
                    this.lblPtNo.Text = "검체정보";
                }
            }
            else
            {
                this.lblPtNo.Text = "";
            }

        }

        void F_PSMH_PTInfoEvent(object sender, string pano, string sname)
        {
            this.txtSearch_PtInfo.Text = pano;
            this.txtSearch_SName.Text = sname;
            ePSMH_UcSupComPtSearch_VALUE += SupComPtInfo_ePSMH_RETURN_VALUE;
            this.ePSMH_UcSupComPtSearch_VALUE(this, pano, sname);
                        
        }

        void SupComPtInfo_ePSMH_RETURN_VALUE(object sender, string pano, string sname)
        {
        
        }

        void txtPtNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (string.IsNullOrEmpty(this.txtSearch_PtInfo.Text.Trim()) == false)
                {
                    setReturnValue();
                }
                else
                {
                    if (sender == this.txtSearch_PtInfo)
                    {
                        this.txtSearch_SName.Text = string.Empty;
                    }
                }
            }
        }

        void setReturnValue()
        {
            if (this.txtSearch_PtInfo.Text.Trim() != "")
            {
                if (this.txtSearch_PtInfo.Text.Length < 2)
                {
                    ComFunc.MsgBox("반드시 2자 이상을 입력하세요");
                    return;
                }

                DataTable dt = null;

                if (ePSMH_TYPE == enmType.PTINFO )
                {
                    dt = comSql.sel_BAS_PATIENT(clsDB.DbCon, this.txtSearch_PtInfo.Text.Trim(), clsComSQL.enmSearchType.PTINFO, this.isUNITKEY);
                }
                else if (ePSMH_TYPE == enmType.ERPINFO)
                {
                    dt = comSql.sel_BAS_PATIENT(clsDB.DbCon, this.txtSearch_PtInfo.Text.Trim(), clsComSQL.enmSearchType.ERPINFO, this.isUNITKEY);
                }
                else if (ePSMH_TYPE == enmType.BLOODINFO)
                {
                    dt = comSql.sel_BAS_PATIENT(clsDB.DbCon, this.txtSearch_PtInfo.Text.Trim(), clsComSQL.enmSearchType.BLOODINFO, this.isUNITKEY);
                }
                else if (ePSMH_TYPE == enmType.SPECINFO)
                {
                    dt = comSql.sel_BAS_PATIENT(clsDB.DbCon, this.txtSearch_PtInfo.Text.Trim(), clsComSQL.enmSearchType.SPECINFO, this.isUNITKEY);
                }

                if (ComFunc.isDataTableNull(dt)== false)
                {
                    if (dt.Rows.Count == 1)
                    {
                        this.txtSearch_PtInfo.Text = dt.Rows[0][clsComSQL.enmSelBasPatient.PANO.ToString()].ToString();
                        this.txtSearch_SName.Text = dt.Rows[0][clsComSQL.enmSelBasPatient.SNAME.ToString()].ToString();

                        F_PSMH_PTInfoEvent(this, dt.Rows[0][clsComSQL.enmSelBasPatient.PANO.ToString()].ToString(), dt.Rows[0][clsComSQL.enmSelBasPatient.SNAME.ToString()].ToString());
                    }
                    else
                    {
                        if (this.isUNITKEY == false)
                        {
                            if (PSMH_TYPE == enmType.PTINFO)
                            {
                                string[] sSsel_BAS_PATIENT_PTINFO = { "등록번호", "환자명", "생년월일", "나이", "성별", "주소", "최종내원일", "진료과" };
                                int[] nSsel_BAS_PATIENT_PTINFO = { clsParam.nCol_PANO, clsParam.nCol_SNAME, clsParam.nCol_TIME, clsParam.nCol_AGE, clsParam.nCol_SCHK, clsParam.nCol_JUSO, clsParam.nCol_DATE, clsParam.nCol_DPCD };

                                setPopUp(dt, sSsel_BAS_PATIENT_PTINFO, nSsel_BAS_PATIENT_PTINFO);

                            }
                            else if (PSMH_TYPE == enmType.ERPINFO)
                            {

                                string[] sSsel_BAS_PATIENT_PTINFO = { "사번", "성명", "소속", "전화번호" };
                                int[] nSsel_BAS_PATIENT_PTINFO = { clsParam.nCol_PANO, clsParam.nCol_SNAME, clsParam.nCol_TIME, clsParam.nCol_TEL };

                                setPopUp(dt, sSsel_BAS_PATIENT_PTINFO, nSsel_BAS_PATIENT_PTINFO);

                            }
                            else if (PSMH_TYPE == enmType.BLOODINFO)
                            {
                                string[] sSsel_BAS_PATIENT_PTINFO = { "등록번호", "성명", "성별", "나이", "병동", "병실", "WS" };
                                int[] nSsel_BAS_PATIENT_PTINFO = { clsParam.nCol_PANO, clsParam.nCol_SNAME, clsParam.nCol_SCHK, clsParam.nCol_AGE, clsParam.nCol_AGE, clsParam.nCol_AGE, clsParam.nCol_IOPD };

                                setPopUp(dt, sSsel_BAS_PATIENT_PTINFO, nSsel_BAS_PATIENT_PTINFO);

                            }
                            else if (PSMH_TYPE == enmType.SPECINFO)
                            {
                                string[] sSsel_BAS_PATIENT_PTINFO = { "등록번호", "성명", "검체번호", "나이", "병동", "병실", "WS" };
                                int[] nSsel_BAS_PATIENT_PTINFO = { clsParam.nCol_PANO, clsParam.nCol_SNAME, clsParam.nCol_SCHK, clsParam.nCol_AGE, clsParam.nCol_AGE, clsParam.nCol_AGE, clsParam.nCol_IOPD };

                                setPopUp(dt, sSsel_BAS_PATIENT_PTINFO, nSsel_BAS_PATIENT_PTINFO);
                            }
                        }
                        else
                        {
                            ComFunc.MsgBox("값이 존재 하지 않습니다.");

                            this.txtSearch_PtInfo.Text = string.Empty;
                            this.txtSearch_SName.Text = string.Empty;

                            this.txtSearch_PtInfo.Focus();

                        }
                    }
                }
                else
                {
                    ComFunc.MsgBox("값이 존재 하지 않습니다.");

                    this.txtSearch_PtInfo.Text = string.Empty;
                    this.txtSearch_SName.Text = string.Empty;

                    this.txtSearch_PtInfo.Focus();

                }
            }
        }

        void setPopUp(DataTable dt, string[] sSsel_BAS_PATIENT_PTINFO, int[] nSsel_BAS_PATIENT_PTINFO)
        {
            frmComSupPtInfo f;
            f = new frmComSupPtInfo(dt, this.pPSMH_Point, sSsel_BAS_PATIENT_PTINFO, nSsel_BAS_PATIENT_PTINFO, this);
            f.ePSMH_PTInfo += F_PSMH_PTInfoEvent;


            if (PSMH_TYPE == enmType.PTINFO)
            {
                f.Text = "환자정보";
            }
            else if (PSMH_TYPE == enmType.ERPINFO)
            {
                f.Text = "직원정보";
            }
            else if (PSMH_TYPE == enmType.BLOODINFO)
            {
                f.Text = "수혈환자";
            }
            else if (PSMH_TYPE == enmType.SPECINFO)
            {
                f.Text = "검체정보";
            }

            f.ShowDialog();

        }

        public void setSname()
        {
            setReturnValue();
        }

        ColumnHeader setListView_HeaderColumn(string strText, HorizontalAlignment hz, int width)
        {
            ColumnHeader ch = new ColumnHeader();
            ch.Text = strText;
            ch.TextAlign = hz;
            ch.Width = width;

            return ch;

        }

    }
}
