using System;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Student.DAL;
using Student.Models;

public class Student_DalBase : Dal_Helper
{
    SqlDatabase db = new SqlDatabase(ConncetionString);


    public bool StudentAdd(Student_Model student_Model)
    {
        try
        {
            DbCommand dbCommand;

            // Check for a new student
            if (student_Model.Studentid == 0) // Check for 0 instead of null
            {
                dbCommand = db.GetStoredProcCommand("PR_Insert_Student");
            }
            else
            {
                dbCommand = db.GetStoredProcCommand("PR_Update_Student");
                db.AddInParameter(dbCommand, "@Studentid", DbType.Int32, student_Model.Studentid);
            }

            // Add parameters
            db.AddInParameter(dbCommand, "@StudentName", DbType.String, student_Model.StudentName);
            db.AddInParameter(dbCommand, "@Mobile", DbType.String, student_Model.Mobile);
            db.AddInParameter(dbCommand, "@Email", DbType.String, student_Model.Email);
            db.AddInParameter(dbCommand, "@Gender", DbType.String, student_Model.Gender);
            db.AddInParameter(dbCommand, "@Password", DbType.String, student_Model.Password);
            db.AddInParameter(dbCommand, "@Date", DbType.DateTime, student_Model.Date);
            db.ExecuteNonQuery(dbCommand);
            return true;
        }
        catch (Exception e)
        {
            // Log the error message
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public DataTable StudentGet()
    {
        DataTable dt = new DataTable();
        try
        {
            DbCommand dbCommand = db.GetStoredProcCommand("PR_SelectAll_Student");

            using (IDataReader reader = db.ExecuteReader(dbCommand))
            {
                dt.Load(reader);
            }
            return dt;
        }
        catch (Exception e)
        {
            return null;
        }
    }
    public Student_Model EditStudent(Student_Model student_Model)
    {
        DataTable dt = new DataTable();
        try
        {
            // Fetch student data
            DbCommand dbCommand = db.GetStoredProcCommand("PR_Select_Student_By_PK");
            db.AddInParameter(dbCommand, "@Studentid", DbType.Int32, student_Model.Studentid);

            using (IDataReader reader = db.ExecuteReader(dbCommand))
            {
                dt.Load(reader);
            }

            // Check if data exists
            if (dt.Rows.Count == 0)
            {
                return null; // No record found
            }

            Student_Model mdl = new Student_Model();
            foreach (DataRow dr in dt.Rows)
            {
                mdl.Studentid = Convert.ToInt32(dr["Studentid"]);
                mdl.StudentName = dr["StudentName"].ToString();
                mdl.Email = dr["Email"].ToString();
                mdl.Gender = dr["Gender"].ToString();
                mdl.Mobile = dr["Mobile"].ToString();
                mdl.Password = dr["Password"].ToString();
                mdl.Date = Convert.ToDateTime(dr["date"].ToString());
            }

            return mdl;
        }
        catch (Exception e)
        {
            // Log the exception if necessary
            return null; // Handle exceptions as appropriate
        }
    }

    public void DeleteStudent(int Studentid)
    {
        try
        {
            DbCommand dbCommand = db.GetStoredProcCommand("PR_Delete_Student");
            db.AddInParameter(dbCommand, "@Studentid", DbType.Int32, Studentid);
            db.ExecuteNonQuery(dbCommand);
        }
        catch (Exception e)
        {
            return;
        }
    }
}