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
using System.Threading.Tasks;
using ClubManager.Models;

namespace ClubManager.Controllers
{
    public class ClubEventsController : Controller
    {
        private ClubDbContext db = new ClubDbContext();

        public PartialViewResult _Create(int? id)
        {
            if (id == null)
                throw new ArgumentNullException();
            else
                id = (int)id;

            return PartialView(new ClubEvent
            {
                ClubId = (int)id,
                StartDateTime = DateTime.Now,
                EndDateTime = DateTime.Now
            });
        }

        public async Task<ActionResult> Join(int? id)
        {
            if (id == null)
                throw new ArgumentNullException();
            else
                id = (int)id;

            using (ApplicationDbContext ctx = new ApplicationDbContext())
            {
                var user = ctx.Users.Where(u => u.Email == User.Identity.Name).First();

                var member = db.Members.Where(m => m.student.StudentId == user.StudentId).First();
                var ev = db.ClubEvents.Where(e => e.EventId == id).First();
                ev.attendees.Add(member);
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Clubs", new { id = ev.ClubId});
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _Create([Bind(Include = "EventID,Venue,Location,ClubId,StartDateTime,EndDateTime")] ClubEvent clubEvent)
        {
            if (ModelState.IsValid)
            {
                db.ClubEvents.Add(new ClubEvent
                {
                    ClubId = clubEvent.ClubId,
                    Venue = clubEvent.Venue,
                    Location = clubEvent.Location,
                    StartDateTime = clubEvent.StartDateTime,
                    EndDateTime = clubEvent.EndDateTime
                });
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Details", "Clubs", new { id = clubEvent.ClubId });
        }

        // GET: ClubEvents
        public ActionResult Index()
        {
            var clubEvents = db.ClubEvents.Include(c => c.associatedClub);
            return View(clubEvents.ToList());
        }

        public PartialViewResult _Attendees(int? id)
        {
            var data = db.ClubEvents
                .Where(ce => ce.EventId == id)
                .Select(m => m.attendees).First();
            return PartialView(data);
        }

        public PartialViewResult _ClubEvents(int? id)
        {
            ViewBag.clubId = id;
            var data = db.ClubEvents.Where(x => x.ClubId == id);
            return PartialView(data.ToList());
        }

        // GET: ClubEvents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClubEvent clubEvent = db.ClubEvents.Find(id);
            if (clubEvent == null)
            {
                return HttpNotFound();
            }
            return View(clubEvent);
        }

        // GET: ClubEvents/Create
        public ActionResult Create()
        {
            ViewBag.ClubId = new SelectList(db.Clubs, "ClubId", "ClubName");
            return View();
        }

        // POST: ClubEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EventId,Venue,Location,StartDateTime,EndDateTime,ClubId")] ClubEvent clubEvent)
        {
            if (ModelState.IsValid)
            {
                db.ClubEvents.Add(clubEvent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClubId = new SelectList(db.Clubs, "ClubId", "ClubName", clubEvent.ClubId);
            return View(clubEvent);
        }

        // GET: ClubEvents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClubEvent clubEvent = db.ClubEvents.Find(id);
            if (clubEvent == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClubId = new SelectList(db.Clubs, "ClubId", "ClubName", clubEvent.ClubId);
            return View(clubEvent);
        }

        // POST: ClubEvents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EventId,Venue,Location,StartDateTime,EndDateTime,ClubId")] ClubEvent clubEvent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clubEvent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Clubs", new { id = clubEvent.ClubId });
            }
            ViewBag.ClubId = new SelectList(db.Clubs, "ClubId", "ClubName", clubEvent.ClubId);
            return View(clubEvent);
        }

        // GET: ClubEvents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClubEvent clubEvent = db.ClubEvents.Find(id);
            if (clubEvent == null)
            {
                return HttpNotFound();
            }
            return View(clubEvent);
        }

        // POST: ClubEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ClubEvent clubEvent = db.ClubEvents.Find(id);
            db.ClubEvents.Remove(clubEvent);
            db.SaveChanges();
            return RedirectToAction("Details", "Clubs", new { id = clubEvent.ClubId });
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
