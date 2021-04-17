using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using HindusthanUniversity.DAL;
using HindusthanUniversity.Models;
using System.Data.Entity.Infrastructure;
using PagedList;

namespace HindusthanUniversity.Controllers
{
    public class StudentsController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Students
        public ActionResult Index(string sortOrder, string searchString)
        {
            try
            {
                ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                var studentsRecord = from s in db.Students
                                     select s;
                if (!string.IsNullOrEmpty(searchString))
                {
                    studentsRecord = studentsRecord.Where(d => d.LastName.Contains(searchString) || d.FirstMidName.Contains(searchString));
                }
                switch (sortOrder)
                {
                    case "name_desc":
                        studentsRecord = studentsRecord.OrderByDescending(s => s.LastName);
                        break;
                    case "Date":
                        studentsRecord = studentsRecord.OrderBy(d => d.EnrollmentDate);
                        break;
                    case "date_desc":
                        studentsRecord = studentsRecord.OrderByDescending(d => d.EnrollmentDate);
                        break;
                    default:
                        studentsRecord = studentsRecord.OrderBy(s => s.LastName);
                        break;
                }
                return View(studentsRecord.ToList());
            }
            catch(RetryLimitExceededException)
            {                
                ModelState.AddModelError("", "Unable to savechanges. Try again!");
                //TODO: Logging

                return RedirectToRoute("~/Shared/Error");
    }
}

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Student student = db.Students.Find(id);
                if (student == null)
                {
                    return HttpNotFound();
                }
                return View(student);
            }
            catch
            {
                return RedirectToRoute("~/Shared/Error");
            }
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            try
            {
                return View();
            }
            catch
            {
                return RedirectToRoute("~/Shared/Error");
            }
}

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,LastName,FirstMidName,EnrollmentDate")] Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Students.Add(student);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(student);
            }
            catch
            {
                return RedirectToRoute("~/Shared/Error");
            }
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Student student = db.Students.Find(id);
                if (student == null)
                {
                    return HttpNotFound();
                }
                return View(student);
            }
            catch
            {
                return RedirectToRoute("~/Shared/Error");
            }
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,LastName,FirstMidName,EnrollmentDate")] Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(student).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(student);
            }
            catch
            {
                return RedirectToRoute("~/Shared/Error");
            }
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Student student = db.Students.Find(id);
                if (student == null)
                {
                    return HttpNotFound();
                }
                return View(student);
            }
            catch
            {
                return RedirectToRoute("~/Shared/Error");
            }
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Student student = db.Students.Find(id);
                db.Students.Remove(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToRoute("~/Shared/Error");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
