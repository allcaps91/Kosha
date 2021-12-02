using ComBase;
using ComBase.Mvc.Utils;
using ComDbB;
using HC.Core.Dto;
using HC.Core.Model;
using HC.Core.Repository;
using HC.Core.Service;
using HC_Core.Model;
using HC_Core.Service;
using HC_OSHA;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HC_OSHA_LOCALDB
{
    public partial class Login : Form
    {
        HcUserService hcUserService;
        Dashboard dashboard = null;
        DataSyncRepository dataSyncRepository;
        public Login()
        {
            InitializeComponent();
            hcUserService = new HcUserService();
            dataSyncRepository = new DataSyncRepository();
        }
        private void DoLogin()
        {
            //if (clsDB.DbCon != null && clsDB.DbCon.Con.State == System.Data.ConnectionState.Open)
            //{
            //    clsDB.DbCon.DisDBConnect();
            //}


            //try
            //{
            //    PsmhDb pPsmhDb = new PsmhDb();

            //    if (pPsmhDb.DBConnectEx("192.168.100.31", "1521", "ORA7", "KOSMOS_PMPA", "hospital") == true)
            //    {
            //        clsDB.DbCon = pPsmhDb;
            //    }

            //}
            //catch (Exception exc)
            //{

            //}

            try
            {
                //  HC_USER user = hcUserService.FindByUserId(CommonService.Instance.Session.UserId);

                dataSyncRepository.UpdatePasswordByNotbook(CommonService.Instance.Session.UserId);

            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }


            try
            {

                Cursor.Current = Cursors.WaitCursor;
                bool isValid = hcUserService.IsValid(txtIdNumber.Text.Trim(), txtPassword.Text.Trim());

                if (isValid)
                {
                    clsType.User.Sabun = txtIdNumber.Text;


                    dashboard = new Dashboard();
                    dashboard.exitDelegate += Dashboard_exitDelegate;
                    dashboard.Show();
                    this.Visible = false;

                }
                else
                {
                    Cursor.Current = Cursors.Default;
                    MessageUtil.Alert("아이디 또는 비빌번호를 확인하세요");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                Cursor.Current = Cursors.Default;
                MessageUtil.Alert(ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }

        private void Dashboard_exitDelegate()
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            DoLogin();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void txtIdNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return) txtPassword.Focus();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return) DoLogin();
        }

        /// <summary>
        /// DB 깨졋을경우 복구
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRecover_Click(object sender, EventArgs e)
        {
            if (MessageUtil.Confirm("노트북 DB 복구를 계속하시겠습니까?") == DialogResult.No)
            {
                return;
            }

            try
            {
                Cursor.Current = Cursors.WaitCursor;


                DataSyncService.Instance.ConnectOra7();

                List<DbLinkModel> tables = new List<DbLinkModel>();
                tables.Add(new DbLinkModel("HIC_LTD", true));
                tables.Add(new DbLinkModel("HIC_OSHA_SITE", true));
                tables.Add(new DbLinkModel("HIC_OSHA_CONTRACT", true));
                tables.Add(new DbLinkModel("HIC_OSHA_SCHEDULE", true));
                tables.Add(new DbLinkModel("HIC_USERS"));
                tables.Add(new DbLinkModel("HIC_CODE"));
                tables.Add(new DbLinkModel("HIC_CODES"));
                tables.Add(new DbLinkModel("HIC_OSHA_DATASYNC"));
                tables.Add(new DbLinkModel("HIC_OSHA_RELATION"));
                tables.Add(new DbLinkModel("HIC_OSHA_PRICE"));

                List<string> exportTables = new List<string>();
                for (int i = 0; i < tables.Count; i++)
                {
                    exportTables.Add(tables[i].TableName);
                }

                string tableList = string.Join(",", exportTables);

                DataSyncService.Instance.Export(tableList);

                try
                {
                    DataSyncService.Instance.ConnectNotebook();
                }
                catch (Exception ex)
                {
                    MessageUtil.Alert(ex.Message);
                    return;
                }

                string servieName = clsDB.DbCon.Con.ServiceName;
                if (servieName == "psmh_dev" || servieName == "orcl" || servieName == "xe")
                {
                    try
                    {

                        DataSyncRepository dataSyncRepository = new DataSyncRepository();
                        for (int i = 0; i < tables.Count; i++)
                        {

                            if (dataSyncRepository.HasTable(tables[i].TableName))
                            {
                                dataSyncRepository.DropTable(tables[i].TableName);
                            }
                        }

                        dataSyncRepository.CreateSiteView();
                    }
                    catch (Exception ex)
                    {
                        MessageUtil.Alert("테이블 삭제중 오류가 발생했습니다 \n" + ex.Message);
                    }
                }

                DataSyncService.Instance.Import(tableList, null);

                MessageUtil.Info("DB 복구  완료!");

            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                Log.Error(ex);
                MessageUtil.Alert(ex.Message);

            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }


        }
    }
}
