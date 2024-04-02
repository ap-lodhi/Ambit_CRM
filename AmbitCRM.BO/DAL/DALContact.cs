using AmbitCRM.BO.Helper;

using AmbitCRM_Core.Interface;
using AmbitCRM_Core.Model;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace AmbitCRM.BO.DAL
{
    public class DALContact : IDALContact
    {

        #region Get  Contact  List 
        public List<ContactModel> GetContactList(int? start, int? length, string? searchText, string? sortColumn, string? sortDirection, string? CompanyName, string? ContactName, string? Email, string? City)

        {
            DALCommon log = new DALCommon();
            List<ContactModel> ContactDetails = new List<ContactModel>();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString);


            try
            {

                con.Open();

                var cmd = new SqlCommand();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@start_index", start);
                parameters.Add("@page_size", length);
                parameters.Add("@search_text", searchText);
                parameters.Add("@sort_column", sortColumn);
                parameters.Add("@sort_direction", sortDirection);
                parameters.Add("@CompanyName", CompanyName);
                parameters.Add("@ContactName", ContactName);
                parameters.Add("@Email", Email);
                parameters.Add("@City", City);

                ContactDetails = con.Query<ContactModel>("EXEC SP_GetContactDetailsList @start_index,@page_size,@search_text,@sort_column,@sort_direction,@CompanyName, @ContactName,@Email,@City", parameters).ToList();



            }
            catch (Exception ex)
            {

                log.SaveLog("Get ContactList", "" + ex);
            }
            finally
            {
                con.Close();
                //con.Dispose();
            }
            return ContactDetails;

        }

        #endregion


        #region BookMarked Status 


        public ResponseModel BookmarkStatus(BookmarkedModel BM )
        {
            ResponseModel result = new ResponseModel();
            DALCommon log = new DALCommon();
            using (SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString))
            {
                try
                {
                    con.Open();

               

                    using (SqlCommand cmd = new SqlCommand("sp_save_and_update_boomark_details", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@UserId", BM.UserId);
                        cmd.Parameters.AddWithValue("@ContactId", BM.ContactId);
                        cmd.Parameters.AddWithValue("@IsBookMarked", BM.IsBookMarked);
                        cmd.Parameters.AddWithValue("@CreatedBy", BM.UserId);
                        


                        var id = cmd.ExecuteNonQuery();

                        if (id > 0)
                        {
                            result.status = true;
                            result.message = "BookMarked Status Update Save Successfully";
                            log.SaveLog("Book Marked ", "BookMarked Status Update Save Successfully");
                        }
                        else
                        {
                            result.status = false;
                            result.message = "Please Check...Something Went wrong...!!!";
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.status = false;
                    log.SaveLog("BookMarked LoginDetails", "ex" + ex);

                }
                finally
                {
                    con.Close();
                }
            }
            return result;
        }
        #endregion

        public ResponseModel SaveSearch(SearchModel SM)
        {
            ResponseModel result = new ResponseModel();
            DALCommon log = new DALCommon();
            using (SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString))
            {
                try
                {
                    con.Open();



                    using (SqlCommand cmd = new SqlCommand("sp_save_search_details", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@SearchId", SM.searchId);
                        cmd.Parameters.AddWithValue("@UserId",SM.UserID );
                        cmd.Parameters.AddWithValue("@CompanyName",SM.CompanyName );
                        cmd.Parameters.AddWithValue("@ContactName",SM.ContactName );
                        cmd.Parameters.AddWithValue("@Email", SM.Email);
                        cmd.Parameters.AddWithValue("@City", SM.City);
                        cmd.Parameters.AddWithValue("@@CreatedBy", SM.UserID);



                        var id = cmd.ExecuteNonQuery();

                        if (id > 0)
                        {
                            result.status = true;
                            result.message = "search save  Successfully";
                            log.SaveLog("Search", "Search  Save Successfully");
                        }
                        else
                        {
                            result.status = false;
                            result.message = "Please Check...Something Went wrong...!!!";
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.status = false;
                    log.SaveLog("Search Save", "ex" + ex);

                }
                finally
                {
                    con.Close();
                }
            }
            return result;
        }


    }
}
