using cwagnerBugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Domain;

namespace cwagnerBugTracker.Helpers
{
    public class HistoryHelper
    {
        public List<TicketHistory> AddToHistory(Ticket oldTicket, Ticket newTicket, string userId)
        {
            var ticketHistories = new List<TicketHistory>();
            
            foreach(var property in typeof(Ticket).GetProperties().Where(p => p.GetCustomAttributes(typeof(AuditAttribute), false).Any()))
            {
                var newVal = property.GetValue(newTicket);
                var oldVal = property.GetValue(oldTicket);

                if(newVal != oldVal)
                {
                    ticketHistories.Add(new TicketHistory
                    {
                        OldValue = oldVal.ToString(),
                        NewValue = newVal.ToString(),
                        TicketId = newTicket.Id,
                        AuthorId = userId,
                        Created = DateTimeOffset.Now,
                        Property = property.Name
                    });
                }
            }

            return ticketHistories;
        }
    }
}