using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using EmployeeManagement.Models;
using EmployeeManagement.Data;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EmployeeManagementContext _context;


        public HomeController(ILogger<HomeController> logger,EmployeeManagementContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {            
            return View(await _context.Department.ToListAsync());
        }

        // GET: Home/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }            
            var employees = _context.Employee.Include(e => e.Department)
                .Where(e => e.Department.DeptId == id);
            if (employees == null)
            {
                return NotFound();
            }            
            ViewData["Department"] = employees.Count() > 0 ? employees.First().Department.Name
                                    : (await _context.Department.FirstOrDefaultAsync(m => m.DeptId == id)).Name;                                 
            return View("Dept/Details",employees);
        }

        // GET: Home/Create
        public IActionResult Create()
        {
            ViewData["DeptCode"] = new SelectList(_context.Department, "Code", "Code");
            return View("Shared/Create");
        }


        // POST: Home/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DeptId,Code,Name")] Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        // POST: Home/EmpCreate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmpCreate([Bind("EmpId,Name,DeptCode")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeptCode"] = new SelectList(_context.Department, "Code", "Code", employee.DeptCode);
            return View(employee);
        }


        // GET: Home/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Department.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            return View("Dept/Edit",department);
        }

        // POST: Home/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DeptId,Code,Name")] Department department)
        {
            if (id != department.DeptId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.DeptId))
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
            return View(department);
        }

        // GET: Home/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Department
                .FirstOrDefaultAsync(m => m.DeptId == id);
            if (department == null)
            {
                return NotFound();
            }

            return View("Dept/Delete",department);
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _context.Department.FindAsync(id);
            _context.Department.Remove(department);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(int id)
        {
            return _context.Department.Any(e => e.DeptId == id);
        }

        // GET: Home/EmpEdit/5
        public async Task<IActionResult> EmpEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["DeptCode"] = new SelectList(_context.Department, "Code", "Code", employee.DeptCode);
            return View("Emp/Edit",employee);
        }

        // POST: Home/EmpEdit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmpEdit(int id, [Bind("EmpId,Name,DeptCode")] Employee employee)
        {
            if (id != employee.EmpId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmpId))
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
            ViewData["DeptCode"] = new SelectList(_context.Department, "Code", "Code", employee.DeptCode);
            return View(employee);
        }

        // GET: Home/EmpDelete/5
        public async Task<IActionResult> EmpDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .Include(e => e.Department)
                .FirstOrDefaultAsync(m => m.EmpId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View("Emp/Delete",employee);
        }

        [HttpPost, ActionName("EmpDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmpDeleteConfirmed(int id)
        {
            var employee = await _context.Employee.FindAsync(id);
            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.EmpId == id);
        }


        public IActionResult Privacy()
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
