using Microsoft.AspNetCore.Mvc;
using Student.DAL;
using Student.Models;
using System.Data;
using System.Diagnostics;

namespace Student.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        Student_DalBase _studentDalBase = new Student_DalBase();

        public IActionResult Edit(int Studentid)
        {
            Student_Model student_Model = new Student_Model { Studentid = Studentid };

            student_Model = _studentDalBase.EditStudent(student_Model);

            if (student_Model == null)
            {
                return RedirectToAction("StudentList");
            }

            return View("StudentAdd", student_Model);
        }

        // Action method to get the student list
        public IActionResult StudentList()
        {
            DataTable dt = _studentDalBase.StudentGet();
            return View(dt);
        }

        // Action method to delete a student
        public IActionResult Delete(int Studentid)
        {
            _studentDalBase.DeleteStudent(Studentid);
            return RedirectToAction("StudentList");
        }

        // Action method to save a student
        [HttpPost]
        public IActionResult Save(Student_Model student_Model)
        {
                           // Attempt to add the student using the data access layer
                if (_studentDalBase.StudentAdd(student_Model))
                {
                    return RedirectToAction("StudentList"); // Redirect to the list view upon success
                }
                else
                {
                    ModelState.AddModelError("", "An error occurred while adding the student.");
                }
            return View("StudentAdd", student_Model); // Return the Add view with the current model for validation errors
        }


        // Action method to show the student add form
        public IActionResult StudentAdd()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
