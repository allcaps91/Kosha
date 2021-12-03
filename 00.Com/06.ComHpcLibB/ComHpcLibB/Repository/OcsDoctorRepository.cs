namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class OcsDoctorRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public OcsDoctorRepository()
        {
        }

        public string GetDrCodebySabun(string strSabun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT DRCODE                  ");
            parameter.AppendSql("  FROM KOSMOS_OCS.OCS_DOCTOR   ");
            parameter.AppendSql(" WHERE SABUN = :SABUN          ");

            parameter.Add("SABUN", strSabun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public string GetDrNamebySabun(string strSabun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT DRNAME                  ");
            parameter.AppendSql("  FROM KOSMOS_OCS.OCS_DOCTOR   ");
            parameter.AppendSql(" WHERE SABUN = :SABUN          ");

            parameter.Add("SABUN", strSabun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public string GetSabunByDrCode(string argDrCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SABUN                   ");
            parameter.AppendSql("  FROM KOSMOS_OCS.OCS_DOCTOR   ");
            parameter.AppendSql(" WHERE DRCODE = :DRCODE        ");

            parameter.Add("DRCODE", argDrCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public string GetRedadDrNmaebyDrBunho(long pANJENGDRNO8)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT DRNAME                      ");
            parameter.AppendSql("  FROM KOSMOS_OCS.OCS_DOCTOR       ");
            parameter.AppendSql(" WHERE DRBUNHO = :DRBUNHO          ");

            parameter.Add("DRBUNHO", pANJENGDRNO8, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }
    }
}
