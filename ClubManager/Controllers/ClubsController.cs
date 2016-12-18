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
    public class ClubsController : Controller
    {
        private ClubDbContext db = new ClubDbContext();
        private ApplicationDbContext db2 = new ApplicationDbContext();

        // GET: Clubs
        public ActionResult Index(string Search_ClubName = null)
        {
            ViewBag.CurrentSearch = Search_ClubName;
            IQueryable<Club> clubs = db.Clubs;

            if (!string.IsNullOrEmpty(Search_ClubName))
            {
                clubs = clubs.Where(c => Search_ClubName == null || c.ClubName.Contains(Search_ClubName));
            }

            return View(clubs.ToList());
        }

        public PartialViewResult _Clubs()
        {
            return PartialView(db.Clubs.ToList());
        }

        [Authorize]
        public RedirectResult Join(int id)
        {
            using (ApplicationDbContext ctx = new ApplicationDbContext())
            {
                ApplicationUser user = ctx.Users.Where(u => u.Email == User.Identity.Name).First();

                IQueryable<Member> members = db.Members.Where(m => m.ClubId == id && m.StudentId == user.StudentId);

                if (members.Count() == 0)
                {
                    db.Members.Add(new Member
                    {
                        ClubId = id,
                        StudentId = user.StudentId,
                        Approved = false
                    });

                    db.SaveChanges();
                }
            }

            return Redirect(string.Format("/Clubs/Details/{0}", id));
        }


        // GET: Clubs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Club club = db.Clubs.Find(id);
            if (club == null)
            {
                return HttpNotFound();
            }

            ViewBag.clubId = id;
            return View(club);
        }

        // GET: Clubs/Create
        public ActionResult _Create()
        {
            return PartialView(new Club
            {
                CreationDate = DateTime.Now
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _Create(Club club)
        {
            if (ModelState.IsValid)
            {
                db.Clubs.Add(club);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Home", new { id = club.ClubId });
        }

        // POST: Clubs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClubId,ClubName,CreationDate,AdminId")] Club club)
        {
            if (ModelState.IsValid)
            {
                db.Clubs.Add(club);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(club);
        }

        // GET: Clubs/Edit/5
        [Authorize(Roles = "Admin, ClubAdmin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Club club = db.Clubs.Find(id);
            if (club == null)
            {
                return HttpNotFound();
            }
            return View(club);
        }

        // POST: Clubs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClubId,ClubName,CreationDate,AdminId")] Club club)
        {
            if (ModelState.IsValid)
            {
                db.Entry(club).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(club);
        }

        // GET: Clubs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Club club = db.Clubs.Find(id);
            if (club == null)
            {
                return HttpNotFound();
            }
            return View(club);
        }

        // POST: Clubs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            Club club = db.Clubs.Where(c => c.ClubId == id).First();
            db.Clubs.Remove(club);
            //db.SaveChanges();
            return RedirectToAction("Index");
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
