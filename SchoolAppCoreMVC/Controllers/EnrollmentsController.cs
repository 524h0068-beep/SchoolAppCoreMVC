
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolAppCoreMVC.Models;

public class EnrollmentsController : Controller
{
    private readonly SchoolContext _context;

    public EnrollmentsController(SchoolContext context)
    {
        _context = context;
    }

    // GET: Enrollments
    public async Task<IActionResult> Index()
    {
        var enrollments = _context.Enrollments
            .Include(e => e.Course)
            .Include(e => e.Student);

        return View(await enrollments.ToListAsync());
    }

    // GET: Enrollments/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var enrollment = await _context.Enrollments
            .Include(e => e.Course)
            .Include(e => e.Student)
            .FirstOrDefaultAsync(e => e.EnrollmentID == id);

        if (enrollment == null)
        {
            return NotFound();
        }

        return View(enrollment);
    }

    // GET: Enrollments/Create
    public IActionResult Create()
    {
        LoadCourses();
        LoadStudents();
        return View();
    }

    // POST: Enrollments/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("EnrollmentID,CourseID,StudentID,Grade")] Enrollment enrollment)
    {
        if (ModelState.IsValid)
        {
            _context.Add(enrollment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        LoadCourses(enrollment.CourseID);
        LoadStudents(enrollment.StudentID);

        return View(enrollment);
    }

    // GET: Enrollments/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var enrollment = await _context.Enrollments.FindAsync(id);

        if (enrollment == null)
        {
            return NotFound();
        }

        LoadCourses(enrollment.CourseID);
        LoadStudents(enrollment.StudentID);

        return View(enrollment);
    }

    // POST: Enrollments/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("EnrollmentID,CourseID,StudentID,Grade")] Enrollment enrollment)
    {
        if (id != enrollment.EnrollmentID)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(enrollment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnrollmentExists(enrollment.EnrollmentID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        LoadCourses(enrollment.CourseID);
        LoadStudents(enrollment.StudentID);

        return View(enrollment);
    }

    // GET: Enrollments/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var enrollment = await _context.Enrollments
            .Include(e => e.Course)
            .Include(e => e.Student)
            .FirstOrDefaultAsync(e => e.EnrollmentID == id);

        if (enrollment == null)
        {
            return NotFound();
        }

        return View(enrollment);
    }

    // POST: Enrollments/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var enrollment = await _context.Enrollments.FindAsync(id);

        if (enrollment != null)
        {
            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    // Load Course dropdown
    private void LoadCourses(object? selectedCourse = null)
    {
        ViewData["CourseID"] = new SelectList(
            _context.Courses.OrderBy(c => c.Title).ToList(),
            "CourseID",
            "Title",
            selectedCourse
        );
    }

    // Load Student dropdown
    private void LoadStudents(object? selectedStudent = null)
    {
        var students = _context.Students
            .OrderBy(s => s.LastName)
            .ThenBy(s => s.FirstName)
            .Select(s => new
            {
                s.StudentID,
                FullName = s.FirstName + " " + s.LastName
            })
            .ToList();

        ViewData["StudentID"] = new SelectList(
            students,
            "StudentID",
            "FullName",
            selectedStudent
        );
    }

    private bool EnrollmentExists(int id)
    {
        return _context.Enrollments.Any(e => e.EnrollmentID == id);
    }
}
