using System.Data;
using System.Diagnostics;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;


namespace MyServer
{
    public class Repository
    {
        EventsContext db;

        public Repository()
        {
            db = new EventsContext();
        }

        public Repository(EventsContext db)
        {
            this.db = db;
        }
        public void CreateEvent(string Name, DateTime startDate, DateTime endDate, int MaxRegistrations, string Location)
        {
            Event newEvent = new Event();
            newEvent.Name = Name;
            newEvent.StartDate = startDate;
            newEvent.EndDate = endDate;
            newEvent.MaxRegistrations = MaxRegistrations;   
            newEvent.Location = Location;
            db.Events.Add(newEvent);
            db.SaveChanges();
        }
        public List<User> FetchUsersOfEvent(int eventID)
        {
            return db.EventUsers.Include(e => e.UserRefNavigation).Where(e => e.EventRef == eventID).Select(u => new User { Name = u.UserRefNavigation.Name, Id = u.UserRef, DateOfBirth = u.UserRefNavigation.DateOfBirth }).ToList();
        }

        public void RegisterUserToEvent(int UserId, int EventId)
        {
            EventUser relation = new EventUser();
            relation.UserRef = UserId;
            relation.EventRef = EventId;
            relation.Creation = DateTime.Now;
            db.Add(relation);
            db.SaveChanges();
        }

        public Event FetchEvent(int Id)
        {
            return db.Events.Where(ev => ev.Id == Id).SingleOrDefault();       
        }
        public void updateEventDetails(int Id, DateTime startDate, DateTime endDate, int maxRegistrations, string location)
        {
           Event ev = FetchEvent(Id);
           ev.StartDate = startDate;
            ev.EndDate = endDate;
            ev.MaxRegistrations = maxRegistrations;
            ev.Location = location;
            db.Update(ev);
            db.SaveChanges();
        }
        public void deleteEvent(int Id)
        {
            List<EventUser> relation = db.EventUsers.Where(e => e.EventRef == Id).ToList();
            foreach (var record in relation)
            {
                db.Remove(record);
            }
            db.SaveChanges();
            Event ev = FetchEvent(Id);
            db.Remove(ev);
            db.SaveChanges();
        }
        public List<Event> FetchAllEvents()
        {
            return db.Events.ToList();
        }
    }
}
