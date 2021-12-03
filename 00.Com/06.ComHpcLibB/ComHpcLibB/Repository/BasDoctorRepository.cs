namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class BasDoctorRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public BasDoctorRepository()
        {
        }

        public BAS_DOCTOR GetItembyDrCode(string strDrCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT DRNAME                                      ");
            parameter.AppendSql("  FROM ADMIN.BAS_DOCTOR                      ");
            parameter.AppendSql(" WHERE DRCODE = :DRCODE                            ");

            parameter.Add("DRCODE", strDrCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<BAS_DOCTOR>(parameter);
        }

        public List<BAS_DOCTOR> GetItembyDrCodes(List<string> strDrCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT DRDEPT1, DRCODE, DRNAME                     ");
            parameter.AppendSql("  FROM ADMIN.BAS_DOCTOR                      ");
            parameter.AppendSql(" WHERE 1=1                                         ");
            parameter.AppendSql(" AND DRCODE IN (:DRCODE)                           ");

            parameter.AddInStatement("DRCODE", strDrCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader <BAS_DOCTOR>(parameter);
        }

        public List<BAS_DOCTOR> GetItembyDrDept1DrCode(string strDeptCode, string strDRCODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT DRCODE,DRDEPT1,DRNAME,TELNO,ROWID           ");
            parameter.AppendSql("  FROM ADMIN.BAS_DOCTOR                      ");
            parameter.AppendSql(" WHERE TOUR = 'N'                                  ");
            parameter.AppendSql("   AND TELNO IS NOT NULL                           ");
            parameter.AppendSql("   AND DRDEPT1 = :DRDEPT                           ");
            parameter.AppendSql("   AND DRCODE  = :DRCODE                           ");

            parameter.Add("DRDEPT", strDeptCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("DRCODE", strDRCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<BAS_DOCTOR>(parameter);
        }
    }
}
