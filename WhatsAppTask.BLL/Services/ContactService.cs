using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using WhatsAppTask.BLL.Interfaces;
using WhatsAppTask.DAL.DbContext;
using WhatsAppTask.DAL.Entities;
using WhatsAppTask.DTO;

namespace WhatsAppTask.BLL.Services
{
    public class ContactService : IContactService
    {
        private readonly AppDbContext _context;

        public ContactService(AppDbContext context)
        {
            _context = context;
        }

        public Contact CreateContact(
            int userId,
            string phoneNumber,
            string? name,
            string? imageUrl)
        {
            phoneNumber = NormalizePhone(phoneNumber);

            var existingContact = _context.Contacts
                .FirstOrDefault(c =>
                    c.UserId == userId &&
                    c.PhoneNumber == phoneNumber);

            if (existingContact != null)
            {
                if (!string.IsNullOrWhiteSpace(name))
                    existingContact.Name = name;

                if (!string.IsNullOrWhiteSpace(imageUrl))
                    existingContact.ImageUrl = imageUrl;

                var existingConversation = _context.Conversations
                    .FirstOrDefault(c =>
                        c.UserId == userId &&
                        c.ContactId == existingContact.Id);

                if (existingConversation == null)
                {
                    var conversation = new Conversation
                    {
                        UserId = userId,
                        ContactId = existingContact.Id,
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.Conversations.Add(conversation);
                }

                _context.SaveChanges();
                return existingContact;
            }

            var contact = new Contact
            {
                UserId = userId,
                PhoneNumber = phoneNumber,
                Name = name,
                ImageUrl = imageUrl,
                CreatedAt = DateTime.UtcNow
            };

            _context.Contacts.Add(contact);
            _context.SaveChanges();

            var newConversation = new Conversation
            {
                UserId = userId,
                ContactId = contact.Id,
                CreatedAt = DateTime.UtcNow
            };

            _context.Conversations.Add(newConversation);
            _context.SaveChanges();

            return contact;
        }


        private string NormalizePhone(string phone)
        {
            return phone
                .Replace(" ", "")
                .Replace("-", "")
                .Replace("(", "")
                .Replace(")", "")
                .Trim()
                .TrimStart('+');
        }


        public List<Contact> GetUserContacts(int userId)
        {
            return _context.Contacts
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.CreatedAt)
                .ToList();
        }
        public Contact? GetContactById(int userId, int contactId)
        {
            return _context.Contacts
                .FirstOrDefault(c =>
                    c.Id == contactId &&
                    c.UserId == userId);
        }
        public List<Contact> SearchContacts(int userId, string query)
        {
            query = query.ToLower();

            return _context.Contacts
                .Where(c =>
                    c.UserId == userId &&
                    (
                        c.PhoneNumber.Contains(query) ||
                        (c.Name != null && c.Name.ToLower().Contains(query))
                    )
                )
                .OrderByDescending(c => c.CreatedAt)
                .ToList();
        }

        public BulkCreateContactsResultDto BulkCreateContacts(
            int userId,
            List<BulkContactItemDto> contacts)
            {
                var result = new BulkCreateContactsResultDto();

                var existingPhones = _context.Contacts
                    .Where(c => c.UserId == userId)
                    .Select(c => c.PhoneNumber)
                    .ToHashSet();

                foreach (var item in contacts)
                {
                    if (!Regex.IsMatch(item.PhoneNumber, @"^\+?\d{10,15}$"))
                    {
                        result.Invalid.Add(item.PhoneNumber);
                        continue;
                    }

                    var normalized = item.PhoneNumber.Trim().TrimStart('+');

                    if (existingPhones.Contains(normalized))
                    {
                        result.Duplicates.Add(item.PhoneNumber);
                        continue;
                    }

                    var contact = new Contact
                    {
                        UserId = userId,
                        PhoneNumber = normalized,
                        Name = item.Name,
                        ImageUrl = item.ImageUrl,
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.Contacts.Add(contact);
                    _context.SaveChanges();

                    var conversation = new Conversation
                    {
                        UserId = userId,
                        ContactId = contact.Id,
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.Conversations.Add(conversation);
                    _context.SaveChanges();

                    existingPhones.Add(normalized);

                    result.Created.Add(new ContactResponseDto
                    {
                        Id = contact.Id,
                        PhoneNumber = contact.PhoneNumber,
                        Name = contact.Name,
                        ImageUrl = contact.ImageUrl,
                        CreatedAt = contact.CreatedAt
                    });
                }
                return result;
            }
            public Contact UpdateContact(
                int userId,
                int contactId,
                string? name,
                string? imageUrl)
                    {
                        var contact = _context.Contacts
                            .FirstOrDefault(c => c.Id == contactId && c.UserId == userId);

                        if (contact == null)
                            throw new Exception("Contact not found");

                        if (!string.IsNullOrWhiteSpace(name))
                            contact.Name = name;

                        if (!string.IsNullOrWhiteSpace(imageUrl))
                            contact.ImageUrl = imageUrl;

                        _context.SaveChanges();
                        return contact;
                    }
        public void DeleteContact(int userId, int contactId)
        {
            var contact = _context.Contacts
                .FirstOrDefault(c => c.Id == contactId && c.UserId == userId);

            if (contact == null)
                throw new Exception("Contact not found");

            _context.Contacts.Remove(contact);
            _context.SaveChanges();
        }
    }
}
