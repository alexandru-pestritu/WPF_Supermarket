using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Supermarket.Models.BusinessLayerLogic
{
    public class UserBLL
    {
        private readonly SupermarketDBContext _context;
        public UserBLL()
        {
            _context = new SupermarketDBContext();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUserById(int id)
        {
            return _context.Users.Find(id);
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            var existingUser = _context.Users.Find(user.Id);
            if (existingUser != null)
            {
                _context.Entry(existingUser).CurrentValues.SetValues(user);
                _context.SaveChanges();
            }
        }

        public void DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public IEnumerable<DailyRevenue> GetDailyRevenuesByUserAndMonth(int userId, int year, int month)
        {
            var receipts = _context.Receipts
                .Where(r => r.CashierId == userId && r.Date.Year == year && r.Date.Month == month)
                .GroupBy(r => DbFunctions.TruncateTime(r.Date))
                .Select(g => new DailyRevenue
                {
                    Date = g.Key.Value,
                    Total = g.Sum(r => r.Total)
                })
                .ToList();

            var daysInMonth = Enumerable.Range(1, DateTime.DaysInMonth(year, month))
                                        .Select(day => new DateTime(year, month, day))
                                        .ToList();

            var dailyRevenues = daysInMonth.Select(day => new DailyRevenue
            {
                Date = day,
                Total = receipts.FirstOrDefault(r => r.Date == day)?.Total ?? 0
            }).ToList();

            return dailyRevenues;
        }
    }
}

