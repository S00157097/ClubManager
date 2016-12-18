using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClubManager.Models.ClubModels;
using ClubManager.Models.Clubs;
using ClubManager.Models;
using System.Threading.Tasks;

namespace ClubManager.Controllers
{
    public class MembersController : Controller
    {
        private ClubDbContext db = new ClubDbContext();

        // GET: Members
        public ActionResult Index()
        {
            var members = db.Members.Include(m => m.club).Include(m => m.student);
            return View(members.ToList());
        }

        public PartialViewResult _Create(int? id)
        {
            if (id == null)
                throw new ArgumentNullException();
            else
                id = (int)id;

            using (ApplicationDbContext ctx = new ApplicationDbContext())
            {
                ApplicationUser user = ctx.Users
                    .Where(u => u.Email == User.Identity.Name)
                    .FirstOrDefault();

                return PartialView(new Member
                {
                    ClubId = (int)id,
                    StudentId = user.StudentId
                });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _Create(Member member)
        {
            if (ModelState.IsValid)
            {
                db.Members.Add(member);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Details", "Clubs", new { id = member.ClubId });
        }

        public PartialViewResult _ClubMembers(int? id)
        {
            ViewBag.clubId = id;
            var data = db.Members.Where(m => m.ClubId == id);
            return PartialView(data.ToList());
        }

        // GET: Members/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.FirstOrDefault(m => m.MemberId == id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // GET: Members/Create
        public ActionResult Create()
        {
            ViewBag.ClubId = new SelectList(db.Clubs, "ClubId", "ClubName");
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FirstName");
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MemberId,ClubId,StudentId,Approved")] Member member)
        {
            if (ModelState.IsValid)
            {
                db.Members.Add(member);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClubId = new SelectList(db.Clubs, "ClubId", "ClubName", member.ClubId);
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FirstName", member.StudentId);
            return View(member);
        }

        // GET: Members/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.FirstOrDefault(m => m.MemberId == id);
            if (member == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClubId = new SelectList(db.Clubs, "ClubId", "ClubName", member.ClubId);
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FirstName", member.StudentId);
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MemberId,ClubId,StudentId,Approved")] Member member)
        {
            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Clubs", new { id = member.ClubId });
            }
            ViewBag.ClubId = new SelectList(db.Clubs, "ClubId", "ClubName", member.ClubId);
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FirstName", member.StudentId);
            return View(member);
        }

        // GET: Members/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.FirstOrDefault(m => m.MemberId == id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Member member = db.Members.FirstOrDefault(m => m.MemberId == id);
            db.Members.Remove(member);
            db.SaveChanges();
            return RedirectToAction("Details", "Clubs", new { id = member.ClubId });
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
