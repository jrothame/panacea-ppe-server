using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using upload.web.Controllers.api;
using static upload.web.Controllers.api.BatchController;

namespace upload.web.Classes
{

    public class HCPCSFile
    {
        public int file_id { get; set; }
        public string file_desc { get; set; }
     
    }

    public class ExtensionMethods
    {
        public const string SQLCONN = "Server=10.7.2.12;Database=zbp_dev;User Id=jrothamel;Password=AM34pe9x!@#$";
        public static DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable dt = new DataTable();
            for (int i = 0; i < properties.Count; i++)
            {
                PropertyDescriptor property = properties[i];
                dt.Columns.Add(property.Name, property.PropertyType);
            }
            object[] values = new object[properties.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = properties[i].GetValue(item);
                }
                dt.Rows.Add(values);
            }
            return dt;
        }

        //this method grabs the current list of rev codes and returns
        //the data as an array list of string values
        public static List<string> GetRevCodes(string fileId)
        {
            List<string> revCodes = new List<string>();

            try
            {
                using (SqlConnection conn = new SqlConnection(SQLCONN))
                {

                    conn.Open();

                    //string cmdText = "select rev_code from rev_code_xwalk where file_id = " + fileId + " order by rev_code";

                    SqlCommand cmd = new SqlCommand("get_rev_codes", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@fileId", SqlDbType.Int).Value = Convert.ToInt32(fileId);


                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {

                        string revCode = reader["rev_code"].ToString();

                        revCodes.Add(revCode);

                    }

                }

                return revCodes;
            }

            catch (Exception ex)
            {
                throw ex;
            }
          
        }

        public static List<string> GetHCPCSCodes(string fileType)
        {
            List<string> hcpcsCodes = new List<string>();


            try
            {
                using (SqlConnection conn = new SqlConnection(SQLCONN))
                {

                    conn.Open();

                    SqlCommand cmd = new SqlCommand("get_hcpcs_codes", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@fileType", SqlDbType.VarChar).Value = fileType;


                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {

                        string hcpcsCode = reader["hcpcs"].ToString();

                        hcpcsCodes.Add(hcpcsCode);

                    }

                }

                return hcpcsCodes;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static List<HCPCSFile> GetHCPCSFiles()
        {
            List<HCPCSFile> files = new List<HCPCSFile>();

            try
            {
                using (SqlConnection conn = new SqlConnection(SQLCONN))
                {

                    conn.Open();

                    SqlCommand cmd = new SqlCommand("ppe_get_hcpcs_files", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {

                        HCPCSFile file = new HCPCSFile();

                        file.file_id = (int)reader["file_id"];
                        file.file_desc = reader["file_desc"].ToString();

                        files.Add(file);
                      


                    }

                }

                return files;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static bool InsertBatch(byte[] data, List<Batch> rows)
        {
            int rowCount = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(SQLCONN))
                {

                    conn.Open();

                    string cmdText = "ppe_insert_batch";


                    using (SqlCommand cmd = new SqlCommand(cmdText, conn))
                    {
                        cmd.Parameters.Add("@batchId", SqlDbType.VarChar).Value = rows[0].batchId;
                        cmd.Parameters.Add("@fileName", SqlDbType.VarChar).Value = rows[0].fileName;
                        cmd.Parameters.Add("@created", SqlDbType.DateTime).Value = rows[0].created;
                        cmd.Parameters.Add("@submitted", SqlDbType.DateTime).Value = rows[0].submitted;
                        cmd.Parameters.Add("@data", SqlDbType.Binary).Value = data;
                        cmd.CommandType = CommandType.StoredProcedure;

                        rowCount = cmd.ExecuteNonQuery();

                        conn.Close();
    
                        return (rowCount == 1);
                    }
            

                   // result = Request.CreateResponse(HttpStatusCode.OK, "The file was loaded into the database successfully!");
                }
            }

            catch(Exception ex)
            {
                throw ex;
            }
          
        

        }

        public static bool InsertBatchData(string batchId, List<BatchData> rows)
        {
            //int rowCount = 0;

            try
            {
                DataTable dt = new DataTable();

                dt = ToDataTable(rows);

                //add the batch_id to the datatable
                dt.Columns.Add(new DataColumn("batch_id"));

                foreach(DataRow row in dt.Rows)
                {
                    row["batch_id"] = batchId;


                }

                using (SqlConnection conn = new SqlConnection(SQLCONN))
                {

                    conn.Open();


                    SqlBulkCopy bulkCopy = new SqlBulkCopy(conn);

                    bulkCopy.DestinationTableName = "ppe_file_import_raw";

                    bulkCopy.ColumnMappings.Add("batch_id", "batch_id");
                    bulkCopy.ColumnMappings.Add("codedesc", "codedesc");
                    bulkCopy.ColumnMappings.Add("contractid", "contractid");
                    bulkCopy.ColumnMappings.Add("rev", "rev");
                    bulkCopy.ColumnMappings.Add("hcpcs", "hcpcs");
                    bulkCopy.ColumnMappings.Add("productid", "productid");
                    bulkCopy.ColumnMappings.Add("ip_ind", "ip_ind");
                    bulkCopy.ColumnMappings.Add("er_ind", "er_ind");
                    bulkCopy.ColumnMappings.Add("op_ind", "op_ind");
                    bulkCopy.ColumnMappings.Add("sds_ind", "sds_ind");
                    bulkCopy.ColumnMappings.Add("ex_ip_ind", "ex_ip_ind");
                    bulkCopy.ColumnMappings.Add("ex_er_ind", "ex_er_ind");
                    bulkCopy.ColumnMappings.Add("ex_op_ind", "ex_op_ind");
                    bulkCopy.ColumnMappings.Add("ex_sds_ind", "ex_sds_ind");

                    bulkCopy.WriteToServer(dt);

                    conn.Close();

                    return true;
       
 
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }



        }


        //this method creates a web client and downloads the original source data
        //using FlatFileIO Api
        //returns: byte array
        public static async Task<byte[]> DownloadFile(string url)
        {

            var client = new HttpClient();
            var response = await client.GetAsync(url);
            byte[] bytes;

            using (MemoryStream stream = (MemoryStream)await response.Content.ReadAsStreamAsync())
            {
                bytes = stream.ToArray();
   
            }

            return bytes;

        }
    }
}