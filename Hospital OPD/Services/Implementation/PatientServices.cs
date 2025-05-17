using Hospital_OPD.Data;
using Hospital_OPD.Model;
using Hospital_OPD.Services.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Hospital_OPD.Services.Implementation
{
    public class PatientServices : IPatientServices
    {
        private readonly AppDbContext _context;
        public PatientServices(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Patient> AddPatient(Patient patient)
        {
            _context.Patient.Add(patient);
            await _context.SaveChangesAsync();
            return patient;
        }

        public Task<List<Patient>> GetALLPatient()
        {
            var patients = _context.Patient.ToListAsync();
            if (patients == null)
            {
                return null;
            }
            return patients;
        }

        public async Task<List<Patient>> SearchPatient(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return await _context.Patient.ToListAsync();
            }
            query = query.ToLower();
            return await _context.Patient.Where(p =>

             p.Name.ToLower().Contains(query)
             ||
             p.Email.ToLower().Contains(query)
             ||
             p.MobileNumber.ToLower().Contains(query)).ToListAsync();
        }
    }
}
