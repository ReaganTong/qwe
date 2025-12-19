using CampusSafety.Admin.Models;
using System.Net.Http.Json;

namespace CampusSafety.Admin.Services
{
    public class IncidentService
    {
        private readonly HttpClient _httpClient;
        private readonly SupabaseService _supabaseService;

        public IncidentService(HttpClient httpClient, SupabaseService supabaseService)
        {
            _httpClient = httpClient;
            _supabaseService = supabaseService;
        }

        public async Task<List<Incident>> GetRecentIncidentsAsync(int count = 10)
        {
            // Using Supabase
            var incidents = await _supabaseService
                .From<Incident>()
                .Order(x => x.CreatedAt, Constants.Ordering.Descending)
                .Limit(count)
                .Get();
            
            return incidents.Models;
        }

        public async Task<IncidentStats> GetStatsAsync()
        {
            var allIncidents = await _supabaseService
                .From<Incident>()
                .Get();
            
            return new IncidentStats
            {
                Total = allIncidents.Models.Count,
                Pending = allIncidents.Models.Count(i => i.Status == "pending"),
                Resolved = allIncidents.Models.Count(i => i.Status == "resolved"),
                Today = allIncidents.Models.Count(i => i.CreatedAt.Date == DateTime.Today)
            };
        }

        public async Task<List<CategoryData>> GetCategoryDistributionAsync()
        {
            var incidents = await _supabaseService
                .From<Incident>()
                .Get();
            
            var grouped = incidents.Models
                .GroupBy(i => i.Category)
                .Select(g => new CategoryData
                {
                    Category = g.Key,
                    Count = g.Count()
                })
                .ToList();
            
            return grouped;
        }

        public async Task<List<TrendData>> GetWeeklyTrendAsync()
        {
            // Get data for last 7 days
            var endDate = DateTime.Today;
            var startDate = endDate.AddDays(-6);
            
            var incidents = await _supabaseService
                .From<Incident>()
                .Where(i => i.CreatedAt >= startDate && i.CreatedAt <= endDate)
                .Get();
            
            var trendData = new List<TrendData>();
            
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var dayIncidents = incidents.Models
                    .Where(i => i.CreatedAt.Date == date)
                    .ToList();
                
                trendData.Add(new TrendData
                {
                    Date = date,
                    Count = dayIncidents.Count,
                    AvgSeverity = dayIncidents.Any() 
                        ? dayIncidents.Average(i => GetSeverityValue(i.Severity))
                        : 0
                });
            }
            
            return trendData;
        }

        private double GetSeverityValue(string severity)
        {
            return severity.ToLower() switch
            {
                "low" => 1,
                "medium" => 2,
                "high" => 3,
                _ => 0
            };
        }

        public async Task<Incident> GetIncidentAsync(Guid id)
        {
            var incident = await _supabaseService
                .From<Incident>()
                .Where(i => i.Id == id)
                .Single();
            
            return incident;
        }

        public async Task<Incident> CreateIncidentAsync(Incident incident)
        {
            incident.Id = Guid.NewGuid();
            incident.CreatedAt = DateTime.UtcNow;
            incident.UpdatedAt = DateTime.UtcNow;
            
            var response = await _supabaseService
                .From<Incident>()
                .Insert(incident);
            
            return response.Model;
        }

        public async Task<Incident> UpdateIncidentAsync(Incident incident)
        {
            incident.UpdatedAt = DateTime.UtcNow;
            
            var response = await _supabaseService
                .From<Incident>()
                .Where(i => i.Id == incident.Id)
                .Set(i => i.Status, incident.Status)
                .Set(i => i.Severity, incident.Severity)
                .Set(i => i.UpdatedAt, incident.UpdatedAt)
                .Update();
            
            return response.Model;
        }

        public async Task DeleteIncidentAsync(Guid id)
        {
            await _supabaseService
                .From<Incident>()
                .Where(i => i.Id == id)
                .Delete();
        }
    }

    public class IncidentStats
    {
        public int Total { get; set; }
        public int Pending { get; set; }
        public int Resolved { get; set; }
        public int Today { get; set; }
    }

    public class CategoryData
    {
        public string Category { get; set; }
        public int Count { get; set; }
    }

    public class TrendData
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
        public double AvgSeverity { get; set; }
    }
}