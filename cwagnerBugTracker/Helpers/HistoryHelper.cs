using cwagnerBugTracker.Models;
using System;
using System.Linq;
using Domain;
using System.ComponentModel.DataAnnotations;

namespace cwagnerBugTracker.Helpers
{
    public class HistoryHelper
    {
        public TicketHistory GetHistory(Ticket oldTicket, Ticket newTicket, string userId)
        {
            var ticketHistory = new TicketHistory
            {
                TicketId = newTicket.Id,
                AuthorId = userId,
                Created = DateTimeOffset.Now,
            };

            if(oldTicket.AssignToUserId != newTicket.AssignToUserId)
            {
                ticketHistory.Changes.Add(new HistoryChange
                {
                    OldValue = !string.IsNullOrWhiteSpace(oldTicket.AssignToUser?.FullName) ? oldTicket.AssignToUser.FullName : "Unassigned",
                    NewValue = !string.IsNullOrWhiteSpace(newTicket.AssignToUser?.FullName) ? newTicket.AssignToUser.FullName : "Unassigned",
                    Property = "AssignedToUser"
                });
            }

            foreach (var property in typeof(Ticket).GetProperties().Where(p => p.GetCustomAttributes(typeof(AuditAttribute), false).Any()))
            {
                var newVal = property.GetValue(newTicket) ?? string.Empty;
                var oldVal = property.GetValue(oldTicket) ?? string.Empty;

                //If the type is an enum, store changes for friendly names
                if(property.PropertyType.IsEnum)
                {
                    oldVal = ((Enum)oldVal).GetFriendlyName();
                    newVal = ((Enum)newVal).GetFriendlyName();
                }

                if(!Equals(newVal, oldVal))
                {
                    ticketHistory.Changes.Add(new HistoryChange
                    {
                        OldValue = oldVal.ToString(),
                        NewValue = newVal.ToString(),
                        Property = property.Name
                    });
                }
            }

            return ticketHistory;
        }
    }
}