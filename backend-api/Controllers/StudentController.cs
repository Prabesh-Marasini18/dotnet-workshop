using Microsoft.AspNetCore.Mvc;
using WeatherAPI.Models;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        
        private static List<Student> students = new List<Student>
        {
            new Student { Id = "NP01MS7A240036", Name = "John Doe", Age = 20, Course = "Computer Science" },
            new Student { Id = "NP01MS7A240037", Name = "Jane Smith", Age = 21, Course = "Information Technology" },
            new Student { Id = "NP01MS7A240038", Name = "Bob Johnson", Age = 19, Course = "Software Engineering" },
            new Student { Id = "NP01MS7A240039", Name = "Alice Williams", Age = 22, Course = "Data Science" }
        };

    
        [HttpGet("getall")]
        public ActionResult<IEnumerable<Student>> GetAllStudents()
        {
            return Ok(students);
        }

    
        [HttpGet("{id}")]
        public ActionResult<Student> GetStudentById(string id)
        {
            var student = students.FirstOrDefault(s => s.Id == id);
            
            if (student == null)
            {
                return NotFound(new { message = $"Student with ID {id} not found" });
            }

            return Ok(student);
        }
    }
}
