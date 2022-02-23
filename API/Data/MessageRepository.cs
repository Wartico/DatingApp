﻿using API.Entities;
using API.Helpers;
using API.Interfaces;
using API.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public MessageRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AddMessage(Message messsage)
        {
            _context.Messsages.Add(messsage);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messsages.Remove(message);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messsages.FindAsync(id);
        }

        public async Task<PagedList<MessageResponse>> GetMessagesForUser(MessageParams messageParams)
        {
            var query = _context.Messsages
                .OrderByDescending(m => m.MessageSent)
                .AsQueryable();

            query = messageParams.Container switch
            {
                "Inbox" => query.Where(u => u.Recipient.UserName == messageParams.Username),
                "Outbox" => query.Where(u => u.Sender.UserName == messageParams.Username),
                _ => query.Where(u => u.Recipient.UserName == messageParams.Username && u.DateRead == null)
            };

            var messages = query.ProjectTo<MessageResponse>(_mapper.ConfigurationProvider);

            return await PagedList<MessageResponse>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<MessageResponse>> GetMessageThread(string currentUsername, string recipientUsername)
        {
            var messages = await _context.Messsages
                .Include(u => u.Sender)
                    .ThenInclude(p => p.Photos)
                .Include(u => u.Recipient)
                    .ThenInclude(p => p.Photos)
                .Where(m => m.Recipient.UserName == currentUsername
                    && m.Sender.UserName == recipientUsername
                    || m.Recipient.UserName == recipientUsername
                    && m.Sender.UserName == currentUsername
                )
                .OrderBy(m => m.MessageSent)
                .ToListAsync();

            var unreadMessages = messages
                .Where(m => m.DateRead == null && m.Recipient.UserName == currentUsername)
                .ToList();

            if (unreadMessages.Any())
            {
                foreach(var message in unreadMessages)
                {
                    message.DateRead = DateTime.Now;
                }
                await _context.SaveChangesAsync();
            }

            return _mapper.Map<IEnumerable<MessageResponse>>(messages);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
