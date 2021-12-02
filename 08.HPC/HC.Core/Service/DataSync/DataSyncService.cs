using ComBase;
using ComBase.Mvc.Utils;
using HC.Core.Model;
using HC.Core.Dto;
using HC.Core.Repository;
using HC.Core.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HC_Core.Model;

namespace HC_Core.Service
{
    public class DataSyncService
    {
        /// <summary>
        /// 오프라인 노특북 , 로컬 DB 사용 여부
        /// </summary>
        public bool IsLocalDB { get; set; }
        private readonly static object Lock_Object = new object();
        private static DataSyncService instance;
        private DataSyncRepository dataSyncRepository;

        string tempBatchFile = @"Resources\dmp\osha_export.bat";
        string tempBatchFileBySchema = @"Resources\dmp\osha_export_schema.bat";

        public DataSyncService()
        {
            IsLocalDB = false;
            dataSyncRepository = new DataSyncRepository();
        }
        public static DataSyncService Instance
        {
            get
            {
                lock (Lock_Object)
                {
                    if (instance == null)
                    {
                        instance = new DataSyncService();
                    }
                    return instance;
                }

            }
        }
        /// <summary>
        /// 노트북의 싱크정보를 ORA7에 업로드 운영DB의 싱크정보는 모두 삭제한다.
        /// </summary>
        public bool CopyFromNotebook()
        {
            try
            {
                ConnectNotebook();

                List<DataSyncDto> list = dataSyncRepository.GetNotSyncAll();

                ConnectOra7();

                clsDB.setBeginTran(clsDB.DbCon);

                foreach (DataSyncDto dto in list)
                {
                    dataSyncRepository.Insert(dto);
                }

                clsDB.setCommitTran(clsDB.DbCon);

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                clsDB.setRollbackTran(clsDB.DbCon);
                return false;

            }

        }
        public void ConnectOra7()
        {
            if (clsDB.DbCon != null && clsDB.DbCon.Con.State == System.Data.ConnectionState.Open)
            {

                clsDB.DbCon.DisDBConnect();

            }


            clsDB.DbCon = clsDB.DBConnect("192.168.100.31", "1521", "ORA7", "KOSMOS_PMPA", "hospital");
            DataSyncService.Instance.IsLocalDB = false;
            if (clsDB.DbCon == null)
            {
                MessageUtil.Alert("원내 DB에 연결할 수 없습니다");
            }
        }
        public void ConnectNotebook()
        {
            if (clsDB.DbCon != null)
            {
                clsDB.DbCon.DisDBConnect();
            }

            clsDB.DbCon = clsDB.DBConnect("localhost", "1521", "ORCL", "KOSMOS_PMPA", "hospital");

            DataSyncService.Instance.IsLocalDB = true;
            if (clsDB.DbCon == null)
            {
                MessageUtil.Alert("노트북 DB에 연결할 수 없습니다");
            }

        }


        public void Insert(string tableName, long tableKey)
        {
            Save(tableName, tableKey.ToString(), "I");
        }
        public void Insert(string tableName, string tableKey)
        {
            Save(tableName, tableKey, "I");
        }
        public void Update(string tableName, string tableKey)
        {
            Save(tableName, tableKey, "U");
        }
        public void Update(string tableName, long tableKey)
        {
            Save(tableName, tableKey.ToString(), "U");
        }
        public void Delete(string tableName, long tableKey)
        {
            Save(tableName, tableKey.ToString(), "D");
        }
        public void Delete(string tableName, string tableKey)
        {
            Save(tableName, tableKey, "D");
        }
        //public void UpdateDelete(string tableName, long tableKey)
        //{
        //    Save(tableName, tableKey.ToString(), "N");
        //}
        //public void UpdateDelete(string tableName, string tableKey)
        //{
        //    Save(tableName, tableKey, "N");
        //}
        private void Save(string tableName, string tableKey, string dmlType)
        {
            if (IsLocalDB)
            {
                DataSyncDto dto = new DataSyncDto();
                dto.TABLENAME = tableName;
                dto.TABLEKEY = tableKey;
                dto.DMLTYPE = dmlType;
                dto.ISSYNC = "N";
                dataSyncRepository.Insert(dto);
            }

        }


        public void Export(string tables)
        {
            string command = @"exp kosmos_pmpa/hospital@ora7 tables=" + tables + " file='Resources\\dmp\\hic.dmp' ";
            StreamWriter writer = new StreamWriter(tempBatchFile, false, Encoding.Default);
            writer.WriteLine(command);
            writer.Close();

            ProcessStartInfo pi = new ProcessStartInfo();
            Process process = new Process();

            pi.FileName = tempBatchFile;
            pi.CreateNoWindow = false;
            pi.UseShellExecute = false;

            process.StartInfo = pi;
            process.Start();

            process.WaitForExit();
            process.Close();

        }
        public void ImportSchema(string schemaTables)
        {
            string servieName = clsDB.DbCon.Con.ServiceName;
            if (servieName == "psmh_dev" || servieName == "orcl")
            {


                string command = @"imp kosmos_pmpa/hospital@orcl tables=" + schemaTables + " file='Resources\\dmp\\hic_schema.dmp' ignore=y";
                StreamWriter writer = new StreamWriter(tempBatchFileBySchema, false, Encoding.Default);
                writer.WriteLine(command);
                writer.Close();

                ProcessStartInfo pi = new ProcessStartInfo();
                Process process = new Process();

                pi.FileName = tempBatchFileBySchema;
                pi.CreateNoWindow = false;
                pi.UseShellExecute = false;

                process.StartInfo = pi;
                process.Start();

                process.WaitForExit();
                process.Close();
            }
        }

        public void Import(string tables, List<SequenceModel> seqList)
        {
            string servieName = clsDB.DbCon.Con.ServiceName;
            if (servieName == "psmh_dev" || servieName == "orcl" || servieName == "xe")
            {
                if (seqList != null)
                {
                    dataSyncRepository.CreateSequence(seqList);
                }

                string command = @"imp kosmos_pmpa/hospital@orcl tables=" + tables + " file='Resources\\dmp\\hic.dmp' ignore=y";
                using (StreamWriter writer = new StreamWriter(tempBatchFile, false, Encoding.Default))
                {



                    writer.WriteLine(command);
                    writer.Close();

                    ProcessStartInfo pi = new ProcessStartInfo();
                    Process process = new Process();

                    pi.FileName = tempBatchFile;
                    pi.CreateNoWindow = false;
                    pi.UseShellExecute = false;

                    process.StartInfo = pi;
                    process.Start();

                    process.WaitForExit();
                    process.Close();
                }

            }
        }

        public void ExportSchema(string tables)
        {
            string command = @"exp kosmos_pmpa/hospital@ora7 tables=" + tables + " file='Resources\\dmp\\hic_schema.dmp' rows=n ";
            StreamWriter writer = new StreamWriter(tempBatchFileBySchema, false, Encoding.Default);
            writer.WriteLine(command);
            writer.Close();

            ProcessStartInfo pi = new ProcessStartInfo();
            Process process = new Process();

            pi.FileName = tempBatchFileBySchema;
            pi.CreateNoWindow = false;
            pi.UseShellExecute = false;

            process.StartInfo = pi;
            process.Start();

            process.WaitForExit();
            process.Close();

        }
    }
}
