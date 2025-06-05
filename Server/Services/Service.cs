using MyServer;
using MyServer.DTO;
using FinalProject.Models;
using System.Net;
using System.Text.Json.Nodes;

namespace Server.Services
{
    public class Service
    {
        private readonly Repository _repository;

        public Service()
        {
            this._repository = new Repository();
        }

        public Service(Repository repo)
        {
            this._repository = repo;
        }
        public void CreateEvent(string Name, DateTime startDate, DateTime endDate, int MaxRegistrations, string Location)
        {
            _repository.CreateEvent(Name, startDate, endDate, MaxRegistrations, Location);
        }

        public List<User> RetrieveUsersOfEvent(int evenId)
        {
            return _repository.FetchUsersOfEvent(evenId);
        }
        public void RegisterUserToEvent(int UserId, int EventId)
        {
           _repository.RegisterUserToEvent(UserId, EventId);
        }
        public EventDTO RetrieveEvent(int Id)
        {
            EventDTO evDTO = new EventDTO();
            Event ev = _repository.FetchEvent(Id);
            evDTO.Name = ev.Name;
            evDTO.StartDate = ev.StartDate;
            evDTO.EndDate = ev.EndDate;
            evDTO.MaxRegistrations = ev.MaxRegistrations;
            evDTO.Location = ev.Location;
            return evDTO;
        }
        public void updateEventDetails(int Id, DateTime startDate, DateTime endDate, int maxRegistrations, string location)
        {
            _repository.updateEventDetails(Id, startDate, endDate, maxRegistrations, location);
        }
        public void deleteEvent(int Id)
        {
            _repository.deleteEvent(Id);
        }
        public List<EventScheduleDTO> getEventsSchedule()
        {
            List<EventScheduleDTO> sch = new List<EventScheduleDTO>();
            List<Event> events = _repository.FetchAllEvents();

            foreach (var item in events)
            {
                EventScheduleDTO dto = new EventScheduleDTO();
                dto.Id = item.Id;
                dto.StartDate = item.StartDate;
                dto.EndDate = item.EndDate;
                sch.Add(dto);
            }
            return sch;
        }
        public WeatherDTO RetrieveWeather(int Id)
        {
            string location = RetrieveEvent(Id).Location;
            string apiKey = "8235f01d2cc8420cdc7d85aa89102241";
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={location}&appid={apiKey}&units=metric";
            string json = (new WebClient()).DownloadString(url);

            WeatherDTO dto = new WeatherDTO();
            var weatherObj = JsonNode.Parse(json);

            dto.Forecast = weatherObj["weather"][0]["main"]?.ToString();
            dto.TemperatureC = weatherObj["main"]["temp"]?.GetValue<double>() ?? 0;
            return dto;

        }
    }

    }




